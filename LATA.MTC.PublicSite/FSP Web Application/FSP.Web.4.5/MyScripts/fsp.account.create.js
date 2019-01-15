$(function () {

    $("#accountRegisterForm").validate({
        rules: {
            "Email": {
                required: true,
                email: true,
                minlength: 2
            },
            "FirstName": {
                required: true,
                minlength: 2
            },
            "LastName": {
                required: true,
                minlength: 2
            },
            "Password": {
                required: true,
                minlength: 7
            },
            "ConfirmPassword": {
                required: true,
                minlength: 7,
                equalTo: '#Password'
            }
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element)
            error.addClass('error');  // add a class to the wrapper       
        }

    });

});