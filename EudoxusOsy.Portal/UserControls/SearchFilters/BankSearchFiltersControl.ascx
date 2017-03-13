<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.BankSearchFiltersControl" CodeBehind="BankSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 60px" />
        <col style="width: 300px" />
        <col style="width: 100px" />
        <col style="width: 130px" />
        <col style="width: 60px" />
        <col style="width: 130px" />
    </colgroup>
    <tr>
        <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>Όνομα:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtName" runat="server" TabIndex="1" />
        </td>
        <th>Είναι Τράπεζα:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlIsBank" runat="server" ValueType="System.Int32" OnInit="ddlIsBank_Init" TabIndex="2" />
        </td>
        <th>Ενεργή:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlIsActive" runat="server" ValueType="System.Int32" OnInit="ddlIsActive_Init" TabIndex="3" />
        </td>
    </tr>
</table>
