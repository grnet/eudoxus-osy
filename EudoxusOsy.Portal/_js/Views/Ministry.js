function hideAllButtons() {
    Imis.Lib.HideButton(btnSubmit);
    Imis.Lib.HideButton(btnCancelWithRefresh);
    Imis.Lib.HideButton(btnExport);
    btnCancel.SetVisible(true);
}

function showChangePasswordPopup(requestOldPassword) {
    var popupUrl = [ministryUrls.ChangePasswordUrl, '?id=', requestOldPassword].join('');

    popUp.show(popupUrl, 'Αλλαγή Κωδικού Πρόσβασης', null, 600, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showVatDataEditPopup() {
    var popupUrl = ministryUrls.VatDataEditPopupUrl;

    popUp.show(popupUrl, 'Αλλαγή προκαθορισμένου Φ.Π.Α.', null, 800, 600);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewSupplierPopup(supplierID) {
    var popupUrl = [ministryUrls.ViewSupplierUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Προβολή Στοιχείων Εκδότη', cmdRefresh, 1000, 700);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewIbanChangesPopup(supplierID, supplierName) {
    var popupUrl = [ministryUrls.ViewIbanChangesUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Αλλαγές IBAN Εκδότη (' + supplierName + ')', cmdRefresh, 600, 500);
    hideAllButtons();    
}

function showChangeIbanPopup(supplierID, supplierName) {
    var popupUrl = [ministryUrls.ChangeIbanUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Αλλαγή IBAN Εκδότη (' + supplierName + ')', cmdRefresh, 600, 500);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showManageFilesPopup(supplierID) {
    var popupUrl = [ministryUrls.ManageFilesUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Δικαιολογητικά', cmdRefresh, 1000, 800);
    hideAllButtons();
}

function showViewCatalogGroupDetailsPopup(groupID) {
    var popupUrl = [ministryUrls.ViewCatalogGroupDetailsUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Προβολή Στοιχείων Κατάστασης', refresh, 900, 600);
    hideAllButtons();
}

function showManageInvoicesPopup(groupID) {
    var popupUrl = [ministryUrls.ManageInvoicesUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Διαχείριση Παραστατικών', refresh, 800, 600);
    hideAllButtons();
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showManageTransfersPopup(supplierID) {
    var popupUrl = [ministryUrls.ManageTransfersUrl, '?id=', supplierID].join('');

    popUp.show(popupUrl, 'Διαχείριση Εκχωρήσεων', cmdRefresh, 900, 700);
    hideAllButtons();
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showEditGroupDeductionPopup(groupID) {
    var popupUrl = [ministryUrls.EditGroupDeductionUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Καθεστώς Φ.Π.Α. Κατάστασης', cmdRefresh, 700, 500);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showSetBankTransferPopup(groupID) {
    var popupUrl = [ministryUrls.SetBankTransferUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Εκχώρηση σε Τράπεζα', refresh, 500, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
    btnCancel.SetVisible(false);
    btnCancelWithRefresh.SetVisible(true);
}

function showViewBankTransferPopup(groupID) {
    var popupUrl = [ministryUrls.ViewBankTransferUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Εκχώρηση σε Τράπεζα', refresh, 500, 350);
    hideAllButtons();
}

function showEditBookActiveStatusPopup(bookID) {
    var popupUrl = [ministryUrls.ShowEditBookActiveStatusUrl, '?id=', bookID].join('');

    popUp.show(popupUrl, 'Επεξεργασία δυνατότητας τιμολόγησης και αποζημίωσης βιβλίου', null, 500, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function approveGroupPopup(groupID) {
    var popupUrl = [ministryUrls.ApproveGroupUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Έγκριση για πληρωμή', null, 500, 300);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function revertApprovalPopup(groupID) {
    var popupUrl = [ministryUrls.RevertApprovalUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Επαναφορά σε Επιλεγμένη για Πληρωμή', null, 500, 300);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function sendToYDEPopup(groupID) {
    var popupUrl = [ministryUrls.SendToYDEUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Αποστολή προς ΥΔΕ', null, 500, 400);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function returnFromYDEPopup(groupID) {
    var popupUrl = [ministryUrls.ReturnFromYDEUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Επιστροφή από ΥΔΕ', null, 500, 300);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showViewCatalogGroupLogPopup(groupID) {
    var popupUrl = [ministryUrls.ViewCatalogGroupLogUrl, '?id=', groupID].join('');

    popUp.show(popupUrl, 'Προβολή Ιστορικού Μεταβολών', null, 800, 400);
    hideAllButtons();
}

function showAddCatalogPopup() {
    var popupUrl = ministryUrls.AddCatalogUrl;

    popUp.show(popupUrl, 'Προσθήκη Διανομής', cmdRefresh, 600, 400);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showAddBankPopup() {
    var popupUrl = ministryUrls.AddBankUrl;

    popUp.show(popupUrl, 'Προσθήκη Τράπεζας', cmdRefresh, 600, 300);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showEditBankPopup(bankID) {
    var popupUrl = [ministryUrls.EditBankUrl, '?id=', bankID].join('');

    popUp.show(popupUrl, 'Επεξεργασία Τράπεζας', cmdRefresh, 810, 650);
    hideAllButtons();
    btnSubmit.SetVisible(true);
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
                break;
            case "PaymentOrders":
                var doCatalogGroupsCallback = function () { gvCatalogGroups.PerformCallback(params); };
                var doCatalogsCallback = function () { gvCatalogs.PerformCallback(params); };

                if (actionName == 'delete') {
                    showConfirmBox('Διαγραφή Κατάστασης', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την κατάσταση για το Ίδρυμα \'' + confirmParam + '\';', doCatalogGroupsCallback);
                }
                else if (actionName == 'lock') {
                    showConfirmBox('Κλείδωμα Κατάστασης', 'Είστε σίγουροι ότι θέλετε να κλειδώσετε την κατάσταση για το Ίδρυμα \'' + confirmParam + '\';', doCatalogGroupsCallback);
                }
                else if (actionName == 'unlock') {
                    showConfirmBox('Ξεκλείδωμα Κατάστασης', 'Είστε σίγουροι ότι θέλετε να ξεκλειδώσετε την κατάσταση για το Ίδρυμα \'' + confirmParam + '\';', doCatalogGroupsCallback);
                }
                else if (actionName == 'movetophase')
                {
                    doCatalogsCallback(params);
                }
                break;
            case "EditCatalogGroup":
                if (actionName == 'addToGroup') {
                    gvNotConnectedCatalogs.PerformCallback(params);
                }
                break;
            case "BankManagement":
                if (actionName == 'delete') {
                    showConfirmBox('Διαγραφή Τράπεζας', 'Είστε σίγουροι ότι θέλετε να διαγράψετε την τράπεζα \'' + confirmParam + '\';', doCallback);
                }
                else if (actionName == 'deactivate') {
                    showConfirmBox('Απενεργοποίηση Τράπεζας', 'Είστε σίγουροι ότι θέλετε να απενεργοποιήσετε την τράπεζα \'' + confirmParam + '\';', doCallback);
                }
                else if (actionName == 'activate') {
                    showConfirmBox('Ενεργοποίηση Τράπεζας', 'Είστε σίγουροι ότι θέλετε να ενεργοποιήσετε την τράπεζα \'' + confirmParam + '\';', doCallback);
                }
                break;
            case "PriceVerificationMinistry":

                var doBooksCallback = function () { gvBooks.PerformCallback(params); };
                //var doBookPriceChangeCallback = function () { gvBookPriceChange.PerformCallback(params); };

                if (actionName == 'lock') {
                    showConfirmBox('Κλείδωμα Βιβλίου', 'Είστε σίγουροι ότι θέλετε να κλειδώσετε το βιβλίο \'' + confirmParam + '\';', doBooksCallback);
                }
                else if (actionName == 'unlock') {
                    showConfirmBox('Ξεκλείδωμα Βιβλίου', 'Είστε σίγουροι ότι θέλετε να ξεκλειδώσετε το βιβλίο \'' + confirmParam + '\';', doBooksCallback);
                }
                else if (actionName == 'search') {
                    gvBooks.PerformCallback(params);
                    }
                break;
            case "PriceVerificationUnexpected":                
                var doBooksCallback = function () { gvBooks.PerformCallback(params); };

                if (actionName == 'lock') {
                    showConfirmBox('Κλείδωμα Βιβλίου', 'Είστε σίγουροι ότι θέλετε να κλειδώσετε το βιβλίο \'' + confirmParam + '\';', doBooksCallback);
                }
                else if (actionName == 'unlock') {
                    showConfirmBox('Ξεκλείδωμα Βιβλίου', 'Είστε σίγουροι ότι θέλετε να ξεκλειδώσετε το βιβλίο \'' + confirmParam + '\';', doBooksCallback);
                }
                else if (actionName == 'search') {
                    gvBooks.PerformCallback(params);
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


function showEditVatDataPopup(supplierID, phaseID) {
    popUp.show(popupUrl, 'Αλλαγή προκαθορισμένου Φ.Π.Α.', null, 800, 600);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showBankTransferPopup(supplierID, phaseID) {
    var popupUrl = [ministryUrls.BankTransferPopupUrl, '?id=', supplierID, '&pID=', phaseID].join('');
    popUp.show(popupUrl, 'Εκχωρήσεις εκδότη', null, 800, 600);
    hideAllButtons();
}

function showBankEditPopup() {
    var popupUrl = [ministryUrls.BankEditPopupUrl].join('');
    popUp.show(popupUrl, 'Επεξεργασία τράπεζας', null, 800, 600);
    hideAllButtons();
}

function showExportBookCatalogs() {
    var popupUrl = [ministryUrls.ExportBookCatalogsPopupUrl].join('');
    popUp.show(popupUrl, 'Εξαγωγή Διανομών και Καταστάσεων Βιβλίου', null, 600, 400);
    hideAllButtons();
    btnExport.SetVisible(true);
}

function showItemIsActiveCommentsPopup(bookID) {
    var popupUrl = [ministryUrls.ShowItemIsActiveCommentsPopupUrl, '?id=', bookID].join('');
    popUp.show(popupUrl, 'Δυνατότητα τιμολόγησης και αποζημίωσης', null, 300, 200);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function insertNewCatalogPopup() {
    var popupUrl = [ministryUrls.InsertNewCatalogPopupUrl].join('');
    popUp.show(popupUrl, 'Δημιουργία νέας Διανομής', null, 300, 200);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function editCatalogPopup(catalogID) {
    var popupUrl = [ministryUrls.EditCatalogPopupUrl, '?id=', catalogID].join('');
    popUp.show(popupUrl, 'Επεξεργασία Διανομής', null, 300, 200);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}



function changePhase(groupID) {
    var popupUrl = [ministryUrls.ChangePhasePopupUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Αλλαγή Φάσης', null, 300, 200);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}



function exportOfficeSlipPopup(pdfOutput) {
    var popupUrl = ministryUrls.ExportOfficeSlipPopupUrl;
    if (pdfOutput == 1) {
        popupUrl = [popupUrl, '?pdf=1'].join('');
        popUp.show(popupUrl, 'Δημιουργία Διαβιβαστικού', null, 500, 400);
    }
    else {
        popUp.show(popupUrl, 'Εξαγωγή Διαβιβαστικού σε Excel', null, 500, 400);
    }
    hideAllButtons();
    btnSubmit.SetVisible(true);
}


function exportCatalogInvoicePopup(groupID) {
    var popupUrl = ministryUrls.ExportCatalogInvoicePopupUrl;;
    popupUrl = [popupUrl, '?id=', groupID].join('');
    popUp.show(popupUrl, 'Εξαγωγή σε PDF', null, 300, 200);

    hideAllButtons();
    btnSubmit.SetVisible(true);
}

function showExportNoLogisticBooks() {
    var popupUrl = [ministryUrls.SelectPhasePopupUrl,"?type=nolog"].join('');
    popUp.show(popupUrl, 'Εξαγωγή μη υπόχρεων λογ. βιβλίων', null, 500, 350);
    hideAllButtons();
    btnExport.SetVisible(true);
}

function showExportSupplierStats() {
    var popupUrl = [ministryUrls.SelectPhasePopupUrl, "?type=supplierstats"].join('');
    popUp.show(popupUrl, 'Εξαγωγή στατιστικών εκδοτών', null, 500, 350);
    hideAllButtons();
    btnExport.SetVisible(true);
}

function showExportCommitments() {
    var popupUrl = [ministryUrls.SelectPhasePopupUrl, "?type=commit"].join('');
    popUp.show(popupUrl, 'Εξαγωγή μητρώου δεσμεύσεων', null, 500, 350);
    hideAllButtons();
    btnExport.SetVisible(true);
}

function showExportCoAuthors() {
    var popupUrl = [ministryUrls.SelectPhasePopupUrl, "?type=coauthors"].join('');
    popUp.show(popupUrl, 'Εξαγωγή στοιχείων συνεκδοτών', null, 500, 350);
    hideAllButtons();
    btnExport.SetVisible(true);
}

function showMovetoPhase() {
    var popupUrl = [ministryUrls.SelectPhasePopupUrl, "?type=move"].join('');
    popUp.show(popupUrl, 'Αλλαγή φάσης', null, 500, 350);
    hideAllButtons();
    btnSubmit.SetVisible(true);
}