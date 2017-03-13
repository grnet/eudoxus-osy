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
<%--        <dx:GridViewDataTextColumn FieldName="paymentPfo" Name="paymentPfo" Caption="ΔΟΥ" />
        <dx:GridViewDataTextColumn FieldName="taxRoll_number" Name="taxRoll_number" Caption="Α.Φ.Μ." />--%>
        <dx:GridViewDataTextColumn FieldName="supplierType" Name="supplierType" Caption="Τύπος Εκδότη">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="books_registered" Name="books_registered" Caption="Βιβλία που έχουν δηλωθεί">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totalPrice" Name="totalPrice" Caption="΄Σύνολο Οφειλής">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totalpayment" Name="totalpayment" Caption="Ποσό Φάσης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="totaltoyde" Name="totaltoyde" Caption="Ποσό προς ΥΔΕ">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="books_registeredNOTPRICED" Name="books_registeredNOTPRICED" Caption="Μη κοστολογημένα Βιβλία που έχουν δηλωθεί">
        </dx:GridViewDataTextColumn>

    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSuppliers" runat="server" GridViewID="gvSuppliers" OnRenderBrick="gveSuppliers_RenderBrick" />
