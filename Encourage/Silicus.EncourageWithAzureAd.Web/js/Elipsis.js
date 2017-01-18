//elipsis
$(function () {
    $(document).ready(function () {
        var showChar = 80, showtxt = "less", hidetxt = "more";
        $('.more').each(function () {
            var content = $(this).text();
            if (content.length > showChar) {
                
                var con = content.substr(0, showChar);
                var hcon = content.substr(showChar, content.length - showChar);
                var txt = con + '<div class="morecontent">' +
                        '<span id="wholeContent" style="display:none">' + content.trim() + '</span>' +
                        '<div style="width:100%;max-width:100%;display: none;word-wrap: break-word;">' + hcon + '</div>' +
                        '<a href="" id="moretxt" class="moretxt" onclick="showCommentInPopup(this);" data-toggle="modal" data-target="#wholeCommentBox">' + hidetxt + '</a>' +
                     '</div>';
                $(this).html(txt);
            }
        });
    });    
});


 /* or whatever width you want. */
 /* or whatever width you want. */
