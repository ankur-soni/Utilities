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
        confirmButtonText: options. confirmButtonText ||  "Ok"
    });
}

