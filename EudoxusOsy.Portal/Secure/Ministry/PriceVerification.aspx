<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PriceVerification.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PriceVerification" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">    
    
    <p>Λίστα βιβλίων που βρίσκονται υπό διαδικασία ελέγχου κοστολόγησης από την Επιτροπή Ελέγχου Κοστολόγησης Διδακτικών Συγγραμμάτων. Για την προβολή της λίστας και την έγκριση των τιμών των βιβλίων αυτών παρακαλούμε όπως πατήσετε εδώ: </p>
    <a href="PriceVerificationMinistry.aspx">Βιβλία Υπό Διαδικασία Ελέγχου</a>
    
    <p>Λίστα βιβλίων που τροποποιήθηκε η τιμή τους χωρίς προηγούμενη ενημέρωση από την Επιτροπή Ελέγχου Κοστολόγησης Διδακτικών Συγγραμμάτων για την υπαγωγή τους στη λίστα των βιβλίων προς έλεγχο. Για την προβολή των βιβλίων και τη διαχείριση της τιμής τους παρακαλούμε όπως πατήσετε εδώ: </p>
    <a href="PriceVerificationUnexpected.aspx">Βιβλία με μη αναμενόμενες αλλαγές τιμών</a>

    <p>Προβολή Ιστορικού αλλαγών τιμών</p>
    <a href="PriceVerificationHistory.aspx">Ιστορικό</a>

</asp:Content>