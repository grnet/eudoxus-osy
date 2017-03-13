<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuppliersExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.SuppliersExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvSuppliers" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="ID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Επωνυμία" />
        <dx:GridViewDataTextColumn FieldName="TradeName" Name="TradeName" Caption="Διακριτικός Τίτλος" />
        <dx:GridViewDataTextColumn FieldName="AFM" Name="AFM" Caption="Α.Φ.Μ." />
        <dx:GridViewDataTextColumn FieldName="ContactName" Name="ContactName" Caption="Υπεύθυνος Επικοινωνίας">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierType" Name="SupplierType" Caption="Τύπος Εκδότη">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierStatus" Name="SupplierStatus" Caption="Κατάσταση">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="NoLogisticBooks" Name="NoLogisticBooks" Caption="Μη Υπόχρεος τήρησης τιμολογίων">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PaymentDOY" Name="PaymentDOY" Caption="ΔΟΥ Πληρωμών">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="IBAN" Name="IBAN" Caption="IBAN">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Address" Name="Address" Caption="Διεύθυνση">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ZipCode" Name="ZipCode" Caption="Τ.Κ.">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Telephone" Name="Telephone" Caption="Τηλέφωνο">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Fax" Name="Fax" Caption="Fax">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Email" Name="Email" Caption="Email">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Url" Name="Url" Caption="Ηλ. Διεύθυνση">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSuppliers" runat="server" GridViewID="gvSuppliers" OnRenderBrick="gveSuppliers_RenderBrick" />
