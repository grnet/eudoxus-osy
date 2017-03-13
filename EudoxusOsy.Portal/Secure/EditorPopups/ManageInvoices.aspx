<%@ Page Title="Προβολή Παραστατικών" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ManageInvoices.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ManageInvoices" %>

<%@ Register TagName="InvoicesGridView" TagPrefix="my" Src="~/UserControls/GridViews/InvoicesGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="divWarning" runat="server" class="alert warning" visible="false">
                Το συνολικό ποσό των παραστατικών είναι μικρότερο από το συνολικό ποσό της κατάστασης.
            </div>

            <div id="divError" runat="server" class="alert error" visible="false">
                Το συνολικό ποσό των παραστατικών είναι μεγαλύτερο από το συνολικό ποσό της κατάστασης.<br />
                Για να μπορέσετε να την επιλέξετε για πληρωμή θα πρέπει είτε να προσθέσετε ασύνδετες διανομές είτε να μειώσετε το ποσό των παραστατικών.
            </div>

            <div class="gridViewTopButtons">
                <dx:ASPxButton ID="btnAddInvoice" runat="server" Text="Προσθήκη Παραστατικού" Image-Url="~/_img/iconAddNewItem.png" />
            </div>

            <my:InvoicesGridView ID="gvInvoices" runat="server" DataSourceID="odsInvoices" OnCustomCallback="gvInvoices_CustomCallback">
                <Columns>
                    <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="70px" VisibleIndex="5">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" class="tooltip" title="Επεξεργασία Παραστατικού"
                                onclick='<%# string.Format("showEditInvoicePopup({0})", Eval("ID"))%>'>
                                <img src="/_img/iconEdit.png" alt="Επεξεργασία Παραστατικού" /></a>
                            <a runat="server" href="javascript:void(0);" class="tooltip" title="Διαγραφή Παραστατικού"
                                onclick='<%# string.Format("deleteInvoice({0}, \"{1}\");", Eval("ID"), Eval("InvoiceNumber")) %>'>
                                <img src="/_img/iconDelete.png" alt="Διαγραφή Παραστατικού" /></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </my:InvoicesGridView>

            <div style="display: none">
                <asp:Button ID="btnDeleteInvoice" runat="server" OnClick="btnDeleteInvoice_Click" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" />
                <input type="hidden" id="hfInvoiceID" runat="server" />
            </div>

            <asp:ObjectDataSource ID="odsInvoices" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Invoices"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsInvoices_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function deleteInvoice(invoiceID, invoiceNumber) {
            var doDelete = function () {
                $('#<%= hfInvoiceID.ClientID %>').val(invoiceID);
                <%= ClientScript.GetPostBackEventReference(btnDeleteInvoice, null) %>
            };

            showConfirmBox('Διαγραφή Παραστατικού', 'Είστε σίγουροι ότι θέλετε να διαγράψετε το παραστατικό με αριθμό \'' + invoiceNumber + '\';', doDelete);
        }

        function cmdRefresh() {
            <%= ClientScript.GetPostBackEventReference(btnRefresh, null) %>
        }
    </script>
</asp:Content>
