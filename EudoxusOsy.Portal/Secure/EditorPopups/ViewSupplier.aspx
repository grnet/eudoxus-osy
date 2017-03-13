<%@ Page Title="Προβολή Εκδότη" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewSupplier.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ViewSupplier" %>

<%@ Register TagName="SupplierMinistryView" TagPrefix="my" Src="~/UserControls/SupplierControls/ViewControls/SupplierMinistryView.ascx" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <my:SupplierMinistryView ID="ucSupplierMinistryView" runat="server" />
    
    <dx:ASPxButton  ClientVisible="false" runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" OnClick="btnSubmitHidden_Click"></dx:ASPxButton>

    <script type="text/javascript">
        function doSubmit() {
            btnSubmitHidden.DoClick();
        }
    </script>
</asp:Content>
