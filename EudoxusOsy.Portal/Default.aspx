<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EudoxusOsy.Portal.Default" Title="Αρχική Σελίδα" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cpHelpBar">
    <div id="primary-menu">
        <ul>
            <li><a class="home" href='/Default.aspx'>Αρχική Σελίδα</a></li>            
            <%--<li><a class="contact" href='/Browse/ContactForm.aspx'>Επικοινωνία</a></li>--%>
        </ul>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <div id="registration" class="column" style="display: none;">
        <h3>Εγγραφή</h3>
        <div class="inner">
            <p>
                <label>
                    Για να εγγραφείτε στην εφαρμογή, επιλέξτε την κατηγορία χρήστη που ανήκετε:</label>
            </p>
            <div class="buttonwrapper">
                <a runat="server" clientidmode="Static" class="squarebutton beneficiary" href="#"><span>Δικαιούχος</span></a>
                <a runat="server" clientidmode="Static" class="squarebutton beneficiary" href="#"><span>Πάροχος</span></a>
            </div>
        </div>
    </div>
    <div id="login" class="column" style="text-align: left; width: 86.5em">
        <h3 style="margin-left: 24px">Είσοδος</h3>
        <div class="inner">
            <p>
                <label>
                    Για να συνδεθείτε στην εφαρμογή, εισάγετε το Όνομα Χρήστη και τον Κωδικό Πρόσβασης που δηλώσατε κατά την εγγραφή σας.</label>
            </p>
            <div id="login-form" style="text-align: left">
                <asp:Login runat="server" ID="login1" DestinationPageUrl="~/Default.aspx" LoginButtonText="Σύνδεση"
                    PasswordLabelText="Κωδικός πρόσβασης:" PasswordRecoveryText="Ξέχασα τον κωδικό μου"
                    PasswordRecoveryUrl="~/Common/ForgotPassword.aspx" PasswordRequiredErrorMessage="Ο κωδικός πρόσβασης είναι υποχρεωτικός"
                    RememberMeText="Θυμήσου με" TitleText="" UserNameLabelText="Όνομα χρήστη:" UserNameRequiredErrorMessage="Το όνομα χρήστη είναι υποχρεωτικό"
                    FailureText="Λάθος όνομα χρήστη ή κωδικός πρόσβασης." OnLoggingIn="login1_LoggingIn" OnLoggedIn="login1_LoggedIn">
                    <LayoutTemplate>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Όνομα χρήστη:</asp:Label></span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="UserName" runat="server" Columns="30"></asp:TextBox></span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" Display="Dynamic"
                                ControlToValidate="UserName" ErrorMessage="Το όνομα χρήστη είναι υποχρεωτικό"
                                ValidationGroup="login1">Το όνομα χρήστη είναι υποχρεωτικό</asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Κωδικός πρόσβασης:</asp:Label></span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" Columns="30"></asp:TextBox></span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" Display="Dynamic"
                                ControlToValidate="Password" ErrorMessage="Ο κωδικός πρόσβασης είναι υποχρεωτικός"
                                ValidationGroup="login1">Ο κωδικός πρόσβασης είναι υποχρεωτικός</asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <span class="label login-error">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                        </div>
                        <div class="row">
                            <span class="button">
                                <asp:LinkButton ID="LoginButton" runat="server" CommandName="Login" CssClass="icon-btn"
                                    Text="Σύνδεση" ValidationGroup="login1" /></span> <span class="check">
                                        <asp:CheckBox ID="RememberMe" runat="server" Text="Θυμήσου με" /></span>
                        </div>                        
                    </LayoutTemplate>
                </asp:Login>
            </div>
            <p>
                Εάν αντιμετωπίζετε πρόβλημα σύνδεσης με το λογαριασμό σας, μπορείτε να επικοινωνήσετε με το <a href='/Browse/ContactForm.aspx'>Γραφείο Αρωγής Χρηστών</a>
            </p>

            <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
            
        </div>
    </div>
    <script type="text/javascript">

        $(function () {
            $('.login-btn-form').click(function () {
                $('#login-form').slideToggle();
                return false;
            });

            <% if (IsPostBack)
               { %>
            $('#login-form').slideDown();
            <% } %>

        });
        
    </script>

</asp:Content>
