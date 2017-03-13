/// <reference name="MicrosoftAjax.js"/>
/// <reference path="../../_js/Imis/ASPxScriptIntelliSense.js" />
/// <reference path="../../_js/jquery-1.9.1.min.js" />

Type.registerNamespace("EudoxusOsy.Portal.Controls.ScriptControls");

EudoxusOsy.Portal.Controls.ScriptControls.FileUpload = function (element) {
    EudoxusOsy.Portal.Controls.ScriptControls.FileUpload.initializeBase(this, [element]);
    // Instance members go here    
    this._initParams = null;
    this._isUploading = false;
    this._btnUpload = null;
    this._btnShowPopup = null;
    this._btnDelete = null;
    this._lnkDownload = null;
    this._uploadRow = null;
    this._downloadRow = null;
    this._hfFileID = null;
    this._ifrDownload = null;
    this._dxUploadControl = null;
    this._downloadTimeout = null;
    this._cvFileUpload = null;
    this._cbName = null;
    this._fieldDescription = null;
    this._popupFileUpload = null;
    this._clientValidationFunction = null;
}

EudoxusOsy.Portal.Controls.ScriptControls.FileUpload.prototype = {
    initialize: function () {
        EudoxusOsy.Portal.Controls.ScriptControls.FileUpload.callBaseMethod(this, 'initialize');

        this.render();

        this._cbName = this._element.id + "_registerUploadControls";
        window[this._cbName] = Function.createDelegate(this, this._registerUploadControls);

        this._cvFileUpload.clientvalidationfunction = '$find("' + this.get_element().id + '").validateRequired';
        this._btnShowPopup.Click.AddHandler(Function.createDelegate(this, this._showPopup));
        this._btnDelete.Click.AddHandler(Function.createDelegate(this, this._fileDelete));
        $addHandler(this._lnkDownload, 'click', Function.createDelegate(this, this._fileDownload));
    },

    dispose: function () {
        $clearHandlers(this._lnkDownload);
        this._btnShowPopup.Click.ClearHandlers();
        this._btnDelete.Click.ClearHandlers();
        EudoxusOsy.Portal.Controls.ScriptControls.FileUpload.callBaseMethod(this, 'dispose');
    },

    render: function () {
        if (this._hfFileID.value) {
            $(this._uploadRow).hide();
            $(this._downloadRow).show();
        }
        else {
            $(this._uploadRow).show();
            $(this._downloadRow).hide();
        }
    },

    validateRequired: function (s, e) {
        if (this._clientValidationFunction) {
            var func = eval(this._clientValidationFunction);
            if (typeof (func) === 'function') {
                func(this, e);
            }
        }
        else {
            e.IsValid = true;
            if (this._initParams.IsRequired && this._hfFileID.value == '') {
                e.IsValid = false;
            }
        }
        ValidatorUpdateDisplay(this._cvFileUpload);
    },

    add_valueChanged: function (handler) {
        this.get_events().addHandler('valueChanged', handler);
    },

    remove_valueChanged: function (handler) {
        this.get_events().removeHandler('valueChanged', handler);
    },

    get_isUploading: function () {
        return this._isUploading;
    },

    _fileUploadStart: function (s, e) {
        if (this._isUploading)
            return;

        this._isUploading = true;
        this._btnUpload.SetEnabled(false);
    },

    _fileUploadComplete: function (s, e) {
        this._isUploading = false;
        this._btnUpload.SetEnabled(true);

        if (e.isValid) {
            this._cvFileUpload.innerHTML = '';
            var callbackData = eval('(' + e.callbackData + ')');
            this._lnkDownload.innerHTML = callbackData.FileName;
            this._lnkDownload.title = callbackData.Title;
            this._hfFileID.value = callbackData.FileID;

            $(this._uploadRow).hide();
            $(this._downloadRow).show();

            this._popupFileUpload.SetContentUrl('about:blank');
            this._popupFileUpload.Hide();
            this._raiseEvent('valueChanged', null);
            window.ValidatorValidate(this._cvFileUpload);
        }
        else {
            showMessageBox('Σφάλμα', e.errorText);
        }
    },

    _showPopup: function () {
        var url = ["/Secure/EditorPopups/FileUpload.aspx?cb=", this._cbName].join('');
        this._popupFileUpload.SetHeaderText('[ ' + this._fieldDescription + ' ] Επισύναψη αρχείου');
        this._popupFileUpload.SetContentUrl(url);
        this._popupFileUpload.SetPopupElementID(this.get_element().id);
        this._popupFileUpload.Show();

        var w = 500;
        if (w > (window.innerWidth - 200))
            w = window.innerWidth - 200;
        var h = 80;
        if (h > (window.innerHeight - 100))
            h = window.innerHeight - 100;
        this._popupFileUpload.SetSize(w, h);
        this._popupFileUpload.UpdatePosition();
    },

    _fileDelete: function () {
        var instance = this;
        showConfirmBox('', String.format('Θέλετε να διαγραφεί το αρχείο "{0}"', instance._lnkDownload.innerHTML), function () {
            instance._hfFileID.value = '';
            instance._lnkDownload.innerHTML = '';
            instance._lnkDownload.title = '';
            $(instance._uploadRow).show();
            $(instance._downloadRow).hide();
            instance._raiseEvent('valueChanged', null);
            window.ValidatorValidate(instance._cvFileUpload);
        });
    },

    _clearDownloadTimeout: function () {
        this._downloadTimeout = null;
    },

    _fileDownload: function () {
        if (this._downloadTimeout != null)
            return;

        this._downloadTimeout = window.setTimeout(Function.createDelegate(this, this._clearDownloadTimeout));
        this._ifrDownload.src = [this._initParams.DownloadUrl, "?fid=", this._hfFileID.value, "&rnd=", Math.random()].join('');
    },

    _registerUploadControls: function (ctrls) {
        this._dxUploadControl = ctrls[0];
        this._dxUploadControl.FileUploadStart.AddHandler(Function.createDelegate(this, this._fileUploadStart));
        this._dxUploadControl.FileUploadComplete.AddHandler(Function.createDelegate(this, this._fileUploadComplete));

        var uploadControl = this._dxUploadControl;

        this._btnUpload = ctrls[1];
        this._btnUpload.Click.AddHandler(function (s, e) {
            if (uploadControl.GetText(0))
                uploadControl.Upload();
            else
                showAlertBox('Δεν έχετε επιλέξει αρχείο.');
        });
    },

    _raiseEvent: function (eventName, eventArgs) {
        if (eventName == 'valueChanged') {
            window.ValidatorValidate(this._cvFileUpload);
        }

        var handler = this.get_events().getHandler(eventName);
        if (handler) {
            eventArgs = eventArgs || Sys.EventArgs.Empty;
            handler(this, eventArgs);
        }
    },

    setFileDetails: function (details) {
        this._lnkDownload.innerHTML = details.FileName;
        this._lnkDownload.title = details.Title;
    },

    hasValue: function () {
        return (this._hfFileID.value);
    },

    get_fileID: function () { return this._hfFileID.value == '' ? null : this._hfFileID.value; },
    set_fileID: function (val) {
        if (typeof (val) === 'undefined' || val == null)
            this._hfFileID.value = '';
        else
            this._hfFileID.value = val;
    },

    get_cvFileUpload: function () { return this._cvFileUpload; },
    set_cvFileUpload: function (val) { this._cvFileUpload = val; },

    get_initParams: function () { return this._initParams; },
    set_initParams: function (val) { if (!this.get_isInitialized()) this._initParams = val; },

    get_btnShowPopup: function () { return this._btnShowPopup; },
    set_btnShowPopup: function (val) { this._btnShowPopup = val; },

    get_btnDelete: function () { return this._btnDelete; },
    set_btnDelete: function (val) { this._btnDelete = val; },

    get_lnkDownload: function () { return this._lnkDownload; },
    set_lnkDownload: function (val) { this._lnkDownload = val; },

    get_uploadRow: function () { return this._uploadRow; },
    set_uploadRow: function (val) { this._uploadRow = val; },

    get_downloadRow: function () { return this._downloadRow; },
    set_downloadRow: function (val) { this._downloadRow = val; },

    get_fieldDescription: function () { return this._fieldDescription; },
    set_fieldDescription: function (val) { this._fieldDescription = val; },

    get_ifrDownload: function () { return this._ifrDownload; },
    set_ifrDownload: function (val) { this._ifrDownload = val; },

    get_hfFileID: function () { return this._hfFileID; },
    set_hfFileID: function (val) { this._hfFileID = val; },

    get_popupFileUpload: function () { return this._popupFileUpload; },
    set_popupFileUpload: function (val) { this._popupFileUpload = val; },

    get_clientValidationFunction: function () { return this._clientValidationFunction; },
    set_clientValidationFunction: function (val) { this._clientValidationFunction = val; }

};

EudoxusOsy.Portal.Controls.ScriptControls.FileUpload.registerClass('EudoxusOsy.Portal.Controls.ScriptControls.FileUpload', Sys.UI.Control);