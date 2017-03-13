<%@ Control Inherits="EudoxusOsy.Portal.UserControls.GenericControls.NumberRangeInput" CodeBehind="NumberRangeInput.ascx.cs" Language="C#" AutoEventWireup="true" %>

<table>
    <tr>
        <td style="padding-right: 5px;">Από:
        </td>
        <td>
            <dx:ASPxSpinEdit ID="txtNumberFrom" runat="server" Width="50px" />
        </td>
        <td style="padding-left: 10px; padding-right: 5px;">Έως:
        </td>
        <td>
            <dx:ASPxSpinEdit ID="txtNumberTo" runat="server" Width="50px" />
        </td>
    </tr>
</table>
