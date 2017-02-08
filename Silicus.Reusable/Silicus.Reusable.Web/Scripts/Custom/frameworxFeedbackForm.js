
//Function to open feedbcak form using ajax call 
function openFeedbackForm() {
    //Ajax call for controller's OpenFeedbackForm action method to get bulletin details to open dialog 
    blockUI();
    $.ajax({
        url: '/FrameworxFeedback/OpenFeedbackForm/',
        data: { OwnerId: $('#OwnerId').val(), Title: $('#Title').val(), Id: $("#currentTile").val() },
        success: function (data) {            
            //Set feedback form body html with data 
            $("#feedbackFormModal .modal-body").html(data);
            //Show feedback form modal            
            $('#feedbackFormModal').modal('show');
        },
        error: function (e) {            
            showAlert({ title: 'Error', text: 'Oops!', type: 'error', timer: 2000 });
        },
        complete: function () {
            unblockUI();
        }
    });
}

function openContactOwnerForm() {
    //Ajax call for controller's OpenFeedbackForm action method to get bulletin details to open dialog 
    blockUI();
    $.ajax({
        url: '/FrameworxFeedback/OpenContactOwnerForm/',
        data: { ownerId: $('#OwnerId').val() },
        success: function (data) {
            //Set feedback form body html with data 
            $("#contactOwnerFormModal .modal-body").html(data);
            //Show feedback form modal
            $('#contactOwnerFormModal').modal('show');
        },
        error: function (e) {
            toshowAlert({ title: 'Error', text: 'Oops!', type: 'error', timer: 2000 });
        },
        complete: function () {
            unblockUI();
        }
    });
}
//Function to open feedbcak form using ajax call
function SaveFeedback() {
    //Call rebindValidation() to rebind all validations
    rebindValidation('#frmFeedback');
    //call submit event
    $("#frmFeedback").submit();
}

function OnFailureFeedback(e) {
    showAlert({ title: 'Error', text: 'Error occurred while updating feedback details.', type: 'error', timer: 2000 });
}


function onSuccessFeedback(data) {
    if (data == true) {
        //Hide Feedback form if details have beed saved to database
        $('#feedbackFormModal').modal('hide');
        showAlert({ title: 'Submitted successfully!', text: 'Feedback has been submitted successfully!', type: 'success',timer:2000});
    }    
}

function placholderTxt() {

    if (document.getElementById('Idea').checked) {
        document.getElementById('txtSummary').placeholder = "Describe your idea in 2 words... ";
        document.getElementById("txtDescription").placeholder = "We would love to hear your idea, just type it in...";
    }

    if (document.getElementById('Question').checked) {
        document.getElementById('txtSummary').placeholder = "What's your question?";
        document.getElementById("txtDescription").placeholder = "Don't be afraid, describe your question in full...";
    }

    if (document.getElementById('Problem').checked) {
        document.getElementById('txtSummary').placeholder = "What's your problem?";
        document.getElementById("txtDescription").placeholder = "No worries, just type in your problem and we will take care of it...";
    }

    if (document.getElementById('Praise').checked) {
        document.getElementById('txtSummary').placeholder = "What's your praise?";
        document.getElementById("txtDescription").placeholder = "Let us know your praise...";
    }
}

$(document).ready(function () {
    $('#feedbackFormModal .modal-dialog').addClass("modal-lg");
});


