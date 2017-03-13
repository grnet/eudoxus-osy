<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceInput.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.InvoiceControls.InputControls.InvoiceInput" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 150px" />        
    </colgroup>
    <tr>
        <th class="header" colspan="2">&raquo; Στοιχεία Παραστατικού
        </th>
    </tr>
    <tr>
        <th>Αριθμός Παραστατικού:
        </th>
        <td>            
            <dx:ASPxTextBox ID="txtInvoiceNumber" runat="server" Width="160px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Αριθμός Παραστατικού' είναι υποχρεωτικό." />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <th>Ημ/νία Έκδοσης:
        </th>
        <td>
            <dx:ASPxDateEdit ID="txtInvoiceDate" runat="server" Width="160px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ημ/νία Έκδοσης' είναι υποχρεωτικό." />
            </dx:ASPxDateEdit>
        </td>
    </tr>
    <tr>
        <th>Ποσό (χωρίς ΦΠΑ):
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtInvoiceValue" runat="server" NumberType="Float" DisplayFormatString="C" Width="160px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ποσό (χωρίς ΦΠΑ)' είναι υποχρεωτικό." />
            </dx:ASPxSpinEdit>
        </td>
    </tr>
</table>