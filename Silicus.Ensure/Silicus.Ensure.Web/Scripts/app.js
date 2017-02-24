/*****
* CONFIGURATION
*/
    //Main navigation
    $.navigation = $('nav > ul.nav');

  $.panelIconOpened = 'icon-arrow-up';
  $.panelIconClosed = 'icon-arrow-down';

  //Default colours
  $.brandPrimary =  '#20a8d8';
  $.brandSuccess =  '#4dbd74';
  $.brandInfo =     '#63c2de';
  $.brandWarning =  '#f8cb00';
  $.brandDanger =   '#f86c6b';

  $.grayDark =      '#2a2c36';
  $.gray =          '#55595c';
  $.grayLight =     '#818a91';
  $.grayLighter =   '#d1d4d7';
  $.grayLightest =  '#f8f9fa';

'use strict';

/****
* MAIN NAVIGATION
*/

$(document).ready(function($){ 
	  
	/*Serach dropdown*/
		var serch='[{"name": "Personal Details ", "link": "index.html","icon":"fa fa-user"},{"name": "Contact Details", "link": "contact.html", "icon":"fa fa-mobile"},{"name": "Education Details", "link": "education.html", "icon":"fa fa-graduation-cap"},{"name": "Emplyoment Details", "link": "emplyoment.html", "icon":"fa fa-briefcase"},{"name": "Family Details", "link": "family.html", "icon":"fa fa-users"},{"name": "Upload Doucments", "link": "documentupload.html", "icon":"fa fa-cloud"},{"name": "Learn", "link": "learn.html", "icon":"fa fa-leanpub"},{"name": "Support", "link": "support.html", "icon":"fa fa-life-ring"}]'; 
		var Serchsort = $.parseJSON(serch);
			var sort_by = function(field, reverse, primer){ 
				var key = primer ? 
				function(x) {return primer(x[field])} : 
				function(x) {return x[field]}; 
				reverse = !reverse ? 1 : -1; 
				return function (a, b) {
				return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
				} 
			} 			
		var result = Serchsort.sort(sort_by('name', false, function(a){return a.toUpperCase()})); 
		
			$(document).on('keyup', '#keysug', function(e) {  
				var numcar=$(this).val().length;     
				var timer;     
				clearTimeout(timer);    
				var ms = 300; // milliseconds     
				var key = this.value;        
				timer = setTimeout(function() {    
					$("#keysugList ul").html(""); 
					$("#keysugList").hide();       
					if(numcar>0) {  
						$.each(result, function() { 
							if(this.name.toLowerCase().indexOf(key.toLowerCase()) >= 0){ 
								$("#keysugList ul").append("<li class='keyFinal'> <a href='"+this.link+"'><i class='"+this.icon+"'></i>"+this.name+"</a> </li>");   
							}  
						});
						 
						if($("#keysugList ul li").length == 0){
							$("#keysugList ul").append("<div class='RecordNotFound'>Record Not Found </div>");   
						}
						$("#keysugList").show();        
					}
				}, ms); 
			});    
			
	//tooltip		
	$('[data-toggle="tooltip"]').tooltip()
	 
  // Add class .active to current link
  $.navigation.find('a').each(function(){

    var cUrl = String(window.location).split('?')[0];

    if (cUrl.substr(cUrl.length - 1) == '#') {
      cUrl = cUrl.slice(0,-1);
    }

    if ($($(this))[0].href==cUrl) {
      $(this).parent().addClass('active');

      $(this).parents('ul').add(this).each(function(){
		//$(this).parent().addClass('open');
		 $(this).parent().addClass('open');
      });
    }
  });

  // Dropdown Menu
  $.navigation.on('click', 'a', function(e){

    if ($.ajaxLoad) {
      e.preventDefault();
    }

    if ($(this).hasClass('nav-dropdown-toggle')) {
      $(this).parent().toggleClass('open');
      resizeBroadcast();
    }

  });

  function resizeBroadcast() {  
    var timesRun = 0;
    var interval = setInterval(function(){
      timesRun += 1;
      if(timesRun === 5){ 
        clearInterval(interval); 
      }
      window.dispatchEvent(new Event('resize'));
    }, 62.5);
  }

  /* ---------- Main Menu Open/Close, Min/Full ---------- */
  $('.navbar-toggler').click(function(){

    if ($(this).hasClass('sidebar-toggler')) {
      $('body').toggleClass('sidebar-hidden');
	   if($("body" ).hasClass('sidebar-hidden')){
		 $(this).removeClass('activeBtn');
	  }
	  else{
		  $(this).addClass('activeBtn'); 
	  }
      resizeBroadcast();
    }

    if ($(this).hasClass('aside-menu-toggler')) { 
      $('body').toggleClass('aside-menu-hidden');
	  if($("body" ).hasClass('aside-menu-hidden')){
		 $('.asideBtn').removeClass('activeBtn');
	  }
	  else{
		  $('.asideBtn').addClass('activeBtn');
		  
	  }
      resizeBroadcast();
    }

    if ($(this).hasClass('mobile-sidebar-toggler')) {
      $('body').toggleClass('sidebar-mobile-show');
      resizeBroadcast();
    }

  });
  
  //setting button active class
   $('.dropdown-toggle').click(function(){ 
	   if( $(this).attr('aria-expanded')  === "false"){
			 $('.settingBtn').addClass('activeBtn');
		}
	   else{
		    $('.settingBtn').removeClass('activeBtn');
	   }
   }); 

  $('.sidebar-close').click(function(){
    $('body').toggleClass('sidebar-opened').parent().toggleClass('sidebar-opened');
  });

  /* ---------- Disable moving to top ---------- */
  $('a[href="#"][data-top!=true]').click(function(e){
    e.preventDefault();
  });

});

/****
* CARDS ACTIONS
*/

/*$(document).on('click', '.card-actions a', function(e){
  e.preventDefault();

  if ($(this).hasClass('btn-close')) {
    $(this).parent().parent().parent().fadeOut();
  } else if ($(this).hasClass('btn-minimize')) {
    var $target = $(this).parent().parent().next('.card-block');
    if (!$(this).hasClass('collapsed')) {
      $('i',$(this)).removeClass($.panelIconOpened).addClass($.panelIconClosed);
    } else {
      $('i',$(this)).removeClass($.panelIconClosed).addClass($.panelIconOpened);
    }

  } else if ($(this).hasClass('btn-setting')) {
    $('#myModal').modal('show');
  }

});*/

function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

function init(url) {

  /* ---------- Tooltip ---------- */
  $('[rel="tooltip"],[data-rel="tooltip"]').tooltip({"placement":"bottom",delay: { show: 400, hide: 200 }});

  /* ---------- Popover ---------- */
  $('[rel="popover"],[data-rel="popover"],[data-toggle="popover"]').popover();

}

