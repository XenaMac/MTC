$(document).ready(function () {

    $("#websitePath").hide();

    //enable cross-domain calls
    jQuery.support.cors = true

    //$('.datePicker').datepicker()

    //$('.nav li').click(function (e) {

    //    //remove all "active"
    //    $('.nav li').each(function (index) {
    //        if ($(this).hasClass('active'))
    //            $(this).removeClass('active');
    //    });

    //    //add "active" to selected menu item

    //    if (!$(this).hasClass('active')) {
    //        $(this).addClass('active');
    //    }
    //    //e.preventDefault();
    //});

    function checkForAlerts() {
        var url = $("#websitePath").attr("data-websitePath") + '/Truck/HaveAlarms';
        $.get(url,
                function (value) {
                    if (value === true) {
                        //$("#alertScreenLink").attr('class', 'btn btn-danger');
                        $("#alertScreenLink").css('color', 'red');
                        $("#monitoringTab").css('color', 'red');
                        $("#alertMenuItem").css('color', 'red');
                    }
                    else {
                        //$("#alertScreenLink").attr('class', 'btn btn-info');
                        $("#alertScreenLink").css('color', 'white');
                        $("#monitoringTab").css('color', '#999999');
                        $("#alertMenuItem").css('color', 'black');
                    }

                }, "json");
    }
    checkForAlerts();
    setTimeout(function checkForAlertsFunction() {
        console.log('Checking for Alerts for red buttons');
        checkForAlerts();
        setTimeout(checkForAlertsFunction, 10000);
    }, 10000);


});