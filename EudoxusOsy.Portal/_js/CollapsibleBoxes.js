
$(function () {
    $('.collapsibleHidden').hide();

    var cValues = getCookieValue();
    for (var i in cValues) {
        if (cValues[i] == '0') {
            var rootContainer = $('#' + i);
            var triggers = $(rootContainer).find('.collapsibleTrigger');
            if (triggers.length != 0) {
                toggleCollapsible(0, triggers[0]);
            }
        }
    }

    $('.collapsibleTrigger').click(function () {
        toggleCollapsible(300, this, function () {
            setCookie(updateCookieValue());
        });
        return false;
    });

    function toggleCollapsible(speed, trigger, callback) {
        var rp = $(trigger).closest('.rp');
        var cont = $(rp).find('.collapsibleContainer');
        var head = $(rp).find('.collapsibleHeader');
        var hidden = $(rp).find('.collapsibleHidden');
        var img = $(rp).find('.collapsibleTrigger img');

        $(cont).slideToggle({
            duration: speed,
            complete: function () {
                if (img.attr("src").indexOf("iconMinimize.png") > 0)
                    img.attr("src", img.attr("src").replace("iconMinimize.png", "iconMaximize.png"));
                else
                    img.attr("src", img.attr("src").replace("iconMaximize.png", "iconMinimize.png"));
                if (typeof (callback) === 'function')
                    callback();
                $(head).slideToggle(true);
                $(hidden).toggle(speed);
            }
        });
    }

    function getCookieValue() {
        var result = {};
        var strVal = $.cookie('CollapsibleBoxes');

        if (strVal != null && typeof (strVal) !== 'undefined') {
            var curItem;
            var items = strVal.split(';');
            for (var i = 0; i < items.length; i++) {
                curItem = items[i].split('=');
                result[curItem[0]] = curItem[1];
            }
        }
        return result;
    }

    function setCookie(values) {
        var result = [];
        for (var i in values) {
            result.push(i + '=' + values[i]);
        }
        $.cookie('CollapsibleBoxes', result.join(';'), { path: '/' });
    }

    function updateCookieValue(values) {
        if (typeof (values) === 'undefined') {
            values = getCookieValue();
        }

        $('.collapsibleContainer').each(function () {
            var relId = $(this).closest('.rp').attr('id');
            values[relId] = $(this).is(':visible') ? '1' : '0';
        });
        return values;
    }

});