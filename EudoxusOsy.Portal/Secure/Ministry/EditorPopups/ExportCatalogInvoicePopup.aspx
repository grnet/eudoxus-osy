<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ExportCatalogInvoicePopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ExportCatalogInvoicePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <table>
        <tr>
            <th>Σχόλια: </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtComments">
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmitHidden.DoClick();
        }
    </script>

</asp:Content>
