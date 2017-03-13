/// <reference name="MicrosoftAjax.js"/>
/// <reference path="/_js/jquery.js"/>

Type.registerNamespace("EudoxusOsy.Portal.Controls.ScriptControls");

EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit = function (element) {
    EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit.initializeBase(this, [element]);
    // Instance members go here   
    this._isRequired = null;
    this._hfClientState = null;
    this._decodedValue = null;
    //this._cvRequired = null;
    this._maxLength = null;
    this._counterText = null;
    this._cbName = null;
    this._spCharCounter = null;
    this._memoArea = null;
    this._currentValidText = "";
    this._saving = false;
    this._validationGroup = "";
    this._proposalFieldName = "";
    this._handlerTimeout = null;
};

EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit.prototype = {
    initialize: function () {
        EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit.callBaseMethod(this, 'initialize');

        this._memoArea.Validation.AddHandler(Function.createDelegate(this, this._validateText));

        //removes name so it doesn't send postback
        this._memoArea.GetInputElement().name = '';
        $(this._memoArea.GetInputElement())
            .bind('keyup paste', Function.createDelegate(this, this._handleTextChange))
            .bind('blur', Function.createDelegate(this, this._saveMemo));

        if (this._hfClientState.value)
            this._setCounterText(Imis.Lib.HtmlDecode(this._hfClientState.value));
        else
            this._setCounterText();

        if (this._decodedValue)
            this._hfClientState.value = Imis.Lib.HtmlEncode(this._decodedValue);

        if (this._hfClientState.value != null)
            this._memoArea.SetValue(this._decodedValue);

        this._currentValidText = this._memoArea.GetValue();
        this._colorCounterText();
    },

    dispose: function () {
        this._memoArea.Validation.ClearHandlers();
        EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit.callBaseMethod(this, 'dispose');
    },

    add_valueChanged: function (handler) {
        this.get_events().addHandler('valueChanged', handler);
    },

    remove_valueChanged: function (handler) {
        this.get_events().removeHandler('valueChanged', handler);
    },

    _updateCounter: function (event) {
        clearTimeout(this._handlerTimeout);
        this._handlerTimeout = null;
        var currentText = this._memoArea.GetValue();
        var showAlert = false;
        if (currentText != null && currentText.length >= this._maxLength) {
            if (currentText.length > this._maxLength + 2) {
                showAlert = true;
            }
            currentText = currentText.substr(0, this._maxLength);
            if (showAlert) {
                showAlertBox('Η επικόλληση του κειμένου δε θα ολοκληρωθεί, διότι το κείμενο που προσπαθείτε να εισάγετε υπερβαίνει το επιτρεπόμενο όριο χαρακτήρων.');
                currentText = this._currentValidText;
            }
            this._memoArea.SetValue(currentText);
        }
        else
            this._currentValidText = currentText;
        this._setCounterText(currentText);
        this._colorCounterText(currentText);
    },

    _colorCounterText: function (currentText) {
        if (currentText == null) { currentText = ''; }
        if (currentText.length >= (this._maxLength * 0.95)) {
            $(this._spCharCounter).removeClass("brown-text");
            $(this._spCharCounter).addClass("red-text");
        }
        else if (currentText.length >= (this._maxLength * 0.9)) {
            $(this._spCharCounter).removeClass("red-text");
            $(this._spCharCounter).addClass("brown-text");
        }
        else {
            $(this._spCharCounter).removeClass("brown-text");
            $(this._spCharCounter).removeClass("red-text");
        }
    },

    _handleTextChange: function (e) {
        if (this._handlerTimeout != null) {
            clearTimeout(this._handlerTimeout);
            this._handlerTimeout = null;
        }
        this._handlerTimeout = setTimeout(Function.createDelegate(this, this._updateCounter), 50);
    },

    _setCounterText: function (text) {
        var chars = 0;
        if (text) {
            chars = text.length;
        }
        if (this.get_counterText() != null) {
            this._spCharCounter.innerHTML = this.get_counterText().replace("{0}", this._maxLength).replace("{1}", chars);
        }
        else {
            var info = 'Μέγιστο μέγεθος {0} χαρακτήρες, έχετε ήδη εισάγει {1}';
            this._spCharCounter.innerHTML = info.replace("{0}", this._maxLength).replace("{1}", chars);
        }
    },

    _saveMemo: function () {
        var val = this._memoArea.GetValue() || "";
        this._decodedValue = val;
        this._hfClientState.value = Imis.Lib.HtmlEncode(val == null ? "" : val.replace("\r\n", "\n"));
        this._saving = true;
        this._raiseEvent('valueChanged', null);
        this._memoArea.Validate();
    },

    _raiseEvent: function (eventName, eventArgs) {
        var handler = this.get_events().getHandler(eventName);
        if (handler) {
            eventArgs = eventArgs || Sys.EventArgs.Empty;
            handler(this, eventArgs);
        }
    },

    GetValue: function () {
        return this._hfClientState.value;
    },

    GetValueEncoded: function () {
        return Imis.Lib.HtmlEncode(this._hfClientState.value);
    },

    SetValue: function (v) {
        this._hfClientState.value = v;
        this._memoArea.SetValue(Imis.Lib.HtmlDecode(v));
    },

    _validateText: function (s, e) {
        e.IsValid = true;
        if (this._isRequired && this._hfClientState.value == '') {
            e.IsValid = false;
        }
    },

    get_decodedValue: function () { return this._decodedValue; },
    set_decodedValue: function (val) { this._decodedValue = val; },

    get_isRequired: function () { return this._isRequired; },
    set_isRequired: function (val) { this._isRequired = val; },

    get_hfClientState: function () { return this._hfClientState; },
    set_hfClientState: function (val) { this._hfClientState = val; },

    get_maxLength: function () { return this._maxLength; },
    set_maxLength: function (val) { this._maxLength = val; },

    //get_proposalFieldName: function () { return this._proposalFieldName; },
    //set_proposalFieldName: function (val) { this._proposalFieldName = val; },

    get_spCharCounter: function () { return this._spCharCounter; },
    set_spCharCounter: function (val) { this._spCharCounter = val; },

    get_counterText: function () { return this._counterText; },
    set_counterText: function (val) { this._counterText = val; },

    get_memoArea: function () { return this._memoArea; },
    set_memoArea: function (val) { this._memoArea = window[val]; },

    get_validationGroup: function () { return this._validationGroup; },
    set_validationGroup: function (val) { this._validationGroup = val; }
};

EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit.registerClass('EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit', Sys.UI.Control);