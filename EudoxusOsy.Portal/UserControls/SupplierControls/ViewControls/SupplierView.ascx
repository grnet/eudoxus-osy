<%@ Control Inherits="EudoxusOsy.Portal.UserControls.SupplierControls.ViewControls.SupplierView" CodeBehind="SupplierView.ascx.cs" Language="C#" AutoEventWireup="true" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 160px" />
    </colgroup>
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Εκδότη
        </th>
    </tr>    
    <tr>
        <th>Κατηγορία:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierType" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Επωνυμία:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierName" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Διακριτικός Τίτλος:
        </th>
        <td>
            <dx:ASPxLabel ID="lblTradeName" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Α.Φ.Μ.:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierAFM" runat="server" />
        </td>
    </tr>    
    <tr>
        <th>Διεύθυνση:
        </th>
        <td>
            <dx:ASPxLabel ID="lblAddress" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τ.Κ.:
        </th>
        <td>
            <dx:ASPxLabel ID="lblZipCode" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Πόλη:
        </th>
        <td>
            <dx:ASPxLabel ID="lblCity" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Νομός:
        </th>
        <td>
            <dx:ASPxLabel ID="lblPrefecture" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Υπεύθυνος Επικοινωνίας:
        </th>
        <td>
            <dx:ASPxLabel ID="lblContactName" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (σταθερό):
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierPhone" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (κινητό):
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierMobilePhone" runat="server" />
        </td>
    </tr>    
    <tr>
        <th>Email:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierEmail" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Ιστοσελίδα:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierUrl" runat="server" />
        </td>
    </tr>    
</table>