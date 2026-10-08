<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="LookUpFInalBudgetForm.aspx.cs" Inherits="Forms_LookUpFInalBudgetForm" Title="LookUp Final Budget" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>


        <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
            <div class="col-md-4 col-sm-4 col-xs-6">
                <div class="search">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                        TabIndex="0"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-2" style="margin-left: -60px;">
                <asp:LinkButton ID="btnsearch" runat="server" Text="Search" OnClick="btnFilter_Click"
                    CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
            </div>
            <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
        </asp:Panel>
                <div class="btnlist pull-right" style="margin-bottom: 5px">
                <asp:LinkButton CssClass="btn btn-warning" OnClick="Unnamed_Click" runat="server" ID="btnAdd" Text="Add">
                               <span class="fa fa-plus-circle"></span>Add New
                 </asp:LinkButton>
                </div>
                  
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <%--TableGrid--%>
    <div class="row center ">
        <div class="col-md-12">
            <div class="emp-table">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdFinal_Budget" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            HorizontalAlign="Center"
                            AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdFinalBudgetChanging" EmptyDataText="No Record exist">
                            <Columns>
                                <asp:BoundField DataField="Voucher_no" HeaderText="Project Code" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Client_Name" HeaderText="Client Name" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Event_Date" HeaderText="Event Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Venue" HeaderText="Venue" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Event_Type" HeaderText="Event Type" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                           <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandArgument='<%#Eval("Id")%>' OnClick="btnEdit_Click">
                                        </asp:LinkButton>
                                         |
                                        <asp:LinkButton ID="del" runat="server" CssClass="fa fa-trash-o" CommandArgument='<%#Eval("Id")%>' OnClick="del_Click" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;" >
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings PageButtonCount="10" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
