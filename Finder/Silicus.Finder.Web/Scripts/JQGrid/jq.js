$.lastSelectedRow = null;
$(document).ready(function () {
    $("#tblJQGrid").jqGrid({
        url: 'jqgrid',
        datatype: "json",
        mtype: 'GET',
        colNames: ['UserId', 'FirstName', 'LastName', 'Role', 'Address', 'NewPassword', 'ConfirmPassword', 'isActive', 'Email', 'MembershipId'],
        colModel: [{ name: 'UserId', index: 'UserId',hidden: true, editable: true },
        { name: 'FirstName', index: 'FirstName', editable: true },
        { name: 'LastName', index: 'LastName', editable: true },
        { name: 'Role', index: 'RoleName', align: 'left', editable: true, edittype: 'select', editoptions: { dataUrl: 'Categories' }, editrules: { required: true } },
           { name: 'Address', index: 'Address',  editable: true },
           { name: 'NewPassword', index: 'NewPassword',formatter:form, edittype:"password",  editable: true },
      
        { name: 'ConfirmPassword', index: 'ConfirmPassword',formatter:form, edittype: "password", editable: true },
        { name: 'isActive', index: 'isActive', formatter:"Boolean" , edittype: "checkbox",  editable: true },
        { name: 'Email', index: 'Email', align: "right", editable: true },
       
        { name: 'MembershipId', index: 'MembershipId',hidden: true, editable: true }
        ],
        pager: $('#tblJQGrid'),
        rowNum: 10,
        sortname: 'UserId',
        afterSubmit: function () {
            $(this).jqGrid("setGridParam", { datatype: 'json' });
            return [true];
        },
       
        rowList: [],        // disable page size dropdown
        pgbuttons: false,     // disable page control like next, back button
        pgtext: null,         // disable pager text like 'Page 0 of 10'
        viewrecords: false,
        sortorder: "desc",
        caption: "List Employee Details",
        scrollOffset: 0
    });
    function form(cellvalue, options, rowObject) {
        return "********";
    };
    
  
    $('#tblJQGrid').jqGrid('navGrid', '#tblJQGrid',
               { add: true, del: true, edit: true, search: false },
               { width: 'Auto', url: 'UpdateUser' },
               { width: 'Auto', url: 'CreateUser' },
               { width: 'Auto', url: 'DeleteUser' });
   });
 