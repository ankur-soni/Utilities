function onHrsChange(e) {
    var vm = this;
    var grid = $("#productBacklogs").data("kendoGrid");
    var tr = vm.element.closest('tr'); //get the row for deletion
    var data = grid.dataItem(tr);
    var time = vm.value();
    time = time.toHrs();
    var url = this.element.hasClass('allocated-hours') ? "/ProductBacklog/UpdateTimeAllocated" : "/ProductBacklog/UpdateTimeSpent";
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



function Accept(e) {    
    var vm = this;
    var dataItem = vm.dataItem($(e.currentTarget).closest("tr"));
    $.ajax({
        url: "/ProductBacklog/AcceptworkItem",
        data: {
            AreaPath: dataItem.AreaPath,
            Assignee: dataItem.Assignee,
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
            dataItem["Assignee"]= data.Assignee;
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