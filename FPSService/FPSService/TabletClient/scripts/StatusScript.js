$(document).ready(function () {
    localStorage.setItem("zoom", 14);
    checkLogonStatus();
    FindAssistRequests();
    assistTrigger();
    GetTruckStatus();
    messageTrigger();
    //checkBreakStatus();

    function statusTrigger() {
        return window.setInterval(function () { GetTruckStatus(); }, 10000);
    }

    //Alerts

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


    function playAlert() {
        $('#jpID').jPlayer("play");
    }

    function stopAlert() {
        $('#jpID').jPlayer("stop");
    }

    var st = statusTrigger();

    //hide the assist respond button until we need it
    $('#btnRespond').hide();

    $('#btnRespond').click(function () {
        document.location.href = "Assist.html";
    });

    $('#btnIncident').click(function () {
        //localStorage.setItem("assistid", "");
        SetTruckStatus("On Incident");
        //check for current dispatch time
        //x1097dt = localStorage.getItem("dispatchtime");
        var x1097dt;
        x1097dt = localStorage.getItem("dispatchtime");
        if (typeof x1097dt === 'undefined') {
            //don't worry about it.  This is a dispatch from console operator with actual, valid time
        }
        else {
            //var rightNow = FixDate('now');
            //var rightNow = GetDate();
            //localStorage.setItem("dispatchtime", rightNow);
            GetDate();
        }
        //var OnSiteTime = FixDate('now');
        //localStorage.setItem("onsitetime", OnSiteTime);

        setTimeout(function () { document.location.href = "Assist.html" }, 1250);
    });

    function GetDate() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetDate';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var _data = $.parseJSON(data.d);
                _data = FixDate(_data);
                localStorage.setItem("dispatchtime", _data);
                localStorage.setItem("onsitetime", _data);
            },
            error: function (error) {
                //alert(error);
            }
        });
    }

    $('#btnSetBeat').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/SetBeat';
        var _data = "AssignedBeat=" + $('#selBeats').find(':selected').attr('id').toString();
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "appliction/json; charset=utf-8",
            success: SetBeatSuccess,
            error: SetBeatError
        });
    });

    function SetBeatSuccess(result) {
        localStorage.setItem("assignedbeat", $('#selBeats').find(':selected').text().toString());
        var assignedBeat = localStorage.getItem("assignedbeat");
        $('#assignedBeatVal').empty();
        $('#assignedBeatVal').append(assignedBeat);
    }

    function SetBeatError(error) {
        //alert(error);
    }

    GetBeats();

    function GetBeats() { //Get a list of current beats from the service, use the result to populate the beat list drop down
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetBeats';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: GetBeatsSuccess,
            error: GetBeatsError
        });
    }

    function GetBeatsSuccess(result) {
        var _data = result.d;
        $('#selBeats').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i].BeatID + '">' + _data[i].BeatName + '</option>';
        }
        $('#selBeats').append(_selCode);
    }

    function GetBeatsError(result, error) {
        //alert(error);
    }

    function GetTruckStatus() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckStatus';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: GetStatusSuccess,
            error: GetStatusError
        });
    }

    function GetStatusSuccess(result) {
        var _data = result.d;
        _data = $.parseJSON(_data);
        var _status = _data.TruckStatus;
        localStorage.setItem("truckstate", _status);
        var truckState = localStorage.getItem("truckstate");
        if (truckState == 'Waiting for Driver Login') {
            document.location.href = "Logon.html";
        }
        $('#currentStatus').empty();
        $('#currentStatus').append(truckState);

    }

    function GetStatusError(result) {
        var error = result;
    }

    function checkBreakStatus() {
        var status = localStorage.getItem("truckstate");
        if (status == "On Break") {
            $('#btnBreak').prop('value', 'Off Break');
            $('#btnBreak').css("background-color", "red");
            //add functionality to get time in minutes remaining from service.  service needs to note time break started and return value
            //use that value to set timer
            //localStorage.setItem("truckstate", "On Break");
            $('#break').jqm({ modal: true });
            $('#break').jqmShow();
            clearInterval(counter);
            GetBreakTimeRemaining();
            BreakTimer();
            //GetBreakTimeRemaining();
        }
    }

    function checkLunchStatus() {
        var status = localStorage.getItem("truckstate");
        if (status == "On Lunch") {
            $('#btnLunch').prop('value', 'Off Lunch');
            $('#btnLunch').css("background-color", "red");
            $('#lunch').jqm({ modal: true });
            $('#lunch').jqmShow();
            clearInterval(counter);
            GetLunchTimeRemaining();
            LunchTimer();
        }
    }


    $('#btnModalOffLunch').click(function () {
        SetTruckStatus("On Patrol");
        localStorage.setItem("truckstate", "On Patrol");
        var truckState = localStorage.getItem("truckstate");
        $('#currentStatus').empty();
        $('#currentStatus').append(truckState);
        $('#btnLunch').prop('value', 'On Lunch');
        $('#btnLunch').css("background-color", "");
        $('#btnPatrol').prop('value', 'Off Patrol');
        $('#btnPatrol').css("background-color", "gray");
    });

    $('#btnModalOffBreak').click(function () {
        SetTruckStatus("On Patrol");
        localStorage.setItem("truckstate", "On Patrol");
        var truckState = localStorage.getItem("truckstate");
        $('#currentStatus').empty();
        $('#currentStatus').append(truckState);
        $('#btnBreak').prop('value', 'On Break');
        $('#btnBreak').css("background-color", "");
        clearInterval(counter);
    });

    function assistTrigger() {
        return window.setInterval(function () { FindAssistRequests(); }, 10000);
    }

    function messageTrigger() {
        return window.setInterval(function () { FindMessages(); }, 10000);
    }

    function checkLogonStatus() {
        var logon = localStorage.getItem("logon");
        var driver = localStorage.getItem("name");
        var truckNumber = localStorage.getItem("trucknumber");
        var assisgnedBeat = localStorage.getItem("assignedbeat");
        var truckState = localStorage.getItem("truckstate");
        $('#driverNameVal').empty();
        $('#driverNameVal').append(driver);
        $('#logonStatusVal').empty();
        $('#logonStatusVal').append(logon);
        $('#truckIdVal').empty();
        $('#truckIdVal').append(truckNumber);
        $('#assignedBeatVal').empty();
        $('#assignedBeatVal').append(assisgnedBeat);
        $('#currentStatus').empty();
        $('#currentStatus').append(truckState);
    }

    var asID = assistTrigger();

    //GPS Tracking Section (GetPosition, GetPositionSuccess, GetPositionError)

    GetPosition();

    function positionTrigger() {
        return window.setInterval(function () { GetPosition(); }, 20000);
    }

    var gpID = positionTrigger();

    var map = $('#map_canvas').gmap('get', 'map');
    $(map).addEventListener('zoom_changed', function () { getZoom(); });

    function getZoom() {
        try {
            var map = $('#map_canvas').gmap('get', 'map');
            //var currentZoom = $(map).gmap('option','zoom');
            var currentZoom = map.getZoom();
            localStorage.setItem("zoom", currentZoom);
        }
        catch (error) {
            //alert(error);
        }
    }

    function GetPosition() {
        var logon = localStorage.getItem("logon");

        if (logon == "true") {
            var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckPosition';
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                contentType: "application/json; charset=utf-8",
                success: GetPositionSuccess,
                error: GetPositionError
            });
        }
    }

    function GetPositionSuccess(result) {
        if (result.d != "") {
            var currentZoom = localStorage.getItem("zoom");
            var _data = $.parseJSON(result.d);
            if (_data.TruckStatus == "Waiting for Driver Login") {
                LogOff();
            }
            var zoomLevel = $('#map_canvas').gmap('option', 'zoom');
            var Content = 'TruckNumber: ' + _data.TruckNumber + ' Speed: ' + _data.Speed;
            $('#speedVal').empty();
            $('#speedVal').append(_data.Speed);
            $('#truckStatusVal').empty();
            $('#truckStatusVal').append(_data.TruckStatus);

            try {
                var position = new google.maps.LatLng(_data.Lat, _data.Lon);
                //var position = _data.Lat + ',' + _data.Lon;
                /*
                $('#map_canvas').gmap('clear', 'markers');
                $('#map_canvas').gmap({ 'center': position })
                $('#map_canvas').gmap('option', 'zoom', currentZoom);
                $('#map_canvas').gmap('addMarker', { 'position': position, 'bounds': false });
                $('#map_canvas').gmap('refresh');
                */

                $('#map_canvas').gmap('clear', 'markers');
                $('#map_canvas').gmap('option', 'zoom', parseInt(currentZoom));
                $('#map_canvas').gmap('addMarker', { 'position': position });
                $('#map_canvas').gmap('get', 'map').setOptions({
                    'center': position
                });

                $('map_canvas').gmap('refresh');

            }
            catch (error) {
                //alert(error); //alert was for testing only
            }
            localStorage.setItem("truckstate", _data.TruckStatus);
            checkStatus(_data.TruckStatus);
        }
        else {
            LogOff();
        }
    }

    function GetPositionError(error) {
        //alert(error);
    }

    function checkStatus(status) {
        if (status == "On Patrol" || status == "On Assist" || status == "On Break" || status == "On Lunch") {
            $('#btnRoll').prop('value', 'Roll In');
            $('#btnRoll').css("background-color", "green");
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
        }
        if (status == "On Incident" || status == "In Route") {
            //$('#btnIncident').prop('value', 'Off Assist');
            $('#btnIncident').css("background-color", "red");
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
        }
        /*
        if (status == "On Break") {
        $('#btnBreak').prop('value', 'Off Break');
        $('#btnBreak').css("background-color", "red");
        //add functionality to get time in minutes remaining from service.  service needs to note time break started and return value
        //use that value to set timer
        localStorage.setItem("truckstate", "On Break");
        $('#break').jqm({ modal: true });
        $('#break').jqmShow();
        GetBreakTimeRemaining();
        BreakTimer();
        //GetBreakTimeRemaining();
        }
        */
        if (status == "On Lunch") {
            $('#btnLunch').prop('value', 'Off Lunch');
            $('#btnLunch').css("background-color", "red");
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
            $('#lunch').jqm({ modal: true });
            $('#lunch').jqmShow();
            //Get lunch time just like in break
            //start countdown timer just like in break
        }

        if (status == "Roll In") {
            $('#btnRoll').prop('value', 'Roll Out');
            $('#btnRoll').css("background-color", "");
            $('#btnPatrol').prop('value', 'On Patrol');
            $('#btnPatrol').css("background-color", "");
        }
    }

    function GetUsedBreakTime() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/FindUsedBreakTime';
        var driverid = localStorage.getItem("driverid");
        var _data = 'DriverID=' + driverid + '&Type=Break';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetUsedBreakTimeSuccess,
            error: GetUsedBreakTimeError
        });

    }

    function GetUsedBreakTimeSuccess(result) {
        BreakDurationInSeconds = result.d;
        BreakDurationInSeconds = BreakDurationInSeconds * 60;
    }

    function GetUsedBreakTimeError(error) { }

    function GetUsedLunchTime() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/FindUsedBreakTime';
        var driverid = localStorage.getItem("driverid");
        var _data = 'DriverID=' + driverid + '&Type=Lunch';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json charset=utf-8",
            success: GetUsedLunchTimeSuccess,
            error: GetUsedLunchTimeError
        });
    }

    function GetUsedLunchTimeSuccess(result) {
        LunchDurationInSeconds = result.d;
        LunchDurationInSeconds = LunchDurationInSeconds * 60;
    }

    function GetUsedLunchTimeError(error) { }

    /* Deprectated, using a count up from used break and lunch time now
    function GetBreakTimeRemaining() {
    var _url = ServiceLocation + 'AJAXFSPService.svc/GetUsedBreakTime';
    var driverid = localStorage.getItem("driverid");
    var _data = 'DriverID=' + driverid;
    $.ajax({
    type: "GET",
    dataType: "json",
    url: _url,
    data: _data,
    contentType: "application/json; charset=utf-8",
    success: SetBreakTimeRemaining,
    error: SetBreakTimeRemainingError
    });
    }

    function SetBreakTimeRemaining(result) {

    var timeDiff = result.d;
    BreakDuration = timeDiff;
    BreakTimer();
    }

    function SetBreakTimeRemainingError(error) {
    //alert(error);
    }

    function GetLunchTimeRemaining() {
    var _url = ServiceLocation + 'AJAXFSPService.svc/GetUsedLunchTime';
    var driverid = localstorage.getItem("driverid");
    var _data = 'DriverID=' + driverid;
    $.ajax({
    type: "GET",
    dataType: "json",
    url: _url,
    data: _data,
    contentType: "application/json; charset=utf-8",
    success: SetLunchTimeRemaining,
    error: SetLunchTimeRemainingError
    });
    }

    function SetLunchTimeRemaining(result) {
    var timeDiff = result.d;
    LunchDuration = timeDiff;
    LunchTimer();
    }
    */

    //Log Off events
    function LogOff() {
        //add set logoff on service code, set truck status to logged off or something
        localStorage.setItem("name", "");
        localStorage.setItem("trucknumber", "");
        localStorage.setItem("truckid", "");
        localStorage.setItem("logon", "false");
        localStorage.setItem("truckstate", "loggedoff");
        //Truck is no longer reporting, force Logoff and stop timer
        window.clearInterval(gpID);
        //window.clearInterval(asID);
        var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: RedirectToLogon,
            error: LogOffError
        });
    }

    $('#btnLogOff').click(function () {
        if ($('#btnRoll').val() == "Roll In") {
            $.fallr('show', {
                content: '<p>You must Roll In before you can Log Off</p>',
                position: 'center',
                icon: 'lamp'
            });
            //alert('You must Roll In before you can Log Off');
            return;
        }
        LogOff();
    });

    function LogOffError(error) {
        //alert(error.responseText);
    }

    function RedirectToLogon(result) {
        var response = result.d;
        if (response == "early") {
            document.location.href = "EarlyRollIn.html?Type=EarlyLogOff";
        }
        else {
            document.location.href = "Logon.html";
        }
    }

    //Status changes

    $('#btnRoll').click(function () {
        var RollStatus = "Roll Out";
        if ($('#btnRoll').val() == "Roll Out") {
            RollStatus = "Roll Out";
            $('#btnRoll').prop('value', 'Roll In');
            $('#btnRoll').css("background-color", "green");
            //Initialize the jplay sounds
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
        }
        else {
            RollStatus = "Roll In";
            $('#btnRoll').prop('value', 'Roll Out');
            $('#btnRoll').css("background-color", "");
        }
        SetTruckStatus(RollStatus);
    });

    $('#btnBreak').click(function () {
        var BreakStatus = "On Break";
        if ($('#btnBreak').val() == "On Break") {
            BreakStatus = "On Break";
            $('#btnBreak').prop('value', 'Off Break');
            $('#btnBreak').css("background-color", "red");
            localStorage.setItem("truckstate", "On Break");
            $('#break').jqm({ modal: true });
            $('#break').jqmShow();
            SetTruckStatus(BreakStatus);
            FindBreakDuration();
            GetUsedBreakTime();
            BreakTimer();
        }
        else {
            BreakStatus = "Off Break";
            $('#btnBreak').prop('value', 'On Break');
            $('#btnBreak').css("background-color", "");
            clearInterval(counter);
            $('#breakTimeRemaining').empty();
            SetTruckStatus(BreakStatus);
        }
        
    });

    $('#btnLunch').click(function () {
        var LunchStatus = "On Lunch";
        if ($('#btnLunch').val() == "On Lunch") {
            LunchStatus = "On Lunch";
            $('#btnLunch').prop('value', 'Off Lunch');
            $('#btnLunch').css("background-color", "red");
            localStorage.setItem("truckstate", "On Lunch");
            $('#lunch').jqm({ modal: true });
            $('#lunch').jqmShow();
            SetTruckStatus(LunchStatus);
            FindLunchDuration();
            GetUsedLunchTime();
            LunchTimer();
        }
        else {
            LunchStatus = "Off Lunch";
            $('#btnLunch').prop('value', 'On Lunch');
            $('#btnLunch').css("background-color", "");
            clearInterval(counter);
            $('#lunchTimeRemaining').empty();
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
            SetTruckStatus(LunchStatus);
        }
        
    });

    var BreakDuration = 15;
    var BreakDurationInSeconds = 15 * 60;

    var LunchDuration = 30;
    var LunchDurationInSeconds = 30 * 60;

    function FindBreakDuration(DriverID) {
        var DriverID = localStorage.getItem("driverid");
        var _data = "DriverID=" + DriverID;
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetBreakDuration';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: FindBreakDurationSuccess,
            error: FindBreakDurationError
        });
    }

    function FindBreakDurationSuccess(result) {
        BreakDuration = result.d;
    }

    function FindBreakDurationError(error) {
        //alert(error);
    }

    function FindLunchDuration(DriverID) {
        var DriverID = localStorage.getItem("driverid");
        var _data = "DriverID=" + DriverID;
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetLunchDuration';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: FindLunchDurationSuccess,
            error: FindLunchDurationError
        });
    }

    function FindLunchDurationSuccess(result) {
        LunchDuration = result.d;
    }

    function FindLunchDurationError(error) {
        //alert(error);
    }

    var counter;

    function BreakTimer() {
        //BreakDurationInSeconds = BreakDuration * 60;
        BreakDurationInSeconds = 0;
        //counter = setInterval(timer,1000);
        clearInterval(counter);
        counter = setInterval(timer, 1000);
    }

    function LunchTimer() {
        //LunchDurationInSeconds = LunchDuration * 60;
        LunchDurationInSeconds = 0;
        clearInterval(counter);
        counter = setInterval(lunchtimer, 1000);
    }

    function lunchtimer() {
        LunchDurationInSeconds = LunchDurationInSeconds + 1;
        var TruckState = localStorage.getItem("truckstate");
        if (TruckState != "On Lunch") {
            clearInterval(counter);
            $('#lunchTimeRemaining').empty();
        }
        if (LunchDurationInSeconds <= 0) {
            clearInterval(counter);
            $('#lunchTimeRemaining').text();
            $('#lunchTimeRemaining').text("Lunch Over");
        }
        else {
            //var date = new Date(null);
            //date.setSeconds(LunchDurationInSeconds);
            //var time = date.toTimeString().substr(3, 5);
            $('#lunchTimeRemaining').text();
            var usedMinutes = Math.round(LunchDurationInSeconds / 60);
            $('#lunchTimeRemaining').text('You have used ' + usedMinutes + ' minutes of lunch today');
        }
    }

    function timer() {
        BreakDurationInSeconds = BreakDurationInSeconds + 1;
        var TruckState = localStorage.getItem("truckstate");
        if (TruckState != "On Break") {
            clearInterval(counter);
            $('#breakTimeRemaining').empty();
        }
        if (BreakDurationInSeconds <= 0) {
            clearInterval(counter);
            $('#breakTimeRemaining').text();
            $('#breakTimeRemaining').text("Break Over");
        }
        else {
            //var date = new Date(null);
            //date.setSeconds(BreakDurationInSeconds);
            //var time = date.toTimeString().substr(3, 5);
            $('#breakTimeRemaining').text();
            //$('#breakTimeRemaining').text(time);
            var usedMinutes = Math.round(BreakDurationInSeconds / 60);
            $('#breakTimeRemaining').text('You have used ' + usedMinutes + ' minutes of break today');
        }
    }



    $('#btnPatrol').click(function () {
        var PatrolStatus = "On Patrol";
        if ($('#btnPatrol').val() == "On Patrol") {
            PatrolStatus = "On Patrol";
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
            if ($('#btnRoll').val() == "Roll Out") {
                $('#btnRoll').prop('value', 'Roll In');
                $('#btnRoll').css("background-color", "green");
            }
        }
        else {
            /*
            PatrolStatus = "Roll In";
            $('#btnPatrol').prop('value', 'On Patrol');
            $('#btnPatrol').css("background-color", "");
            $('#btnRoll').prop('value', 'Roll Out');
            $('#btnRoll').css("background-color", "");
            */
            $.fallr('show', {
                content: '<p>You cannot go Off Patrol\nPlease select Roll In, On Break or any other valid status</p>',
                position: 'center',
                icon: 'lamp'
            });
            //alert('You cannot go Off Patrol\nPlease select Roll In, On Break or any other valid status');
            $('#btnPatrol').prop('value', 'Off Patrol');
            $('#btnPatrol').css("background-color", "gray");
            return;
        }
        SetTruckStatus(PatrolStatus);
    });
    /* ASsist button removed
    $('#btnAssist').click(function () {
    var AssistStatus = "On Assist";
    if ($('#btnAssist').val() == "On Assist") {
    AssistStatus = "On Assist";
    $('#btnAssist').prop('value', 'Off Assist');
    $('#btnAssist').css("background-color", "red");
    }
    else {
    AssistStatus = "On Patrol";
    $('#btnAssist').prop('value', 'On Assist');
    $('#btnAssist').css("background-color", "");
    }
    SetTruckStatus(AssistStatus);
    });
    */
    function SetTruckStatus(statusMsg) {
        var logon = localStorage.getItem("logon");
        if (logon == "true") {
            var _data = "Status=" + statusMsg;
            var _url = ServiceLocation + 'AJAXFSPService.svc/SetTruckStatus';
            TruckStatus = statusMsg;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: SetStatusSuccess,
                error: SetStatusError
            });
        }
    }

    function SetStatusSuccess(result) {
        $('#result').empty();
        var _input = result.d;
        var _splitcheck = _input.indexOf("|");
        if (_splitcheck != -1) {
            //found a pipe, probably early or late status change
            var splitData = _input.split("|");
            var status = splitData[0];
            var lateness = splitData[1];
            if (lateness != 'ontime') {
                //some kind of status set has been fired either early or late, force the driver to explain what's going on.
                status = getExceptionType(status);
                document.location.href = "EarlyRollIn.html?Type=" + status;
            }
        }
        else { //didnt' find a pipe
            $('#result').append(result.d);
            localStorage.setItem("truckstate", result.d);
            var truckState = localStorage.getItem("truckstate");
            $('#currentStatus').empty();
            $('#currentStatus').append(truckState);
            if (TruckStatus == "On Assist") {
                $('#StatusScreenHeader').empty();
                //$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckNumber + ' ON ASSIST');
            }
            if (TruckStatus == "On Patrol") {
                $('#StatusScreenHeader').empty();
                //$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckNumber + ' ON PATROL');
            }
            if (result.d == "EARLY") {
                //driver attempted early roll in
                $('#earlyRollIn').jqm({ modal: true });
                $('#earlyRollIn').jqmShow();
            }
        }
        //GetTruckStatus();
    }

    function getExceptionType(response) {
        switch (response) {
            case 'Roll Out':
                return 'LateRollOut';
            case 'On Patrol':
                return 'LateOnPatrol';
        }
    }

    function SetStatusError(error) {
        //alert(error);
    }

    $('#btnModalOKEarlyRollIn').click(function () {
        SetTruckStatus("Roll In OK");
        document.location.href = "EarlyRollIn.html"
    });

    $('#btnModalCancelEarlyRollIn').click(function () {

    });

    function FindMessages() {
        var logon = localStorage.getItem("logon");
        if (logon == "true") {
            var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckMessages';
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                contentType: "application/json; charset=utf-8",
                success: GetTruckMessagesSuccess,
                error: GetTruckMessagesError
            });
        }
    }

    function GetTruckMessagesError(error) {
        //alert(error);
    }

    function GetTruckMessagesSuccess(result) {
        var _data = result.d;
        if (_data.length > 2) {
            var _msg = $.parseJSON(_data);
            localStorage.setItem('MessageID', _msg[0].MessageID);
            $('#MessageInfo').empty();
            $('#MessageInfo').append(_msg[0].MessageText);
            $('#message').jqm({ modal: true });
            $('#message').jqmShow();
        }
    }

    function FindAssistRequests() {
        var logon = localStorage.getItem("logon");

        if (logon == "true") {
            var _url = ServiceLocation + 'AJAXFSPService.svc/GetAssistRequests';
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                contentType: "application/json; charset=utf-8",
                success: GetAssistsSuccess,
                error: GetAssistsError
            });
        }
    }

    $('#btnAckMessage').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckMessage';
        var _data = "MessageID=" + localStorage.getItem('MessageID');
        try {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: AckMsgSuccess
            });
        }
        catch (error) {
            //alert(error);
        }
    });

    function AckMsgSuccess() {
        $.fallr('show', {
            content: '<p>Acknowledged</p>',
            position: 'center',
            icon: 'lamp'
        });
    }

    function GetAssistsSuccess(result) {
        try {

            //display modal dialog
            var _data = result.d;
            if (_data.length > 2) {
                var _requests = $.parseJSON(_data);
                localStorage.setItem('assistid', _requests[0].AssistID);
                localStorage.setItem('incidentid', _requests[0].IncidentID);
                localStorage.setItem('createdbyid', _requests[0].CreatedByID);
                localStorage.setItem('incidentdescription', _requests[0].Description);
                localStorage.setItem('dispatchtime', FixDate(_requests[0].DispatchTime));
                SetTruckStatus('Request Received');
                playAlert();

                $('#AssistInfo').empty();
                $('#AssistInfo').append(_requests[0].AssistInfo);
                $('#dialog').jqm({ modal: true });
                $('#dialog').jqmShow();

            }
            /*
            $('#assistRequests').empty(); //clear the ul

            var _requestCode = "";
            for(var i = 0;i<_requests.length;i++){
            var ButtonText = 'ASSIST';
            var dtText = FixDate(_requests[i].DispatchTime);
            ButtonText += ' ' + dtText;
            _requestCode += '<input type="button" id="' + _requests[i].AssistID + '" class="assistButton" value="Assist Request" />'
            //_requestCode += '<li><a href="#" id="' + _requests[i].AssistID + '" class="request">' + ButtonText + '</a></li>';
            }
            $('#assistRequests').append(_requestCode);
            $('.assistButton').bind("click", function(event){ RespondToRequestClick(event.target.id);});
            }
            */
        }
        catch (error) {
            //alert(error); //Testing only
        }
    }

    function GetAssistsError(error) {
        //alert(error.responseText);
    }

    $('#btnAckAssist').click(function () {
        stopAlert();
        AckAssistRequest();
    });

    function AckAssistRequest() {
        var logon = localStorage.getItem("logon");
        if (logon == "true") {
            var _url = ServiceLocation + 'AJAXFSPService.svc/AckAssistRequest';
            var _data = "_AssistID=" + localStorage.getItem('assistid');
            try {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: AckSuccess,
                    error: AckError
                });
            }
            catch (error) {
                //alert(error);
            }
        }
    }

    function AckSuccess() {
        //alert('Acknowledged');
        $.fallr('show', {
            content: '<p>Acknowledged</p>',
            position: 'center',
            icon: 'lamp'
        });
        //$('#btnIncident').prop('value', 'Off Assist');
        $('#btnIncident').css("background-color", "red");
        SetTruckStatus('In Route');
        stopAlert();
    }

    function AckError(error) {
        //alert('uh,oh');
    }

    function RespondToRequestClick(e) {
        //alert('clicked' + e.toString());
        //$('RequestData').empty();
        var logon = localStorage.getItem("logon");

        if (logon == "true") {
            var _url = ServiceLocation + 'AJAXFSPService.svc/GetAssistRequestDetail';
            var _data = "_AssistID=" + e.toString();
            try {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: GetRequestDetailSuccess,
                    error: GetRequestDetailError
                });
            }
            catch (error) {
                //alert(error);
            }
        }
    }

    function RespondToRequestButtonClick(e) {
        var logon = localStorage.getItem("logon");

        if (logon == "true") {
            var id = e.Id;
            $.mobile.changePage("LogAssist.html", "slideup");
        }
    }

    function GetRequestDetailSuccess(result) {
        $('#RequestData').empty();
        var _data = $.parseJSON(result.d);
        $('#vehicleMakeVal').empty();
        $('#vehicleMakeVal').append(_data.Make);
        $('#vehicleTypeVal').empty();
        $('#vehicleTypeVal').append(_data.VehicleType);
        $('#vehicleColorVal').empty();
        $('#vehicleColorVal').append(_data.Color);
        $('#vehiclePositionVal').empty();
        $('#vehiclePositionVal').append(_data.VehiclePosition);
        $('#dispatchTimeVal').empty();
        $('#dispatchTimeVal').append(FixDate(_data.x1097.toString()));
        $('#btnRespond').show();
        localStorage.setItem("assistid", _data.AssistID);
        localStorage.setItem("assisttype", _data.AssistType);
        localStorage.setItem("incidentid", _data.IncidentID);
        localStorage.setItem("dispatchtime", _data.DispatchTime);
        localStorage.setItem("servicetype", _data.ServiceType);
        localStorage.setItem("dropzone", _data.DropZone);
        localStorage.setItem("make", _data.Make);
        localStorage.setItem("vehicletype", _data.VehicleType);
        localStorage.setItem("vehicleposition", _data.VehiclePosition);
        localStorage.setItem("color", _data.Color);
        localStorage.setItem("licenseplate", _data.LicensePlate);
        localStorage.setItem("state", _data.State);
        localStorage.setItem("startod", _data.StartOD);
        localStorage.setItem("endod", _data.EndOD);
        localStorage.setItem("towlocation", _data.TowLocation);
        localStorage.setItem("tip", _data.Tip);
        localStorage.setItem("tipdetail", _data.TipDetail);
        localStorage.setItem("customerlastname", _data.CustomerLastName);
        localStorage.setItem("comments", _data.Comments);
        localStorage.setItem("ismdc", _data.IsMDC);
        localStorage.setItem("x1097", _data.x1097);
        localStorage.setItem("x1098", _data.x1098);
        localStorage.setItem("contractor", _data.Contractor);
        localStorage.setItem("lognumber", _data.LogNumber);
        localStorage.setItem("beatsegmentid", _data.BeatSegmentID);
        //var _requestCode = '<div id="RequestInfo">';
        //_requestCode += '<button id=" + _data.AssistID + " class="getRequestDetail">Respond to Assist</button>';
        //_requestCode += '<span><b>Service Type:</b> ' + _data.ServiceType + '</span><span>&nbsp<b>Vehicle Position:</b> ' + _data.VehiclePosition + '</span>';
        //_requestCode += '</div>';
        //_requestCode += '<div id="VehicleInfo">';
        //_requestCode += '<span><b>Make:</b> ' + _data.Make + '</span><span>&nbsp<b>Color:</b> ' + _data.Color + '</span><span>&nbsp<b>License Plate:</b> ' + _data.State + ' ' + _data.LicensePlate + '</span>';
        //_requestCode += '</div>';
        //$('#RequestData').append(_requestCode);
        //$('.getRequestDetail').bind("click", function(event){ RespondToRequestButtonClick(event.target.id);});
    }

    function GetRequestDetailError(error) {
        //alert(error);
    }

    function FixDate(dtVal) {
        try {
            if (dtVal != "now") {
                var valFix = dtVal;
                valFix = dtVal.replace("/Date(", "").replace(")/", "");
                var iMil = parseInt(valFix, 10);
                var d = new Date(iMil);
                var Month = (d.getMonth() + 1).toString();
                if (Month.length < 2)
                { Month = "0" + Month; }
                var _date = (d.getDate()).toString();
                if (_date.length < 2)
                { _date = "0" + _date; }
                var Year = (d.getFullYear()).toString();
                var Hour = (d.getHours()).toString();
                if (Hour.length < 2)
                { Hour = "0" + Hour; }
                var Minute = (d.getMinutes()).toString();
                if (Minute.length < 2)
                { Minute = "0" + Minute; }
                var Second = (d.getSeconds()).toString();
                if (Second.length < 2)
                { Second = "0" + Second; }
                return Month + "/" + _date + "/" + Year + " " + Hour + ":" + Minute + ":" + Second;
            }
            else {
                var d = new Date();
                var Month = (d.getMonth() + 1).toString();
                if (Month.length < 2)
                { Month = "0" + Month; }
                var _date = (d.getDate()).toString();
                if (_date.length < 2)
                { _date = "0" + _date; }
                var Year = (d.getFullYear()).toString();
                var Hour = (d.getHours()).toString();
                if (Hour.length < 2)
                { Hour = "0" + Hour; }
                var Minute = (d.getMinutes()).toString();
                if (Minute.length < 2)
                { Minute = "0" + Minute; }
                var Second = (d.getSeconds()).toString();
                if (Second.length < 2)
                { Second = "0" + Second; }
                return Month + "/" + _date + "/" + Year + " " + Hour + ":" + Minute + ":" + Second;
            }
        }
        catch (err) {
            //alert("Bad Input");
        }
    }
});