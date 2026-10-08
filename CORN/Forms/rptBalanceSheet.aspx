<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptBalanceSheet.aspx.cs" Inherits="Forms_rptBalanceSheet" Title="CORN :: Balance Sheet" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="javascript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row">
                <div class="col-md-8">
                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <label><span class="fa fa-caret-right rgt_cart"></span>Level</label>
                    <dx:ASPxComboBox ID="DrpLevel" runat="server" CssClass="form-control">
                        
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <label><span class="fa fa-caret-right rgt_cart"></span>As On Date</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" MaxLength="10" onkeyup="BlockEndDateKeyPress()"></asp:TextBox>
                </div>
                <div class="col-md-1" style="margin-top: 27px">
                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                        Width="30px" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="btn btn-success" OnClick="btnViewPDF_Click" Text="View PDF" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" OnClick="btnViewExcel_Click" Text="View Excel" />
                </div>
            </div>
            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                TargetControlID="txtEndDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
        </div>
    </div>
</asp:Content>
