$(document).ready(function () {

    $("#websitePath").hide();
   
    //enable cross-domain calls
    jQuery.support.cors = true

    //$('.datePicker').datepicker()

    $('.nav li').click(function (e) {

        //remove all "active"
        $('.nav li').each(function (index) {
            if ($(this).hasClass('active'))
                $(this).removeClass('active');
        });

        //add "active" to selected menu item

        if (!$(this).hasClass('active')) {
            $(this).addClass('active');
        }
        //e.preventDefault();
    });

});