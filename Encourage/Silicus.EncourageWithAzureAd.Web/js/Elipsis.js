//elipsis
$(function () {
    var showChar = 80, showtxt = "less", hidetxt = "more";
    $('.more').each(function () {
        var content = $(this).text();
        if (content.length > showChar) {
            debugger;
            var con = content.substr(0, showChar);
            var hcon = content.substr(showChar, content.length - showChar);
            var txt = con + '<div class="morecontent"><div style="width:100%;max-width:100%;display: none;word-wrap: break-word;">' + hcon + '</div><a href="" class="moretxt">' + hidetxt + '</a></div>';
            $(this).html(txt);
        }
    });
    $(".moretxt").click(function () {
        debugger;
        if ($(this).hasClass("sample")) {
            $(this).removeClass("sample");
            $(this).text(hidetxt);
        } else {
            $(this).addClass("sample");
            $(this).text(showtxt);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});


 /* or whatever width you want. */
 /* or whatever width you want. */
