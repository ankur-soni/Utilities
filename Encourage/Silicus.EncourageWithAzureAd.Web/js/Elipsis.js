//elipsis
$(function () {
    var showChar = 80, showtxt = "less", hidetxt = "more";
    $('.more').each(function () {
        var content = $(this).text();
        if (content.length > showChar) {
            debugger;
            var con = content.substr(0, showChar);
            var hcon = content.substr(showChar, content.length - showChar);
            var txt = con + '<div class="morecontent"><div style="width:100%;max-width:100%;display: inline-block;word-wrap: break-word;">' + hcon + '</div>&nbsp;&nbsp;<a href="" class="moretxt">' + showtxt + '</a></div>';
            $(this).html(txt);
        }
    });
    $(".moretxt").click(function () {
        debugger;
        if ($(this).hasClass("sample")) {
            $(this).removeClass("sample");
            $(this).text(showtxt);
        } else {
            $(this).addClass("sample");
            $(this).text(hidetxt);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});


 /* or whatever width you want. */
 /* or whatever width you want. */
