<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommitmentsRegistryExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.CommitmentsRegistryExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvCommitmentsRegistry" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν δεσμεύσεις
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="InvoiceNumber" Name="InvoiceNumber" Caption="Αριθμός παραστατικού" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="InvoiceDate" FieldName="InvoiceDate" Caption="Ημ/νία παραστατικού">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="ID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Name" FieldName="Name" Caption="Επωνυμία Εκδότη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="GroupID" FieldName="GroupID" Caption="ID Κατάστασης">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="InvoiceValue" FieldName="InvoiceValue" Caption="Αξία παραστατικού">
        </dx:GridViewDataTextColumn>                                 
        <dx:GridViewDataTextColumn Name="CatalogsAmount" FieldName="CatalogsAmount" Caption="Κόστος Κατάστασης">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="GroupState" FieldName="GroupState" Caption="Στάδιο Κατάστασης">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="OfficeSlipDate" FieldName="OfficeSlipDate" Caption="Ημ/νία Διαβιβαστικού">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PhaseID" Name="PhaseID" Caption="Περίοδος Πληρωμών">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="AFM" FieldName="AFM" Caption="ΑΦΜ">
        </dx:GridViewDataTextColumn> 
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveCommitmentsRegistry" runat="server" GridViewID="gvCommitmentsRegistry" OnRenderBrick="gveCommitmentsRegistry_RenderBrick" />
