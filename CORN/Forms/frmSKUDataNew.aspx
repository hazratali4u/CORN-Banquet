<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmSKUDataNew.aspx.cs" Inherits="Forms_frmSKUDataNew" Title="Item Information" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <%--<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>--%>
    <script src="../js/angular.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">



        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function Message(msg) {
            alert(msg);
        }
        function ValidateForm() {

            str = document.getElementById('<%=txtskuname.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Item name is required')
                return false;
            }

            str = document.getElementById('<%=txtDescription2.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Description is required')
                return false;
            }

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

        function OnChkSelClick(s, e) {

            if (s.GetChecked())
                chkSelOpt.SelectAll();
            else
                chkSelOpt.UnselectAll();

            <%--if (s.checked) {
                var chklstSection1 = document.getElementById('<%= ChCategoryList.ClientID %>');
                chklstSection1.SelectAll();
            }--%>
        }

        function OnChkSelClick1(s, e) {

            if (s.GetChecked())
                chkSelOpt1.SelectAll();
            else
                chkSelOpt1.UnselectAll();

            <%--if (s.checked) {
                var chklstSection1 = document.getElementById('<%= ChCategoryList.ClientID %>');
                chklstSection1.SelectAll();
            }--%>
                }
    </script>
    <script type="text/javascript" language="javascript">

        function bindSKUImage() {

            var val = document.getElementById('<%=hidSKUImageName.ClientID%>').value;

            if (val == '' || val == null) {
                $('#pic').attr('src', '../images/no_image.gif').width(266).height(130);
            } else {
                var logo = val;
                $('#pic').attr('src', '../UserImages/Sku/' + logo).width(266).height(130);
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

    </script>

    <style type="text/css">
        .form-group {
            border: 1px solid #ccc;
        }

        .btn-file {
            position: relative;
            overflow: hidden;
            float: left;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 90%;
                min-height: 90%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }

        #img-upload, #img-upload2 {
            width: 100%;
        }


        .border {
            border: 1px solid #999999;
        }

        .mt-5 {
            margin-top: 5px;
        }

        .mr-5 {
            margin-right: 5px !important;
        }

        .h-75 {
            width: 400px;
            height: 150px;
            overflow-y: scroll;
        }

        .p {
            padding-inline-start: 0% !important;
            margin-left: 7px;
        }



        .pr-5 {
            padding-right: 5px;
        }

        .row-s {
            height: 200px !important;
        }

        .ml-5 {
            margin-left: 10px
        }
    </style>

    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
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


                                <asp:LinkButton CssClass="btn btn-warning" OnClick="btnAdd_Click" runat="server" ID="btnAdd" Text="Add">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="hbutton"></asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are your sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                <!-- POP UP MODEL-->
                                <ajaxToolkit:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="hbutton" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" DefaultButton="btnSave" runat="server" Style="display: none; width: 100%" ScrollBars="Auto">
                                    <div class="modal-dialog2" style="width: 70%">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" onserverclick="btnClose_Click" id="btnClose" class="close" runat="server">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel">
                                                    <span></span>Add New Item Information</h1>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                            Height="100%" Width="100%">
                                                            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" Height="100%">
                                                                <HeaderTemplate>
                                                                    Item Info
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <asp:HiddenField ID="hidSKUImageName" runat="server" />
                                                                    <asp:HiddenField ID="hidSKUImageSource" runat="server" />
                                                                    <div class="main-contents" style="height: 100%;">
                                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                            <ContentTemplate>
                                                                                <div class="modal-body">
                                                                                    <div class="row">
                                                                                        <div class="col-md-6">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>
                                                                                            <asp:TextBox ID="txtskuname" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtskucode" runat="server" CssClass="form-control" Style="display: none;"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Description</label>
                                                                                            <asp:TextBox ID="txtDescription2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-6">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Category</label>
                                                                                            <asp:Panel ID="Panel2" runat="server" Height="243px" ScrollBars="Vertical" BorderColor="Silver"
                                                                                                BorderStyle="Groove" BorderWidth="1px" Width="100%">
                                                                                                <div style="padding-left: 10px;">
                                                                                                <dx:ASPxCheckBox ID="chkSel" runat="server" ClientInstanceName="chkSelAll" Text="Select All"
                                                                                                    Font-Bold="true">
                                                                                                    <ClientSideEvents CheckedChanged="OnChkSelClick" />
                                                                                                </dx:ASPxCheckBox>
                                                                                                    </div>
                                                                                                <dx:ASPxCheckBoxList ID="ChCategoryList" runat="server" ClientInstanceName="chkSelOpt" ValueField="SKU_HIE_ID" TextField="SKU_HIE_NAME">
                                                                                                    <Border BorderStyle="None" />
                                                                                                </dx:ASPxCheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <!--Item Category-->
                                                                                        </div>

                                                                                        <!--vendor-->
                                                                                        <div class="col-md-6 col-xs-2">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Add Vendor</label>
                                                                                            <asp:Panel ID="Panel1" runat="server" Height="243px" ScrollBars="Vertical" BorderColor="Silver"
                                                                                                BorderStyle="Groove" BorderWidth="1px" Width="100%">
                                                                                                <div style="padding-left: 10px;">
                                                                                                <dx:ASPxCheckBox ID="chkSel1" runat="server" ClientInstanceName="chkSelAll1" Text="Select All"
                                                                                                    Font-Bold="true">
                                                                                                    <ClientSideEvents CheckedChanged="OnChkSelClick1" />
                                                                                                </dx:ASPxCheckBox>
                                                                                                    </div>
                                                                                                <dx:ASPxCheckBoxList ID="ChVendorList" RepeatColumns="1" ClientInstanceName="chkSelOpt1" RepeatDirection="Vertical" runat="server" ValueField="SKU_HIE_ID" TextField="SKU_HIE_NAME">
                                                                                                    <Border BorderStyle="None" />
                                                                                                </dx:ASPxCheckBoxList>
                                                                                            </asp:Panel>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </ajaxToolkit:TabPanel>


                                                        </ajaxToolkit:TabContainer>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-11" style="text-align: right; margin-top: 20px; margin-right: 40px">
                                                            <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                            <asp:HiddenField ID="hfSKU_ID" runat="server" />
                                                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-success" />
                                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"></asp:ImageButton>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!--class change (center) 663 style style="padding-left:25px"-->

            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>

                                <asp:GridView ID="grdSKUData" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    HorizontalAlign="Center"
                                    OnRowEditing="grdSKUData_RowEditing"
                                    OnPageIndexChanging="grdSKUData_PageIndexChanging"
                                    AutoGenerateColumns="False"
                                    AllowPaging="true" PageSize="20" EmptyDataText="No Record exist">

                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle />

                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                        <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="SKU_NAME" HeaderText="Name" ReadOnly="true">
                                            <ItemStyle Width="30%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true">
                                            <ItemStyle Width="30%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ISACTIVE" HeaderText="Status" ReadOnly="true">
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandArgument='<%#Eval("SKU_ID")%>' OnClick="btnEdit_Click" ToolTip="Edit">
                                                </asp:LinkButton> <%--|
                                        <asp:LinkButton ID="del" runat="server" CssClass="fa fa-trash-o" CommandArgument='<%#Eval("SKU_ID")%>' OnClick="del_Click" ToolTip="Delete">
                                        </asp:LinkButton>--%>
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
    </div>
</asp:Content>
