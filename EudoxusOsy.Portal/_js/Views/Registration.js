$(function () {
    if (typeof (txtPasswordConfirmation) !== 'undefined') {
        txtPasswordConfirmation.GetInputElement().setAttribute('onpaste', 'return false');
    }

    if (typeof (txtEmailConfirmation) !== 'undefined') {
        txtEmailConfirmation.GetInputElement().setAttribute('onpaste', 'return false');
    }
});

var userNamesChecked = {};
var emailsChecked = {};

function userNameChecker(s, e) {
    dx_Blur(s, e);

    var userName = s.GetValue();

    if (!(userName))
        return;

    if (!Imis.Lib.IsUserNameValid(userName))
        return;

    var checkResult = userNamesChecked[userName];

    if (checkResult == -1)
        return;
    else if (checkResult === undefined)
        userNamesChecked[userName] = -1;
    else
        return;

    $('#userName-checking').show();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/PortalServices/Services.asmx/UserNameExists",
        data: JSON.stringify({ "userName": userName }),
        dataType: "json",
        async: true,
        success: function (data, textStatus) {
            if (textStatus == "success") {
                if (data.hasOwnProperty('d')) {
                    msg = data.d;
                } else {
                    msg = data;
                }
                userNamesChecked[userName] = !msg;
                s.Validate();
            }
            $('#userName-checking').hide();
        },
        error: function (data, status, error) {
            $('#userName-checking').hide();
            showAlertBox("Παρουσιάστηκε κάποιο σφάλμα. Παρακαλώ δοκιμάστε αργότερα.");
        }
    });
}

function userNameCheckerValidate(s, e) {
    var userName = s.GetValue();

    if (!(userName))
        return;

    Imis.Lib.CheckUserName(s, e);
    if (!e.isValid) {
        e.errorText = "Το πεδίο 'Όνομα Χρήστη' δεν είναι έγκυρο. Επιτρέπονται μόνο λατινικοί χαρακτήρες, αριθμητικοί (π.χ. 1,2,3) ή ειδικοί (_, - και .). Ελάχιστο μήκος 5 χαρακτήρες.";
        return;
    }

    var checkResult = userNamesChecked[userName];
    e.isValid = (checkResult == -1 || checkResult === undefined);

    if (e.isValid) {
        return;
    }

    e.isValid = checkResult;
    e.errorText = "Το Όνομα Χρήστη χρησιμοποιείται";
}

function emailChecker(s, e) {
    dx_Blur(s, e);

    var email = s.GetValue();

    if (!(email))
        return;

    if (!Imis.Lib.IsEmailValid(email))
        return;

    var checkResult = emailsChecked[email];

    if (checkResult == -1)
        return;
    else if (checkResult === undefined)
        emailsChecked[email] = -1;
    else
        return;

    if (email.length >= 5) {
        $('#email-checking').show();

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/PortalServices/Services.asmx/emailExists",
            data: JSON.stringify({ "email": email }),
            dataType: "json",
            async: true,
            success: function (data, textStatus) {
                if (textStatus == "success") {
                    if (data.hasOwnProperty('d')) {
                        msg = data.d;
                    } else {
                        msg = data;
                    }
                    emailsChecked[email] = !msg;
                    s.Validate();
                }
                $('#email-checking').hide();
            },
            error: function (data, status, error) {
                $('#email-checking').hide();
                showAlertBox("Παρουσιάστηκε κάποιο σφάλμα. Παρακαλώ δοκιμάστε αργότερα.");
            }
        });
    }
}

function emailCheckerValidate(s, e) {
    var email = s.GetValue();

    if (!(email))
        return;

    Imis.Lib.CheckEmail(s, e);
    if (!e.isValid) {
        e.errorText = "Το Email δεν είναι έγκυρο";
        return;
    }

    var checkResult = emailsChecked[email];
    e.isValid = (checkResult == -1 || checkResult === undefined);

    if (e.isValid) {
        return;
    }

    e.isValid = checkResult;
    e.errorText = "Το Email χρησιμοποιείται";
}