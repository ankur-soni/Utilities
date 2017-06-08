    $("#pinnacle-tab").on("click", function () {
        debugger;
        var awardId = +$(this).attr("data-awardId");
        showCustomLoader();
        $.ajax({
            type: "GET",
            url: "",
            data: { awardId: awardId },
        success: function (response) {
            hideCustomLoader();
            $("#pinnacle #pinnacle-currentMonth").html(response);
        },
        error: function (error) {
            hideCustomLoader();
            console.log(error);
        }
    });
});
