<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true"
    CodeBehind="LoginAsUser.aspx.cs" Inherits="EudoxusOsy.Portal.Admin.LoginAsUser"
    Title="Σύνδεση ως Χρήστης" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:TextBox ID="txtUsername" runat="server" />
    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
        ErrorMessage="Το όνομα χρήστη είναι απαραίτητο" Display="Dynamic" />
    <asp:CustomValidator ID="cvUsername" runat="server" OnServerValidate="cvUsername_ServerValidate"
        ErrorMessage="Ο χρήστης δεν βρέθηκε. Παρακαλούμε ελέγξτε το username." Display="Dynamic" />
    <asp:LinkButton runat="server" ID="lbtnLogin" Text="Είσοδος ως άλλος χρήστης" OnClick="lbtnLogin_Click" />
</asp:Content>
