<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCatalogsExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.EditCatalogsExportGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal.UserControls.GridViews" %>

<dx:ASPxGridView ID="gvEditCatalogs" runat="server" 
    KeyFieldName="ID" 
    DataSourceForceStandardPaging="true" 
    SettingsPager-PageSize="20">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="ID" Name="CatalogID" Caption="ID Διανομής" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="GroupID" Name="GroupID" Caption="ID Κατάστασης" Width="50px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PhaseID" Name="PhaseID" Caption="Φάση" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SupplierKpsID" Name="SupplierKpsID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />            
        </dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn FieldName="Percentage" Name="Percentage" Caption="Ποσοστό" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Right" />           
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="IsForLibrary" Caption="Διανομή σε" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />              
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SecretaryOrLibaryID" Name="SecretaryOrLibaryID" Caption="ID Γραμματείας/ Βιβλιοθήκης" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />                
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Name="BookKpsID" Caption="ID Βιβλίου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Αριθμός Αντιτύπων" Width="50px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DiscountPercentage" Name="Discount" Caption="Έκπτωση" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Right" />           
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Amount" Name="Amount" Caption="Κόστος Διανομής" Width="50px" PropertiesTextEdit-DisplayFormatString="C">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="GroupState" Name="GroupState" Caption="Στάδιο Κατάστασης">            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Name="CreatedAt" Caption="Ημ/νία Δημιουργίας" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
            <CellStyle HorizontalAlign="Center" />            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Status" Name="Status" Caption="Status Διανομής">
            <HeaderStyle Wrap="true" />            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn ToolTip="State Διανομής (κατάσταση)" FieldName="State" Name="State" Caption="State Διανομής">
            <HeaderStyle Wrap="true" />            
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="CatalogType" FieldName="CatalogType" Caption="Είδος Διανομής" Width="50px">
            <HeaderStyle Wrap="true" />            
        </dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn Name="IsLocked" Caption="Κλειδωμένη">
            <HeaderStyle Wrap="true" />                                      
        </dx:GridViewDataTextColumn>               
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveEditCatalogs" runat="server" GridViewID="gvEditCatalogs" OnRenderBrick="gveEditCatalogs_OnRenderBrick" />