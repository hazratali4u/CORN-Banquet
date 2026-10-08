<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Forms/PageMaster.master" CodeFile="frmEventBooking.aspx.cs" Inherits="Forms_frmEventBooking" Title="CORN :: Banquet" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function ClearSelection(lb) {
            lb.selectedIndex = -1;
        }
        function MyFunction() {
            var txtAmount = document.getElementById('<%=txtAmount.ClientID%>').value;
            var txtRate = document.getElementById('<%=txtRate.ClientID%>').value;
            var txtQty = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (txtAmount == "")
                document.getElementById('<%=txtAmount.ClientID%>').value = 0;
            if (txtQty == "")
                document.getElementById('<%=txtQuantity.ClientID%>').value = 0;
            var value = parseFloat(txtRate) * parseFloat(txtQty);
            var result = parseFloat(value);
            if (!isNaN(result)) {
                document.getElementById('<%=txtAmount.ClientID%>').value = result;
            }
            else {
                document.getElementById('<%=txtAmount.ClientID%>').value = 0;
            }
        }

        function CalculateRemaniningAmount() {

            var txtallTotal = document.getElementById('<%=txtallTotal.ClientID%>').value;
            var txtReceiptsAmount = document.getElementById('<%=txtReceiptsAmount.ClientID%>').value;
            var txtAdvanceAmount = document.getElementById('<%=txtAdvanceAmount.ClientID%>').value;
                        
            if (txtallTotal == "")
                document.getElementById('<%=txtallTotal.ClientID%>').value = 0;
            if (txtAdvanceAmount == "")
                document.getElementById('<%=txtAdvanceAmount.ClientID%>').value = 0;
            if (txtReceiptsAmount == "")
                document.getElementById('<%=txtReceiptsAmount.ClientID%>').value = 0;

            

           var balance = parseInt(CheckComma(empty(txtallTotal))) - parseInt(CheckComma(empty(txtAdvanceAmount))) - parseInt(CheckComma(empty(txtReceiptsAmount)));
           document.getElementById('<%=txtRemainingAmount.ClientID%>').value = numberWithCommas(balance);

            function empty(str) {
                if (typeof str == 'undefined' || !str || str.length === 0 || str === "" || !/[^\s]/.test(str) || /^\s*$/.test(str) || str.replace(/\s/g, "") === "") {
                    return 0;
                }
                else {
                    return str;
                }
            }
            function isAlphanumeric(str) {
                return /^[0-9a-zA-Z]+$/.test(str);
            }
            function CheckComma(str) {
                if (isAlphanumeric(str)) {
                    return str;
                }
                else {
                    return str.replace(/,/g, '');
                }
            }
            function numberWithCommas(number) {
                var parts = number.toString().split(".");
                parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                return parts.join(".");
            }

        }

        //ctl00_ctl00_mainCopy_cphPage_txtRate

        <%--function pageLoad() {
            if (document.getElementById('<%=BtnAdd.ClientID%>').value == "Add") {
                var SKUDetail = $("#ctl00_ctl00_mainCopy_cphPage_ddlSKU_I").val().split(':');
                document.getElementById("<%= txtRate.ClientID %>").value = parseFloat(SKUDetail[2]).toFixed(2);
                document.getElementById("<%= txtUOM.ClientID %>").value = SKUDetail[1];
                document.getElementById("<%= txtQuantity.ClientID %>").focus();
            }
            $("#ctl00_ctl00_mainCopy_cphPage_ddlSKU_I").change(function () {
                var SKUDetail = $("#ctl00_ctl00_mainCopy_cphPage_ddlSKU_I").val().split(':');
                document.getElementById("<%= txtRate.ClientID %>").value = parseFloat(SKUDetail[2]).toFixed(2);
                document.getElementById("<%= txtUOM.ClientID %>").value = SKUDetail[1];
                document.getElementById("<%= txtQuantity.ClientID %>").focus();
            });
        }--%>
        //$(function () {
        //    $("#ctl00_ctl00_mainCopy_cphPage_ddlSKU_I").change(function () {
        //        var selectedText = $(this).find("option:selected").text();
        //        var selectedValue = $(this).val();
        //        alert("Selected Text: " + selectedText + " Value: " + selectedValue);
        //    });
        //});
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row">
                <div class="col-md-10">
                    <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>
            <div class="row">
                <br />
            </div>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Customer Name:</label>
                            <dx:ASPxComboBox ID="ddlCustomer" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" runat="server" CssClass="form-control"></dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>ID Card:</label>
                            <asp:TextBox ID="txtIDCard" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Customer Number:</label>
                            <asp:TextBox ID="txtCustomerNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Other No:</label>
                            <asp:TextBox ID="txtOtherNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-8">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Mailing Address:</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Event Date:</label>
                            <asp:TextBox ID="txtEventDate" runat="server" AutoPostBack="true" OnTextChanged="txtEventDate_TextChanged" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 30px">
                            <asp:ImageButton ID="ibtnEventDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" />
                            <cc1:CalendarExtender ID="CEEventDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnEventDate" TargetControlID="txtEventDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Dine Time</label>
                            <dx:ASPxComboBox ID="ddDineType" runat="server" CssClass="form-control"></dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Event Type:</label>
                            <dx:ASPxComboBox ID="ddlEventType" runat="server" CssClass="form-control"></dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Booking Date:</label>
                            <asp:TextBox ID="txtBookingDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 30px">
                            <asp:ImageButton ID="ibtnBookingDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" />
                            <cc1:CalendarExtender ID="CEBookingDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnBookingDate" TargetControlID="txtBookingDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Hall #</label>
                            <dx:ASPxComboBox ID="ddlHallNo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlHallNo_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Ladies</label>
                            <asp:TextBox ID="txtLadies" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Gents</label>
                            <asp:TextBox ID="txtGents" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row center">
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Particulars</label>
                            <dx:ASPxComboBox ID="ddlSKU" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSKU_SelectedIndexChanged" AutoPostBack="true" Width="100%" onchange="MyFunction();"></dx:ASPxComboBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                            <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Quantity</label>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" onkeyup="MyFunction()" Style="text-align: right;" Width="100%"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Rate</label>
                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Style="text-align: right;" Width="100%" onblur="MyFunction();"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Amount</label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Style="text-align: right;" Width="100%"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="BtnAdd" OnClick="BtnAdd_Click" runat="server" CssClass="btn btn-primary" Text="Add" Style="margin-top: 25px;" Width="100%" />
                            <%--OnClientClick="grandTotal();"--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="250px" Width="100%" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdItems" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                        CssClass="table table-striped table-bordered table-hover table-condensed cf" ShowHeader="False">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" ReadOnly="true">
                                                <ItemStyle Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_DESC" ReadOnly="true">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QTY" DataFormatString="{0:f2}" ReadOnly="true">
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PRICE" DataFormatString="{0:f2}" ReadOnly="true">
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AMOUNT" DataFormatString="{0:f2}" ReadOnly="true">
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("SKU_ID")%>' OnClick="btnEdit_Click" ToolTip="Edit" class="fa fa-pencil">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("SKU_ID")%>' OnClick="btnDelete_Click" ToolTip="Edit" class="fa fa-trash-o" OnClientClick="javascript:return confirm('Are you sure you want to perform this action?');return false;">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <%--<div class="col-md-offset-10 col-md-2 right">
                    <br />
                    <b>Grand Total:</b>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="itemsGrandTotal" runat="server" Style="text-align: right;">0</asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>--%>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%; border-top: none !important;" class="table Mytable">
                                <tr style="border-top: none !important;">
                                    <td style="width: 65%;"></td>
                                    <td style="width: 20%; text-align: right;">
                                        <b>Total Amount:</b>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:TextBox Width="100%" ID="txtallTotal" ReadOnly="true" runat="server" Style="text-align: right;">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="border-top: none !important;">
                                    <td style="width: 55%;"></td>
                                    <td style="width: 20%; text-align: right;">
                                        <b>Advance Amount:</b>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:TextBox Width="100%" onkeyup="CalculateRemaniningAmount(this);" ID="txtAdvanceAmount" runat="server" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <asp:Panel ID="PanelReceipts" runat="server" Visible="true">
                                    <tr style="border-top: none !important;">
                                        <td style="width: 55%;"></td>
                                        <td style="width: 20%; text-align: right;">
                                            <b>Receipts Amount:</b>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:TextBox Width="100%" ID="txtReceiptsAmount" runat="server" Style="text-align: right;"></asp:TextBox>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr style="border-top: none !important;">
                                    <td style="width: 55%;"></td>
                                    <td style="width: 20%; text-align: right;">
                                        <b>Remaining Amount:</b>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:TextBox Width="100%" ID="txtRemainingAmount" ReadOnly="true" runat="server" Style="text-align: right;">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="border-top: none !important;">
                                    <td style="width: 55%; text-align: left;">
                                        <asp:Button ID="btnSaveDocument" OnClick="btnSaveDocument_Click" AccessKey="S" Width="150px" runat="server" Text="Save" CssClass="btn btn-success" />
                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" AccessKey="C" runat="server" Text="Cancel" Width="150px" UseSubmitBehavior="False" CssClass="btn btn-danger" />
                                    </td>
                                    <td style="width: 20%; text-align: right;">
                                        <asp:HiddenField ID="hfEVENT_BOOKING_ID" runat="server" Value="0" />
                                    </td>
                                    <td style="width: 25%;"></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
