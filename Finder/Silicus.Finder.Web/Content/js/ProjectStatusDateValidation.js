function statusUpdate() {
    var status = $("#status").val();
    if (status == 0) {
        $('#startDate').attr("disabled", true);
        $('#expectedEndDate').attr("disabled", true);
        $('#actualEndDate').attr("disabled", true);
        $("#startDate").val('');
        $("#expectedEndDate").val('');
        $("#actualEndDate").val('');
    }
    if (status == 1) {
        $('#startDate').removeAttr("disabled");
        $('#expectedEndDate').removeAttr("disabled");
        $('#actualEndDate').attr("disabled", true);
        $("#actualEndDate").val('');
    }
    if (status == 2) {
        $('#startDate').removeAttr("disabled");
        $('#expectedEndDate').removeAttr("disabled");
        $('#actualEndDate').removeAttr("disabled");
    }
}

$("#status").change(function () {
    statusUpdate();
});

$(function () {
    statusUpdate();
});

$('#CreateEditProjectButton').on('click', function () {

    var startDate = $("#startDate").val();
    var actualEndDate = $("#actualEndDate").val();

    if (Date.parse(startDate) > Date.parse(actualEndDate)) {
        $("#errorMessageActualEndDate").text("Actual End Date should be greater than Start Date");
        $("#actualEndDate").val('');
        $("#actualEndDate").focus();
        return false;
    }

    if (Date.parse(actualEndDate) > new Date()) {
        $("#errorMessageActualEndDate").text("Actual End Date should be smaller than or equals to Today's Date");
        $("#actualEndDate").val('');
        $("#actualEndDate").focus('');
        return false;
    }

    var startDate1 = $("#startDate").val();
    var expectedEndDate = $("#expectedEndDate").val();

    if (Date.parse(startDate1) > Date.parse(expectedEndDate)) {
        $("#errorMessageExpectedEndDate").text("Expected End Date should be greater than Start Date");
        $("#expectedEndDate").val('');
        $("#expectedEndDate").focus('');
        return false;
    }

    //checkStartAndExpectedEndDate();
    //checkStartAndActualEndDate();
});

function checkStartAndExpectedEndDate() {
    var startDate1 = $("#startDate").val();
    var expectedEndDate = $("#expectedEndDate").val();

    if (Date.parse(startDate1) > Date.parse(expectedEndDate)) {
        $("#errorMessageExpectedEndDate").text("Expected End Date should be greater than Start Date");
        $("#expectedEndDate").val('');
        $("#expectedEndDate").focus('');
        return false;
    }
}

function checkStartAndActualEndDate() {
    var startDate = $("#startDate").val();
    var actualEndDate = $("#actualEndDate").val();

    if (Date.parse(startDate) > Date.parse(actualEndDate)) {
        $("#errorMessageActualEndDate").text("Actual End Date should be greater than Start Date");
        $("#actualEndDate").val('');
        $("#actualEndDate").focus();
        return false;
    }

    if (Date.parse(actualEndDate) > new Date())
    {
        $("#errorMessageActualEndDate").text("Actual End Date should be smaller than or equals to Today's Date");
        $("#actualEndDate").focus('');
        $("#actualEndDate").val('');
        return false;
    }
}

$("#actualEndDate").change(function () {
    checkStartAndActualEndDate();
});

$("#expectedEndDate").change(function () {
    checkStartAndExpectedEndDate();
});