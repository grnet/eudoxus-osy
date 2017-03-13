/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("EudoxusOsy.Portal.Controls.ScriptControls");


EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit = function (element) {
    EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit.initializeBase(this, [element]);
    this._rowTemplate = ["<tr>",
        "<td style='white-space: nowrap;'>{Index}</td>",
        "<td style='white-space: nowrap;' align=\"center\">{InvoiceNumber}</td>",
        "<td style='white-space: nowrap;' align=\"center\">{Date}</td>",
        "<td style='white-space: nowrap;' align=\"center\">{Amount}</td>",
        "<td style='white-space: nowrap;' align=\"center\">{Edit}</td>",
    "</tr>"].join('');
    this._currencyFields = ['Amount'];
    this._percentageFields = [''];
    this._dateFields = ['Date'];
    this._records = [];
    this._recordsCount = null;
    this._clientStateField = null;
    this._popupControl = null;
    this._inputControls = {};
    this._cvRequired = null;
    this._cvFinancialChecks = null;
    this._cvCurrentAmount = null;
    this._isRequired = null;
    this._readOnly = null;
    this._addHandle = null;
    this._addHandleFooter = null;
    this._submitHandle = null;
    this._cancelHandle = null;
    this._emptyRow = null;
    this._NEW_RECORD_INDEX = -1;
    this._editIndex = this._NEW_RECORD_INDEX;
    this._totalAmount = null;
}

EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit.prototype = {
    initialize: function () {
        EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit.callBaseMethod(this, 'initialize');

        this._clientStateField.value = Sys.Serialization.JavaScriptSerializer.serialize(this._records);

        Sys.Application.add_load(Function.createDelegate(this, function () {
            for (var i in this._inputControls) {
                var inputcontrol = $find(this._inputControls[i]);
                if (inputcontrol == null) {
                    this._inputControls[i] = window[this._inputControls[i]];
                }
                else {
                    this._inputControls[i] = inputcontrol;
                }
            }
        }));

        var control = this;
        var controlSelector = "#" + this._element.id;
        var rowSelector = controlSelector + " tbody tr";

        var elementId = this.get_element().id;
        this._cvCurrentAmount.clientvalidationfunction = '$find("' + elementId + '").validateCurrentAmount';

        var deleteBtnSelector = controlSelector + " a[rel='deleteRecord']";
        $(document).on("click", deleteBtnSelector, function (e) {
            e.preventDefault();
            var instance = this;
            var tr = $(instance).closest("tr");
            tr.css('background-color', '#ffcccc');
            showConfirmBox('', 'Διαγραφή της συγκεκριμένης εγγραφής;',
                function () {
                    var rowIndex = $(rowSelector).index($(instance).closest("tr"));
                    control.delete_record(rowIndex);
                },
                function () {
                    tr.css('background-color', '');
                });
        });

        var editBtnSelector = controlSelector + " a[rel='editRecord']";
        $(document).on("click", editBtnSelector, function (e) {
            e.preventDefault();
            var tr = $(this).closest("tr").css('background-color', '#fffacd');
            var rowIndex = $(rowSelector).index($(this).closest("tr"));
            control.show_editForm(e, rowIndex);
        });

        var moveupBtnSelector = controlSelector + " a[rel='moveUp']";
        $(document).on("click", moveupBtnSelector, function (e) {
            e.preventDefault();
            var tr = $(this).closest("tr");
            var rowIndex = $(rowSelector).index($(this).closest("tr"));
            control.moveUp(rowIndex);
            $($(rowSelector)[rowIndex - 1]).css('background-color', '#fffacd').animate({ backgroundColor: '' }, 500);
        });

        var movedownBtnSelector = controlSelector + " a[rel='moveDown']";
        $(document).on("click", movedownBtnSelector, function (e) {
            e.preventDefault();
            var tr = $(this).closest("tr");
            var rowIndex = $(rowSelector).index($(this).closest("tr"));
            control.moveDown(rowIndex);
            $($(rowSelector)[rowIndex + 1]).css('background-color', '#fffacd').animate({ backgroundColor: '' }, 500);
        });

        this._cvRequired.clientvalidationfunction = '$find("' + this.get_element().id + '").validateRequired';
        this._cvFinancialChecks.clientvalidationfunction = '$find("' + this.get_element().id + '").validateFinancialChecks';

        this._popupControl.CloseButtonClick.AddHandler(Function.createDelegate(this, this.close_editForm));
        $addHandler(this._addHandle, "click", Function.createDelegate(this, this.show_editForm));
        $addHandler(this._addHandleFooter, "click", Function.createDelegate(this, this.show_editForm));
        $addHandler(this._submitHandle, "click", Function.createDelegate(this, this.save_record));
        $addHandler(this._cancelHandle, "click", Function.createDelegate(this, this.close_editForm));

        this._emptyRow = $(controlSelector).find('.emptyRow');

        if (this._readOnly) {
            $(this._addHandle).hide();
            $(this._submitHandle).hide();
        }

        this.render();
    },

    dispose: function () {
        $clearHandlers(this._addHandle);
        $clearHandlers(this._addHandleFooter);
        $clearHandlers(this._submitHandle);
        $clearHandlers(this._cancelHandle);
        EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit.callBaseMethod(this, 'dispose');
    },

    render: function () {
        $("#" + this._element.id + " tr:gt(1)").each(function () {
            $(this).remove();
        });

        for (var i = 0; i < this._records.length; i++) {
            this._applyRowTemplate(this._records[i], i + 1);
        }
        if (this._records.length == 0) {
            $(this._emptyRow).show();
        }
        else {
            $(this._emptyRow).hide();
        }
    },

    validateRequired: function (s, e) {
        e.IsValid = true;
        if (this._isRequired && this._records.length == 0) {
            e.IsValid = false;
        }
        ValidatorUpdateDisplay(this._cvRequired);
    },

    isAmountSumLessThanTotal: function () {
        var currentTotal = 0;
        for (var i = 0; i < this._records.length; i++) {
            currentTotal += this._records[i].Amount;
        }

        return currentTotal < this._totalAmount;
    },

    validateFinancialChecks: function (s, e) {
        var errors = [];
        var currentTotal = 0;
        //if (this._inputControls['NetValueAfterDiscount'].GetValue() > this._inputControls['NetValue'].GetValue()) {
        //    errors.push('Το καθαρό ποσό μετά την έκπτωση πρέπει να είναι μικρότερο ή ίσο του καθαρού ποσού');
        //}
        
        for (var i = 0; i < this._records.length; i++) {
            currentTotal += this._records[i].Amount;
        }

        if (currentTotal + this._inputControls["Amount"].GetValue() > this._totalAmount) {
            errors.push('Το συνολικό ποσό των παραστατικών θα πρέπει να είναι μικρότερο ή ίσο του συνολικού ποσού της κατάστασης.');
        }

        if (errors.length == 0) {
            e.IsValid = true;
        }
        else {
            s.errormessage = errors.join('<br />');
            e.IsValid = false;
        }
        ValidatorUpdateDisplay(this._cvFinancialChecks);
    },

    validateCurrentAmount: function (s, e) {
        var errors = [];

        if (this._inputControls["Amount"].GetValue() > this._totalAmount) {
            errors.push('Το ποσό του παραστατικού δε μπορεί να ξεπερνά το συνολικό ποσό της κατάστασης.');
        }

        if (errors.length == 0) {
            e.IsValid = true;
        }
        else {
            s.errormessage = errors.join('<br />');
            e.IsValid = false;
        }
        ValidatorUpdateDisplay(this._cvCurrentAmount);
    },

    moveUp: function (index) {
        if (index == 0 || this._records.length <= 1)
            return;
        var item = this._records[index];
        Array.removeAt(this._records, index);
        Array.insert(this._records, index - 1, item);

        this.render();

        this._onRecordsChanged();
    },

    moveDown: function (index) {
        if (index == this._records.length - 1 || this._records.length <= 1)
            return;
        var item = this._records[index];
        Array.removeAt(this._records, index);
        Array.insert(this._records, index + 1, item);

        this.render();

        this._onRecordsChanged();
    },

    _onRecordsChanged: function () {

        for (var i = 0; i < this._records.length; i++) {
            this._records[i].OrderIndex = i + 1;
        }

        this._clientStateField.value = Sys.Serialization.JavaScriptSerializer.serialize(this._records);
        this._raiseEvent('valueChanged', null);
        ValidatorUpdateDisplay(this._cvRequired);
        ValidatorUpdateDisplay(this._cvFinancialChecks);
    },

    _raiseEvent: function (eventName, eventArgs) {
        var handler = this.get_events().getHandler(eventName);
        if (handler) {
            eventArgs = eventArgs || Sys.EventArgs.Empty;
            handler(this, eventArgs);
        }
    },

    _applyRowTemplate: function (record, index) {
        var isNew = this._editIndex == this._NEW_RECORD_INDEX;

        if (typeof (index) === 'undefined')
            index = isNew ? this._records.length : this._editIndex + 1;

        var currentRecord = this._records[index - 1];
        var description = currentRecord.Description;

        var values = $.extend({
            Index: index,
            Move: this._buildMove(),
            Edit: this._buildEdit()
        }, record);

        for (var i in values) {
            if (values[i] == null) {
                values[i] = '';
            }
            else if (this._currencyFields.indexOf(i) >= 0) {
                values[i] = this._formatCurrency(values[i]);
            }
            else if (this._percentageFields.indexOf(i) >= 0) {
                values[i] = this._formatPercentage(values[i]);
            }
            else if (this._dateFields.indexOf(i) >= 0) {
                values[i] = this._formatDate(values[i]);
            }
        }

        var row = this._rowTemplate.templateApply(values);

        if (isNew)
            $(this._element).find('tbody').append(row);
        else
            $(this._element).find("tbody tr").eq(this._editIndex).replaceWith(row);

    },
    
    _formatDate: function (value) {
        return value.toLocaleDateString();
    }
    ,
    _formatCurrency: function (value) {
        return value.toFixed(2).toString().replace('.', ',') + ' &euro;';
    },
    
    _formatPercentage: function (value) {
        return value.toString().replace('.', ',');
    },

    _buildEdit: function () {
        if (!this._readOnly)
            return ["<a href='javascript:void(0);' rel='deleteRecord' class='img-btn bg-delete tooltip' style='float:left;' title='Διαγραφή'><img src='/_img/s.gif' style='height:16px;width:16px;border:none;' /></a>", ].join('');
        else
            return "<a href='javascript:void(0);' rel='editRecord' class='img-btn bg-viewDetails tooltip' title='Προβολή' style='float:left;'><img src='/_img/s.gif' style='height:16px;width:16px;border:none;' /></a>";
    },

    _buildMove: function () {
        if (this._readOnly)
            return "";
        else
            return ["<a href='javascript:void(0);' rel='moveUp' class='img-btn bg-arrow_up tooltip' style='float:left;' title='Μετακίνηση επάνω'><img src='/_img/s.gif' style='height:16px;width:16px;border:none;' /></a>",
                "<a href='javascript:void(0);' rel='moveDown' class='img-btn bg-arrow_down tooltip' style='float:left;' title='Μετακίνηση κάτω'><img src='/_img/s.gif' style='height:16px;width:16px;border:none;' /></a>", ].join('');
    },

    _areEditorsValid: function () {
        var valid = true;
        for (var i in this._inputControls) {
            this._inputControls[i].Validate();
            valid = valid && this._inputControls[i].GetIsValid();
        }
        return valid;
    },

    save_record: function () {
        if (this._readOnly) {
            return false;
        }

        if (ASPxClientEdit.ValidateGroup('vgItems') && Page_ClientValidate('vgItems')) {
            var record = (this._editIndex == this._NEW_RECORD_INDEX) ? {} : this._records[this._editIndex];
            for (var fieldname in this._inputControls) {
                record[fieldname] = this._inputControls[fieldname].GetValue();
            }

            if (this._editIndex == this._NEW_RECORD_INDEX)
                this._records.push(record);
            this._applyRowTemplate(record);

            this.clear_editForm();
            this._onRecordsChanged();
            $(this._emptyRow).hide();
            this._editIndex = this._NEW_RECORD_INDEX;
            this._popupControl.Hide();
       }
    },

    delete_record: function (index) {
        if (this._readOnly) {
            return false;
        }

        Array.removeAt(this._records, index);
        $("#" + this._element.id + " tbody tr").each(function (i) {
            if (i == index) $(this).remove();
            else if (i > index) $(this).children(":first").html(index + i);
        });

        this._onRecordsChanged();

        if (this._records.length == 0) {
            $(this._emptyRow).show();
        }
    },

    clear_editForm: function () {
        ASPxClientEdit.ClearGroup('vgItems');
        $("#" + this._element.id + " tbody tr").css('background-color', '');
        for (var fieldname in this._inputControls) {
            this._inputControls[fieldname].SetValue(null);
        }
        for (var i = 0; i < Page_Validators.length; i++) {
            Page_Validators[i].isvalid = true;
            Page_Validators[i].errormessage = "";
        }

        if (typeof (Page_ValidationSummaries) != "undefined") { //hide the validation summaries
            for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
                summary = Page_ValidationSummaries[sums];
                summary.style.display = "none";
            }
        }
    },

    close_editForm: function () {
        this.clear_editForm();
        this._popupControl.Hide();
        this._editIndex = this._NEW_RECORD_INDEX;
    },

    show_editForm: function (e, index) {
        e.preventDefault();
        if (index >= 0) {
            this._editIndex = index;
            for (var fieldname in this._inputControls) {
                this._inputControls[fieldname].SetValue(this._records[this._editIndex][fieldname]);
            }
        }
        this._popupControl.ShowInView(700, 400);
        for (var fieldname in this._inputControls) { this._inputControls[fieldname].Focus(); break; }
    },

    add_valueChanged: function (handler) {
        this.get_events().addHandler('valueChanged', handler);
    },

    remove_valueChanged: function (handler) {
        this.get_events().removeHandler('valueChanged', handler);
    },

    set_clientStateField: function (val) { this._clientStateField = val; },
    get_clientStateField: function () { return this._clientStateField; },

    set_records: function (val) { this._records = val; },
    get_records: function () { return this._records; },

    set_recordsCount: function (val) { this._recordsCount = val; },
    get_recordsCount: function () { return this._recordsCount; },

    set_popupControl: function (val) { this._popupControl = window[val]; },
    get_popupControl: function () { return this._popupControl; },

    set_addHandle: function (val) { this._addHandle = val; },
    get_addHandle: function () { return this._addHandle; },

    set_addHandleFooter: function (val) { this._addHandleFooter = val; },
    get_addHandleFooter: function () { return this._addHandleFooter; },

    set_submitHandle: function (val) { this._submitHandle = val; },
    get_submitHandle: function () { return this._submitHandle; },

    set_cancelHandle: function (val) { this._cancelHandle = val; },
    get_cancelHandle: function () { return this._cancelHandle; },

    get_cvRequired: function () { return this._cvRequired; },
    set_cvRequired: function (val) { this._cvRequired = val; },

    get_cvCurrentAmount: function () { return this._cvCurrentAmount; },
    set_cvCurrentAmount: function (val) { this._cvCurrentAmount = val; },
    
    get_isRequired: function () { return this._isRequired; },
    set_isRequired: function (val) { this._isRequired = val; },

    get_cvFinancialChecks: function () { return this._cvFinancialChecks; },
    set_cvFinancialChecks: function (val) { this._cvFinancialChecks = val; },

    get_readOnly: function () { return this._readOnly; },
    set_readOnly: function (val) { this._readOnly = val; },

    set_inputControls: function (val) { this._inputControls = val; },
    get_inputControls: function () { return this._inputControls; },

    get_totalAmount: function () { return this._totalAmount; },
    set_totalAmount: function (val) { this._totalAmount = val; },
}
EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit.registerClass('EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit', Sys.UI.Control);