<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransferInput.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.TransferInput" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 150px" />
    </colgroup>
    <tr>
        <th class="header" colspan="2">&raquo; Στοιχεία Εκχώρησης
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
        <th>Ημ/νία Παραστατικού:
        </th>
        <td>
            <dx:ASPxDateEdit ID="txtInvoiceDate" runat="server" Width="160px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ημ/νία Παραστατικού' είναι υποχρεωτικό." />
            </dx:ASPxDateEdit>
        </td>
    </tr>
    <tr>
        <th>Ποσό:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtInvoiceValue" runat="server" NumberType="Float" DisplayFormatString="C" Width="160px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ποσό' είναι υποχρεωτικό." />
            </dx:ASPxSpinEdit>
        </td>
    </tr>
    <tr>
        <th>Τράπεζα:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlBank" runat="server" ValueType="System.Int32" OnInit="ddlBank_Init" Width="320px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Τράπεζα' είναι υποχρεωτικό" />
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <th>Περίοδος Πληρωμών:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlPhase" runat="server" ValueType="System.Int32" OnInit="ddlPhase_Init" Width="320px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Περίοδος Πληρωμών' είναι υποχρεωτικό" />
            </dx:ASPxComboBox>
        </td>
    </tr>
</table>
