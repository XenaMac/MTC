$(function () {

    // creates a proxy to the health check hub
    var testHub = $.connection.testHub;

    // handles the callback sent from the server
    testHub.updateMessages = function (data) {
        $("li").remove();

        $.each(data, function () {
            $('#messages').append('<li>' + this + '</li>');
        });
    };

    $("#trigger").click(function () {
        testHub.updateServiceState();
    });

    // Start the connection and request current state
    $.connection.hub.start(function () {
        testHub.getServiceState();
    });


});