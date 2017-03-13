function hideAllButtons() {
    Imis.Lib.HideButton(btnPrint);
    Imis.Lib.HideButton(btnSubmit);    
    Imis.Lib.HideButton(btnCancelWithRefresh);
    btnCancel.SetVisible(true);
}

function showChangePasswordPopup(requestOldPassword) {
    var popupUrl = [supplierUrls.ChangePasswordUrl, '?id=', requestOldPassword].join('');

    popUp.show(popupUrl, 'Αλλαγή Κωδικού Πρόσβασης', null, 600, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewCatalogGroupDetailsPopup(groupID) {
    var popupUrl = [supplierUrls.ViewCatalogGroupDetailsUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Προβολή Στοιχείων Κατάστασης', refresh, 900, 600);
    hideAllButtons();    
}

function showManageInvoicesPopup(groupID) {
    var popupUrl = [supplierUrls.ManageInvoicesUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Διαχείριση Παραστατικών', refresh, 800, 600);
    hideAllButtons();    
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showSetBankTransferPopup(groupID) {
    var popupUrl = [supplierUrls.SetBankTransferUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Εκχώρηση σε Τράπεζα', refresh, 500, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showViewBankTransferPopup(groupID) {
    var popupUrl = [supplierUrls.ViewBankTransferUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Εκχώρηση σε Τράπεζα', refresh, 500, 350);
    hideAllButtons();
}

//AlterPassword.aspx
function clearErrors() {
    $('#divErrors').hide();
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
            case "SelectPhase":
                if (actionName == 'selectPhase') {
                    gv.GetValuesOnCustomCallback(params, onActionDone);
                }
            case "CatalogGroups":
                if (actionName == 'delete') {
                    var doDelete = function () { gvCatalogGroups.PerformCallback(params); };
                    showConfirmBox('Διαγραφή Κατάστασης', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την κατάσταση για το Ίδρυμα \'' + confirmParam + '\';', doDelete);
                }
            case "EditCatalogGroup":
                if (actionName == 'addToGroup') {
                    gvNotConnectedCatalogs.PerformCallback(params);
                }
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