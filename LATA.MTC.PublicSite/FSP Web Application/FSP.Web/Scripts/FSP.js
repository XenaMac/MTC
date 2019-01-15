

$(document).ready(function () {


    $("#websitePath").hide();

    //#region
    //enable cross-domain calls
    jQuery.support.cors = true

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

//    //user tickets
//    var userTable = $('.tablesorter').dataTable({
//        "sDom": '<"clear">f<"top"i>rt<"bottom"pl><"clear">R',
//        "oLanguage": {
//            "oPaginate": {
//                "sPrevious": "‹",
//                "sNext": "›"
//            }
//        },
//        "aaSorting": [[1, "desc"]],
//        "sPaginationType": "full_numbers",
//        "sPaginationType": "bootstrap",
//        "iDisplayLength": 25,
//        "bStateSave": false
//    });

});