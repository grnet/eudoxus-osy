<%@ Page Title="Επεξεργασία Κατάστασης" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.master" AutoEventWireup="true" CodeBehind="EditCatalogGroup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.EditCatalogGroup" %>

<%@ Register TagName="CatalogGroupsGridView" TagPrefix="my" Src="~/UserControls/GridViews/CatalogGroupsGridView.ascx" %>
<%@ Register TagName="CatalogsGridView" TagPrefix="my" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>
<%@ Register TagName="CatalogSearchFilters" TagPrefix="my" Src="~/UserControls/SearchFilters/UnconnectedCatalogSearchFiltersControl.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () { window.onpopstate = function (e) { alert("Για τη σωστή λειτουργία της εφαρμογής χρησιμοποιήστε το κουμπί 'Επιστροφή στη διαχείριση καταστάσεων'."); }; history.pushState({}, ''); });
    </script>
    <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή στη διαχείριση Καταστάσεων" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />

    <div class="br"></div>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="dv" style="width: 100%">
                <colgroup>
                    <col style="width: 120px" />
                </colgroup>
                <tr>
                    <th class="header" colspan="2">&raquo; Στοιχεία Κατάστασης</th>
                </tr>
                <tr>
                    <th>ID Κατάστασης:
                    </th>
                    <td>
                        <dx:ASPxLabel ID="lblGroupID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Ίδρυμα:
                    </th>
                    <td>
                        <dx:ASPxLabel ID="lblInstitution" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Αρ. Διανομών:
                    </th>
                    <td>
                        <dx:ASPxLabel ID="lblCatalogCount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Χρηματικό Ποσό<br />
                        (χωρίς Φ.Π.Α.):
                    </th>
                    <td>
                        <dx:ASPxLabel ID="lblTotalAmount" runat="server" />
                    </td>
                </tr>
            </table>

            <div class="br"></div>
            <div class="br"></div>
            <div class="br"></div>
            <div class="br"></div>
            
            <my:CatalogSearchFilters runat="server" ID="ucConnectedCatalogSearchFilters"></my:CatalogSearchFilters>
            <div class="gridViewTopButtons">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btnSearchConnected" runat="server" ClientInstanceName="btnSearchConnected" 
                                           Text="Αναζήτηση" Image-Url="~/_img/iconEdit.png" OnClick="btnSearchConnected_OnClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="br"></div>
            <table class="dv" style="width: 100%">
                <tr>
                    <th class="header">&raquo; Διανομές που περιέχονται στην Κατάσταση</th>
                </tr>
                <tr>
                    <td>
                        <my:CatalogsGridView ID="gvConnectedCatalogs" runat="server" HideInstitution="true"
                            DataSourceID="odsConnectedCatalogs" OnHtmlRowPrepared="gvConnectedCatalogs_HtmlRowPrepared">
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Αφαίρεση από Κατάσταση" Width="70px" VisibleIndex="8">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a class="img-btn" runat="server" href="javascript:void(0);"
                                            onclick='<%# string.Format("removeFromGroup({0});", Eval("ID")) %>'>
                                            <img src="/_img/iconReject.png" alt="Αφαίρεση από Κατάσταση" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </my:CatalogsGridView>
                    </td>
                </tr>
            </table>

            <div class="br"></div>
            <div class="br"></div>
            <div class="br"></div>
            <div class="br"></div>
             <my:CatalogSearchFilters runat="server" ID="ucCatalogSearchFilters" Mode="Supplier"></my:CatalogSearchFilters>
            <div class="gridViewTopButtons">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btnSearch" runat="server" ClientInstanceName="btnSearch" Text="Αναζήτηση" Image-Url="~/_img/iconEdit.png" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="br"></div>
            <table class="dv" style="width: 100%">
                <tr>
                    <th class="header">&raquo; Ασύνδετες Διανομές που μπορούν να προστεθούν στην Κατάσταση</th>
                </tr>
                <tr>
                    <td>
                        <my:CatalogsGridView ID="gvNotConnectedCatalogs" runat="server" HideInstitution="true" ClientInstanceName="gvNotConnectedCatalogs" 
                            DataSourceID="odsNotConnectedCatalogs"
                            OnCustomCallback="gvNotConnectedCatalogs_CustomCallback" OnHtmlRowPrepared="gvNotConnectedCatalogs_OnHtmlRowPreparedatalogs_HtmlRowPrepared">
                            <Columns>
                                <dx:GridViewDataTextColumn Name="Actions" Caption="Προσθήκη σε Κατάσταση" Width="70px" VisibleIndex="8">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a class="img-btn" runat="server" href="javascript:void(0);"
                                            onclick='<%# string.Format("addToGroup({0});", Eval("ID")) %>'
                                            visible='<%# CanAddToGroup((Catalog)Container.DataItem) %>'>
                                            <img src="/_img/iconAdd.png" alt="Προσθήκη σε Κατάσταση" /></a>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </my:CatalogsGridView>
                    </td>
                </tr>
            </table>

            <div style="display: none">
                <asp:Button ID="btnAddToGroup" runat="server" OnClick="btnAddToGroup_Click" />
                <asp:Button ID="btnRemoveFromGroup" runat="server" OnClick="btnRemoveFromGroup_Click" />
                <input type="hidden" id="hfCatalogID" runat="server" />
            </div>

            <script type="text/javascript">
                function addToGroup(catalogID) {
                    $('#<%= hfCatalogID.ClientID %>').val(catalogID);                    
                    <%= ClientScript.GetPostBackEventReference(btnAddToGroup, null) %>
                }

                function removeFromGroup(catalogID) {
                    $('#<%= hfCatalogID.ClientID %>').val(catalogID);
                    <%= ClientScript.GetPostBackEventReference(btnRemoveFromGroup, null) %>
                }
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:ObjectDataSource ID="odsConnectedCatalogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Catalogs"
                          SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                          EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsConnectedCatalogs_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="odsNotConnectedCatalogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Catalogs"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsNotConnectedCatalogs_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
