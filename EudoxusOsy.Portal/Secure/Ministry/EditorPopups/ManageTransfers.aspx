<%@ Page Title="Διαχείριση Εκχωρήσεων" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ManageTransfers.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ManageTransfers" %>

<%@ Register TagName="TransfersGridView" TagPrefix="my" Src="~/UserControls/GridViews/TransfersGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <div class="gridViewTopButtons">
        <dx:ASPxButton ID="btnAddTransfer" runat="server" Text="Προσθήκη Εκχώρησης" Image-Url="~/_img/iconAddNewItem.png" />
    </div>

    <my:TransfersGridView ID="gvTransfers" runat="server" DataSourceID="odsTransfers" OnCustomCallback="gvTransfers_CustomCallback">
        <Columns>
            <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="70px" VisibleIndex="6">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <a runat="server" style="text-decoration: none" href="javascript:void(0)" class="tooltip" title="Επεξεργασία Παραστατικού"
                        onclick='<%# string.Format("showEditTransferPopup({0})", Eval("ID"))%>'>
                        <img src="/_img/iconEdit.png" alt="Επεξεργασία Παραστατικού" /></a>
                    <a runat="server" href="javascript:void(0);" class="tooltip" title="Διαγραφή Παραστατικού"
                        onclick='<%# string.Format("return doAction(\"delete\", {0}, \"ManageTransfers\", \"{1}\");", Eval("ID"), Eval("InvoiceNumber"))%>'>                        
                        <img src="/_img/iconDelete.png" alt="Διαγραφή Παραστατικού" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:TransfersGridView>

    <asp:ObjectDataSource ID="odsTransfers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Transfers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsTransfers_Selecting">
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
