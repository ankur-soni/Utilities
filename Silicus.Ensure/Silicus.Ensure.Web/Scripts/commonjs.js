$(window).load(function(){
	var mainW = $(window).height() -70;
	//$('.nav').css({ height : mainW })
	$('.nav-logo').hide()
	$('#loading').fadeOut(1000);
	$('.nav-logo').fadeIn()
}) // Window load

$(window).resize(function() {
    //var mainW = $('.main-content').height();
    //var mainW = $(window).height() - 70;
    //$('.nav').css({ height: mainW + 50 })
    //mQuery();

    //Grid Resizing
    kendo.resize($(".kendogrid"));
    
}) // Window resize

$(document).ready(function () {

    //toastr.options = {
    //    "closeButton": true,
    //    "debug": false,
    //    "positionClass": "toast-bottom-right",
    //    "onclick": null,
    //    "showDuration": "300",
    //    "hideDuration": "1000",
    //    "timeOut": "5000",
    //    "extendedTimeOut": "1000",
    //    "showEasing": "swing",
    //    "hideEasing": "linear",
    //    "showMethod": "fadeIn",
    //    "hideMethod": "fadeOut"
    //}

    //setTimeout(function () {
    //    toastr.info('<span style="color:#333;">Welcome to Silicus! The subtle and striking admin theme.</span>');
    //}, 2000);

    //setTimeout(function () {
    //    toastr.warning('<span style="color:#333;">You could navigate the different sections to discover it...</span>');
    //}, 4500);

    mQuery();

    // $('.nav').hide();
			$('.nav-button').click(function(){
				// $('.nav').toggleClass('show');
				$('.nav').toggleClass('show')
				//$('.nav').fadeToggle(function(){
					
				//})
			})
			$('.collapsible > a').click(function(){
				$(this).parent().toggleClass('open')
			})
    
}); // Ready

function mQuery() {
    // Same as @media (max-width: 767px) -> hide the navigation
    if ($('.fluid [class*="grid"]').css('float') == 'none' && $('.kendogrid [class*="grid"]').css('float') != 'none') {
        $('.nav').removeClass('show');
    } else {
        //if($(window).width() >= 995)
        //    $('.nav').addClass('show');
        //else
        //    $('.nav').removeClass('show');
        $('.nav').addClass('show');
    }
}

function ShowMessage(content, isSuceess) {
    $("#messageDiv").removeClass("alert-danger");
    $("#messageDiv").removeClass("alert-success");

    if (isSuceess == 0) { $("#messageDiv").addClass("alert-danger");}
    else { $("#messageDiv").addClass("alert-success");}

    $("#messageDiv").show();
    $("#messageContent").text(content);
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $("#messageDiv").fadeOut(9000);
}
