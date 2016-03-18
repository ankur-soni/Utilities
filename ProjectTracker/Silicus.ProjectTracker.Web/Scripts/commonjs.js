$(window).load(function(){
	var mainW = $(window).height() -70;
	$('.Leftnav').css({ height : mainW })
	$('.Leftnav-logo').hide()
	$('#loading').fadeOut(1000);
	$('.Leftnav-logo').fadeIn()
}) // Window load

$(window).resize(function() {
    //var mainW = $('.main-content').height();
    var mainW = $(window).height() - 70;
    $('.Leftnav').css({ height: mainW + 50 })
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

    // $('.Leftnav').hide();
			$('.Leftnav-button').click(function(){
			    // $('.Leftnav').toggleClass('show');
				$('.Leftnav').toggleClass('show')
			    //$('.Leftnav').fadeToggle(function(){
					
				//})
			})
			$('.collapsible > a').click(function(){
				$(this).parent().toggleClass('open')
			})
    
}); // Ready

function mQuery() {
    // Same as @media (max-width: 767px) -> hide the navigation
    if ($('.fluid [class*="grid"]').css('float') == 'none' && $('.kendogrid [class*="grid"]').css('float') != 'none') {
        $('.Leftnav').removeClass('show');
    } else {
        //if($(window).width() >= 995)
        //    $('.Leftnav').addClass('show');
        //else
        //    $('.Leftnav').removeClass('show');
        $('.Leftnav').addClass('show');
    }
}
