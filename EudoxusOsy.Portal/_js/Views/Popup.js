function hideAllButtons() {
    Imis.Lib.HideButton(btnPopupSubmit);    
    btnPopupCancel.SetVisible(true);
}

function showAddInvoicePopup(groupID) {
    var popupUrl = [popupUrls.AddInvoiceUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Προσθήκη Παραστατικού', cmdRefresh, 500, 500);
    hideAllButtons();
    btnPopupSubmit.SetVisible(true);
}

function showAddFilePopup(supplierID) {
    var popupUrl = [popupUrls.AddFileUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Προσθήκη Αρχείου', cmdRefresh, 900, 500);
    hideAllButtons();
    btnPopupSubmit.SetVisible(true);
}

function showEditInvoicePopup(invoiceID) {
    var popupUrl = [popupUrls.EditInvoiceUrl, '?id=', invoiceID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Παραστατικού', cmdRefresh, 500, 500);
    hideAllButtons();
    btnPopupSubmit.SetVisible(true);
}

function showAddTransferPopup(supplierID) {
    var popupUrl = [popupUrls.AddTransferUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Προσθήκη Εκχώρησης', cmdRefresh, 700, 500);
    hideAllButtons();
    btnPopupSubmit.SetVisible(true);
}

function showEditTransferPopup(transferID) {
    var popupUrl = [popupUrls.EditTransferUrl, '?id=', transferID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Εκχώρησης', cmdRefresh, 700, 500);
    hideAllButtons();
    btnPopupSubmit.SetVisible(true);
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
            case "ManageInvoices":
                if (actionName == 'delete') {
                    showConfirmBox('Διαγραφή Παραστατικού', 'Είστε σίγουροι ότι θέλετε να διαγράψετε το παραστατικό με αριθμό \'' + confirmParam + '\';', doCallback);
                }
                break;
            case "ManageTransfers":
                if (actionName == 'delete') {
                    showConfirmBox('Διαγραφή Εκχώρησης', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την εκχώρηση με αριθμό παραστατικού \'' + confirmParam + '\';', doCallback);
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
    else if (gv.cpMessage) {
        Imis.Lib.notify(gv.cpMessage);
    }
}