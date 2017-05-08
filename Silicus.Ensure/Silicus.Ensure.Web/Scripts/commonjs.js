
$(window).load(function () {
    var mainW = $(window).height() - 70;
    $('.nav-logo').hide()
    $('#loading').fadeOut(1000);
    $('.nav-logo').fadeIn()
})

$(window).resize(function () {
    //Grid Resizing
    kendo.resize($(".kendogrid"));
})

$(document).ready(function () {
 //   $(document).on('click', '#QuestionBank', showCustomLoader);
    $(document).on('click', '#menu-toggle', function () {
        $("body").toggleClass("mini-navbar");
    });
});
function SelectObjectiveQuestion() {
    $('#practical-test-side-bar').hide();
    $('#objective-test-side-bar').show();
    $('#practical-test').removeClass('active');
    $('#objective-test').addClass('active');
    //$('#pracQuestion').hide();
    //$('#objQuestion').show();
    //$('#objQuestion').text('OBJECTIVE QUESTION');
}

function SelectPracticalQuestion() {
    $('#objective-test-side-bar').hide();
    $('#practical-test-side-bar').show();
    $('#objective-test').removeClass('active');
    $('#practical-test').addClass('active');
    //$('#objQuestion').hide();
    //$('#pracQuestion').show();
    //$('#pracQuestion').text('PRACTICAL QUESTION');
}

function ShowMessage(content, isSuceess) {
    if (isSuceess) {
        toastr.success(content);
    }
    else {
        toastr.error(content)
    }
}

function RefreshKendoGrid(gridName) {
    if (gridName) {
        var grid = $('#' + gridName).data('kendoGrid');
        if (grid) {
            grid.dataSource.read();
            grid.refresh();
        }
    }
}

function SetNavigationMenuActive(menuId) {
    $('#' + menuId).addClass('active');
}

function deleteKendoGridRow(gridId, data) {
    swal({
        title: "Are you sure,",
        text: "Are you sure, you want to delete this record?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: true,
        closeOnCancel: true
    },
    function (isConfirm) {
        if (isConfirm) {
            grid = $("#" + gridId).data("kendoGrid");
            grid.dataSource.remove(data);
            grid.dataSource.sync();         
           
        } else {          
           
        }
    });
}




function ShowSweetAlertWithoutCancel(title, text, type) {
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    });
}

function showCustomLoader() {
    $(".loader-overlay").show();
}

function hideCustomLoader() {
    $(".loader-overlay").hide();
}