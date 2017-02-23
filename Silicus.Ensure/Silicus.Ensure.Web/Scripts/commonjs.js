
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
         
            ShowMessage("Record has been deleted.", 1);
        } else {
          
            ShowMessage("Record deletion cancelled.", 1);
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