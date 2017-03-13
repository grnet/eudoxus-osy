<%@ Page Title="Προσθήκη Παραστατικού" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.AddInvoice" CodeBehind="AddInvoice.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="InvoiceInput" TagPrefix="my" Src="~/UserControls/InvoiceControls/InputControls/InvoiceInput.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">    

    <my:InvoiceInput ID="ucInvoiceInput" runat="server" ValidationGroup="vgInvoice" />

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgInvoice" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgInvoice" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
