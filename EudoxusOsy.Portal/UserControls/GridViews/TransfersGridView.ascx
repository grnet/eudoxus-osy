<%@ Control CodeBehind="TransfersGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.TransfersGridView" Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvTransfers" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn Caption="Α/Α" Name="aa" Width="50px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# Container.ItemIndex + 1 %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InvoiceNumber" Caption="Αριθμός Παραστατικού" Width="50px">
            <HeaderStyle HorizontalAlign="Right" Wrap="True" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InvoiceDate" Name="InvoiceDate" Caption="Ημ/νία Παραστατικού" PropertiesTextEdit-DisplayFormatString="dd/MM/yyyy" Width="50px">
            <HeaderStyle HorizontalAlign="Right" Wrap="True" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InvoiceValue" Name="Amount" Caption="Ποσό" PropertiesTextEdit-DisplayFormatString="c" Width="50px">
            <HeaderStyle HorizontalAlign="Right" />
            <CellStyle HorizontalAlign="Right" Wrap="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Supplier.SupplierKpsID" Name="Bank" Caption="ID Εκδότη" Width="50px">
            <DataItemTemplate>
                <%# Eval("Supplier.SupplierKpsID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Supplier.Name" Name="Bank" Caption="Εκδότης" Width="200px">
            <DataItemTemplate>
                <%# Eval("Supplier.Name") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Bank.Name" Name="Bank" Caption="Τράπεζα" Width="200px">
            <DataItemTemplate>
                <%# Eval("Bank.Name") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Phase.AcademicYearAndSemester" Name="PhaseID" Caption="Περίοδος Πληρωμών" Width="80px">            
            <DataItemTemplate>
                <%# Eval("Phase.AcademicYearAndSemester") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveTransfers" runat="server" GridViewID="gvTransfers" OnRenderBrick="gveTransfers_RenderBrick" />
