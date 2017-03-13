<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EmailVerificationInfo.aspx.cs"
    Inherits="EudoxusOsy.Portal.EmailVerificationInfo" Title="Οδηγίες Επιβεβαίωσης Email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="reminder" style="text-align: left; font-weight: normal;">
        Κατά την εγγραφή σας στην εφαρμογή, σας στάλθηκε email επιβεβαίωσης με τίτλο:
        <br />
        <br />
        <b>[ΨΗΦΙΑΚΗ ΑΛΛΗΛΕΓΓΥΗ] <asp:Literal ID="ltSubject" runat="server" /></b>
        <br />
        <br />
        Εάν το έχετε λάβει , πατήστε το link που έχει στο κείμενό του, ώστε να επιβεβαιώσετε
        το email του λογαριασμού σας.
        <br />
        <br />
        Εάν δεν το έχετε λάβει μπορεί να έχουν συμβεί τα
        εξής:
        <ul>
            <li class="firstListItem">Να μην έχετε δηλώσει σωστά το email σας. Πηγαίνετε στην καρτέλα «Στοιχεία Λογαριασμού»
                για να δείτε το email που έχετε δηλώσει και, εάν έχετε κάνει λάθος να το διορθώσετε.
                Μόλις το διορθώσετε, θα σας έρθει νέο email επιβεβαίωσης.</li>
            <li class="firstListItem">Το email επιβεβαίωσης που σας στάλθηκε, να έχει μαρκαριστεί ως SPAM και να έχει
                καταλήξει στην Ανεπιθύμητη Αλληλογραφία του γραμματοκιβωτίου σας. Ψάξτε, λοιπόν,
                στον φάκελο της Ανεπιθύμητης Αλληλογραφίας (Junk). Σε αυτήν την περίπτωση, να έχετε
                υπόψη σας ότι κάθε email που σας στέλνει η εφαρμογή θα καταλήγει στην Ανεπιθύμητη
                Αλληλογραφία σας. Για να το αποφύγετε αυτό, μπορείτε να ορίσετε ένα διαφορετικό
                email λογαριασμό (εφόσον διαθέτετε) που να μην μαρκάρει τα email της εφαρμογής
                ως Ανεπιθύμητη Αλληλογραφία.</li>
        </ul>
    </div>
</asp:Content>
