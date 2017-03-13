var btnEmail;
var btnMobilePhone;
var btnUnlock;
var emailError;
var mobilePhoneError;
var unlockError;

$.fn.extend({
    dropIn: function (speed, callback) {
        var $t = $(this);

        if ($t.css("display") == "none") {
            eltop = $t.css('top');
            elouterHeight = $t.outerHeight(true);

            $t.css({ top: -elouterHeight, display: 'block' }).animate({ top: eltop }, speed, 'swing', callback);
        }
    }
});

$(function () {
    btnEmail = $('#btnEmail');
    btnMobilePhone = $('#btnMobilePhone');
    btnMobilePhoneForeign = $('#btnMobilePhoneForeign');
    btnUnlock = $('#btnUnlock');
    emailError = $('#emailError');
    mobilePhoneError = $('#mobilePhoneError');
    unlockError = $('#mobilePhoneError');

    if (btnEmail.length != 0) {
        btnEmail.click(showEmailPopup);
    }

    if (typeof(btnAddEmail) !== 'undefined') {
        btnAddEmail.Click.AddHandler(showEmailPopup);
    }

    if (btnMobilePhone.length != 0) {
        btnMobilePhone.click(showMobilePhonePopup);
    }

    if (typeof (btnAddMobilePhone) !== 'undefined') {
        btnAddMobilePhone.Click.AddHandler(showMobilePhonePopup);
    }

    if (btnMobilePhoneForeign.length != 0) {
        btnMobilePhoneForeign.click(function () {
            var onblur = "$(this).removeClass('focused');";
            var onfocus = "$(this).addClass('focused');";
            var txt = 'Νέο Κινητό: <input type="text" class="tb" onfocus="' + onfocus + '" onblur="' + onblur + '" id="mobilePhoneForeign" name="mobilePhoneForeign" value="" maxlength="10" />';
            var dialog = $('<div>' + txt + '</div>')
             .dialog({
                 modal: true,
                 resizable: false,
                 draggable: true,
                 width: 360,
                 title: 'Αλλαγή Κινητού',
                 show: 'fade',
                 hide: 'fade',
                 dialogClass: 'main-dialog-class',
                 buttons: {
                     "Αλλαγή Κινητού": function () {
                         beginChangeMobilePhoneForeign(dialog);
                         dialog.dialog('close');
                     },
                     "Ακύρωση": function () {
                         dialog.dialog('close');
                     }
                 },
                 open: function () {
                     $(this).parent().find('.ui-dialog-buttonpane button:nth-child(1)').button({
                         icons: { primary: 'bg-accept' }
                     });
                     $(this).parent().find('.ui-dialog-buttonpane button:nth-child(2)').button({
                         icons: { primary: 'bg-cancel' }
                     });
                 }
             });
            //$.prompt(txt, { callback: beginChangeMobilePhoneForeign, show: 'dropIn', buttons: { 'Αλλαγή Κινητού': true, Ακύρωση: false } });
            return false;
        });
    }

    if (btnUnlock.length != 0) {
        btnUnlock.click(function () {
            btnUnlock.addClass('bg-loading');
            begin(unlock);
            return false;
        });
    }
});

function showEmailPopup() {
    var onblur = "$(this).removeClass('focused');";
    var onfocus = "$(this).addClass('focused');";
    var txt = 'Νέο Email: <input type="text" class="tb" onfocus="' + onfocus + '" onblur="' + onblur + '" id="email" name="email" value="" />';
    var dialog = $('<div>' + txt + '</div>')
      .dialog({
          modal: true,
          resizable: false,
          draggable: true,
          width: 380,
          title: 'Αλλαγή Email',
          show: 'fade',
          hide: 'fade',
          dialogClass: 'main-dialog-class',
          buttons: {
              "Αλλαγή Email": function () {
                  beginChangeEmail(dialog);
                  dialog.dialog('close');
              },
              "Ακύρωση": function () {
                  dialog.dialog('close');
              }
          },
          open: function () {
              $(this).parent().find('.ui-dialog-buttonpane button:nth-child(1)').button({
                  icons: { primary: 'bg-accept' }
              });
              $(this).parent().find('.ui-dialog-buttonpane button:nth-child(2)').button({
                  icons: { primary: 'bg-cancel' }
              });
          }
      });
    //$.prompt(txt, { callback: beginChangeEmail, show: 'dropIn', buttons: { 'Αλλαγή Email': true, Ακύρωση: false } });
    return false;
}

function showMobilePhonePopup() {
    var onblur = "$(this).removeClass('focused');";
    var onfocus = "$(this).addClass('focused');";
    var txt = 'Νέο Κινητό: <input type="text" class="tb" onfocus="' + onfocus + '" onblur="' + onblur + '" id="mobilePhone" name="mobilePhone" value="" maxlength="10" />';
    var dialog = $('<div>' + txt + '</div>')
      .dialog({
          modal: true,
          resizable: false,
          draggable: true,
          width: 360,
          title: 'Αλλαγή Κινητού',
          show: 'fade',
          hide: 'fade',
          dialogClass: 'main-dialog-class',
          buttons: {
              "Αλλαγή Κινητού": function () {
                  beginChangeMobilePhone(dialog);
                  dialog.dialog('close');
              },
              "Ακύρωση": function () {
                  dialog.dialog('close');
              }
          },
          open: function () {
              $(this).parent().find('.ui-dialog-buttonpane button:nth-child(1)').button({
                  icons: { primary: 'bg-accept' }
              });
              $(this).parent().find('.ui-dialog-buttonpane button:nth-child(2)').button({
                  icons: { primary: 'bg-cancel' }
              });
          }
      });
    //$.prompt(txt, { callback: beginChangeMobilePhone, show: 'dropIn', buttons: { 'Αλλαγή Κινητού': true, Ακύρωση: false } });
    return false;
}

function beginChangeEmail(m) {
    var newEmail = m.find('#email').val();
    if (!newEmail.match(/^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$/)) {
        showError(emailError, 'To Email ' + newEmail + ' δεν είναι έγκυρο');
        return;
    }
    var userContext = {};
    userContext.errorArea = emailError;
    userContext.btn = btnEmail;
    userContext.keepBtnVisible = true;
    userContext.email = newEmail;
    userContext.errorMessage = 'Προκλήθηκε σφάλμα. Παρακαλούμε δοκιμάστε ξανά αργότερα.';
    userContext.successMessage = 'Η αλλαγή ολοκληρώθηκε επιτυχώς και στάλθηκε ένα email επιβεβαίωσης στη διεύθυνση που δηλώσατε';
    userContext.failMessage = 'Το email ' + newEmail + ' χρησιμοποιείται και δεν μπορεί να πραγματοποιηθεί η αλλαγή.';
    //PageMethods.ChangeEmail(_USERNAME, newEmail, pageMethodCompleted, onFailed, userContext);
    if (typeof (_USERNAME) === 'undefined') {
        EudoxusOsy.Portal.PortalServices.Services.ChangeEmail(null, newEmail, pageMethodCompleted, onFailed, userContext);
    }
    else {
        EudoxusOsy.Portal.PortalServices.Services.ChangeEmail(_USERNAME, newEmail, pageMethodCompleted, onFailed, userContext);
    }
}

function beginChangeMobilePhone(m) {
    var newMobilePhone = m.find('#mobilePhone').val();
    if (!newMobilePhone.match(/^69[0-9]{8}$/)) {
        showError(mobilePhoneError, 'To Κινητό ' + newMobilePhone + ' δεν είναι έγκυρο');
        return;
    }
    var userContext = {};
    userContext.errorArea = mobilePhoneError;
    userContext.btn = btnMobilePhone;
    userContext.keepBtnVisible = true;
    userContext.mobilePhone = newMobilePhone;
    userContext.errorMessage = 'Προκλήθηκε σφάλμα. Παρακαλούμε δοκιμάστε ξανά αργότερα.';
    userContext.successMessage = 'Η αλλαγή του κινητού ολοκληρώθηκε επιτυχώς';
    userContext.failMessage = 'Η αλλαγή δεν μπορεί να ολοκληρωθεί είτε γιατί έχετε ξεπεράσει τον μέγιστο επιτρεπόμενο αριθμό επαναποστολών του SMS επιβεβαίωσης είτε γιατί υπάρχει ήδη εγγεγραμμένος χρήστης με το κινητό που δηλώσατε. Παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.';
    //PageMethods.ChangeMobilePhone(_USERNAME, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    if (typeof (_USERNAME) === 'undefined') {
        EudoxusOsy.Portal.PortalServices.Services.ChangeMobilePhone(null, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    }
    else {
        EudoxusOsy.Portal.PortalServices.Services.ChangeMobilePhone(_USERNAME, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    }
}

function beginChangeMobilePhoneForeign(m) {
    var newMobilePhone = m.find('#mobilePhoneForeign').val();
    var userContext = {};
    userContext.errorArea = mobilePhoneError;
    userContext.btn = btnMobilePhoneForeign;
    userContext.keepBtnVisible = true;
    userContext.mobilePhone = newMobilePhone;
    userContext.errorMessage = 'Προκλήθηκε σφάλμα. Παρακαλούμε δοκιμάστε ξανά αργότερα.';
    userContext.successMessage = 'Η αλλαγή του κινητού ολοκληρώθηκε επιτυχώς';
    userContext.failMessage = 'Έχετε ξεπεράσει τον μέγιστο επιτρεπόμενο αριθμό επαναποστολών του SMS επιβεβαίωσης. Παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.';
    //PageMethods.ChangeMobilePhone(_USERNAME, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    if (typeof (_USERNAME) === 'undefined') {
        EudoxusOsy.Portal.PortalServices.Services.ChangeMobilePhone(null, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    }
    else {
        EudoxusOsy.Portal.PortalServices.Services.ChangeMobilePhone(_USERNAME, newMobilePhone, pageMethodCompleted, onFailed, userContext);
    }
}

function begin(fn) {
    setTimeout(fn, 1000);
}

function unlock() {
    var userContext = {};
    userContext.btn = btnUnlock;
    userContext.errorArea = unlockError;
    userContext.unlock = 'ΟΧΙ';
    userContext.errorMessage = 'Προκλήθηκε σφάλμα. Παρακαλούμε δοκιμάστε ξανά αργότερα.';

    if (typeof (_USERNAME) === 'undefined') {
        EudoxusOsy.Portal.PortalServices.Services.UnlockUser(null, pageMethodCompleted, onFailed, userContext);
    }
    else {
        EudoxusOsy.Portal.PortalServices.Services.UnlockUser(_USERNAME, pageMethodCompleted, onFailed, userContext);
    }
}


//Helpers
function onFailed(args, userContext) {
    userContext.btn.removeClass('bg-loading');
    showError(userContext.errorArea, userContext.errorMessage);
}

function pageMethodCompleted(args, userContext) {
    if (args != null) {
        if (args) {
            if (userContext.successMessage) {
                showSuccess(userContext.errorArea, userContext.successMessage);
            }
            if (userContext.email) {
                if ($('#txtEmail').length != 0) {
                    $('#txtEmail').val(userContext.email);
                }
                if ($('#ltrEmail').length != 0) {
                    $('#ltrEmail').html(userContext.email);
                }
            }
            else if (userContext.mobilePhone) {
                if ($('#txtMobilePhone').length != 0) {
                    $('#txtMobilePhone').val(userContext.mobilePhone);
                }
                if ($('#ltrMobilePhone').length != 0) {
                    $('#ltrMobilePhone').html(userContext.mobilePhone);
                }
            }
            else if (userContext.unlock) {
                if ($('#ltrIsLockedOut').length != 0) {
                    $('#ltrIsLockedOut').html(userContext.unlock);
                }
            }
            if (!userContext.keepBtnVisible)
                userContext.btn.fadeOut();

            window.location = window.location;
        }
        else {
            userContext.btn.removeClass('bg-loading');
            showError(userContext.errorArea, userContext.failMessage);
        }
    }
    else {
        userContext.btn.removeClass('bg-loading');
        showError(userContext.errorArea, userContext.errorMessage);
    }
}

function showSuccess(element, message) {
    var initText = element.text();
    element.html(initText + '<span style="color:green;"> (' + Imis.Lib.HtmlDecode(message) + ')</span>');
    setTimeout(function () {
        element.children().fadeOut('normal', function () {
            element.children().remove();
        });
    }, 4000);
}

function showError(element, message) {
    var initText = element.text();
    element.html(initText + '<span style="color:red"> (' + Imis.Lib.HtmlDecode(message) + ')</span>');
    setTimeout(function () {
        element.children().fadeOut('normal', function () {
            element.children().remove();
        });
    }, 10000);
}

