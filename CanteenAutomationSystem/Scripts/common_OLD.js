//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [1]Function cant return function name itself
//    -will cause function only run once
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

// AMOUNT  VALIDATION
function OnTextNumericKeyPress(evt) {
    var theEvent = evt || window.event;
    var rv = true;
    //var key = theEvent.keyCode || theEvent.which;
    var key = (typeof theEvent.which === 'undefined') ? theEvent.keyCode : theEvent.which;
    if (key && (key !== 8)) {
        var keychar = String.fromCharCode(key);
        var keycheck = /[,.0-9]/;
        if (!keycheck.test(keychar) || (keychar == '.' && evt.OnTextNumericKeyPress.value.indexOf('.') > -1)) {
            rv = theEvent.returnValue = false; //for IE
            if (theEvent.preventDefault) theEvent.preventDefault(); //Firefox
        }
    }
    return rv;
}

// TEXT VALIDATION WITH SYMBOL
function fValidate(evt) {
    var theEvent = evt || window.event;
    var rv = true;

    var key = (typeof theEvent.which === 'undefined') ? theEvent.keyCode : theEvent.which;

    if (key && (key !== 8)) {
        var keychar = String.fromCharCode(key);
        var keycheck = /[a-zA-Z0-9]/;
        // !~`@$*()-_.      KEY SEQUENCE
        if (!keycheck.test(keychar) && !(key == 32 || key == 33 || key == 126 || key == 96 || key == 64 || key == 36 || key == 42 || key == 40 || key == 41 || key == 45 || key == 95 || key == 46)) {
            rv = theEvent.returnValue = false; //for IE
            if (theEvent.preventDefault) theEvent.preventDefault(); //Firefox
        }
    }
    return rv;
}


// AMOUNT FORMATING SCRIPT [,] [.]    //Pass in Inputtext itself
function RoundtoDecimal_onPrs(ctrl, dpoint) {

    if (ctrl.value.length > 0) {
        //REMOVE [,]
        ctrl.value = ctrl.value.split(",").join("");

        var num = parseFloat(ctrl.value);
        ctrl.value = num.toFixed(dpoint).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');

        if (ctrl.value == "NaN") {
            var num = parseFloat(0);
            ctrl.value = num.toFixed(dpoint).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
        }

        var number = ctrl.value.substring(0, ctrl.value.indexOf('.'));
        var decimalpoint = ctrl.value.substring(ctrl.value.indexOf('.'), ctrl.value.length);

        decimalpoint = decimalpoint.replace(",", "");
        ctrl.value = number + decimalpoint;
    }
}

// AMOUNT FORMATING SCRIPT [,] [.] // Only pass in value
function RoundtoDecimal_pgLoad(ctrl, dpoint) {
    //REMOVE [,]
    ctrl = String(ctrl).split(",").join("");

    var num = parseFloat(ctrl);
    ctrl = num.toFixed(dpoint).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');

    var number = ctrl.substring(0, ctrl.indexOf('.'));
    var decimalpoint = ctrl.substring(ctrl.indexOf('.'), ctrl.length);

    decimalpoint = decimalpoint.replace(",", "");
    return ctrl = number + decimalpoint;
}

// DATE  VALIDATION SCRIPT [/]
function isDateKey(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 47 || charCode == 8))
        return false;
    else {
        var len = $(element).val().length;
        var index = $(element).val().indexOf('/');
        if (index > 0 && charCode == 47) {
            return false;
        }
    }
    return true;
}

function fCHK(value) {
    var result = false;
    if (value == null || value == "") {
        result = true;
    }
    return result;
}

var matches = document.querySelectorAll("input[type=text]");
//['] VALIDATION SCRIPT
$(document).ready(function () {
    $(matches).keydown(function (e) {
        // Ensure that it is ['] stop the keypress
        if (e.keyCode == 222) {
            return false;
        }
    });
});

$(document).ready(function () {
    $("input:text").focus(function () { $(this).select(); });
});