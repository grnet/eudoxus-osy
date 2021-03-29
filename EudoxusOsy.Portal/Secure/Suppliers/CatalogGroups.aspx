<%@ Page Title="Καταστάσεις" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.master" AutoEventWireup="true" CodeBehind="CatalogGroups.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.CatalogGroups" %>

<%@ Register TagPrefix="my" TagName="SupplierPhaseStatisticsGridView" Src="~/UserControls/GridViews/SupplierPhaseStatisticsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SupplierPhaseStatisticsView" Src="~/UserControls/SupplierControls/ViewControls/SupplierPhaseStatisticsView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogGroupsGridView" Src="~/UserControls/GridViews/CatalogGroupsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogsGridView" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BookGridView" Src="~/UserControls/ExportGridViews/BookExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="UnconnectedCatalogSearchFilters" Src="~/UserControls/SearchFilters/UnconnectedCatalogSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="TipIcon" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureHead" runat="server">
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">


    <asp:MultiView ID="mvCatalogGroups" runat="server">
        <asp:View ID="vSelectPhase" runat="server">

            <div class="br"></div>

            <table class="dv" style="width: 100%">
                <tr>
                    <th class="header">&raquo; Επιλογή Περιόδου Πληρωμών</th>
                </tr>
                <tr>
                    <td>
                        <my:SupplierPhaseStatisticsGridView ID="gvSupplierPhaseStatistics" runat="server"
                            OnHtmlRowPrepared="gvSupplierPhaseStatistics_HtmlRowPrepared">
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Επιλογή" Width="20px" VisibleIndex="0">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <input runat="server" type="checkbox"
                                            onclick='<%# string.Format("window.location.assign(\"CatalogGroups.aspx?pID={0}&t=true\");", Eval("Phase.ID")) %>'
                                            checked='<%# IsSelected((SupplierPhaseStatistics)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </my:SupplierPhaseStatisticsGridView>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="vManageCatalogGroups" runat="server">
            <script type="text/javascript">
                function IncludeInPayment(catalogID, previousState) {
                    gvCatalogGroups.PerformCallback('include:' + catalogID + ":"+previousState);
                }

                function confirmUngroup(s, e) {
                    showConfirmBox('Αποσύνδεση Διανομών', 'Με την αποσύνδεση των διανομών θα διαγραφούν όλες οι καταστάσεις που δεν έχουν επιλεγεί για πληρωμή και τα παραστατικά που έχετε εισάγει σε αυτές.',
                        function () { btnUngroupHidden.DoClick(); }, function () { return false; });
                }

                function groupsEndCallback(s, e) {
                    if (gvCatalogGroups.cpError) {
                        showAlertBox(gvCatalogGroups.cpError);
                        gvCatalogGroups.cpError = null;
                    }
                    else if (gvCatalogGroups.cpexport == null) {
                        if (gvCatalogGroups.cpMessage)
                        {
                            showAlertBox(gvCatalogGroups.cpMessage);
                        }
                        cbpStatistics.PerformCallback();
                        gvCatalogGroups.cpMessage = null;
                    }
                }

                function exportCatalogData(catalogID) {
                    $('#<%= hfExportCatalogID.ClientID %>').val(catalogID);
                    btnExportHidden.DoClick();
                }

                function exportCatalogInvoice(catalogID) {
                    $('#<%= hfExportCatalogID.ClientID %>').val(catalogID);
                    btnExportPdfHidden.DoClick();
                }
            </script>

            <div runat="server" class="selectedPhase" clientidmode="Static">
                <asp:Literal ID="ltSelectedPhase" runat="server" />
            </div>

            <div class="br"></div>
            <div class="br"></div>

            <div class="alert info">
                Με την υποβολή παραστατικού προς την αρμόδια Διεύθυνση Οικονομικής Διαχείρισης του ΥΠΠΕΘ ή την έναρξη της διαδικασίας πληρωμής, ο εκδότης/διαθέτης/δικαιούχος δηλώνει ότι αποδέχεται την καθορισθείσα τιμή κοστολόγησης 
                και παραιτείται του δικαιώματος ένστασης που προβλέπεται από την παρ. 5 του άρθρο 3 της Φ12/97315/2011-Β3 30.08.2011 ΚΥΑ (Β’ 1915) 
            </div>
            <div class="br"></div>
            <div class="br"></div>
            <div runat="server" id="divGroupsCreationLockedMessage" class="reminder">
                Δεν είναι δυνατή προσωρινά η δημιουργία καταστάσεων για την επιλεγμένη περίοδο πληρωμών, λόγω εργασιών συγχρονισμού των διανομών. Ζητούμε συγγνώμη για την ταλαιπωρία.
            </div>

            <div class="br"></div>

            <dx:ASPxCallbackPanel runat="server" ID="cbpStatistics" ClientInstanceName="cbpStatistics" OnCallback="cbpStatistics_Callback">
                <PanelCollection>
                    <dx:PanelContent>
                        <my:SupplierPhaseStatisticsView ID="ucSupplierPhaseStatisticsView" runat="server" />
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <dx:ASPxButton runat="server" ID="btnUngroupHidden" ClientInstanceName="btnUngroupHidden" ClientVisible="false" OnClick="btnUngroupCatalogs_Click" />
            <dx:ASPxButton runat="server" ID="btnExportHidden" ClientInstanceName="btnExportHidden" ClientVisible="false" OnClick="btnExportHidden_Click" />
            <dx:ASPxButton runat="server" ID="btnExportPdfHidden" ClientInstanceName="btnExportPdfHidden" ClientVisible="false" OnClick="btnExportPdfHidden_OnClick" />            
            <input type="hidden" runat="server" id="hfExportCatalogID" />            

            <div class="br"></div>
            <div class="br"></div>

            <table class="dv" style="width: 100%">
                <tr>
                    <th class="header">&raquo; Καταστάσεις Διανομών Εκδότη</th>
                </tr>
                <tr>
                    <td style="margin-bottom: 0; border-bottom: 0;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="float: left;">
                                    <table class="dv">
                                        <colgroup>
                                            <col style="width: 105px" />
                                            <col style="width: 10px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <th colspan="3" class="popupHeader">Φίλτρα Αναζήτησης
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>ID Κατάστασης:
                                            </th>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtGroupID" runat="server" />
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnSearchGroups" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                                                    <ClientSideEvents Click="function(s,e) { refresh(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="float: right; margin-top: 39px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnGroupCatalogs" runat="server" ClientInstanceName="btnGroupCatalogs" Text="Ομαδοποίηση διανομών ανά Ίδρυμα" OnClick="btnGroupCatalogs_Click" />
                                            </td>
                                            <td>
                                                <dx:ASPxButton runat="server" ClientInstanceName="btnUngroupCatalogs" ID="btnUngroupCatalogs" Text="Αποσύνδεση Διανομών">
                                                    <ClientSideEvents Click="confirmUngroup" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="margin-top: 0; border-top: 0;">
                        <my:CatalogGroupsGridView ID="gvCatalogGroups" runat="server" ClientInstanceName="gvCatalogGroups" DataSourceID="odsCatalogGroups"
                            OnHtmlRowPrepared="gvCatalogGroups_HtmlRowPrepared"
                            OnHtmlCellPrepared="gvCatalogGroups_HtmlCellPrepared"
                            OnCustomCallback="gvCatalogGroups_CustomCallback">
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Warnings" Caption=" " Width="30px" VisibleIndex="0">
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Δεν μπορείτε να επεξεργαστείτε την κατάσταση γιατί περιέχει βιβλία που δεν διανέμονται."
                                            visible='<%# ContainsInActiveBooks((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Το συνολικό ποσό των παραστατικών είναι μικρότερο από το συνολικό ποσό της κατάστασης."
                                            visible='<%# HasWarning((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconError.png" alt="Error" title="Το συνολικό ποσό των παραστατικών είναι μεγαλύτερο από το συνολικό ποσό της κατάστασης."
                                            visible='<%# HasError((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Η κατάσταση περιέχει διανομές βιβλίων των οποίων η τιμή ελέγχεται από την επιτροπή κοστολόγησης. Παρακαλούμε επιλέξτε επεξεργασία της κατάστασης προκειμένου να αφαιρέσετε τις εν λόγω διανομές."
                                            visible='<%# HasPendingPriceVerification((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Η κατάσταση περιέχει διανομές βιβλίων με μη αναμενόμενη αλλαγή στην τιμή τους. Παρακαλούμε επιλέξτε επεξεργασία της κατάστασης προκειμένου να αφαιρέσετε τις εν λόγω διανομές."
                                            visible='<%# HasUnexpectedPriceChange((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Name="IncludeInPayment" Caption="Επιλογή για πληρωμή" Width="60px" VisibleIndex="0">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <input type="checkbox" runat="server" onclick='<%# string.Format("IncludeInPayment({0}, {1})", ((CatalogGroupInfo)Container.DataItem).ID, ((CatalogGroupInfo)Container.DataItem).GroupStateInt) %>' checked="<%# ((CatalogGroupInfo)Container.DataItem).GroupStateInt >= (int)enCatalogGroupState.Selected %>" />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="IsTransfered" Name="BankTransfer" Caption="Εκχώρηση σε Τράπεζα" Width="70px" VisibleIndex="4">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <input type="checkbox" runat="server" onclick='<%# string.Format("setBankTransfer(this, {0})", Eval("ID")) %>' checked='<%# (bool)Eval("IsTransfered") %>' />
                                        <a runat="server" class="img-btn tooltip" title="Προβολή Στοιχείων Εκχώρησης"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("showViewBankTransferPopup({0})", Eval("ID")) %>'
                                            visible='<%# (bool)Eval("IsTransfered") %>'>
                                            <img src="/_img/iconView.png" alt="Προβολή Στοιχείων Εκχώρηση" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="100px" VisibleIndex="5">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" Wrap="false" />
                                    <DataItemTemplate>
                                        <a runat="server" class="img-btn tooltip" title="Επεξεργασία Κατάστασης"
                                            href='<%# string.Format("EditCatalogGroup.aspx?gID={0}&pID={1}", Eval("ID"), SelectedPhase.ID) %>'
                                            visible='<%# CanEditGroup((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconEdit.png" alt="Επεξεργασία Κατάστασης" /></a>
                                        <a runat="server" class="img-btn tooltip" title="Προβολή Κατάστασης"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("showViewCatalogGroupDetailsPopup({0})", Eval("ID")) %>'>
                                            <img src="/_img/iconView.png" alt="Προβολή Κατάστασης" /></a>
                                        <a runat="server" href="javascript:void(0);" προβολή="img-btn tooltip" title="Διαγραφή Κατάστασης"
                                            onclick='<%# string.Format("deleteGroup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanDeleteGroup((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconDelete.png" alt="Διαγραφή Κατάστασης" /></a>
                                        <a runat="server" class="img-btn tooltip buttonwithbadge" title="Διαχείριση Παραστατικών"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("showManageInvoicesPopup({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconMoney.png" alt="Διαχείριση Παραστατικών" />
                                            <span runat="server" class="buttonbadge" visible="<%# ((CatalogGroupInfo)Container.DataItem).InvoiceCount > 0 %>"><%# ((CatalogGroupInfo)Container.DataItem).InvoiceCount %></span></a>
                                        <a runat="server" class="img-btn tooltip" title="Εκτύπωση Κατάστασης"
                                           href='javascript:void(0);' onclick='<%# string.Format("exportCatalogInvoice({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconPdf.gif" alt="Εξαγωγή σε Pdf" /></a>
                                        <a runat="server" class="img-btn tooltip" title="Εξαγωγή Κατάστασης σε Excel"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("exportCatalogData({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconExcel.png" alt="Εξαγωγή σε Excel" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="groupsEndCallback" />
                        </my:CatalogGroupsGridView>
                    </td>
                </tr>
            </table>

            <dx:ASPxLabel ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false" />

            <asp:ObjectDataSource ID="odsCatalogGroups" runat="server" TypeName="EudoxusOsy.Portal.DataSources.CatalogGroups"
                SelectMethod="FindWithSupplierPhaseAndGroup" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsCatalogGroups_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="supplierID" Type="Int32" />
                    <asp:Parameter Name="phaseID" Type="Int32" />
                    <asp:Parameter Name="groupID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <div style="display: none">
                <asp:Button ID="btnDeleteGroup" runat="server" OnClick="btnDeleteGroup_Click" />
                <input type="hidden" id="hfCatalogGroupID" runat="server" />
            </div>

            <script type="text/javascript">
                function deleteGroup(catalogGroupID, institution) {
                    var doDelete = function () {
                        $('#<%= hfCatalogGroupID.ClientID %>').val(catalogGroupID);
                        <%= ClientScript.GetPostBackEventReference(btnDeleteGroup, null) %>
                    };

                    showConfirmBox('Διαγραφή Κατάστασης', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την κατάσταση για το Ίδρυμα \'' + institution + '\';', doDelete);
                }

                function setBankTransfer(checkbox, groupID) {
                    if (checkbox.checked) {
                        showSetBankTransferPopup(groupID);
                    }
                    else {
                        gvCatalogGroups.PerformCallback('removebanktransfer:' + groupID);
                    }
                }
            </script>

            <div class="br"></div>
            <div class="br"></div>
            <div class="br"></div>
            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="dv" style="width: 100%">
                        <tr>
                            <th class="header">&raquo; Ασύνδετες Διανομές Εκδότη</th>
                        </tr>
                        <tr>
                            <td style="margin-bottom: 0; border-bottom: 0;">
                                <my:UnconnectedCatalogSearchFilters runat="server" ID="ucUnconnectedCatalogSearchFilters" Mode="Supplier"></my:UnconnectedCatalogSearchFilters>
                                <div style="padding: 5px 0;">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnSearchCatalogs" runat="server" ClientInstanceName="btnSearchCatalogs" Text="Αναζήτηση" Image-Url="~/_img/iconSearch.png" OnClick="btnSearchCatalogs_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="margin-top: 0; border-top: 0;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <my:CatalogsGridView ID="gvCatalogs" ClientInstanceName="gvCatalogs" runat="server" DataSourceID="odsCatalogs"
                                                OnHtmlRowPrepared="gvCatalogs_HtmlRowPrepared"
                                                OnCustomDataCallback="gvCatalogs_CustomDataCallback">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Name="Warnings" Caption=" " Width="30px" VisibleIndex="0">
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <DataItemTemplate>
                                                            <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title='<%# InabilityToCreateGroup((Catalog)Container.Grid.GetRow(Container.VisibleIndex)) %>'
                                                                visible='<%# InabilityToCreateGroup((Catalog)Container.Grid.GetRow(Container.VisibleIndex)) != "" %>' />                                                                       
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Name="Actions" Caption="Δημιουργία Καστάστασης" Width="50px" VisibleIndex="8">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <DataItemTemplate>
                                                            <a runat="server" class="img-btn tooltip" title="Δημιουργία Κατάστασης"
                                                                href="javascript:void(0);"
                                                                onclick='<%# string.Format("createGroup({0});", Eval("ID")) %>'
                                                                visible='<%# SelectedPhase.ID == Config.HideGroupCreationForPhase ? false :  CanCreateGroup((Catalog)Container.DataItem) %>'>
                                                                <img src="/_img/iconAddNewItem.png" alt="Δημιουργία Κατάστασης" /></a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                            </my:CatalogsGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:ObjectDataSource ID="odsCatalogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Catalogs"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsCatalogs_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <my:BookGridView Mode="GroupExport" ID="gvBooks" runat="server" DataSourceForceStandardPaging="false" GridClientVisible="false">
            </my:BookGridView>

            <div style="display: none">
                <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" />
                <input type="hidden" id="hfCatalogID" runat="server" />
            </div>

            <script type="text/javascript">
                function createGroup(catalogID) {
                    $('#<%= hfCatalogID.ClientID %>').val(catalogID);
                        <%= ClientScript.GetPostBackEventReference(btnCreateGroup, null) %>
                }

                function refresh() {
                    gvCatalogGroups.PerformCallback('refresh');
                }
            </script>
        </asp:View>
    </asp:MultiView>
</asp:Content>
