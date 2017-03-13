<%@ Page Title="Αλλαγή Κωδικού Πρόσβασης" MasterPageFile="~/Common/Common.Master" Inherits="EudoxusOsy.Portal.Common.ChangePassword" CodeBehind="ChangePassword.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="ChangePassword" TagPrefix="my" Src="~/UserControls/GenericControls/ChangePassword.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphCommonMain">
    <h2>Αλλαγή Κωδικού Πρόσβασης</h2>


    <asp:MultiView ID="mvChangePassword" runat="server" ActiveViewIndex="0">

        <asp:View ID="vChangePassword" runat="server">
            <div class="sub-description">
                <p>
                    <b>Επειδή είναι η πρώτη φορά που συνδέεστε στην εφαρμογή μετά την Υπενθύμιση Κωδικού
        Πρόσβασης παρακαλούμε αλλάξτε τον κωδικό που σας ήρθε με email.</b>
                </p>
                <p>
                    Για τη δική σας ασφάλεια σας συνιστούμε να επιλέξετε έναν συνδυασμό από γράμματα,
        αριθμούς ή σύμβολα για να δημιουργήσετε έναν μοναδικό κωδικό πρόσβασης που δεν σχετίζεται
        με τα προσωπικά σας στοιχεία. Ή, επιλέξτε μια τυχαία λέξη ή φράση και εισαγάγετε
        λέξεις και αριθμούς στην αρχή, στη μέση και στο τέλος, για να είναι ακόμα πιο δύσκολο
        να τη μαντέψει κανείς (για παράδειγμα &quot;m1awra1ap3tal0uda&quot;). Η χρήση απλών λέξεων
        ή φράσεων όπως &quot;password&quot; ή &quot;letmein&quot;, οι ακολουθίες πλήκτρων όπως &quot;qwerty&quot; ή &quot;qazwsx&quot;
        ή οι ακολουθίες διαδοχικών χαρακτήρων, όπως &quot;abcd1234&quot; κάνουν πιο εύκολη την αποκρυπτογράφηση
        του κωδικού σας. Επίσης, σε περίπτωση που συνδέεστε στο σύστημα από δημόσιο υπολογιστή,
        βεβαιωθείτε ότι πάντα πατάτε το κουμπί &quot;Αποσύνδεση&quot; πάνω δεξιά στην οθόνη κατά την
        έξοδό σας.
                </p>
            </div>

            <div class="br"></div>

            <my:ChangePassword ID="ucChangePassword" runat="server" ValidationGroup="vgChangePassword" />

            <div class="br"></div>

            <lc:BotShield ID="bsPublisher" runat="server" ClientIDMode="Static" ValidationGroup="vgChangePassword" />

            <div class="br"></div>

            <dx:ASPxButton ID="btnSubmit" runat="server" Text="Αλλαγή Κωδικού Πρόσβασης" Image-Url="~/_img/iconAccept.png" OnClick="btnSubmit_Click" ValidationGroup="vgChangePassword" />

            <div class="br"></div>
            <div class="br"></div>

            <div class="summaryContainer">
                <dx:ASPxValidationSummary runat="server" ValidationGroup="vgChangePassword" />
            </div>

            <dx:ASPxLabel ID="lblErrors" runat="server" ForeColor="Red" />

        </asp:View>

        <asp:View ID="vPasswordChanged" runat="server">

            <div class="br"></div>
            <div class="br"></div>

            <span style="font-weight: bold; color: Red">Η αλλαγή του κωδικού πρόσβασης πραγματοποιήθηκε επιτυχώς. Για να συνεχίσετε με τη χρήση της εφαρμογής πατήστε
            </span><a runat="server" style="font-weight: bold; color: Blue" href="../Default.aspx">εδώ</a>
        </asp:View>

    </asp:MultiView>
</asp:Content>
