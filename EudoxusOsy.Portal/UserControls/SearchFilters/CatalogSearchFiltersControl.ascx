<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.CatalogSearchFiltersControl" CodeBehind="CatalogSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 180px" />
        <col style="width: 160px" />
        <col style="width: 210px" />
        <col style="width: 200px" />
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>ID Διανομής:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtCatalogID" runat="server" TabIndex="1" />
        </td>
        <th>ID Κατάστασης:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtGroupID" runat="server" TabIndex="2" />
        </td>
    </tr>
    <tr>
        <th>Φάση:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlPhase" runat="server" ValueType="System.Int32" OnInit="ddlPhase_Init" TabIndex="3" />
        </td>
        <th>ID Βιβλίου:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtBookKpsID" runat="server" TabIndex="4" />
        </td>
    </tr>
    <tr>
        <th>ID Εκδότη:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSupplierKpsID" runat="server" TabIndex="5" />
        </td>
        <th>Ποσοστό:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtPercentage" runat="server" TabIndex="6" />
        </td>
    </tr>
    <tr>
        <th>Διανομή σε:
        </th>
        <td>
            <dx:ASPxComboBox runat="server" ValueType="System.Int32" ID="ddlIsLibrary" TabIndex="7">
                <Items>
                    <dx:ListEditItem Text="-- αδιάφορο --" Value="0" />
                    <dx:ListEditItem Text="Βιβλιοθήκη" Value="1" />
                    <dx:ListEditItem Text="Φοιτητές" Value="2" />
                </Items>
            </dx:ASPxComboBox>
        </td>
        <th>ID Γραμματείας/Βιβλιοθήκης:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSecretaryKpsID" runat="server" TabIndex="8" />
        </td>
    </tr>
    <tr>
        <th>Αριθμός Αντιτύπων:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtBookCount" runat="server" TabIndex="9" />
        </td>
        <th>Έκπτωση:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtDiscountPercentage" runat="server" TabIndex="10" />
        </td>
    </tr>
    <tr>
        <th>Κόστος Διανομής:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtAmount" runat="server" TabIndex="11" />
        </td>
        <th>Ανήκει σε Kατάσταση:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlIsInGroup" runat="server" OnInit="ddlIsInGroup_Init" TabIndex="12" />
        </td>
    </tr>
    <tr>
        <th>Στάδιο της κατάστασης στην οποία ανήκει η διανομή:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlGroupState" runat="server" OnInit="ddlGroupState_Init" TabIndex="13" />
        </td>
        <th>Είδος Διανομής:
        </th>
        <td>
            <dx:ASPxComboBox runat="server" ID="ddlCatalogType" OnInit="ddlCatalogType_Init" TabIndex="14"></dx:ASPxComboBox>
        </td>
        <%--<th>
            Status Διανομής:
        </th>
        <td>
            <dx:ASPxComboBox runat="server" ID="ddlCatalogStatus" OnInit="ddlCatalogStatus_Init"></dx:ASPxComboBox>
        </td>--%>
    </tr>
    <tr>
        <th>Ημ/νία Δημιουργίας:
        </th>
        <td>
            <dx:ASPxDateEdit ID="txtCreatedAt" runat="server" TabIndex="15" />
        </td>
        <th>State διανομής:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlCatalogState" runat="server" OnInit="ddlCatalogState_Init" TabIndex="16" />
        </td>
    </tr>
</table>
