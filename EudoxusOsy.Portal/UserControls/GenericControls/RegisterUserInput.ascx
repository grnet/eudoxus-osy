<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterUserInput.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.RegisterUserInput" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    <CompositeScript>
        <Scripts>
            <asp:ScriptReference Path="~/_js/Views/Registration.js" />
        </Scripts>
    </CompositeScript>
</asp:ScriptManagerProxy>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: <%= LabelWidth %>" />
    </colgroup>
    <tr>
        <th colspan="2" class="header">&raquo; <%= Header %>
        </th>
    </tr>
    <tr>
        <th>Όνομα Χρήστη:
        </th>
        <td>
            <table>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="txtUserName" runat="server" ClientInstanceName="txtUserName" Width="300px" CssClass="hint"
                            OnValidation="txtUserName_Validation" ToolTip="<%$ Resources:RegistrationInput, UserName %>">
                            <ClientSideEvents GotFocus="dx_Focus" LostFocus="userNameChecker" Validation="userNameCheckerValidate" />
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Όνομα Χρήστη' είναι υποχρεωτικό"
                                RegularExpression-ErrorText="Το Όνομα Χρήστη πρέπει να αποτελείται από τουλάχιστον 5 λατινικούς ή αριθμητικούς χαρακτήρες. Επιτρέπονται επιπλέον τα σύμβολα (.-_)" />
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <div id="userName-checking" class="loader" style="display: none;"></div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trPasswordInfo" runat="server">
        <td colspan="2">
            <div class="sub-description">
                Για τη δική σας ασφάλεια συνιστούμε να επιλέξετε έναν συνδυασμό από γράμματα, αριθμούς ή σύμβολα για να δημιουργήσετε έναν μοναδικό κωδικό πρόσβασης που δεν σχετίζεται με τα προσωπικά σας στοιχεία. Ή, επιλέξτε μια τυχαία λέξη ή φράση και εισαγάγετε λέξεις και αριθμούς στην αρχή, στη μέση και στο τέλος, για να είναι ακόμα πιο δύσκολο να τη μαντέψει κανείς (για παράδειγμα "m1awra1ap3tal0uda"). Η χρήση απλών λέξεων ή φράσεων όπως "password" ή "letmein", οι ακολουθίες πλήκτρων όπως "qwerty" ή "qazwsx" ή οι ακολουθίες διαδοχικών χαρακτήρων, όπως "abcd1234" κάνουν πιο εύκολη την αποκρυπτογράφηση του κωδικού σας. 
            </div>
        </td>
    </tr>
    <tr id="trPassword" runat="server">
        <th>Κωδικός Πρόσβασης:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtPassword" runat="server" Password="true" ClientInstanceName="txtPassword" CssClass="hint" MaxLength="256" AutoCompleteType="Disabled"
                Width="300px" ToolTip="<%$ Resources:RegistrationInput, Password %>">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κωδικός Πρόσβασης' είναι υποχρεωτικό"
                    RegularExpression-ErrorText="Ο Κωδικός Πρόσβασης πρέπει να αποτελείται από τουλάχιστον 8 χαρακτήρες, εκ των οποίων τουλάχιστον ένας να είναι αριθμητικός (0-9) και ένας ειδικός (!@#$%^&*). Επιτρέπονται μόνο λατινικοί, αριθμητικοί και οι παραπάνω ειδικοί χαρακτήρες." />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr id="trPasswordConfirmation" runat="server">
        <th>Επιβεβαίωση Κωδικού:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtPasswordConfirmation" runat="server" Password="true" ClientInstanceName="txtPasswordConfirmation" MaxLength="256" AutoCompleteType="Disabled" Width="300px">
                <ClientSideEvents Validation="function(s,e) { if (txtPassword.GetValue() != null) { e.isValid = txtPassword.GetValue() == txtPasswordConfirmation.GetValue(); } }" />
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Επιβεβαίωση Κωδικού' είναι υποχρεωτικό" ErrorText="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν" />
            </dx:ASPxTextBox>
            <%--<imis:CapsWarning runat="server" TextBoxControlId="txtPasswordConfirmation" CssClass="capsLockWarning" Text="Προσοχή: το πλήκτρο Caps Lock είναι πατημένο" />--%>
        </td>
    </tr>
    <tr id="trEmailInfo" runat="server">
        <td colspan="2">
            <div class="sub-description">
                <span style="font-size: 1em; font-weight: bold">Προσοχή:</span>
                <asp:Literal ID="ltrEmailInfo" runat="server" />
            </div>
        </td>
    </tr>
    <tr>
        <th>Email:
        </th>
        <td>
            <table>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="txtEmail" runat="server" ClientInstanceName="txtEmail" MaxLength="256" Width="300px" CssClass="hint"
                            OnValidation="txtEmail_Validation" ToolTip="<%$ Resources:RegistrationInput, Email %>">
                            <ClientSideEvents GotFocus="dx_Focus" LostFocus="emailChecker" Validation="emailCheckerValidate" />
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Email' είναι υποχρεωτικό"
                                RegularExpression-ErrorText="Το Email δεν είναι έγκυρο" />
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <div id="Email-checking" class="loader" style="display: none;"></div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trEmailConfirmation" runat="server">
        <th>Επιβεβαίωση Email:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtEmailConfirmation" runat="server" ClientInstanceName="txtEmailConfirmation" MaxLength="256" Width="300px">
                <ClientSideEvents Validation="function(s,e) { if (txtEmail.GetValue() != null) { e.isValid = txtEmail.GetValue() == txtEmailConfirmation.GetValue(); } }" />
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Επιβεβαίωση Email' είναι υποχρεωτικό" ErrorText="Τα πεδία 'Email' και 'Επιβεβαίωση Email' πρέπει να ταιριάζουν" />
            </dx:ASPxTextBox>
        </td>
    </tr>
</table>
