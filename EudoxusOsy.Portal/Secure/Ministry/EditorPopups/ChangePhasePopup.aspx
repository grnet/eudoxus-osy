<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ChangePhasePopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ChangePhasePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <table>
        <tr>
            <th>Μεταφορά στη φάση: </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtNewPhaseID">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Νέα Φάση' είναι υποχρεωτικό"></ValidationSettings>
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
