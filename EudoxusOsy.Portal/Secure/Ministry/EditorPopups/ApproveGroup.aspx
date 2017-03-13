<%@ Page Title="Έγκριση για πληρωμή" MasterPageFile="~/PopUp.Master" CodeBehind="ApproveGroup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ApproveGroup" Language="C#" AutoEventWireup="true" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    
    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 50px" />
        </colgroup>
        <tr>
            <th class="header">&raquo; Σχόλια Έγκρισης
            </th>
        </tr>
        <tr>
            <td>
                <dx:ASPxMemo ID="txtApprovalComments" runat="server" Rows="7" />                
            </td>
        </tr>
    </table>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
