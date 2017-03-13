<%@ Control Inherits="EudoxusOsy.Portal.UserControls.MinistryControls.ViewControls.MinistryView" CodeBehind="MinistryView.ascx.cs" Language="C#" AutoEventWireup="true" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 160px" />
    </colgroup>
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Χρήστη
        </th>
    </tr>
    <tr>
        <th>Ον/μο:
        </th>
        <td>
            <dx:ASPxLabel ID="lblContactName" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο:
        </th>
        <td>
            <dx:ASPxLabel ID="lblContactPhone" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Email:
        </th>
        <td>
            <dx:ASPxLabel ID="lblContactEmail" runat="server" />
        </td>
    </tr>
    <tr>
        <th>Εξουσιοδότηση:
        </th>
        <td>
            <dx:ASPxLabel ID="lblAuthorizationType" runat="server" />
        </td>
    </tr>
</table>
