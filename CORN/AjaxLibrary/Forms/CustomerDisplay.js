var CORN = CORN || {};

CORN.CustomerDisplay = {
    handlerURL: "../Handlers/TableBillWS.asmx",
    DiningTableBillDetailTemplate: {},

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
    }

};


$(function () {
    CORN.CustomerDisplay.DiningTableBillDetailTemplate = $(".DiningTableBillDetailTemplate").clone().removeClass('DiningTableBillDetailTemplate');

    $(document).on('click', '#btnGetTables', function () {
        $('#dvDiningTables').html('');
        $('#dvDiningTableBillReport').hide();

        blockUI();
        CORN.CustomerDisplay.getOccupiedTables();
    });

    $(document).on('click', '#btnBillPaid', function () {
        CORN.CustomerDisplay.billPaid();
    });

    $('#btnGetTables').trigger('click');


    var dataInput = document.getElementById('data'),
    output = document.getElementById('fromEvent');

    addEvent(window, 'storage', function (event) {
        if (event.key == 'storage-event-test') {
            output.innerHTML = event.newValue;
        }
    });

    addEvent(dataInput, 'keyup', function () {
        localStorage.setItem('storage-event-test', this.value);
    });

});