<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCatalogsGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.EditCatalogsGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvEditCatalogs" runat="server" 
    KeyFieldName="ID" 
    DataSourceForceStandardPaging="true" 
    OnCustomJSProperties="gvEditCatalogs_CustomJSProperties" 
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
        <dx:GridViewDataTextColumn FieldName="Supplier.SupplierKpsID" Name="SupplierKpsID" Caption="ID Εκδότη" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# Eval("Supplier.SupplierKpsID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn FieldName="Percentage" Name="Percentage" Caption="Ποσοστό" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Right" />
            <DataItemTemplate>
                <%# ((decimal?)Eval("Percentage")).HasValue ? ((decimal)Eval("Percentage")) + "%": string.Empty %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="IsForLibrary" Caption="Διανομή σε" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                <%# Eval("Department.SecretaryKpsID") != null ? "Φοιτητές": "Βιβλιοθήκη" %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department.SecretaryKpsID" Name="SecretariatID" Caption="ID Γραμματείας/ Βιβλιοθήκης" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                <%# Eval("Department.SecretaryKpsID") ?? Eval("Department.LibraryKpsID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Book.BookKpsID" Name="BookKpsID" Caption="ID Βιβλίου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# Eval("Book.BookKpsID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Αριθμός Αντιτύπων" Width="50px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Discount.DiscountPercentage" Name="Discount" Caption="Έκπτωση" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Right" />
            <DataItemTemplate>
                <%# ((decimal?)Eval("Discount.DiscountPercentage")).HasValue ? (100 - ((decimal)Eval("Discount.DiscountPercentage")) * 100) + "%": string.Empty %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Amount" Name="Amount" Caption="Κόστος Διανομής" Width="50px" PropertiesTextEdit-DisplayFormatString="C">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn Name="State" Caption="Στάδιο Κατάστασης">
            <DataItemTemplate>
                <%# GetGroupState((Catalog)Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Name="CreatedAt" Caption="Ημ/νία Δημιουργίας" Width="30px">
            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# ((Catalog)Container.DataItem).CreatedAt > new DateTime(1970, 1,2) ? ((Catalog)Container.DataItem).CreatedAt.ToShortDateString(): string.Empty %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Status" Caption="Status Διανομής">
            <HeaderStyle Wrap="true" />
            <DataItemTemplate>
                <%# ((Catalog)Container.DataItem).Status.GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn ToolTip="State Διανομής (κατάσταση)" FieldName="StateInt" Name="State" Caption="State Διανομής">
            <HeaderStyle Wrap="true" />
            <DataItemTemplate>
                <%# ((Catalog)Container.DataItem).State.GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="CatalogType" FieldName="CatalogTypeInt" Caption="Είδος Διανομής" Width="50px">
            <HeaderStyle Wrap="true" />
            <DataItemTemplate>
                <%# ((Catalog)Container.DataItem).CatalogType.GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="IsDeleteAllowed" Name="IsDeleteAllowed" Visible="false">
            <DataItemTemplate>
                <%# Eval("IsDeleteAllowed") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveEditCatalogs" runat="server" GridViewID="gvEditCatalogs" OnRenderBrick="gveEditCatalogs_RenderBrick" />
