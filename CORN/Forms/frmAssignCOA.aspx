<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmAssignCOA.aspx.cs" Inherits="Forms_frmAssignCOA" Title="Account Head Assignment" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
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
                            <label><span class="fa fa-caret-right rgt_cart"></span>Report Name</label>
                            <dx:ASPxComboBox ID="ddlReport" runat="server" SelectedIndex="0"
                                CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
                                <Items>
                                    <dx:ListEditItem Text="Balance Sheet" Selected="true" Value="1"></dx:ListEditItem>
                                    <dx:ListEditItem Text="Profit & Loss Statement" Value="2"></dx:ListEditItem>
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Account Category</label>
                            <dx:ASPxComboBox ID="DrpAccountCategory" runat="server" AutoPostBack="true" SelectedIndex="0"
                                CssClass="form-control" OnSelectedIndexChanged="DrpAccountCategory_SelectedIndexChanged">
                                <Items>
                                    <dx:ListEditItem Text="Balance Sheet Account" Selected="true" Value="0"></dx:ListEditItem>
                                    <dx:ListEditItem Text="Income Statment Account" Value="1"></dx:ListEditItem>
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class=" row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Account Type</label>
                            <dx:ASPxComboBox ID="DrpMainType" runat="server" AutoPostBack="true"
                                CssClass="form-control" OnSelectedIndexChanged="drpPrincipal_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class=" row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Account Sub Type</label>
                            <dx:ASPxComboBox ID="DrpSubType" runat="server" CssClass="form-control"
                                AutoPostBack="True" OnSelectedIndexChanged="DrpSubType_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class=" row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Account Detail Type</label>
                            <dx:ASPxComboBox ID="DrpDetailType" runat="server" CssClass="form-control"
                                AutoPostBack="True" OnSelectedIndexChanged="DrpDetailType_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row bottom1">
                            <label>
                                <span class="fa fa-caret-right rgt_cart"></span>Account Head Assignment</label>
                        </div>
                        <div class="row bottom">
                            <div class="module_contents">
                                <div class="col-md-5">
                                    <asp:ListBox ID="lstUnAssignModule" runat="server" CssClass="select" TabIndex="5" ></asp:ListBox>
                                </div>
                                <div class="col-md-2">
                                    <div class="navs navslist" style="padding-top:60px; padding-left:50px">
                                        <div class="nxt arrow">
                                            <a href="#"><span>
                                                <asp:LinkButton ID="btnAssign" runat="server" OnClick="btnAssign_Click" CssClass="fa fa-angle-right" TabIndex="6"/>
                                            </span></a>
                                        </div>
                                        <div class="nxt arrow">
                                            <span>
                                                <asp:LinkButton ID="btnAssignAllItem" OnClick="btnAssignAllItem_Click"
                                                    CssClass="fa  fa-angle-double-right" runat="server" /></span>
                                        </div>
                                        <div class="nxt arrow">
                                            <span>
                                                <asp:LinkButton ID="btnUnAssignAllItem" CssClass="fa  fa-angle-double-left"
                                                    OnClick="btnUnAssignAllItem_Click" runat="server" /></span>
                                        </div>
                                        <div class="nxt arrow">
                                            <a href="#"><span>
                                                <asp:LinkButton ID="btnUnAssign" runat="server" OnClick="btnUnAssign_Click" CssClass="fa fa-angle-left" TabIndex="7"/>
                                            </span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <asp:ListBox ID="lstAssignModule" CssClass="select" runat="server" TabIndex="8"></asp:ListBox>
                                </div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>        
    </div>
</asp:Content>
