<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Forms_Default2" Title="CORN ::Default2" %>
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

<style>
.page {
border-collapse: collapse;
}
#page{
border-collapse: collapse;
}
/* And this to your table's `td` elements. */
.page td {
   padding: 0; 
   margin: 0;
}
#page td {
   padding: 0; 
   margin: 0;
}
.form-control{
      
    margin-bottom:0 !important;
}
/*tr{
    display:flex;
}*/
th{
      white-space: nowrap;
}

</style>
        <table id="page"  class="table table-bordered cf">
        <thead>
        <tr>
        <th>Sr no</th>
        <th>Items</th>
        <th>Size & Description</th>
        <th>Vendor Name</th>
        <th>QTY</th>
        <th>Rate</th>
        <th>Internal Cost</th>
        <th>Margin</th>
        <th>External Cost</th>
        <th>Status</th>
        </tr>
        </thead>

        <tbody>
        <tr>
        <td style="padding:7px">
            1
        </td>
        <td>
        <select class="form-control" aria-label="Default select example">
        <option >Select Items</option>
        <option value="1">Entrance Decor</option>
        <option value="2">Stage</option>
        <option value="3">Walkway</option>
        <option value="3">Decore</option>
        <option value="3">Main Area Sitting</option>
        <option value="3">Flower Making Material</option>
        <option value="3">Dining</option>
        <option value="3">Lightning Generator</option>
        <option value="3">Logistics</option>
        </select>
        </td>
        <td>
            <input type="text" class="form-control" aria-label="Phone Number" placeholder="Size and Description">
        </td>
        <td>
        <select class="form-control" aria-label="Default select example">
        <option >Select Vendor</option>
        <option value="1">Ali</option>
        <option value="2">Aqib</option>
        <option value="3">Khurram</option>
        </select>
        </td>
        <td>
            <input type="text" class="form-control" aria-label="QTY" placeholder="QTY">
        </td>
        <td>
            <input type="text" class="form-control" aria-label="Rate" placeholder="Rate">
        </td>
        <td>
            <input type="text" class="form-control" aria-label="Internal Cost" placeholder="Internal Cost">
        </td>
        <td>
            <input type="text" class="form-control" aria-label="Margin" placeholder="Margin">
        </td>
        <td>
            <input type="text" class="form-control" aria-label="External Code" placeholder="External Code">
        </td>
        <td>
            <select class="form-control" aria-label="Default select example">
        <option value="1" >Pending</option>
        <option value="2">Placed</option>
        </select>
        </td>
        </tr>
        </tbody>
        </table>


    <div class="row" style="display:flex; justify-content:end;margin-top:50px ;margin-bottom:50px">
           <div class="col-md-offset-5 col-md-3">
                   <div class="btnlist pull-right">

            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
                <%--OnClick="btnAdd_Click"--%>

                    <button class="btn btn-warning"  ID="btnAdd" Text="Add">
                      <span class="fa fa-plus-circle"></span>Add
                     </button>
               </div>
           </div>
        </div>


        <table class=" page table table-striped table-bordered table-hover cf">
        <thead>
        <tr>
        <th>Sr no</th>
        <th>Items</th>
        <th>Size & Description</th>
        <th>Vendor Name</th>
        <th>QTY</th>
        <th>Rate</th>
        <th>Internal Cost</th>
        <th>Margin</th>
        <th>External Cost</th>
        <th>Status</th>
        </tr>
        </thead>

        <tbody>
        <tr>
        <td style="padding:7px">
            1
        </td>
        <td>
            Entrance Decore
        </td>
        <td>
            25 feet Good 
        </td>
        <td>
        Ali Imran
        </td>
        <td>
            25 pieces
        </td>
        <td>
            40,000
        </td>
        <td>
            40,000
        </td>
        <td>
            30%
        </td>
        <td>
            19500
        </td>
        <td>
        Pending
        </td>
        </tr>
        <tr>
        <td style="padding:7px">
            2
        </td>
        <td>
            Entrance Decore
        </td>
        <td>
            28 feet Good 
        </td>
        <td>
        Zubair Imran
        </td>
        <td>
            50 Pieces
        </td>
        <td>
            970,000
        </td>
        <td>
            970,000
        </td>
        <td>
            50%
        </td>
        <td>
            29500
        </td>
        <td>
        Placed
        </td>
        </tr>
        </tbody>
        </table>

</asp:Content>