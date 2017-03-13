<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.UnconnectedCatalogSearchFiltersControl" CodeBehind="UnconnectedCatalogSearchFiltersControl.ascx.cs" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 120px" />
        <col style="width: 300px" />
        <col style="width: 85px" />
        <col style="width: 300px" />
        <col style="width: 110px" />
    </colgroup>
    <tr>
        <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>Ίδρυμα:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlInstitution" runat="server" ClientInstanceName="ddlInstitution" ValueType="System.Int32" OnInit="ddlInstitution_Init" TabIndex="1">
                <ClientSideEvents SelectedIndexChanged="function(s,e) { FillDepartments(s, e, '-- αδιάφορο --') }" />
            </dx:ASPxComboBox>
        </td>
        <th>Τίτλος:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtTitle" runat="server" TabIndex="3" />
        </td>
        <th>Κωδικός Βιβλίου:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtBookID" runat="server" Width="100%"  TabIndex="5" />
        </td>
    </tr>
    <tr>
        <th>Τμήμα/Βιβλιοθήκη:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlDepartment" runat="server" ClientInstanceName="ddlDepartment" ValueType="System.Int32" TabIndex="2" />
        </td>
        <th>Συγγραφέας:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtAuthor" runat="server" TabIndex="4" />
        </td>
        <th />
        <th />
    </tr>
</table>
