<%@ Page Title="Προσθήκη Τράπεζας" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.AddBank" CodeBehind="AddBank.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="BankInput" TagPrefix="my" Src="~/UserControls/GenericControls/BankInput.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <my:BankInput ID="ucBankInput" runat="server" ValidationGroup="vgBank" />

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgBank" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgBank" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
