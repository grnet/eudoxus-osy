<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.BookSearchFiltersControl" CodeBehind="BookSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 90px" />
        <col style="width: 300px" />
        <col style="width: 80px" />
        <col style="width: 300px" />
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>ID Βιβλίου:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtKpsID" runat="server" Width="100%" TabIndex="1" />
        </td>
        <th>Τίτλος:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtTitle" runat="server" TabIndex="4" />
        </td>
    </tr>
    <tr>
        <th>Συγγραφέας:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtAuthor" runat="server" TabIndex="2" />
        </td>
        <th>Εκδότης:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtPublisher" runat="server" TabIndex="5" />
        </td>
    </tr>
    <tr>
        <th>ISBN:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtISBN" runat="server" TabIndex="3" />
        </td>
        <th>Δυνατότητα τιμολόγησης και αποζημίωσης:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlIsActive" runat="server" ValueType="System.Int32" OnInit="ddlIsActive_Init" Width="120px" TabIndex="6" />
        </td>
    </tr>
</table>
