$(document).ready(function () {
    var ScreenHeight = $(window).height();
    var topmargin = $(".navbar-inverse").height();
    var bxHeight = ScreenHeight - ($(".footer").innerHeight() + $(".navbar-inverse").height());
    $(".same-bx-height").css("min-height", bxHeight);
    $(".same-bx-height").css("top", topmargin);
    $(".body-content").css("margin-top", topmargin);

    var url = window.location.pathname;
    var abc = url.split("/");
    var action = abc[2];
    if (action != undefined) {
        $('#' + action).addClass('active');
        if (action == "AddExtensionCode" || action == "ShowMyExtensionCode" || action == "ShowExtensionCode" || action == "ReviewExtensionCodeList") {
            $("#ExtesionMenu").css("display", "block");
        }
        else {
            $("#OtherCodeMenu").css("display", "block");
            $('.dashboard-name').text('Code Snippets');
        }
    }
    else {
        $('#GetAllCategories').addClass('active');
    }

    sidebarautoheight();

    $(window).resize(function () {
        sidebarautoheight();
    });

    $("#menu-toggle").click(function () {
        $("body").toggleClass("mini-navbar");
    });
});
$('#example4, #example3, #example2, #example1').click(function () {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
            timeOut: 4000
        };
        toastr.success('Card Status Saved');

    }, 1300);
});

$('#side-menu li').find('ul').hide();
$('#side-menu li').hover(function () {
    $(this).find('ul').show(500);
},
    function () {
        $(this).find('ul').hide();
    });

function sidebarautoheight() {
    var sidebarheight = 0;
    sidebarheight = $(window).height() - $(".bbl").outerHeight() - $("footer").outerHeight();
    $(".wrapper .page-wrapper").css("min-height", sidebarheight);
};