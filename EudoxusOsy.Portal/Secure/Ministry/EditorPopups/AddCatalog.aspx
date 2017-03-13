<%@ Page Title="Προσθήκη Διανομής" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.AddCatalog" CodeBehind="AddCatalog.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="CatalogInput" TagPrefix="my" Src="~/UserControls/GenericControls/CatalogInput.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <my:CatalogInput ID="ucCatalogInput" runat="server" ValidationGroup="vgCatalog" />

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgCatalog" />
    </div>

    <div class="br"></div>

    <dx:ASPxLabel ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgCatalog" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
