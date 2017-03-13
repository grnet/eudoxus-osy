<%@ Page Title="Επεξεργασία Εκχωρήσεων" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.IncomingTransfersPopup" CodeBehind="IncomingTransfersPopup.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagPrefix="my" TagName="IncomingTransfersSearchFilters" Src="~/UserControls/SearchFilters/IncomingTransfersSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="IncomingTransfersInvoicesGridView" Src="~/UserControls/GridViews/IncomingTransfersInvoicesGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>


<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript">
        function deleteInvoice(invoiceID)
        {
            ucIncomingTransfersInvoicesGridView.PerformCallback("delete:" + invoiceID);
        }
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <my:IncomingTransfersSearchFilters runat="server" ID="ucIncomingTransfersSearchFilters"></my:IncomingTransfersSearchFilters>
    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { refresh(); }" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    <my:IncomingTransfersInvoicesGridView runat="server" ID="ucIncomingTransfersInvoicesGridView" DataSourceForceStandardPaging="true" OnCustomCallback="ucIncomingTransfersInvoicesGridView_CustomCallback"
        ClientInstanceName="ucIncomingTransfersInvoicesGridView" DataSourceID="odsIncomingTransfers">
        <Columns>
            <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="20px" VisibleIndex="7">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <a class="img-btn tooltip" runat="server" href="javascript:void(0);" title="Διαγραφή"
                        onclick='<%# string.Format("deleteInvoice({0});return false;", Eval("ID")) %>'>
                        <img src="/_img/iconCancel.png" alt="Διαγραφή" /></a>                    
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:IncomingTransfersInvoicesGridView>

    <div class="br"></div>
    <div class="br"></div>
    <div>
        <table class="dv">
            <thead>
                <tr>
                    <th colspan="4"  class="popupHeader">Εισαγωγή εκχωρημένου τιμολογίου</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>Αριθμός Παραστατικού
                    </th>
                    <td>
                        <dx:ASPxTextBox runat="server" ID="txtInvoiceNumberInput"></dx:ASPxTextBox>
                    </td>
                    <th>Ημ/νία Παραστατικού</th>
                    <td>
                        <dx:ASPxDateEdit runat="server" ID="dateInvoiceDateInput"></dx:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <th>Ποσό</th>
                    <td>
                        <dx:ASPxSpinEdit runat="server" ID="spinAmountInput"></dx:ASPxSpinEdit>
                    </td>
                </tr>
                <tr>
                    <th>Τράπεζα</th>
                    <td>
                        <dx:ASPxComboBox runat="server" ID="cmbBankInput" ValueField="ID" ValueType="System.Int32" OnInit="cmbBankInput_Init">
                            <Columns>
                                <dx:ListBoxColumn FieldName="ID" Visible="false" />
                                <dx:ListBoxColumn FieldName="Name" Visible="true" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <dx:ASPxButton runat="server" ID="btnAddNewTransfer" ClientInstanceName="btnAddNewTrasnfer" OnClick="btnAddNewTransfer_Click" Text="Εισαγωγή εκχώρησης"></dx:ASPxButton>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <asp:ObjectDataSource ID="odsIncomingTransfers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.BankTransfers"
        SelectMethod="FindBySupplierIDAndPhaseID" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" MaximumRowsParameterName="maximumRows" StartRowIndexParameterName="startRowIndex" OnSelecting="odsIncomingTransfers_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        function refresh() {
            ucIncomingTransfersInvoicesGridView.PerformCallback('refresh');
        }

        function doSubmit()
        {
            window.parent.popUp.hide();
        }
    </script>
</asp:Content>
