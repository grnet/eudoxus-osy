<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncomingTransfersInvoicesGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.IncomingTransfersInvoicesGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvIncomingTransfers" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="InvoiceNumber" Caption="Αριθμός Παραστατικού" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InvoiceDate" Name="InvoiceDate" Caption="Ημ/νία Παραστατικού" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="InvoiceValue" Name="Amount" Caption="Ποσό" PropertiesTextEdit-DisplayFormatString="c" />
        <dx:GridViewDataTextColumn Name="BankName" Caption="Τράπεζα">   
            <DataItemTemplate>
                <%# EudoxusOsyCacheManager<Bank>.Current.Get(((BankTransfer)Container.DataItem).BankID).Name %>
            </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="GroupID" Name="GroupID" Caption="ID Κατάστασης" />
        <dx:GridViewDataTextColumn FieldName="IsPaid" Name="IsPaid" Caption="Πληρωθέν" />
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveIncomingTransfers" runat="server" GridViewID="gvIncomingTransfers" OnRenderBrick="gveIncomingTransfers_RenderBrick" />
