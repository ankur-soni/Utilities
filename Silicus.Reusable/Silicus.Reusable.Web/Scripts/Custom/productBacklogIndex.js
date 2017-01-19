function onHrsChange(e) {
    var vm = this;
    var grid = $("#productBacklogs").data("kendoGrid");
    var tr = vm.element.closest('tr'); //get the row for deletion
    var data = grid.dataItem(tr);
    var time = vm.value();
    time = time.toHrs();
    var isAllocatedTime = vm.element.hasClass('allocated-hours');
    var url = isAllocatedTime ? "/ProductBacklog/UpdateTimeAllocated" : "/ProductBacklog/UpdateTimeSpent";    
    $.ajax({
        url: url,
        data: { id: data.Id, time: time },
        success: function (data) {
            vm.value(toHHMM(time));           
        },
        error: function () {
            showAlert({ title: 'Error', text: 'Error occurred while updating allocated time.', type: 'error', timer: 2000 });
        },
        beforeSend: function () {
            blockUI();
        },
        complete: function () {
            unblockUI();
        }
    });
}


var ddlItem;
function additionalData(e) {
    return { projectName: ddlItem }
}
function onProjectChange(e) {
    ddlItem = this.value();
    var grid = $("#productBacklogs").data("kendoGrid");
    grid.dataSource.read();
}

function onProjectDataBound(e) {
    ddlItem = this.value();
    var grid = $("#productBacklogs").data("kendoGrid");
    grid.dataSource.read();
}






//Function to open feedbcak form using ajax call 
function openDetails(id) {
    //Ajax call for controller's OpenFeedbackForm action method to get bulletin details to open dialog 
    blockUI();
    $.ajax({
        url: '/ProductBacklog/WorkItemDetails/',
        data: { id: id },
        success: function (data) {
            //Set feedback form body html with data 
            $("#detailsFormModal .modal-body").html(data);
            //Show feedback form modal
            $('#detailsFormModal').modal('show');
        },
        error: function (e) {
            toastr.error(getErrorMessage(e));
        },
        complete: function () {
            unblockUI();
        }
    });
}

function openAssignUserForm(e) {        
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));    
    if (!parseFloat($(e.currentTarget).closest("tr").find('.allocated-hours').val().toHrs())) {
        showAlert({ title: '', text: 'Please allocate time.', type: 'warning', timer: 2000 });
        return false;
    }
    $("#Assignee").data("kendoDropDownList").value(dataItem.AssigneeDisplayName);
    $("#Assignee").prop('target-elem', $(e.currentTarget));
    $('#assineeFormModal').modal('show');
}

function accept(e) {
    var vm = this;
    var dataItem = vm.dataItem($(e.currentTarget).closest("tr"));
    if (!parseFloat($(e.currentTarget).closest("tr").find('.allocated-hours').val().toHrs())) {
        showAlert({ title: 'Error', text: 'Please allocate time.', type: 'error', timer: 2000 });
        return false;
    }
    $.ajax({
        url: "/ProductBacklog/AcceptworkItem",
        data: {
            AreaPath: dataItem.AreaPath,
            AssigneeDisplayName: dataItem.AssigneeDisplayName,
            Description: dataItem.Description,
            Id: dataItem.Id,
            State: dataItem.State,
            TimeAllocated: dataItem.TimeAllocated,
            TimeSpent: dataItem.TimeSpent,
            Title: dataItem.Title,
            Type: dataItem.Type
        },
        success: function (data) {
            showAlert({ title: 'Accepted successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            dataItem["AssigneeDisplayName"] = data.AssigneeDisplayName;
            vm.refresh();
        },
        error: function () {
            showAlert({ title: 'Error', text: 'Error occurred while accept.', type: 'error', timer: 2000 });
        },
        beforeSend: function () {
            blockUI();
        },
        complete: function () {
            unblockUI();
        }
    });
}

function assignUser() {    
    var grid = $("#productBacklogs").data("kendoGrid");
    var target = $("#Assignee").prop('target-elem');
    var dataItem = grid.dataItem(target.closest("tr"));   
    var email = $("#Assignee").data("kendoDropDownList").value();
    var name = $("#Assignee").data("kendoDropDownList").text();
    dataItem["AssigneeDisplayName"] = name;
    dataItem["AssigneeEmail"] = email;
    $.ajax({
        url: "/ProductBacklog/AssignworkItem",
        data: {
            AreaPath: dataItem.AreaPath,
            AssigneeDisplayName: dataItem.AssigneeDisplayName,
            Description: dataItem.Description,
            Id: dataItem.Id,
            State: dataItem.State,
            TimeAllocated: dataItem.TimeAllocated,
            TimeSpent: dataItem.TimeSpent,
            Title: dataItem.Title,
            Type: dataItem.Type
        },
        success: function (data) {
            showAlert({ title: 'Assigned successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            dataItem["AssigneeDisplayName"] = data.AssigneeDisplayName;
            dataItem["AssigneeEmail"] = email;
            grid.refresh();
        },
        error: function () {
            showAlert({ title: 'Error', text: 'Error occurred while accept.', type: 'error', timer: 2000 });
        },
        beforeSend: function () {
            blockUI();
        },
        complete: function () {
            unblockUI();
        }
    });
}

