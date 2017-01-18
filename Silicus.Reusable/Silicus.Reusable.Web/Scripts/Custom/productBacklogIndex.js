function onHrsChange(e){    
    var vm = this;
    var grid = $("#productBacklogs").data("kendoGrid");
    var tr = vm.element.closest('tr'); //get the row for deletion
    var data = grid.dataItem(tr);
    var time = vm.value();
    time = time.toHrs();
    var url  = this.element.hasClass('allocated-hours') ?"/ProductBacklog/UpdateTimeAllocated":"/ProductBacklog/UpdateTimeSpent";
    $.ajax({
        url: url,
        data:{ id: data.Id,time:time },
        success: function (data) {
            vm.value(toHHMM(time));
        },
        error:function(){
            showAlert({ title: 'Error', text: 'Error occurred while updating allocated time.', type: 'error', timer: 2000 });
        },
        beforeSend:function(){
            blockUI();
        },
        complete: function () {
            unblockUI();
        }
    });
}


var ddlItem;
function additionalData(e) {
    return { projectName : ddlItem }
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



function Accept(e){
    //var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    //$.get( "/ProductBacklog/UpdateAssignee", { id: dataItem.Id} );
}