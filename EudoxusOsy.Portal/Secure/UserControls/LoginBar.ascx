<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.Secure.UserControls.LoginBar" CodeBehind="LoginBar.ascx.cs" %>

<asp:LoginView ID="loginView" runat="server">
    <AnonymousTemplate>
        Δεν έχετε συνδεθεί.
        <asp:LoginStatus ID="loginStatus" runat="server" />
    </AnonymousTemplate>
    <LoggedInTemplate>
        <asp:PlaceHolder ID="phUserDetails" runat="server">
            Έχετε συνδεθεί ως <a id="btnUserDetails" runat="server"><asp:LoginName ID="loginName" runat="server" /></a>
            &nbsp;&nbsp;&nbsp;
        </asp:PlaceHolder>
        <a id="btnChangePassword" runat="server" class="icon-btn bg-passwordEdit" href="javascript:void(0)" onclick="showChangePasswordPopup(true)">Αλλαγή Κωδικού Πρόσβασης</a>
        <asp:LinkButton ID="btnLogout" runat="server" CssClass="icon-btn bg-logout" Text="Αποσύνδεση" OnClick="btnLogout_Click" />
    </LoggedInTemplate>
</asp:LoginView>
