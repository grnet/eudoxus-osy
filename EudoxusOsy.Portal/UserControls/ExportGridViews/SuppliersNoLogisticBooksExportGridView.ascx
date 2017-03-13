<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuppliersNoLogisticBooksExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.SuppliersNoLogisticBooksExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvSuppliersNoLogisticBooks" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν μη υπόχρεοι λογιστικών βιβλίων
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="KpsID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Name" FieldName="SupplierName" Caption="Ονοματεπώνυμο">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="PublisherAddress" FieldName="PublisherAddress" Caption="Διεύθυνση">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="PublisherZipCode" FieldName="PublisherZipCode" Caption="Τ.Κ.">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="CityName" FieldName="CityName" Caption="Πόλη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="AFM" FieldName="AFM" Caption="ΑΦΜ">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DOY" Caption="ΔΟΥ">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="TotalAmount" FieldName="TotalAmount" Caption="Ποσό">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSuppliersNoLogisticBooks" runat="server" GridViewID="gvSuppliersNoLogisticBooks" OnRenderBrick="gveSuppliersNoLogisticBooks_RenderBrick" />