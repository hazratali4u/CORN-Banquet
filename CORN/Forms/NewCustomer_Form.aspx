<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="NewCustomer_Form.aspx.cs" Inherits="Forms_NewCustomer_Form" Title="CORN ::New Customer Form" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

    </script>
 <style>

   .border{
    border: 1px solid #999999;
    padding: 30px;
   }
   
   .p-4{
    margin-top:20px;
   }
   label{
       color:#6c6c6c;
       font-weight:normal;
   }
    </style>
 

  <div class="container">
      
      <div class="row">
       <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-4">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                TabIndex="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2" style="margin-left: -60px;">

                         <%--OnClick="btnFilter_Click"--%>

                        <asp:LinkButton ID="btnsearch"  runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                    <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
                </asp:Panel>
                <div class="col-md-offset-5 col-md-3 ">
                   <div class="btnlist pull-right">

            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
                <%--OnClick="btnAdd_Click"--%>

                    <button class="btn btn-warning"  ID="btnAdd" Text="Add"
                       data-toggle="modal" data-target="#flipFlop">
                       <span class="fa fa-plus-circle"></span>Add
                     </button>

                <%--OnClientClick="javasacript:return confirm('Are your sure you want to perform this action?'); return false;"--%>    
                            
                      <a class="btn btn-success"  ID="btnActive"
                                    >
                    <%--OnClick="btnActive_Click"--%>

                     <span class="fa fa-check"></span>Active</a>
                         
            <%--</asp:UpdatePanel>--%>
               </div>
           </div>
       </div>
      <!-- The modal -->
            <div class="modal"  style="margin-top:120px" id="flipFlop" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg" style="width: 70%;margin-left:320px" role="document">
            <div class="modal-content ">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
            </button>
            <h4 class="modal-title" id="modalLabel">Add New Customer Form</h4>
            </div>
                <asp:UpdatePanel id="UpdatePanel" runat="server">
                    <ContentTemplate>

            <div class="modal-body">
            
                
                      <div class="row">
                        <div class="col-md-6">
                          <div class="form-group">
                            <label for="customerName"><span class="fa fa-caret-right rgt_cart"></span>Customer Name</label>
                            <asp:TextBox ID="customerName" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                          </div>
                            </div>
                          <div class="col-md-6">
                            <div class="form-group">
                            <label for="Cnic"> <span class="fa fa-caret-right rgt_cart"></span> CNIC/NTN #</label><br />
                           <asp:TextBox ID="Cnic" runat="server" CssClass="form-control"></asp:TextBox>
                           </div>
                        </div>
                       </div>

                      <div class="row">
                        <div class="col-md-6">
                          <div class="form-group">
                            <label for="CNIC_NTN_no" class="p-4"> <span class="fa fa-caret-right rgt_cart"></span> Contact#</label><br />
                           <asp:TextBox ID="CNIC_NTN_no" runat="server" CssClass="form-control"></asp:TextBox>
                           </div>
                            </div>
                         <div class="col-md-6">  
                          <div class="form-group">
                            <label for="address_no" class="p-4"> <span class="fa fa-caret-right rgt_cart"></span> Address</label><br />
                           <asp:TextBox ID="address_no" runat="server" CssClass="form-control"></asp:TextBox>
                           </div>
                      </div>
                    </div>
          
            <div class="modal-footer">
                     <div class="row">
                <div class="col-md-11" style="text-align: right; margin-top: 20px; margin-right: 40px">
                    <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                    
                    <asp:Button ID="btnSave"  runat="server" Text="Save" CssClass="btn btn-success" Onclick="btnSave_Click" />
                     <%--OnClick="btnCancel_Click"--%>
                    <asp:Button ID="btnCancel" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                </div>
            </div>
            </div>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            </div>
            </div>



      <!--Grid Table-->
      <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <%--OnPageIndexChanging="grdEventBooking_form_PageIndexChanging" OnRowEditing="grdEventBooking_form_RowEditing"--%>
                                <asp:GridView ID="grdNewCustomer_Form" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    HorizontalAlign="Center"
                                    
                                    AutoGenerateColumns="False" 
                                    AllowPaging="true" PageSize="20" EmptyDataText="No Record exist">

                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle />
                                            
                                             <%--onclick="checkAll(this);"--%>

                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server"  />
                                            </HeaderTemplate>
                                            
                                            <%--onclick="Check_Click(this)"--%>

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server"  />
                                                <asp:HiddenField ID="hidSKUImageName" runat="server" Value='<%# Eval("SKU_IMAGE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Booking_date" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EventType_date_venue" HeaderText="EventType_date_venue" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Voucher_no" HeaderText="Voucher_no" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Eventwise_cost_entry" HeaderText="Eventwise_cost_entry" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Payment_plan" HeaderText="Payment_plan" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                           <asp:BoundField DataField="CNIC_NTN_no_client" HeaderText="CNIC_NTN_no_client" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                           <asp:BoundField DataField="Contact_no_client" HeaderText="Contact_no_client" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                            <asp:BoundField DataField="Address_client" HeaderText="Address_client" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>



                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
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


    </div>
    </asp:Content>