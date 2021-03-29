<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicStatsView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.MinistryControls.ViewControls.BasicStatsView" %>
<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 160px" />
    </colgroup>
    <tr>
        <th>Σύνολο βιβλίων
        </th>
        <td>
            <dx:ASPxLabel runat="server" ID="lblTotalBooks"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <th>Σύνολο κοστολογημένων βιβλίων
        </th>
        <td>
            <dx:ASPxLabel runat="server" ID="lblTotalPricedBooks"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <th>Μέση τιμή κοστολογημένων βιβλίων
        </th>
        <td>
            <dx:ASPxLabel runat="server" ID="lblAvgBookPrice"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <th>Συνολικό κόστος βιβλίων που έχουν παραληφθεί
        </th>
        <td>
            <dx:ASPxLabel runat="server" ID="lblTotalCostReceivedBooks"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <th>Συνολικό ποσό προς ΥΔΕ
        </th>
        <td>
            <dx:ASPxLabel runat="server" ID="lblTotalToYDEBooks"></dx:ASPxLabel>
        </td>
    </tr>
</table>
