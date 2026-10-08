<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSKUCategory.aspx.cs" Inherits="frmSKUCategory" Title="Add Category" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <%--<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>--%>
    <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ErrorMessage() {
            alert('Level 4 reached.\n You cannot create next child of the selected Parent category');
        }
        function Message(msg) {
            alert(msg);
        }
        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtCategoryName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Child category name is required');
                return false;
            }

            return true;
        }

        function bindSKUImage() {

            var val = document.getElementById('<%=hidSKUImageName.ClientID%>').value;

            if (val == '' || val == null) {
                $('#pic').attr('src', '../images/no_image.gif').width(266).height(130);
            } else {
                var logo = val;
                $('#pic').attr('src', '../UserImages/Category/' + logo).width(266).height(130);
            }
        }
        function readImageURL(input) {
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png)$/;
            if (input.files && input.files[0]) {

                var file = input.files[0];
                if (regex.test(file.name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#pic').attr('src', e.target.result).width(266).height(130);
                        document.getElementById('<%=hidSKUImageSource.ClientID%>').value = e.target.result;
                    };
                    document.getElementById('<%=hidSKUImageName.ClientID%>').value = file.name;

                    reader.readAsDataURL(file);
                }
                else {
                    alert(file.name + " is not a valid image file.\n Only extentions .jpg|.jpeg|.gif|.png are supported.");
                    return false;
                }
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
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hidSKUImageName" runat="server" />
                    <asp:HiddenField ID="hidSKUImageSource" runat="server" />
                    <div>
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
                            
                            <div class="col-md-offset-5 col-md-3" style="margin-top:0%">
                                <div class="btnlist pull-right">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                                OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                            </asp:LinkButton>
                                            <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                            <!-- POP UP MODEL-->    
                                            <cc1:ModalPopupExtender ID="mPopUpCategory" runat="server" PopupControlID="pnlParameters"
                                                TargetControlID="btnAdd" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                                CancelControlID="btnClose">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none; width: 30%" ScrollBars="Auto" DefaultButton="btnSaveCategory">
                                                <div class="modal-dialog" style="width: 100%">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabel">
                                                                <span></span>Add New Category</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <%--   <div class="row" style="margin-top: 20px">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Category Type</label>
                                                                           <!--Have to remove-->
                                                                            <dx:ASPxComboBox ID="ddlType" runat="server" AutoPostBack="true" CssClass="form-control"
                                                                                OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </div>--%>
                                                                    <div class="row" style="display: none;">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Parent Category</label>
                                                                            <dx:ASPxComboBox ID="drpParentCategory" runat="server" CssClass="form-control">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Category Name </label>

                                                                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div style="display: none">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Category Code</label>
                                                                            <asp:TextBox ID="txtCategoryCode" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="display: none;">
                                                                        <div class="col-md-12">
                                                                            <div id="skuImageUploadArea" runat="server" visible="true">
                                                                                <label>
                                                                                    <span class="fa fa-caret-right rgt_cart"></span>Upload Image
                                                                                    &nbsp;(<%=allowed_extensions%>)
                                                                                </label>
                                                                                <label for="<%=fuPic.ClientID %>" style="cursor: pointer;">
                                                                                    <img src="../images/no_image.gif" id="pic" name="pic" alt="pic" width="266" height="130" />
                                                                                </label>
                                                                                <asp:FileUpload ID="fuPic" onchange="readImageURL(this);" Style="display: none;" runat="server"></asp:FileUpload>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                                                        <div class="col-md-7">
                                                                        </div>
                                                                        <div class="col-md-12" align="right">
                                                                            <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfcategoryId" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
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

                        <%--       ///////////////////////////////////////////////////////////////////////////////////////////////////////--%>

                        <!--Center class remove line 228 style="padding-left:25px"-->
                        <div class="row center">
                            <div class="col-md-12">
                                <div class="emp-table">
                                    <asp:GridView ID="GrdCategory" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        HorizontalAlign="Center" AllowPaging="true"
                                        AutoGenerateColumns="False" OnRowEditing="GrdCategory_RowEditing" PageSize="8"
                                        OnPageIndexChanging="GrdCategory_PageIndexChanging" EmptyDataText="No Record exist">

                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle />

                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                                    <asp:HiddenField ID="hidSKUImageName" runat="server" Value='<%# Eval("ImagePath") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                <HeaderStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                <HeaderStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Category" ReadOnly="true">
                                                <ItemStyle Width="53%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TYPE" HeaderText="Type" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                                <ItemStyle Width="8%" />
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Width="20px" ToolTip="Edit" CssClass="fa fa-pencil">                                            
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />

                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PARENT_SKU_HIE_ID" HeaderText="Type" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                <HeaderStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARENT_SKU_HIE_TYPE_ID" HeaderText="PARENT_SKU_HIE_TYPE_ID" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                <HeaderStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                        </Columns>

                                        <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                        <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel21">
                <ProgressTemplate>
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"></asp:Image>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>
</asp:Content>
