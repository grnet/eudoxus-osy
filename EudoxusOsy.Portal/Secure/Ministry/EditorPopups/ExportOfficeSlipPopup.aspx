<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ExportOfficeSlipPopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ExportOfficeSlipPopup" %>
<%@ Register TagPrefix="my" TagName="OfficeSlipExportGridView" Src="~/UserControls/ExportGridViews/OfficeSlipExportGridView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <table>
        <tr>
            <th>Ημερομηνία αποστολής προς ΥΔΕ: </th>
            <td>
                <dx:ASPxDateEdit runat="server" ID="dateSentAt">
                    <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="vgExport" RequiredField-ErrorText="Το πεδίο 'Ημερομηνία αποστολής προς ΥΔΕ' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr runat="server" id="trProtocol">
            <th>Αριθμός πρωτοκόλλου</th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtProtocolNumber">
                    <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="vgExport" RequiredField-ErrorText="Το πεδίο 'Αριθμός Πρωτοκόλλου' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr runat="server" id="trDecision">
            <th>
                Αριθμός Απόφασης
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtDecision">
                    <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="vgExport" RequiredField-ErrorText="Το πεδίο 'Αριθμός Απόφασης' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />

    <my:OfficeSlipExportGridView runat="server" GridClientVisible="false" ID="ucOfficeSlipExportGridView"></my:OfficeSlipExportGridView>

    <script type="text/javascript">
        function doSubmit(s, e) {
            if (ASPxClientEdit.ValidateGroup("vgExport")) {
                btnSubmitHidden.DoClick();
            }
        }
    </script>

</asp:Content>
