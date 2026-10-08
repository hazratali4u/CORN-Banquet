<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Forms/PageMaster.master" CodeFile="frmBanquet.aspx.cs" Inherits="Forms_frmBanquet" Title="CORN :: Banquet" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
        <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function ValidateForm() {
            debugger;
            str = document.getElementById('<%=txtEventDate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Please select Event Date')
                return false;
            }
            str = $("#ctl00_ctl00_mainCopy_cphPage_ddDineType_I").val();
            if (str == null || str.length == 0) {
                alert('Please select Dine Time')
                return false;
            }
           <%-- str = document.getElementById('<%=drpSaleUnit.ClientID%>').GetValue();
            if (str == null || str.length == 0) {
                alert('Select Sale unit')
                return false;
            }
            str = document.getElementById('<%=drpPurchaseUnit.ClientID%>').GetValue();
            if (str == null || str.length == 0) {
                alert('Select Purchase unit')
                return false;
            }--%>

            return true;
        }

        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }

            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function CalculateAmountByQtyBlur(input) {
            debugger;
            var qtyValue = $(input).val() == '' ? 0 : parseFloat($(input).val());
            var qtyCell = $(input).parent();
            var rateCell = qtyCell.next();

            //for prevoius td
            // qtyCell.prev();

            var rateValue = rateCell.find('input').val() == '' ? 0 : parseFloat(rateCell.find('input').val());

            var amountCell = rateCell.next();

            var totalValue = parseFloat(qtyValue * rateValue).toFixed(2);

            amountCell.find('input').val(totalValue);

        }


        function CalculateAmountByRateBlur(input) {
            debugger;
            var rateValue = $(input).val() == '' ? 0 : parseFloat($(input).val());
            var rateCell = $(input).parent();
            var qtyCell = rateCell.prev();

            //for prevoius td
            // qtyCell.prev();

            var qtyValue = qtyCell.find('input').val() == '' ? 0 : parseFloat(qtyCell.find('input').val());

            var amountCell = rateCell.next();

            var totalValue = parseFloat(qtyValue * rateValue).toFixed(2);

            amountCell.find('input').val(totalValue);

        }

        function grandTotal() {
            var totalAmount = 0;

            var gvDrv = document.getElementById("<%=Gridview1.ClientID %>");
            for (i = 1; i < gvDrv.rows.length; i++) {
                var cell = gvDrv.rows[i].cells;
                debugger;
                var HTML = $(cell[6]).find("input").val();
                HTML = parseFloat(HTML);
                if (HTML == "" || isNaN(HTML) || HTML == undefined || HTML == null) {
                    HTML = 0;
                }
                totalAmount = totalAmount + parseFloat(HTML);
            }

            document.getElementById("<%=itemsGrandTotal.ClientID %>").value = parseFloat(totalAmount).toFixed(2);

            var grandTotal = parseFloat(totalAmount);
            document.getElementById("<%=txtallTotal.ClientID %>").value = parseFloat(grandTotal).toFixed(2);
        }

        function CalculateRemaniningAmount(input) {
            debugger;
            var advanceAmount = document.getElementById("<%= txtAdvanceAmount.ClientID %>").value;
            var totalAmount = document.getElementById("<%=txtallTotal.ClientID %>").value;

            if (advanceAmount == "" || advanceAmount == undefined || isNaN(advanceAmount))
                advanceAmount = 0;

            if (totalAmount == "" || totalAmount == undefined || isNaN(totalAmount))
                totalAmount = 0;

            var remaingAmount = parseFloat(totalAmount) - parseFloat(advanceAmount);
            document.getElementById("<%=txtRemainingAmount.ClientID %>").value = parseFloat(remaingAmount).toFixed(2);
        }



    </script>

    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row top">
                <div class="col-md-offset-5 col-md-6 ">
                    <div class="btnlist pull-right">
                        <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>--%>
                        <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                        </asp:LinkButton>
                        <asp:LinkButton class="btn btn-success" ID="btnActive" runat="server" OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                        <!-- POP UP MODEL-->
                        <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters" TargetControlID="hbtn" BehaviorID="ModelPopup" BackgroundCssClass="modal-background" CancelControlID="btnClose"></cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveDocument">
                            <div class="modal-dialog" style="width: 90%; margin-left: 0px; max-height: 90%; top: 0px; left: 5%; position: fixed; overflow: auto">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                            <span>&times;</span><span class="sr-only">Close</span></button>
                                        <h1 class="modal-title" id="myModalLabel">
                                            <span></span>Add Booking</h1>
                                    </div>
                                    <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>--%>
                                    <div class="modal-body">
                                        
                                        <div class="row">
                                            <div class="col-md-10">
                                                <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                                            </div>
                                        </div>


                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel1" runat="server">

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            Customer Name:
                                                                            <dx:ASPxComboBox ID="ddlCustomer" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            ID Card:
                                                                            <asp:TextBox ID="txtIDCard" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            Customer Number:
                                                                            <asp:TextBox ID="txtCustomerNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            Other No:
                                                                            <asp:TextBox ID="txtOtherNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-8">
                                                            Mailing Address:
                           <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel3" runat="server">

                                                    <div class="row">
                                                        <asp:HiddenField ID="hfMaster_ID" runat="server" Value="0" />
                                                        <div class="col-md-3">
                                                            Event Date:
                                                                            <asp:TextBox ID="txtEventDate" runat="server" AutoPostBack="true" OnTextChanged="txtEventDate_SelectedIndexChanged" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1" style="margin-top: 30px">
                                                            <asp:ImageButton ID="ibtnEventDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                Width="30px" />
                                                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                                            <cc1:CalendarExtender ID="CEEventDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnEventDate"
                                                                TargetControlID="txtEventDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Dine Time</label>
                                                            <dx:ASPxComboBox ID="ddDineType" runat="server" CssClass="form-control">
                                                            </dx:ASPxComboBox>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Event Type:</label>
                                                            <dx:ASPxComboBox ID="ddlEventType" runat="server" CssClass="form-control">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel4" runat="server">

                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            Booking Date:
                                                                            <asp:TextBox ID="txtBookingDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1" style="margin-top: 30px">
                                                            <asp:ImageButton ID="ibtnBookingDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                Width="30px" />
                                                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                                            <cc1:CalendarExtender ID="CEBookingDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnBookingDate"
                                                                TargetControlID="txtBookingDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Hall #</label>
                                                            <dx:ASPxComboBox ID="txtHallNo" runat="server" CssClass="form-control">
                                                            </dx:ASPxComboBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Ladies</label>
                                                            <asp:TextBox ID="txtLadies" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Gents</label>
                                                            <asp:TextBox ID="txtGents" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <hr />

                                        <%--Items--%>
                                        <div class="row">
                                            <div class="col-md-12" style="padding-left: 0px; padding-right: 0px">
                                                <div style="width: 350px; height: auto; margin-left: 8%; padding: 1px; position: relative">
                                                    <h2 style="margin-bottom: 0px; font-size: 28px; font-weight: 600;">Menu</h2>
                                                </div>
                                                <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>--%>
                                                <div class="modal-body">

                                                    <div style="overflow-y: scroll; overflow-x: hidden; max-height: 300px; height: auto;">
                                                        <asp:GridView ID="Gridview1" runat="server" ShowFooter="false" CssClass="table table-responsive"
                                                            OnRowDataBound="BulkAdd_RowDataBound" OnRowCreated="GridView1_RowCreated" AutoGenerateColumns="false">

                                                            <Columns>

                                                                <asp:BoundField DataField="RowNumber" HeaderText="Sr. No" ItemStyle-Width="60px" />

                                                                <asp:TemplateField HeaderText="Particulars">
                                                                    <ItemStyle Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                                                            <ContentTemplate>
                                                                                <dx:ASPxComboBox ID="ddlSKU" runat="server" AutoPostBack="true"
                                                                                    CssClass="form-control" OnSelectedIndexChanged="ddlSKU_SelectedIndexChanged1">
                                                                                </dx:ASPxComboBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="UOM_ID">
                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                    <ItemStyle CssClass="HidePanel" />
                                                                </asp:BoundField>

                                                                <asp:TemplateField HeaderText="UOM">
                                                                    <ItemStyle Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtUOM" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Qty">
                                                                    <ItemStyle Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPax" onblur="CalculateAmountByQtyBlur(this); grandTotal();" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Rate">
                                                                    <ItemStyle Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRate" onblur="CalculateAmountByRateBlur(this); grandTotal();" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Amount">
                                                                    <ItemStyle Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <%--<asp:BoundField DataField="Amount_Value">
                                              <HeaderStyle CssClass="HidePanel" />
                                              <ItemStyle CssClass="HidePanel" />
                                          </asp:BoundField>--%>

                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemStyle Width="50px" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="bulkDeletebtn" OnClick="Gridview1_RowDeleting" runat="server">
                                                     <%-- <img src="../images/delete.gif" alt="Delete" />--%>
                                                      <span class="fa fa-trash-o"></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <HeaderStyle BackColor="SkyBlue" ForeColor="White" />
                                                            <EditRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <AlternatingRowStyle BackColor="#f8f8f8" />
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <asp:Button ID="ButtonAdd" OnClientClick="grandTotal();" OnClick="ButtonAdd_Click" runat="server" CssClass="btn btn-primary" Text="Add Menu" />
                                                        </div>
                                                        <div class="col-md-4"></div>
                                                        <div class="col-md-6">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                           <b style="margin-left: 130px;">Grand Total:</b>
                                                            <asp:TextBox ID="itemsGrandTotal" ReadOnly="true" runat="server">0</asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <%-- </ContentTemplate>
                           <Triggers>
                             <asp:PostBackTrigger ControlID = "ButtonAdd" />
                            </Triggers>
                      </asp:UpdatePanel>--%>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-md-8"></div>
                                            <div class="col-md-4" style="float: right">
                                                <table style="float: right; margin-right: 40px;">
                                                    <thead>
                                                        <th>Cash Flow </th>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Amount:</td>
                                                            <td>
                                                                <asp:TextBox Width="80px" ID="txtallTotal" ReadOnly="true" runat="server">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Advance Amount: </td>
                                                            <td>
                                                                <asp:TextBox Width="80px" onblur="CalculateRemaniningAmount(this);" ID="txtAdvanceAmount" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Remaining Amount: </td>
                                                            <td>
                                                                <asp:TextBox Width="80px" ID="txtRemainingAmount" ReadOnly="true" runat="server">0</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>

                                                </table>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-offset-4 col-md-4 ">
                                                <div class="btnlist mx-auto">
                                                    <asp:Button ID="btnSaveDocument" OnClick="btnSave_Document" AccessKey="S" Width="150px" runat="server" Text="Save" CssClass="btn btn-success" />
                                                    <asp:Button ID="btnCancel" AccessKey="C" runat="server" Text="Cancel" Width="150px" UseSubmitBehavior="False" CssClass="btn btn-danger" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
                            </div>
                        </asp:Panel>
                    </div>


                    <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
        <div class="row center">
            <div class="col-md-12">
                <div class="emp-table">
                    <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                                OnRowEditing="Grid_users_RowEditing" EmptyDataText="No Record exist"
                                OnRowDataBound="Grid_users_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EVENT_BOOKING_ID" HeaderText="FRANCHISE_MASTER_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="CUSTOMER" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EVENT_DATE" HeaderText="EVENT DATE" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="USER_NAME" HeaderText="Created By" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>--%>
                                    <asp:BoundField DataField="EVENT_TIME" HeaderText="EVENT TIME" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EVENT_TIME_ID" HeaderText="EVENT_TIME_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EVENT_TYPE" HeaderText="EVENT TYPE" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EVENT_TYPE_ID" HeaderText="EVENT_TYPE_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HALL_NO" HeaderText="HALL #" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TableDefination_ID" HeaderText="TableDefination_ID" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LADIES" HeaderText="LADIES" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GENTS" HeaderText="GENTS" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BOOKING_DATE" HeaderText="BOOKING_DATE" ReadOnly="true">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel "></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" ReadOnly="true">
                                        <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" class="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

