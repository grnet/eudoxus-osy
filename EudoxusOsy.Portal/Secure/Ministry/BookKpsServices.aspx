<%@ Page Async="true" Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.Secure.Ministry.BookKpsServices"
    MasterPageFile="~/Secure/Ministry/Ministry.Master" Title="Services Βιβλίων ΚΠΣ"
    CodeBehind="BookKpsServices.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSecureMain" runat="server">
    <h1>Admin</h1>
    <p>
        Η ενημέρωση της βάσης συγγραμμάτων του ΟΣΥ με τα στοιχεία της βάσης συγγραμμάτων του Κεντρικού Πληροφοριακού Συστήματος του Ευδόξου πραγματοποιείται μία φορά κάθε ημέρα.
Σε περίπτωση που επιθυμείτε να ενημερωθεί άμεσα η βάση σχετικά με τυχόν αλλαγές στα συγγράμματα, παρακαλούμε όπως προχωρήσετε στις παρακάτω ενέργειες.
    </p>
    <div class="br"></div>
    <div style="text-align: left">
        <table class="dv" border="1">
            <colgroup>
                <col width="80%" />
                <col width="20%" />
            </colgroup>
            <tr>
                <th>
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnRunStats" OnClick="btnRunStats_OnClick" ClientInstanceName="btnRunStats"
                                   Text="Ενημέρωση Στατιστικών">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>Για την ενημέρωση της βάσης συγγραμμάτων του ΟΣΥ για συγκεκριμένα συγγράμματα από τα ΚΠΣ συμπληρώστε το αντίστοιχο πεδίο.
                </th>
                <td>
                    <asp:TextBox TextMode="MultiLine" runat="server" Rows="5" ID="txtBooksList" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <dx:ASPxButton runat="server" ID="btnGetSpecificBooks" OnClick="btnGetSpecificBooks_OnClick" 
                                   Text="Λήψη συγκεκριμένων βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>Για την ενημέρωση της βάσης συγγραμμάτων του ΟΣΥ με τα νέα συγγράμματα που έχουν δημιουργηθεί στο ΚΠΣ πατήστε το κουμπί.
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnGetNewBooks" OnClick="btnGetNewBooks_Click" ClientInstanceName="btnGetNewBooks"
                                   Text="Λήψη νέων βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>
                        
            <tr>
                <th>Για την ενημέρωση της βάσης συγγραμμάτων του Οικονομικού Συστήματος Υπουργείου (ΟΣΥ) σχετικά με τις μεταβολές που έχουν πραγματοποιηθεί στα στοιχεία των συγγραμμάτων στο Κεντρικό Πληροφοριακό Σύστημα του Ευδόξου (ΚΠΣ) πατήστε το κουμπί.
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnGetModifiedBooks" OnClick="btnGetModifiedBooks2_Click" ClientInstanceName="btnGetModifiedBooks"
                        Text="Στάδιο 1" ToolTip="Επικοινωνία με ΚΠΣ">
                    </dx:ASPxButton>
               
                    <dx:ASPxButton runat="server" ID="btnUpdateModifiedBooks" OnClick="btnUpdateModifiedBooks_OnClick" ClientInstanceName="btnUpdateModifiedBooks"
                                   Text="Στάδιο 2" ToolTip="Ενημέρωση δεδομένων">
                    </dx:ASPxButton>
                    <dx:ASPxButton runat="server" ID="btnUpdateModifiedBooksPrices" OnClick="btnUpdateModifiedBooksPrices_OnClick" ClientInstanceName="btnUpdateModifiedBooksPrices"
                                   Text="Στάδιο 3" ToolTip="Ενημέρωση τιμών">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <td>-</td>
                <td>-</td>
            </tr>
            <tr>
                <th>Απευθείας Ενημέρωση Status Νέων Βιβλίων στο ΚΠΣ !! Προσοχή δεν συγχρονίζει με το ΟΣΥ πρώτα.
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnUpdateNewBooksDirectly" OnClick="btnUpdateNewBooksDirectly_Click" ClientInstanceName="btnUpdateNewBooksDirectly"
                                   Text="Απευθείας Ενημέρωση Νέων Βιβλίων στο ΚΠΣ !! Προσοχή δεν συγχρονίζει με το ΟΣΥ πρώτα.">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>Απευθείας Ενημέρωση Status Αλλαγμένων Βιβλίων στο ΚΠΣ !! Προσοχή δεν συγχρονίζει με το ΟΣΥ πρώτα.
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnUpdateModifiedBooksDirectly" OnClick="btnUpdateModifiedBooksDirectly_Click" ClientInstanceName="btnUpdateModifiedBooksDirectly"
                                   Text="Απευθείας Ενημέρωση Αλλαγμένων Βιβλίων στο ΚΠΣ !! Προσοχή δεν συγχρονίζει με το ΟΣΥ πρώτα.">
                    </dx:ASPxButton>
                </td>
            </tr>
            
            
            <%--            <tr>
                <th>Αρχικοποίηση νέων βιβλίων - ΠΡΟΣΟΧΗ !!! θέτει όλα τα βιβλία ως συγχρονισμένα
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnInitKpsNewBooks" OnClick="btnInitKpsNewBooks_Click" ClientInstanceName="btnGetNewBooks"
                        Text="Αρχικοποίηση νέων βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>--%>
            <%--            <tr>
                <td>
                    <dx:ASPxTextBox runat="server" ID="txtUpdateBookStatusID"></dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton runat="server" ID="btnUpdateBookStatus" OnClick="btnUpdateBookStatus_Click" ClientInstanceName="btnUpdateBookStatus"
                        Text="Ενημέρωση κατάστασης βιβλίου ΚΠΣ">
                    </dx:ASPxButton>
                </td>
            </tr>--%>
                        
        </table>

        <div class="br"></div>
        <p>Καταγραφή αποτελεσμάτων</p>
        <div class="br"></div>
        <div class="br"></div>
        <dx:ASPxLabel runat="server" ID="lblPRogress" ClientInstanceName="lblPRogress"></dx:ASPxLabel>
        <div class="br"></div>
        <asp:UpdatePanel runat="server" ID="upResults" UpdateMode="Always">
            <ContentTemplate>
                <asp:TextBox Width="100%" runat="server" TextMode="MultiLine" ID="txtResults" Rows="5">
                </asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
