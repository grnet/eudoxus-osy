<%@ Page Title="Κεντρική Σελίδα" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.Default" %>

<%@ Register TagName="MinistryView" TagPrefix="my" Src="~/UserControls/MinistryControls/ViewControls/MinistryView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">

    <my:MinistryView ID="ucMinistryView" runat="server" />

</asp:Content>
