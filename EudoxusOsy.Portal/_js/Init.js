$(function () {
    $('.hint').tipsy({ fade: true, gravity: $.fn.tipsy.autoSESW, live: true, trigger: 'click', className: 'info-tipsy' });
    $('.tooltip').tipsy({ fade: true, gravity: $.fn.tipsy.autoSESW, live: true });
    $('.tooltipLeft').tipsy({ fade: true, gravity: 'se', live: true });
});