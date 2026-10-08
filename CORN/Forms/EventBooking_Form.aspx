<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="EventBooking_Form.aspx.cs" Inherits="Forms_EventBooking_Form" Title="Event Booking Form" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <%--<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>--%>
    <script src="../js/angular.min.js" type="text/javascript"></script>

    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
       
        
    </script>
    <script type="text/javascript">

        function OnChangeHandler() {
            var mydate = new Date(document.getElementById('<%=Event_type_date.ClientID%>').value);
            var str = mydate.getDate() + '' + (mydate.getMonth() + 1) + '' + mydate.getFullYear();
            ntr = document.getElementById('<%=Name.ClientID%>').value;
            ltr = document.getElementById('<%=Location.ClientID%>').value;
            Alr = str +"-"+ ntr +"-"+ ltr;
            if (str&&ntr&&ltr !="") {
                document.getElementById('<%=voucher_no.ClientID%>').value = Alr;
            }
            var mydate2 = new Date(document.getElementById('<%=txtBookingDate.ClientID%>').value);
            var edr = new Date(mydate);
            var bdr = new Date(mydate2);
            if (edr<=bdr) {
                alert("Event Date Should Be After Booking Date");
                document.getElementById('<%=Event_type_date.ClientID%>').value = null;
                return false;
            }
        }

        function ValidateForm() {
            str = document.getElementById('<%=Event_type_date.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Enter Event Date')
                return false;
            }
       
            str = document.getElementById('<%=Name.ClientID%>').value;
             if (str == null || str.length == 0) {
                 alert('Client Name is required')
                 return false;
            }
            str = document.getElementById('<%=Contact_no.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Contact# is required')
                return false;
            }
            str = document.getElementById('<%=CNIC_NTN_no.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('CNIC or NTN is required')
                return false;
            }
            str = document.getElementById('<%=Location.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Location is required')
                return false;
            }
            str = document.getElementById('<%=Eventcost_entry.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Event Cost is required')
                return false;
            }
            str = document.getElementById('<%=Address.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Address is required')
                return false;
            }
            str = document.getElementById('<%=Payment_plan.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Payment Plan is required')
                return false;
            }
            str = document.getElementById('<%=voucher_no.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Project Code is required')
                return false;
            }
             return true;
         }
    </script>

    <style type="text/css">
        .mt-5 {
            margin-top: 50px;
        }

        .border {
            border: 1px solid #999999;
            padding: 30px;
        }

        .p-4 {
            margin-top: 20px;
        }

        label {
            color: #6c6c6c;
            font-weight: normal;
        }
    </style>

    <div class="container">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="col-md-12">
                    
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
                    <div class="">
                        <div class="btnlist pull-right">
                               <asp:LinkButton CssClass="btn btn-warning" OnClick="btnOpenPopUp_Click" runat="server" ID="btnOpenPopUp" Text="Add">
                            <span class="fa fa-plus-circle"></span>Add
                            </asp:LinkButton>
                            </div>
                            <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                            <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                         </div>
                        </div>
                            <!-- The modal popup -->
                            <asp:Panel runat="server" Style="display: none; width: 100%" DefaultButton="btnSave" ScrollBars="Auto" ID="divPopUp">
                              
                                <div class="modal-dialog modal-lg"role="document">
                                    <div class="modal-content ">
                                        <div class="modal-header">
                                            <button type="button" class="close" runat="server" onserverclick="btnClose_Click" >
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                            <h4 class="modal-title" id="modalLabel">Add Event Booking Form</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="">
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <div class="form-group">
                                                            <div class="row">
                                                                <div class="col-md-10 col-sm-10 col-xs-10">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Booking Date</label>
                                                                    <asp:TextBox ID="txtBookingDate" runat="server"  CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-1 col-sm-1 col-xs-1" style="margin-top:27px;margin-left:10px">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" />
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ImageButton1"
                                                                        TargetControlID="txtBookingDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                         <div class="form-group">
                                                            <div class="row">
                                                                <div class="col-md-10 col-sm-10 col-xs-10">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Event Date</label>
                                                                    <asp:TextBox ID="Event_type_date" runat="server" onChange="OnChangeHandler()" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-1 col-sm-1 col-xs-1" style="margin-top: 27px;margin-left:10px">
                                                                    <asp:ImageButton ID="ibtnEvent_type_date" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" />
                                                                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnEvent_type_date"
                                                                        TargetControlID="Event_type_date" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" >
                                                    <div class="col-md-6 col-sm-6 col-xs-6"style="padding:0px">
                                                        <div class="col-md-12 col-sm-12 col-xs-12" style="padding:0px">
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                                  <div class="form-group">
                                                            <label for="Name"><span class="fa fa-caret-right rgt_cart"></span>Client Name</label><br />
                                                            <asp:TextBox ID="Name" runat="server" CssClass="form-control" onkeyup="OnChangeHandler()"></asp:TextBox>
                                                        </div>
                                                            </div>
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                                <div class="form-group">
                                                            <label for="Contact_no"><span class="fa fa-caret-right rgt_cart"></span>Client Contact</label><br />
                                                            <asp:TextBox ID="Contact_no" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-sm-12 col-xs-12" style="padding:0px">
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                                 <div class="form-group">
                                                            <label for="CNIC_NTN_no"><span class="fa fa-caret-right rgt_cart"></span>Client CNIC/NTN</label><br />
                                                            <asp:TextBox ID="CNIC_NTN_no" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                            </div>
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                         <div class="form-group">
                                                            <label for="Location"><span class="fa fa-caret-right rgt_cart"></span>Event Location</label><br />
                                                            <asp:TextBox ID="Location" runat="server" CssClass="form-control" onkeyup   ="OnChangeHandler()"></asp:TextBox>
                                                            </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6 col-sm-6 col-xs-6" style="padding:0px">
                                                        <div class="col-md-12 col-sm-12 col-xs-12" style="padding:0px">
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                            <div class="form-group">
                                                            <label for="Eventcost_entry"><span class="fa fa-caret-right rgt_cart"></span>Event Cost</label><br />
                                                            <asp:TextBox ID="Eventcost_entry" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                         </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6"> 
                                                       <div class="form-group">
                                                        <label for="Event_type"><span class="fa fa-caret-right rgt_cart"></span>Event Type</label><br />
                                                            <dx:ASPxComboBox ID="DrpEventType" runat="server" SelectedIndex="0"
                                                                CssClass="form-control">
                                                        </dx:ASPxComboBox>
                                                        </div>
                                                             </div>
                                                        </div>

                                                        <div class="col-md-12 col-sm-12 col-xs-12" style="padding:0px">
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <div class="form-group">
                                                            <label for="Payment_plan"><span class="fa fa-caret-right rgt_cart"></span>Payment Plan</label><br />
                                                            <asp:TextBox ID="Payment_plan" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                                </div>
                                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                       <div class="form-group">
                                                        <label for="fname"><span class="fa fa-caret-right rgt_cart"></span>Project Code</label><br />
                                                        <asp:TextBox ID="voucher_no" runat="server" CssClass="form-control"></asp:TextBox>
                                                       </div>
                                                                </div>
                                                          </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-xs-12">
                                                          <div class="form-group">
                                                            <label for="lname"><span class="fa fa-caret-right rgt_cart"></span>Address</label><br />
                                                            <asp:TextBox ID="Address" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="row">
                                                <div class="col-md-11 pull-right">
                                                    <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                    <asp:HiddenField ID="hfID" runat="server" />
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Style="margin-left: 5px" Text="Clear" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>



    <%--TableGrid--%>
    <div class="row center" style="margin:auto">
        <div class="col-md-12">
            <div class="emp-table">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdEventBooking_form" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            HorizontalAlign="Center"
                            AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="8" OnPageIndexChanging="grdEventBookingChanging" EmptyDataText="No Record exist">
                            <Columns>
                                
                                <asp:BoundField DataField="Booking_date" HeaderText="Booking Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="EventType_date_venue" HeaderText="Event Date"  ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}" >
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Client Name" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="Contact_no_client" HeaderText="Contact Number" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CNIC_NTN_no_client" HeaderText="Client CNIC/NTN#" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="Location" HeaderText="Event Location" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="EventName" HeaderText="Event Type" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                               
                                <asp:BoundField DataField="Voucher_no" HeaderText="Project Code" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Payment_plan" HeaderText="Payment Plan" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                             
                                <asp:BoundField DataField="Eventwise_cost_entry" HeaderText="Event Cost" ReadOnly="true">
                                    <HeaderStyle CssClass=""></HeaderStyle>
                                    <ItemStyle CssClass=""></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Address_client" HeaderText="Address" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Id" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandArgument='<%#Eval("Id")%>' OnClick="btnEdit_Click" ToolTip="Edit">
                                        </asp:LinkButton>  |
                                        <asp:LinkButton ID="del" runat="server" CssClass="fa fa-trash-o" CommandArgument='<%#Eval("Id")%>' OnClick="del_Click" ToolTip="Delete">
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
