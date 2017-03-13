<%@ Page Title="Σφάλμα" MasterPageFile="~/Common/Common.Master" Inherits="EudoxusOsy.Portal.Common.GeneralError" CodeBehind="GeneralError.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphCommonMain">
    <% if (IsUploadError)
        {%>
            <p>
                Το μέγεθος της φωτογραφίας που προσπαθήσατε να ανεβάσετε είναι μεγαλύτερο από το κανονικό.
            </p>    
            <p>
                Μεταβείτε στην <a href="~/Secure/Students/UploadPhoto.aspx" runat="server">προηγούμενη σελίδα</a> και ανεβάστε μια <b>φωτογραφία μικρότερη η ίση των 3ΜΒ</b>.
            </p>
        <%}
       else
        {%>
            <p>
                Παρουσιάστηκε κάποιο σφάλμα στην εφαρμογή. Ζητούμε συγνώμη για το πρόβλημα.
            </p>
            <p>
                Μπορείτε να μεταβείτε στην <a href="~/Default.aspx" runat="server">αρχική σελίδα</a> για να ξαναπροσπαθήσετε. 
                Μην προσπαθήσετε να χρησιμοποιήσετε τα πλήκτρα "back" ή "reload" του browser σας καθώς θα επαναληφθεί το ίδιο σφάλμα.
            </p>
            <p>
                <span style="font-weight: bold; color: Red">Αν το πρόβλημα επαναληφθεί μπορείτε να κλείσετε και να ανοίξετε ξανά τον browser
                που χρησιμοποιείτε ή να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών για να το αναφέρετε.</span>
            </p>
        <%}
    %>
</asp:Content>
