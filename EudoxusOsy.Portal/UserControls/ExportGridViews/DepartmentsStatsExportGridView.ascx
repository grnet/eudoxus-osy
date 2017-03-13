<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentsStatsExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.DepartmentsStatsExportGridView" %>

<dx:ASPxGridView ID="gvDepartments" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν υπάρχουν δεδομένα
        </EmptyDataRow>
    </Templates>
    <Columns>  
        <dx:GridViewDataTextColumn Name="InstName" FieldName="InstName" Caption="Τίτλος Ιδρύματος" />
        <dx:GridViewDataTextColumn Name="DepName" FieldName="DepName" Caption="Τίτλος Τμήματος" />
        <dx:GridViewDataTextColumn Name="Debt" FieldName="Debt" Caption="Κόστος Τμήματος" />
        <dx:GridViewDataTextColumn Name="PricedCount" FieldName="PricedCount" Caption="Κοστολογημένα βιβλία" />
        <dx:GridViewDataTextColumn Name="NotPricedCount" FieldName="NotPricedCount" Caption="Μη Κοστολογημένα βιβλία" />       
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveDepartments" runat="server" GridViewID="gvDepartments" OnRenderBrick="gveDepartments_RenderBrick" />
