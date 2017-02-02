Array.prototype.getUnique = function () {
    var u = {}, a = [];
    for (var i = 0, l = this.length; i < l; ++i) {
        if (u.hasOwnProperty(this[i])) {
            continue;
        }
        a.push(this[i]);
        u[this[i]] = 1;
    }
    return a;
}

function toHHMM(time) {
    var decimalTime = parseFloat(time);
    decimalTime = decimalTime * 60 * 60;
    var hours = Math.floor((decimalTime / (60 * 60)));
    decimalTime = decimalTime - (hours * 60 * 60);
    var minutes = Math.floor((decimalTime / 60));
    decimalTime = decimalTime - (minutes * 60);

    if (hours < 10) {
        hours = "0" + hours;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }

    return hours + ':' + minutes;
}

String.prototype.toHrs = function () {
    var hours = 0;
    var timeStr = this.replace(/_/g, '').replace(/__/g, '');
    if (timeStr) {
        var a = timeStr.split(':');
        hours = (+a[0]) + (+a[1]) / 60;
    }
    return hours;
}

function blockUI() {
    $(".loader-overlay").show();
};

function unblockUI() {
    $(".loader-overlay").hide();
};

function rebindValidation(formId) {
    $form = $(formId);
    $form.removeData('validator');
    $form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse($form);
}

function showAlert(options) {
    swal({
        title: options.title,
        text: options.text,
        type: options.type,
        timer: options.timer || null,
        showConfirmButton: options.showConfirmButton || false,
        showCancelButton: options.showCancelButton || false,
        confirmButtonColor: options.confirmButtonColor || "#337ab7",
        confirmButtonText: options.confirmButtonText || "Ok"
    });
}


function onRowEdit(e) {
    if (e.model.isNew()) {
        $(".k-window-title").html("Add");        
    } else {
        $(".k-window-title").html("Edit");        
    }    
}

