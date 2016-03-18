// Common js for Project Tracker

$(document).ready(function () {

    $("#btnCreateProject").click(function (e) {

        $('#btnCreateProject').hide();

        $.ajax({
            url: "/Admin/CreateProject",
            contentType: 'application/json; charset=utf-8',
            type: 'Get',
            dataType: 'html'
        })
        .success(function (result) {
            //alert(JSON.stringify(result));
            $('#dashboardContainerDiv').html(result);
        })
        .error(function (xhr, status) {
            //alert(JSON.stringify(xhr));
            $("div.overlay").hide();
        });
    });    

    $('.LeftnavTab').click(function () {
        $('.LeftnavTab').removeClass('active');
        $(this).addClass('active');
        return false;
    });

    $('#projectSummarylink').click(function () {
        RedirectToPage("userProject", "User");
    });

    $('.projectlink').click(function () {
        RedirectToPage("adminProject", "Admin");
    });

    $('.userdashboardlink').click(function () {
        RedirectToPage("dashboard", "User");
    });

    $('.admindashboardlink').click(function () {
        RedirectToPage("dashboard", "Admin");
    });

    $('.mappinglink').click(function () {
        RedirectToPage("mapping", "Admin");
    });  
   
    $('.statusByUserlink').click(function () {
        RedirectToPage("userlist", "Admin");
    });

    $('.uploadlink').click(function () {
        //alert('ok');
        RedirectToPage("upload", "User");
    });

});

function RedirectToPage(pageName, controllerName) {
    $.ajax({
        url: "/" + controllerName + "/LoadPage?pageName=" + pageName,
        type: 'GET',
        cache: false,
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $("#dashboardContainerDiv").html(response);

            if ((pageName == "adminProject" || pageName == "userProject") && (controllerName != "User")) {
                $('#btnCreateProject').show();
            }
            else {
                $('#btnCreateProject').hide();
            }
        },
        error: function (e) {
            //console.log(JSON.stringify(e));
        }
    });
}

function EditProject(projectid) {

    $.ajax({
        url: "/Admin/EditProject?projectId=" + projectid,
        contentType: 'application/json; charset=utf-8',
        type: 'Get',
        dataType: 'html'
        //data: JSON.stringify({ projectId: projectid }),
    })
    .success(function (result) {
        
        $('#btnCreateProject').hide();

        $('#dashboardContainerDiv').html(result);
    })
    .error(function (xhr, status) {
        $("div.overlay").hide();
    });
}

function OpenProjectDetails(projectid)
{
    $.ajax({
        url: "/User/ProjectDetailById?projectId=" + projectid,
        contentType: 'application/json; charset=utf-8',
        type: 'Get',
        //dataType: 'html'
    })
   .success(function (result) {
       //$('#btnCreateProject').hide();

       $('#dashboardContainerDiv').html(result);
   })
   .error(function (xhr, status) {
       $("div.overlay").hide();
   });
}

function OpenProjectDetailsByWeek(projectid, WeekIdToRedirect) {
    
    $.ajax({
        url: "/User/ProjectDetailByIdAndWeek?projectId=" + projectid + "&weekNumber=" + WeekIdToRedirect,
        contentType: 'application/json; charset=utf-8',
        type: 'Get'
    })
   .success(function (result) {
       getContentTab(1, projectid, WeekIdToRedirect)
       $('#dashboardContainerDiv').html(result);
   })
   .error(function (xhr, status) {
       $("div.overlay").hide();
   });
}

function additionalInfo() {
    var weekid = $("#getWeekControl").val();
    if (typeof (weekid) == null || typeof (weekid) == "" || weekid == "") {
        weekid = 0;
    }
    return {
        projectId: $("#hdnProjectId").val(),
        WeekId: weekid
     }
}

function SendUserName() {
    return {
        username: $("#hdnUserName").val()
    }   
}

function getContentTab(index, Projectid, WeekId) {

    var targetDiv = "#tabs-" + index;
    var projectId = Projectid;

    //Current weekid
    var weekid = WeekId;

    //alert(weekid + " : weekid");   

    $.ajax({
        url: "/User/getAjaxTab?tabId=" + index + "&ProjectId=" + projectId + "&Weekid=" + weekid,
        contentType: 'application/json; charset=utf-8',
        type: 'Get',

    })
    .success(function (result) {
        $(targetDiv).html(result);

        //once the user visits the tab ,mark it as true to stop ajax requests till the week is changed
        tabsPosted(index, false);
    })
    .error(function (xhr, status) {
        $(targetDiv).html("Error While Loading Data");
    });
}

