<%@ Page Title="Εκχωρήσεις" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="Transfers.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.Transfers" %>

<%@ Register TagPrefix="my" TagName="SuppliersSearchFilters" Src="~/UserControls/SearchFilters/SupplierSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="TransfersSearchFilters" Src="~/UserControls/SearchFilters/TransferSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersGridView" Src="~/UserControls/GridViews/SuppliersGridView.ascx" %>
<%@ Register TagName="TransfersGridView" TagPrefix="my" Src="~/UserControls/GridViews/TransfersGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BanksSearchFilters" Src="~/UserControls/SearchFilters/BankSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="BanksGridView" Src="~/UserControls/GridViews/BanksGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <dx:ASPxPageControl runat="server" ID="tcPageTabs" Width="100%">
        <TabPages>
            <dx:TabPage Text="Αναζήτηση Εκχωρήσεων" Name="Transfers" TabStyle-Width="100%">
                <ContentCollection>
                    <dx:ContentControl Width="100%">
                        <my:TransfersSearchFilters ID="ucTransferSearchFilters" runat="server" />
                        <div class="filterButtons">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnSearchTransfers" runat="server" Text="Αναζήτηση Εκχωρήσεων" Image-Url="~/_img/iconView.png">
                                            <ClientSideEvents Click="function(s,e) { gvTransfers.PerformCallback('refresh'); }" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnExportTransfers" runat="server" Text="Εξαγωγή Εκχωρήσεων" ToolTip="Εξαγωγή εκχωρήσεων σε αρχείο Excel" Image-Url="~/_img/iconExcel.png" OnClick="btnExportTransfers_Click">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <my:TransfersGridView runat="server" ID="gvTransfers" ClientInstanceName="gvTransfers" DataSourceForceStandardPaging="true" DataSourceID="odsTransfers" OnCustomCallback="gvTransfers_CustomCallback">
                        </my:TransfersGridView>
                        <asp:ObjectDataSource ID="odsTransfers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Transfers"
                            SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                            EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsTransfers_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="criteria" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Δημιουργία και επεξεργασία εκχωρήσεων" Name="Suppliers" TabStyle-Width="100%">
                <ContentCollection>
                    <dx:ContentControl Width="100%">
                        <my:SuppliersSearchFilters ID="ucSearchFilters" runat="server" />
                        <div class="filterButtons">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                                            <ClientSideEvents Click="function(s,e) { gvSuppliers.PerformCallback('refresh'); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <my:SuppliersGridView ID="gvSuppliers" ClientInstanceName="gvSuppliers" runat="server" DataSourceID="odsSuppliers" OnCustomCallback="gvSuppliers_CustomCallback">
                            <ClientSideEvents EndCallback="gvCallbackEnd" />
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="20px" VisibleIndex="7">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a runat="server" href="javascript:void(0)" class="img-btn tooltip" title="Διαχείριση Εκχωρήσεων"
                                            onclick=<%# string.Format("showManageTransfersPopup('{0}')", Eval("ID"))%>>
                                            <img src="/_img/iconReportEdit.png" alt="Διαχείριση Εκχωρήσεων" /></a>
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
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Τράπεζες" Name="Banks" TabStyle-Width="100%">
                <ContentCollection>
                    <dx:ContentControl Width="100%">
                        <my:BanksSearchFilters ID="BanksSearchFilters1" runat="server" />

                        <div class="filterButtons">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                                            <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnAddBank" runat="server" Text="Προσθήκη Τράπεζας" Image-Url="~/_img/iconAddNewItem.png">
                                            <ClientSideEvents Click="function(s,e) { showAddBankPopup(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" Image-Url="~/_img/iconExcel.png" OnClick="btnExport_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <my:BanksGridView ID="gvBanks" runat="server" DataSourceID="odsBanks"
                            OnHtmlRowPrepared="gvBanks_HtmlRowPrepared"
                            OnCustomCallback="gvBanks_CustomCallback"
                            OnExporterRenderBrick="gveBanks_RenderBrick">
                            <ClientSideEvents EndCallback="gvCallbackEnd" />
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="90px" VisibleIndex="3">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a runat="server" href="javascript:void(0)" class="img-btn tooltip" title="Επεξεργασία Τράπεζας"
                                            onclick=<%# string.Format("showEditBankPopup('{0}')", Eval("ID"))%>>
                                            <img src="/_img/iconEdit.png" alt="Επεξεργασία Τράπεζας" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Απενεργοποίηση Τράπεζας"
                                            onclick='<%# string.Format("return doAction(\"deactivate\", {0}, \"BankManagement\", \"{1}\");", Eval("ID"), Eval("Name"))%>'
                                            visible='<%# (bool)Eval("IsActive") %>'>
                                            <img src="/_img/iconLock.png" alt="Απενεργοποίηση Τράπεζας" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Ενεργοποίηση Τράπεζας"
                                            onclick='<%# string.Format("return doAction(\"activate\", {0}, \"BankManagement\", \"{1}\");", Eval("ID"), Eval("Name"))%>'
                                            visible='<%# !((bool)Eval("IsActive")) %>'>
                                            <img src="/_img/iconUnLock.png" alt="Ενεργοποίηση Τράπεζας" /></a>
                                        <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Διαγραφή Τράπεζας"
                                            onclick='<%# string.Format("return doAction(\"delete\", {0}, \"BankManagement\", \"{1}\");", Eval("ID"), Eval("Name"))%>'>
                                            <img src="/_img/iconDelete.png" alt="Διαγραφή Τράπεζας" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </my:BanksGridView>

                        <asp:ObjectDataSource ID="odsBanks" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Banks"
                            SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                            EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsBanks_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="criteria" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
