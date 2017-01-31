function onHrsChange(e) {    
    var grid = $("#productBacklogs").data("kendoGrid");
    var tr = $(this).closest('tr');  //get the row for deletion
    var dataItem = grid.dataItem(tr);    
    var isAllocatedTime = $(this).hasClass('allocated-hours');
    var propName = isAllocatedTime ? "TimeAllocated" : "TimeSpent";
    var url = isAllocatedTime ? "/ProductBacklog/UpdateTimeAllocated" : "/ProductBacklog/UpdateTimeSpent";
    dataItem[propName] = $(this).val();
    $.ajax({
        url: url,
        dataType: "json",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {
            for (var key in data.result) {
                if (data.result.hasOwnProperty(key)) {
                    dataItem[key] = data.result[key];
                }
            }
            grid.refresh();
        },
        error: function () {
            showAlert({ title: 'Error', text: 'Error occurred while updating  time.', type: 'error', timer: 2000 });
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
        url: '/ProductBacklog/Details/',
        data: { id: id },
        success: function (data) {
            //Set feedback form body html with data 
            $("#detailsFormModal .modal-body").html(data);
            //Show feedback form modal
            $('#detailsFormModal').modal('show');
        },
        error: function (e) {
           showAlert({ title: 'Error', text: 'Oops!', type: 'error', timer: 2000 });
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
    $("#Assignee").data("kendoDropDownList").value(dataItem.AssigneeEmail);
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
            showAlert({ title: 'Accepted successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            for (var key in data.result) {
                if (data.result.hasOwnProperty(key)) {
                    dataItem[key] = data.result[key];
                }
            }
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
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {
            showAlert({ title: 'Assigned successfully!', text: 'The backlog has been accepted successfully!', type: 'success', timer: 2000 });
            for (var key in data.result) {
                if (data.result.hasOwnProperty(key)) {
                    dataItem[key] = data.result[key];
                }
            }
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

function openUpdateForm(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    $('#update-time').val(dataItem.TimeSpent);
    $("#update-time").prop('target-elem', $(e.currentTarget));

    //$("#update-time").kendoMaskedTextBox({
    //    mask: "00 : 00"
    //});

    $('#updateFormModal').modal('show');
}

function update() {
    var grid = $("#productBacklogs").data("kendoGrid");
    var target = $("#update-time").prop('target-elem');
    var dataItem = grid.dataItem(target.closest("tr"));
    var time = $('#update-time').val();
    var url = "/ProductBacklog/UpdateTimeSpent";
    dataItem["TimeSpent"] = time;
    $.ajax({
        url: url,
        dataType: "json",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataItem),
        success: function (data) {
            showAlert({ title: '', text: 'Time spent has been updated successfully!', type: 'success', timer: 2000 });
            for (var key in data.result) {
                if (data.result.hasOwnProperty(key)) {
                    dataItem[key] = data.result[key];
                }
            }
            grid.refresh();

        },
        error: function () {
            showAlert({ title: 'Error', text: 'Error occurred while updating spent time.', type: 'error', timer: 2000 });
        },
        beforeSend: function () {
            blockUI();
        },
        complete: function () {
            unblockUI();
        }
    });
}

