<%@ Page Title="CORN :: Add Packages" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPackage.aspx.cs" Inherits="Forms_frmItemDeals" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
<script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <link href="../css/Popup.css" rel="stylesheet" />
    <script language="JavaScript" type="text/javascript">


        function ValidateForm() {
            var str;
            return true;
        }
        function ShowHideQtyTextBox(value)
        {
        }

        function pageLoad() {

            var popUp = $find('ModelPopup');

            //check it exists so the script won't fail
            if (popUp) {
                //Add the function below as the event
                popUp.add_hidden(HidePopupPanel);
            }
        }

        function HidePopupPanel(source, args) {
            //find the panel associated with the extender
            objPanel = document.getElementById(source._PopupControlID);

            //check the panel exists
            if (objPanel) {
                //set the display attribute, so it remains hidden on postback
                objPanel.style.display = 'none';
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div>
                <asp:UpdateProgress ID="UpdateProgress" runat="server">
                    <ProgressTemplate>
                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="hbtn"
                    PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
            </div>

            <div class="main-contents">
                <div class="container user_assignment">
                    <div class="row">
                        <div class="col-md-3">
                            <label>
                                <span class="fa fa-caret-right rgt_cart"></span>Item</label>
                            <dx:ASPxComboBox ID="ddlHasModifier" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlHasModifier_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                           
                        </div>
                        <div class="col-md-3">
                            <label>
                                <span class="fa fa-caret-right rgt_cart"></span>Category</label>
                            <dx:ASPxComboBox ID="drpCateogry" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="drpCateogry_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-2" style="visibility:hidden">
                            <label>
                                <span class="fa fa-caret-right rgt_cart"></span>
                            </label>
                            <asp:CheckBox ID="chkIsActive" runat="server" Text=" Is Active"
                                Checked="true" Font-Bold="true"></asp:CheckBox>

                        </div>
                    </div>
                    &nbsp;
                    <div class="row bottom">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <span class="fa fa-caret-right rgt_cart"></span>Item Assignment
                                </div>
                                <div class="panel-body">
                                    <div class="module_contents">
                                        <asp:HiddenField runat="server" ID="hfCategoryId" Value="0" />
                                        <div class="col-md-3">
                                            <asp:ListBox ID="lstUnAssignItem" runat="server" Height="350px"
                                                CssClass="select"></asp:ListBox>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="navs navslist">
                                                <div class="nxt arrow">
                                                    <span>
                                                        <asp:LinkButton ID="btnAssignItem" OnClick="btnAssignItem_Click" CssClass="fa  fa-angle-right"
                                                            runat="server" /></span>
                                                </div>
                                                <div class="nxt arrow">
                                                    <span>
                                                        <asp:LinkButton ID="btnAssignAllItem" OnClick="btnAssignAllItem_Click"
                                                            CssClass="fa  fa-angle-double-right" runat="server" /></span>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Panel runat="server" ID="pnlList" Height="350px" ScrollBars="Vertical" BorderWidth="1px"
                                                BorderStyle="Solid" BorderColor="Silver">
                                                <asp:GridView ID="grdPackage" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                    AutoGenerateColumns="False" Width="100%" OnRowDeleting="grdPackage_RowDeleting"
                                                    EmptyDataText="No Item exist">
                                                    <Columns>
                                                        <asp:BoundField DataField="CATEGORY_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category" ReadOnly="true">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Default_Qty" HeaderText="Qty" ReadOnly="true">
                                                            <ItemStyle CssClass="grdDetail" Width="5%"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Is_Modifier" HeaderText="Item Name" ReadOnly="true">
                                                            <ItemStyle Width="20%"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ModifierSKU_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Status">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IsModifierID">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="intStockMUnitCode">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                    Text="X" ToolTip="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-offset-5 col-md-3 ">
                        <div class="btnlist pull-right">
                            <button type="button" id="btnAdd" runat="server" style="display:none;"/>
                            <cc1:ModalPopupExtender ID="mPopUpSection" runat="server" PopupControlID="pnlParameters"
                                TargetControlID="btnAdd" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                CancelControlID="btnCancel">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h2>Are you sure you want to InActive the deal!</h2>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-offset-4 col-md-4 ">
                                                            <div class="btnlist pull-right">
                                                                <asp:Button CssClass="btn btn-success" runat="server" ID="btnOk" OnClick="btnOk_Click" Text="Ok" />
                                                                 <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btn-danger" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-8">

                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


