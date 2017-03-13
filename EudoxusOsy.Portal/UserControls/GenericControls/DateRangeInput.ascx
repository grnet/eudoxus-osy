<%@ Control Inherits="EudoxusOsy.Portal.UserControls.GenericControls.DateRangeInput" CodeBehind="DateRangeInput.ascx.cs" Language="C#" AutoEventWireup="true" %>

<table style="width: 220px">
    <tr>
        <td style="padding-right: 5px;">Από:
        </td>
        <td>
            <dx:ASPxDateEdit ID="txtDateFrom" runat="server" Width="100px">
                <ValidationSettings RequiredField-ErrorText="Το πεδίο 'Από' είναι υποχρεωτικό" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" />
            </dx:ASPxDateEdit>
        </td>
        <td style="padding-left: 10px; padding-right: 5px;">Έως:
        </td>
        <td>
            <dx:ASPxDateEdit ID="txtDateTo" runat="server" Width="100px">
                <ValidationSettings RequiredField-ErrorText="Το πεδίο 'Έως' είναι υποχρεωτικό" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" />
            </dx:ASPxDateEdit>
        </td>
    </tr>
</table>
