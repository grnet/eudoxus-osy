/// <reference path="Imis.ts"/>
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var EudoxusOsy;
(function (EudoxusOsy) {
    var BusEvent = (function () {
        function BusEvent(eventName) {
            this._eventName = eventName;
        }
        BusEvent.prototype.on = function (handler) {
            window.EventBus.on(this._eventName, handler);
        };
        BusEvent.prototype.un = function (handler) {
            window.EventBus.un(this._eventName, handler);
        };
        BusEvent.prototype.publish = function (payload) {
            window.EventBus.publish(this._eventName, payload);
        };
        return BusEvent;
    }());
    var ProviderInfo = (function (_super) {
        __extends(ProviderInfo, _super);
        function ProviderInfo(providerID, name) {
            _super.call(this);
            this.providerID = providerID;
            this.name = name;
        }
        return ProviderInfo;
    }(Imis.EventArgs));
    EudoxusOsy.ProviderInfo = ProviderInfo;
    var ClearanceInfo = (function (_super) {
        __extends(ClearanceInfo, _super);
        function ClearanceInfo(clearanceID, cleranceType) {
            _super.call(this);
            this.clearanceID = clearanceID;
            this.cleranceType = cleranceType;
        }
        return ClearanceInfo;
    }(Imis.EventArgs));
    EudoxusOsy.ClearanceInfo = ClearanceInfo;
    var EventLocator = (function () {
        function EventLocator() {
        }
        EventLocator.ProviderLocked = new BusEvent("providerlocked");
        EventLocator.ProviderUnlocked = new BusEvent("providerunlocked");
        EventLocator.ClearanceSaved = new BusEvent("clearancesaved");
        return EventLocator;
    }());
    EudoxusOsy.EventLocator = EventLocator;
})(EudoxusOsy || (EudoxusOsy = {}));
//EudoxusOsy.EventLocator.ProviderLocked.on(function (p) {
//    alert("Provider Locked:" + p.name + " with ID: " + p.providerID);
//}); 
