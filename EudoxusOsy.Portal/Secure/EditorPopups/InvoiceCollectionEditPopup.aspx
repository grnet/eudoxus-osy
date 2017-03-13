<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="InvoiceCollectionEditPopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.InvoiceCollectionEditPopup" %>
<%@ Register TagName="InvoiceItemCollectionEdit" TagPrefix="my" Src="~/Controls/ScriptControls/InvoiceItemCollectionEdit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
        function SetBankSelected(s, e) {
            var tdBankSelectionID = '<%= tdBankSelection.ClientID %>';

            if (s.GetChecked()) {
                cmbSelectBank.SetClientVisible(true);
                cmbSelectBank.Validate();
            }
            else {
                cmbSelectBank.SetClientVisible(false);
            }
        }
    </script>
    <my:InvoiceItemCollectionEdit ID="InvoiceItemsEdit" runat="server"  />
    <div class="br"></div>
    <table>
        <tr>
            <td>
                <dx:ASPxCheckBox runat="server" Text="Να εκχωρηθεί το ποσό σε τράπεζα" ID="chkTransferToBank" ClientInstanceName="chkTransferToBank">
                    <ClientSideEvents CheckedChanged="SetBankSelected" />
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td runat="server" id="tdBankSelection">
                <dx:ASPxComboBox runat="server" ID="cmbSelectBank" Width="300px" ClientInstanceName="cmbSelectBank" ValueField="ID" ValueType="System.Int32">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Η επιλογή τράπεζας είναι υποχρεωτική" ValidationGroup="vgFormErrors" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                    <Columns>
                        <dx:ListBoxColumn FieldName="ID" Visible="false" />
                        <dx:ListBoxColumn FieldName="Name" Caption="Τράπεζα" Visible="true"/>
                    </Columns>
                </dx:ASPxComboBox>
                <input type="hidden" runat="server" id="hfIsTransferedToBank" />
            </td>
        </tr>
    </table>
    
    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />

    <script type="text/javascript">
        function doSubmit() {
            var hfID = '<%= hfIsTransferedToBank.ClientID %>';
            $('#' + hfID).val(chkTransferToBank.GetChecked());

            var invoiceItemCollection = $find('<%= InvoiceItemsEdit.ClientID %>');
            var confirm = invoiceItemCollection.isAmountSumLessThanTotal();

            if (ASPxClientEdit.AreEditorsValid()) {
                if (confirm) {
                    showConfirmBox('', 'Το συνολικό ποσό των παραστατικών είναι μικρότερο από το συνολικό ποσό της κατάστασης. Είστε σίγουρος/η ότι θέλετε να συνεχίσετε;', function () { btnSubmitHidden.DoClick(); }, null);
                }
                else {
                    btnSubmitHidden.DoClick();
                }
            }
            return false;
        }
    </script>

</asp:Content>
