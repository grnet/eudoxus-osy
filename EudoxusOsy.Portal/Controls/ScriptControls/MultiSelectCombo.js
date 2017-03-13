/// <reference name="MicrosoftAjax.js"/>
/// <reference path="../../_js/Imis/ASPxScriptIntelliSense.js" />

Type.registerNamespace("EudoxusOsy.Portal.Controls.ScriptControls");

EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo = function (element) {
    EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo.initializeBase(this, [element]);
    // Instance members go here
    this._textSeparator = ";";
    this._isRequired = null;
    this._emptyFieldText = null;
    this._dropDownEdit = null;
    this._listBox = null;
    this._hfClientState = null;
    this._validationGroup = "";
    this._proposalFieldName = "";
    this._isValid = false;
}

EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo.prototype = {
    initialize: function () {
        EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo.callBaseMethod(this, 'initialize');

        this._dropDownEdit.Validation.AddHandler(Function.createDelegate(this, this._validateCombo));
        this._dropDownEdit.TextChanged.AddHandler(Function.createDelegate(this, this._synchronizeListBoxValues));
        this._dropDownEdit.DropDown.AddHandler(Function.createDelegate(this, this._synchronizeListBoxValues));

        this._listBox.SelectedIndexChanged.AddHandler(Function.createDelegate(this, this._onListBoxSelectionChanged));
    },

    dispose: function () {
        this._dropDownEdit.Validation.ClearHandlers();
        this._dropDownEdit.TextChanged.ClearHandlers();
        this._dropDownEdit.DropDown.ClearHandlers();

        this._listBox.SelectedIndexChanged.ClearHandlers();

        EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo.callBaseMethod(this, 'dispose');
    },

    _onListBoxSelectionChanged: function (listBox, args) {
        this._updateText();
    },

    _updateText: function () {
        var selectedItems = this._listBox.GetSelectedItems();

        $(this._hfClientState).val(this._getSelectedItemsValue(selectedItems));
        var text = this._getSelectedItemsText(selectedItems);
        if (text == '') {
            this._dropDownEdit.SetText(this._emptyFieldText);
        }
        else {
            this._dropDownEdit.SetText(text);
        }
    },

    _synchronizeListBoxValues: function (dropDown, args) {
        this._listBox.UnselectAll();

        var texts = dropDown.GetText().split(this._textSeparator);
        var values = this._getValuesByTexts(texts);
        this._listBox.SelectValues(values);

        this._updateText();
    },

    _getSelectedItemsText: function (items) {
        var texts = [];

        for (var i = 0; i < items.length; i++) {
            texts.push(items[i].text);
        }

        return texts.join(this._textSeparator);
    },

    _getSelectedItemsValue: function (items) {
        var values = [];

        for (var i = 0; i < items.length; i++) {
            values.push(items[i].value);
        }

        return values.join(this._textSeparator);
    },

    _getValuesByTexts: function (texts) {
        var actualValues = [];
        var item;

        for (var i = 0; i < texts.length; i++) {
            item = this._listBox.FindItemByText(texts[i]);

            if (item != null)
                actualValues.push(item.value);
        }

        return actualValues;
    },

    _validateCombo: function (s, e) {
        e.isValid = true;

        if (this._isRequired && (this._dropDownEdit.GetText() == '' || this._dropDownEdit.GetText() == this._emptyFieldText)) {
            e.isValid = false;
            e.errorText = 'Το πεδίο είναι υποχρεωτικό';
        }

        this._isValid = e.isValid;
    },

    Validate: function() {
        this._dropDownEdit.Validate();
        return this._isValid;
    },

    get_isRequired: function () { return this._isRequired; },
    set_isRequired: function (val) { this._isRequired = val; },

    get_emptyFieldText: function () { return this._emptyFieldText; },
    set_emptyFieldText: function (val) { this._emptyFieldText = val; },

    get_proposalFieldName: function () { return this._proposalFieldName; },
    set_proposalFieldName: function (val) { this._proposalFieldName = val; },

    get_dropDownEdit: function () { return this._dropDownEdit; },
    set_dropDownEdit: function (val) { this._dropDownEdit = val; },

    get_listBox: function () { return this._listBox; },
    set_listBox: function (val) { this._listBox = val; },

    get_hfClientState: function () { return this._hfClientState; },
    set_hfClientState: function (val) { this._hfClientState = val; },

    get_items: function () { return this._items; },
    set_items: function (val) { this._items = val; },

    get_validationGroup: function () { return this._validationGroup; },
    set_validationGroup: function (val) { this._validationGroup = val; }
};

EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo.registerClass('EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo', Sys.UI.Control);