//Validations for Alphbets
function alphabets(id)
{
    $(id).keydown(function (e) {
        if (e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;
            
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key == 9))) {
                e.preventDefault();
            }
        }
    });
}

//validation for Numbers
function numeric(id)
{
    $(id).keydown(function (e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;
            if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key == 9))) {
                e.preventDefault();
            }
        }
    });
}

//validation for Alphanumeric
//function alphanumeric(id)
//{   
//    $(id).keydown(function (e) {

//        function alphanumeric(id) {
//            $(id).keydown(function (e) {

//                var regex = new RegExp("^[a-zA-Z0-9]+$");
//                var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
//                if (!regex.test(key)) {
//                    e.preventDefault();
//                    return false;
//                }
               
//            });
//        }

//    });
//}

function alphanumeric(id) {
    $(id).keydown(function (e) {

        if (e.shiftKey || e.ctrlKey || e.altKey) {
            e.preventDefault();
        }

        else {

            var key = e.keyCode;
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key == 9) || (key == 16))) {
                e.preventDefault();
            }          
        }
    });
}
