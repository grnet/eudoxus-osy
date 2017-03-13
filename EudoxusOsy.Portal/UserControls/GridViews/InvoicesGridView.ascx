<%@ Control CodeBehind="InvoicesGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.InvoicesGridView" Language="C#" AutoEventWireup="true"  %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvInvoices" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn Caption="Α/Α" Width="50px">
                <HeaderStyle HorizontalAlign="Center" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <%# Container.ItemIndex + 1 %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InvoiceNumber" Caption="Αριθμός Παραστατικού" />            
        <dx:GridViewDataTextColumn FieldName="InvoiceDate" Name="InvoiceDate" Caption="Ημ/νία Παραστατικού" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="InvoiceValue" Name="Amount" Caption="Ποσό (χωρίς Φ.Π.Α.)" PropertiesTextEdit-DisplayFormatString="c" />        
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveInvoices" runat="server" GridViewID="gvInvoices" OnRenderBrick="gveInvoices_RenderBrick" />
