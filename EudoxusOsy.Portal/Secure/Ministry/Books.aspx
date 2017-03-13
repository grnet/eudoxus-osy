<%@ Page Title="Συγγράμματα" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="Books.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.Books" %>

<%@ Register TagPrefix="my" TagName="BookSearchFilters" Src="~/UserControls/SearchFilters/BookSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="BooksGridView" Src="~/UserControls/ExportGridViews/BookExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BooksGroupsExportGridView" Src="~/UserControls/ExportGridViews/BookGroupsExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogsGridView" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>


<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <script type="text/javascript">
        function onExportBookGroupsEndCallback(s, e) {
            btnExportBookCatalogsHidden.DoClick();
        }
    </script>
    <my:BookSearchFilters ID="ucSearchFilters" runat="server" />

    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnExport" runat="server" Text="Εξαγωγή Στοιχείων Βιβλίου" Image-Url="~/_img/iconExcel.png" OnClick="btnExport_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnExportBookCatalogs" runat="server" Text="Εξαγωγή Διανομών Βιβλίου" Image-Url="~/_img/iconExcel.png">
                        <ClientSideEvents Click="showExportBookCatalogs" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>

    <my:BooksGridView ID="gvBooks" ClientInstanceName="gv" runat="server" DataSourceID="odsBooks" Mode="BooksPage" OnCustomCallback="gvBooks_CustomCallback">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="IsActive" Name="IsActive" Caption="Δυνατότητα τιμολόγησης και αποζημίωσης" Width="70px" VisibleIndex="7">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <%# ((bool)Eval("IsActive")) ? "ΝΑΙ" : "ΟΧΙ" %>
                    <a runat="server" class="img-btn tooltip" title="Επεξεργασία δυνατότητας τιμολόγησης και αποζημίωσης βιβλίου"
                        href='javascript:void(0);'
                        onclick='<%# string.Format("showEditBookActiveStatusPopup({0})", Eval("ID")) %>'>
                        <img src="/_img/iconReportEdit.png" alt="Επεξεργασία δυνατότητας τιμολόγησης και αποζημίωσης βιβλίου" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="Comments" Caption="Σχόλια" FieldName="Comments" Width="20px" VisibleIndex="7">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:BooksGridView>

    <my:BooksGridView ID="gvBooksExport" GridClientVisible="false" DataSourceForceStandardPaging="false" ClientInstanceName="gvBooksExport" runat="server" Mode="BooksPage" OnCustomCallback="gvBooks_CustomCallback">
    </my:BooksGridView>


    <asp:ObjectDataSource ID="odsBooks" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Books" OnSelected="odsBooks_Selected"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsBooks_Selecting">
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
