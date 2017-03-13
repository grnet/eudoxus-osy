<%@ Control CodeBehind="FileSelfPublisherGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.FileSelfPublisherGridView" Language="C#" AutoEventWireup="true"  %>

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
        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID Αρχείου" />            
        <dx:GridViewDataTextColumn Name="FileName" Caption="Όνομα Αρχείου">
            <DataItemTemplate>
                <%# CreateLink((FileSelfPublisherInfo)Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PhaseID" Name="PhaseID" Caption="Φάση Πληρωμών"  />        
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveFileSelfPublisher" runat="server" GridViewID="gvFileSelfPublisher" OnRenderBrick="gveFileSelfPublisher_RenderBrick" />
