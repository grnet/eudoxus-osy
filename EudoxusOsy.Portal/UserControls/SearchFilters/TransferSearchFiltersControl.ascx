<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.TransferSearchFiltersControl" CodeBehind="TransferSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 100px" />
        <col style="width: 230px" />
        <col style="width: 100px" />
        <col style="width: 200px" />
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης Εκχωρήσεων
        </th>
    </tr>
    <tr>
        <th>Αριθμός Παραστατικού:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtTransferNumber" runat="server" TabIndex="1" />
        </td>
        <th>Ημ/νία Παραστατικού:
        </th>
        <td>
            <dx:ASPxDateEdit ID="dateInvoiceDate" runat="server" TabIndex="6" />
        </td>
    </tr>
    <tr>
        <th>ID Εκδότη:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSupplierKpsID" Width="100%" runat="server" TabIndex="3" />
        </td>
        <th>Επωνυμία Εκδότη:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtSupplierName" runat="server" TabIndex="4" />
        </td>
    </tr>
    <tr>
        <th>Τράπεζα:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlBank" runat="server" ValueType="System.Int32" Width="100%" OnInit="ddlBank_Init" TabIndex="2" />
        </td>
        <th>Περίοδος Πληρωμών:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlPhase" Width="100%" runat="server" TabIndex="5" OnInit="ddlPhase_Init" />
        </td>
    </tr>
</table>
