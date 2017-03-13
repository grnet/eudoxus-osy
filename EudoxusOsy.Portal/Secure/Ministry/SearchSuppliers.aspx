<%@ Page Title="Διανομείς Συγγραμμάτων" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="SearchSuppliers.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.SearchSuppliers" %>

<%@ Register TagPrefix="my" TagName="SuppliersSearchFilters" Src="~/UserControls/SearchFilters/SupplierSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersGridView" Src="~/UserControls/GridViews/SuppliersGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersExportGridView" Src="~/UserControls/ExportGridViews/SuppliersExportGridView.ascx" %>


<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <div style="text-align: left">
        <dx:ASPxButton runat="server" ID="btnExportSuppliers" Text="Εξαγωγή Στοιχείων Εκδοτών" ToolTip="Εξαγωγή Στοιχείων Εκδοτών" Image-Url="~/_img/iconExcel.png" OnClick="btnExportSuppliers_Click" />
        <dx:ASPxButton runat="server" ID="btnExportCoAuthors" Text="Εξαγωγή Στοιχείων Συνεκδοτών" ToolTip="Εξαγωγή Στοιχείων Συνεκδοτών" Image-Url="~/_img/iconExcel.png">
            <ClientSideEvents Click="showExportCoAuthors" />
        </dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnExportSuppliersNoLogisticBooks" Text="Εξαγωγή μη υπόχρεων λογ. βιβλίων" ToolTip="Εξαγωγή Στοιχείων Μη Υπόχρεων τήρησης λογιστικών βιβλίων" Image-Url="~/_img/iconExcel.png">
            <ClientSideEvents Click="showExportNoLogisticBooks" />
        </dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnExportCommitmentsRegistry" Text="Εξαγωγή μητρώου δεσμεύσεων" ToolTip="Εξαγωγή μητρώου δεσμεύσεων" Image-Url="~/_img/iconExcel.png">
            <ClientSideEvents Click="showExportCommitments" />
        </dx:ASPxButton>
    </div>
    <my:SuppliersSearchFilters ID="ucSearchFilters" runat="server" />

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

    <my:SuppliersGridView ID="gvSuppliers" runat="server" DataSourceID="odsSuppliers" OnCustomCallback="gvSuppliers_CustomCallback">
        <ClientSideEvents EndCallback="gvCallbackEnd" />
        <Columns>
            <dx:GridViewDataTextColumn Name="SupplierIBANs" Caption="Αλλαγές IBAN" Width="50px">
                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                <CellStyle HorizontalAlign="Right" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" style="font-size: 14px;" href="javascript:void(0);" 
                        onclick='<%# string.Format("showViewIbanChangesPopup({0}, \"{1}\")", Eval("ID"), Eval("Name")) %>'><%# GetIbanChangeCount((Supplier)Container.Grid.GetRow(Container.VisibleIndex)) %></a>
                    <a runat="server" class="img-btn tooltip" title="Αλλαγή IBAN"
                        href='javascript:void(0);'
                        onclick='<%# string.Format("showChangeIbanPopup({0}, \"{1}\")", Eval("ID"), Eval("Name")) %>'>                        
                        <img src="/_img/iconReportEdit.png" alt="Αλλαγή IBAN" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="50px" VisibleIndex="7">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Left" Wrap="false" />
                <DataItemTemplate>
                    <a class="img-btn tooltip" runat="server" href="javascript:void(0);" title="Προβολή Εκδότη"
                        onclick='<%# string.Format("showViewSupplierPopup({0});return false;", Eval("ID")) %>'>
                        <img src="/_img/iconView.png" alt="Προβολή Εκδότη" /></a>
                    <a class="img-btn tooltip" runat="server" href="javascript:void(0);" title="Δικαιολογητικά"
                        onclick='<%# string.Format("showManageFilesPopup({0});return false;", Eval("ID")) %>' visible='<%# ((Supplier)Container.DataItem).HasLogisticBooks.HasValue ? !((Supplier)Container.DataItem).HasLogisticBooks : false %>'>
                        <img src="/_img/iconUpload2.png" alt="Δικαιολογητικά" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:SuppliersGridView>

    <my:SuppliersExportGridView ID="gvSuppliersExport" Visible="false" runat="server" DataSourceForceStandardPaging="false">
    </my:SuppliersExportGridView>


    <asp:ObjectDataSource ID="odsSuppliers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Suppliers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsSuppliers_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
