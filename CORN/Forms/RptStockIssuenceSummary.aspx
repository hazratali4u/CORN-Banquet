<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptStockIssuenceSummary.aspx.cs" Inherits="Forms_RptStockIssuenceSummary"
    Title="CORN :: Stock Issuence Summary" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="rblRate" runat="server" Width="200px" RepeatDirection="Horizontal" Visible="false">
                                <asp:ListItem Value="0" Selected="True">Trade Price</asp:ListItem>
                                <asp:ListItem Value="1">Purchase Price</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                   <%-- <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Report Type</label>

                            <dx:ASPxComboBox ID="DrpDocumentType" runat="server" CssClass="form-control"
                                SelectedIndex="0" OnSelectedIndexChanged="DrpDocumentType_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <dx:ListEditItem Value="2" Text="Purchase" />
                                    <dx:ListEditItem Value="3" Text="Purchase Return" />
                                    <dx:ListEditItem Value="8" Text="Short" />
                                    <dx:ListEditItem Value="9" Text="Excess" />
                                    <dx:ListEditItem Value="6" Text="Damage" />
                                    <dx:ListEditItem Value="4" Text="Transfer In" />
                                    <dx:ListEditItem Value="5" Text="Transfer Out" />
                                    <dx:ListEditItem Value="19" Text="Stock Issuance" />
                                    <dx:ListEditItem Value="14" Text="Stock Return" />
                                    <dx:ListEditItem Value="15" Text="Physical Stock Taking" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>--%>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                     <div class="row">
                         <asp:Panel ID="PanelSupplier" runat="server">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Supplier</label>
                            <dx:ASPxComboBox ID="DrpPrincipal" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                             </asp:Panel>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>

                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" CssClass=" btn btn-success" Text="View PDF"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" Text="View Excel"
                        OnClick="btnViewExcel_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>