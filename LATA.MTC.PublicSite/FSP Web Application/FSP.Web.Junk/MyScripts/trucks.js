$(function () {

    // creates a proxy to the health check hub
    var truckHub = $.connection.truckHub;

    // handles the callback sent from the server
    truckHub.updateMessages = function (data) {
        $("li").remove();

        $.each(data, function () {
            $('#messages').append('<li>' + this + '</li>');
        });
    };
   
    // Start the connection and request current state
    $.connection.hub.start(function () {
        truckHub.getServiceState();
    });


});