var CORN = CORN || {};

CORN.TableBill = {
    handlerURL: "../Handlers/TableBillWS.asmx",
    TableButtonTemplate: {},
    DiningTableBillDetailTemplate: {},
    DistributorId: 0,

    TableInfoList: [],
    SelectedTableInfo: null,

    getOccupiedTables: function (action) {

        $.ajax({
            cache: false,
            async: true,
            dataType: "json",
            data: {
                DocumentDate: $("input[id$=txtDate]").val(),
                DistributorId: CORN.TableBill.DistributorId
            },
            type: "POST",
            url: CORN.TableBill.handlerURL + '/GetOccupiedTablesForBilling',
            success: function (response) {
                switch (response.Status) {
                    case "Success":
                        unblockUI();

                        // remove table from last updated list, if any table exists in this list, then it means some table(s) are invoiced, so refresh the list and populate tables again ...
                        $.each(response.Records.TableInfoList, function (index, data) {
                            var existingData = $.grep(CORN.TableBill.TableInfoList, function (item) {
                                item.TableId === data.TableId
                            });
                            if (existingData.length > 0) {
                                delete existingData;
                            }
                        });
                        if (CORN.TableBill.TableInfoList.length != response.Records.TableInfoList.length) {
                            CORN.TableBill.TableInfoList = response.Records.TableInfoList;
                            CORN.TableBill.populateTableBoxes();
                        }

                        break;
                    case "Error":
                        unblockUI();
                        showMessageDialog(response.Message, 'Error');
                        break;
                }
            }
        });
    },

    populateTableBoxesOld: function () {
        var $dvTable = $('#dvDiningTables').html('');
        $(CORN.TableBill.TableInfoList).each(function (index, data) {
            var $template = $(CORN.TableBill.TableButtonTemplate).clone();
            var $btn = $("input[type='button']", $template);

            $btn.val(data.TableNo);
            $btn.data('record', data);
            $btn.on('click', function (e) {
                var record = $(e.currentTarget).data('record');
                CORN.TableBill.SelectedTableInfo = record;
                CORN.TableBill.generateTableBill();
            });

            $dvTable.append($template);
        });
    },
    populateTableBoxes: function () {
        var $dvTable = $('#dvDiningTables').html('');
        $('#dvDiningTableBillReport').hide();

        if (CORN.TableBill.TableInfoList.length == 0) {
            showMessageDialog("All Tables are vacant / paid", "No Record Found");
        }

        $(CORN.TableBill.TableInfoList).each(function (index, data) {
            //var $template = $(CORN.TableBill.TableButtonTemplate).clone();
            var $template = $("<input type='button'>");

            $template.val(data.TableNo);
            $template.data('record', data);
            $template.on('click', function (e) {
                var record = $(e.currentTarget).data('record');
                CORN.TableBill.SelectedTableInfo = record;
                CORN.TableBill.generateTableBill();
            });

            $dvTable.append($template);
        });

        setInterval(CORN.TableBill.getOccupiedTables, 2000);
    },

    generateTableBill: function () {
        blockUI();
        $.ajax({
            cache: false,
            async: true,
            dataType: "json",
            data: {
                InvoiceId: CORN.TableBill.SelectedTableInfo.InvoiceId
            },
            type: "POST",
            url: CORN.TableBill.handlerURL + '/GetOccupiedTableBill',
            success: function (response) {
                switch (response.Status) {
                    case "Success":
                        unblockUI();
                        CORN.TableBill.generateTableBillHTML(response.Records);

                        break;
                    case "Error":
                        unblockUI();
                        showMessageDialog(response.Message, 'Error');
                        break;
                }
            }
        });
    },

    generateTableBillHTML: function (data) {
        var $dvBill = $('#dvDiningTableBillReport');

        if (data.TableInvoiceDetailList.length > 0) {
            $dvBill.show();

            $dvBill.find('#table_billDate').html(data.OrderBookerInfo.OrderDateTime);
            $dvBill.find('#table_orderTakerName').html(data.OrderBookerInfo.OrderBookerName);
            $dvBill.find('#table_id').html(data.OrderBookerInfo.TableInfo.TableId);
            $dvBill.find('#table_no').html(data.OrderBookerInfo.TableInfo.TableNo);

            $('#dvDiningTableBillDetail').html('');
            var totalAmount = 0;
            $(data.TableInvoiceDetailList).each(function (index, data) {

                var $row = $('<tr>');
                amount = Math.round(data.Quantity * data.Rate);
                totalAmount = totalAmount + amount;
                $row.append($('<th>', { 'scope': 'row' }).html(index + 1));
                $row.append($('<td>').html(data.ItemName));
                $row.append($('<td>', { 'css': { 'text-align': 'right' } }).html(data.Quantity));
                $row.append($('<td>', { 'css': { 'text-align': 'right' } }).html(data.Rate));
                $row.append($('<td>', { 'css': { 'text-align': 'right' } }).html(amount));

                $('#dvDiningTableBillDetail').append($row);
            });

            // total amount
            var $totalRow = $('<tr>');
            $totalRow.append($("<td colspan='4' style='text-align: right; padding-right: 8px; font-weight: bold;'>").html("Total"));
            $totalRow.append($('<td>', {
                'css': {
                    'text-align': 'right',
                    'padding-right': '5px',
                    'font-weight': 'bold'
                }
            }).html(totalAmount));

            $('#dvDiningTableBillDetail').append($totalRow);

            // discount
            var $discountRow = $('<tr>');
            if (data.OrderBookerInfo.Discount > 0) {
                var discountAmount = 0;
                if (data.OrderBookerInfo.DiscountType == 'VALUE') {
                    discountAmount = data.OrderBookerInfo.Discount;
                    $discountRow.append($("<td colspan='4' style='text-align: right; padding-right: 8px; font-weight: bold;'>").html("Discount"));
                }
                else if (data.OrderBookerInfo.DiscountType == 'PERCENT') {
                    discountAmount = Math.round(data.OrderBookerInfo.Discount / 100.0 * parseFloat(totalAmount));
                    $discountRow.append($("<td colspan='4' style='text-align: right; padding-right: 8px; font-weight: bold;'>").html("Discount @" + data.OrderBookerInfo.Discount + " %"));
                }
                $discountRow.append($('<td>', {
                    'css': {
                        'text-align': 'right',
                        'padding-right': '5px',
                        'font-weight': 'bold'
                    }
                }).html(discountAmount));

                $('#dvDiningTableBillDetail').append($discountRow);
            }

            // GST amount
            var $gstAmountRow = $('<tr>');
            $gstAmountRow.append($("<td colspan='4' style='text-align: right; padding-right: 8px; font-weight: bold;'>").html("G.S.T @" + data.OrderBookerInfo.GST + " %"));
            $gstAmountRow.append($('<td>', {
                'css': {
                    'text-align': 'right',
                    'font-weight': 'bold'
                }
            }).html(Math.round(totalAmount * data.OrderBookerInfo.GST / 100.0)));

            $('#dvDiningTableBillDetail').append($gstAmountRow);

            // net total amount
            var $grandRow = $('<tr>');
            $grandRow.append($("<td colspan='4' style='text-align: right; padding-right: 8px; font-weight: bold;'>").html("Grand Total"));
            $grandRow.append($('<td>', {
                'css': {
                    'text-align': 'right',
                    'font-weight': 'bold'
                }
            }).html(totalAmount + Math.round(totalAmount * data.OrderBookerInfo.GST / 100.0)));

            $('#dvDiningTableBillDetail').append($grandRow);
        }
        else {
            $dvBill.hide();
            showMessageDialog("Invoice detail not found", 'No Record Found');
        }
    },

    billPaid: function () {
        blockUI();
        $.ajax({
            cache: false,
            async: true,
            dataType: "json",
            data: {
                InvoiceId: CORN.TableBill.SelectedTableInfo.InvoiceId
            },
            type: "POST",
            url: CORN.TableBill.handlerURL + '/UnholdTableByBillPaid',
            success: function (response) {
                switch (response.Status) {
                    case "Success":
                        unblockUI();

                        showMessageDialog("Amount paid", 'Amount paid');
                        $('#dvDiningTableBillReport').hide();
                        CORN.TableBill.getOccupiedTables();

                        break;
                    case "Error":
                        unblockUI();
                        showMessageDialog(response.Message, 'Error');
                        break;
                }
            }
        });
    }
};


$(function () {

    CORN.TableBill.DistributorId = parseInt($("input[id$=hidDistributorId]").val());
    $('#dvDiningTableBillReport').hide();
    
    CORN.TableBill.TableButtonTemplate = $(".TableButtonTemplate").clone().removeClass('TableButtonTemplate');
    CORN.TableBill.DiningTableBillDetailTemplate = $(".DiningTableBillDetailTemplate").clone().removeClass('DiningTableBillDetailTemplate');

    $(document).on('click', '#btnGetTables', function () {
        $('#dvDiningTables').html('');
        $('#dvDiningTableBillReport').hide();

        blockUI();
        CORN.TableBill.getOccupiedTables();
    });

    $(document).on('click', '#btnBillPaid', function () {
        CORN.TableBill.billPaid();
    });

    $('#btnGetTables').trigger('click');

});