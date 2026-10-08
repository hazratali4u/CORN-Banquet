<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmCornPosFashion.aspx.cs" Inherits="Forms_frmCornPosFashion" ValidateRequest="false"
    EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Corn POS</title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>

    <script src="../AjaxLibrary/jquery-1.7.1.min.js"></script>
    <script src="../AjaxLibrary/jQuery.print.js"></script>

    <link href="../css/style.css" rel="stylesheet" />
    <%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable({
                maxListSize: 200, // if list size are less than maxListSize, show them all
                maxMultiMatch: 300, // how many matching entries should be displayed
                exactMatch: false, // Exact matching on search
                wildcards: true, // Support for wildcard characters (*, ?)
                ignoreCase: true, // Ignore case sensitivity
                latency: 200, // how many millis to wait until starting search
                warnMultiMatch: 'top {0} matches ...',
                warnNoMatch: 'no matches ...',
                zIndex: 'auto'
            });
        });


        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        //function ValidateForm() {
        //    return true;
        //}  


        function Closepopup() {
            $('#myModalReportCriteria').modal('close');
        }
    </script>




</head>
<body class="cornpos container">
    <form id="form1" runat="server">



        <script type="text/javascript" language="JavaScript">

            function SearchProduct() {
                if (document.getElementById('<%=txtskuCode.ClientID%>').value == '') {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }

                else {
                    var obj = jQuery.parseJSON($("#<%=hfProduct.ClientID %>").val());

                    $('#<%=lblfound.ClientID%>').text('');
                    $('#<%=lblClosingStock.ClientID%>').text('');
                    var Productflag = 0;
                    var Stockflag = 0;

                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        if (item.SKU_CODE == document.getElementById('<%=txtskuCode.ClientID%>').value) {
                            Productflag = 1;
                            document.getElementById("<%= txtskuID.ClientID %>").value = item.SKU_ID;
                            document.getElementById("<%= txtskuName.ClientID %>").value = item.SKU_NAME;

                            document.getElementById("<%= txtsize.ClientID %>").value = item.PACKSIZE;
                            document.getElementById("<%= txtUnitRate.ClientID %>").value = item.TRADE_PRICE;

                            $('#<%=lblClosingStock.ClientID%>').text(item.CLOSING_STOCK);

                            var table = document.getElementById('<%=dataTable.ClientID%>');
                            var qty = document.getElementById("<%= txtQuantity.ClientID %>").value;

                            if (table.rows.length == "0") {

                                if (parseFloat(qty) > item.CLOSING_STOCK) {

                                    Stockflag = 0;
                                }
                                else {
                                    Stockflag = 1;
                                }
                            }
                            else {

                                $('#<%=dataTable.ClientID%>').find('tr').each(function () {

                                    var td1 = $(this).find("td:eq(0)").text();

                                    if (item.SKU_CODE == td1) {

                                        var CurrentQty = $(this).find("td:eq(3)").text();

                                        CurrentQty = parseFloat(CurrentQty) + parseFloat(qty);

                                        var ClosingStock = item.CLOSING_STOCK;

                                        if (parseFloat(CurrentQty) > parseFloat(ClosingStock)) {

                                            Stockflag = 0;
                                        }
                                        else {
                                            Stockflag = 1;
                                        }
                                    }
                                    else {

                                        if (parseFloat(qty) > item.CLOSING_STOCK) {
                                            Stockflag = 0;
                                        }
                                        else {

                                            Stockflag = 1;
                                        }
                                    }
                                });
                            }
                        }
                    }
                    if (Productflag == 0) {
                        $('#<%=lblfound.ClientID%>').text('Product not found.');
                        document.getElementById("<%= txtskuCode.ClientID %>").focus();
                        return false;
                    }

                    if (Stockflag == 0) {
                        $('#<%=lblfound.ClientID%>').text('Stock:  ' + $('#<%=lblClosingStock.ClientID%>').text());
                        document.getElementById("<%= txtskuCode.ClientID %>").focus();
                        return false;
                    }

                    document.getElementById("<%= txtskuCode.ClientID %>").focus();
                    return true;
                }
            }
            // end search product

            function duplicationCheck(skuCode) {

            }

            function storeTblValues() {
                var tableData = new Array();
                $('#<%=dataTable.ClientID%> tr').each(function (row, tr) {
                    tableData[row] = {
                        "SKU_Code": $(tr).find('td:eq(0)').text()
                            , "SKU_Name": $(tr).find('td:eq(1)').text()
                            , "PACKSIZE": $(tr).find('td:eq(2)').text()
                       , "QUANTITY_UNIT": $(tr).find('td:eq(3)').text()
                   , "STANDARD_DISCOUNT": $(tr).find('td:eq(4)').text()
                          , "UNIT_PRICE": $(tr).find('td:eq(5)').text()
                          , "NET_AMOUNT": $(tr).find('td:eq(6)').text()

                        //hidden col's
                       , "SKU_ID": $(tr).find('td:eq(7)').text()
                        //, "COLOR": $(tr).find('td:eq(2)').text()                       
                       , "AMOUNT": $(tr).find('td:eq(8)').text()
                       , "CHECK_DELETE": 0
                       , "GST_RATE": 0
                       , "GST_AMOUNT": 0
                       , "TST_AMOUNT": 0
                       , "STANDARD_DISCOUNT_TEMP": 0
                       , "STANDARD_DISCOUNT_PER": 0
                       , "BATCH_NO": 0
                    }
                });

                return tableData;
            }

            function addRow() {
                if (SearchProduct()) {
                    var flag = 0;
                    var skuCode = $('#<%=txtskuCode.ClientID%>').val();
                    var e = document.getElementById('<%=DrpDiscount.ClientID%>');
                    var discType = e.options[e.selectedIndex].value;
                    var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
                    var table = document.getElementById('<%=dataTable.ClientID%>');

                    var rowCount = table.rows.length;

                    var b;
                    var net;

                    var disc;
                    var perDisc;

                    var i;
                    var tableData;
                    var row;

                    var cellskuCode;
                    var cellskuName;
                    var cellsize;
                    var cellQuantity;
                    var cellDiscount;
                    var cellUnitRate;
                    var cellDiscountedAmount;
                    var cellskuID;
                    var cellAmount;
                    var cellDelete;

                    if (table.rows.length == "0") {
                        row = table.insertRow(rowCount);

                        cellskuCode = row.insertCell(0);
                        cellskuCode.innerHTML = document.getElementById('<%=txtskuCode.ClientID%>').value;
                        cellskuCode.style.width = "167px";

                        cellskuName = row.insertCell(1);
                        cellskuName.innerHTML = document.getElementById('<%=txtskuName.ClientID%>').value;
                        cellskuName.style.width = "320px";

                        cellsize = row.insertCell(2);
                        cellsize.innerHTML = document.getElementById('<%=txtsize.ClientID%>').value;
                        cellsize.style.width = "92px";

                        cellQuantity = row.insertCell(3);
                        cellQuantity.innerHTML = document.getElementById('<%=txtQuantity.ClientID%>').value;
                        cellQuantity.style.width = "85px";

                        cellDiscount = row.insertCell(4);
                        if (discType == 0) {
                            perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                            cellDiscount.innerHTML = parseFloat(perDisc).toFixed(2);
                        } else {
                            if (mode == 'SALE MODE') {
                                perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value;
                                cellDiscount.innerHTML = parseFloat(perDisc).toFixed(2);
                            } else {
                                perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value * -1;
                                cellDiscount.innerHTML = parseFloat(perDisc).toFixed(2);
                            }
                        }
                        cellDiscount.style.width = "89px";

                        cellUnitRate = row.insertCell(5);
                        cellUnitRate.innerHTML = document.getElementById('<%=txtUnitRate.ClientID%>').value;
                        cellUnitRate.style.width = "85px";

                        cellDiscountedAmount = row.insertCell(6);
                        cellDiscountedAmount.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) - perDisc;
                        cellDiscountedAmount.style.width = "93px";

                        //hidden cell's
                        cellskuID = row.insertCell(7);
                        cellskuID.innerHTML = document.getElementById('<%=txtskuID.ClientID%>').value;
                        cellskuID.style.display = 'none';

                        cellAmount = row.insertCell(8);
                        cellAmount.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value);
                        cellAmount.style.display = 'none';

                        cellDelete = row.insertCell(9);
                        cellDelete.innerHTML = '<input type="button" value = "X" style="cursor:pointer" onClick="Javacsript:deleteRow(this)">';


                    <%--document.getElementById('<%=txtTotalPrice.ClientID%>').value = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) - perDisc;--%>

                        //Calculation for loop getting cell values
                        b = 0;
                        net = 0;
                        disc = 0;
                        for (i = 0; i < table.rows.length; i++) {

                            b = table.rows[i].cells[8].innerHTML;

                            disc = parseFloat(table.rows[i].cells[4].innerHTML).toFixed(2);
                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);

                        }
                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc;
                        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc)).toFixed(2);


                        //set data in json and store in hidden field through storeTblValues()
                        debugger;
                        tableData = storeTblValues();
                        //tableData = $.toJSON(tableData);
                        tableData = JSON.stringify(tableData);
                        document.getElementById('<%=tab.ClientID%>').value = tableData;
                    } else { //Work on duplication
                        var duplicrw;

                        $('#<%=dataTable.ClientID%>').find('tr').each(function () {

                            var td1 = $(this).find("td:eq(0)").text();
                            if (skuCode == td1) {

                                duplicrw = $(this);

                                flag = 1;
                            }
                        });

                        if (flag == "1") {
                            var perDisc2;
                            var unitrate = duplicrw.find("td:eq(5)").text();
                            var discount = duplicrw.find("td:eq(4)").text();
                            var qty = duplicrw.find("td:eq(3)").text();

                            if (discType == 0) {
                                perDisc2 = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                            } else {
                                perDisc2 = document.getElementById('<%=txtDiscount.ClientID%>').value;
                            }

                            duplicrw.find("td:eq(3)").text(parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10));
                            duplicrw.find("td:eq(4)").text((parseFloat(perDisc2) + parseFloat(discount)).toFixed(2));
                            duplicrw.find("td:eq(6)").text((parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseInt(unitrate, 10) - ((parseFloat(perDisc2) + parseFloat(discount)).toFixed(2)));
                            duplicrw.find("td:eq(8)").text((parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseInt(unitrate, 10));


                            b = 0;
                            net = 0;
                            var disc3 = 0;
                            for (i = 0; i < table.rows.length; i++) {
                                b = table.rows[i].cells[8].innerHTML;
                                disc3 = (parseFloat(disc3) + parseFloat(table.rows[i].cells[4].innerHTML)).toFixed(2);
                                net = (parseFloat(net) + parseFloat(b)).toFixed(2);
                            }

                            document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                            document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc3;
                            document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc3)).toFixed(2);
                            debugger;
                            tableData = storeTblValues();
                            //tableData = $.toJSON(tableData);
                            tableData = JSON.stringify(tableData);
                            document.getElementById('<%=tab.ClientID%>').value = tableData;

                            ClearControls();

                        } else {
                            row = table.insertRow(rowCount);
                            cellskuCode = row.insertCell(0);
                            cellskuCode.innerHTML = document.getElementById('<%=txtskuCode.ClientID%>').value;
                            cellskuCode.style.width = "140px";

                            cellskuName = row.insertCell(1);
                            cellskuName.innerHTML = document.getElementById('<%=txtskuName.ClientID%>').value;
                            cellskuName.style.width = "180px";

                        <%--cell3 = row.insertCell(2);
                        cell3.innerHTML = document.getElementById('<%=txtcolor.ClientID%>').value;
                        cell3.style.display = 'none';--%>

                            cellsize = row.insertCell(2);
                            cellsize.innerHTML = document.getElementById('<%=txtsize.ClientID%>').value;
                            cellsize.style.width = "92px";

                            cellQuantity = row.insertCell(3);
                            cellQuantity.innerHTML = document.getElementById('<%=txtQuantity.ClientID%>').value;
                            cellQuantity.style.width = "85px";

                            cellDiscount = row.insertCell(4);
                            if (discType == 0) {
                                perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                                cellDiscount.innerHTML = perDisc;
                            } else {
                                if (mode == 'SALE MODE') {
                                    perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value;
                                    cellDiscount.innerHTML = perDisc;
                                } else {
                                    perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value * -1;
                                    cellDiscount.innerHTML = perDisc;
                                }
                            }
                            cellDiscount.style.width = "89px";

                            cellUnitRate = row.insertCell(5);
                            cellUnitRate.innerHTML = document.getElementById('<%=txtUnitRate.ClientID%>').value;
                            cellUnitRate.style.width = "85px";

                            cellDiscountedAmount = row.insertCell(6);
                            cellDiscountedAmount.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) - perDisc;
                        cellDiscountedAmount.style.width = "70px";

                            //hidden cell's
                        cellskuID = row.insertCell(7);
                        cellskuID.innerHTML = document.getElementById('<%=txtskuID.ClientID%>').value;
                        cellskuID.style.display = 'none';

                        cellAmount = row.insertCell(8);
                        cellAmount.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value);
                        cellAmount.style.display = 'none';

                        cellDelete = row.insertCell(9);
                        cellDelete.innerHTML = '<input type="button" value = "X" style="cursor:pointer" onClick="Javacsript:deleteRow(this)">';

                            //Calculation for loop getting cell values
                        b = 0;
                        net = 0;
                        var disc2 = 0;

                        for (i = 0; i < table.rows.length; i++) {

                            b = table.rows[i].cells[8].innerHTML;

                            disc2 = (parseFloat(disc2) + parseFloat(table.rows[i].cells[4].innerHTML)).toFixed(2);

                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);

                        }
                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc2;
                            document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc2)).toFixed(2);

                            //set data in json and store in hidden field through storeTblValues()
                        
                            tableData = storeTblValues();
                            //tableData = $.toJSON(tableData);
                            tableData = JSON.stringify(tableData);
                            document.getElementById('<%=tab.ClientID%>').value = tableData;
                        }
                    }

                }
            }

            function deleteRow(obj) {

                var index = obj.parentNode.parentNode.rowIndex;
                var table = document.getElementById("<%=dataTable.ClientID %>");

                table.deleteRow(index);

                var b = 0;
                var net = 0;
                var disc = 0;

                for (var i = 0; i < table.rows.length; i++) {
                    b = table.rows[i].cells[8].innerHTML;
                    disc = table.rows[i].cells[4].innerHTML;
                    net = (parseFloat(net) + parseFloat(b)).toFixed(2);
                }

                document.getElementById('<%=txtGrossAmount.ClientID%>').value = (parseFloat(net)).toFixed(2);
                document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = (parseFloat(disc)).toFixed(2);
                document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc)).toFixed(2);

                // Calculate2();

                if (table.rows.length == "0") {
                    document.getElementById('<%=txtCashRecieved2.ClientID%>').value = "";
                    document.getElementById('<%=txtBalance.ClientID%>').value = "";
                    document.getElementById("<%= btnToggleMode.ClientID %>").disabled = false;
                }
                document.getElementById('<%=txtskuCode.ClientID%>').focus();

                var tableData = storeTblValues();
                //tableData = $.toJSON(tableData);
                tableData = JSON.stringify(tableData);
                document.getElementById('<%=tab.ClientID%>').value = tableData;
            }

            function ClearControls() {
                document.getElementById('<%=txtskuID.ClientID%>').value = "";
                document.getElementById('<%=txtskuCode.ClientID%>').value = "";
                document.getElementById('<%=txtskuName.ClientID%>').value = "";
                var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
                if (mode == 'SALE MODE') {
                    document.getElementById('<%=txtQuantity.ClientID%>').value = "1";
                }
                else {
                    document.getElementById('<%=txtQuantity.ClientID%>').value = "-1";
                }
                document.getElementById('<%=txtDiscount.ClientID%>').value = "0";
                document.getElementById('<%=txtsize.ClientID%>').value = "";
                document.getElementById('<%=txtUnitRate.ClientID%>').value = "";
                document.getElementById("<%=btnToggleMode.ClientID%>").disabled = true;
            }

            function Calculate(e) {
               
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                if (key == 13) {
                    e.preventDefault();
                    if (document.getElementById('<%=txtskuCode.ClientID%>').value != "") {
                        addRow();
                        ClearControls();
                        document.getElementById('<%=txtskuCode.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                    }
                }
            }

            function CalculateBalance(e) {
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                if (key == 13) {
                    e.preventDefault();
                    var cashRcd = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                if (cashRcd == "") {
                    cashRcd = 0;
                }

                var netAmount = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                var disc = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                var balce = 0;
                var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;

                if (mode == 'SALE MODE') {
                    document.getElementById('<%=txtBalance.ClientID%>').value = (parseFloat(cashRcd) - parseFloat(netAmount)).toFixed(2);
                }
                else {

                    if ((cashRcd > 0) && (netAmount < 0)) {
                        balce = (parseFloat(netAmount) - parseFloat(disc) + parseFloat(cashRcd)).toFixed(2);
                        document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                    }
                    else if ((cashRcd > 0) && (netAmount > 0)) {
                        balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(cashRcd)).toFixed(2);
                        document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                    }
            }
        }
    }

    function Calculate2() {
        var cashRcd = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
        if (cashRcd == "") {
            cashRcd = 0;
        }
        var netAmount = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
        var disc = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
        var balce = 0;
        var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
        if (mode == 'SALE MODE') {

            if (cashRcd > 0) {
                document.getElementById('<%=txtBalance.ClientID%>').value = (parseFloat(cashRcd) - parseFloat(netAmount)).toFixed(2);
            }
            else {
                document.getElementById('<%=txtBalance.ClientID%>').value = parseFloat(netAmount).toFixed(2);
            }
        }
        else {
            if ((cashRcd > 0) && (netAmount < 0)) {

                balce = (parseFloat(netAmount) - parseFloat(disc) + parseFloat(cashRcd)).toFixed(2);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
            else if ((cashRcd > 0) && (netAmount > 0)) {
                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(cashRcd)).toFixed(2);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
    }
}



function ValidateForm() {
    var str = document.getElementById('<%=txtQuantity.ClientID%>').value;
    if (str == null || str.length == 0) {
        alert('Must Enter Quantity');
        return false;
    }
    return true;
}



function SetFocusTocashRecived(e) {
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    if (key == 13) {
        e.preventDefault();
        setTimeout(function () { document.getElementById("<%= txtAuthorisedBy.ClientID %>").focus(); }, 10);
    }
}

function SetFocusTocode(e) {
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    if (key == 13) {
        document.getElementById("<%= txtskuCode.ClientID %>").focus();
    }
    if (document.getElementById("<%= btnToggleMode.ClientID %>").value == 'SALE MODE') {
        if (document.getElementById("<%= txtQuantity.ClientID %>").value == '-') {

            alert('- is not allowed in sale mode!');
            document.getElementById("<%= txtQuantity.ClientID %>").value = '';
        }
    }
}

function FocusToCash(e) {
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    if (key == 13) {
        e.preventDefault();
        setTimeout(function () { document.getElementById("<%= txtCashRecieved2.ClientID %>").focus(); }, 10);
            }
        }


        function ProductSelected(source, eventArgs) {
            var skuDetail = eventArgs.get_text();
            var num = eventArgs.get_value();

            document.getElementById("<%=txtskuCode.ClientID %>").value = skuDetail.substring(0, skuDetail.indexOf('-'));
    }

    function toggle(t) {

        var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

        if (mode == 'SALE MODE') {
            document.getElementById("<%=btnToggleMode.ClientID %>").value = "REFUND MODE";
            document.getElementById("<%=hfToggleMode.ClientID %>").value = "REFUND MODE";
            document.getElementById("<%=txtQuantity.ClientID %>").value = "-1";
            document.getElementById("<%=btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModereturn");
            document.getElementById("<%=txtskuCode.ClientID %>").focus();
        } else if (mode == 'REFUND MODE') {
            document.getElementById("<%= btnToggleMode.ClientID %>").value = "SALE MODE";
            document.getElementById("<%=hfToggleMode.ClientID %>").value = "SALE MODE";
            document.getElementById("<%= txtQuantity.ClientID %>").value = "1";
            document.getElementById("<%= btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModesale");
            document.getElementById("<%=txtskuCode.ClientID %>").focus();
        }
}
///------------------------------------------
function CheckCreditLimit() {
    var e = document.getElementById("<%= DrpPayMode.ClientID %>");
    var payMode = e.options[e.selectedIndex].value;

    if (payMode == "218") {

        var balanceCeiling = document.getElementById("lblAllowLimit").innerHTML;
        var NetAmount = document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value;

        if (parseFloat(balanceCeiling) < parseFloat(NetAmount)) {

            alert('Please Check Customer Balance Ceiling');

            return false;
        }
    }
    else if (payMode == "214") {
        var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
        if (mode == 'SALE MODE') {
            var NetAmount = document.getElementById("<%= txtBalance.ClientID %>").value;

            var CashReceived = $("#txtCashRecieved2").value;
            if (parseInt(CashReceived <= 0)) {
                alert('Please Enter Cash Received');
                document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                return false;
            }
            else {
                if (NetAmount == null || NetAmount.length == 0) {
                    alert('Please enter Payment');
                    document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                    return false;
                }
                else if (parseFloat(NetAmount) < 0) {

                    alert('Receive amount not match with Net Amount');
                    document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                    return false;
                }
        }
    }
}
    return true;
}


        <%-- //////////////////Print Invoice region\\\\\\\\\\\\\\\\\\\\\--%>
            function PrintSaleInvoice() {
                if (CheckCreditLimit()) {
                    var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
                    if (mode == 'REFUND MODE') {

                        $("#invoiceMode").text("Sale Return");
                    }
                    else {
                        $("#invoiceMode").text("Sale Invoice");
                    }

                    var payMode = document.getElementById("<%= DrpPayMode.ClientID %>");
                    $("#payMode").text(payMode.options[payMode.selectedIndex].text);

                    var saleMan = document.getElementById("<%= ddsalesForce.ClientID %>");
                    $("#saleMan").text(saleMan.options[saleMan.selectedIndex].text);

                    $("#Cashier").text(saleMan.options[saleMan.selectedIndex].text);

                    var CustomerName = document.getElementById("<%= ddlCustomer.ClientID %>");
                    $("#lblCustomerName").text(CustomerName.options[CustomerName.selectedIndex].text);

                    var Units = 0;
                    $('#<%=dataTable.ClientID%>').find('tr').each(function () {

                        Units += parseInt($(this).find("td:eq(4)").text());
                    });

                    $("#Units").text(Units);

                    var orderedProducts = document.getElementById('<%=tab.ClientID%>').value;                  
                    orderedProducts = eval(orderedProducts);
                    $('#invoiceDetailBody').empty(); // clear all skus  from invoice

                    for (var i = 0, len = orderedProducts.length; i < len; i++) {
                        var row = $('<tr><td>' + orderedProducts[i].SKU_Code + '<br />' + orderedProducts[i].SKU_Name + '</td><td class="text-right">' + orderedProducts[i].QUANTITY_UNIT + '</td><td class="text-right">' + parseFloat(orderedProducts[i].UNIT_PRICE).toFixed(1) + '</td><td class="text-right">' + parseFloat(orderedProducts[i].STANDARD_DISCOUNT).toFixed(1) + '</td><td class="text-right">' + parseFloat(orderedProducts[i].NET_AMOUNT).toFixed(1) + '</td></tr>');

                        $('#invoiceDetailBody').append(row);
                    }
                    var gross = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
                    var discount = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                    var amountDue = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                    var paid = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                    var balance = document.getElementById('<%=txtBalance.ClientID%>').value;

                    $("#TotalValue").text(gross);
                    $("#DiscountTotal").text(parseFloat(discount).toFixed(2));
                    $("#GrandTotal").text(parseFloat(amountDue).toFixed(2));
                    $("#Paid").text(parseFloat(paid).toFixed(2));
                    $("#Balance").text(parseFloat(balance).toFixed(2));

                    if ($("#invoiceDetailBody tr").length > 0) {
                        $.print("#dvSaleInvoice");
                    }
                 
                    debugger;
                    
                    SaveInvoiceInDataBase();
                }
            }

            function SaveInvoiceInDataBase() {
                $.ajax({
                type: "POST", //HTTP method
                url: "http://localhost:5511/Forms/frmCornPosFashion.aspx/InsertInvoice", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    orderedProducts: document.getElementById('<%=tab.ClientID%>').value,
                    amountDue: document.getElementById('<%=txtGrossAmount.ClientID%>').value,
                    author: document.getElementById('<%=txtAuthorisedBy.ClientID%>').value,
                    discount: $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val(),
                    netAmount: $('#<%=numTxtTotlAmnt.ClientID%>').val(),
                    paidIn: $('#<%=txtCashRecieved2.ClientID%>').val(),
                    payType: document.getElementById("<%= DrpPayMode.ClientID %>").value,
                    Gst: document.getElementById("<%=numTxtTotalGST.ClientID %>").value,
                    manualId: document.getElementById("<%= hfToggleMode.ClientID %>").value,
                    customerId: document.getElementById("<%= ddlCustomer.ClientID %>").value,
                    saleForce: document.getElementById("<%= ddsalesForce.ClientID %>").value
                }),
                success: InvoiceSaved,
                error: invoiceNotSaved
            });
            }

              function savetoLocal() {

                var ordData = localStorage.getItem("ordData");
                var obj = [];
                if (ordData) {
                    obj = JSON.parse(ordData);
                }
                obj.push({orderedProducts: document.getElementById('<%=tab.ClientID%>').value,
                    amountDue: document.getElementById('<%=txtGrossAmount.ClientID%>').value,
                    author: document.getElementById('<%=txtAuthorisedBy.ClientID%>').value,
                    discount: $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val(),
                    netAmount: $('#<%=numTxtTotlAmnt.ClientID%>').val(),
                    paidIn: $('#<%=txtCashRecieved2.ClientID%>').val(),
                    payType: document.getElementById("<%= DrpPayMode.ClientID %>").value,
                    Gst: document.getElementById("<%=numTxtTotalGST.ClientID %>").value,
                    manualId: document.getElementById("<%= hfToggleMode.ClientID %>").value,
                    customerId: document.getElementById("<%= ddlCustomer.ClientID %>").value,
                    saleForce: document.getElementById("<%= ddsalesForce.ClientID %>").value 
                });
                localStorage.setItem("ordData", JSON.stringify(obj));
            }

            // save sale invoice to data base from local storage
            function SaveInvoiceInDataBaseFromLocalStorage() {
                var ordData = JSON.parse(localStorage.getItem("ordData"));

                //alert(ordData.length);
                for (var i = 0; i < ordData.length; i++) {
                    $.ajax({
                        type: "POST", //HTTP method  data: JSON.stringify({ d: arr }),
                        url: "http://localhost:5511/Forms/frmCornPosFashion.aspx/InsertInvoice", //page/method name
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            orderedProducts: ordData[i].orderedProducts,
                            amountDue: ordData[i].amountDue,
                            author: ordData[i].author,
                            discount: ordData[i].discount,
                            netAmount: ordData[i].netAmount,
                            paidIn: ordData[i].paidIn,
                            payType: ordData[i].payType,
                            Gst: ordData[i].Gst,
                            manualId: ordData[i].manualId,
                            customerId: ordData[i].customerId,
                            saleForce: ordData[i].saleForce
                        }),
                        success: InvoiceSaved,
                        error: invoiceNotSaved
                    });
                }
            }

            function InvoiceSaved() {
            document.getElementById('<%=tab.ClientID%>').value = "";
            $('#<%=dataTable.ClientID%> tr').empty();
            Clear();
            __doPostBack('UpdatePanel1', '');
            UpdateCreditLimit();
            }

            function invoiceNotSaved() {
                savetoLocal();
                alert('Some error occurred!');
            }

        // Update Limit After Insertion
        function UpdateCreditLimit() {
            var e = document.getElementById("<%= DrpPayMode.ClientID %>");
            var payMode = e.options[e.selectedIndex].value;

            if (payMode == "218") {
                document.getElementById('<%= btnUpdateLimit.ClientID %>').click();
            }
        }

        function Clear() {
            $('#<%=txtGrossAmount.ClientID%>').val('');
            $('#<%=txtAuthorisedBy.ClientID%>').val('');
            $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val('');
            $('#<%=numTxtTotlAmnt.ClientID%>').val('');
            $('#<%=txtCashRecieved2.ClientID%>').val('');
            $('#<%=txtBalance.ClientID%>').val('');
            $('#<%=numTxtTotalGST.ClientID %>').val('');

            var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
            if (mode == 'REFUND MODE') {
                document.getElementById("<%= txtQuantity.ClientID %>").value = "-1";
            }
            else {
                document.getElementById("<%= txtQuantity.ClientID %>").value = "1";
            }
        }


        function PaymentMode() {
            var e = document.getElementById("<%= DrpPayMode.ClientID %>");
            var payMode = e.options[e.selectedIndex].value;
            if (payMode == "215" || payMode == "218") {

                document.getElementById("<%= txtCashRecieved2.ClientID %>").value = "";

                document.getElementById("<%= txtBalance.ClientID %>").value = document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value;
                document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = true;
            } else {
                document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = false;
            }
            document.getElementById("<%=txtskuCode.ClientID %>").focus();
        }

        </script>







        <asp:ScriptManager ID="ScriptManger1" runat="Server">
        </asp:ScriptManager>

        <nav class="nav navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand" href="#">
                        <img src="../images/logo.png" /></a>
                </div>

                <ul class="nav navbar-nav navbar-right">
                    <li><a>
                        <asp:Label Text="" runat="server" ID="lblLoacation"></asp:Label></a></li>
                    <li><a>
                        <asp:Label ID="lbllogintimedate" runat="server" Text=""></asp:Label></a></li>
                    <li class="dropdown">
                        <a href="#" class="avatar dropdown-toggle" data-toggle="dropdown">
                            <img src="../images/avatar.png" class="img-circle" height="40" width="40" alt="Avatar" />

                            <asp:Label ID="lbluserlogin" runat="server" Text=""></asp:Label>
                            <span class="caret"></span>
                        </a>
                        <ul class="dropdown-menu">

                            <li><a href="#">Account Settings</a></li>
                            <li class="divider"></li>
                            <li><a href="#">Messages</a></li>
                            <li class="divider"></li>
                            <li><a href="#">Sign Out</a></li>
                        </ul>
                    </li>
                    <li><%--<a class="sale-mode btn" data-toggle="modal" data-target="#myModal">SALE MODE</a>--%>

                        <input style="background-color: #64B5F6; color: #fff; padding-top: 15px; padding-bottom: 15px;" type="button" id="btnToggleMode" runat="server" value="SALE MODE" class="sale-mode btn"
                            onclick="toggle(this);" />
                        <asp:HiddenField runat="server" ID="hfToggleMode" Value="SALE MODE" />



                    </li>



                    <li><a class="close-button" href='javascript:history.go(-1)'><span class="glyphicon glyphicon-remove"></span></a></li>
                </ul>

            </div>
        </nav>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Header</h4>
                    </div>
                    <div class="modal-body">
                        <p>Popup Text.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>



        <div class="modal fade" id="myModalReportCriteria" role="dialog">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Report Criteria</h4>
                    </div>
                    <div class="modal-body">
                        <table>
                            <tr>
                                <td>Type: </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server" class="form-control">
                                        <asp:ListItem Text="Summary" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Detail Report" Value="2"></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>From: </td>
                                <td>
                                    <asp:TextBox ID="txtstartDate" runat="server" CssClass="form-control" MaxLength="11" onkeyup="BlockStartDateKeyPress()"></asp:TextBox></td>
                                <td>&nbsp;
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="25px" /></td>
                            </tr>
                            <tr>
                                <td>To: </td>
                                <td>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control" MaxLength="11" onkeyup="BlockEndDateKeyPress()"></asp:TextBox></td>
                                <td>&nbsp;
                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="25px" />
                                    
                                    
                                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                        TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="modal-footer">
                        <%-- <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                        <asp:LinkButton runat="server" ID="btnViewSalesReport" OnClick="btnViewSalesReport_Click" class="btn btn-default" ToolTip="View">
                                        Submit</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="btnUpdateLimit" runat="server" />
                <asp:HiddenField ID="hfCompanyName" runat="server" />
                <asp:HiddenField ID="hfLocationName" runat="server" />
                <asp:HiddenField ID="hfContactNo" runat="server" />
                <asp:HiddenField ID="hfMaxId" runat="server" />
                <asp:HiddenField ID="hfProduct" runat="server" Value="0" />
                <asp:HiddenField ID="txtskuID" runat="server" />
                <asp:HiddenField ID="tab" runat="server" />


                <asp:HiddenField ID="hfuserlogin" runat="server" />
                <asp:HiddenField ID="hfSlipNote" runat="server" />

                <div style="z-index: 101; left: 450px; width: 400px; position: absolute; top: 369px; height: 100px">
                    &nbsp;<asp:Panel ID="Panel21" runat="server">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Granite/Images/image003.gif" />
                                Wait Update
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </asp:Panel>
                </div>

                <div class="container-fluid top-menu">
                    <div class="row text-center">
                        <div class="col-lg-1 col-md-3 col-sm-3 customer">
                            <a href="#" onclick="window.open('frmCustomer.aspx', '_blank', 'status=no'); return false;">CUSTOMER</a>
                        </div>
                        <div class="col-lg-1 col-md-2 col-sm-2 hold">
                            <a href="#">HOLD (3) 
                            </a>
                        </div>
                        <div class="col-lg-1 col-md-2 col-sm-2 unhold">
                            <a href="#">UN HOLD
                            </a>
                        </div>
                        <div class="col-lg-1 col-md-2 col-sm-2 price">
                            <a href="#">PRICE LOOKUP
                            </a>
                        </div>
                        <div class="col-lg-1 col-md-3 col-sm-3 credit">
                            <a href="#">CREDIT NOTE
                            </a>
                        </div>




                        <div class="col-lg-2 col-md-3 col-sm-3 bigger">
                            <p>CUSTOMER</p>
                            <%-- <input type="text" class="form-control" id="customer">--%>

                            <asp:DropDownList ID="ddlCustomer" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" class="form-control" AutoPostBack="true">
                            </asp:DropDownList>

                            <%--<dx:ASPxComboBox ID="ddlCustomer" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { SetComboBoxImage(s); }" />
                            </dx:ASPxComboBox>--%>
                        </div>
                        <div class="col-lg-2 col-md-3 col-sm-3 bigger">
                            <p>PAYMENT MODE</p>
                            <%--<input type="text" class="form-control" id="payment-mode">--%>
                            <asp:DropDownList ID="DrpPayMode" runat="server" class="form-control">
                                <asp:ListItem Selected="True" Value="214">Cash</asp:ListItem>
                                <asp:ListItem Value="217">Cash & Credit Card</asp:ListItem>
                                <asp:ListItem Value="215">Credit Card</asp:ListItem>
                                <asp:ListItem Value="218">Credit</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-lg-3 col-md-6 col-sm-6 two-dropdown">
                            <div class="col-lg-5 col-md-5 col-sm-5 bigger">
                                <p>DISCOUNT</p>
                                <%--<select class="form-control" id="discount1">
                        <option></option>
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                        <option>4</option>
                    </select>--%>

                                <select id="DrpDiscount" runat="server" class="form-control">
                                    <option value="0">% age</option>
                                    <option value="1">Value</option>
                                </select>

                            </div>
                            <div class="col-lg-7 col-md-7 col-sm-7 bigger">
                                <p>SALE PERSON</p>
                                <%--<select class="form-control" id="sale-person">
                        <option></option>
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                        <option>4</option>
                    </select>--%>

                                <asp:DropDownList ID="ddsalesForce" runat="server" class="form-control">
                                </asp:DropDownList>
                            </div>

                        </div>

                    </div>
                </div>











                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-9 middle">

                            <div class="middle-form">
                                <div class="col-lg-2 col-md-3 col-sm-3">
                                    <p>Item Code</p>
                                    <%-- <input type="text" class="form-control" id="sku-code">--%>
                                    <asp:TextBox ID="txtskuCode" class="form-control" runat="server" onkeypress="Calculate(event);"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoComplete" runat="server" TargetControlID="txtskuCode"
                                        ServicePath="wsProductList.asmx" MinimumPrefixLength="4" CompletionInterval="500"
                                        UseContextKey="true" BehaviorID="AutoCompleteBehavior" CompletionSetCount="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                        EnableCaching="true" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        OnClientItemSelected="ProductSelected" FirstRowSelected="true" ServiceMethod="GetPosProducts">
                                    </cc1:AutoCompleteExtender>
                                </div>
                                <div class="col-lg-4 col-md-5 col-sm-5">
                                    <p>Item Name</p>

                                    <asp:TextBox ID="txtskuName" class="form-control" runat="server" Enabled="False"
                                        Font-Bold="True"></asp:TextBox>
                                </div>
                                <div class="col-lg-1 col-md-4 col-sm-4">
                                    <p>Size</p>

                                    <asp:TextBox ID="txtsize" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                                </div>
                                <div class="col-lg-1 col-md-3 col-sm-3">
                                    <p>Quantity</p>

                                    <asp:TextBox ID="txtQuantity" runat="server" onkeypress="SetFocusTocode(event)" class="qty-input form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                        ValidChars="-0123456789." TargetControlID="txtQuantity" />
                                </div>
                                <div class="col-lg-1 col-md-3 col-sm-3">
                                    <p>Discount</p>

                                    <asp:TextBox ID="txtDiscount" class="form-control" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                        ValidChars="0123456789." TargetControlID="txtDiscount" />
                                </div>
                                <div class="col-lg-1 col-md-3 col-sm-3">

                                    <p>Unit Price</p>
                                    <asp:TextBox ID="txtUnitRate" class="form-control" runat="server" Enabled="False"></asp:TextBox>

                                </div>
                                <%--  <div class="col-lg-2 col-md-3 col-sm-3">
                                    <p>Amount</p>
                                  
                                    <asp:TextBox ID="txtTotalPrice" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                                </div>--%>
                            </div>


                            <div class="result">
                                <asp:Panel ID="Panel2" runat="server" Height="405px" BorderColor="Silver">
                                    <asp:Label ID="lblfound" ForeColor="Red" Font-Size="Medium" runat="server"></asp:Label>
                                    <asp:Label ID="lblClosingStock" ForeColor="White" runat="server"></asp:Label>
                                    <%--<table border="1" width="100%"><tr><td>Item Code</td><td>Item Name</td><td>3</td><td>4</td><td>5</td></tr></table>--%>
                                    <asp:Table ID="dataTable" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed">
                                    </asp:Table>
                                    <%-- <asp:GridView ID="GrdPurchase" runat="server" ForeColor="SteelBlue" RowStyle-Height="30px"
                                        Visible="false" BackColor="White" HorizontalAlign="Center" AutoGenerateColumns="False"
                                        BorderColor="White" ShowHeader="False" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle CssClass="HidePanel" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel" VerticalAlign="Middle"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" BorderColor="Silver" BorderWidth="2px"
                                                    BorderStyle="Solid" Font-Bold="true" Font-Size="16px" Width="167"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="317px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COLOR" HeaderText="COLOR">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Font-Bold="true"
                                                    VerticalAlign="Middle" HorizontalAlign="Left" Font-Size="16px" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PACKSIZE" HeaderText="PACKSIZE">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QUANTITY_UNIT" HeaderText="QUANTITY_UNIT">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STANDARD_DISCOUNT" HeaderText="DISCOUNT" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIT_PRICE" HeaderText="PRICE" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="90px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NET_AMOUNT" HeaderText="Amount" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="65px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CHECK_DELETE" HeaderText="CHECK_DELETE">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" BorderColor="Red" Text="Void"
                                                        CommandName="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle BorderColor="Silver" HorizontalAlign="Center" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="30px" Font-Bold="true" ForeColor="Red" Font-Overline="true" Font-Size="14px"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Height="30px" />
                                    </asp:GridView>--%>
                                </asp:Panel>
                            </div>

                            <div class="row bottom">
                                <%--OnClick="btnViewSalesReport_Click"--%>
                                <div class="col-lg-4 col-md-4 col-sm-5 report">
                                    <a href="#" data-toggle="modal" data-target="#myModalReportCriteria">
                                        <span>
                                            <img src="../images/printF.png"></span><span>SALE<br>
                                                REPORT</span></a></div>
                                <div class="col-lg-8 col-md-8 col-sm-7 void">
                                    <br />
                                    <asp:LinkButton runat="server" ID="btnVoid" OnClick="btnVoid_Click" class="btn-void">VOID</asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnSaveOrder" OnClientClick="PrintSaleInvoice();" ToolTip="SAVE & PRINT"
                                        class="btn-save" AccessKey="S">SAVE & PRINT</asp:LinkButton>
                                </div>

                                 <div class="col-lg-8 col-md-8 col-sm-7 void">

                                    <asp:LinkButton runat="server" ID="btnSaveOrderFromLocalStorage" OnClientClick="SaveInvoiceInDataBaseFromLocalStorage();" ToolTip="SAVE MISSED ORDERS"
                                        class="btn-save" AccessKey="S">SAVE MISSED ORDERS</asp:LinkButton>
                                  </div>
                                
                                <div class="row detail">
                                    <div class="col-lg-8 col-md-8 col-sm-10 col-xs-10 pull-right">
                                        <div class="col-lg-4 col-md-4 col-sm-10">
                                            <span class="credit-ceiling">CREDIT CEILING: </span>

                                            <asp:Label ID="lblCreditLimit" runat="server"></asp:Label></h2>
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-10">
                                            <span class="ledger-balance">LEDGER BALANCE: </span>
                                            <asp:Label ID="lblLedgerBalance" runat="server"></asp:Label></h2>
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-10">
                                            <span class="balance-ceiling">BALANCE CEILING: </span>
                                            <asp:Label ID="lblAllowLimit" runat="server"></asp:Label></h2>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="col-sm-3 right-sidebar">

                            <div class="thumbnail">
                                <img src="../images/product.png" alt="product" width="400" height="300">
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <h5>GROSS SALE</h5>
                                    <%--<input type="text" class="form-control" id="cross-sale" placeholder="0.00">--%>
                                    <asp:TextBox ID="txtGrossAmount" class="form-control" placeholder="0.00" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <h5>DISCOUNT</h5>
                                    <%-- <input type="text" class="form-control" id="discount" placeholder="0.00">--%>
                                    <asp:TextBox ID="numtxtTotalExtraDiscnt" class="form-control" runat="server" placeholder="0.00" ValidationGroup="NumbersOnly"></asp:TextBox>
                                </div>

                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <h5>AUTHORISED BY</h5>
                                    <%--<input type="text" class="form-control" id="authorised">--%>
                                    <asp:TextBox ID="txtAuthorisedBy" onkeypress="FocusToCash(event)" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <h5>SALES TAX</h5>
                                    <%--<input type="text" class="form-control" id="sales-tax" placeholder="0.00">--%>
                                    <asp:TextBox ID="numTxtTotalGST" class="form-control" placeholder="0.00" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <h5>NET AMOUNT</h5>
                                    <%--<input type="text" class="form-control" id="net-amount" placeholder="0.00">--%>
                                    <asp:TextBox ID="numTxtTotlAmnt" class="form-control" placeholder="0.00" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <h5>CASH RECIEVED</h5>
                                    <%--<input type="text" class="form-control cash" id="cash-rec" placeholder="0.00">--%>
                                    <asp:TextBox ID="txtCashRecieved2" class="form-control cash" placeholder="0.00" onkeypress="CalculateBalance(event);" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                        ValidChars="0123456789." TargetControlID="txtCashRecieved2" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <h5>BALANCE</h5>
                                    <%--<input type="text" class="form-control balance" id="balance" placeholder="0.00">--%>
                                    <asp:TextBox ID="txtBalance" class="form-control balance" placeholder="0.00" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnViewSalesReport" />
            </Triggers>
        </asp:UpdatePanel>




        <div style="display: none; width: 2.6in;">
            <div id="dvSaleInvoice">
                <style type="text/css">
                    #dvSaleInvoice {
                        width: 2.6in;
                    }

                    #SaleInvoice {
                        width: 2.6in;
                    }

                    #CompanyName {
                        font-size: 18px;
                        font-weight: bold;
                    }

                    #SaleInvoiceText {
                        font-size: 14px;
                    }

                    #InvoiceDate {
                        font-weight: bold;
                    }

                    #CustomerType {
                        font-weight: bold;
                    }

                    #phoneNo {
                        font-weight: bold;
                    }

                    #hrSaleInvoiceHead {
                        border: #333333 solid 1px;
                    }

                    #invoiceDetail {
                        width: 98%;
                        margin-top: 10px;
                    }

                    #invoiceDetailBody tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 12px;
                        padding: 2px;
                    }

                    .text-right {
                        text-align: right;
                    }

                    #invoiceDetailFoot tr td {
                        font-family: Sans-Serif;
                        font-size: 14px;
                        font-weight: bold;
                    }
                </style>
                <table id="SaleInvoice">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="CompanyName">
                                <%=hfCompanyName.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <%=hfLocationName.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <%=hfContactNo.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-size: 12px; font-family: Sans-Serif; font-style: italic;">
                            <label id="invoiceMode"></label>

                        </td>

                        <td>MOP: 
                        <td>MOP: <span id="Span1" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;">
                            <label id="payMode">Cash</label>
                        </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;">Date : 
                        <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"></span>
                            <asp:Label ID="lblInvoiceDate" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;">No of Units : &nbsp;<label style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"
                            id="Units">
                        </label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;">Inv #<label style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"
                            id="CustomerType">
                            <%=hfMaxId.Value %>
                        </label>
                        </td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;">Saleman: <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <label id="saleMan"></label>
                        </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" style="font-style: italic; font-size: 12px; margin-bottom: 5px;">CUSTOMER: &nbsp;<span id="customerName" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerName">Walk In Customer</label></span>
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: -8px; margin-top: 2px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="invoiceDetail">
                                <thead id="invoiceDetailHead">
                                    <tr>
                                        <td style="text-align: left; font-size: 12px; font-family: Sans-Serif; width: 32%">Item Name
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 10%">Qty
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 15%">Price
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 10%">Disc
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 15%">Amount
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="invoiceDetailBody">
                                </tbody>
                                <tfoot id="invoiceDetailFoot">
                                    <tr>
                                        <td colspan="2">
                                            <label id="TotalValue-text">
                                                GROSS AMOUNT :
                                            </label>
                                        </td>
                                        <td colspan="3" style="text-align: right;">
                                            <label id="TotalValue">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label>
                                                DISCOUNT :
                                            </label>
                                        </td>

                                        <td colspan="3" style="text-align: right;">
                                            <label id="DiscountTotal">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="GrandTotal-text">
                                                AMOUNT-DUE :
                                            </label>
                                        </td>

                                        <td colspan="3" style="text-align: right;">
                                            <label id="GrandTotal">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="Paid-text">
                                                CASH-PAID-IN :
                                            </label>
                                        </td>
                                        <td colspan="3" style="text-align: right;">
                                            <label id="Paid">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="Balance-text">
                                                BALANCE :
                                            </label>
                                        </td>
                                        <td colspan="3" style="text-align: right;">
                                            <label id="Balance">
                                            </label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">CASHIER : 
                                            <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                                                <%=hfuserlogin.Value%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <%=hfSlipNote.Value%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">THANKS FOR YOUR VISIT.
                                        <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                                            <span style="font-size: 10px;">Powered by:FastServices.pk</span>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </div>


    </form>
</body>
</html>
