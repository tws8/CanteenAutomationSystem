/// <reference path="variable.js" />
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [1]Function cant return function name itself
//    -will cause function only run once
// [2] Function must return, else only work once
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//Version: v2021.11.03
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

$(document).ready(function () {
    $('input[type="text"]:not([readonly]):first').focus();
    $("input:text").focus(function () { $(this).select(); });
});


function VwDate(evt) {
    $('#' + evt).datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999);
            }, 0);
        }
    });
    $('#' + evt).addClass('DtPickerFormat');
    $('#' + evt).datepicker("show")
        .on('changeDate', function (e) {
            // Revalidate the date field
            $('#' + evt).val(this.value);
        });
}

//#region Key press Validation
function fValidatePrice(evt) {
    var theEvent = evt || window.event;
    var rv = true;
    var key = (typeof theEvent.which === 'undefined') ? theEvent.keyCode : theEvent.which;

    if (key && !(key == 8)) {
        var keychar = String.fromCharCode(key);
        var keycheck = /[,.0-9]/;
        if (!keycheck.test(keychar)) {
            rv = theEvent.returnValue = false; //for IE
            if (theEvent.preventDefault) theEvent.preventDefault(); //Firefox
        }
    }
    return rv;
}              // AMOUNT VALIDATION
function fValidatePriceNg(evt) {
    var theEvent = evt || window.event;
    var rv = true;
    var key = (typeof theEvent.which === 'undefined') ? theEvent.keyCode : theEvent.which;

    if (key && !(key == 8 || key == 45)) {
        var keychar = String.fromCharCode(key);
        var keycheck = /[,.0-9]/;
        if (!keycheck.test(keychar)) {
            rv = theEvent.returnValue = false; //for IE
            if (theEvent.preventDefault) theEvent.preventDefault(); //Firefox
        }
    }
    return rv;
}            // AMOUNT VALIDATION Nagetive
function fValidate(evt) {
    var theEvent = evt || window.event;
    var rv = true;
    var key = (typeof theEvent.which === 'undefined') ? theEvent.keyCode : theEvent.which;
    //console.log("key " + key);
    if (key && (key !== 8)) {
        var keychar = String.fromCharCode(key);
        var keycheck = /[a-zA-Z0-9]/;
        // !~`@$*()-_./#+      KEY SEQUENCE
        if (!keycheck.test(keychar) && !(key == 32 || key == 33 || key == 126 || key == 96 || key == 64 || key == 36 || key == 42 || key == 40 || key == 41 || key == 45 || key == 95 || key == 46 || key == 47 || key == 35 || key == 43)) {
            rv = theEvent.returnValue = false; //for IE
            if (theEvent.preventDefault) theEvent.preventDefault(); //Firefox
        }
    }
    return rv;
}                   // TEXT VALIDATION WITH SYMBOL
function fIsDateKey(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 47 || charCode == 8)) {
        return false;
    }
    return true;
}         // DATE  VALIDATION SCRIPT [/]
//#endregion

//#region Decimal Point
function fFormatDecBlur(ctrl, dpoint) {
    var number = "";
    var decimalpoint = "";
    var Result = 0;

    //Rounding [Get Correct Amount]
    Result = fRound(ctrl.value, dpoint);

    //Rounding [Set ### only] - toFixed rounding got issue (1.5555,3) = 1.555
    Result = Result.toFixed(dpoint);

    //Format #,###.##
    Result = Result.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    if (Result.includes(".")) {
        //Separate Number/Decimal
        number = Result.substring(0, Result.indexOf('.'));                          // Number before [.]
        decimalpoint = Result.substring(Result.indexOf('.'), Result.length);        // Number after [.]
    } else {
        number = Result;
    }
    //Remove Decimal [,]
    decimalpoint = decimalpoint.replace(",", ""); //Remove decimal point [,] Symbol
    //Combine Both
    ctrl.value = number + decimalpoint;
}     // AMOUNT FORMATING SCRIPT [,] [.]    //Pass in Inputtext itself
function fFormatDec(ctrl, dpoint) {
    var number = "";
    var decimalpoint = "";

    //Rounding
    Result = fRound(ctrl, dpoint);

    //Rounding [Set ### only] - toFixed rounding got issue (1.5555,3) = 1.555
    Result = Result.toFixed(dpoint);

    //Format #,###.##    
    Result = Result.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    if (Result.includes(".")) {
        //Separate Number/Decimal
        number = Result.substring(0, Result.indexOf('.'));                          // Number before [.]
        decimalpoint = Result.substring(Result.indexOf('.'), Result.length);        // Number after [.]
    } else {
        number = Result;
    }
    //Remove Decimal [,]
    decimalpoint = decimalpoint.replaceAll(",", "");
    //Combine Both
    return number + decimalpoint;
}         // AMOUNT FORMATING SCRIPT [,] [.] // Only pass in value
function fRound(num, dpoint) {
    var Result = 0;

    if (fChkStr(num)) {
        num = 0;
    }
    //Convert to string,Remove[,]
    num = num.toString();
    if (num.length > 0) {
        num = String(num).split(",").join("");
    } else {
        num = 0;
    }

    //Convert to Decimal
    num = parseFloat(num);
    if (isNaN(num)) {
        num = 0;
    }

    //To solve [17.65*3.5 = 61.77499999999999]
    //Should be 61.775
    num = num.toFixed(10);

    if (num < 0) {
        //Negative Number
        num = Math.abs(parseFloat(num));
        if (!("" + num).includes("e")) {
            Result = +(Math.round(num + "e+" + dpoint) + "e-" + dpoint);
            Result = Result * -1;
        } else {
            var arr = ("" + num).split("e");
            var sig = ""
            if (+arr[1] + dpoint > 0) {
                sig = "+";
            }
            Result = +(Math.round(+arr[0] + "e" + sig + (+arr[1] + dpoint)) + "e-" + dpoint);
            Result = Result * -1;
        }

    } else {
        //Positive
        if (!("" + num).includes("e")) {
            return +(Math.round(num + "e+" + dpoint) + "e-" + dpoint);
        } else {
            var arr = ("" + num).split("e");
            var sig = ""
            if (+arr[1] + dpoint > 0) {
                sig = "+";
            }
            Result = +(Math.round(+arr[0] + "e" + sig + (+arr[1] + dpoint)) + "e-" + dpoint);
        }
    }

    return Result;
}              //Only Rounding and Remove all [,]
//#endregion

//#region Loading wrapper
function fTurnoff() {
    document.getElementById("loader-wrapper").style.display = "none";
}   //#region Loading wrapper
function fTurnon() {
    document.getElementById("loader-wrapper").style.display = "";
}
//#endregion

//#region Form
function fSave(pFldName, pUrl_Chk, pUrl, pForm, pBackUrl) {
    pType = "NEW";
    pTitle = "Save";
    pFldName.value = pType;  //[NEW/EDIT]

    fChkAuthoz();    //Chk User Login Status

    //Promise
    return new Promise((resolve, reject) => {

        axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
            .then((response) => {
                if (response.data == "OK") {

                    axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                        .then((response) => {
                            if (response.data == "OK") {
                                Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                                    .then(function () {
                                        if (pBackUrl != "") {
                                            window.location = window.applicationBaseUrl + pBackUrl
                                        }
                                        resolve("Y"); //Return as onSuccess
                                        return true;
                                    });
                            } else {
                                $(response.data).each(function () {
                                    if (this.NEW_REFNO != "") {
                                        resolve(this.NEW_REFNO); //Return as onSuccess
                                        Swal.fire('Save!', 'Save Done. Number Change To ' + this.NEW_REFNO, 'success');
                                        return false;
                                    }
                                });

                                Swal.fire('Failed!', response.data[0].ErrorMessage, 'error');
                                reject(response.data[0].ErrorMessage);    //Return as onError
                                return false;
                            }
                        })

                }
                else if (response.data == "E101") {
                    window.location = window.applicationBaseUrl + gUrl_E101;
                    return false;
                }
                else {
                    Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
                }
                return false;
            })
            .catch((error) => {
                Swal.fire('Failed!', error, 'error'); return false;
            });
    })
}

//Form Save
function fCRUD(pTitle, pUrl_Chk, pUrl, pForm = '#form1', pBackUrl = '') {
    fChkAuthoz();    //Chk User Login Status

    if (pUrl_Chk != '') {
        //Promise
        return new Promise((resolve, reject) => {
            axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
                .then((response) => {

                    if (response == undefined) {
                        location.reload();
                        return false;
                    };  //Solve Login expired

                    if (response.data == "OK") {
                        Swal.fire({
                            title: 'Are you sure?',
                            text: "You wanna to " + pTitle + " this Record?",
                            imageUrl: window.applicationBaseUrl + gI_Update,
                            imageWidth: 400,
                            imageHeight: 200,
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Yes'
                        }).then((result) => {
                            if (result.value) {

                                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                                    .then((response) => {

                                        if (response.data == "OK") {
                                            Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                                                .then(function () {
                                                    if (pBackUrl != "") {
                                                        window.location = window.applicationBaseUrl + pBackUrl
                                                    }
                                                    resolve("Y"); //Return as onSuccess
                                                    return true;
                                                });
                                        } else {
                                            Swal.fire('Failed!', response.data[0].ErrorMessage, 'error');
                                            console.log(response.data[0].ErrorMessage);    //Return as onError
                                            return false;
                                        }
                                    });
                            }
                        });
                        return false;
                    }
                    else if (response.data[0].ErrorMessage.toUpperCase().includes("WARNING!")) {
                        Swal.fire({
                            title: 'Warning!',
                            text: response.data[0].ErrorMessage,
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Yes'
                        }).then((result) => {
                            if (result.value) {
                                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                                    .then((response) => {

                                        if (response.data == "OK") {
                                            Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                                                .then(function () {
                                                    if (pBackUrl != "") {
                                                        window.location = window.applicationBaseUrl + pBackUrl
                                                    }
                                                    resolve("Y"); //Return as onSuccess
                                                    return true;
                                                });
                                        }
                                        else {
                                            Swal.fire('Failed!', response.data[0].ErrorMessage, 'error');
                                            reject(response.data[0].ErrorMessage);    //Return as onError
                                            return false;
                                        }
                                    });
                            } else {
                                reject("N");
                                return false;
                            }
                        });
                        return false;
                    }
                    else if (response.data == "E101") {
                        window.location = window.applicationBaseUrl + gUrl_E101;
                        return false;
                    }
                    else {
                        Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
                    }
                    return false;
                })
                .catch((error) => {
                    Swal.fire('Failed!', error, 'error'); return false;
                });
        })
    } else {
        Swal.fire({
            title: 'Are you sure?',
            text: "You wanna to " + pTitle + " this Record?",
            imageUrl: window.applicationBaseUrl + gI_Update,
            imageWidth: 400,
            imageHeight: 200,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {

                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                    .then((response) => {

                        if (response.data == "OK") {
                            Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                                .then(function () {
                                    if (pBackUrl != "") {
                                        window.location = window.applicationBaseUrl + pBackUrl
                                    }
                                    resolve("Y"); //Return as onSuccess
                                    return true;
                                });
                        } else {
                            Swal.fire('Failed!', response.data[0].ErrorMessage, 'error');
                            console.log(response.data[0].ErrorMessage);    //Return as onError
                            return false;
                        }
                    });
            }
        });
        return false;
    }
}

function fNewRecord(pUrl, pID, pRecord) {
    fChkAuthoz();    //Chk User Login Status

    //Promise
    return new Promise((resolve, reject) => {
        axios.post(window.applicationBaseUrl + pUrl + '?oID=' + pID)
            .then((response) => {
                document.getElementById(pRecord).value = response.data;
                return false;
            });
    });
}

function fResetAllToZero(pTitle, str, pForm = '#form1', pBackUrl = '') {
    Swal.fire({
        title: 'Are you sure?',
        text: "You wanna to " + pTitle + "?",
        imageUrl: window.applicationBaseUrl + gI_Update,
        imageWidth: 400,
        imageHeight: 200,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            const collection = document.getElementsByClassName(str);
            for (let i = 0; i < collection.length; i++) {
                collection[i].value = "0";
            }
        }
    });
    return false;
}

//Form Update
function fDel(pFldName, pUrl_Chk, pUrl, pForm, pBackUrl) {
    pType = "DEL";
    pTitle = "Delete";

    fChkAuthoz();    //Chk User Login Status

    Swal.fire({
        title: 'Are you sure?',
        text: "You wanna to " + pTitle + " this Record?",
        imageUrl: window.applicationBaseUrl + gI_Delete,
        imageWidth: 400,
        imageHeight: 200,
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                .then((response) => {
                    if (response.data == "OK") {
                        Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                            .then(function () {
                                window.location = window.applicationBaseUrl + pBackUrl;
                                return false;
                            });
                    }
                    else if (response.data == "E101") {
                        window.location = window.applicationBaseUrl + gUrl_E101;
                        return false;
                    }
                    else {
                        Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
                    }
                })
        }
    })
}           //Form Del
function fImport(pFldName, pUrl_Chk, pUrl, pForm, pBackUrl) {

    pType = "IMPORT";
    pTitle = "Import";
    pFldName.value = pType;

    fChkAuthoz();    //Chk User Login Status

    //Promise
    return new Promise((resolve, reject) => {

        axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
            .then((response) => {
                if (response.data == "OK") {

                    Swal.fire({
                        title: 'Are you sure?',
                        text: "You wanna to " + pTitle + " ?",
                        imageUrl: window.applicationBaseUrl + gI_Update,
                        imageWidth: 400,
                        imageHeight: 200,
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes'
                    }).then((result) => {
                        if (result.value) {

                            axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                                .then((response) => {
                                    if (response.data == "OK") {
                                        Swal.fire({ icon: 'success', title: pTitle + ' Done.', showConfirmButton: false, timer: 1500 })
                                            .then(function () {
                                                resolve("Y"); //Return as onSuccess
                                                return true;
                                            });
                                    } else {
                                        Swal.fire('Failed!', response.data[0].ErrorMessage, 'error');
                                        reject(response.data[0].ErrorMessage);    //Return as onError
                                        return false;
                                    }
                                })
                        }
                        else {
                            reject("Y"); //Return as onSuccess
                        }
                    });
                    return false;
                }
                else if (response.data == "E101") {
                    window.location = window.applicationBaseUrl + gUrl_E101;
                    return false;
                }
                else {
                    Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
                }
                return false;
            })
            .catch((error) => {
                Swal.fire('Failed!', error, 'error'); return false;
            });
    })
}        //Form Import
function fPHeader(pFldName, pUrl_Chk, pUrl, pForm, pBackUrl, pType, pTitle) {
    //pType = Post type
    //pTitle = Message disply when meet [Warning] condition

    pFldName.value = pType;  //[NEW/EDIT]

    fChkAuthoz();    //Chk User Login Status

    //Promise
    return new Promise((resolve, reject) => {

        axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
            .then((response) => {
                if (response.data == "OK") {

                    axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                        .then((response) => {
                            if (response.data == "OK") {
                                resolve("Y"); //Return as onSuccess
                                return true;

                            } else {
                                $(response.data).each(function () {
                                    if (this.NEW_REFNO != "") {
                                        resolve(this.NEW_REFNO); //Return as onSuccess
                                        Swal.fire('Save!', 'Save Done. Number Change To ' + this.NEW_REFNO, 'success');
                                        resolve("Y"); //Return as onSuccess
                                        return false;
                                    }
                                });

                                reject(response.data[0].ErrorMessage);
                                return false;
                            }
                        })

                }
                else if (response.data == "E101") {
                    window.location = window.applicationBaseUrl + gUrl_E101;
                    return false;
                }

                else if (response.data[0].ErrorMessage.toUpperCase().includes("WARNING!")) {
                        Swal.fire({
                            title: 'Warning!',
                            text: response.data[0].ErrorMessage,
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Yes'
                        }).then((result) => {
                            if (result.value) {
                                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                                    .then((response) => {
                                        if (response.data == "OK") {
                                            resolve("Y"); //Return as onSuccess
                                            return true;

                                        } else {
                                            reject(response.data[0].ErrorMessage);
                                            return false;
                                        }
                                    })
                            } else {
                                reject("N");
                                return false;
                            }
                        });
                    return false;
                }
                else {
                    Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
                }

                return false;
            })
            .catch((error) => {
                Swal.fire('Failed!', error, 'error'); return false;
            });
    })
}   //Form [update Header]
//#endregion

//#region Report
function fPrtA4(pUrl_Chk, pUrl, pForm) {
    fChkAuthoz();    //Chk User Login Status

    fTurnon();

    axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
        .then((response) => {
            if (response.data == "OK") {
                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                    .then((response) => {

                        fTurnoff();
                        window.open('', pUrl, "width=1000,height=700,top=50,left=50,scrollbars=yes,toolbar=no").document.body.innerHTML = response.data;

                        return false;
                    }).catch((error) => {
                        fTurnoff(); return false;
                    });
            }
            else if (response.data == "E101") {
                window.location = window.applicationBaseUrl + gUrl_E101; return false;
            }
            else {
                fTurnoff();
                Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
            }
            return false;
        })
        .catch((error) => {
            fTurnoff();
            Swal.fire('Failed!', error, 'error'); return false;
        });
}   //Report[A4]
function fPrtA4_Land(pUrl_Chk, pUrl, pForm) {
    fChkAuthoz();    //Chk User Login Status

    fTurnon();

    axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
        .then((response) => {
            if (response.data == "OK") {
                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                    .then((response) => {

                        fTurnoff();
                        window.open('', pUrl, "width=1400,height=700,top=50,left=50,scrollbars=yes,toolbar=no").document.body.innerHTML = response.data;

                        return false;
                    }).catch((error) => {
                        fTurnoff(); return false;
                    });
            }
            else if (response.data == "E101") {
                window.location = window.applicationBaseUrl + gUrl_E101; return false;
            }
            else {
                fTurnoff();
                Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
            }
            return false;
        })
        .catch((error) => {
            fTurnoff();
            Swal.fire('Failed!', error, 'error'); return false;
        });
}   //Report[A4L]
function fPrtUS(pUrl_Chk, pUrl, pForm) {
    fChkAuthoz();    //Chk User Login Status

    fTurnon();

    axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
        .then((response) => {
            if (response.data == "OK") {
                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                    .then((response) => {
                        fTurnoff();
                        window.open('', pUrl, "width=" + screen.width + ",height=" + screen.height + ",top=50,left=50,scrollbars=yes,toolbar=no").document.body.innerHTML = response.data;
                        return false;
                    }).catch((error) => {
                        console.log("error = " + error);
                        fTurnoff(); return false;
                    });
            }
            else if (response.data == "E101") {
                window.location = window.applicationBaseUrl + gUrl_E101; return false;
            }
            else {
                fTurnoff();
                Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
            }
            return false;
        })
        .catch((error) => {
            fTurnoff();
            Swal.fire('Failed!', error, 'error'); return false;
        });
}   //Report[US]
function fPrtLetter(pUrl_Chk, pUrl, pForm) {
    fChkAuthoz();    //Chk User Login Status

    fTurnon();

    axios.post(window.applicationBaseUrl + pUrl_Chk, $(pForm).serialize())
        .then((response) => {
            if (response.data == "OK") {

                axios.post(window.applicationBaseUrl + pUrl, $(pForm).serialize())
                    .then((response) => {
                        fTurnoff();
                        window.open('', pUrl, "width=1200,height=700,top=50,left=50,scrollbars=yes,toolbar=no").document.body.innerHTML = response.data;
                        return false;
                    }).catch((error) => {
                        fTurnoff(); return false;
                    });
            }
            else if (response.data == "E101") {
                window.location = window.applicationBaseUrl + gUrl_E101; return false;
            }
            else {
                fTurnoff();
                Swal.fire('Failed!', response.data[0].ErrorMessage, 'error'); return false;
            }
            return false;
        })
        .catch((error) => {
            fTurnoff();
            Swal.fire('Failed!', error, 'error'); return false;
        });
}   //Report[Letter] - Lew Add 08/12/2020
//#endregion

//#region Function
function fChkStr(value) {
    var result = false;
    if (value == null || value == "") {
        result = true;
    }
    return result;
}       // CHECK STRING "" OR NULL
function fEncodeUrl(value) {
    value = encodeURIComponent(value);
    if (value.includes("#")) {
        value.replace("'", "%27");
    }
    return value;
}    //ENCODE URL SYMBOL
function fChkAuthoz() {
    //Promise
    return new Promise((resolve, reject) => {
        axios.post(window.applicationBaseUrl + gAuthz)
            .then((response) => {
                if (response.data != "OK") {
                    window.location.assign(window.applicationBaseUrl);
                }
                resolve();
            });
    });
    return false;
}         //Check User Login Timeout status   //KHOMAS 20201023
//#endregion