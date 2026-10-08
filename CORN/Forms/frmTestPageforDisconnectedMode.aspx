<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmTestPageforDisconnectedMode.aspx.cs" Inherits="Forms_frmTestPageforDisconnectedMode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadPage" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <div style="font-family: Arial">

        <asp:Button ID="btnGetDataFromDB" runat="server" Text="Get Data from Database" OnClick="btnGetDataFromDB_Click" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ID" OnRowEditing="GridView1_RowEditing"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowDeleting="GridView1_RowDeleting"
            OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False"
                    ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Gender" HeaderText="Gender"
                    SortExpression="Gender" />
                <asp:BoundField DataField="TotalMarks" HeaderText="TotalMarks"
                    SortExpression="TotalMarks" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnUpdateDatabaseTable" runat="server"
            Text="Update Database Table" OnClick="btnUpdateDatabaseTable_Click" />
        <asp:Label ID="lblStatus" runat="server"></asp:Label>

        <hr />
        <br />

       <%-- <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button"  onclick="Button1_Click" />
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>--%>


    </div>


</asp:Content>

