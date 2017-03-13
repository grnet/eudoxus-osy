<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeSlipExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.OfficeSlipExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvOfficeSlip" runat="server">
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν Εντολές Πληρωμής για την ημερομηνία που επιλέξατε.
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="GroupID" Name="GroupID" Caption="Κωδικός κατάστασης" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="OfficialName" FieldName="CatalogGroup.Supplier.Name" Caption="Επωνυμία">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="SupplierKpsID" FieldName="CatalogGroup.Supplier.SupplierKpsID" Caption="ID Εκδότη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="AFM" FieldName="CatalogGroup.Supplier.AFM" Caption="ΑΦΜ">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="PaymentOffice" FieldName="PaymentOffice" Caption="Ταμείο">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="TotalAmount" FieldName="TotalAmount" Caption="Ποσό (με ΦΠΑ)">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveOfficeSlip" runat="server" GridViewID="gvOfficeSlip" OnRenderBrick="gveOfficeSlip_RenderBrick"/>