<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PriceVerification.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PriceVerification" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">    
    
    <p>Χειρισμός των βιβλίων που η τιμή του ελέγχεται από το Υπουργείο</p>
    <a href="PriceVerificationMinistry.aspx">Υπουργείο</a>
    
    <p>Χειρισμός βιβλίου (που δεν ελέγχεται από το Υπουργείο) όταν το ΚΠΣ επιστρέψει διαφορετική Τιμή Υπουργείου</p>
    <a href="PriceVerificationUnexpected.aspx">ΚΠΣ</a>
</asp:Content>