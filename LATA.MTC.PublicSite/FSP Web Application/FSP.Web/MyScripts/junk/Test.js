$(document).ready(function () {
    $('#container').layout();


    window.onresize = function (event) {
        try {


            var windowHeight = window.innerHeight;
            $('#container').css({ 'height': windowHeight - 100 + 'px' });


        } catch (e) {

        }

    }
});