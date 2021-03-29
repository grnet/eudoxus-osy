<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookPriceVerificationFiltersControl.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.BookPriceVerificationFiltersControl" %>
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
        <th>Τίτλος Βιβλίου:
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
        <th>
            <span id="spanHasBookPriceChanges" runat="server">Έχει έρθει τιμή Υπουργείου:</span>
        </th>
        <td>            
            <dx:ASPxComboBox ID="ddlHasBookPriceChanges" runat="server"
                ValueType="System.Int32" OnInit="ddlHasBookPriceChanges_Init" Width="120px" TabIndex="6" />
        </td>
    </tr>
    <tr>
        <th>Έτος:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtChangeYear" runat="server" TabIndex="6"  />
        </td>
        <th>
        </th>
        <td>            
        </td>
    </tr>
</table>
