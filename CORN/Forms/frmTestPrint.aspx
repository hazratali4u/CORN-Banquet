<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmTestPrint.aspx.cs" Inherits="Forms_frmTestPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadPage" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">

    <asp:Button ID="btnLoadData" runat="server" Text="Load Data"  OnClick="btnLoadData_Click" />
    <asp:Button ID="btnClearnCache" runat="server" Text="Clear Cache"  OnClick="btnClearnCache_Click" />
    <br />
    <br />
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <br />
    <br />
    <asp:GridView ID="gvProducts" runat="server">
    </asp:GridView>

</asp:Content>

