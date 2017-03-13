var endpoint = '';
var auth = null;

ServiceProxy = function () {
    function call(method, url, jsonData, onSuccess, onError) {
        var data = null;

        if (jsonData == null) {
            data = {};
        }
        else {
            data = JSON.stringify(jsonData);
        }

        $.ajax({
            url: url,
            data: data,
            type: method,
            beforeSend: function (xhrObj) {
                xhrObj.setRequestHeader("access_token", auth);
            },
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (typeof (onSuccess) === 'function') {
                    onSuccess(data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (typeof (onError) === 'function') {
                    onError({
                        HttpCode: xhr.status,
                        StatusText: textStatus,
                        HttpStatus: errorThrown,
                        ResponseText: xhr.responseText
                    });
                }
            }
        });
    }

    return {
        /*
        Login: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'common/login';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetTelecomProviders: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'common/telecomproviders';
            call('GET', url, jsonData, onSuccess, onError);
        },
        */
       
        /*------------------------------------------------------------------------------------------*/
    };
}();