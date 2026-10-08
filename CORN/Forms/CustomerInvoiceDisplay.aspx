<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerInvoiceDisplay.aspx.cs" Inherits="Forms_CustomerInvoiceDisplay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport' />
    <title></title>
    
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <style>
        .scrolla {
            overflow-y: scroll;
            overflow-x: hidden;
        }
        .table-bar {
            width: 100%;
            background-color: #eff2f8;
            float: left;
            padding: 5px 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-10">
                    <div class="table-bar">
                        <div class="emp-table scrolla" style="min-height: 240px;">
                            <table class="table table-striped table-bordered table-hover table-condensed cf">
                                <thead class="cf head">
                                    <tr>
                                        <th class="numeric table-text-head">ITEM(S)
                                        </th>
                                        <th class="table-text-head">QTY
                                        </th>
                                        <th class="numeric table-text-head">PRICE
                                        </th>
                                        <th class="numeric table-text-head">AMOUNT
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="tble-ordered-products"></tbody>
                                <tfoot>
                                    <tr>
                                        <th colspan="3" style="text-align: right; padding-right: 8px;">Due Amount</th>
                                        <th class="due-amount" colspan="3" style="text-align: right; padding-right: 2px;"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="3" style="text-align: right; padding-right: 8px;">Payment In</th>
                                        <th class="payment-in" colspan="3" style="text-align: right; padding-right: 2px;"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="3" style="text-align: right; padding-right: 8px;">Customer Balance</th>
                                        <th class="customer-balance" colspan="3" style="text-align: right; padding-right: 2px;"></th>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-1"></div>
            </div>
        </div>
        <table class="table table-bordered table-hover">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Item Name</th>
                    <th>Quantity</th>
                    <th>Rate</th>
                    <th>Amount</th>
                </tr>
            </thead>
            <tbody id="dvDiningTableBillDetail"></tbody>
        </table>

        <%--Start:Templates--%>
        <div style="display: none;">
            <div class="DiningTableBillDetailTemplate">
                <tr>
                    <th scope="row">1</th>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </div>
        </div>

        <div id="dialogAlert" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Modal title</h4>
                </div>
                <div class="modal-body">
                    <p>One fine body&hellip;</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Ok</button>
                </div>
            </div><!-- /.modal-content -->
        </div><!-- /.modal-dialog -->
    </div><!-- /.modal -->

        <%--End:Templates--%>

    </form>
    
    <script src="../js/jquery-2.0.2.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function showMessageDialog(msg, title) {
            var $dialog = $('#dialogAlert');
            $('.modal-title', $dialog).html(title);
            $('.modal-body', $dialog).first('p').html(msg);
            $dialog.modal('show');
        }
    </script>
</body>
</html>
