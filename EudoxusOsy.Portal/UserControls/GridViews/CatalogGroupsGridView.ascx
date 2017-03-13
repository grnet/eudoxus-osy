<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogGroupsGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.CatalogGroupsGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvCatalogGroups" runat="server" DataSourceForceStandardPaging="true">    
    <Columns>
        <dx:GridViewDataTextColumn FieldName="ID" Name="ID" Caption="ID Κατάστασης" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="InstitutionID" Name="InstitutionID" Caption="Ίδρυμα">
            <DataItemTemplate>
                <%# CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="CatalogCount" Name="CatalogCount" Caption="Αριθμός Διανομών" Width="50px">
            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="TotalAmount" Name="TotalAmount" Caption="Χρηματικό Ποσό (χωρίς Φ.Π.Α.)" Width="80px" PropertiesTextEdit-DisplayFormatString="C">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="GroupStateInt" Name="GroupState" Caption="Στάδιο Κατάστασης" Width="100px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# ((enCatalogGroupState)Eval("GroupStateInt")).GetLabel() + (((enCatalogGroupState)Eval("GroupStateInt")) == enCatalogGroupState.Sent ?  " ("+ (((CatalogGroupInfo)Container.DataItem).OfficeSlipDate.HasValue ? ((CatalogGroupInfo)Container.DataItem).OfficeSlipDate.Value.ToShortDateString() : string.Empty) +")": string.Empty) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveCatalogGroups" runat="server" GridViewID="gvCatalogGroups" OnRenderBrick="gveCatalogGroups_RenderBrick" />
