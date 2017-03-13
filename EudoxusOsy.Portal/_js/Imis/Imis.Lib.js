///<reference path="ASPxScriptIntelliSense.js" />

Type.registerNamespace("Imis.Lib");

Imis.Lib.PopupRequestManager = (function () {

    var inPostback = false;
    var buttonTrigger = null;

    var loadHandler = function () {
        if (inPostback) {
            if (buttonTrigger) {
                buttonTrigger.SetEnabled(true);
                buttonTrigger = null;
            }
            inPostback = false;
        }
    };

    return {
        Run: function (s, handler) {
            buttonTrigger = s;
            inPostback = true;

            devExPopup.GetContentIFrameWindow().onAjaxPageLoaded(loadHandler);

            buttonTrigger.SetEnabled(false);
            var handlerResult = handler();
            var handlerIsValid = handlerResult === undefined ? true : handlerResult;

            setTimeout(function () {
                var dxValid = ASPxClientEdit.AreEditorsValid(devExPopup.GetContentIFrameWindow(), '', true);
                var aspValid = devExPopup.GetContentIFrameWindow().Page_IsValid;
                if (!dxValid || !aspValid || !handlerIsValid) {
                    buttonTrigger.SetEnabled(true);
                    buttonTrigger = null;
                    inPostback = false;
                }
            }, 500);
        }
    };

})();

Imis.Lib.ShowConstrained = function (popupCtrl) {

}

Imis.Lib.GetQueryStringParameter = function (name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

Imis.Lib.HtmlEncode = function (value) {
    return $('<div/>').text(value).html();
}

Imis.Lib.HtmlDecode = function (value) {
    return $('<div/>').html(value).text();
}

Imis.Lib.IsNullOrWhiteSpace = function (value) {
    return value === null || value.match(/^ *$/) !== null;
}

Imis.Lib.ToHumanReadableFileSize = function (byteSize) {
    var sizes = ["B", "KB", "MB", "GB"];
    var order = 0;
    while (byteSize >= 1024 && order + 1 < sizes.length) {
        order++;
        byteSize = byteSize / 1024;
    }
    return (Math.round(byteSize * 100) / 100) + ' ' + sizes[order];
}

Imis.Lib.ToUpper = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';

        /* Special characters */
        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9') { change = 1; cOut = c; }
        if (c == ' ' || c == '-' || c == '_' || c == ',' || c == '.' || c == '(' || c == ')' || c == '/' || c == '&') { cOut = c; }

        /* English Characters ToUpperCase */
        if (c >= 'A' && c <= 'Z') { cOut = c; }
        if (c == 'a') { change = 1; cOut = 'A'; }
        if (c == 'b') { change = 1; cOut = 'B'; }
        if (c == 'c') { change = 1; cOut = 'C'; }
        if (c == 'd') { change = 1; cOut = 'D'; }
        if (c == 'e') { change = 1; cOut = 'E'; }
        if (c == 'f') { change = 1; cOut = 'F'; }
        if (c == 'g') { change = 1; cOut = 'G'; }
        if (c == 'h') { change = 1; cOut = 'H'; }
        if (c == 'i') { change = 1; cOut = 'I'; }
        if (c == 'j') { change = 1; cOut = 'J'; }
        if (c == 'k') { change = 1; cOut = 'K'; }
        if (c == 'l') { change = 1; cOut = 'L'; }
        if (c == 'm') { change = 1; cOut = 'M'; }
        if (c == 'n') { change = 1; cOut = 'N'; }
        if (c == 'o') { change = 1; cOut = 'O'; }
        if (c == 'p') { change = 1; cOut = 'P'; }
        if (c == 'q') { change = 1; cOut = 'Q'; }
        if (c == 'r') { change = 1; cOut = 'R'; }
        if (c == 's') { change = 1; cOut = 'S'; }
        if (c == 't') { change = 1; cOut = 'T'; }
        if (c == 'u') { change = 1; cOut = 'U'; }
        if (c == 'v') { change = 1; cOut = 'V'; }
        if (c == 'w') { change = 1; cOut = 'W'; }
        if (c == 'x') { change = 1; cOut = 'X'; }
        if (c == 'y') { change = 1; cOut = 'Y'; }
        if (c == 'z') { change = 1; cOut = 'Z'; }

        /* Greek Characters ToUpperCase */
        if (c >= 'Α' && c <= 'Ω') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά') { change = 1; cOut = 'Α'; }
        if (c == 'β') { change = 1; cOut = 'Β'; }
        if (c == 'γ') { change = 1; cOut = 'Γ'; }
        if (c == 'δ') { change = 1; cOut = 'Δ'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ') { change = 1; cOut = 'Ε'; }
        if (c == 'ζ') { change = 1; cOut = 'Ζ'; }
        if (c == 'Ή' || c == 'η' || c == 'ή') { change = 1; cOut = 'Η'; }
        if (c == 'θ') { change = 1; cOut = 'Θ'; }
        if (c == 'Ί' || c == 'ι' || c == 'ί') { change = 1; cOut = 'Ι'; }
        if (c == 'Ϊ' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'Ϊ'; }
        if (c == 'κ') { change = 1; cOut = 'Κ'; }
        if (c == 'λ') { change = 1; cOut = 'Λ'; }
        if (c == 'μ') { change = 1; cOut = 'Μ'; }
        if (c == 'ν') { change = 1; cOut = 'Ν'; }
        if (c == 'ξ') { change = 1; cOut = 'Ξ'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό') { change = 1; cOut = 'Ο'; }
        if (c == 'π') { change = 1; cOut = 'Π'; }
        if (c == 'ρ') { change = 1; cOut = 'Ρ'; }
        if (c == 'σ' || c == 'ς') { change = 1; cOut = 'Σ'; }
        if (c == 'τ') { change = 1; cOut = 'Τ'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ') { change = 1; cOut = 'Υ'; }
        if (c == 'φ') { change = 1; cOut = 'Φ'; }
        if (c == 'χ') { change = 1; cOut = 'Χ'; }
        if (c == 'ψ') { change = 1; cOut = 'Ψ'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ') { change = 1; cOut = 'Ω'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.ToUpperForNames = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';

        /* Only space allowed from special characters */
        if (c == ' ') { cOut = c; }

        /* English Characters ToUpperCase */
        if (c >= 'A' && c <= 'Z') { cOut = c; }
        if (c == 'a') { change = 1; cOut = 'A'; }
        if (c == 'b') { change = 1; cOut = 'B'; }
        if (c == 'c') { change = 1; cOut = 'C'; }
        if (c == 'd') { change = 1; cOut = 'D'; }
        if (c == 'e') { change = 1; cOut = 'E'; }
        if (c == 'f') { change = 1; cOut = 'F'; }
        if (c == 'g') { change = 1; cOut = 'G'; }
        if (c == 'h') { change = 1; cOut = 'H'; }
        if (c == 'i') { change = 1; cOut = 'I'; }
        if (c == 'j') { change = 1; cOut = 'J'; }
        if (c == 'k') { change = 1; cOut = 'K'; }
        if (c == 'l') { change = 1; cOut = 'L'; }
        if (c == 'm') { change = 1; cOut = 'M'; }
        if (c == 'n') { change = 1; cOut = 'N'; }
        if (c == 'o') { change = 1; cOut = 'O'; }
        if (c == 'p') { change = 1; cOut = 'P'; }
        if (c == 'q') { change = 1; cOut = 'Q'; }
        if (c == 'r') { change = 1; cOut = 'R'; }
        if (c == 's') { change = 1; cOut = 'S'; }
        if (c == 't') { change = 1; cOut = 'T'; }
        if (c == 'u') { change = 1; cOut = 'U'; }
        if (c == 'v') { change = 1; cOut = 'V'; }
        if (c == 'w') { change = 1; cOut = 'W'; }
        if (c == 'x') { change = 1; cOut = 'X'; }
        if (c == 'y') { change = 1; cOut = 'Y'; }
        if (c == 'z') { change = 1; cOut = 'Z'; }

        /* Greek Characters ToUpperCase */
        if (c >= 'Α' && c <= 'Ω') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά') { change = 1; cOut = 'Α'; }
        if (c == 'β') { change = 1; cOut = 'Β'; }
        if (c == 'γ') { change = 1; cOut = 'Γ'; }
        if (c == 'δ') { change = 1; cOut = 'Δ'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ') { change = 1; cOut = 'Ε'; }
        if (c == 'ζ') { change = 1; cOut = 'Ζ'; }
        if (c == 'Ή' || c == 'η' || c == 'ή') { change = 1; cOut = 'Η'; }
        if (c == 'θ') { change = 1; cOut = 'Θ'; }
        if (c == 'Ί' || c == 'ι' || c == 'ί') { change = 1; cOut = 'Ι'; }
        if (c == 'Ϊ' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'Ϊ'; }
        if (c == 'κ') { change = 1; cOut = 'Κ'; }
        if (c == 'λ') { change = 1; cOut = 'Λ'; }
        if (c == 'μ') { change = 1; cOut = 'Μ'; }
        if (c == 'ν') { change = 1; cOut = 'Ν'; }
        if (c == 'ξ') { change = 1; cOut = 'Ξ'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό') { change = 1; cOut = 'Ο'; }
        if (c == 'π') { change = 1; cOut = 'Π'; }
        if (c == 'ρ') { change = 1; cOut = 'Ρ'; }
        if (c == 'σ' || c == 'ς') { change = 1; cOut = 'Σ'; }
        if (c == 'τ') { change = 1; cOut = 'Τ'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ') { change = 1; cOut = 'Υ'; }
        if (c == 'φ') { change = 1; cOut = 'Φ'; }
        if (c == 'χ') { change = 1; cOut = 'Χ'; }
        if (c == 'ψ') { change = 1; cOut = 'Ψ'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ') { change = 1; cOut = 'Ω'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.ToEnUpperForNames = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';
        if (c >= 'A' && c <= 'Z') { cOut = c; }
        if (c == ' ') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά' || c == 'a' || c == 'Α') { change = 1; cOut = 'A'; }
        if (c == 'β' || c == 'b' || c == 'Β') { change = 1; cOut = 'B'; }
        if (c == 'ψ' || c == 'c' || c == 'Ψ') { change = 1; cOut = 'C'; }
        if (c == 'δ' || c == 'd' || c == 'Δ') { change = 1; cOut = 'D'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ' || c == 'e' || c == 'Ε') { change = 1; cOut = 'E'; }
        if (c == 'φ' || c == 'f' || c == 'Φ') { change = 1; cOut = 'F'; }
        if (c == 'γ' || c == 'g' || c == 'Γ') { change = 1; cOut = 'G'; }
        if (c == 'Ή' || c == 'η' || c == 'ή' || c == 'h' || c == 'Η') { change = 1; cOut = 'H'; }
        if (c == 'Ί' || c == 'Ϊ' || c == 'ι' || c == 'ί' || c == 'ϊ' || c == 'ΐ' || c == 'i' || c == 'Ι') { change = 1; cOut = 'I'; }
        if (c == 'ξ' || c == 'j' || c == 'Ξ') { change = 1; cOut = 'J'; }
        if (c == 'κ' || c == 'k' || c == 'Κ') { change = 1; cOut = 'K'; }
        if (c == 'λ' || c == 'l' || c == 'Λ') { change = 1; cOut = 'L'; }
        if (c == 'μ' || c == 'm' || c == 'Μ') { change = 1; cOut = 'M'; }
        if (c == 'ν' || c == 'n' || c == 'Ν') { change = 1; cOut = 'N'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό' || c == 'o' || c == 'Ο') { change = 1; cOut = 'O'; }
        if (c == 'π' || c == 'p' || c == 'Π') { change = 1; cOut = 'P'; }
        if (c == 'q' || c == 'Q' || c == ';') { change = 1; cOut = 'Q'; }
        if (c == 'ρ' || c == 'r' || c == 'Ρ') { change = 1; cOut = 'R'; }
        if (c == 'σ' || c == 's' || c == 'Σ') { change = 1; cOut = 'S'; }
        if (c == 'τ' || c == 't' || c == 'Τ') { change = 1; cOut = 'T'; }
        if (c == 'θ' || c == 'u' || c == 'Θ') { change = 1; cOut = 'U'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ' || c == 'v' || c == 'Ω') { change = 1; cOut = 'V'; }
        if (c == 'ς' || c == 'w') { change = 1; cOut = 'W'; }
        if (c == 'χ' || c == 'x' || c == 'Χ') { change = 1; cOut = 'X'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ' || c == 'y' || c == 'Υ') { change = 1; cOut = 'Y'; }
        if (c == 'ζ' || c == 'z' || c == 'Ζ') { change = 1; cOut = 'Z'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.ToElUpperForNames = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';
        if (c >= 'Α' && c <= 'Ω') { cOut = c; }
        if (c == ' ') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά' || c == 'a' || c == 'A') { change = 1; cOut = 'Α'; }
        if (c == 'β' || c == 'b' || c == 'B') { change = 1; cOut = 'Β'; }
        if (c == 'γ' || c == 'g' || c == 'G') { change = 1; cOut = 'Γ'; }
        if (c == 'δ' || c == 'd' || c == 'D') { change = 1; cOut = 'Δ'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ' || c == 'e' || c == 'E') { change = 1; cOut = 'Ε'; }
        if (c == 'ζ' || c == 'z' || c == 'Z') { change = 1; cOut = 'Ζ'; }
        if (c == 'Ή' || c == 'η' || c == 'ή' || c == 'h' || c == 'H') { change = 1; cOut = 'Η'; }
        if (c == 'θ' || c == 'u' || c == 'U') { change = 1; cOut = 'Θ'; }
        if (c == 'Ί' || c == 'ι' || c == 'i' || c == 'I') { change = 1; cOut = 'Ι'; }
        if (c == 'Ϊ' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'Ϊ'; }
        if (c == 'κ' || c == 'k' || c == 'K') { change = 1; cOut = 'Κ'; }
        if (c == 'λ' || c == 'l' || c == 'L') { change = 1; cOut = 'Λ'; }
        if (c == 'μ' || c == 'm' || c == 'M') { change = 1; cOut = 'Μ'; }
        if (c == 'ν' || c == 'n' || c == 'N') { change = 1; cOut = 'Ν'; }
        if (c == 'ξ' || c == 'j' || c == 'J') { change = 1; cOut = 'Ξ'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό' || c == 'o' || c == 'O') { change = 1; cOut = 'Ο'; }
        if (c == 'π' || c == 'p' || c == 'P') { change = 1; cOut = 'Π'; }
        if (c == 'ρ' || c == 'r' || c == 'R') { change = 1; cOut = 'Ρ'; }
        if (c == 'σ' || c == 'ς' || c == 's' || c == 'S') { change = 1; cOut = 'Σ'; }
        if (c == 'τ' || c == 't' || c == 'T') { change = 1; cOut = 'Τ'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ' || c == 'y' || c == 'Y') { change = 1; cOut = 'Υ'; }
        if (c == 'φ' || c == 'f' || c == 'F') { change = 1; cOut = 'Φ'; }
        if (c == 'χ' || c == 'x' || c == 'X') { change = 1; cOut = 'Χ'; }
        if (c == 'ψ' || c == 'c' || c == 'C') { change = 1; cOut = 'Ψ'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ' || c == 'v' || c == 'V') { change = 1; cOut = 'Ω'; }
        if (c == 'ς' || c == 'w' || c == 'W') { change = 1; cOut = 'Σ'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.ToElUpper = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';
        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9') { change = 1; cOut = c; }
        if (c >= 'Α' && c <= 'Ω') { cOut = c; }
        if (c == ' ' || c == '-' || c == '_' || c == ',' || c == '.' || c == '(' || c == ')' || c == '/' || c == '&') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά' || c == 'a' || c == 'A') { change = 1; cOut = 'Α'; }
        if (c == 'β' || c == 'b' || c == 'B') { change = 1; cOut = 'Β'; }
        if (c == 'γ' || c == 'g' || c == 'G') { change = 1; cOut = 'Γ'; }
        if (c == 'δ' || c == 'd' || c == 'D') { change = 1; cOut = 'Δ'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ' || c == 'e' || c == 'E') { change = 1; cOut = 'Ε'; }
        if (c == 'ζ' || c == 'z' || c == 'Z') { change = 1; cOut = 'Ζ'; }
        if (c == 'Ή' || c == 'η' || c == 'ή' || c == 'h' || c == 'H') { change = 1; cOut = 'Η'; }
        if (c == 'θ' || c == 'u' || c == 'U') { change = 1; cOut = 'Θ'; }
        if (c == 'Ί' || c == 'ι' || c == 'i' || c == 'I') { change = 1; cOut = 'Ι'; }
        if (c == 'Ϊ' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'Ϊ'; }
        if (c == 'κ' || c == 'k' || c == 'K') { change = 1; cOut = 'Κ'; }
        if (c == 'λ' || c == 'l' || c == 'L') { change = 1; cOut = 'Λ'; }
        if (c == 'μ' || c == 'm' || c == 'M') { change = 1; cOut = 'Μ'; }
        if (c == 'ν' || c == 'n' || c == 'N') { change = 1; cOut = 'Ν'; }
        if (c == 'ξ' || c == 'j' || c == 'J') { change = 1; cOut = 'Ξ'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό' || c == 'o' || c == 'O') { change = 1; cOut = 'Ο'; }
        if (c == 'π' || c == 'p' || c == 'P') { change = 1; cOut = 'Π'; }
        if (c == 'ρ' || c == 'r' || c == 'R') { change = 1; cOut = 'Ρ'; }
        if (c == 'σ' || c == 'ς' || c == 's' || c == 'S') { change = 1; cOut = 'Σ'; }
        if (c == 'τ' || c == 't' || c == 'T') { change = 1; cOut = 'Τ'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ' || c == 'y' || c == 'Y') { change = 1; cOut = 'Υ'; }
        if (c == 'φ' || c == 'f' || c == 'F') { change = 1; cOut = 'Φ'; }
        if (c == 'χ' || c == 'x' || c == 'X') { change = 1; cOut = 'Χ'; }
        if (c == 'ψ' || c == 'c' || c == 'C') { change = 1; cOut = 'Ψ'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ' || c == 'v' || c == 'V') { change = 1; cOut = 'Ω'; }
        if (c == 'ς' || c == 'w' || c == 'W') { change = 1; cOut = 'Σ'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.ToEnUpper = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';
        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9') { change = 1; cOut = c; }
        if (c == ' ' || c == '-' || c == '_' || c == ',' || c == '.' || c == '(' || c == ')' || c == '/' || c == '&') { cOut = c; }
        if (c >= 'A' && c <= 'Z') { cOut = c; }
        if (c == 'Ά' || c == 'α' || c == 'ά' || c == 'a' || c == 'Α') { change = 1; cOut = 'A'; }
        if (c == 'β' || c == 'b' || c == 'Β') { change = 1; cOut = 'B'; }
        if (c == 'ψ' || c == 'c' || c == 'Ψ') { change = 1; cOut = 'C'; }
        if (c == 'δ' || c == 'd' || c == 'Δ') { change = 1; cOut = 'D'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ' || c == 'e' || c == 'Ε') { change = 1; cOut = 'E'; }
        if (c == 'φ' || c == 'f' || c == 'Φ') { change = 1; cOut = 'F'; }
        if (c == 'γ' || c == 'g' || c == 'Γ') { change = 1; cOut = 'G'; }
        if (c == 'Ή' || c == 'η' || c == 'ή' || c == 'h' || c == 'Η') { change = 1; cOut = 'H'; }
        if (c == 'Ί' || c == 'Ϊ' || c == 'ι' || c == 'ί' || c == 'ϊ' || c == 'ΐ' || c == 'i' || c == 'Ι') { change = 1; cOut = 'I'; }
        if (c == 'ξ' || c == 'j' || c == 'Ξ') { change = 1; cOut = 'J'; }
        if (c == 'κ' || c == 'k' || c == 'Κ') { change = 1; cOut = 'K'; }
        if (c == 'λ' || c == 'l' || c == 'Λ') { change = 1; cOut = 'L'; }
        if (c == 'μ' || c == 'm' || c == 'Μ') { change = 1; cOut = 'M'; }
        if (c == 'ν' || c == 'n' || c == 'Ν') { change = 1; cOut = 'N'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό' || c == 'o' || c == 'Ο') { change = 1; cOut = 'O'; }
        if (c == 'π' || c == 'p' || c == 'Π') { change = 1; cOut = 'P'; }
        if (c == 'q' || c == 'Q' || c == ';') { change = 1; cOut = 'Q'; }
        if (c == 'ρ' || c == 'r' || c == 'Ρ') { change = 1; cOut = 'R'; }
        if (c == 'σ' || c == 's' || c == 'Σ') { change = 1; cOut = 'S'; }
        if (c == 'τ' || c == 't' || c == 'Τ') { change = 1; cOut = 'T'; }
        if (c == 'θ' || c == 'u' || c == 'Θ') { change = 1; cOut = 'U'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ' || c == 'v' || c == 'Ω') { change = 1; cOut = 'V'; }
        if (c == 'ς' || c == 'w') { change = 1; cOut = 'W'; }
        if (c == 'χ' || c == 'x' || c == 'Χ') { change = 1; cOut = 'X'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ' || c == 'y' || c == 'Υ') { change = 1; cOut = 'Y'; }
        if (c == 'ζ' || c == 'z' || c == 'Ζ') { change = 1; cOut = 'Z'; }
        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl) {
            s.SetText(StrOut);
            s.ValueChanged.FireEvent(s, null);
        }
        else
            s.value = StrOut;
    }
    return;
}

Imis.Lib.NoGreekCharacters = function (elem, upperCase) {
    var change = 0;
    var Str = elem.value;
    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';

        if ((c >= 'Α' && c <= 'Ω') || (c >= 'α' && c <= 'ω') ||
             c == 'Ά' || c == 'ά' ||
             c == 'Έ' || c == 'ε' || c == 'έ' ||
             c == 'Ί' || c == 'Ϊ' || c == 'ι' || c == 'ί' || c == 'ϊ' || c == 'ΐ' ||
             c == 'Ό' || c == 'ο' || c == 'ό' || c == 'Ο' ||
             c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ' ||
             c == 'Ώ' || c == 'ω' || c == 'ώ' ||
             c == ' ') {
            showAlertBox('Επιτρέπονται μόνο λατινικοί χαρακτήρες χωρίς κενά!');
            elem.value = StrOut;
            return;
        } else {
            change = 1;
            if (upperCase) {
                cOut = c.toUpperCase();
            } else {
                cOut = c;
            }
        }

        StrOut = StrOut + cOut;
    };

    if (change == 1) elem.value = StrOut;

    return;
}

Imis.Lib.StudentNumberTransformation = function (s, e) {
    var change = 0;
    var Str;

    if (s.isASPxClientControl)
        Str = s.GetText();
    else
        Str = s.value;

    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = '';
        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9') { cOut = c; }
        if (c >= 'Α' && c <= 'Ω') { cOut = c; }

        /* Greek Characters To Uppercase */
        if (c == 'Ά' || c == 'α' || c == 'ά') { change = 1; cOut = 'Α'; }
        if (c == 'β') { change = 1; cOut = 'Β'; }
        if (c == 'γ') { change = 1; cOut = 'Γ'; }
        if (c == 'δ') { change = 1; cOut = 'Δ'; }
        if (c == 'Έ' || c == 'ε' || c == 'έ') { change = 1; cOut = 'Ε'; }
        if (c == 'ζ') { change = 1; cOut = 'Ζ'; }
        if (c == 'Ή' || c == 'η' || c == 'ή') { change = 1; cOut = 'Η'; }
        if (c == 'θ') { change = 1; cOut = 'Θ'; }
        if (c == 'Ί' || c == 'Ϊ' || c == 'ι' || c == 'ί' || c == 'ϊ' || c == 'ΐ') { change = 1; cOut = 'Ι'; }
        if (c == 'κ') { change = 1; cOut = 'Κ'; }
        if (c == 'λ') { change = 1; cOut = 'Λ'; }
        if (c == 'μ') { change = 1; cOut = 'Μ'; }
        if (c == 'ν') { change = 1; cOut = 'Ν'; }
        if (c == 'ξ') { change = 1; cOut = 'Ξ'; }
        if (c == 'Ό' || c == 'ο' || c == 'ό') { change = 1; cOut = 'Ο'; }
        if (c == 'π') { change = 1; cOut = 'Π'; }
        if (c == 'ρ') { change = 1; cOut = 'Ρ'; }
        if (c == 'σ' || c == 'ς') { change = 1; cOut = 'Σ'; }
        if (c == 'τ') { change = 1; cOut = 'Τ'; }
        if (c == 'Ύ' || c == 'Ϋ' || c == 'υ' || c == 'ύ' || c == 'ϋ' || c == 'ΰ') { change = 1; cOut = 'Υ'; }
        if (c == 'φ') { change = 1; cOut = 'Φ'; }
        if (c == 'χ') { change = 1; cOut = 'Χ'; }
        if (c == 'ψ') { change = 1; cOut = 'Ψ'; }
        if (c == 'Ώ' || c == 'ω' || c == 'ώ') { change = 1; cOut = 'Ω'; }

        /* English Characters ToUpperCase */
        if (c == 'C' || c == 'D' || c == 'F' || c == 'G' || c == 'J' || c == 'L' ||
            c == 'Q' || c == 'R' || c == 'S' || c == 'U' || c == 'V' || c == 'W') { cOut = c; }
        if (c == 'a' || c == 'A') { change = 1; cOut = 'Α'; }
        if (c == 'b' || c == 'B') { change = 1; cOut = 'Β'; }
        if (c == 'c') { change = 1; cOut = 'C'; }
        if (c == 'd') { change = 1; cOut = 'D'; }
        if (c == 'e' || c == 'E') { change = 1; cOut = 'Ε'; }
        if (c == 'f') { change = 1; cOut = 'F'; }
        if (c == 'g') { change = 1; cOut = 'G'; }
        if (c == 'h' || c == 'H') { change = 1; cOut = 'Η'; }
        if (c == 'i' || c == 'I') { change = 1; cOut = 'Ι'; }
        if (c == 'j') { change = 1; cOut = 'J'; }
        if (c == 'k' || c == 'K') { change = 1; cOut = 'Κ'; }
        if (c == 'l') { change = 1; cOut = 'L'; }
        if (c == 'm' || c == 'M') { change = 1; cOut = 'Μ'; }
        if (c == 'n' || c == 'N') { change = 1; cOut = 'Ν'; }
        if (c == 'o' || c == 'O') { change = 1; cOut = 'Ο'; }
        if (c == 'p' || c == 'P') { change = 1; cOut = 'Ρ'; }
        if (c == 'q') { change = 1; cOut = 'Q'; }
        if (c == 'r') { change = 1; cOut = 'R'; }
        if (c == 's') { change = 1; cOut = 'S'; }
        if (c == 't' || c == 'T') { change = 1; cOut = 'Τ'; }
        if (c == 'u') { change = 1; cOut = 'U'; }
        if (c == 'v') { change = 1; cOut = 'V'; }
        if (c == 'w') { change = 1; cOut = 'W'; }
        if (c == 'x' || c == 'X') { change = 1; cOut = 'Χ'; }
        if (c == 'y' || c == 'Y') { change = 1; cOut = 'Υ'; }
        if (c == 'z' || c == 'Z') { change = 1; cOut = 'Ζ'; }

        if (cOut == '') { change = 1; }

        StrOut = StrOut + cOut;
    };

    if (change == 1) {
        if (s.isASPxClientControl)
            s.SetText(StrOut);
        else
            s.value = StrOut;
    }
    return;
}

function RemoveTags(Obj) {
    var Str = Obj.value;
    var change = 0;
    var StrL = Str.length;
    var StrOut = "";

    var c = ' ';
    var cOut = ' ';

    for (var i = 0; i < StrL; i++) {
        c = Str.substring(i, i + 1);
        cOut = c;

        if (c == '<' || c == '>') { change = 1; cOut = ''; }

        StrOut = StrOut + cOut;
    };

    if (change == 1)
        Obj.value = StrOut;

    return;
}

Imis.Lib.EnterHandler = function (e, callback) {
    e = e || window.event || {};
    var charCode = e.keyCode || e.charCode || 0;
    if (Sys.UI.Key.enter == charCode) {
        callback();
        if (e.stopPropagation) {
            e.stopPropagation();
        } else {
            e.cancelBubble = true;
        }
    }
}

Imis.Lib.WindowOpen = function (url, features) {
    if (!features) features = {};
    if (!features.width) features.width = 500;
    if (!features.height) features.height = 500;
    if (!features.name) features.name = 'ImisLibPopup';
    if (!features.openInParent) features.openInParent = false;
    if (features.openInParent) {
        if (opener != null) {
            opener.location = url;
            if (opener.location.toString().indexOf('#&&', 0) != -1)
                opener.location.reload();
        }
        else {
            window.location = url;
        }
    }
    else {
        var w = window.open(url, features.name, "height=" + features.height + ",width=" + features.width + ",status=yes,resizable=yes,toolbars=no,location=no,scrollbars=yes");
        w.focus();
    }
}

Imis.Lib.notify = function (notification) {
    $("<div id='notification-container'>" + notification + "</div>")
        .appendTo(document.body)
        .slideDown()
        .delay(3000)
        .slideUp();
}

Imis.Lib.IsAfmValid = function (afm) {
    if (!afm.match(/^\d{9}$/) || afm == '000000000') {
        return false;
    }

    afm = afm.split('').reverse().join('');

    var Num1 = 0;
    for (var iDigit = 1; iDigit <= 8; iDigit++) {
        Num1 += afm.charAt(iDigit) << iDigit;
    }
    return (Num1 % 11) % 10 == afm.charAt(0);
}

Imis.Lib.CheckAfm = function (val, e) {
    if (!e || !e.value || e.value == null) return;
    if (e.value) {
        var afm = e.value;
        e.isValid = false;
        if (!afm.match(/^\d{9}$/) || afm == '000000000') {
            e.isValid = false;
            return;
        }
        afm = afm.split('').reverse().join('');

        var Num1 = 0;
        for (var iDigit = 1; iDigit <= 8; iDigit++) {
            Num1 += afm.charAt(iDigit) << iDigit;
        }
        e.isValid = (Num1 % 11) % 10 == afm.charAt(0);
    }
}

Imis.Lib.OnlyCharacters = function (s, e) {
    var noCharacters = new RegExp('[^a-zA-zα-ωΑ-Ω\\s]', 'g');
    var input = new String();
    if (s.GetText)
        input = s.GetText();
    else
        input = s.value;
    input = input.replace(noCharacters, '');
    if (s.SetText)
        s.SetText(input);
    else
        s.value = input;
    if (s.Validate) {
        s.Validate();
    }
}

Imis.Lib.OnlyDigits = function (s, e) {
    var noDigits = new RegExp('\\D', 'g');
    var input = new String();
    if (s.GetText)
        input = s.GetText();
    else
        input = s.value;
    input = input.replace(noDigits, '');
    if (s.SetText)
        s.SetText(input);
    else
        s.value = input;
    if (s.Validate) {
        s.Validate();
    }
}

Imis.Lib.OnlyISBN = function (s, e) {
    var noDigitsOrDash = new RegExp('[\\d\\-}]', 'g');
    var input = new String();
    if (s.GetText)
        input = s.GetText();
    else
        input = s.value;
    input = input.replace(noDigitsOrDash, '');
    input = input.replace(/\-/g, '');
    if (s.SetText)
        s.SetText(input);
    else
        s.value = input;
    if (s.Validate) {
        s.Validate();
    }
}

Imis.Lib.CheckUserName = function (s, e) {
    if (e.value == null)
        return;

    e.isValid = Imis.Lib.IsUserNameValid(e.value);
}

Imis.Lib.IsUserNameValid = function (userName) {
    var regexp = /^([A-Za-z0-9_\-\.]){5,}$/;
    return regexp.test(userName);
}

Imis.Lib.CheckPassword = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckEmail = function (s, e) {
    if (e.value == null)
        return;

    e.isValid = Imis.Lib.IsEmailValid(e.value);
}

Imis.Lib.IsEmailValid = function (email) {
    var regexp = /^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_-])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$/;
    return regexp.test(email);
}

Imis.Lib.CheckUri = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckFixedPhone = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^2\d{9}$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckMobilePhone = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^69\d{8}$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckPhone = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^2\d{9}|69\d{8}$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckZipCode = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^\d{5}$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckISBN = function (s, e) {
    if (e.value == null)
        return;

    var regexp = /^(\d+-?)+\d+$/;
    e.isValid = regexp.test(e.value);
}

Imis.Lib.CheckCheckbox = function (s, e) {
    var cb = document.getElementById(s.checkboxToValidate);
    e.IsValid = cb.checked;
    ValidatorUpdateDisplay(s);
}

Imis.Lib.DisableGreekKeyPress = function (e) {
    e = e || window.event || {};
    var charCode = e.keyCode || e.charCode || 0;
    //it's 902 actually but just in case we miss something we make it 900
    var isValid = charCode < 900;
    if (!isValid) {
        var txt = "Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες.";
        var dialog = $('<div>' + txt + '</div>')
          .dialog({
              modal: true,
              resizable: false,
              draggable: true,
              show: 'fade',
              hide: 'fade',
              dialogClass: 'main-dialog-class',
              buttons: {
                  "ΟΚ": function () {
                      dialog.dialog('close');
                  }
              },
              open: function () {
                  $(this).parent().find('.ui-dialog-buttonpane button:first-child').button({
                      icons: { primary: 'bg-accept' }
                  });
              }
          });
        //$.prompt('Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες.');
    }
    return isValid;
}

Imis.Lib.ValidateDateTime = function (s, e) {

    var inputDate = Date.parseLocale(e.Value.substr(0, e.Value.length - 6));
    var x = e.Value.substr(e.Value.length - 5, e.Value.length - 3);
    inputDate.setHours(x.split(/:/)[0]);
    inputDate.setMinutes(x.split(/:/)[1]);
    if (inputDate > new Date()) {
        e.IsValid = false;
    }
}

Imis.Lib.ValidateGroup = function (vg) {
    var isValid = true;
    isValid = isValid && ASPxClientEdit.ValidateGroup(vg);
    for (var i = 0; i < Page_Validators.length; i++) {
        if (Page_Validators[i].validationGroup == vg) {
            window.ValidatorValidate(Page_Validators[i]);
            isValid = isValid && Page_Validators[i].isvalid;
        }
    }
    return isValid;
}

Imis.Lib.GetDxValidationErrors = function (validationGroup, processInvisibleEditors) {
    var errors = [];
    var container = null;
    if (typeof (_aspxValidateProcessingProc) == 'undefined')
        return errors;
    var processingProc = _aspxValidateProcessingProc;
    var collection = ASPxClientControl.GetControlCollection();
    var choiceCondition = _aspxValidateChoiceCondition;
    for (var key in collection.elements) {
        var control = collection.elements[key];
        if (control != null && ASPxIdent.IsASPxClientEdit(control)) {
            var mainElement = control.GetMainElement();
            if (_aspxIsExists(mainElement) && (container == null || _aspxGetIsParent(container, mainElement))
                    && (processInvisibleEditors || control.IsVisible()) && choiceCondition(control, validationGroup)) {
                var isSuccess = processingProc(control);
                if (!isSuccess) {
                    errors.push({ validationGroup: control.validationGroup, message: control.GetErrorText(), control: control });
                }
            }
        }
    }
    return errors;
}

//Imis.Lib.GetDxValidationErrors = function (validationGroup, processInvisibleEditors) {
//    var errors = [];
//    var container = null;
//    if (typeof (_aspxValidateProcessingProc) == 'undefined')
//        return errors;
//    var processingProc = _aspxValidateProcessingProc;
//    var collection = ASPxClientControl.GetControlCollection();
//    var choiceCondition = _aspxValidateChoiceCondition;
//    for (var key in collection.elements) {
//        var control = collection.elements[key];
//        if (control != null && ASPxIdent.IsASPxClientEdit(control)) {
//            var mainElement = control.GetMainElement();
//            if (_aspxIsExists(mainElement) && (container == null || _aspxGetIsParent(container, mainElement))
//                    && (processInvisibleEditors || control.IsVisible()) && choiceCondition(control, validationGroup)) {
//                var isSuccess = processingProc(control);
//                if (!isSuccess) {
//                    errors.push({ validationGroup: control.validationGroup, message: control.GetErrorText(), control: control });
//                }
//            }
//        }
//    }
//    return errors;
//}

Imis.Lib.GetValidationErrors = function (validationGroup, validate, arrayErrors) {
    var errors = [];

    var DxErrors = Imis.Lib.GetDxValidationErrors(validationGroup, true);
    if (DxErrors.length > 0) {
        for (var i = 0; i < DxErrors.length; i++) {
            errors.push(DxErrors[i]);
        }
    }

    if (arrayErrors.length > 0) {
        for (var i = 0; i < arrayErrors.length; i++) {
            errors.push(arrayErrors[i]);
        }
    }
    if (validate) {
        var val = new Imis.Lib.Validation();
        val.Page_ClientValidate(validationGroup);
    }

    var summary, sums;
    var i;
    if (typeof (Page_Validators) != "undefined") {
        for (i = 0; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid && typeof (Page_Validators[i].errormessage) == "string") {
                errors.push({
                    validationGroup: Page_Validators[i].validationGroup,
                    message: Page_Validators[i].errormessage,
                    element: Page_Validators[i].controltovalidate
                });
            }
        }
    }
    return errors;
}

Imis.Lib.CreateErrorList = function (validationGroup, elemID, validate) {
    if (!elemID)
        elemID = 'error-area';
    var errors = [];

    var div = $('#' + elemID);
    if (div.length == 0) {
        div = $($.create('div', { id: elemID }, []));
        $(document.forms[0]).append(div);
    }
    errors = Imis.Lib.GetValidationErrors(validationGroup, validate, pageErrors);
    div.html('');
    if (errors.length == 0)
        return;
    div.hide();

    var ul = $($.create('ul', {}, []));
    ul.attr("style", "list-style-type: square;")

    for (var i = 0; i < errors.length; i++) {
        var o = $.create('li',
                        {}, [errors[i].message]);
        ul.append($(o));
    }
    div.append($(ul));
    div.show();
    //    div.animate({ backgroundColor: 'pink' }, 500)
    //    .animate({ backgroundColor: 'white' }, 500);
}

Imis.Lib.BuildImgButton = function (rel, bgClass, title) {
    var builder = ["<a href='javascript:void(0);'"];
    if (rel)
        builder.push(" rel='", rel, "'");
    builder.push(" class='img-btn");
    if (bgClass)
        builder.push(" ", bgClass, "'");
    if (title)
        builder.push(" title='", title, "'");
    builder.push("><img src='/_img/s.gif' style='height:16px;width:16px;border:none;' /></a>");
    return builder.join('');
}
$imgbtn = Imis.Lib.BuildImgButton;

Imis.Lib.IsUndefined = function (arg) {
    return typeof (arg) === 'undefined';
}
$undefined = Imis.Lib.IsUndefined;

Imis.Lib.ClearErrorList = function (elemID) {
    if (!elemID)
        elemID = 'error-area';
    $('#' + elemID).html('');
}

Imis.Lib.OpenPopup = function (url, width, height, name) {
    if (!width) width = '400';
    if (!height) height = '400';
    if (!name) name = 'popupWindow';
    window.open(url, name, 'height=' + height + ',width=' + width + ',status=no,toolbars=no,location=no');
}

Imis.Lib.HideButton = function (button) {
    if (typeof (button) !== 'undefined') {
        button.SetVisible(false);
    }
}

Imis.Lib.SetLatin = function (elem) {
    elem.onkeyup = null;
    $clearHandlers(elem);
    $addHandler(elem, 'keyup', function () { Imis.Lib.NoGreekCharacters(elem, true); });
}

Imis.Lib.ChangeLang = function (elem, txtID) {
    var ddl = $(elem);
    var txt = $get(txtID);
    if (ddl.val() == 'el-GR') {
        Imis.Lib.SetGreek(txt);
    }
    else if (ddl.val() == 'en-US') {
        Imis.Lib.SetLatin(txt);
    }
}

Imis.Lib.Validate = function (group) {
    return ASPxClientEdit.ValidateGroup(group);
}

Imis.Lib.Validation = function () {
};

Imis.Lib.Validation.prototype = {
    ValidatorValidate: function (val, validationGroup, event) {
        val.isvalid = true;
        if ((typeof (val.enabled) == "undefined" || val.enabled != false) && IsValidationGroupMatch(val, validationGroup)) {
            if (typeof (val.evaluationfunction) == "function") {
                val.isvalid = val.evaluationfunction(val);
                if (!val.isvalid) { ValidatorUpdateDisplay(val); }
            }
        }
    },
    Page_ClientValidate: function (validationGroup) {
        Page_InvalidControlToBeFocused = null;
        if (typeof (Page_Validators) == "undefined") {
            return true;
        }
        Page_InvalidControlToBeFocused = null;

        var i;
        for (i = 0; i < Page_Validators.length; i++) {
            this.ValidatorValidate(Page_Validators[i], validationGroup, null);
        }
        ValidationSummaryOnSubmit(validationGroup);
        Page_BlockSubmit = !Page_IsValid;
        return Page_IsValid;
    }
};


Imis.Lib.TabErrorHandler = function () {
    this._mainGroups = null;
    this._errors = null;
    this._proposalFormManager = null;
    this._proposalTabCtrl = null;
};

Imis.Lib.TabErrorHandler.prototype = {
    init: function (proposalFormManager, proposalTabCtrl) {
        this._proposalFormManager = proposalFormManager;
        this._proposalTabCtrl = proposalTabCtrl;
    },
    showWaitMessage: function () {
        $('#wait_message').show();
    },
    hideWaitMessage: function () {
        $('#wait_message').hide();
    },
    createList: function (errors) {
        this._errors = errors;
        this.clearList();
        var valGroups = this._proposalFormManager.get_Groups();
        this._mainGroups = {};
        var nestedGroups = {};

        var __realErrors = {};
        for (var i in this._errors) {
            if (this._errors[i] != null)
                __realErrors[i] = 'val';
        }

        for (var i in valGroups) {
            this._mainGroups[valGroups[i][0]] = { name: valGroups[i][0], text: valGroups[i][1] };
            if (valGroups[i][2] == null || valGroups[i][2].length < 1) {
                nestedGroups[valGroups[i][0]] = { name: valGroups[i][0], text: valGroups[i][1], parent: null };
            }
            else {
                for (var j in valGroups[i][2]) {
                    nestedGroups[valGroups[i][2][j][0]] = { name: valGroups[i][2][j][0], text: valGroups[i][2][j][1], parent: valGroups[i][0] };
                }
            }
        }
        for (var i in nestedGroups) {
            if (__realErrors[nestedGroups[i].parent] == 'val' || __realErrors[nestedGroups[i].name] == 'val') {
                if (nestedGroups[i].parent != null)
                    this.createNestedDiv(nestedGroups[i].name, nestedGroups[i].parent, nestedGroups[i].text, this._mainGroups[nestedGroups[i].parent].text);
                else
                    this.createDiv(nestedGroups[i].name, false, nestedGroups[i].parent, nestedGroups[i].text);
            }
        }
        for (var valGroup in this._errors) {
            for (var error in this._errors[valGroup]) {
                this.createElement(valGroup, this._errors[valGroup][error].message);
            }
        }
        return nestedGroups;
    },

    clearList: function clearList() {
        $('#errors_1').remove();
        var errdiv = $($.create('div', { id: 'errors_1' }, []));
        $('#errors_2').append(errdiv);
    },

    createElement: function (valGroup, msg) {
        var ul = $('#err_ul_' + valGroup);
        var li = $($.create('li', {}, [msg]));
        ul.append(li);
    },

    createNestedDiv: function (valGroup, parentGroup, text, parentText) {
        if (!text) text = valGroup;
        var parentDiv = this.createDiv(parentGroup, true, undefined, parentText);
        var nestedDiv = this.createDiv(valGroup, false, parentGroup, text);
        parentDiv.append(nestedDiv);
    },

    createDiv: function (valGroup, hasNested, parentName, text) {

        var div = $('#err_' + valGroup);
        if (div.length == 0) {
            if (!text) text = valGroup;
            //Δεν υπαρχει
            var errdiv = $('#errors_1');
            var ul = $('#err_ul' + valGroup);
            div = $($.create('div', { id: 'err_' + valGroup }, []));

            var title = $($.create('a', { href: 'javascript:void(0);', 'class': 'val-group' }, [text]));
            div.append(title);
            if (!hasNested) {
                ul = $($.create('ul', { id: 'err_ul_' + valGroup }, []));
                div.append(ul);
            }
            if (!parentName) {
                title.click(Function.createDelegate(this, function () {
                    tabValidate = false;
                    this._proposalTabCtrl.SetActiveTab(this._proposalTabCtrl.GetTabByName(valGroup));
                    tabValidate = true;
                })
                );
                div.addClass('main-error-div');
                title.addClass('nested');
            }
            else {
                title.click(Function.createDelegate(this, function () {
                    tabValidate = false;
                    this._proposalTabCtrl.SetActiveTab(this._proposalTabCtrl.GetTabByName(parentName));
                    var tabCtrl = window.parent[parentName + 'TabCtrl'];
                    tabCtrl.SetActiveTab(tabCtrl.GetTabByName(valGroup));
                    tabValidate = true;
                })
                );
            }
            errdiv.append(div);
        }
        return div;
    }
};

var tabErrorHandler = new Imis.Lib.TabErrorHandler();
var tabValidate = true;

var dx_cbRequired = function (s, e) {
    if (s.GetSelectedItem() == null || s.GetValue() == '') { e.isValid = false; }
};

var dx_Focus = function (s, e) {
    // var ss = ASPxClientTextBox.Cast(s);
    var el = s.GetMainElement();
    el.className += ' dx-focused';

    if ($find) {
        var popup = $find(el.id + '_popup');
        if (popup)
            popup.showPopup();
    }
};

var dx_Blur = function (s, e) {
    var el = s.GetMainElement();
    el.className = el.className.replace(' dx-focused', '');
    s.Validate();

    if ($find) {
        var popup = $find(el.id + '_popup');
        if (popup)
            popup.hidePopup();
    }
};

/**
* Cookie plugin
* Copyright (c) 2006 Klaus Hartl (stilbuero.de)
*/
jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }

        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

Imis.Lib.DisableCompression = function () {
    $.cookie('disable-compression', '1', { expires: 0.00015 });
}

function showErrorMessage(msg) {
    $('#errorPanel span').text(msg);
    //var btm = ($('#errorPanel').position().top - ).toString();
    $('#errorPanel').css('top', $('body').height() - 140);
    $('#errorPanel').slideDown("slow", function () { hideErrorPanel(9000); });
}

function hideErrorPanel(delay) {
    if (delay) {
        errorPanelTimeOut = setTimeout(function () { $('#errorPanel').fadeOut('slow'); }, delay);
    } else {
        if (errorPanelTimeOut != null)
            window.clearTimeout(errorPanelTimeOut);
        $('#errorPanel').fadeOut('slow');
    }
}

function preventPageLeave(warningMessage, canLeaveCallback) {
    window.onbeforeunload = function () {
        if (!canLeaveCallback()) {
            //return confirmPageLeave(warningMessage);
            return warningMessage;
        }
    }
}

function confirmPageLeave(warningMessage, onOk, onCancel) {
    if (/Firefox[\/\s](\d+)/.test(navigator.userAgent) && new Number(RegExp.$1) >= 4) {
        if (confirm(warningMessage)) {
            if (typeof (onOk) === 'function')
                onOk();
            else
                history.go();
        } else {
            if (typeof (onCancel) === 'function')
                onCancel();
            else {
                window.setTimeout(function () {
                    window.stop();
                }, 1);
            }
        }
    }
    else {
        return warningMessage;
    }
}

String.prototype.templateReplace = function (fieldName, fieldValue) {
    return this.replace(new RegExp("{" + fieldName + "}", "g"), fieldValue);
}

String.prototype.templateApply = function (templateValues) {
    var retVal = this;
    for (var fieldName in templateValues)
        retVal = retVal.templateReplace(fieldName, templateValues[fieldName]);
    return retVal;
}
