GoogleMap = function () {
    this._mapId = null;
    this._map = null;
    this._iwOpen = null;
    this._mapClickHandlerList = [];
    this._mapTypeID = google.maps.MapTypeId.ROADMAP;
    this._hiddenLabels = false;
};

GoogleMap.prototype = {
    initialize: function (mapId, customOptions) {

        if (typeof (customOptions) !== 'undefined'
            && typeof (customOptions.hideCountryLabels) !== 'undefined'
            && customOptions.hideCountryLabels == true) {
            this._mapTypeID = 'CustomMap'
            this._hiddenLabels = true;
        }

        var opt = {
            zoom: 14,
            center: new google.maps.LatLng(37.975418, 23.735694),
            mapTypeId: this._mapTypeID,
            disableDoubleClickZoom: true,
            draggable: true,
            streetViewControl: false,
            mapTypeControl: false,
            panControl: false
        };

        if (customOptions) {
            $.extend(opt, customOptions);
        }

        this._mapId = mapId;
        this._map = new google.maps.Map(document.getElementById(this._mapId), opt);

        if (this._hiddenLabels) {
            this._hideCountryLabels();
        }

        google.maps.event.addListener(this._map, 'click', Function.createDelegate(this, function (e) {
            if (this._iwOpen != null) {
                this._iwOpen.close();
                this._iwOpen = null;
            }

            this._mapClicked(e);
        }));
    },

    _hideCountryLabels: function () {
        var featureOpts = [{
            featureType: "administrative.country",
            elementType: "labels",
            stylers: [{ visibility: "off" }]
        }];

        var customMapType = new google.maps.StyledMapType(featureOpts, {
            name: 'No country labels'
        });

        this._map.mapTypes.set(this._mapTypeID, customMapType);
    },

    _mapClicked: function (e) {
        for (var i = 0; i < this._mapClickHandlerList.length; i++) {
            var handler = this._mapClickHandlerList[i];
            handler(e.latLng);
        }
    },

    onClick: function (handler) {
        if (typeof (handler) === 'function') {
            this._mapClickHandlerList.push(handler);
        }
    },

    setMapCenter: function (lat, lng) {
        this._map.setCenter(new google.maps.LatLng(lat, lng));
    },

    setMapBounds: function (lat1, lng1, lat2, lng2) {
        var sw = new google.maps.LatLng(lat1, lng1);
        var ne = new google.maps.LatLng(lat2, lng2);
        this._map.fitBounds(new google.maps.LatLngBounds(sw, ne));
    },

    setZoom: function (zoom) {
        this._map.setZoom(zoom);
    },

    setInfoWindowContent: function (iwContent) {
        if (this._iwOpen != null) {
            this._iwOpen.setContent(iwContent);
        }
    },

    resize: function () {
        google.maps.event.trigger(this._map, 'resize');
    },

    getMap: function () {
        return this._map;
    },

    addMarker: function (lat, lng, drag, iwContent, icon) {
        var opt = null;
        if (typeof (icon) === 'undefined') {
            var opt = {
                map: this._map,
                position: new google.maps.LatLng(lat, lng),
                draggable: drag
            };
        }
        else {
            var opt = {
                map: this._map,
                position: new google.maps.LatLng(lat, lng),
                draggable: drag,
                icon: icon
            };
        }

        var m = new google.maps.Marker(opt);

        if (typeof (iwContent) !== 'undefined') {
            var iw = new google.maps.InfoWindow();
            iw.setContent(iwContent);

            google.maps.event.addListener(m, 'click', Function.createDelegate(this, function () {
                if (this._iwOpen != null)
                    this._iwOpen.close();
                iw.open(this._map, m);
                this._iwOpen = iw;
            }));

            google.maps.event.addListener(m, 'visible_changed', Function.createDelegate(this, function () {
                if (this._iwOpen != null && this._iwOpen === iw) {
                    iw.close();
                    this._iwOpen = null;
                }
            }));
        }

        var marker = new GoogleMarker();
        marker.initialize(m);

        return marker;
    }
};