<%@ Page Title="Στατιστικά εκδοτών" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="SuppliersStats.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.SuppliersStats" %>

<%@ Register TagPrefix="my" TagName="SuppliersSearchFilters" Src="~/UserControls/SearchFilters/SupplierSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersGridView" Src="~/UserControls/GridViews/SuppliersGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersExportGridView" Src="~/UserControls/ExportGridViews/SuppliersExportGridView.ascx" %>


<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <div style="text-align: left">
        <dx:ASPxButton runat="server" ID="btnExportSuppliersStats" Text="Εξαγωγή στατιστικών εκδοτών" ToolTip="Εξαγωγή στατιστικών εκδοτών" Image-Url="~/_img/iconExcel.png">
            <ClientSideEvents Click="showExportSupplierStats" />
        </dx:ASPxButton>
    </div>
    <div style="text-align: right">
        <a href="AcademicsStats.aspx">Στατιστικά Ιδρυμάτων</a>
    </div>
    <my:SuppliersSearchFilters ID="ucSearchFilters" runat="server" Mode="Stats"/>

    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxGridView ID="gvSuppliersStats" runat="server" DataSourceID="odsSuppliers" 
        DataSourceForceStandardPaging="true" OnCustomCallback="gvSuppliersStats_CustomCallback">
        <Columns>
            <dx:GridViewDataTextColumn Caption="InnerID" FieldName="supplier_id" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="supplier_kpsid"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Επωνυμία" FieldName="official_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΔΟΥ" FieldName="paymentPfo" Settings-AllowSort="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΑΦΜ" FieldName="taxRoll_number" Settings-AllowSort="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Υπεύθυνος Επικοινωνίας" FieldName="contact_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τύπος Εκδότη" FieldName="supplierType" Settings-AllowSort="false">
                <DataItemTemplate>
                    <%# ((enSupplierType)((SupplierFullStatistics)Container.DataItem).supplierType).GetLabel() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Συνολικό Ποσό" FieldName="totalprice"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ποσό Φάσης" FieldName="totalofferprice"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ποσό προς ΥΔΕ" FieldName="totaltoyde"></dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>

    <my:SuppliersExportGridView ID="gvSuppliersExport" Visible="false" runat="server" DataSourceForceStandardPaging="false">
    </my:SuppliersExportGridView>


    <asp:ObjectDataSource ID="odsSuppliers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Suppliers" CacheDuration="600"
        SelectMethod="GetSuppliersStats" SelectCountMethod="CountResult" MaximumRowsParameterName="maximumRows" OnSelecting="odsSuppliers_Selecting"
         StartRowIndexParameterName="startRowIndex"
        EnablePaging="true" SortParameterName="sortExpression">
        <SelectParameters>
            <asp:Parameter Name="supplierKpsID" Type="Int32" />
            <asp:Parameter Name="afm" Type="String" />
            <asp:Parameter Name="supplierType" Type="Int32" />
            <asp:Parameter Name="name" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
