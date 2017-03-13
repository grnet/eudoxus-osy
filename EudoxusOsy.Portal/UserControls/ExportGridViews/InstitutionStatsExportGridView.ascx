<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstitutionStatsExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.InstitutionStatsExportGridView" %>

<dx:ASPxGridView ID="gvInstitutions" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν υπάρχουν δεδομένα
        </EmptyDataRow>
    </Templates>
    <Columns>        
        <dx:GridViewDataTextColumn Name="InstName" FieldName="InstName" Caption="Τίτλος Ιδρύματος" />
        <dx:GridViewDataTextColumn Name="Debt" FieldName="Debt" Caption="Κόστος Ιδρύματος" />
        <dx:GridViewDataTextColumn Name="PricedCount" FieldName="PricedCount" Caption="Κοστολογημένα βιβλία" />
        <dx:GridViewDataTextColumn Name="NotPricedCount" FieldName="NotPricedCount" Caption="Μη Κοστολογημένα βιβλία" />       
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveInstitutions" runat="server" GridViewID="gvInstitutions" OnRenderBrick="gveInstitutions_RenderBrick" />
