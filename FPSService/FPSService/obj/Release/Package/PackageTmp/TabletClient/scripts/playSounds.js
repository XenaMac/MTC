$(document).ready(function () {

    $('#jpID').jPlayer({
        ready: function () {
            $(this).jPlayer("setMedia", {
                mp3: "sounds/Alarm.mp3"
            });
        },
        supplied: "mp3",
        swfPath: "js",
        errorAlerts: true
    });


    $('#playAlert').click(function () {
        $('#jpID').jPlayer("play");
    });

    $('#stopAlert').click(function () {
        $('#jpID').jPlayer("stop");
    });

});