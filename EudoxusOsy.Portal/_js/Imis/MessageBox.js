function showAlertBox(msg, modal) {
    if (typeof (modal) === 'undefined') {
        modal = true;
    }
    $("<p>" + msg + "</p>").dialog({
        modal: modal,
        title: '',
        resizable: false,
        draggable: false,
        width: 500,
        height: 'auto',
        show: 'fade',
        hide: 'fade',
        dialogClass: 'main-dialog-class  main-dialog-notittle',
        buttons: {
            'Κλείσιμο': function () {
                $(this).dialog('close');
            }
        }
    });
}

function showMessageBox(title, msg, modal, okCallBack) {
    var mainDialog = 'main-dialog-class';
    if (typeof (modal) === 'undefined') {
        modal = true;
    }
    if (title == null || title == '') {
        mainDialog += " main-dialog-notittle"
    }
    $("<p>" + msg + "</p>").dialog({
        modal: modal,
        title: title,
        resizable: false,
        draggable: false,
        width: 500,
        height: 'auto',
        show: 'fade',
        hide: 'fade',
        dialogClass: mainDialog,
        buttons: {
            'Κλείσιμο': function () {
                $(this).dialog('close');
                if (typeof (okCallBack) === 'function') {
                    okCallBack();
                }
            }
        },
        open: function () {            
            $(this).parent().find('.ui-dialog-buttonpane button:nth-child(1)').button({
                icons: { primary: 'bg-cancel' }
            });
        }
    });
}

function showConfirmBox(title, msg, yesCallback, noCallback) {
    var mainDialog = 'main-dialog-class';
    if (title == null || title == '') {
        mainDialog += " main-dialog-notittle"
    }
    
    $("<p>" + msg + "</p>").dialog({
        modal: true,
        title: title,
        resizable: false,
        draggable: false,
        width: 450,
        height: 'auto',
        show: 'fade',
        hide: 'fade',
        dialogClass: mainDialog,
        buttons: {
            'Ναι': function () {
                $(this).dialog('close');
                if (typeof (yesCallback) == 'function')
                    yesCallback();
            },
            'Όχι': function () {
                $(this).dialog('close');
                if (typeof (noCallback) == 'function')
                    noCallback();
            }
        }
    });
}