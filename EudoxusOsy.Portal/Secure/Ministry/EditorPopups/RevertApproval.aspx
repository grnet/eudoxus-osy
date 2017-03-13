<%@ Page Title="Επαναφορά σε Επιλεγμένη για Πληρωμή" MasterPageFile="~/PopUp.Master" CodeBehind="RevertApproval.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.RevertApproval" Language="C#" AutoEventWireup="true" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    
    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 50px" />
        </colgroup>
        <tr>
            <th class="header">&raquo; Σχόλια Επαναφοράς
            </th>
        </tr>
        <tr>
            <td>
                <dx:ASPxMemo ID="txtRevertComments" runat="server" Rows="7" />                
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
