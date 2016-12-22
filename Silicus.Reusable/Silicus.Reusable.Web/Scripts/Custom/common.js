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
    document.getElementById("loader").style.display = "block";
};

function unblockUI() {
    document.getElementById("loader").style.display = "none";
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
        timer: options.timer || 1000,
        showConfirmButton: options.showConfirmButton || false
    });
}

