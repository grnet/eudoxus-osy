<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuppliersGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.SuppliersGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvSuppliers" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="ID" Caption="ID" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Επωνυμία" />
        <dx:GridViewDataTextColumn FieldName="AFM" Name="AFM" Caption="Α.Φ.Μ." Width="60px" />
        <dx:GridViewDataTextColumn FieldName="Reporter.ContactName" Name="ContactName" Caption="Υπεύθυνος Επικοινωνίας">
            <DataItemTemplate>
                <%# Eval("Reporter.ContactName") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierTypeInt" Name="SupplierTypeInt" Caption="Τύπος Εκδότη" Width="90px">
            <DataItemTemplate>
                <%# ((enSupplierType)Eval("SupplierTypeInt")).GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="StatusInt" Name="SupplierStatusInt" Caption="Κατάσταση" Width="60px">
            <DataItemTemplate>
                <%# ((enSupplierStatus)Eval("StatusInt")).GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSuppliers" runat="server" GridViewID="gvSuppliers" OnRenderBrick="gveSuppliers_RenderBrick" />
