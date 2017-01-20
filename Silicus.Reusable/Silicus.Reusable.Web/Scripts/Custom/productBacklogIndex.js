function onHrsChange(e) {
    var vm = this;
    var grid = $("#productBacklogs").data("kendoGrid");
    var tr = vm.element.closest('tr'); //get the row for deletion
    var dataItem = grid.dataItem(tr);
    var time = vm.value();    
    var isAllocatedTime = vm.element.hasClass('allocated-hours');
    var propName = isAllocatedTime ? "TimeAllocated" : "TimeSpent";
    var url = isAllocatedTime ? "/ProductBacklog/UpdateTimeAllocated" : "/ProductBacklog/UpdateTimeSpent";
    dataItem[propName] = time.toHrs();
    dataItem[propName+ "String"] = toHHMM(time);
    $.ajax({
        url: url,
        dataType: "json",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {
            debugger;
            dataItem = data.result;
            grid.refresh();
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
    if (!dataItem.TimeAllocated) {
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
    if (!dataItem.TimeAllocated) {
        showAlert({ title: '', text: 'Please allocate time.', type: 'warning', timer: 2000 });
        return false;
    }
    $.ajax({
        url: "/ProductBacklog/AcceptworkItem",
        dataType: "json",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {
            debugger;
            showAlert({ title: 'Accepted successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            debugger;
            dataItem = data.result;            
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
        dataType: "json",
        type:'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {            
            showAlert({ title: 'Assigned successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            dataItem = data.result;
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

function update(e) {
    debugger;
    var $target = $(e.currentTarget).closest("tr").find('.spent-hours');
    this.editCell($target);
}

