<%@ Page Title="Επεξεργασία Τραπεζών" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.BankEditPopup" CodeBehind="BankEditPopup.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>


<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript">
        function deleteInvoice(invoiceID) {
            ucIncomingTransfersInvoicesGridView.PerformCallback("delete:" + invoiceID);
        }

        function confirmDelete()
        {
            showConfirmBox("Διαγραφή τράπεζας","Είστε βέβαιος ότι θέλετε να διαγράψετε την τράπεζα;", onConfirm);
            return false;
        }

        function onConfirm()
        {
            btnDeleteBankHidden.DoClick();
        }
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <div>
        <table class="dv" width="100%">
            <thead>
                <tr>
                    <th colspan="4" class="popupHeader">Επεξεργασία τραπεζών</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>
                        Επιλογή Τράπεζας:
                    </th>
                    <td>
                        <dx:ASPxComboBox runat="server" ID="cmbBankInput" ClientInstanceName="cmbBankInput" AutoPostBack="true" ValueField="ID" ValueType="System.Int32" Width="350px"
                            OnInit="cmbBankInput_Init"  OnSelectedIndexChanged="cmbBankInput_SelectedIndexChanged">
                            <Columns>
                                <dx:ListBoxColumn FieldName="ID" Visible="false" />
                                <dx:ListBoxColumn FieldName="Name" Visible="true" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <th>Όνομα:</th>
                    <td>
                        <dx:ASPxTextBox runat="server" ID="txtBankName"></dx:ASPxTextBox>
                    </td>
                    <td colspan="2">
                        <dx:ASPxCheckBox runat="server" ID="chkIsBankEdit" Text="Είναι τράπεζα"></dx:ASPxCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <dx:ASPxButton runat="server" ID="btnSaveChanges" ClientInstanceName="btnSaveChanges" OnClick="btnSaveChanges_Click" Text="Αποθήκευση Αλλαγών"></dx:ASPxButton>
                        <dx:ASPxButton runat="server" ID="btnDeleteBank" ClientInstanceName="btnDeleteBank" Text="Διαγραφή Τράπεζας">
                            <ClientSideEvents Click="confirmDelete" />
                        </dx:ASPxButton>
                        <dx:ASPxButton runat="server" ID="btnDeleteBankHidden" ClientInstanceName="btnDeleteBankHidden" OnClick="btnDeleteBank_Click" ClientVisible="false"></dx:ASPxButton>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="br"></div>
    <div class="br"></div>
    <div>
        <table class="dv" width="100%">
            <thead>
                <tr>
                    <th colspan="4" class="popupHeader">Προσθήκη Νέας Τράπεζας</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>Όνομα:</th>
                    <td>
                        <dx:ASPxTextBox ID="txtBankNameInput" runat="server">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Όνομα' είναι υποχρεωτικό" ValidationGroup="vgDelete"></ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                    <td colspan="2">
                        <dx:ASPxCheckBox runat="server" ID="chkIsBank" Text="Είναι τράπεζα"></dx:ASPxCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <dx:ASPxButton runat="server" ID="btnAddNewBank" ClientInstanceName="btnAddNewBank" OnClick="btnAddNewBank_Click" CausesValidation="true" ValidationGroup="vgDelete" Text="Προσθήκη Τράπεζας"></dx:ASPxButton>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script type="text/javascript">
        function refresh() {
            ucIncomingTransfersInvoicesGridView.PerformCallback('refresh');
        }

        function doSubmit() {
            window.parent.popUp.hide();
        }
    </script>
</asp:Content>
