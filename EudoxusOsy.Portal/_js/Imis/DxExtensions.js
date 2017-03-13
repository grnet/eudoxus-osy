/// <reference path="ASPxScriptIntelliSense.js" />
$(function () {

    //TODO: Please Implement!!!
    //if (typeof(ASPxClientGridView) !== 'undefined')
    //    $.extend(ASPxClientGridView.prototype, {
    //        MyDoCallback: function (args, onCompleted) {
    //            var endCallbackHandler = function (s, e) {
    //                if (onCompleted)
    //                    onCompleted(s, e);
    //                s.EndCallback.RemoveHandler(endCallbackHandler);
    //            };

    //            this.EndCallback.AddHandler(endCallbackHandler);
    //            this.PerformCallback(args);
    //        }
    //    }); 

    if (typeof (ASPxClientPopupControl) !== 'undefined')
        $.extend(ASPxClientPopupControl.prototype, {
            ShowInView: function (maxWidth, maxHeight, widthToSubtract, heightToSubtract) {
                var popup = ASPxClientPopupControl.Cast(this);

                var width = maxWidth || 900;
                var height = maxHeight || 850;

                var subtractWidth = widthToSubtract || 100;
                var subtractHeight = heightToSubtract || 100;

                if (width > (window.innerWidth - subtractWidth))
                    width = window.innerWidth - subtractWidth;

                if (height > (window.innerHeight - subtractHeight))
                    height = window.innerHeight - subtractHeight;

                popup.SetSize(width, height);
                popup.Show();
                popup.UpdatePosition();
            }
        });

    if (typeof (ASPxClientUploadControl) !== 'undefined') {
        ASPxClientUploadControl.prototype._updateTimer = false;

        ASPxClientUploadControl.prototype.SuperCancel = function () {
            var self = this;

            if (self._updateTimer != null) {
                clearTimeout(self._updateTimer);
            }
            self.Cancel();
        }

        ASPxClientUploadControl.prototype.SuperUpload = function () {
            var self = this;
            var _isFileUploaded = false;

            var _updateProgress = function () {
                if (_isFileUploaded)
                    return;

                $.get('/ASPxUploadProgressHandlerPage.ashx?DXProgressHandlerKey=' + self.uploadingKey, function (xml) {
                    //<status fileName="70-480.pdf" fileSize="0" fileUploadedSize="0" fileProgress="100" contentType="application/pdf" totalUploadedSize="36864" totalSize="1154327" progress="3">
                    var $xml = $(xml.activeElement);
                    var isEmpty = $xml.attr('empty');
                    if (typeof (isEmpty) !== 'undefined' && isEmpty == 'true') {
                        self._updateTimer = setTimeout(_updateProgress, 1000);
                    }
                    else {
                        var fileCount = 1;
                        var fileName = $xml.attr('fileName');
                        var fileContentLength = parseInt($xml.attr('totalSize'));
                        var fileUploadedSize = parseInt($xml.attr('totalUploadedSize'));
                        var progress = parseInt($xml.attr('progress'));
                        self.RaiseUploadingProgressChanged(1, fileName, fileContentLength, fileUploadedSize, progress, fileContentLength, fileUploadedSize, progress);
                        self._updateTimer = setTimeout(_updateProgress, 1000);
                    }
                });
            }

            var _onUploadStarted = function () {
                self._updateTimer = setTimeout(_updateProgress, 1000);
            }

            var _onUploadCompleted = function () {
                _isFileUploaded = true;
                clearTimeout(self._updateTimer);
            }

            if (!self.IsSlUploadHelperEnabled()) {
                self.FileUploadStart.AddHandler(_onUploadStarted);
                self.FileUploadComplete.AddHandler(_onUploadCompleted);
                self.Upload();
            }
            else {
                self.Upload();
            }
        }
    }
});

