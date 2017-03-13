<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.SearchFilters.IncomingTransfersSearchFiltersControl" CodeBehind="IncomingTransfersSearchFiltersControl.ascx.cs" %>

<table class="dv">
    <colgroup>
        <col style="width: 90px" />
        <col style="width: 300px" />
        <col style="width: 100px" />
        <col style="width: 150px" />        
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>Αριθμός Παραστατικού:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtInvoiceNumber" runat="server" Width="100%" TabIndex="1" />
        </td>
        <th>Ημ/νία Παραστατικού:
        </th>
        <td>
            <dx:ASPxDateEdit ID="dateInvoiceDate" runat="server"/>
        </td>
    </tr>
    <tr>
        <th>Ποσό:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtInvoiceValue" runat="server" TabIndex="2" />
        </td>
    </tr>    
</table>
