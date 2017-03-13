<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BooksGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.BooksGridView" %>


<dx:ASPxGridView ID="gvBooks" runat="server" DataSourceForceStandardPaging="true">
    <Columns>               
        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID"  />        
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Caption="BookKpsID"  />
        <dx:GridViewDataTextColumn FieldName="BookType" Caption="BookType"  />
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title"  />
        <dx:GridViewDataTextColumn FieldName="Subtitle" Caption="Subtitle"  />
        <dx:GridViewDataTextColumn FieldName="Author" Caption="Author"  />
        <dx:GridViewDataTextColumn FieldName="Publisher" Caption="Publisher"  />
        <dx:GridViewDataTextColumn FieldName="Pages" Caption="Pages"  />        
        <dx:GridViewDataTextColumn FieldName="ISBN" Caption="ISBN"  />        
        <dx:GridViewDataTextColumn FieldName="Comments" Caption="Comments"  />        
        <dx:GridViewDataTextColumn FieldName="FirstRegistrationYear" Caption="FirstRegistrationYear"  />        
        <dx:GridViewDataTextColumn FieldName="SupplierCode" Caption="SupplierCode"  />        
        <dx:GridViewDataTextColumn FieldName="IsActive" Caption="IsActive"  />        
        <dx:GridViewDataTextColumn FieldName="PendingCommitteePriceVerification" Caption="PendingCommitteePriceVerification"  />        
    </Columns>
</dx:ASPxGridView>


