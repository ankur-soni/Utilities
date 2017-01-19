
$(window).load(function () {
    var mainW = $(window).height() - 70;
    $('.nav-logo').hide()
    $('#loading').fadeOut(1000);
    $('.nav-logo').fadeIn()
    InitializeKendoGridIcons();
})

$(window).resize(function () {
    //Grid Resizing
    kendo.resize($(".kendogrid"));
})

$(document).ready(function () {
    $(document).on('click', '#menu-toggle', function () {
        $("body").toggleClass("mini-navbar");
    });

    $('.nav').css({ height: 0 })
    mQuery();
    $('.nav-button').click(function () {
        $('.nav').toggleClass('show')
    })
    $('.collapsible > a').click(function () {
        $(this).parent().toggleClass('open')
    })
});

function mQuery() {
    // Same as @media (max-width: 767px) -> hide the navigation
    if ($('.fluid [class*="grid"]').css('float') == 'none' && $('.kendogrid [class*="grid"]').css('float') != 'none') {
        $('.nav').removeClass('show');
    } else {
        $('.nav').addClass('show');
    }
}

function ShowMessage(content, isSuceess) {
    $("#messageDiv").removeClass("alert-danger");
    $("#messageDiv").removeClass("alert-success");

    if (isSuceess == 0) { $("#messageDiv").addClass("alert-danger"); }
    else { $("#messageDiv").addClass("alert-success"); }

    $("#messageDiv").show();
    $("#messageContent").text(content);
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $("#messageDiv").fadeOut(9000);
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
        closeOnConfirm: false,
        closeOnCancel: false
    },
    function (isConfirm) {
        if (isConfirm) {
            grid = $("#" + gridId).data("kendoGrid");
            grid.dataSource.remove(data);
            grid.dataSource.sync();
            swal("Deleted!", "Record has been deleted.", "success");
        } else {
            swal("Cancelled", "Record deletion cancelled.", "error");
        }
    });
}

//function InitializeKendoGridIcons() {
//    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
//    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
//}