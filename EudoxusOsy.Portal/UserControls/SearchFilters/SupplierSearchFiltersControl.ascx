<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.SupplierSearchFiltersControl" CodeBehind="SupplierSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 90px" />
        <col style="width: 300px" />
        <col style="width: 100px" />
        <col style="width: 150px" />        
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>ID Εκδότη:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSupplierKpsID" runat="server" Width="100%" TabIndex="1" />
        </td>
        <th>Τύπος Εκδότη:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlSupplierType" runat="server" ValueType="System.Int32" OnInit="ddlSupplierType_Init" TabIndex="4" />
        </td>
    </tr>
    <tr>
        <th>Επωνυμία:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtSupplierName" runat="server" TabIndex="2" />
        </td>
        <th>Κατάσταση:
        </th>
        <td runat="server" id="tdStatus">
            <dx:ASPxComboBox ID="ddlSupplierStatus" runat="server" ValueType="System.Int32" OnInit="ddlSupplierStatus_Init" TabIndex="5" />
        </td>
    </tr>    
    <tr>
        <th>Α.Φ.Μ.:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtSupplierAFM" runat="server" TabIndex="3" />
        </td>
        <th />
        <th />
    </tr>
</table>
