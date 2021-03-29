<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceVerificationMinistryExportGrid.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.PriceVerificationMinistryExportGrid" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<dx:ASPxGridView ID="gvPriceVerification" runat="server" DataSourceForceStandardPaging="true">
    <Columns>                       
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Caption="Κωδικός Βιβλίου"  />        
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Τίτλος Βιβλίου"  />
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Caption="Ημερομηνία ενημέρωσης τιμής" >               
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SuggestedPrice" Caption="Προτεινόμενη τιμή Εκδότη" Settings-AllowSort ="False">                                     
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Price" Caption="Εγκεκριμένη τιμή Υπουργείου"  Settings-AllowSort ="False">                              
        </dx:GridViewDataTextColumn>                
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gvePriceVerification" runat="server" GridViewID="gvPriceVerification" OnRenderBrick="gvePriceVerification_RenderBrick" />
