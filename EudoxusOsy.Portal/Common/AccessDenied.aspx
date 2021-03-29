<%@ Page Language="C#" MasterPageFile="~/Portal.master" AutoEventWireup="true"
    CodeBehind="AccessDenied.aspx.cs" Inherits="EudoxusOsy.Portal.Common.AccessDenied"
    Title="Δεν έχετε δικαίωμα πρόσβασης" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h2>Δεν έχετε δικαίωμα πρόσβασης</h2>
    <p>
        Δεν έχετε δικαίωμα πρόσβασης σε αυτή τη σελίδα. 
        Για περισσότερες πληροφορίες απευθυνθείτε στο Γραφείο Αρωγής Χρηστών της υπηρεσίας Εύδοξος.
    </p>
    <asp:LoginView ID="LoginView1" runat="server">
        <LoggedInTemplate>
            <p>
                Έχετε συνδεθεί ως
                <asp:LoginName ID="LoginName1" runat="server" />
                <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutPageUrl="~/Default.aspx" 
                    OnLoggingOut="LoginStatus1_OnLoggingOut" OnLoggedOut="LoginStatus1_LoggedOut" />
            </p>
        </LoggedInTemplate>
    </asp:LoginView>
</asp:Content>
