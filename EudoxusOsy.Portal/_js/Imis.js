var Imis;
(function (Imis) {
    var EventArgs = (function () {
        function EventArgs() {
        }
        EventArgs.Empty = {};
        return EventArgs;
    }());
    Imis.EventArgs = EventArgs;
    var EventList = (function () {
        function EventList() {
            this._invocationHash = {};
        }
        EventList.prototype._getHandlers = function (windowId, eventName) {
            if (!this._invocationHash[eventName])
                this._invocationHash[eventName] = {};
            if (!this._invocationHash[eventName][windowId])
                this._invocationHash[eventName][windowId] = new Array();
            var handlers = this._invocationHash[eventName][windowId];
            return handlers;
        };
        EventList.prototype.add = function (windowId, eventName, handler) {
            this._getHandlers(windowId, eventName).push(handler);
        };
        EventList.prototype.remove = function (windowId, eventName, handler) {
            var handlers = this._getHandlers(windowId, eventName);
            for (var i = handlers.length - 1; i >= 0; i--)
                if (handlers[i] === handler)
                    handlers.splice(i, 1);
        };
        EventList.prototype.clearHandlers = function (windowId) {
            // TODO: clear handlers for given windowId
        };
        EventList.prototype.getHandlers = function (eventName) {
            var invocationList = [];
            if (this._invocationHash[eventName])
                for (var wId in this._invocationHash[eventName])
                    for (var i = 0; i < this._invocationHash[eventName][wId].length; i++)
                        invocationList.push(this._invocationHash[eventName][wId][i]);
            return invocationList;
        };
        return EventList;
    }());
    var EventBus = (function () {
        function EventBus(windowId, eventSink) {
            this._windowId = windowId;
            this._eventSink = eventSink;
        }
        EventBus.prototype.on = function (eventName, handler) {
            this._eventSink.on(this._windowId, eventName, handler);
        };
        EventBus.prototype.un = function (eventName, handler) {
            this._eventSink.un(this._windowId, eventName, handler);
        };
        EventBus.prototype.publish = function (eventName, args) {
            this._eventSink.publish(eventName, args);
        };
        return EventBus;
    }());
    var EventSink = (function () {
        function EventSink() {
            this._idSeed = 1;
            this._eventList = new EventList();
        }
        EventSink.prototype._getNextId = function () {
            return this._idSeed++;
        };
        EventSink.prototype._clearWindowHandlers = function (windowId) {
            this._eventList.clearHandlers(windowId);
        };
        EventSink.prototype.ensureBus = function (win) {
            var w = win; // Cast Window to ImisWindow
            if (w._wId) {
                w._wId = this._getNextId();
            }
            var eventBus = w.EventBus;
            if (!w.EventBus) {
                eventBus = new EventBus(w._wId, this);
                var self = this;
                w.onunload = function (e) {
                    self._clearWindowHandlers(w._wId);
                };
            }
            return eventBus;
        };
        EventSink.prototype.on = function (windowId, eventName, handler, scope) {
            this._eventList.add(windowId, eventName, handler);
        };
        EventSink.prototype.un = function (windowId, eventName, handler, scope) {
            this._eventList.remove(windowId, eventName, handler);
        };
        EventSink.prototype.publish = function (eventName, eventArgs) {
            var handlers = this._eventList.getHandlers(eventName);
            for (var i = 0; i < handlers.length; i++) {
                handlers[i](eventArgs);
            }
        };
        EventSink.init = function () {
            var topWindow = window;
            while (topWindow !== topWindow.top)
                topWindow = topWindow.top;
            if (!topWindow["EventSink"])
                topWindow["EventSink"] = new EventSink();
            var eventSink = topWindow["EventSink"];
            return eventSink.ensureBus(window);
        };
        return EventSink;
    }());
    window.EventBus = EventSink.init();
})(Imis || (Imis = {}));
