<%@ Page Title="Διανομές" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="Catalogs.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.Catalogs" %>

<%@ Register TagName="CatalogSearchFiltersControl" TagPrefix="my" Src="~/UserControls/SearchFilters/CatalogSearchFiltersControl.ascx" %>
<%@ Register TagName="EditCatalogsGridView" TagPrefix="my" Src="~/UserControls/GridViews/EditCatalogsGridView.ascx" %>
<%@ Register TagName="EditCatalogsExportGridView" TagPrefix="my" Src="~/UserControls/ExportGridViews/EditCatalogsExportGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content runat="server" ContentPlaceHolderID="cphSecureHead">
    <script type="text/javascript">
        function doDelete() {
            var keys = gv.GetSelectedKeysOnPage();
            if (keys.length > 1) {
                alert('Δεν μπορείτε να διαγράψετε πάνω από μία διανομή!');
            } else {
                showConfirmBox('Διαγραφή', 'Είστε σίγουρος/η ότι θέλετε να διαγράψετε την διανομή ' + keys[0],
                function () {
                    keys.forEach(function (key, index) {
                        gv.PerformCallback('delete:' + key);
                    });
                })

            }
        }

        function doDeleteItem(itemID) {
            showConfirmBox('Διαγραφή', 'Η διανομή με ID ' + itemID + ' θα διαγραφεί. Είστε σίγουρος/η;',
            function () { gv.PerformCallback('delete:' + itemID) });
        }

        function doReverse() {
            var keys = gv.GetSelectedKeysOnPage();
            if (keys.length > 1) {
                alert('Δεν μπορείτε να προχωρήσετε σε αντιλογισμό περισσότερων από μία διανομών!');
            } else {
                showConfirmBox('Αντιλογισμός', 'Θα γίνει αντιλογισμός της διανομής με ID: ' + keys[0] + '. Θέλετε να συνεχίσετε;',
                function () {
                    keys.forEach(function (key, index) {
                        gv.PerformCallback('reverse:' + key);
                    });
                })

            }
        }

        function doRevertItem(itemID) {
            showConfirmBox('Αντιλογισμός', 'Θα γίνει αντιλογισμός της διανομής με ID ' + itemID + '. Θέλετε να συνεχίσετε; ',
               function () {
                   gv.PerformCallback('reverse:' + itemID);
               })
        }       

        function checkErrors() {
            if (gv.cperrors != null) {
                showAlertBox(gv.cperrors);
                gv.cperrors = null;
            }
        }

        function doCreateCatalogs() {
            showConfirmBox('Δημιουργία Διανομών', 'Είστε σίγουρος/η ότι θέλετε να δημιουργήσετε διανομές για την τρέχουσα περίοδο πληρωμών;',
            function () {
                btnCreateCatalogsHidden.DoClick();
            });
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <br />
    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnExport" runat="server" Text="Εξαγωγή Επιλεγμένων Διανομών" Image-Url="~/_img/iconExcel.png" OnClick="btnExport_OnClick">                        
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton runat="server" ID="btnExportCatalogsReport" Text="Αναφορά διανομών φάσης" ToolTip="Εξαγωγή αναφοράς διανομών ανά περίοδο" Image-Url="~/_img/iconExcel.png">
                        <ClientSideEvents Click="showExportCatalogsReport" />
                    </dx:ASPxButton>
                </td>                
            </tr>
        </table>
    </div>        
    <my:CatalogSearchFiltersControl runat="server" ID="ucCatalogSearchFiltersControl"></my:CatalogSearchFiltersControl>
    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="btnAddCatalog" runat="server" Text="Δημιουργία Νέας Διανομής" Image-Url="~/_img/iconAddNewItem.png">
                        <ClientSideEvents Click="function(s,e) { showAddCatalogPopup(); }" />
                    </dx:ASPxButton>
                </td>                                
            </tr>
        </table>
    </div>    

    <my:EditCatalogsGridView ID="gvCatalogs" runat="server" ClientInstanceName="gv" DataSourceID="odsCatalogs" PagingMode="ShowPager"
        OnHtmlRowPrepared="gvCatalogs_HtmlRowPrepared"
        OnCustomDataCallback="gvCatalogs_CustomDataCallback"
        OnCustomCallback="gvCatalogs_CustomCallback">
        <ClientSideEvents EndCallback="checkErrors" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ενέργειες" Width="50px">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Διαγραφή Διανομής"
                        onclick='<%# string.Format("doDeleteItem({0})", Eval("ID"))%>'
                        visible='<%# (bool)Eval("IsDeleteAllowed") %>'>
                        <img src="/_img/iconDelete.png" alt="Διαγραφή Διανομής" /></a>
                    <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Επεξεργασία Διανομής"
                        onclick='<%# string.Format("editCatalogPopup({0})", Eval("ID"))%>'
                        visible='<%# (bool)Eval("IsDeleteAllowed") && (((Catalog)Container.DataItem).CatalogGroup == null || ((Catalog)Container.DataItem).CatalogGroup.State == enCatalogGroupState.New ) %>'>
                        <img src="/_img/iconEdit.png" alt="Επεξεργασία Διανομής" /></a>
                    <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Αντιλογισμός Διανομής"
                        onclick='<%# string.Format("doRevertItem({0})", Eval("ID"))%>'
                        visible='<%# !(bool)Eval("IsDeleteAllowed") && ((Catalog)Container.DataItem).CatalogGroup != null && ((Catalog)Container.DataItem).CatalogGroup.State == enCatalogGroupState.Sent %>'>
                        <img src="/_img/iconReturn.png" alt="Αντιλόγισμός Διανομής" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:EditCatalogsGridView>
        
    <my:EditCatalogsExportGridView ID="gvCatalogsExport" GridClientVisible="false" DataSourceForceStandardPaging="false" 
                        OnExporterRenderBrick="gvCatalogsExport_ExporterRenderBrick"
                      ClientInstanceName="gvCatalogsExport" runat="server" OnCustomCallback="gvCatalogs_CustomCallback">
    </my:EditCatalogsExportGridView>

    <asp:ObjectDataSource ID="odsCatalogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Catalogs"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" OnSelecting="odsCatalogs_Selecting"
        MaximumRowsParameterName="maximumRows" StartRowIndexParameterName="startRowIndex"
        EnablePaging="true" SortParameterName="sortExpression">
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
