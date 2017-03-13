interface Window {
    EventBus: Imis.IEventBus;
}

module Imis {

    export interface IEventBus {
        on(eventName: string, handler: (args: EventArgs) => any);
        un(eventName: string, handler: (args: EventArgs) => any);
        publish(eventName: string, args: EventArgs);
    }

    export class EventArgs { 
        static Empty = {};
    }

    class EventList {
        private _invocationHash = {};

        private _getHandlers(windowId: number, eventName: string) {
            if (!this._invocationHash[eventName])
                this._invocationHash[eventName] = {};

            if (!this._invocationHash[eventName][windowId])
                this._invocationHash[eventName][windowId] = new Array();

            var handlers: { (args: EventArgs): any }[] = this._invocationHash[eventName][windowId];
            return handlers;
        }

        public add(windowId: number, eventName: string, handler: (args: EventArgs) => any) {
            this._getHandlers(windowId, eventName).push(handler);
        }

        public remove(windowId: number, eventName: string, handler: (args: EventArgs) => any) {
            var handlers = this._getHandlers(windowId, eventName);
            for (var i = handlers.length - 1; i >= 0; i--)
                if (handlers[i] === handler)
                    handlers.splice(i, 1);
        }

        public clearHandlers(windowId: number) {
            // TODO: clear handlers for given windowId
        }

        public getHandlers(eventName: string): { (args: EventArgs): any }[]  {
            var invocationList = [];
            if (this._invocationHash[eventName])
                for (var wId in this._invocationHash[eventName])
                    for (var i = 0; i < this._invocationHash[eventName][wId].length; i++)
                        invocationList.push(this._invocationHash[eventName][wId][i]);
            return invocationList;
        }
    }

    class EventBus implements IEventBus {
        private _eventSink: EventSink;
        private _windowId: number;

        constructor(windowId: number, eventSink: EventSink) {
            this._windowId = windowId;
            this._eventSink = eventSink;
        }

        public on(eventName: string, handler: (args: EventArgs) => any) {
            this._eventSink.on(this._windowId, eventName, handler);
        }

        public un(eventName: string, handler: (args: EventArgs) => any) {
            this._eventSink.un(this._windowId, eventName, handler);
        }

        public publish(eventName: string, args: EventArgs) {
            this._eventSink.publish(eventName, args);
        }
    }

    interface ImisWindow extends Window {
        _wId: number;
        _eventSink: EventSink;
    }

    class EventSink {
        private _idSeed = 1;
        private _eventList = new EventList();
        private _getNextId() {
            return this._idSeed++;
        }

        private _clearWindowHandlers(windowId: number) {
            this._eventList.clearHandlers(windowId);
        }

        public ensureBus(win: Window) {
            var w = <ImisWindow> win; // Cast Window to ImisWindow
            if (w._wId) {
                w._wId = this._getNextId();
            }

            var eventBus: IEventBus = w.EventBus;
            if (!w.EventBus) {
                eventBus = new EventBus(w._wId, this);
                w.onunload = function (e) {
                    this._clearWindowHandlers(w._wId);
                }
            }

            return eventBus;
        }
        
        public on(windowId: number, eventName: string, handler: (EventArgs) => any, scope?) {
            this._eventList.add(windowId, eventName, handler);
        }

        public un(windowId: number, eventName: string, handler: (EventArgs) => any, scope?) {
            this._eventList.remove(windowId, eventName, handler);
        }

        public publish(eventName: string, eventArgs: EventArgs) {
            var handlers = this._eventList.getHandlers(eventName);
            for (var i = 0; i < handlers.length; i++) {
                handlers[i](eventArgs);
            }
        }

        static init() {
            var topWindow = window;
            while(topWindow !== topWindow.top)
                topWindow = topWindow.top;

            if (!topWindow["EventSink"])
                topWindow["EventSink"] = new EventSink();
            
            var eventSink: EventSink = topWindow["EventSink"];
            return eventSink.ensureBus(window);
        }
    }

    window.EventBus = EventSink.init();
}