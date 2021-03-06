﻿(function () {
    'use strict';
    var listItemHeight = 18;    
    function getsearchContain(searchString) {

        var url = "/FrameworxProject/Details";
        $.get(url, { searchString: searchString }, function (data) {

        });

    }

    function nextSlide() {
        var selects = $(".TitleDivTemplate");
        selects.each(function myfunction(index) {
            var id = $(this).attr("my-data");
            var currentId = $("#currentTile").val();
            if (id == currentId) {
                if (index !== selects.length - 1) {
                    // select the next span
                    getComponentdetail(selects.eq(index + 1).attr('my-data'), selects.eq(index + 1).attr('my-title'));
                    return false;
                } else {
                    getComponentdetail(selects.eq(0).attr('my-data'), selects.eq(index + 1).attr('my-title'));
                }
            }
        });
    }

    function prevSlide() {
        var selects = $(".TitleDivTemplate");
        selects.each(function myfunction(index) {

            var id = $(this).attr("my-data");
            var currentId = $("#currentTile").val();
            if (id == currentId) {
                if (index !== selects.length - 1) {
                    // select the next span
                    getComponentdetail(selects.eq(index - 1).attr('my-data'), selects.eq(index - 1).attr('my-title'));
                    return false;
                } else {
                    getComponentdetail(selects.eq(index - 1).attr('my-data'), selects.eq(index - 1).attr('my-title'));
                }
            }
        });
    }


    function searchComponent(value) {
        var filter = value;
        var viewType = $('#component-list-view').is(':disabled') ? 'list' : 'tile';
        var noResult = true;

        if (viewType == 'list') {
            // Loop through the comment list
            $(".list-wrapper>li").each(function () {

                // If the list item does not contain the text phrase fade it out
                if ($(this).text().search(new RegExp(filter, "i")) < 0) {
                    $(this).fadeOut();
                    $(this).addClass('no-found');
                    $(this).removeClass('found');
                    // Show the list item if the phrase matches and increase the count by 1
                } else {
                    $(this).show();
                    $(this).removeClass('no-found');
                    $(this).addClass('found');
                    noResult = false;
                }
            });
        } else {
            // Loop through the comment list
            $(".TitleDivTemplate").each(function () {
                // If the list item does not contain the text phrase fade it out
                if ($(this).text().search(new RegExp(filter, "i")) < 0) {
                    $(this).parent().fadeOut();
                    // Show the list item if the phrase matches and increase the count by 1
                } else {
                    $(this).parent().show();
                    noResult = false;
                }
            });
        }

        if (noResult) {
            $('#noResultMsg').show();
            $('.categoryHeading').hide();
        } else {
            $('#noResultMsg').hide();
            $('.categoryHeading').show();
        }
    }

    function showTileView() {
        var listItems = $(".list-wrapper>li").toArray();
        var categories = listItems.map(function (a) { return a.attributes['category'].value; });
        var filter = $('#searchString').val().trim();

        categories = categories.getUnique();
        var htmlString = '';
        var itemHtmlString = '';
        categories.forEach(function (category, index) {
            htmlString += '<div class="col-sm-12 col-lg-12 col-md-12 col-xs-12"><div> <h3 class="componentHeading categoryHeading">' + category + '</h3> </div></div>';
            var thisCatItems = listItems.filter(function (a) { return a.attributes['category'].value === category });
            if (thisCatItems.length) {
                htmlString += '<div class="container category-container"> <div class="row">';
            }
            thisCatItems.forEach(function (value, index) {
                var id = value.attributes['id'].value, title = value.attributes['title'].value;
                var style = title.search(new RegExp(filter, "i")) < 0 ? "display: none;" : "";
                itemHtmlString += '<div class="col-sm-4" style="' + style + '"> <div category="' + category + '" id="' + id + '" title="' + title +
                    '" class="TitleDivTemplate TitleDiv"  my-data="' + id + '" my-title="'
                    + title + '">' +
                    '<h3 class="maintitle"> ' + title + '</h3>   </div> </div>';
            });

            htmlString += itemHtmlString;

            if (thisCatItems.length) {
                htmlString += '</div></div>';
            }

            itemHtmlString = '';
        });
        $('#containerDiv').html('');
        $('#containerDiv').html(htmlString);
    }

    function showListView() {
        var listItems = $(".TitleDivTemplate").toArray();
        var htmlString = ' <ul class="list-wrapper" id="list-view-wrapper">', itemHtmlString = '';
        var filter = $('#searchString').val().trim();
        listItems.forEach(function (value, index) {
            var id = value.attributes['id'].value, title = value.attributes['title'].value, category = value.attributes['category'].value;
            var style = title.search(new RegExp(filter, "i")) < 0 ? "display: none;" : "";
            var className = title.search(new RegExp(filter, "i")) < 0 ? "not-found" : "found";
            itemHtmlString += ' <li category="' + category + '" id="' + id + '" title="' + title + '" class="' + className + '" style="' + style + '">' +
                    '<a class="icon-' + category + ' TitleDivTemplate"  my-data="' + id + '" my-title="'
                    + title + '">' + title + '</a></li>';
        });
        htmlString += itemHtmlString + ' </ul>';
        $('#containerDiv').html('');
        $('#containerDiv').html(htmlString);
    }

    function getComponentdetail(id, showModel) {
        $("#currentTile").val(id);
        blockUI();
        $.ajax({
            url: "/FrameworxProject/Details",
            type: "get",
            data: { id: id },
            success: function (data) {
                $(".carousel-inner").html(data);
                if (showModel) {
                    $('#component-modal').modal('show');
                }
            },
            error: function () {
                showAlert({ title: 'Error', text: 'Error occurred while getting details.', type: 'error', timer: 2000 });
            },
            complete: function () {
                unblockUI();
            }
        });
    }

    function likeComponent() {
        blockUI();
        $.get("/FrameworxProject/LikeComponent", { componentId: $("#currentTile").val() }, function (data) {
            $('#like-count').text(parseInt($('#like-count').text()) + 1);
            $('#like-text').text('Unlike');
            $('.like').addClass('liked');
            $("#LikeId").val(data.likeId);
            showAlert({ title: 'Liked', text: '', type: 'success', timer: 2000 });
        }).done(function () {
            unblockUI();
        });
    }


    function unLikeComponent() {
        blockUI();
        $.get("/FrameworxProject/UnLikeComponent", { likeId: $("#LikeId").val() }, function (data) {
            $('#like-count').text(parseInt($('#like-count').text()) - 1);
            $('#like-text').text('Like');
            $('.like').removeClass('liked');
            showAlert({ title: 'Unliked', text: '', type: 'success', timer: 2000 });
        }).done(function () {
            unblockUI();
        });
    }

    function showCredits() {
        showAlert({
            title: 'Contributors',
            text: "If you like the component, thank these people \n \n" + $("#Credits").val() + '\n',
            type: null,
            showConfirmButton: true
        });
    }

    function hideCategoryText() {
        $('.category-container').each(function () {
            if ($(this).find('.row').children(':visible').length == 0) {
                $(this).prev().find('.categoryHeading').hide();
            } else {
                $(this).prev().find('.categoryHeading').show();
            }
        });
    }

    // get the available width inside an element - can convert % width in pixels!
    function getInnerWidth(elem) {
        return parseFloat(window.getComputedStyle(elem).width);
    }

    // get the total width of borders, padding and margin - basically anything but the inner width
    function getAirWidth(elem) {
        return elem.offsetWidth - getInnerWidth(elem);
    }

    // set the total width of an element (including padding, border and margin)
    function setTotalWidth(elem, width) {
        elem.style.width = (width - getAirWidth(elem)) + "px";
    }


    // split a list into columns (scalable size)
    function splitColumnsPercent(list, nbColumns, itemName) {
        if (list) {
            list.style.position = "relative";
            var items = list.getElementsByTagName(itemName || "li");

            var colHeight = [];
            for (var i = 0; i < nbColumns; ++i) colHeight[i] = 0;

            for (var i = 0; i < items.length; ++i) {
                var col = colHeight.indexOf(Math.min.apply(Math, colHeight));
                items[i].style.position = "absolute";
                items[i].style.left = (100 * col / nbColumns) + "%";
                items[i].style.top = colHeight[col] + "px";
                items[i].style.width = "calc(100% / " + nbColumns + " - " + getAirWidth(items[i]) + "px)";
                colHeight[col] += items[i].offsetHeight;
            }

            list.style.height = Math.max.apply(Math, colHeight) + "px";
        }
    }

    // split a list into columns (fixed size)
    function splitColumnsFixed(list, nbColumns, itemName) {
        if (list) {
            list.style.position = "relative";
            var items = typeof itemName != 'undefined' ? list.getElementsByClassName(itemName) : list.getElementsByTagName("li");

            var width = getInnerWidth(list) / nbColumns;

            var colHeight = [];
            for (var i = 0; i < nbColumns; ++i) colHeight[i] = 0;
            for (var i = 0; i < items.length; ++i) {
                var col = colHeight.indexOf(Math.min.apply(Math, colHeight));
                items[i].style.position = "absolute";
                items[i].style.left = width * col + "px";
                items[i].style.top = colHeight[col] + "px";
                setTotalWidth(items[i], width);
                colHeight[col] += items[i].offsetHeight;
            }

           // list.style.height = Math.max.apply(Math, colHeight) + "px";
        }
    }


    $(document).ready(
        function () {
            var timer;
            $("#searchString").on('keyup', function () {
                clearTimeout(timer);
                var ms = 1000; // milliseconds
                var val = this.value.trim();
                timer = setTimeout(function () {
                    blockUI();
                    searchComponent(val);
                    splitColumnsFixed(document.getElementById("list-view-wrapper"), Math.ceil(($('.found').length * listItemHeight) / $('#container-box').innerHeight()), 'found');
                    setTimeout(function () {
                        hideCategoryText();
                    }, ms);                   
                    unblockUI();
                }, ms);
            });


            $('#searchString').on('input propertychange', function () {
                var $this = $(this);
                var visible = Boolean($this.val());
                $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
            }).trigger('propertychange');

            $('.form-control-clear').click(function () {
                $(this).siblings('input[type="text"]').val('')
                  .trigger('propertychange').trigger('keyup').focus();
            });


            $('#component-tile-view').click(function () {
                $('#component-list-view').attr('disabled', false);
                $(this).attr('disabled', true);
                showTileView();
                $('#container-box').height("100%");
                if ($('#noResultMsg').is(":visible")) {
                    $('.categoryHeading').hide();
                } else {
                    $('.categoryHeading').show();
                    hideCategoryText();
                }
            });

            $('#component-list-view').click(function () {
                $('#component-tile-view').attr('disabled', false);
                $(this).attr('disabled', true);
                showListView();
                $('#container-box').height("680px");
                splitColumnsFixed(document.getElementById("list-view-wrapper"), Math.ceil(($('.TitleDivTemplate').length * listItemHeight) / $('#container-box').innerHeight()));
                splitColumnsFixed(document.getElementById("list-view-wrapper"), Math.ceil(($('.found').length * listItemHeight) / $('#container-box').innerHeight()), 'found');               
            });

            $('body').on('click', '.TitleDivTemplate', function () {
                var id = $(this).attr('my-data');
                getComponentdetail(id, true);
            });

            $('#prev-slide-button').on('click', function () { prevSlide(); });
            $('#next-slide-button').on('click', function () { nextSlide(); });

            $('body').on('click', '.like', function () {
                var text = $('#like-text').text();
                if (text == "Like") {
                    likeComponent();
                } else {
                    unLikeComponent();
                }
            });

            $('body').on('click', '#btn-credits', function () {
                showCredits();
            });
                        
            splitColumnsFixed(document.getElementById("list-view-wrapper"), Math.ceil(($('.TitleDivTemplate').length * listItemHeight) / $('#container-box').innerHeight()));

        }
        );

})();
