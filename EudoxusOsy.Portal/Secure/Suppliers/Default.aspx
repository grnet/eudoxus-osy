<%@ Page Title="Κεντρική Σελίδα" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.Default" %>

<%@ Register TagName="SupplierView" TagPrefix="my" Src="~/UserControls/SupplierControls/ViewControls/SupplierView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">

    <my:SupplierView ID="ucSupplierView" runat="server" />

</asp:Content>
