<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="VerificationFinalBudget_Form.aspx.cs" Inherits="Forms_VerificationFinalBudget_Form" Title="Verification Final Budget" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script type="text/javascript">
        function Calculation() {
            var grid = document.getElementById("<%= grdVerificationFinal_budget_form.ClientID%>");
            
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var QUANTITY = $("input[id*=QUANTITY]")
                var Actual_Quantity = $("input[id*=Actual_Quantity]")
                var Diff_Id = $("input[id*=Diff_Id]")
                var balance = 1;
                //if (parseInt(CheckComma(empty(QUANTITY[i].value))) < parseInt(CheckComma(empty(Actual_Quantity[i].value)))) {
                //    Diff_Id[i].value = " ";
                //    alert('Actual Quantity should be less than Budget Quantity');
                //    return false;
                //}
                //else {
                    balance = parseInt(CheckComma(empty(QUANTITY[i].value))) - parseInt(CheckComma(empty(Actual_Quantity[i].value)));
                    /*balance = parseInt(CheckComma(empty(QUANTITY[i].value))) - parseInt(CheckComma(empty(Actual_Quantity[i].value)));*/
                //}
                Diff_Id[i].value = numberWithCommas(balance);
            }
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
    </script>
    <style>
        .form-control{
            margin:0px !important;  
        }
        
        .pd{
            white-space:nowrap;
        }
        
        .mb{
            margin-bottom:10px !important;
        }
    </style>

    <div class="container" style="min-height:300px;">
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="mb row">
                    <div class="col-md-5 col-sm-5 col-xs-4" style="font-size:17px">
                        <label for=""><span class="fa fa-caret-right rgt_cart"></span>Project Code</label><br />
                        <dx:ASPxComboBox ID="DrpVoucher" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DrpVoucher_SelectedIndexChanged"
                            CssClass="form-control">
                        </dx:ASPxComboBox>
                    </div>
                     <div class="col-md-1 col-sm-3 col-xs-4 pull-right" style="margin-top:30px">
                           <asp:Button ID="btnPost" runat="server" Text="Post" CssClass="btn btn-warning"/>
                    </div>
                    <div class="col-md-1 col-sm-3 col-xs-4 pull-right" style="margin-top:30px">
                           <asp:Button ID="btnSave" runat="server" Text="Save Draft" CssClass="btn btn-warning" OnClick="btnSave_Click" />
                    </div>
                </div>
                 
                <div class="row ">
                    <div class="col-md-12">
                        <div class="emp-table" style="padding:0px;margin:0px">
                            <asp:GridView ID="grdVerificationFinal_budget_form" runat="server" CssClass="table table-striped table-bordered table-hover pd table-condensed cf"
                                HorizontalAlign="Center"
                                AutoGenerateColumns="False" Height="1px" style="white-space:nowrap"
                                AllowPaging="true" PageSize="40" OnPageIndexChanging="grdVerificationChanging" EmptyDataText="No Record exist">
                                <Columns>
                                     <asp:BoundField DataField="FINAL_BUDGET_ID" HeaderText="FINAL_BUDGET_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                       <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CATEGORY_ID" HeaderText="Category_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENDOR_ID" HeaderText="VENDOR_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name" ReadOnly="true">
                                        <HeaderStyle CssClass=""></HeaderStyle>
                                        <ItemStyle CssClass="" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Size_Description" HeaderText="Size & Description" ReadOnly="true">
                                        <HeaderStyle CssClass=""></HeaderStyle>
                                        <ItemStyle CssClass="" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENDOR_NAME" HeaderText="Supplier Name" ReadOnly="true">
                                        <HeaderStyle CssClass=""></HeaderStyle>
                                        <ItemStyle CssClass="" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                      <asp:TemplateField HeaderText="Budget QTY">
                                        <ItemTemplate>
                                            <asp:TextBox ID="QUANTITY" CssClass="form-control" runat="server" Text='<%# Eval("QUANTITY") %>' Enabled="false" placeholder="Actual QTY"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px"/>
                                    </asp:TemplateField >
                                    <asp:TemplateField HeaderText="Actual QTY">
                                        <ItemTemplate>
                                    <asp:TextBox ID="Actual_Quantity" CssClass="form-control" runat="server" Text='<%# Eval("Actual_Quantity") %>' placeholder="Actual QTY" onkeyup="Calculation(this.value)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px"/>
                                    </asp:TemplateField >

                                    <asp:TemplateField HeaderText="Difference" >
                                        <ItemTemplate>
                                     <asp:TextBox ID="Diff_Id" CssClass="form-control" Enabled="False" runat="server" Text='<%# Eval("Diff_Id") %>' placeholder="Difference"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RATE" HeaderText="RATE" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel" Width="100px" ></ItemStyle>
                                    </asp:BoundField>
                                <%-- <asp:templatefield headertext="edit">
                                    <itemtemplate>
                                        <asp:linkbutton id="btnedit" runat="server" cssclass="fa fa-pencil" onclick="btnedit_click" tooltip="edit">
                                        </asp:linkbutton>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="center" width="5%" />
                                </asp:templatefield>--%>
                                </Columns>
                                <PagerSettings PageButtonCount="10" />
                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
               
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
