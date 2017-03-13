<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="InsertNewCatalogPopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.InsertNewCatalogPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <table>
        <tr>
            <th>
                ID Βιβλίου
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtBookID">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Βιβλίου' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th>
                ID Γραμματείας/Βιβλιοθήκης
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtSecretary">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Γραμματείας/Βιβλίοθήκης' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th>
               ID Εκδότη
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtSupplier">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Εκδότη' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th>
                Φάση
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtPhase">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Φάση' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th>Αριθμός Βιβλίων</th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtBookCount">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Αριθμός Βιβλίων' είναι υποχρεωτικό"></ValidationSettings>
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
