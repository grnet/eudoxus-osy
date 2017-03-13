Imis = Imis || {};
Imis.EventSink = function () {
    var _idSeed = 1;
    var _eventList = {};

    var _getNextId = function () {
        return _idSeed++;
    }

    var _clearWindowHandlers = function (wId) {
        for (var eventName in _eventList) {
            if (_eventList[eventName][wId]) {
                while (_eventList[eventName][wId].length > 0)
                    _eventList[eventName][wId].pop();
                delete _eventList[eventName][wId];
            }
        }
    }

    var _invoke = function (handler, scope, eventArgs) {
        if (scope) {
            // TODO: apply handler to scope alert('lol');
        }
        else
            handler(eventArgs);
    }

    return {
        ensureBus: function(winInstance) {
            if (!winInstance._wId) {
                winInstance._wId = _getNextId();
            }

            var eventBus = winInstance.EventBus;
            if (!winInstance.EventBus) {
                eventBus = new Imis.EventBus(this, winInstance._wId);
                winInstance.onunload = function () {
                    _clearWindowHandlers(winInstance._wId);
                    //console.log('Window unloaded: ' + winInstance._wId);
                };
            }

            return eventBus;
        },
        on: function (windowId, eventName, handler, scope) {
            if (!_eventList[eventName])
                _eventList[eventName] = {};

            if (!_eventList[eventName][windowId])
                _eventList[eventName][windowId] = [];                
            
            _eventList[eventName][windowId].push(handler);
        },
        un: function (windowId, eventName, handler) {
            if (!_eventList[eventName])
                return;

            if (!_eventList[eventName][windowId])
                return;
            
            var handlers = _eventList[eventName][windowId];
            for (var i = handlers.length - 1; i >= 0; i--)
                if (handlers[i] === handler)
                    handlers.splice(i, 1);
        },
        publish: function (eventName, eventArgs) {
            var invocationList = [];
            if (_eventList[eventName])
                for (var wId in _eventList[eventName])
                    for (var i = 0; i < _eventList[eventName][wId].length; i++)
                        invocationList.push(_eventList[eventName][wId][i]);                                   

            for (var i = 0; i < invocationList.length; i++)
                _invoke(invocationList[i], null, eventArgs);
        }
    };
};

Imis.EventSink.init = function () {
    /// <returns value='new Imis.EventBus()' />
    var topWindow = window;
    while (topWindow !== topWindow.top)
        topWindow = topWindow.top;

    if (!topWindow.EventSink)
        topWindow.EventSink = new Imis.EventSink();

    var eventSink = topWindow.EventSink;    
    return eventSink.ensureBus(window);
};

Imis.EventBus = function (eventSink, windowId) {
    /// <param name='eventSink' value='new Imis.EventSink()'/>
    /// <param name='windowId' type='Number'  />
    var _eventSink = eventSink;
    var _windowId = windowId;

    return {
        on: function (eventName, handler) {
            _eventSink.on(_windowId, eventName, handler);
        },
        un: function (eventName, handler) {
            _eventSink.un(_windowId, eventName, handler);
        },
        publish: function (eventName, eventArgs) {
            _eventSink.publish(eventName, eventArgs);
        }
    };
};

EventBus = Imis.EventSink.init();