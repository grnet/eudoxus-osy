GoogleMarker = function () {
    this._marker = null;
    this._posChangedHandlerList = [];
};

GoogleMarker.prototype = {
    initialize: function (m) {
        this._marker = m;

        google.maps.event.addListener(this._marker, "dragend", Function.createDelegate(this, function (e) {
            this._posChanged(e);
        }));
    },

    getMarker: function(){
        return this._marker;
    },

    setVisible: function (vis) {
        this._marker.setVisible(vis);
    },

    getPosition: function () {
        return this._marker.getPosition();
    },
    
    onPosChanged: function (handler) {
        if (typeof (handler) === 'function') {
            this._posChangedHandlerList.push(handler);
        }
    },

    _posChanged: function (e) {
        for (var i = 0; i < this._posChangedHandlerList.length; i++) {
            var handler = this._posChangedHandlerList[i];
            handler(e.latLng.lat(), e.latLng.lng());
        }
    }
};