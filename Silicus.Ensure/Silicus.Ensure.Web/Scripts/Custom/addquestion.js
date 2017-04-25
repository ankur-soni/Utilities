
window.onbeforeunload = function () {
    return "Do you really want to close?";
};

$('#btnSave').click(function()
{
    window.onbeforeunload = null;
    $(window).unbind('unload');
});

$('#btnSaveAndAddNewQuestion').click(function()
{
    window.onbeforeunload = null;
});

function isAnySelectedAns(input)
{
    $('#IsAnsOption1_validationMessage').appendTo('#answerDiv');
    var qType = $('#QuestionType').val();
    if (input.is("[name=IsAnsOption1]") && qType == 1) {
        input.attr("data-IsAnsOption1-msg", "Please select at least single answer.");
        return $(".ansCheckBox:checked").length>0;
    }
    return true;
}
function validateOtherOptions(input, params,optionNo)
{
    var qType = $('#QuestionType').val();
    if (input.is("[name=Option"+optionNo+"]") && qType == 1 && input.is(".otherOption")) {
        input.attr("data-Option"+optionNo+"-msg", "Option "+optionNo+" can't be blank.");
        return input.val().trim()!="";
    }
    return true;
}
function validateOptions(input, params,optionNo)
{
    var qType = $('#QuestionType').val();
    if (input.is("[name=Option"+optionNo+"]") && qType == 1) {
        input.attr("data-Option"+optionNo+"-msg", "Option "+optionNo+" can't be blank.");
        return input.val().trim()!="";
    }
    return true;
}
function OnSubmit(e) {
    window.onbeforeunload = null;
    var dropdownlist = $("#QuestionType").data("kendoDropDownList");
    dropdownlist.enable(true);
}

// Show Error Animation
function ShowError(content) {
    $("#errorDiv").show();
    $("#errorContent").text(content);
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $("#errorDiv").fadeOut(10000);
}

$('#QuestionType').change(function() {
    var skillTag = $('#SkillTag').data("kendoMultiSelect");
    skillTag.value([]);
});

// Question Type Change - > Set default values of controls.
function QuestionTypeChange() {

    var value = $("#QuestionType").val();
    if (value == "2") {
        $("#practical").hide();
        $("#practicalAns").show();
        $("#durationRequired").show();
    } else {
        $("#practical").show();
        $("#practicalAns").hide();
        $("#durationRequired").hide();

    }

    $("#Option1").val("");
    $("#Option2").val("");
    $("#Option3").val("");
    $("#Option4").val("");
    $("#Option5").val("");
    $("#Option6").val("");
    $("#Option7").val("");
    $("#Option8").val("");
    var answer = $("#Answer").data("kendoEditor");
    //answer.value("");
    //var correctAnswer = $('#CorrectAnswer').data("kendoMultiSelect");
    //correctAnswer.value([]);
    $(".ansCheckBox").prop("checked",false);

    var answerType = $("#AnswerType");
    answerType.val("1");
}

function OnCancel() {
    window.onbeforeunload = null;
    $(window).unbind('unload');
    var success="@Model.Success";
    window.location.href = "/QuestionBank/QuestionBank";
}

// On Click Of "Add" button of any Option
function OptionAdd() {
    var param = this.element.attr("param");
    OptionDispaly('A', param);
}

// On Click Of "Delete" button of any Option
function OptionDelete() {
    var param = this.element.attr("param");
    $.when(showConfirmationWindow('Are you sure,  you want to delete this record?', '&nbsp;Delete')).then(function (confirmed) {
        if (confirmed) {
            OptionDispaly('D', param);
        }
    });
}

// Display Option Method For "Add" and "Delete" button Click.
function OptionDispaly(task, opt) {
    var op = parseInt(opt);
    if (task == 'A') {
        $("#OptionCount").val(op + 1);
        var opCnt = parseInt($("#OptionCount").val());
        if (op == 2 || op == 7) {
            $("#btnAddOpt" + op).hide();
        }
        for (var i = op; i <= op + 1; i++) {
            $("#rowOpt" + i).show();
            $("#Option" + opCnt).addClass('otherOption');
            $("#btnDelOpt" + i).show();
        }
        if (op != 7) {
            for (var i = op + 2; i <= 8; i++) {
                $("#rowOpt" + i).hide();
                $("#btnAddOpt" + op).hide();
            }
        }
    } else {
        var opCnt = parseInt($("#OptionCount").val());
        $("#IsAnsOption" + opCnt).prop("checked",false);
        $("#Option" + opCnt).val("");
        $("#Option" + opCnt).removeClass('otherOption');
        ;
        $("#rowOpt" + opCnt).hide();
        $("#btnAddOpt" + (opCnt - 1)).show();
        $("#OptionCount").val(opCnt - 1);
    }

}

// Display Option On Edit Question
function OptionDispalyOnEdit(opt) {
    var op = parseInt(opt);
    if (op > 2) {
        $("#btnAddOpt2").hide();
        for (var i = 3; i <= op; i++) {
            $("#rowOpt" + i).show();
            $("#btnDelOpt" + i).show();

            if (i != op)
                $("#btnAddOpt" + i).hide();
        }
    }
}

function saveTag()
{
    if($("#tagForm").valid())
    {
        $.ajax({
            type: "POST",
            url: '/Tag/Save',
            data: $('#tagForm').serialize(),
            success: function (returndata) {
                $("#addTagPopup").modal('hide');
                var $tagMultiSelect=$('#SkillTag').data('kendoMultiSelect');
                var multidata=$tagMultiSelect.dataSource.data();
                multidata.unshift({ TagName: returndata.TagName, TagId: returndata.TagId});
                var selectedValues=$tagMultiSelect.value().slice();
                selectedValues.push(returndata.TagId);
                $tagMultiSelect.value(selectedValues);
                $("#tagForm").trigger("reset");
            }
        });
    }
    else
    {
        $('#tagForm').submit();
    }
}

function AddTag()
{
    // $("#tagForm").kendoValidator();
    $("#tagForm").kendoValidator({
        rules: {
            tagNamevalidation: function (input, params) {
                if (input.is("[name='TagName']") && input.val() != "") {
                    input.attr("data-tagNamevalidation-msg", "Tag name already exists.");
                    return isTagNameAvailable("", input.val());
                }

                return true;
            }
        }
    });
    $('#TagName').val("");
    $('#Description').val("");
    $("#addTagPopup").modal('show');
}

function isTagNameAvailable(existingName,updatedName)
{
    var isAvailable = true;
    if (existingName != updatedName) {
        $.ajax({
            type: "POST",
            url: '/Tag/IsDuplicateTagName',
            async: false,
            data: { existingTagName: existingName, tagName: updatedName },
            success: function (returndata) {
                if (returndata == true) {
                    isAvailable = true;
                }
                else {
                    isAvailable = false;
                }
            }
        });
    }
    return isAvailable;
}