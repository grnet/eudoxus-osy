$(function () {
    var gmap = new GoogleMap();
    var geocoder = new GoogleGeocoder();
    var marker = null;
    var autocomplete = null;

    gmap.initialize('map');
    geocoder.initialize();

    init();

    $("#btnFindLocation").click(function (e) {
        e.preventDefault();
        var addr = $('#txtFullAddress').val();
        geocoder.geocodeAddress(addr, function (result) {
            if (result) {
                handleGeocodeResult(result);
                addMarker(result.lat, result.lng);
            }
        });
        return false;
    });

    var txtAddr = document.getElementById('txtFullAddress');
    if (txtAddr){
        autocomplete = new google.maps.places.Autocomplete(txtAddr, {
            types: ['geocode'],
            componentRestrictions: { country: 'gr' }
        });
    }

    function handleGeocodeResult(result) {
        $("#hdfLocationPoint").val(result.lat + ";" + result.lng);
        $('#hdfFullAddress').val(result.address);

        var addr = result.addressParts.street;
        if (result.addressParts.streetNumber)
            addr = addr + ' ' + result.addressParts.streetNumber;

        $('#hdfAddressName').val(addr);
        $('#hdfZipCode').val(result.addressParts.zipCode.replace(' ', ''));
    }

    function addMarker(lat, lng) {
        if (marker != null)
            marker.setVisible(false);

        marker = gmap.addMarker(lat, lng, _draggableMarker, $('#hdfFullAddress').val());
        marker.onPosChanged(markerMoved);

        gmap.setMapCenter(lat, lng);
        gmap.setZoom(17);
    }

    function markerMoved(lat, lng) {
        geocoder.geocodeLocation(lat, lng, function (result) {
            if (result) {
                handleGeocodeResult(result);
                var addr = $('#hdfFullAddress').val();
                $('#txtFullAddress').val(addr);
                gmap.setInfoWindowContent(addr);
            }
        });
    }

    function valueChanged() {
        var street = txtAddressName.GetValue();
        var zipCode = txtZipCode.GetValue();
        var prefecture = ddlPrefecture.GetText();
        var city = ddlCity.GetText();

        if (street && zipCode && prefecture && city) {
            var addr = [street, ', ', zipCode].join('');
            geocoder.geocodeAddress(addr, function (result) {
                if (result) {
                    addMarker(result.lat, result.lng);
                    $("#hdfLocationPoint").val(result.lat + ";" + result.lng);
                    $('#hdfFullAddress').val(result.address);
                    $('#txtFullAddress').val(result.address);
                }
            });
        }
    }

    function init() {
        if ($("#hdfLocationPoint").val() != '') {
            var parts = $("#hdfLocationPoint").val().split(';');
            var lat = parseFloat(parts[0].replace(',', '.'));
            var lng = parseFloat(parts[1].replace(',', '.'));

            addMarker(lat, lng);
            $('#txtFullAddress').val($("#hdfFullAddress").val());
        }
        else {
            if (_initialAddress) {
                geocoder.geocodeAddress(_initialAddress, function (result) {
                    if (result) {
                        handleGeocodeResult(result);
                        addMarker(result.lat, result.lng);
                        $('#txtFullAddress').val($("#hdfFullAddress").val());
                    }
                });
            }
        }
    }
});

function cvGoogleMapsAddressRequiredValidate(s, e) {
    if ($("#hdfLocationPoint").val() != '') {
        e.IsValid = true;
    }
    else {
        e.IsValid = false;
    }
    ValidatorUpdateDisplay(cvGoogleMapsAddressRequired);
}