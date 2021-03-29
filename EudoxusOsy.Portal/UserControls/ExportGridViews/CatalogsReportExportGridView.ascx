<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogsReportExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.CatalogsReportExportGridView" %>

<dx:ASPxGridView ID="gvCatalogsReport" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="BookTitle" Name="BookTitle" Caption="Βιβλίο" Width="30px">            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Name="BookKpsID">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierName" Name="SupplierName" Caption="Εκδότης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="SupplierKpsID" >
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department" Name="Department" Caption="Τμήμα">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="LibraryKpsID" Name="LibraryKpsID">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SecretaryKpsID" Name="SecretaryKpsID">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="FinalAmount" Name="FinalAmount" Caption="Τελικό Ποσό">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Πλήθος βιβλίων">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PricePerBook" Name="PricePerBook" Caption="Τιμή βιβλίου">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="FirstRegistrationYear" Name="FirstRegistrationYear" Caption="Έτος πρώτης κυκλοφορίας">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DiscountPercentage" Name="DiscountPercentage" Caption="Ποσοστό έκπτωσης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Percentage" Name="Percentage" Caption="Ποσοστό εκδότη">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveCatalogsReport" runat="server" GridViewID="gvCatalogsReport" OnRenderBrick="gveCatalogsReport_RenderBrick" />

