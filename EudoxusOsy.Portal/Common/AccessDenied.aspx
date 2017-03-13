<%@ Page Language="C#" MasterPageFile="~/Portal.master" AutoEventWireup="true"
    CodeBehind="AccessDenied.aspx.cs" Inherits="EudoxusOsy.Portal.Common.AccessDenied"
    Title="Απαγορεύεται η πρόσβαση" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h2>Απαγορεύεται η πρόσβαση</h2>
    <p>Απαγορεύεται η πρόσβαση σε αυτή τη σελίδα</p>
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
