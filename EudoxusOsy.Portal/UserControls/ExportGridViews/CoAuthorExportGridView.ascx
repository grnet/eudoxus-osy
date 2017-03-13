<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoAuthorExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.CoAuthorExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvCoAuthors" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν συνεκδότες
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="KpsID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Επωνυμία Εκδότη" Width="80px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="SupplierType" FieldName="supplierType" Caption="Τύπος Εκδότη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="Percentage" FieldName="percentage" Caption="Ποσοστό (%)">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="ID Βιβλίου" FieldName="BookKpsID" Caption="ID Βιβλίου">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="Title" FieldName="Title" Caption="Τίτλος Βιβλίου">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="Author" FieldName="Author" Caption="Συγγραφέας">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="isbn" Caption="ISBN">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="SupplierCode" FieldName="SupplierCode" Caption="ID Διαθέτη">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="RegisteredSupplier" FieldName="Book_publisherInBook" Caption="Επωνυμία Διαθέτη του βιβλίου">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveCoAuthors" runat="server" GridViewID="gvCoAuthors" OnRenderBrick="gveCoAuthors_RenderBrick" />