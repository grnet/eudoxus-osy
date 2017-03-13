<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.HyperLinkInput" CodeBehind="HyperLinkInput.ascx.cs" %>

<asp:MultiView ID="mvModeType" runat="server" ActiveViewIndex="0">
    <asp:View ID="vEditMode" runat="server">
        <dx:ASPxTextBox ID="txtHyperLink" runat="server" MaxLength="500" CssClass="hint">
            <ValidationSettings RegularExpression-ErrorText="Η ιστοσελίδα δεν είναι έγκυρη (παράδειγμα σωστής ιστοσελίδας http://www.mywebpage.gr)" />                
        </dx:ASPxTextBox>
    </asp:View>
    <asp:View ID="vViewMode" runat="server">
        <a id="lnkHyperLink" runat="server" target="_blank" href="#" />
    </asp:View>
</asp:MultiView>