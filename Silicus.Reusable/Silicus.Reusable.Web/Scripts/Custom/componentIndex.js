(function () {
    'use strict';
    function getsearchContain(searchString) {

        var url = "/FrameworxProject/Details";
        $.get(url, { searchString: searchString }, function (data) {

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
                    // Show the list item if the phrase matches and increase the count by 1
                } else {
                    $(this).show();
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

            //$(".category-container").each(function () {
            //    if ($(this).children().children(':visible').length == 0) {
            //        $(this).prev().hide();
            //    } else {
            //        $(this).prev().show();
            //    }
            //});
            
        }

        if (noResult) {
            $('#noResultMsg').show();
        } else {
            $('#noResultMsg').hide();
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
                    '" class="TitleDivTemplate TitleDiv" data-toggle="modal" data-target="#myModal"  onclick="getdetail(' + id + ',' + title + ')" my-data="' + id + '" my-title="'
                    + title + '">' +
                    '<h3 class="maintitle"> ' + title + '</h3>   </div> </div>';
            });

            htmlString += itemHtmlString;

            if (thisCatItems.length) {
                htmlString += '</div> <div id="divTiles" style="display:none"> <input type="text" id="currentTile" name="currentTile" value="" /></div></div>';
            }            

            itemHtmlString = '';
        });
        $('#containerDiv').html('');
        $('#containerDiv').html(htmlString);
    }

    function showListView() {        
        var listItems = $(".TitleDivTemplate").toArray();        
        var htmlString = ' <ul class="list-wrapper">', itemHtmlString = '';
        var filter = $('#searchString').val().trim();
        listItems.forEach(function (value, index) {
            var id = value.attributes['id'].value, title = value.attributes['title'].value, category = value.attributes['category'].value;
            var style = title.search(new RegExp(filter, "i")) < 0? "display: none;":"";
            itemHtmlString += ' <li category="' + category + '" id="' + id + '" title="' + title + '" style="' + style + '">' +
                    '<a class="icon-'+ category+'" data-toggle="modal" data-target="#myModal" onclick="getdetail('+ id+','+ title+')">'+ title+'</a></li>';
        });
        htmlString += itemHtmlString + ' </ul>';
        $('#containerDiv').html('');
        $('#containerDiv').html(htmlString);
    }

    $(document).ready(
        function () {
            var timer;
            $("#searchString").on('keyup', function () {              
                clearTimeout(timer);
                var ms = 1000; // milliseconds
                var val = this.value.trim();
                timer = setTimeout(function () {
                    searchComponent(val);
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
            });

            $('#component-list-view').click(function () {
                $('#component-tile-view').attr('disabled', false);
                $(this).attr('disabled', true);                
                showListView();
            });


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
        }
        );

})();
