<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogInput.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.CatalogInput" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 175px" />
    </colgroup>
    <tr>
        <th class="header" colspan="2">&raquo; Στοιχεία Διανομής
        </th>
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
    <tr>
        <th>ID Βιβλίου:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtBookKpsID" runat="server">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Βιβλίου' είναι υποχρεωτικό" />
            </dx:ASPxSpinEdit>
        </td>
    </tr>
    <tr>
        <th>ID Εκδότη:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSupplierKpsID" runat="server">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Εκδότη' είναι υποχρεωτικό" />
            </dx:ASPxSpinEdit>
        </td>
    </tr>
    <tr>
        <th>ID Γραμματείας/Βιβλιοθήκης:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtSecretaryKpsID" runat="server">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ID Γραμματείας/Βιβλιοθήκης' είναι υποχρεωτικό" />
            </dx:ASPxSpinEdit>
        </td>
    </tr>    
    <tr>
        <th>Αριθμός Βιβλίων:
        </th>
        <td>
            <dx:ASPxSpinEdit ID="txtBookCount" runat="server">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Αριθμός Βιβλίων' είναι υποχρεωτικό" />
            </dx:ASPxSpinEdit>
        </td>
    </tr>
</table>
