<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuppliersStatsExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.SuppliersStatsExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvSuppliers" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="ID" Name="ID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Επωνυμία">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="supplierType" Name="supplierType" Caption="Τύπος Εκδότη">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="booksPriced" Name="books_registered" Caption="Βιβλία που έχουν δηλωθεί">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totalPrice" Name="totalPrice" Caption="Οφειλή φάσης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totalpayment" Name="totalpayment" Caption="Ανάθεση ποσού φάσης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totaltoyde" Name="totaltoyde" Caption="Ποσό προς ΥΔΕ">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="booksNotPriced" Name="books_registeredNOTPRICED" Caption="Μη κοστολογημένα Βιβλία που έχουν δηλωθεί">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phaseId" Name="phaseId" Caption="Φάση">
        </dx:GridViewDataTextColumn>

    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSuppliers" runat="server" GridViewID="gvSuppliers" OnRenderBrick="gveSuppliers_RenderBrick" />
