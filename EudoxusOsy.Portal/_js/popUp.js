var popUp = function() {
    var _onHideCallback = null;

    var _wasAlreadyShown = false;
    var _currentIFrame = null;

    var _showWithSize = function (width, height) {
        var w = width;
        if (w > (window.innerWidth - 200))
            w = window.innerWidth - 200;

        var h = height;
        if (h > (window.innerHeight - 100))
            h = window.innerHeight - 100;

        if (!_wasAlreadyShown) {
            devExPopup.Shown.AddHandler(function (s, e) {
                devExPopup.SetSize(w, h);
                devExPopup.UpdatePosition();
                devExPopup.Shown.ClearHandlers();
            });

            devExPopup.Show();
            _wasAlreadyShown = true;
        }
        else {
            devExPopup.Show();
            devExPopup.SetSize(w, h);
            devExPopup.UpdatePosition();
        }
    }

    function GetIframeContainer() {
        var id = $('.iframePopup').find('.dxpc-content').attr('id');
        return document.getElementById(id);
    }

    function GetNewIFrame() {
        var iframeContainer = GetIframeContainer();
        var iframe = iframeContainer.childNodes[0];
        if (iframe) {
            iframeContainer.removeChild(iframe);
        }
        iframe = CreateIFrame();
        iframeContainer.appendChild(iframe);

        _currentIFrame = iframe;
        return iframe;
    }

    function CreateIFrame() {
        iframe = document.createElement("IFRAME");
        iframe.scrolling = "auto";
        iframe.frameBorder = 0;
        iframe.style.width = "100%";
        iframe.style.height = "100%";
        return iframe;
    }

    function SetContentUrl(src) {
        GetNewIFrame().src = src;
    }

    function UpdatePosition(width, height) {
        var w = width;
        if (w > (window.innerWidth - 200))
            w = window.innerWidth - 200;

        var h = height;
        if (h > (window.innerHeight - 50))
            h = window.innerHeight - 50;

        devExPopup.SetSize(w, h);
        devExPopup.UpdatePosition();
    }

    return {

        getCurrentIFrame: function () {
            return _currentIFrame.contentWindow;
        },

        show: function (url, title, callback, width, height) {
            SetContentUrl(url);

            if (title)
                devExPopup.SetHeaderText(title);

            if (callback)
                _onHideCallback = callback;

            if (width != null && height != null)
                _showWithSize(width, height);
            else
                devExPopup.Show();
        },

        showDynamic: function(url, title, width, height, callback) {
            SetContentUrl(url);

            if (title)
                devExPopup.SetHeaderText(title);

            if (callback)
                _onHideCallback = callback;

            screenHeight = screen.height;

            if (screenHeight < height + 150) {
                height = height - 250;
            }

            _showWithSize(width, height);
        },

        showSpecific: function (url, title, callback, width, height) {
            SetContentUrl(url);

            if (title)
                devExPopup.SetHeaderText(title);

            if (callback)
                _onHideCallback = callback;

            _showWithSize(width, height);
        },

        hide: function () {
            SetContentUrl('about:blank');
            devExPopup.Hide();

            if (typeof _onHideCallback == 'function') {
                _onHideCallback();
            }
        },

        hideWithoutRefresh: function () {
            SetContentUrl('about:blank');
            devExPopup.Hide();
            return false;
        },

        hideWithoutReset: function () {
            devExPopup.Hide();
            return false;
        },

        refresh: function () {
            if (hideAllButtons){
                hideAllButtons();
            }
            SetContentUrl(devExPopup.GetContentUrl());
        },

        init: function() {
            return false;
        },

        instance: function () {
            return devExPopup;
        },

        print: function () {
            var popupContentWindow = _currentIFrame.contentWindow;
            popupContentWindow.focus();
            popupContentWindow.print();
        }
    };
} ();