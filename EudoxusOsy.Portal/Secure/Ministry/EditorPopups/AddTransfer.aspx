<%@ Page Title="Προσθήκη Εκχώρησης" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.AddTransfer" CodeBehind="AddTransfer.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="TransferInput" TagPrefix="my" Src="~/UserControls/GenericControls/TransferInput.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <my:TransferInput ID="ucTransferInput" runat="server" ValidationGroup="vgTransfer" />

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgTransfer" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgTransfer" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
