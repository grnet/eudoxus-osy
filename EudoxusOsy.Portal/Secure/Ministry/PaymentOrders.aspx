<%@ Page Title="Καταστάσεις" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PaymentOrders.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PaymentOrders" %>

<%@ Register TagPrefix="my" TagName="SuppliersSearchFilters" Src="~/UserControls/SearchFilters/SupplierSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="UnconnectedCatalogSearchFilters" Src="~/UserControls/SearchFilters/UnconnectedCatalogSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersGridView" Src="~/UserControls/GridViews/SuppliersGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SupplierPhaseStatisticsGridView" Src="~/UserControls/GridViews/SupplierPhaseStatisticsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SupplierPhaseStatisticsView" Src="~/UserControls/SupplierControls/ViewControls/SupplierPhaseStatisticsView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogGroupsGridView" Src="~/UserControls/GridViews/CatalogGroupsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogsGridView" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BookGridView" Src="~/UserControls/ExportGridViews/BookExportGridView.ascx" %>
<%@ Register TagName="TipIcon" TagPrefix="my" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureHead" runat="server">
    <script type="text/javascript">
        function createOfficeSlip() {
            exportOfficeSlipPopup(1);
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">

    <asp:MultiView ID="mvPaymentOrders" runat="server">
        <asp:View ID="vSelectSupplier" runat="server">
            <div class="filterButtons">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Κατάσταση δαπάνης ανά εκδότη" Image-Url="~/_img/iconExcel.png">
                                <ClientSideEvents Click="exportOfficeSlipSupplierPopup" />
                            </dx:ASPxButton>
                              <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Διαβιβαστικό ανά εκδότη" Image-Url="~/_img/iconExcel.png">
                                <ClientSideEvents Click="exportTransferPerIssuerPopup" />
                            </dx:ASPxButton>
                           <%-- <dx:ASPxButton ID="btnCreateOfficeSlip" runat="server" Text="Διαβιβαστικό εκδοτών ανά ημέρα (pdf)" Image-Url="~/_img/iconEmailSend.png">
                                <ClientSideEvents Click="createOfficeSlip" />
                            </dx:ASPxButton>--%>
                           <%-- <dx:ASPxButton ID="btnExportOfficeSlip" runat="server" Text="Διαβιβαστικό εκδοτών ανά ημέρα (excel)" Image-Url="~/_img/iconExcel.png">
                                <ClientSideEvents Click="exportOfficeSlipPopup" />
                            </dx:ASPxButton>    --%>                        
                        </td>
                    </tr>
                </table>
            </div>
            <div class="br"></div>

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
                    <dx:GridViewDataTextColumn Name="Actions" Caption="Επιλογή" Width="20px" VisibleIndex="7">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" class="img-btn tooltip" title="Διαχείριση Διανομών και Καταστάσεων"
                                href='<%# string.Format("PaymentOrders.aspx?sID={0}&t=false", Eval("ID")) %>'>
                                <img src="/_img/iconReportEdit.png" alt="Διαχείριση Διανομών και Καταστάσεων" /></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </my:SuppliersGridView>

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

        </asp:View>

        <asp:View ID="vSelectPhase" runat="server">

            <div runat="server" class="selectedPhase" clientidmode="Static">
                <span ID="ltSelectPhase" runat="server" />
            </div>

            <div class="br"></div>
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
                                            onclick='<%# string.Format("window.location.assign(\"PaymentOrders.aspx?sID={0}&pID={1}&t=true\");", Entity.ID, Eval("Phase.ID")) %>'
                                            checked='<%# IsSelected((SupplierPhaseStatistics)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </my:SupplierPhaseStatisticsGridView>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="vManagePaymentOrders" runat="server">

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
                    else if (gvCatalogGroups.cpexportPDF != null) {
                        btnExportPDFHidden.DoClick();
                    }
                    else {
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

                function exportPDFCatalog(catalogID) {
                    $('#<%= hfExportCatalogID.ClientID %>').val(catalogID);
                    gvCatalogGroups.PerformCallback("exportPDF:" + catalogID);
                }
            </script>

            <div runat="server" class="selectedPhase" clientidmode="Static">
                <asp:Literal ID="ltSelectedPhase" runat="server" />
            </div>

            <div class="br"></div>
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
            <dx:ASPxButton runat="server" ID="btnExportPDFHidden" ClientInstanceName="btnExportPDFHidden" ClientVisible="false" OnClick="btnExportPDFHidden_Click" />
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
                        <my:CatalogGroupsGridView ID="gvCatalogGroups" AllowSorting="true" runat="server"
                            ClientInstanceName="gvCatalogGroups"
                            DataSourceID="odsCatalogGroups"
                            OnHtmlRowPrepared="gvCatalogGroups_HtmlRowPrepared"
                            OnHtmlCellPrepared="gvCatalogGroups_HtmlCellPrepared"
                            OnCustomCallback="gvCatalogGroups_CustomCallback">
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Warnings" Caption=" " Width="30px" VisibleIndex="0">
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Η κατάσταση περιέχει διανομές βιβλίων που δεν είναι δυνατόν να τιμολογηθούν και να αποζημιωθούν. Παρακαλούμε επιλέξτε επεξεργασία της κατάστασης προκειμένου να αφαιρέσετε τις εν λόγω διανομές."
                                            visible='<%# ContainsInActiveBooks((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Η κατάσταση περιέχει διανομές βιβλίων των οποίων η τιμή ελέγχεται από την επιτροπή κοστολόγησης. Παρακαλούμε επιλέξτε επεξεργασία της κατάστασης προκειμένου να αφαιρέσετε τις εν λόγω διανομές."
                                            visible='<%# HasPendingPriceVerification((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Η κατάσταση περιέχει διανομές βιβλίων με μη αναμενόμενη αλλαγή στην τιμή τους. Παρακαλούμε επιλέξτε επεξεργασία της κατάστασης προκειμένου να αφαιρέσετε τις εν λόγω διανομές."
                                            visible='<%# HasUnexpectedPriceChange((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title="Το συνολικό ποσό των παραστατικών είναι μικρότερο από το συνολικό ποσό της κατάστασης."
                                            visible='<%# HasWarning((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                        <img runat="server" class="img-btn tooltip" src="~/_img/iconError.png" alt="Error" title="Το συνολικό ποσό των παραστατικών είναι μεγαλύτερο από το συνολικό ποσό της κατάστασης."
                                            visible='<%# HasError((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>' />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Settings-AllowSort="False" Name="IncludeInPayment" Caption="Επιλογή για πληρωμή" Width="60px" VisibleIndex="0">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <input type="checkbox" runat="server"
                                            onclick='<%# string.Format("IncludeInPayment({0},{1})", ((CatalogGroupInfo)Container.DataItem).ID, ((CatalogGroupInfo)Container.DataItem).GroupStateInt) %>'
                                            checked="<%# ((CatalogGroupInfo)Container.DataItem).GroupStateInt >= (int)enCatalogGroupState.Selected %>" />
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
                                            <img src="/_img/iconView.png" alt="Προβολή Στοιχείων Εκχώρησης" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Name="GroupDeduction" Settings-AllowSort="True" FieldName="DeductionVatType" Caption="Καθεστώς Φ.Π.Α." Width="100px" VisibleIndex="5">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <a runat="server" href="javascript:void(0)" class="img-btn tooltip" title="Καθεστώς Φ.Π.Α."
                                                        onclick='<%# string.Format("showEditGroupDeductionPopup({0})", Eval("ID")) %>'
                                                        Visible="<%# !IsZeroVatElegible() %>">
                                                        <img src="/_img/iconPencilAdd.png" alt="Καθεστώς Φ.Π.Α." /></a>
                                                </td>
                                                <td style="padding-left: 10px;">
                                                    <%# GetGroupDeduction((CatalogGroupInfo)Container.Grid.GetRow(Container.VisibleIndex)) %>
                                                </td>
                                            </tr>
                                        </table>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Settings-AllowSort="False" Name="CatalogGroupLog" Caption="Ιστορικό Μεταβολών" Width="50px" VisibleIndex="5">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a runat="server" href="javascript:void(0)" class="img-btn tooltip" title="Ιστορικό Μεταβολών"
                                            onclick='<%# string.Format("showViewCatalogGroupLogPopup({0})", Eval("ID")) %>'>
                                            <img src="/_img/iconViewDetails.png" alt="Ιστορικό Μεταβολών" />
                                        </a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Settings-AllowSort="False" Name="Actions" Caption="Ενέργειες" Width="180px" VisibleIndex="5">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Right" />
                                    <DataItemTemplate>
                                        <a runat="server" class="img-btn tooltip" title="Επεξεργασία Κατάστασης"
                                            href='<%# string.Format("EditCatalogGroup.aspx?gID={0}&pID={1}", Eval("ID"), SelectedPhase.ID) %>'
                                            visible='<%# CanEditGroup((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconEdit.png" alt="Επεξεργασία Κατάστασης" /></a>
                                        <a runat="server" class="img-btn tooltip" title="Προβολή Κατάστασης"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("showViewCatalogGroupDetailsPopup({0})", Eval("ID")) %>'>
                                            <img src="/_img/iconView.png" alt="Προβολή Κατάστασης" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Έγκριση για πληρωμή"
                                            onclick='<%# string.Format("approveGroupPopup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanApproveGroup((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconAccept.png" alt="Έγκριση για πληρωμή" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Επαναφορά σε Επιλεγμένη για Πληρωμή"
                                            onclick='<%# string.Format("revertApprovalPopup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanRevertApproval((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconReturn.png" alt="Επαναφορά σε Επιλεγμένη για Πληρωμή" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Αποστολή προς ΥΔΕ"
                                            onclick='<%# string.Format("sendToYDEPopup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanSendToYDE((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconEmailSend.png" alt="Αποστολή προς ΥΔΕ" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Επιστροφή από ΥΔΕ"
                                            onclick='<%# string.Format("returnFromYDEPopup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanReturnFromYDE((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconUndo.png" alt="Επιστροφή από ΥΔΕ" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Διαγραφή Κατάστασης"
                                            onclick='<%# string.Format("deleteGroup({0}, \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name) %>'
                                            visible='<%# CanDeleteGroup((CatalogGroupInfo)Container.DataItem) %>'>
                                            <img src="/_img/iconDelete.png" alt="Διαγραφή Κατάστασης" /></a>
                                        <a runat="server" class="img-btn tooltip buttonwithbadge" title="Διαχείριση Παραστατικών"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("showManageInvoicesPopup({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconMoney.png" alt="Διαχείριση Παραστατικών" />
                                            <span runat="server" class="buttonbadge" visible="<%# ((CatalogGroupInfo)Container.DataItem).InvoiceCount > 0 %>"><%# ((CatalogGroupInfo)Container.DataItem).InvoiceCount %></span></a>
                                        <a runat="server" class="img-btn tooltip" title="Εκτύπωση Κατάστασης"
                                            href='javascript:void(0);' onclick='<%# string.Format("exportCatalogInvoicePopup({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconPdf.gif" alt="Εξαγωγή σε Pdf" /></a>
                                        <a runat="server" class="img-btn tooltip" title="Εξαγωγή Κατάστασης σε Excel"
                                            href='javascript:void(0);'
                                            onclick='<%# string.Format("exportCatalogData({0})", ((CatalogGroupInfo)Container.DataItem).ID) %>'>
                                            <img src="/_img/iconExcel.png" alt="Εξαγωγή σε Excel" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Κλείδωμα Κατάστασης"
                                            onclick='<%# string.Format("return doAction(\"lock\", {0}, \"PaymentOrders\", \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name)%>'
                                            visible='<%# !((bool)Eval("IsLocked")) %>'>
                                            <img src="/_img/iconLock.png" alt="Κλείδωμα Κατάστασης" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Ξεκλείδωμα Κατάστασης"
                                            onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"PaymentOrders\", \"{1}\");", Eval("ID"), CacheManager.Institutions.Get((int)Eval("InstitutionID")).Name)%>'
                                            visible='<%# (bool)Eval("IsLocked") %>'>
                                            <img src="/_img/iconUnLock.png" alt="Ξεκλείδωμα Κατάστασης" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="groupsEndCallback" />
                        </my:CatalogGroupsGridView>
                    </td>
                </tr>
            </table>

            <dx:ASPxLabel ID="lblCatalogGroupError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false" />

            <asp:ObjectDataSource ID="odsCatalogGroups" runat="server" TypeName="EudoxusOsy.Portal.DataSources.CatalogGroups"
                SelectMethod="FindWithSupplierPhaseAndGroup"
                SelectCountMethod="CountWithCriteria"
                EnablePaging="true"
                SortParameterName="sortExpression"
                OnSelecting="odsCatalogGroups_Selecting">
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
                    <dx:ASPxLabel ID="lblCatalogError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false" />
                    <div class="br"></div>
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
                                                OnCustomDataCallback="gvCatalogs_CustomDataCallback"
                                                 OnCustomCallback="gvCatalogs_CustomCallback">
                                                <Columns>
                                                     <dx:GridViewDataTextColumn Name="Warnings" Caption=" " Width="30px" VisibleIndex="0">
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <DataItemTemplate>
                                                            <img runat="server" class="img-btn tooltip" src="~/_img/iconWarning.png" alt="Warning" title='<%# InabilityToCreateGroup((Catalog)Container.Grid.GetRow(Container.VisibleIndex)) %>'
                                                                visible='<%# InabilityToCreateGroup((Catalog)Container.Grid.GetRow(Container.VisibleIndex)) != "" %>' />                                                                                                                    
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Name="CreateGroup" Caption="Δημιουργία Καστάστασης" Width="50px" VisibleIndex="8">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <DataItemTemplate>
                                                            <a runat="server" class="img-btn tooltip" title="Δημιουργία Κατάστασης"
                                                                href="javascript:void(0);"
                                                                onclick='<%# string.Format("createGroup({0});", Eval("ID")) %>'
                                                                visible='<%# CanCreateGroup((Catalog)Container.DataItem) %>'>
                                                                <img src="/_img/iconAddNewItem.png" alt="Δημιουργία Κατάστασης" /></a>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Name="ChangePhase" Caption="Αλλαγή Φάσης" Width="50px" VisibleIndex="8">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <DataItemTemplate>
                                                            <a runat="server" class="img-btn tooltip" title="Αλλαγή Φάσης"
                                                                href="javascript:void(0);"
                                                                visible='<%# CanMovePhase((Catalog)Container.DataItem) %>'
                                                                onclick='<%# string.Format("changePhase({0});", Eval("ID")) %>'>
                                                                <img src="/_img/iconPencilAdd.png" alt="Αλλαγή Φάσης" /></a>
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

            <my:BookGridView ID="gvBooks" runat="server" DataSourceForceStandardPaging="false" GridClientVisible="false" />

            <div style="display: none">
                <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" />
                <input type="hidden" id="hfCatalogID" runat="server" />
            </div>

            <script type="text/javascript">
                function createGroup(catalogID) {
                    $('#<%= hfCatalogID.ClientID %>').val(catalogID);
                    <%= ClientScript.GetPostBackEventReference(btnCreateGroup, null) %>
                }

                function changePhase(catalogID) {
                    $('#<%= hfCatalogID.ClientID %>').val(catalogID);
                    showMovetoPhase();
                }

                function refresh() {
                    gvCatalogGroups.PerformCallback('refresh');
                }

                function cmdRefresh() {
                    gvCatalogGroups.PerformCallback('refresh');
                }
            </script>

        </asp:View>

    </asp:MultiView>

</asp:Content>
