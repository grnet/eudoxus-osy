<%@ Control Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.ChangePassword" CodeBehind="ChangePassword.ascx.cs" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 200px" />
    </colgroup>
    <tr>
        <th colspan="2" class="header">&raquo; Αλλαγή Κωδικού Πρόσβασης
        </th>
    </tr>
    <tr id="trOldPassword" runat="server">
        <th>Παλιός Κωδικός Πρόσβασης:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtOldPassword" runat="server" Password="true" ClientInstanceName="txtOldPassword" CssClass="rq" MaxLength="256" Width="90%">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Παλιός Κωδικός Πρόσβασης' είναι υποχρεωτικό" />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <th>Νέος Κωδικός Πρόσβασης:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtNewPassword" runat="server" Password="true" ClientInstanceName="txtNewPassword" CssClass="rq" MaxLength="256" Width="90%">
                <ClientSideEvents Validation="Imis.Lib.CheckPassword" />
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Νέος Κωδικός Πρόσβασης' είναι υποχρεωτικό" ErrorText="Ο Κωδικός Πρόσβασης πρέπει να αποτελείται από τουλάχιστον 8 χαρακτήρες, εκ των οποίων τουλάχιστον ένας να είναι αριθμητικός (0-9) και ένας ειδικός (!@#$%^&*). Επιτρέπονται μόνο λατινικοί, αριθμητικοί και οι παραπάνω ειδικοί χαρακτήρες." />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <th>Επιβεβαίωση Κωδικού:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtNewPasswordConfirmation" runat="server" Password="true" ClientInstanceName="txtNewPasswordConfirmation" CssClass="rq" MaxLength="256" Width="90%">
                <ClientSideEvents Validation="function(s,e) { if (txtNewPassword.GetValue() != null) { e.isValid = txtNewPassword.GetValue() == txtNewPasswordConfirmation.GetValue(); } }" />
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Επιβεβαίωση Κωδικού' είναι υποχρεωτικό" ErrorText="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν" />
            </dx:ASPxTextBox>
        </td>
    </tr>
</table>
