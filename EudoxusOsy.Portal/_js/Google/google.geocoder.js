GoogleGeocoder = function () {
    this._geocoder = null;
};

GoogleGeocoder.prototype = {

    initialize: function () {
        this._geocoder = new google.maps.Geocoder();
    },

    _parseAddressComponents: function (addressComponents) {
        var street = null;
        var number = null;
        var zipCode = null;
        var area = null;
        var country = null;

        for (var i = 0; i < addressComponents.length; i++) {
            if (Array.contains(addressComponents[i].types, 'route')) {
                street = addressComponents[i].long_name;
                continue;
            }
            if (Array.contains(addressComponents[i].types, 'street_number')) {
                number = addressComponents[i].long_name;
                continue;
            }
            if (Array.contains(addressComponents[i].types, 'postal_code')) {
                zipCode = addressComponents[i].long_name;
                continue;
            }
            if (Array.contains(addressComponents[i].types, 'locality')) {
                area = addressComponents[i].long_name;
                continue;
            }
            if (Array.contains(addressComponents[i].types, 'country')) {
                country = addressComponents[i].long_name;
                continue;
            }
        }

        return {
            street: street,
            streetNumber: number,
            area: area,
            zipCode: zipCode,
            country: country
        };
    },


    geocodeLocation: function (lat, lng, callback) {
        this._geocoder.geocode({
            location: new google.maps.LatLng(lat, lng)
        }, Function.createDelegate(this, function (results, status) {
            var result = results[0];
            if (result) {
                callback({
                    addressParts: this._parseAddressComponents(result.address_components),
                    address: result.formatted_address,
                    lat: result.geometry.location.lat(),
                    lng: result.geometry.location.lng()
                });
            }
            else {
                callback(null);
            }
        }));
    },

    geocodeAddress: function (address, callback) {
        this._geocoder.geocode({
            address: address
        }, Function.createDelegate(this, function (results, status) {
            var result = results[0];
            if (result) {
                callback({
                    addressParts: this._parseAddressComponents(result.address_components),
                    address: result.formatted_address,
                    lat: result.geometry.location.lat(),
                    lng: result.geometry.location.lng()
                });
            }
            else {
                callback(null);
            }
        }));
    }

};