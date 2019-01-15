$(function () {

    //set selected row
    $("#towTruckGrid tbody tr").live("click", function () {
        $(this).addClass("success").siblings().removeClass("success");
    });

});