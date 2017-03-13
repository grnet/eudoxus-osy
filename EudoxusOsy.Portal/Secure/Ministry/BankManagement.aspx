<%@ Page Title="Διαχείριση Τραπεζών" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="BankManagement.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.BankManagement" %>

<%@ Register TagPrefix="my" TagName="BanksSearchFilters" Src="~/UserControls/SearchFilters/BankSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="BanksGridView" Src="~/UserControls/GridViews/BanksGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">

    <my:BanksSearchFilters ID="ucSearchFilters" runat="server" />

    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
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

    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
