<%@ Page Title="Υπενθύμιση Κωδικού Πρόσβασης" MasterPageFile="~/Common/Common.Master" Inherits="EudoxusOsy.Portal.Common.ForgotPassword" CodeBehind="ForgotPassword.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphCommonMain">
    <h2>Υπενθύμιση Κωδικού Πρόσβασης
    </h2>

    <div class="sub-description">
        <p>
            Σε περίπτωση που ξεχάσατε τον κωδικό πρόσβασης, πληκτρολογήστε το email που είχατε
            δηλώσει κατά τη δημιουργία του λογαριασμού για να σταλεί ένας καινούργιος κωδικός.
            Τον κωδικό αυτό μπορείτε να τον αλλάξετε αφότου συνδεθείτε στο σύστημα.
        </p>
    </div>

    <table class="dv" style="width: 400px">
        <colgroup>
            <col style="width: 60px" />
        </colgroup>
        <tr>
            <th colspan="2" class="header">&raquo; Στοιχεία Χρήστη
            </th>
        </tr>
        <tr>
            <th>Email:
            </th>
            <td>
                <dx:ASPxTextBox ID="txtEmail" runat="server" MaxLength="256" Width="100%">
                    <ClientSideEvents Validation="Imis.Lib.CheckEmail" />
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Email' είναι υποχρεωτικό" ErrorText="Το Email δεν είναι έγκυρο" ValidationGroup="vgForgotPassword" />
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>

    <div class="br"></div>

    <lc:BotShield ID="bsPublisher" runat="server" ClientIDMode="Static" ValidationGroup="vgForgotPassword" />

    <div class="br"></div>

    <dx:ASPxButton ID="btnSendNewPassword" runat="server" Text="Αποστολή Κωδικού Πρόσβασης" Image-Url="~/_img/iconAccept.png" OnClick="btnSendNewPassword_Click" ValidationGroup="vgForgotPassword" />

    <div class="br"></div>
    <div class="br"></div>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgForgotPassword" />
    </div>

    <div class="br"></div>

    <asp:Label ID="lblInfo" runat="server" Font-Bold="true" ForeColor="Red" />
</asp:Content>
