/*****
* CONFIGURATION
*/
//Main navigation
$.navigation = $('nav ul.nav');

$.panelIconOpened = 'icon-arrow-up';
$.panelIconClosed = 'icon-arrow-down';

//Default colours
$.brandPrimary = '#20a8d8';
$.brandSuccess = '#4dbd74';
$.brandInfo = '#63c2de';
$.brandWarning = '#f8cb00';
$.brandDanger = '#f86c6b';

$.grayDark = '#2a2c36';
$.gray = '#55595c';
$.grayLight = '#818a91';
$.grayLighter = '#d1d4d7';
$.grayLightest = '#f8f9fa';

'use strict';

/****
* MAIN NAVIGATION
*/

$(document).ready(function ($) {

    $('.selectpicker').on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
        var selected = $(e.currentTarget).val();
        window.location.href = selected;
    });

    //for mobile search
    $('.mobileSearchIcon a').on('click', function () {
        $('.SearchBar').addClass('SearchBarMobile');
    });
    $('.SearchBar .input-group-btn').on('click', function () {
        if ($('.SearchBar').hasClass('SearchBarMobile')) {
            $('.SearchBar').removeClass('SearchBarMobile');
        }
    });

    //poup algin center 
    function alignModal() {
        //debugger;
        var modalDialog = $(this).find(".modal-dialog");

        // Applying the top margin on modal dialog to align it vertically center
        modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
    }
    // Align modal when it is displayed
    $(".modal").on("shown.bs.modal", alignModal);

    // Align modal when user resize the window
    $(window).on("resize", function () {
        $(".modal:visible").each(alignModal);
    });


    /*Serach dropdown*/
    var serch = '[{ "name": "Final Status", "link": "/document/FinalStatus", "icon": "fa fa-th-list" }, { "name": "Personal Details ", "link": "/User/PersonalDetails", "icon": "fa fa-user" }, { "name": "Contact Details", "link": "/User/ContactDetails", "icon": "fa fa-mobile" }, { "name": "Education Details", "link": "/Education/GetEducationalDetails", "icon": "fa fa-graduation-cap" }, { "name": "Employment Details", "link": "/Employement/Index", "icon": "fa fa-briefcase" }, { "name": "Family Details", "link": "/Family/FamilyDetails", "icon": "fa fa-users" }, { "name": "Upload Documents", "link": "/document/index", "icon": "fa fa-cloud" }, { "name": "Learn", "link": "/document/Learn", "icon": "fa fa-leanpub" }, { "name": "Support", "link": "/document/Support", "icon": "fa fa-life-ring" }]';
    //var serch = [{ "label": "Personal Details ", "link": "/User/PersonalDetails", "icon": "fa fa-user" }, { "label": "Contact Details", "link": "/User/ContactDetails", "icon": "fa fa-mobile" }, { "label": "Education Details", "link": "/Education/GetEducationalDetails", "icon": "fa fa-graduation-cap" }, { "label": "Employment Details", "link": "/Employement/Index", "icon": "fa fa-briefcase" }, { "label": "Family Details", "link": "/Family/FamilyDetails", "icon": "fa fa-users" }, { "label": "Upload Documents", "link": "/document/index", "icon": "fa fa-cloud" }, { "label": "Learn", "link": "learn.html", "icon": "fa fa-leanpub" }, { "label": "Support", "link": "support.html", "icon": "fa fa-life-ring" }];
   // $("#keysug").autocomplete({
   //     minLength: 0,
   //     source: serch,
   //     focus: function (event, ui) {
   //         $("#keysug").val(ui.item.label);
   //         return false;
   //     },
   //     select: function (event, ui) {
   //         location.href = ui.item.link;
   //     }
   // })
   //.autocomplete("instance")._renderItem = function (ul, item) {
   //    return $("<li>")
   //    .append("<li class='keyFinal'> <a href='" +  item.link + "'><i class='" + item.icon + "'></i>" + item.label + "</a> </li>")
   //      //.append("<div>" + item.label + "<br>" + item.desc + "</div>")
   //      .appendTo(ul);
   //}


    
    var Serchsort = $.parseJSON(serch);
    var sort_by = function (field, reverse, primer) {
        var key = primer ?
        function (x) { return primer(x[field]) } :
        function (x) { return x[field] };
        reverse = !reverse ? 1 : -1;
        return function (a, b) {
            return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
        }
    }
    var result = Serchsort.sort(sort_by('name', false, function (a) { return a.toUpperCase() }));

    $(document).on('keyup', '#keysug', function (e) {
        var numcar = $(this).val().length;
        var timer;
        clearTimeout(timer);
        var ms = 300; // milliseconds     
        var key = this.value;
        var key1 = e.keyCode;
        timer = setTimeout(function () {
            if (key1 != 40 && key1 != 38 && key1 != 13) {
                $("#keysugList ul").html("");
                $('#keysug').attr('data', '');
                $("#keysugList").hide();
                if (numcar > 0) {
                    $.each(result, function () {
                        if (this.name.toLowerCase().indexOf(key.toLowerCase()) >= 0) {
                            $("#keysugList ul").append("<li class='keyFinal'> <a href='" + this.link + "'><i class='" + this.icon + "'></i>" + this.name + "</a> </li>");
                        }
                    });

                    if ($("#keysugList ul li").length == 0) {
                        $("#keysugList ul").append("<div class='RecordNotFound'>Record Not Found </div>");
                    }
                    $("#keysugList").show();
                }
            }

            var $listItems = $('#keysugList li');
            var $selected = $listItems.filter('.selected'),
            $current;
            if (key1 != 40 && key1 != 38 && key1 != 13) return;
            $listItems.removeClass('selected');
            if (key1 == 40) {// Down key
                if (!$selected.length || $selected.is(':last-child')) {
                    $current = $listItems.eq(0);
                    setInput($current);
                }
                else {
                    $current = $selected.next();
                    setInput($current);
                }
            }
            else if (key1 == 38) // Up key
            {
                if (!$selected.length || $selected.is(':first-child')) {
                    $current = $listItems.last();
                    setInput($current);
                }
                else {
                    $current = $selected.prev();
                    setInput($current);
                }
            }
            $current.addClass('selected');


        }, ms);

        if (key1 == 13) {
            if ($('#keysug').attr('data')) {
                window.location.href = $('#keysug').attr('data');
            }
        }
    });

    function setInput(a) {
        var link = $(a).find('a').attr('href');
        $('#keysug').val($(a).text());
        $('#keysug').attr('data', link);
    }



    //tooltip		
    $('[data-toggle="tooltip"]').tooltip()

    // Add class .active to current link
    $.navigation.find('a').each(function () {

        var cUrl = String(window.location).split('?')[0];

        if (cUrl.substr(cUrl.length - 1) == '#') {
            cUrl = cUrl.slice(0, -1);
        }

        if ($($(this))[0].href == cUrl) {
            console.log(cUrl);
            $(this).parent().addClass('active');

            $(this).parents('ul').add(this).each(function () {
                //$(this).parent().addClass('open');
                $(this).parent().addClass('open');
            });
        }
    });

    // Dropdown Menu
    $.navigation.on('click', 'a', function (e) {

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
        var interval = setInterval(function () {
            timesRun += 1;
            if (timesRun === 5) {
                clearInterval(interval);
            }
            window.dispatchEvent(new Event('resize'));
        }, 62.5);
    }

    /* ---------- Main Menu Open/Close, Min/Full ---------- */
    $('.navbar-toggler').click(function () {

        if ($(this).hasClass('sidebar-toggler')) {
            $('body').toggleClass('sidebar-hidden');
            if ($("body").hasClass('sidebar-hidden')) {
                $(this).removeClass('activeBtn');
            }
            else {
                $(this).addClass('activeBtn');
            }
            resizeBroadcast();
        }

        if ($(this).hasClass('mobile-sidebar-toggler')) {
            $('body').toggleClass('sidebar-mobile-show');
            resizeBroadcast();
        }

        //for mobile active aside menu button class
        function aciveButononMobile() {
            if ($("body").hasClass('mobile-aside-menu-show')) {
                $('.asideMobileBtn').addClass('activeBtn');
            }
            else {
                $('.asideMobileBtn').removeClass('activeBtn');
            }
        }


        //for aside menu
        if ($(this).hasClass('aside-menu-toggler')) {
            //on click header menu for aside
            if ($(this).hasClass('mobile-aside-menu')) {
                $('body').toggleClass('mobile-aside-menu-show');
                aciveButononMobile();
                resizeBroadcast();
            }
                //on click humberbug menu for aside 
            else {
                if ($('.mobile-aside-menu').hasClass('activeBtn')) {
                    $('body').toggleClass('mobile-aside-menu-show');
                    aciveButononMobile();
                }
                $('body').toggleClass('aside-menu-hidden');
                if ($("body").hasClass('aside-menu-hidden')) {
                    $('.asideBtn').removeClass('activeBtn');
                }
                else {
                    $('.asideBtn').addClass('activeBtn');
                }
                resizeBroadcast();
            }
        }
    });

    //setting button active class
    $('.dropdown-toggle').click(function () {
        if ($(this).attr('aria-expanded') === "false") {
            $('.settingBtn').addClass('activeBtn');
        }
        else {
            $('.settingBtn').removeClass('activeBtn');
        }
    });

    $('.sidebar-close').click(function () {
        $('body').toggleClass('sidebar-opened').parent().toggleClass('sidebar-opened');
    });

    /* ---------- Disable moving to top ---------- */
    $('a[href="#"][data-top!=true]').click(function (e) {
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
    $('[rel="tooltip"],[data-rel="tooltip"]').tooltip({ "placement": "bottom", delay: { show: 400, hide: 200 } });

    /* ---------- Popover ---------- */
    $('[rel="popover"],[data-rel="popover"],[data-toggle="popover"]').popover();

}
