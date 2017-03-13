<%@ Page Title="Αλλαγές IBAN" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ViewIbanChanges" CodeBehind="ViewIbanChanges.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <dx:ASPxGridView ID="gvSupplierIBANs" runat="server">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="IBAN" Caption="IBAN" />
            <dx:GridViewDataTextColumn FieldName="CreatedAt" Caption="Ημ/νία Μεταβολής" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyy HH:mm}" />
            <dx:GridViewDataTextColumn FieldName="CreatedBy" Caption="Μεταβολή Από" SortIndex="0" SortOrder="Ascending" />
        </Columns>
    </dx:ASPxGridView>

</asp:Content>
