<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSKUDataFashion.aspx.cs" Inherits="frmSKUDataFashion" Title="CORN :: Add Item Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtStyleCode.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Style Code is required');
                return false;
            }
            str = document.getElementById('<%=txtBarCode.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Bar Code is required');
                return false;
            }
            str = document.getElementById('<%=txtItemName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Product Name is required');
                return false;
            }
            str = document.getElementById('<%=txtSize.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Size is required');
                return false;
            }
            str = document.getElementById('<%=txtYear.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Year is required');
                return false;
            }
            str = document.getElementById('<%=txtColor.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Year is required');
                return false;
            }
            str = document.getElementById('<%=txtSKU.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('SKU is required');
                return false;
            }

            if (document.getElementById('<%=ddlDivision.ClientID%>').selectedIndex == 0) {
                alert("Please select Division");
                return false;
            }
          <%--  if (document.getElementById('<%=ddlCategory.ClientID%>').selectedIndex == 0) {
                alert("Please select Category");
                return false;
            }--%>
           <%--if (document.getElementById('<%=ddlSubCategory.ClientID%>').selectedIndex == 0) {
                alert("Please select Sub Category");
                return false;
            }--%>
            if (document.getElementById('<%=ddlGender.ClientID%>').selectedIndex == 0) {
                alert("Please select Gender");
                return false;
            }
            if (document.getElementById('<%=ddlBrand.ClientID%>').selectedIndex == 0) {
                alert("Please select Gender");
                return false;
            }
            if (document.getElementById('<%=ddlGSTOn.ClientID%>').selectedIndex == 0) {
                alert("Please select GST On");
                return false;

                if (document.getElementById('<%=ddlSeasion.ClientID%>').selectedIndex == 0) {
                    alert("Please select Seasion");
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

               function MyJSFunction2() {

            var val = document.getElementById('<%=hfPic.ClientID%>').value;

            if (val == '' || val == null) {
                $('#pic').attr('src', '/images/no-image.jpg').width(150).height(168);
            } else {
                var logo = val;
                $('#pic').attr('src', '/pics/' + logo).width(150).height(168);
            }
        }
        function readURL(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#pic').attr('src', e.target.result).width(170).height(170);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }


    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 100px">
                <asp:Panel ID="Panel2" runat="server">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                        <ProgressTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="26px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                Width="27px" />
                            Wait Update.......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
            </div>
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
                                <asp:HiddenField ID="hfPic" runat="server" />
                                <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                    OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                <!-- POP UP MODEL-->
                                <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="btnAdd" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSaveSKUDataFashion">
                                    <div class="modal-dialog" style="width: 1030px; margin-left: 0px">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel" runat="server">
                                                    <span></span>Add Item Information</h1>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <%--<Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="TextChanged" />
                                                </Triggers>--%>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlSubCategory" EventName="TextChanged" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div class="modal-body">
                                                        <div class="col-md-8">
                                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                            </div>
                                                        <div class="row">
                                                            <div class="col-md-9">
                                                            
                                                        
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Division</label>
                                                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select Division" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sub Category</label>
                                                                <asp:DropDownList ID="ddlSubCategory" runat="server" OnTextChanged="ddlSubCategory_TextChanged" CssClass="form-control" AutoPostBack="True">
                                                                                    <asp:ListItem Text="Select Sub Category" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                                                                <asp:DropDownList ID="ddlCategory" runat="server"  CssClass="form-control" Enabled="False"> <%--OnTextChanged="ddlCategory_TextChanged"--%>
                                                                                    <asp:ListItem Text="Select Category" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                        <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Gender</label>
                                                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select Gender" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Brand</label>
                                                                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select Brand" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Orign</label>
                                                                <asp:DropDownList ID="ddlOrign" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select Orign" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="PK" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="USA" Value="2"></asp:ListItem>
                                                                                    
                                                                </asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>
                                                       
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>GST On</label>
                                                                <asp:DropDownList ID="ddlGSTOn" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select GST On" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="Exempted" Value="E"></asp:ListItem>
                                                                                    <asp:ListItem Text="Applied" Value="A"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Seasion</label>
                                                                <asp:DropDownList ID="ddlSeasion" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Select Seasion" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="Spring" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="Summer" Value="2"></asp:ListItem>
                                                                                    <asp:ListItem Text="Autumn" Value="3"></asp:ListItem>
                                                                                    <asp:ListItem Text="Winter" Value="4"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Style Code</label>
                                                                <asp:TextBox ID="txtStyleCode" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                            
                                                        </div>
                                                        <div class="row">
                                                            
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Bar Code</label>
                                                                <asp:TextBox ID="txtBarCode" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>
                                                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Size</label>
                                                                <asp:TextBox ID="txtSize" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Year</label>
                                                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                        
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Color</label>
                                                                <asp:TextBox ID="txtColor" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>SKU</label>
                                                                <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control "></asp:TextBox>
                                                            </div>
                                                        </div>
                                                       
                                                                
                                                                   
                                                                
                                                       
                                                        <div class="row">
                                                            <div class="col-md-12" align="right">
                                                                <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                                <asp:HiddenField ID="hfSKUlId" runat="server" Value="0" />
                                                                <asp:Button ID="btnSaveSKUDataFashion" OnClick="btnSaveSKUDataFashion_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                   <div class="col-md-3" style="margin-top: 25px;">
                                                                <div class="col-md-12">
                                                                    <label for="ctl00_ctl00_mainCopy_cphPage_fuPic" id="lbl"
                                                                        runat="server" style="cursor: pointer;">
                                                                        <img src="../images/noimage.jpg" id="pic" name="pic" width="170" height="170" />
                                                                    </label>
                                                                    <asp:FileUpload ID="fuPic" onchange="readURL(this);" Style="display: none;" runat="server"></asp:FileUpload>
                                                                </div>
                                                       <div class="col-md-12">
                                                                    <asp:CheckBox ID="chkShowPOS" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                    <label>Show on POS</label>
                                                                </div>
                                                                
                                                                
                                                            </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSaveSKUDataFashion" />
                                                </Triggers>
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
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
                        <ProgressTemplate>
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"
                                 />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
            </div>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:GridView ID="GrdSKUSDataFashion" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdSKUSDataFashion_RowEditing" OnPageIndexChanging="grdData_PageIndexChanging"
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
                                        <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMPANY_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DIVISION_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CATEGORY_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SUB_CATEGORY_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BRAND_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="TAG_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>


                                       <%-- Visible Grid Columns--%>

                                        <asp:BoundField DataField="Division" HeaderText="Division" ReadOnly="true">
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Category" HeaderText="Category" ReadOnly="true">
                                            <ItemStyle Width="15%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Brand" HeaderText="Brand" ReadOnly="true">
                                            <ItemStyle Width="15%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="true">
                                            <ItemStyle Width="15%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BAR_CODE" HeaderText="Bar Code" ReadOnly="true">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_CODE" HeaderText="Style Code" ReadOnly="true">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_NAME" HeaderText="Name" ReadOnly="true">
                                            <ItemStyle Width="15%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PACKSIZE" HeaderText="Size" ReadOnly="true">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COLOR" HeaderText="Color" ReadOnly="true">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GST_ON" HeaderText="GST" ReadOnly="true">
                                            <ItemStyle Width="2%"></ItemStyle>
                                        </asp:BoundField>
                                     
                                        <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                            <ItemStyle Width="30%"></ItemStyle>
                                        </asp:BoundField>
                                        
                                         <asp:BoundField DataField="YEAR" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="SKU" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DIVISION_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BRAND_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GENDER_ID" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_SEASON" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="GST_ON" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FileName" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="SKU_COUNTRY" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShowOnPOS" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel" />
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                      
                                  
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("SKU_ID" )%>' ToolTip="Edit">
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
</asp:Content>
