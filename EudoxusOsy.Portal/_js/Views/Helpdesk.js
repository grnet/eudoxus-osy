function hideAllButtons() {
    Imis.Lib.HideButton(btnPending);
    Imis.Lib.HideButton(btnVerify);
    Imis.Lib.HideButton(btnUnVerify);
    Imis.Lib.HideButton(btnUnderEvaluation);
    Imis.Lib.HideButton(btnApprove);
    Imis.Lib.HideButton(btnReject);
    Imis.Lib.HideButton(btnRevertEvaluation);
    Imis.Lib.HideButton(btnRevertEvaluationVerdict);
    Imis.Lib.HideButton(btnAccept);
    Imis.Lib.HideButton(btnRevertAccept);
    Imis.Lib.HideButton(btnSendBackToProvider);
    Imis.Lib.HideButton(btnUnlockReport);
    Imis.Lib.HideButton(btnSubmit);    
    Imis.Lib.HideButton(btnCancelWithRefresh);
    Imis.Lib.HideButton(btnCloseEvaluation);
    Imis.Lib.HideButton(btnSendEmail);

    Imis.Lib.HideButton(btnSubmitEvaluation);
    Imis.Lib.HideButton(btnSubmitReceipt);
    Imis.Lib.HideButton(btnVerdict);
    Imis.Lib.HideButton(btnRevert);
    btnCancel.SetVisible(true);
}

function showChangePasswordPopup(requestOldPassword) {
    var popupUrl = [helpdeskUrls.ChangePasswordUrl, '?id=', requestOldPassword].join('');

    popUp.show(popupUrl, 'Αλλαγή Κωδικού Πρόσβασης', null, 600, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewAccountDetailsPopup(reporterID) {
    var popupUrl = [helpdeskUrls.ViewAccountDetailsUrl, '?id=', reporterID].join('');

    popUp.show(popupUrl, 'Στοιχεία Λογαριασμού', cmdRefresh, 600, 300);
    hideAllButtons();
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showEditReporterPopup(reporterID) {
    var popupUrl = [helpdeskUrls.EditReporterUrl, '?id=', reporterID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Αναφέροντα', cmdRefresh, 800, 450);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showReportIncidentPopup(reporterID, refreshPage) {
    var popupUrl = helpdeskUrls.ReportIncidentUrl;
    
    if (reporterID) {
        popupUrl = [popupUrl, '?id=', reporterID].join('');
    }
    
    if (refreshPage) {
        popUp.show(popupUrl, 'Αναφορά Συμβάντος', cmdRefresh, 1100, 900);
    }
    else {
        popUp.show(popupUrl, 'Αναφορά Συμβάντος', null, 800, 900);
    }

    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showEditIncidentReportPopup(incidentReportID) {
    var popupUrl = [helpdeskUrls.EditIncidentReportUrl, '?id=', incidentReportID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Συμβάντος', cmdRefresh, 800, 900);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewIncidentReportPopup(incidentReportID) {
    var popupUrl = [helpdeskUrls.ViewIncidentReportUrl, '?id=', incidentReportID].join('');

    popUp.show(popupUrl, 'Προβολή Συμβάντος', cmdRefresh, 900, 900);
    hideAllButtons();
}

function showAddIncidentReportPostPopup(incidentReportID, reportStatus) {
    var popupUrl = [helpdeskUrls.AddIncidentReportPostUrl, '?id=', incidentReportID].join('');

    popUp.show(popupUrl, 'Προσθήκη Απάντησης', cmdRefresh, 800, 900);
    hideAllButtons();

    if (reportStatus == helpdeskUrls.ClosedReport) {
        btnUnlockReport.SetVisible(true);
    }
    else {
        btnSubmit.SetVisible(true);
    }
}

function showEditLastIncidentReportPostPopup(incidentReportPostID) {
    var popupUrl = [helpdeskUrls.EditLastIncidentReportPostUrl, '?id=', incidentReportPostID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Τελευταίας Απάντησης', cmdRefresh, 800, 400);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showEditIncidentReportHandlerPopup(incidentReportID) {
    var popupUrl = [helpdeskUrls.EditIncidentReportHandlerUrl, '?id=', incidentReportID].join('');

    popUp.show(popupUrl, 'Αλλαγή Χειριστή Συμβάντος', cmdRefresh, 700, 300);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showAddHelpdeskUserPopup() {
    var popupUrl = helpdeskUrls.AddHelpdeskUserUrl;

    popUp.show(popupUrl, 'Δημιουργία Χρήστη Γραφείου Υποστήριξης Χρηστών', cmdRefresh, 800, 695);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showEditHelpdeskUserPopup(reporterID) {
    var popupUrl = [helpdeskUrls.EditHelpdeskUserUrl, '?id=', reporterID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Χρήστη Γραφείου Υποστήριξης Χρηστών', cmdRefresh, 800, 695);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showSendMassEmail(cb) {
    var popupUrl = [helpdeskUrls.SendMassEmailUrl, '?idCallback=', cb].join('');

    popUp.show(popupUrl, 'Μαζική Αποστολή Email', cmdRefresh, 900, 700);
    hideAllButtons();
    btnSendEmail.SetVisible(true);
}

function showViewProjectsPopup(reporterID) {
    var popupUrl = [helpdeskUrls.ViewProjectsUrl, '?id=', reporterID].join('');

    popUp.show(popupUrl, 'Έργα', cmdRefresh, 800, 800);
    hideAllButtons();    
}

function onActionDone(result) {
    if (result == "ERROR") {
        showAlertBox('Προέκυψε ένα σφάλμα κατά την εκτέλεση της ενέργειας! Παρακαλώ προσπαθήστε ξανά.');
    }
    else if (result == "DATABIND") {
        gv.PerformCallback('databind:0');
    }
    else {
        window.location = result;
    }
}

function doAction(actionName, id, pageCode, confirmParam) {
    var params = [actionName, id].join(':');

    if (actionName == 'refresh') {
        gv.PerformCallback(params);
    }
    else {
        var doCallback = function () { gv.PerformCallback(params); };
        switch (pageCode) {
            case "SearchOrders":
                if (actionName == 'cancel') {
                    showConfirmBox('Ακύρωση Παραγγελίας', 'Είστε σίγουροι ότι θέλετε να ακυρώσετε την παραγγελία με κωδικό \'' + confirmParam + '\';', doCallback);
                }
                break;
            case "SearchVisits":
                if (actionName == 'delete') {
                    showConfirmBox('Διαγραφή Επίσκεψης', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την επίσκεψη που ξεκίνησε στις \'' + confirmParam + '\';', doCallback);
                }
                break;
            case "HelpdeskUsers":
                if (actionName == 'delete') {
                    showConfirmBox('', 'Είστε σίγουροι ότι θέλετε να διαγράψετε το χρήστη;', doCallback);
                }
                else if (actionName == 'lock') {
                    showConfirmBox('', 'Είστε σίγουροι ότι θέλετε να κλειδώσετε το χρήστη;', doCallback);
                }
                else if (actionName == 'unlock') {
                    showConfirmBox('', 'Είστε σίγουροι ότι θέλετε να ξεκλειδώσετε το χρήστη;', doCallback);
                }
                break;
        }
    }

    return false;
}

function gvCallbackEnd(s, e) {
    if (gv.cpError) {
        showAlertBox(gv.cpError);
        gv.cpError = null;
    }
}