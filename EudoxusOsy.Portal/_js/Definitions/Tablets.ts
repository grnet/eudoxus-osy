/// <reference path="Imis.ts"/>

module EudoxusOsy {
    export interface IEvent<T extends Imis.EventArgs> {
        on(handler: (payload: T) => any);
        un(handler: (payload: T) => any);
        publish(payload: T);
    }   

    class BusEvent<T extends Imis.EventArgs> implements IEvent<T> {
        private _eventName: string;

        constructor(eventName: string) {
            this._eventName = eventName;
        }

        public on(handler: (payload: T) => any) {
            window.EventBus.on(this._eventName, handler);
        }

        public un(handler: (payload: T) => any) {
            window.EventBus.un(this._eventName, handler);
        }

        public publish(payload: T) {
            window.EventBus.publish(this._eventName, payload);
        }
    }

    export class ProviderInfo extends Imis.EventArgs {
        providerID: number;
        name: string;
        constructor(providerID: number, name: string) {
            super();
            this.providerID = providerID;
            this.name = name;
        }
    }

    export class ClearanceInfo extends Imis.EventArgs {
        clearanceID: number;
        cleranceType: string;
        constructor(clearanceID: number, cleranceType: string) {
            super();
            this.clearanceID = clearanceID;
            this.cleranceType = cleranceType;
        }
    }

    export class EventLocator {
        static ProviderLocked = new BusEvent<ProviderInfo>("providerlocked");
        static ProviderUnlocked = new BusEvent<ProviderInfo>("providerunlocked");
        static ClearanceSaved = new BusEvent<ClearanceInfo>("clearancesaved");
    }
}

//EudoxusOsy.EventLocator.ProviderLocked.on(function (p) {
//    alert("Provider Locked:" + p.name + " with ID: " + p.providerID);
//});