<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSKUHierarchyDataFashion.aspx.cs" Inherits="frmSKUHierarchyDataFashion" Title="CORN :: Add Supplier" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script language="JavaScript" type="text/javascript">

        // Code for Division Tab

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtPrincipalName.ClientID%>').value +
            document.getElementById('<%=txtCategoryName.ClientID%>').value +
            document.getElementById('<%=txtSubCategoryName.ClientID%>').value +
            document.getElementById('<%=txtBrandName.ClientID%>').value +
            document.getElementById('<%=txtGenderName.ClientID%>').value;

          //  str = document.getElementById('<%=txtPrincipalName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Name is required');
                return false;
            }
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
        // End Code For Division Tab
    </script>
    <div>
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#division">Division</a></li>
            <li><a data-toggle="tab" href="#category">Category</a></li>
            <li><a data-toggle="tab" href="#subcategory">Sub Category</a></li>
            <li><a data-toggle="tab" href="#brand">Brand</a></li>
            <li><a data-toggle="tab" href="#gender">Gender</a></li>
        </ul>

        <div class="tab-content">
            <div id="division" class="tab-pane fade in active">
                <h3>Division</h3>
                <div class="main-contents">
                    <div class="container employee-infomation">
                        <div class="row top">
                            <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
                                <div class="col-md-4">
                                    <div class="search">
                                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                            TabIndex="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2" style="margin-left: -60px;">
                                    <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                                </div>
                                <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
                            </asp:Panel>
                            <div class="col-md-offset-5 col-md-3 ">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                                OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server"  PopupControlID="pnlParameters"
                                                TargetControlID="btnAdd" BehaviorID="ModelPopup1" BackgroundCssClass="modal-background"
                                                CancelControlID="btnClose">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSavePrincipal">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabel" runat="server">
                                                                <span></span>Add New Division</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-1">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <%--<label><span class="fa fa-caret-right rgt_cart"></span>Name</label>--%>
                                                                            <asp:TextBox ID="txtPrincipalName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-6" style="display:none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Contact Person</label>
                                                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6" >
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                                ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationGroup="emailvalidate"
                                                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Phone</label>
                                                                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftetxtPhoneNumber" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtPhoneNumber" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Fax Number</label>
                                                                            <asp:TextBox ID="txtFaxNumber" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtFaxNumber" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtFaxNumber" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-5" align="right">
                                                                            <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfPrincipalId" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSavePrincipal" OnClick="btnSavePrincipal_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <%--//////////////////////////////////////////////////////////////////////////////////////////////////--%>
                        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 90px">
                            &nbsp;<asp:Panel ID="Panel21" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel5">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="row center">
                                    <div class="col-md-12">
                                        <div class="emp-table">
                                            <asp:GridView ID="GrdDivision" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdDivision_RowEditing" OnPageIndexChanging="grdDataDivision_PageIndexChanging"
                                                EmptyDataText="No Record exist"
                                                PageSize="8">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_CODE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_MANUALDISCOUNT" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADDRESS" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" ReadOnly="true">
                                                        <%--<ItemStyle Width="30%"></ItemStyle>--%>
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMAIL" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PHONE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FAX" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                        <ItemStyle Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_HIE_ID" )%>' ToolTip="Edit">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div id="category" class="tab-pane fade">
                <h3>Category</h3>
                <div class="main-contents">
                    <div class="container employee-infomation">
                        <div class="row top">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFilterSearch">
                                <div class="col-md-4">
                                    <div class="search">
                                        <asp:TextBox ID="txtSearchCategory" runat="server" placeholder="Search" CssClass="form-control"
                                            TabIndex="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2" style="margin-left: -60px;">
                                    <asp:LinkButton ID="btnFilterSearch" OnClick="btnFilterSearch_Click" runat="server" Text="Search"
                                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                                </div>
                                <asp:LinkButton ID="btndummyCategory" runat="server" UseSubmitBehavior="false" />
                            </asp:Panel>
                            <div class="col-md-offset-5 col-md-3 ">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAddCategory" Text="Add"
                                                OnClick="btnAddCategory_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActiveCategory_Click" ID="btnActiveCategory" runat="server"
                                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpLocationCategory" runat="server" PopupControlID="pnlParametersCategory"
                                                TargetControlID="btnAddCategory" BehaviorID="ModelPopup2"  BackgroundCssClass="modal-background"
                                                CancelControlID="btnCloseCategory">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParametersCategory" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveCategory">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnCloseCategory" class="close" runat="server" onserverclick="btnCloseCategory_ServerClick">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabelCategory" runat="server">
                                                                <span></span>Add New Category</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <asp:Label ID="lblErrorMsgCategory" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-1">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                        </div>
                                                                            <div class="col-md-8">
                                                                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-6" style="display:none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Contact Person</label>
                                                                            <asp:TextBox ID="txtContactPersonCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                            <asp:TextBox ID="txtAddressCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                                                            <asp:TextBox ID="txtEmailCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                                ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationGroup="emailvalidate"
                                                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Phone</label>
                                                                            <asp:TextBox ID="txtPhoneNumberCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtPhoneNumberCategory" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Fax Number</label>
                                                                            <asp:TextBox ID="txtFaxNumberCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtFaxNumberCategory" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-5" align="right">
                                                                            <asp:HiddenField ID="hfStatusCategory" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfCategoryID" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                            <asp:Button ID="btnCancelCategory" OnClick="btnCancelCategory_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <%--//////////////////////////////////////////////////////////////////////////////////////////////////--%>
                        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 90px">
                            &nbsp;<asp:Panel ID="Panel3" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="row center">
                                    <div class="col-md-12">
                                        <div class="emp-table">
                                            <asp:GridView ID="GrdCategory" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdCategory_RowEditing" OnPageIndexChanging="GrdCategory_PageIndexChanging"
                                                EmptyDataText="No Record exist"
                                                PageSize="8">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_CODE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_MANUALDISCOUNT" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADDRESS" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" ReadOnly="true">
                                                        <%--<ItemStyle Width="30%"></ItemStyle>--%>
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMAIL" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PHONE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FAX" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                        <ItemStyle Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_HIE_ID" )%>' ToolTip="Edit">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
            <div id="subcategory" class="tab-pane fade">
                <h3>Sub Category</h3>

                <div class="main-contents">
                    <div class="container employee-infomation">
                        <div class="row top">
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnsearch">
                                <div class="col-md-4">
                                    <div class="search">
                                        <asp:TextBox ID="txtSearchSubCategory" runat="server" placeholder="Search" CssClass="form-control"
                                            TabIndex="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2" style="margin-left: -60px;">
                                    <asp:LinkButton ID="btnsearchSubCategory" OnClick="btnsearchSubCategory_Click" runat="server" Text="Search"
                                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                                </div>
                                <asp:LinkButton ID="btndummySubCategory" runat="server" UseSubmitBehavior="false" />
                            </asp:Panel>
                            <div class="col-md-offset-5 col-md-3 ">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAddSubCategory" Text="Add"
                                                OnClick="btnAddSubCategory_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActiveSubCategory_Click" ID="btnActiveSubCategory" runat="server"
                                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpLocationSubCategory" runat="server" PopupControlID="pnlParametersSubCategory"
                                                TargetControlID="btnAddSubCategory" BehaviorID="ModelPopup3" BackgroundCssClass="modal-background"
                                                CancelControlID="btnCloseSubCategory">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParametersSubCategory" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveSubCategory">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnCloseSubCategory" class="close" runat="server" onserverclick="btnCloseSubCategory_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabelSubCategory" runat="server">
                                                                <span></span>Add New Sub Category</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <asp:Label ID="lblErrorMsgSubCategory" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                               <asp:DropDownList ID="ddlParentCategory" runat="server" Width="200px">
                                                                                    <asp:ListItem Text="Select Category" Value="0"></asp:ListItem>
                                                                                    <%--<asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="Female" Value="2"></asp:ListItem>--%>
                                                                                </asp:DropDownList>                                                                            
                                                                        </div>
                                                                        </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                            <asp:TextBox ID="txtSubCategoryName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-6" style="display:none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Contact Person</label>
                                                                            <asp:TextBox ID="txtContactPersonSubCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                            <asp:TextBox ID="txtAddressSubCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                                                            <asp:TextBox ID="txtEmailSubCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                                                ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationGroup="emailvalidate"
                                                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Phone</label>
                                                                            <asp:TextBox ID="txtPhoneNumberSubCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtPhoneNumberSubCategory" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Fax Number</label>
                                                                            <asp:TextBox ID="txtFaxNumberSubCategory" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtFaxNumberSubCategory" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-5" align="right">
                                                                            <asp:HiddenField ID="hfStatusSubCategory" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfSubCategoryID" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveSubCategory" OnClick="btnSaveSubCategory_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                            <asp:Button ID="btnCancelSubCategory" OnClick="btnCancelSubCategory_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <%--//////////////////////////////////////////////////////////////////////////////////////////////////--%>
                        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 90px">
                            &nbsp;<asp:Panel ID="Panel6" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel9">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButton2SubCategory" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <div class="row center">
                                    <div class="col-md-12">
                                        <div class="emp-table">
                                            <asp:GridView ID="GrdSubCategory" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdSubCategory_RowEditing" OnPageIndexChanging="grdDataSubCategory_PageIndexChanging"
                                                EmptyDataText="No Record exist"
                                                PageSize="8">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_CODE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_MANUALDISCOUNT" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADDRESS" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" ReadOnly="true">
                                                        <%--<ItemStyle Width="30%"></ItemStyle>--%>
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMAIL" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PHONE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FAX" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="PCategory" HeaderText="Category" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                        <ItemStyle Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PARENT_SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                     
                                                    
                                                   
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_HIE_ID" )%>' ToolTip="Edit">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>


            <div id="brand" class="tab-pane fade">
                <h3>Brand</h3>
                <div class="main-contents">
                    <div class="container employee-infomation">
                        <div class="row top">
                            <asp:Panel ID="Panel5" runat="server" DefaultButton="btnsearch">
                                <div class="col-md-4">
                                    <div class="search">
                                        <asp:TextBox ID="txtSearchBrand" runat="server" placeholder="Search" CssClass="form-control"
                                            TabIndex="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2" style="margin-left: -60px;">
                                    <asp:LinkButton ID="btnsearchBrand" OnClick="btnsearchBrand_Click" runat="server" Text="Search"
                                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                                </div>
                                <asp:LinkButton ID="btndummyBrand" runat="server" UseSubmitBehavior="false" />
                            </asp:Panel>
                            <div class="col-md-offset-5 col-md-3 ">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAddBrand" Text="Add"
                                                OnClick="btnAddBrand_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActiveBrand_Click" ID="btnActiveBrand" runat="server"
                                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpLocationBrand" runat="server" PopupControlID="pnlParametersBrand"
                                                TargetControlID="btnAddBrand" BehaviorID="ModelPopup4" BackgroundCssClass="modal-background"
                                                CancelControlID="btnCloseBrand">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParametersBrand" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveBrand">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnCloseBrand" class="close" runat="server" onserverclick="btnCloseBrand_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabelBrand" runat="server">
                                                                <span></span>Add New Brand</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <asp:Label ID="lblErrorMsgBrand" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-1">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <asp:TextBox ID="txtBrandName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-6" style="display:none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Contact Person</label>
                                                                            <asp:TextBox ID="txtContactPersonBrand" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                            <asp:TextBox ID="txtAddressBrand" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                                                            <asp:TextBox ID="txtEmailBrand" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                                                ErrorMessage="Invalid Email" ControlToValidate="txtEmailBrand" ValidationGroup="emailvalidate"
                                                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Phone</label>
                                                                            <asp:TextBox ID="txtPhoneNumberBrand" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtPhoneNumberBrand" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Fax Number</label>
                                                                            <asp:TextBox ID="txtFaxNumberBrand" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtFaxNumberBrand" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-5" align="right">
                                                                            <asp:HiddenField ID="hfStatusBrand" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfBrandId" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveBrand" OnClick="btnSaveBrand_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                            <asp:Button ID="btnCancelBrand" OnClick="btnCancelBrand_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <%--//////////////////////////////////////////////////////////////////////////////////////////////////--%>
                        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 90px">
                            &nbsp;<asp:Panel ID="Panel8" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel12">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButtonBrand" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <div class="row center">
                                    <div class="col-md-12">
                                        <div class="emp-table">
                                            <asp:GridView ID="GrdBrand" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdBrand_RowEditing" OnPageIndexChanging="GrdBrand_PageIndexChanging"
                                                EmptyDataText="No Record exist"
                                                PageSize="8">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_CODE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_MANUALDISCOUNT" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADDRESS" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" ReadOnly="true">
                                                        <%--<ItemStyle Width="30%"></ItemStyle>--%>
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMAIL" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PHONE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FAX" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                        <ItemStyle Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_HIE_ID" )%>' ToolTip="Edit">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
            <div id="gender" class="tab-pane fade">
                <h3>Gender</h3>

                <div class="main-contents">
                    <div class="container employee-infomation">
                        <div class="row top">
                            <asp:Panel ID="Panel7" runat="server" DefaultButton="btnsearch">
                                <div class="col-md-4">
                                    <div class="search">
                                        <asp:TextBox ID="txtSearchGender" runat="server" placeholder="Search" CssClass="form-control"
                                            TabIndex="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2" style="margin-left: -60px;">
                                    <asp:LinkButton ID="btnsearchGender" OnClick="btnsearchGender_Click" runat="server" Text="Search"
                                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                                </div>
                                <asp:LinkButton ID="btndummyGender" runat="server" UseSubmitBehavior="false" />
                            </asp:Panel>
                            <div class="col-md-offset-5 col-md-3 ">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAddGender" Text="Add"
                                                OnClick="btnAddGender_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActiveGender_Click" ID="btnActiveGender" runat="server"
                                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpLocationGender" runat="server" PopupControlID="pnlParametersGender"
                                                TargetControlID="btnAddGender" BehaviorID="ModelPopup5" BackgroundCssClass="modal-background"
                                                CancelControlID="btnCloseGender">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParametersGender" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveBrand">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnCloseGender" class="close" runat="server" onserverclick="btnCloseGender_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabelGender" runat="server">
                                                                <span></span>Add New Gender</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <asp:Label ID="lblErrorMsgGender" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-1">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <asp:TextBox ID="txtGenderName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-6" style="display:none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Contact Person</label>
                                                                            <asp:TextBox ID="txtContactPersonGender" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                            <asp:TextBox ID="txtAddressGender" runat="server" CssClass="form-control "></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                                                            <asp:TextBox ID="txtEmailGender" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                                                ErrorMessage="Invalid Email" ControlToValidate="txtEmailGender" ValidationGroup="emailvalidate"
                                                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display:none">
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Phone</label>
                                                                            <asp:TextBox ID="txtPhoneNumberGender" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtPhoneNumberGender" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Fax Number</label>
                                                                            <asp:TextBox ID="txtFaxNumberGender" runat="server" CssClass="form-control "></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Custom"
                                                                                TargetControlID="txtFaxNumberGender" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-5" align="right">
                                                                            <asp:HiddenField ID="hfStatusGender" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfGenderId" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveGender" OnClick="btnSaveGender_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                            <asp:Button ID="btnCancelGender" OnClick="btnCancelBrand_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <%--//////////////////////////////////////////////////////////////////////////////////////////////////--%>
                        <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 90px">
                            &nbsp;<asp:Panel ID="Panel10" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel15">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButtonGender" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                            <ContentTemplate>
                                <div class="row center">
                                    <div class="col-md-12">
                                        <div class="emp-table">
                                            <asp:GridView ID="GrdGender" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdGender_RowEditing" OnPageIndexChanging="GrdGender_PageIndexChanging"
                                                EmptyDataText="No Record exist"
                                                PageSize="8">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SKU_HIE_ID" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_CODE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name" ReadOnly="true">
                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_MANUALDISCOUNT" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADDRESS" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" ReadOnly="true">
                                                        <%--<ItemStyle Width="30%"></ItemStyle>--%>
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMAIL" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PHONE" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FAX" ReadOnly="true">
                                                        <ItemStyle CssClass="HidePanel" />
                                                        <HeaderStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                        <ItemStyle Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_HIE_ID" )%>' ToolTip="Edit">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>


    </div>



</asp:Content>
