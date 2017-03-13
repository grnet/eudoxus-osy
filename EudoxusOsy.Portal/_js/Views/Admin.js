function hideAllButtons() {    
    Imis.Lib.HideButton(btnSubmit);
    btnCancel.SetVisible(true);
}

function showChangePasswordPopup(requestOldPassword) {
    var popupUrl = [adminUrls.ChangePasswordUrl, '?id=', requestOldPassword].join('');

    popUp.show(popupUrl, 'Αλλαγή Κωδικού Πρόσβασης', null, 600, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
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

function doAction(actionName, id, pageCode) {
    var params = [actionName, id].join(':');

    if (actionName == 'refresh') {
        gv.PerformCallback(params);
    }
    else {
        var doCallback = function () { gv.PerformCallback(params); };
        switch (pageCode) {
            
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