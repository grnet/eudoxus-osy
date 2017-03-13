<%@ Control Inherits="EudoxusOsy.Portal.UserControls.SupplierControls.ViewControls.SupplierMinistryView" CodeBehind="SupplierMinistryView.ascx.cs" Language="C#" AutoEventWireup="true" %>
<%@ Register TagName="FileUploadControl" TagPrefix="my" Src="~/Controls/ScriptControls/FileUpload.ascx" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 160px" />
    </colgroup>
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Εκδότη
        </th>
    </tr>
    <tr>
        <th>ID Εκδότη:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierKpsID" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τύπος Εκδότη:
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
    <%--    <tr>
        <th>Δ.Ο.Υ.:
        </th>
        <td>
            <dx:ASPxLabel ID="lblDOY" runat="server" />
        </td>
    </tr>--%>
    <tr>
        <th>Δ.Ο.Υ. Πληρωμών:
        </th>
        <td>
            <dx:ASPxLabel ID="lblPaymentPfo" runat="server" />
        </td>
    </tr>
    <tr>
        <th>IBAN:
        </th>
        <td>
            <dx:ASPxLabel ID="lblIBAN" runat="server" />
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
        <th>Υπεύθυνος Επικοινωνίας:
        </th>
        <td>
            <dx:ASPxLabel ID="lblContactName" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierPhone" runat="server" />
        </td>
    </tr>
    <tr>
        <th>ΦΑΞ:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierFax" runat="server" />
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
        <th>Ηλεκτρονική Διεύθυνση:
        </th>
        <td>
            <dx:ASPxLabel ID="lblSupplierUrl" runat="server" />
        </td>
    </tr>
    <tr runat="server" id="trNoLogisticBooks">
        <th>Μη Υπόχρεος Λογιστικών Βιβλίων:
        </th>
        <td>
            <dx:ASPxCheckBox ID="chkNoLogisticBooks" runat="server" />
        </td>
    </tr>
    <%--    <tr>
        <th>
            Ανέβασμα αρχείου:
        </th>
        <td>
            <my:FileUploadControl ID="ucUploadFiles" runat="server" />
        </td>
    </tr>--%>
</table>
