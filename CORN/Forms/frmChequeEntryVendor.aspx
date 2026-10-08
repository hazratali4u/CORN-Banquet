<%@ Page Title="Supplier Payment" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmChequeEntryVendor.aspx.cs" Inherits="Forms_frmChequeEntryVendor" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100005;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;
            str = document.getElementById("<%= txtAmount.ClientID %>").value;
            if (str == null || str.length == 0) {
                alert('Amount is required');
                return false;
            }

        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;
            return true;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 100px">
                <asp:Panel ID="Panel21" runat="server">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">

                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Payment Type</label>
                                    <asp:CheckBox ID="ChkAdvance" Text=" Advance Payment" Style="margin-left: 100px;" runat="server" />
                                    <dx:ASPxComboBox ID="DrpAccountType" runat="server" AutoPostBack="True" SelectedIndex="0"
                                        OnSelectedIndexChanged="DrpAccountType_SelectedIndexChanged" CssClass="form-control">
                                        <Items>
                                            <dx:ListEditItem Value="18" Text="Cheque Payment"></dx:ListEditItem>
                                            <dx:ListEditItem Value="21" Text="Cash Payment"></dx:ListEditItem>
                                            <dx:ListEditItem Value="33" Text="Online Transfer"></dx:ListEditItem>
                                        </Items>
                                    </dx:ASPxComboBox>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Account Head</label>
                                    <dx:ASPxComboBox ID="DrpBankAccount" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="DrpBankAccount_SelectedIndexChanged" AutoPostBack="True">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Amount</label>
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"
                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <label runat="server" id="lblChequeDate"><span class="fa fa-caret-right rgt_cart"></span>Cheque Date</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" Style="padding-top: 26px; margin-left: -10px;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>

                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Button AccessKey="S" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" Width="125px" />
                            <asp:Button AccessKey="C" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" Width="125px" />
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Supplier</label>
                                    <dx:ASPxComboBox ID="DrpVendor" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="DrpVendor_SelectedIndexChanged" AutoPostBack="True">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-11">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Transaction Date</label>
                                    <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:ImageButton ID="ibTransaction" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" Style="padding-top: 26px; margin-left: -10px;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="display: none">
                                    <label runat="server" id="lblStatus"><span class="fa fa-caret-right rgt_cart"></span>Status</label>
                                    <dx:ASPxComboBox ID="DrpStatus" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="DrpStatus_SelectedIndexChanged" AutoPostBack="True">
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-6">
                                    <label runat="server" id="lblChequeNo"><span class="fa fa-caret-right rgt_cart"></span>Cheque No</label>
                                    <dx:ASPxComboBox ID="drpCheque" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="col-md-12">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Payable Amount:</label>
                                <asp:TextBox ID="txtTotalCreditAmount" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <asp:CheckBox ID="ChkIsDiscount" Text="%" runat="server" Visible="false" />
                    <asp:TextBox ID="txtDiscount" runat="server"
                        onkeypress="return onlyDotsAndNumbers(this,event);" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtTax" runat="server"
                        onkeypress="return onlyDotsAndNumbers(this,event);" Visible="false"></asp:TextBox>
                    <dx:ASPxComboBox ID="ddlAccountHead" runat="server" Visible="false">
                    </dx:ASPxComboBox>
                    <asp:Label ID="Label1" Visible="false" runat="server" Text="Received Date" CssClass="lblbox"></asp:Label>
                    <asp:TextBox ID="txtReceivedDate" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>

                    <asp:TextBox ID="txtBankName" runat="server" Width="192px" CssClass="txtBox" Visible="false"></asp:TextBox>
                    <asp:Label runat="server" ID="lblAmount" Visible="false"></asp:Label>

                    <cc1:CalendarExtender ID="CEStartDate" OnClientShown="calendarShown" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibTransaction"
                        TargetControlID="txtTransactionDate"></cc1:CalendarExtender>
                    <asp:HiddenField ID="HFChqueProcessId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfChequeNo" Value="" runat="server"></asp:HiddenField>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <hr />
            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-2">
                                            <strong>
                                                <label id="Label110"><span class="fa fa-caret-right rgt_cart"></span>Search Type</label></strong>
                                        </div>
                                        <div class="col-md-4">
                                            <dx:ASPxComboBox ID="ddSearchType" runat="server" CssClass="form-control" SelectedIndex="0">
                                                <Items>
                                                    <dx:ListEditItem Value="CHEQUE_NO" Text="All Records"></dx:ListEditItem>
                                                    <dx:ListEditItem Value="VENDOR_NAME" Text="Supplier"></dx:ListEditItem>
                                                    <dx:ListEditItem Value="account_name" Text="Bank Account"></dx:ListEditItem>
                                                </Items>
                                            </dx:ASPxComboBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtSeach" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn box-primary" Text="Filter"
                                                OnClick="btnFilter_Click"></asp:Button>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="Panel12" runat="server" Width="100%" Height="200px" ScrollBars="Vertical"
                                            BorderWidth="1px" BorderStyle="Solid" BorderColor="Silver">

                                            <asp:GridView ID="GrdCheque" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AutoGenerateColumns="False" OnRowEditing="GrdCheque_RowEditing"
                                                OnRowDeleting="GrdCheque_RowDeleting">
                                                <Columns>
                                                    <asp:BoundField DataField="CHEQUE_PROCESS_ID" HeaderText="CHEQUE_PROCESS_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VENDOR_ID" HeaderText="VENDOR_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VENDOR_NAME" HeaderText="Supplier" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="account_name" HeaderText="Bank Account" ReadOnly="true">
                                                        <ControlStyle />
                                                        <ItemStyle />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_NO" HeaderText="Cheque No" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_DATE" HeaderText="Chq.Date" DataFormatString="{0:dd-MMM-yyyy}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RECEIVED_DATE" HeaderText="Paid Date" DataFormatString="{0:dd-MMM-yyyy}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_AMOUNT" DataFormatString="{0:F2}" HeaderText="Amount" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="account_head_id" HeaderText="account_head_id" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DISCOUNT" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DISCOUNT_TYPE" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TAX" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TAX_ACCOUNT_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VoucherAndVendor" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="btnEdit" CommandName="Edit" Text="Edit" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head" />
                                            </asp:GridView>
                                            <asp:GridView ID="GrdCO" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AutoGenerateColumns="False"
                                                Visible="false" OnRowDeleting="GrdCO_RowDeleting">
                                                <Columns>
                                                    <asp:BoundField DataField="VENDOR_ID" HeaderText="VENDOR_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VENDOR_NAME" HeaderText="Supplier" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_NO" HeaderText="Inv.No" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="account_name" HeaderText="Bank Account" ReadOnly="true">
                                                        <ControlStyle />
                                                        <ItemStyle />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_DATE" HeaderText="Transfer Date" DataFormatString="{0:dd-MMM-yyyy}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RECEIVED_DATE" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CHEQUE_AMOUNT" DataFormatString="{0:N}" HeaderText="Amount" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="account_head_id" HeaderText="account_head_id" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDelete" runat="server" class="fa fa-trash-o" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this?');return false;"
                                                                ToolTip="Delete">            
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head" />
                                            </asp:GridView>

                                        </asp:Panel>
                                    </div>
                                </div>
                                <table width="90%">
                                    <tr>
                                        <td style="text-align: right;">
                                            <strong>Total Amount: </strong>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
