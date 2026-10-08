<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="AddFinal_Budjet.aspx.cs" Inherits="Forms_AddFinal_Budjet" Title="Add Final Budget" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OnchangeHandler() {
            qty = document.getElementById('<%=Qty.ClientID%>').value;
            rte = document.getElementById('<%=Rate.ClientID%>').value;
            total = qty * rte;
            /*toLocaleString()*/
            if (total != "") {
                document.getElementById('<%=Internal_cost.ClientID%>').value = total.toFixed(2);
            }
            mrt = document.getElementById('<%=Margin.ClientID%>').value;
            divide = total * mrt / 100 + total;
            if (divide != "") {
                document.getElementById('<%=External_Code.ClientID%>').value = divide.toFixed(2);
            }
            str = document.getElementById('<%=Rate.ClientID%>').value;
            mtr = document.getElementById('<%=Margin.ClientID%>').value;
            var letters = /^[A-Za-z]+$/;
            if (str.match(letters)) {
                alert("No Character Allowed")
                document.getElementById('<%=Rate.ClientID%>').value = "";
            }
        }

        function ValidateForm() {
            str = document.getElementById('<%=Size_Description.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Size is required')
                return false;
            }
            str = document.getElementById('<%=Qty.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Qty is required')
                return false;
            }
            str = document.getElementById('<%=Rate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Rate is required')
                return false;
            }
            str = document.getElementById('<%=Margin.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Margin is required')
                return false;
            }
            str = document.getElementById('<%=Internal_cost.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Internal Cost is required')
                return false;
            }
            str = document.getElementById('<%=External_Code.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('External Cost is required')
                return false;
            }
            return true;
        }

    </script>
    <style type="text/css">
        .page {
            border-collapse: collapse;
        }

        #page {
            border-collapse: collapse;
        }
        /* Add this to your table's `td` elements. */
        .page td {
            padding: 0;
            margin: 0;
        }

        #page td {
            padding: 0;
            margin: 0;
        }

        .form-control {
            margin-bottom: 0 !important;
        }

        th {
            white-space: nowrap;
        }

        select option:checked,
        select option:hover {
            background-color: #808080 !important;
            color: antiquewhite !important;
            padding: 2px !important;
        }

        .mtb {
            margin-bottom: 30px;
        }

        .w-5 {
            width: 29%;
        }
    </style>
    <%--Voucher and Category--%>
    <div class="container mtb">
        <div class="row">
            <%--CategorySelect--%>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="col-md-8" style="margin: 0; padding: 0">
                        <div class="col-md-4 col-sm-3 col-xs-4" style="width: 340px">
                            <label for=""><span class="fa fa-caret-right rgt_cart"></span>Project Code</label><br />
                            <dx:ASPxComboBox ID="DrpVoucher" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DrpVoucher_SelectedIndexChanged"
                                CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4" style="display:none;">
                            <label for=""><span class="fa fa-caret-right rgt_cart"></span>Category</label><br />
                            <dx:ASPxComboBox ID="DrpCategory" runat="server" TextField="SKU_HIE_NAME" OnSelectedIndexChanged="DrpCategory_SelectedIndexChanged" AutoPostBack="true" SelectedIndex="0"
                                CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--inputfields--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="page" class="table table-bordered cf">
                <thead>
                    <tr>
                        <th>Items</th>
                        <th>Size & Description</th>
                        <th>Supplier Name</th>
                        <th>Quantity</th>
                        <th>Rate</th>
                        <th>Internal Cost</th>
                        <th>Margin(%)</th>
                        <th>External Cost</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width:20%">
                            <%--AutoPostBack="True"--%>
                            <dx:ASPxComboBox ID="DrpItemType" runat="server" SelectedIndex="0"
                                CssClass="form-control" TextField="SKU_NAME" OnSelectedIndexChanged="DrpItemType_SelectedIndexChanged" AutoPostBack="true">
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width:15%">
                            <asp:TextBox ID="Size_Description" runat="server" autocomplete="off" CssClass="form-control" placeholder="Size and Description"></asp:TextBox>
                        </td>
                        <td style="width:17%">
                            <dx:ASPxComboBox ID="DrpVendorName" runat="server" CssClass="form-control"></dx:ASPxComboBox>
                        </td>
                        <td style="width:7%">
                            <asp:TextBox ID="Qty" runat="server" onkeyup="OnchangeHandler()" CssClass="form-control" autocomplete="off" type="number" placeholder="Quantity"></asp:TextBox>
                        </td>
                        <td style="width:5%">
                            <asp:TextBox ID="Rate" runat="server" onkeyup="OnchangeHandler()" CssClass="form-control" autocomplete="off" placeholder="Rate"></asp:TextBox>
                        </td>
                        <td style="width:5%">
                            <asp:TextBox ID="Internal_cost" runat="server" CssClass="form-control" autocomplete="off" placeholder="Internal Cost"></asp:TextBox>
                        </td>
                        <td style="width:5%">
                            <asp:TextBox ID="Margin" runat="server" onkeyup="OnchangeHandler()" CssClass="form-control" autocomplete="off" placeholder="Margin"></asp:TextBox>
                        </td>
                        <td style="width:5%">
                            <asp:TextBox ID="External_Code" runat="server" CssClass="form-control" autocomplete="off" placeholder="External Code"></asp:TextBox>
                        </td>
                        <td style="width:12%">
                            <div class="pull-right" style="width:100%">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" Style="width:98%" CssClass="btn btn-success" OnClick="btnAdd_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            
            <div class="row" style="display: flex; justify-content: end; margin-top: 30px; margin-bottom: 20px">
                <asp:Panel ID="Panel3" Width="100%" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-6">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                </asp:Panel>
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="pull-right" style="width:130px;">
                        <asp:Button ID="btnSave" runat="server" Width="125px" Text="Save" CssClass="btn btn-warning" OnClick="btnSave_Click" />
                    </div>
                </div>
                <%--<div class="col-md-offset-5 col-md-3">
                    <asp:HiddenField ID="hfSKU_ID" runat="server" />
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



    <%--NewTable--%>
    <div class="row center ">
        <div class="col-md-12">
            <div class="emp-table">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <%--Add footer in it --%>
                         <%--ShowFooter="true"--%>
                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            HorizontalAlign="Center"
                            AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdFinalDataChanging" EmptyDataText="no record exist">
                            <Columns>
                                <asp:BoundField Visible="false" DataField="Voucher_Number" HeaderText="Project Code" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category" ReadOnly="true">
                                   <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Size_Description" HeaderText="Size & Description" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor Name" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="Rate" HeaderText="Rate" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="INTERNAL_COST" HeaderText="Internal Cost" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                             <%--   <asp:TemplateField HeaderText="Action">
                                    <FooterTemplate>
                                        <asp:Label ID="lbltotal" runat="server" Text="Label"></asp:Label>
                                        <asp:TextBox ID="txtInternal" CssClass="form-control" Enabled="False" runat="server" Text='Internal' placeholder="Difference"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="Margin" HeaderText="Margin(%)" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="External_Cost" HeaderText="External Cost" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>

                                 <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                   <dx:ASPxComboBox ID="DrpStatusType" runat="server" AutoPostBack="true"
                                CssClass="form-control" OnSelectedIndexChanged="DrpStatusType_SelectIndexChange">
                                <Items>
                                    <dx:ListEditItem Value="0" Text="Order Pending"></dx:ListEditItem>
                                    <dx:ListEditItem Value="1" Text="Order Placed"></dx:ListEditItem>
                                </Items>
                            </dx:ASPxComboBox>
                                        </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="VOUCHER_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="CATEGORY_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="VENDOR_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Item_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="STATUS" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="FINAL_BUDGET_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditC" runat="server" CssClass="fa fa-pencil" OnClick="btnEditC_Click">
                                        </asp:LinkButton>
                                        |
                                        <asp:LinkButton ID="del" runat="server" CssClass="fa fa-trash-o" CommandArgument='<%#Eval("VOUCHER_ID")%>' OnClick="del_Click">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                       
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerSettings PageButtonCount="10" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                        </asp:GridView>
                        <%--  <div class="row" style="margin-top:100px">
                            <div class="col-md-1 col-md-offset-8" style="margin-right:50px">
                                <asp:Label ID="txtInternalLabel" runat="server" Text="Internal"></asp:Label>
                                <asp:TextBox ID="txtInternal" CssClass="form-control" Enabled="False" runat="server" Text='Internal' placeholder="Difference"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="">
                                <asp:Label ID="txtExternalLabel" runat="server" Text="External"></asp:Label>
                                <asp:TextBox ID="txtExternal" CssClass="form-control" Enabled="False" runat="server" Text='External' placeholder="Difference"></asp:TextBox>
                            </div>
                        </div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="panel13" runat="server">
        <ContentTemplate>
            <div class="row" style="display: flex; justify-content: start; margin-top: 5px; margin-bottom: 50px">
                <div class="col-md-3">
                    <asp:Button ID="Total" runat="server" Text="Total" CssClass="btn btn-danger HidePanel" OnClick="Total_Click" />
                    <asp:Button ID="LookUp" class="btn btn-success HidePanel" runat="server" OnClick="LookUp_Click" Text="Show All"
                        PostBackUrl="~/forms/lookupfinalbudgetform.aspx?LevelType=3&LevelID=312" />
                    <%--~/forms/lookupfinalbudgetform.aspx?LevelType=3&LevelID=320  CssClass="HidePanel"--%>
                </div>
          
                <div class="col-md-1 HidePanel col-sm-2 col-md-offset-6" style="margin-right: 10px">
                    <asp:TextBox ID="txtInternal" CssClass="form-control" Enabled="False" runat="server" Text='Internal' placeholder="Difference"></asp:TextBox>
                </div>
                <div class="col-md-1 col-sm-2 HidePanel" style="">
                    <asp:TextBox ID="txtExternal" CssClass="form-control" Enabled="False" runat="server" Text='External' placeholder="Difference"></asp:TextBox>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
