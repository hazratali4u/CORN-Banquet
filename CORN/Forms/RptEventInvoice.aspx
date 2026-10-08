<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptEventInvoice.aspx.cs" Inherits="Forms_RptEventInvoice" Title="Event Invoice" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="javascript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents" style="min-height:300px;">
        <div class="container employee-infomation">
            <div class="row">
                <div class="col-md-8">
                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-sm-3 col-xs-4" style="width: 340px">
                    <label for=""><span class="fa fa-caret-right rgt_cart"></span>Project Code</label><br />
                    <dx:ASPxComboBox ID="DrpVoucher" runat="server"
                        CssClass="form-control">
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="btn btn-success" OnClick="btnViewPDF_Click" Text="View PDF" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" OnClick="btnViewExcel_Click" Text="View Excel" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
