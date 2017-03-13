<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="RefreshCache.aspx.cs" Inherits="EudoxusOsy.Portal.Admin.RefreshCache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Button ID="btnRefreshCache" runat="server" Text="Refresh Cache" OnClick="btnRefreshCache_Click" />    
</asp:Content>
