
function showRemainingCharacterTextMessage(element) {
    $(element).next('div.textarea').show();
    var textMax = parseInt($(element).attr('maxlength'));
    var textLength = $("#"+$(element).attr("id")+"").val().length;
    var textRemaining = textMax - textLength;
    $(element).next('div.textarea').html(textRemaining + ' characters remaining');
}

function hideRemainingCharacterTextMessage(maxLength,id) {
    $('div.textarea').hide();
}


