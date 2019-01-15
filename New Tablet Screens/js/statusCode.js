$(document).ready(function () {
    checkLogonStatus(); //find out if and when a driver logged on, kick 'em off if they haven't logged on
    checkStatus(); //find current vehicle status
    var tgl = 0; //test variable for toggling states, delete before production
    getDropSites(localStorage["Beat"]);
    getFreeways(localStorage["Beat"]);
    var assists = 1;

    //localStorage['incidentID'] = null;
    localStorage["assistID"] = null;
    localStorage['licensePlate'] = null;
    localStorage["state"] = "California";
    localStorage['direction'] = null;
    localStorage['position'] = null;
    localStorage['laneNumber'] = 0;
    localStorage['chpIncidentType'] = null;
    localStorage['chpLogNumber'] = 0;
    localStorage['incidentSurveyNumber'] = null;
    localStorage['odometer'] = 0;
    localStorage['wazeComments'] = null
    localStorage['editingField'] = null;
    localStorage["preAssistSurveyNum"] = null;
    localStorage["trafficCollision"] = null;
    localStorage["breakDown"] = null;
    localStorage["debris"] = null;
    localStorage['other'] = null;
    localStorage['comments'] = null;
    localStorage['assistDesc'] = null;
    localStorage['transportation'] = null;
    localStorage['startODO'] = 0;
    localStorage['endODO'] = 0;
    localStorage['vehicleType'] = null;
    localStorage['OTAuth'] = null;
    localStorage['dropSite'] = null;
    localStorage['freeway'] = null;
    localStorage['ptn'] = null;
    localStorage['otherDS'] = null;
    localStorage['otherDSBeat'] = null;
    localStorage["reqComments"] = null;
    localStorage["problemType"] = null;
    localStorage["actionTaken"] = null;
    localStorage["detailNotes"] = null;

    //hide initial buttons
    $('#submitIncidentReport').hide();
    $('#submitFinalAssistReport').hide();
    $('#submitAssistAdd').hide();
    $('#laneNumber').hide();
    $('#btnReturn').hide();

    //hide divs
    $('#assistReport').hide();
    $('#incidentNotes').hide();
    $('#trafficcollisiondropdownlist').hide();
    $('#breakdowndropdownlist').hide();
    $('#debrisdropdownlist').hide();
    $('#otherdropdownlist').hide();
    $('#dropSiteArea').hide();
    $('#transportArea').hide();

    $('#submitAssistAdd').click(function () {
        var req = Required();
        if (req == true) {
            //alert('Submit assist, blank out values, create new assist');
            //incidentID gets nulled after closeIncident. If that's happened, a new
            //new incident will need to be created to handle the assist
            if (localStorage["incidentID"] == null || localStorage["incidentID"] == "" || localStorage["incidentID"] == "null") {
                alert("No current incident, aborting");
                return;
            }
            //clearVals();
            //each time an assist is posted the current assistid gets nulled,
            //check to see if that has happened and, if necessary, make a new one
            if (localStorage["assistID"] == null || localStorage["assistID"] == "" || localStorage["assistID"] == "null") {
                startAssist();
            }
            //setVals();  -- Values are already set
            closeAssist();
            //$('#incidentDetails').modal('hide');
            //$('#postReport').modal('hide');
            assists += 1;
            $('#assistTitle').html("Assist Report # " + assists);
        } else {
            alert('Required fields missing. All required fields are in orange. ' + localStorage["reqComments"]);
        }
    });

    $('#submitFinalAssistReport').click(function () {
        var req = Required();
        if (req == true) {
            //incidentID gets nulled after closeIncident. If that's happened, a new
            //new incident will need to be created to handle the assist
            if (localStorage["incidentID"] == null || localStorage["incidentID"] == "" || localStorage["incidentID"] == "null") {
                alert("No current incident, aborting");
                return;
            }

            //each time an assist is posted the current assistid gets nulled,
            //check to see if that has happened and, if necessary, make a new one
            if (localStorage["assistID"] == null || localStorage["assistID"] == "" || localStorage["assistID"] == "null") {
                startAssist();
            } else {
                closeFinalAssist();
            }
        } else {
            alert('Required fields missing. All required fields are in orange. ' + localStorage["reqComments"]);
        }
    });

    function Required() {
        var required = true;

        if ($('#problemTypeDropDown').text().trim() == "Problem Type") {
            required = false;
        }

        if (localStorage['vehicleType'] == null) {
            required = false;
        }

        if (localStorage['assistDesc'] == null) {
            required = false;
        }

        if ($('#ptnDropDown').text().trim() == "P/T/N") {
            required = false;
        }

        if ($('#problemTypeDropDown').text().trim() == "COLLISION" && $('#trafficCollisionDropDown').text().trim() == "Traffic Collision") {
            required = false;
        }

        if ($('#problemTypeDropDown').text().trim() == "BREAKDOWN" && $('#breakDownDropDown').text().trim() == "Breakdown") {
            required = false;
        }

        if ($('#problemTypeDropDown').text().trim() == "DEBRIS" && $('#debrisDropDown').text().trim() == "Debris") {
            required = false;
        }

        if ($('#problemTypeDropDown').text().trim() == "OTHER" && $('#otherDropDown').text().trim() == "Other" && $('#commentsDiv').text() == "") {
            localStorage["reqComments"] = "The parameters above require that notes are entered.";
            required = false;
        }

        return required;
    }

    //Timer, check status every 20 seconds
    var statusTimer;
    statusTimer = setInterval(function () { checkStatus() }, 20000);

    //Message Time, check for new messages every 30 seconds
    var messageTimer;
    messageTimer = setInterval(function () { checkForMessages() }, 30000);

    //Assist Request Timer, check for assist requests every 60 seconds
    var assistTimer;
    assistTimer = setInterval(function () { checkForAssistRequests() }, 60000);

    GetBeats();

    /***************************************
    *
    * Load up data for the drop down boxes
    *
    ***************************************/

    //Beats
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

        //get the order right
        var options = $.parseJSON(result.d);
        var beats = [];
        for (var b = 0; b < options.length; b++) {
            beats.push(parseInt(options[b].BeatName));
        }

        beats.sort(function (a, b) { return a - b });

        var _selCode = '';
        for (var i = 0; i < beats.length; i++) {
            if (beats[i] != "Select") {
                _selCode += "<li><a href='#' id='" + beats[i] + "'>" + beats[i] + "</a></li>";
            }
        }
        $('#beatList').append(_selCode);
    }

    function GetBeatsError(result, error) {
        var err = error;
        setError('GetBets Error: A problem occurred getting the list of beats, please reload or contact the administrator');
    }

    /**********************
    *
    * Redo valules
    *
    **********************/

    function clearSummary() {
        //$('#PT').html('');
        //$('#IDir').html('');
        //$('#FW').html('');
        //$('#POS').html('');
        //$('#inLane').html('');
        $('#sumBtnIncidentReport').removeClass('NavDiv').addClass('btnNavDiv');
        $('#lp').html('');
        $('#lps').html("<b>License Plate: </b> CA");
        $('#pt').html('');
        $('#ptd').html('');
        $('#actionTaken').html('');
        $('#ptn').html('');
        $('#vehicleTypeDiv').html('');
        $('#oTAuthDiv').html('');
        $('#transDesc').html('');
        $('#dropSiteDiv').html('');
        $('#startOdoDiv').html('');
        $('#endOdoDiv').html('');
    }

    function restButtons() {
        $('#btnProblemType').text('CHP Incident Type');
        $('#btnProblemType').removeClass("btn-default").addClass("btn-warning");
        $('#incidentDirection').text('Direction');
        $('#incidentDirection').removeClass("btn-default").addClass("btn-warning");
        $('#incidentFreeway').text('Freeway');
        $('#incidentFreeway').removeClass("btn-default").addClass("btn-warning");
        $('#incidentPosition').text('Position');
        $('#incidentPosition').removeClass("btn-default").addClass("btn-warning");
        $('#licensePlate').text('License Plate');
        $('#stateDropDown').text('California');
        $('#problemTypeDropDown').text('Problem Type');
        $('#trafficCollisionDropDown').text('Traffic Collision');
        $('#breakDownDropDown').text('Breakdown');
        $('#debrisDropDown').text('Debris');
        $('#otherDropDown').text('Other');
        $('#assistDescDropDown').text('Action Taken');
        $('#ptnDropDown').text('P/T/N');
        $('#assistProvidedDropDown').text('Transportation');
        $('#dropsiteDropDown').text('Drop Site');
        $('#otherDS').text('Other Drop Site Name');
        $('#otherDSBeat').text('Other Drop Site Beat');
        $('#startOdo').text('Start Odometer');
        $('#endOdo').text('End Odometer');
        $('#vehicleTypeDropDown').text('Vehicle Type');
        $('#OTAuth').text('OT Authorization #');
        $('#transportArea').slideUp();
        $('#dropSiteArea').slideUp();
    }

    /**********************
	*
	* Assist Requests
	*
	**********************/

    function checkForAssistRequests() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/findMyAssists';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: findAssistSuccess,
            error: findAssistError
        });
    }

    function findAssistSuccess(result) {
        var _data = result.d;
        if (_data != null && _data.length > 5) {
            var _msg = $.parseJSON(_data);
            localStorage["IncidentID"] = _msg.IncidentID;
            localStorage["preAssistDirection"] = _msg.direction;
            localStorage["preAssistDispatchLocation"] = _msg.location;
            localStorage["crossStreets"] = _msg.crossStreet;
            localStorage["comment"] = _msg.comments;
            localStorage["dispatchCode"] = _msg.dispatchCode;
            localStorage["freeway"] = _msg.freeway;
            localStorage["laneNumber"] = _msg.LaneNumber;
            localStorage["CHPLogNumber"] = _msg.CHPLogNumber;
            localStorage["chpLogNumber"] = _msg.CHPLogNumber; //for new incident model
            $('#CHPLN').html("CHP Incident Log #: " + localStorage["CHPLogNumber"]); // for incident information div

            var assistMessage = "Incident Type: " + localStorage["dispatchCode"] + "<br/>Direction: " + localStorage["preAssistDirection"] + "<br/>Location: " +
				localStorage["preAssistDispatchLocation"] + "<br/>Cross Streets: " + localStorage["crossStreets"] + "<br/>Highway: " + localStorage["freeway"] + "<br/>Lane Number: " +
				localStorage["laneNumber"] + "<br/>Comment: " + localStorage["comment"];
            localStorage["assistMessage"] = assistMessage;
            window.open('AssistRequest.html', 'ASSISTREQUEST');
            /*
			$('#AssistInfo').empty();
			//$('#dialog').empty();
			$('#AssistInfo').append(assistMessage);
			$('#dialog').jqm({ modal: true});
			$('#dialog').jqmShow();
			*/
        }
    }

    function findAssistError(error) { }

    function ackAssistRequest() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/ackAssistRequest';
        var _data = "IncidentID=" + localStorage["IncidentID"];
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: ackSuccess,
            error: ackError
        });
    }

    function ackSuccess(result) { }

    function ackError(error) { }

    /**********************
	*
	* Start Assist
	*
	**********************/

    function startAssist() {
        if (localStorage['incidentID'] == null || localStorage['incidentID'] == '') {
            alert('No current incident, aborting');
            return;
        }
        var _assistData;
        _assistData = {
            assistID: null,
            incidentID: localStorage['incidentID'],
            assistDatePosted: null,
            lastAssistInIncidentReport: 0,
            problemType: null,
            problemDetail: null,
            problemNote: null,
            otherNote: null,
            transportType: null,
            StartODO: 0,
            EndODO: null,
            dropSite: null,
            state: null,
            licensePlate: localStorage['licensePlate'],
            vehicleType: null,
            OTAuthorizationNumber: null,
            detailNote: null,
            assistLat: null,
            assistLon: null,
            dropSiteOther: null,
            callSign: null,
            timeOnAssist: null,
            timeOffAssist: null,
            actionTaken: null,
			PTN: null
        };

        var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/createAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: createAssistSuccess,
            error: createAssistError
        });
    }

    function createAssistSuccess(result) {
        //alert("Assist Started, " + result.d);
        localStorage['assistID'] = result.d;
    }

    function createAssistError(error) {
        alert("Create Assist Error: " + error);
    };

    /**********************
	*
	* Get DropSites
	*
	**********************/

    function getDropSites(BeatNumber) {
        var _url = ServiceLocation + 'AJAXFSPService.svc/getDropSites';
        var _data = "BeatNumber=" + BeatNumber;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetDropsSuccess,
            error: GetDropsError
        });
    }

    function GetDropsSuccess(result) {
        var _data = result.d;
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            if (_data[i] != "Select") {
                if (_data[i] != "Other") {
                    _selCode += "<li><a href='#' id='" + _data[i] + "'>" + _data[i] + "</a></li>";
                } else {
                    _selCode += "<li><a href='#' id='" + _data[i] + "'>" + _data[i] + " (Add To Notes)</a></li>";
                }
            }
        }
        $('#dropsiteList').append(_selCode);
    }

    function GetDropsError(error) {
        alert("Get Drops Error: " + error);
    }

    /**********************
    *
    * Get Other DropSites
    *
    **********************/

    function getOtherDropSites(BeatNumber) {
        var _url = ServiceLocation + 'AJAXFSPService.svc/getDropSites';
        var _data = "BeatNumber=" + BeatNumber;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetOtherDropsSuccess,
            error: GetOtherDropsError
        });
    }

    function GetOtherDropsSuccess(result) {
        var _data = result.d;
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            if (_data[i] != "Select") {
                if (_data[i] != "Other") {
                    _selCode += "<li><a href='#' id='" + _data[i] + "'>" + _data[i] + "</a></li>";
                }
            }
        }
        $('#ODSList').append(_selCode);
    }

    function GetOtherDropsError(error) {
        alert("Get Other Drops Error: " + error);
    }

    /**********************
	*
	* Status Checker
	*
	**********************/

    function checkLogonStatus() {
        var status = localStorage["logonStatus"];
        var logonTime = localStorage["logonTime"];
        var beat = localStorage["Beat"];
        var callSign = localStorage["CallSign"];
        if (status != "1") {
            window.location.href = 'Logon.html';
        }
        $('#beatNumber').text(beat);
        $('#callSignNumber').text(callSign);
        $('#driverName').text(localStorage["name"]);
    }

    function checkStatus() {
        hideButtons();
        var truckStatus = localStorage["TruckStatus"];
        $('#statusName').text(truckStatus);
        var cStatus = $('#statusName').text();

        //start checking status.  Depending on current status, different buttons are shown
        if (cStatus.toUpperCase() == 'LOGGED ON') {
            $('#btnLogOff').show();
            $('#btnRollOut').show();
            //$('#btnOnIncident').show();
            $('#holder').css("background-color", "#b2469c");
        }

        if (cStatus.toUpperCase() == 'ROLL OUT') {
            $('#btnRollIn').show();
            $('#btnOnPatrol').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            //$('#btnOnBreak').show();
            //$('#btnOnIncident').show();
            $('#holder').css("background-color", "#5ef2fb");
        }

        if (cStatus.toUpperCase() == 'ON PATROL') {
            $('#btnRollIn').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            $('#btnOnBreak').show();
            $('#btnOnIncident').show();
            $('#holder').css("background-color", "#219846");
        }

        if (cStatus.toUpperCase() == 'ON BREAK') {
            $('#btnRollIn').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            $('#btnOnBreak').show();
            $('#btnOnIncident').show();
            $('#holder').css("background-color", "#5ef2fb");
        }

        if (cStatus.toUpperCase() == 'FORCED BREAK' || cStatus.toUpperCase() == 'BUSY') {
            $('#btnRollIn').show();
            $('#forcedBreak').jqm({ modal: true });
            $('#forcedBreak').jqmShow();
            $('#holder').css("background-color", "#5ef2fb");
        }

        if (cStatus.toUpperCase() == 'ON LUNCH') {
            $('#btnRollIn').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            $('#btnOnBreak').show();
            $('#btnOnIncident').show();
            $('#holder').css("background-color", "#5ef2fb");
        }

        if (cStatus.toUpperCase() == 'ENROUTE') {
            $('#btnRollIn').show();
            $('#btnCancel').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            $('#btnOnBreak').show();
            $('#btnOnIncident').show();
            $('#holder').css("background-color", "#f37330");
        }

        if (cStatus.toUpperCase() == 'ON TOW') {
            $('#btnRollIn').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            $('#btnAssist').show();
            $('#btnOnBreak').show();
            $('#btnOnIncident').show();
            $('#btnTowing').show();
            $('#holder').css("background-color", "#dee742");
        }

        if (cStatus.toUpperCase() == 'ON INCIDENT') {
            $('#btnRollIn').show();
            //$('#btnAssist').show();
            var today = new Date();
            /*Test code, forcing weekend*/
            //today.setDate(23);
            var dt = today.getDay();
            if (today.getDay() == 6 || today.getDay() == 0) {
                $('#btnOnLunch').show();
            }
            //$('#btnOnBreak').show();
            $('#btnOnPatrol').show();
            //$('#btnOnTow').show();
            $('#holder').css("background-color", "#265cff");
        }

        if (cStatus.toUpperCase() == 'ROLL IN') {
            $('#btnRollOut').show();
            $('#btnLogOff').show();
            $('#holder').css("background-color", "#5ef2fb");
        }
    }

    //Hide all buttons, checkStatus shows them depending on status
    function hideButtons() {
        //hide all the buttons
        $('#btnLogOff').hide();
        $('#btnRollOut').hide();
        $('#btnRollIn').hide();
        $('#btnOnBreak').hide();
        $('#btnOnLunch').hide();
        $('#btnOnIncident').hide();
        $('#btnOnTow').hide();
        $('#btnOnPatrol').hide();
        $('#btnAssist').hide();
        $('#btnCancel').hide();
        $('#btnActive').hide();
    }

    /************************
	*
	* get Survery number
	*
	************************/
    function getSurveyNumber() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/MakeSurveyNum';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getSurveyNumberSuccess,
            error: getSurveyNumberError
        });
    }

    function getSurveyNumberSuccess(result) {
        $('#SURNUM').html("<b>Incident Survey ID #: </b>" + result.d)
        localStorage['incidentSurveyNumber'] = result.d;
        localStorage["SurveyNumber"] = result.d;
        localStorage["preAssistSurveyNum"] = result.d;
    }

    function getSurveyNumberError(error) {
        alert("Get Survey Number error: " + error);
    }

    /************************
	*
	* Status Changes
	*
	************************/

    function setStatus(status) {
        var message = 'Selected ' + status;
        //$('#statusName').emtpy();
        localStorage["TruckStatus"] = status;
        $('#statusName').text(status);
        //tell the service truck status has changed
        var logonStatus = localStorage["logonStatus"];
        if (logonStatus != "1") {
            localStorage.clear();
            window.location.href = 'Logon.html';
        }
        else {
            var _data = "Status=" + status;
            var _url = ServiceLocation + 'AJAXFSPService.svc/SetTruckStatus';

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
        checkStatus();
        //showFallR(message);
    }

    function SetStatusSuccess(result) {
        $('#result').empty();
        var _input = result.d;
        //GetTruckStatus();
    }

    function SetStatusError(error) {
        //alert("Set Status Error: " + error);
    }

    function setLogOff(endODO) {
        localStorage["TruckStatus"] = status;
        $('#statusName').text(status);
        //tell the service truck status has changed
        var logonStatus = localStorage["logonStatus"];
        if (logonStatus != "1") {
            localStorage.clear();
            window.location.href = 'Logon.html';
        }
        else {
            var _data = "_ok=force";
            var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff?endODO=' + endODO;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: SetLogOffSuccess,
                error: SetLogOffError
            });
        }
    }

    function SetLogOffSuccess(result) {
        localStorage.clear();
        document.location.href = "Logon.html";
    }

    function SetLogOffError(error) {
        alert("Log Off Error: " + error);
    }

    /************************
    *
    * Close Final Assists
    *
    ************************/

    function closeAssist() {
        if (localStorage['incidentID'] == null || localStorage['incidentID'] == '') {
            alert('No current incident, aborting');
            return;
        }
        if (localStorage['assistID'] == null || localStorage['assistID'] == '') {
            alert('No current assist, aborting');
            return;
        }
        var _assistData;
        _assistData = {
            assistID: localStorage['assistID'],
            incidentID: localStorage['incidentID'],
            assistDatePosted: null,
            lastAssistInIncidentReport: 0,
            problemType: localStorage["problemType"],
            problemDetail: localStorage["detailNotes"],
            problemNote: localStorage["comments"],
            otherNote: localStorage["other"],
            transportType: localStorage["transportation"],
            StartODO: localStorage["startODO"],
            EndODO: localStorage["endODO"],
            dropSite: localStorage["dropSite"],
            state: localStorage["state"],
            licensePlate: localStorage["licensePlate"],
            vehicleType: localStorage["vehicleType"],
            OTAuthorizationNumber: localStorage["OTAuth"],
            detailNote: $('#commentsDiv').text(),
            assistLat: null,
            assistLon: null,
            dropSiteOther: localStorage["otherDS"],
            dropSiteBeat: localStorage["otherDSBeat"],
            callSign: null,
            timeOnAssist: null,
            timeOffAssist: null,
            actionTaken: localStorage["actionTaken"],
			PTN: localStorage["ptn"]
        };

        var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeAssistSuccess,
            error: closeAssistError
        });
    }

    function closeAssistSuccess(result) {
        var res = result.d;
        if (res == 'OK') {
            //alert('Assist closed');
        }
        else {
            alert("Close Assist Error: " + result.d);
        }
        localStorage['assistID'] = null;
        clearVals();
        clearSummary();
        restButtons();
        startAssist();

        $('#onIncidentModal').modal('show');
    }

    function closeAssistError(error) {
        alert("Close Assist Error: " + error);
    };

    /************************
    *
    * Close Final Assists
    *
    ************************/

    function closeFinalAssist() {
        if (localStorage['incidentID'] == null || localStorage['incidentID'] == '') {
            alert('No current incident, aborting');
            return;
        }
        if (localStorage['assistID'] == null || localStorage['assistID'] == '') {
            alert('No current assist, aborting');
            return;
        }
        var _assistData;
        _assistData = {
            assistID: localStorage['assistID'],
            incidentID: localStorage['incidentID'],
            assistDatePosted: null,
            lastAssistInIncidentReport: 1,
            problemType: localStorage["problemType"],
            problemDetail: localStorage["detailNotes"],
            problemNote: localStorage["problemNotes"],
            otherNote: localStorage["actionNotes"],
            transportType: localStorage["transportType"],
            StartODO: localStorage["startODO"],
            EndODO: localStorage["endODO"],
            dropSite: localStorage["dropSite"],
            state: localStorage["state"],
            licensePlate: localStorage["licensePlate"],
            vehicleType: localStorage["vehicleType"],
            OTAuthorizationNumber: localStorage["OTAuth"],
            detailNote: $('#commentsDiv').text(),
            assistLat: null,
            assistLon: null,
            dropSiteOther: localStorage["otherDS"],
            dropSiteBeat: localStorage["otherDSBeat"],
            callSign: null,
            timeOnAssist: null,
            timeOffAssist: null,
            actionTaken: localStorage["actionTaken"],
			PTN: localStorage["ptn"]
        };

        var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeFinalAssistSuccess,
            error: closeFinalAssistError
        });
    }

    function closeFinalAssistSuccess(result) {
        var res = result.d;
        if (res == 'OK') {
            //alert('Final Assist closed');
            closeIncident();
        }
        else {
            alert("Close Final Assist Error: " + result.d);
        }
        localStorage['assistID'] = null;
    }

    function closeFinalAssistError(error) {
        alert("Close Final Assist Error: " + error);
    };

    function clearVals() {
	//for the incident
        localStorage["assistID"] = null;
        localStorage['licensePlate'] = null;
        localStorage["state"] = null;
        localStorage['direction'] = null;
        localStorage['position'] = null;
        localStorage['laneNumber'] = null;
        localStorage['chpProblemType'] = null;
        localStorage['chpLogNumber'] = null;
        localStorage['incidentSurveyNumber'] = null;
        localStorage['odometer'] = 0;
        localStorage['wazeComments'] = null;
		localStorage['comments'] = null; //this is the comments field data for incidents
		localStorage['editingField'] = null;
        localStorage["preAssistSurveyNum"] = null;
        localStorage["trafficCollision"] = null;
        localStorage["breakDown"] = null;
        localStorage["debris"] = null;
        localStorage['other'] = null;
        localStorage['comments'] = null;
        localStorage['assistDesc'] = null;
        localStorage['transportation'] = null;
        localStorage['startOdo'] = 0;
        localStorage['endODO'] = 0;
        localStorage['vehicleType'] = null;
        localStorage['OTAuth'] = null;
        localStorage['dropSite'] = null;
        localStorage['problemType'] = null;

        //txts
        $('#licensePlate').text("License Plate");
        $('#CHPILN').text("CHP Incident Log #");
        $('#surveyNumber').text("");
        $('#currentODO').text("Current Odometer");
        $('#commentsDiv').text("");
        $('#startOdo').text("Start Odometer");
        $('#endOdo').text("End Odometer");
        $('#oTAuth').text("OT Authorization #");
        $('#stateDropDown').text("State");
        $('#dropsiteDropDown').text("Drop Site");
        $('#startOdo').text('Start Odometer');
        $('#endOdo').text('End Odometer');
        $('#beatDropDown').text('Beat #');
        $('#ODSDropDown').text('Other Drop Site');
        $('#laneNumberList').val('1');

        //Divs
        $('#lp').html("");
        $('#chpiln').html("");
        $('#survNumber').html("");
        $('#curODO').html("");
        $('#startOdoDiv').html("");
        $('#endOdoDiv').html("");
        $('#dropSiteDiv').html("");
        $('#oTAuthDiv').html("");
        $('#dir').html("");
        $('#inLane').html("");
        $('#pos').html("");
        $('#traffCollision').html("");
        $('#breakdown').html("");
        $('#debrisInLane').html("");
        $('#otherDiv').html("");
        $('#actionTaken').html("");
        $('#transDesc').html("");
        $('#stateDropDown').html("");
        $('#vehicleTypeDiv').html("");

        //functions
        //getSurveyNumber();

    }

    /************************
	*
	* Button Click Events
	*
	************************/

    $('#btnAckAssist').click(function () {
        setStatus('ENROUTE');
        ackAssistRequest();
    });

    $('#btnLogOff').click(function () {
        var endODO = null;

        endODO = prompt('Enter Odometer Reading', '');

        if (isNaN(endODO)) {
            alert("Odometer reading is not a number")
        } else {
            setLogOff(endODO);
        }
    });

    $('#btnRollOut').click(function () {
        setStatus('ROLL OUT');
    });

    $('#btnRollIn').click(function () {
        setStatus('ROLL IN');
    });

    $('#btnOnPatrol').click(function () {
        setStatus('ON PATROL');
    });

    $('#btnBackOnPatrol').click(function () {
        alert("Going back on patrol");
        $('#forcedBreak').jqm({ modal: true });
        $('#forcedBreak').jqm();
        setStatus('ON PATROL');
    });

    $('#btnOnLunch').click(function () {
        setStatus('ON LUNCH');
        $('#lunch').jqm({ modal: true });
        $('#lunch').modal('show');
    });

    $('#btnOnIncident').click(function () {
        startIncident();
        setStatus('ON INCIDENT');
    });

    $('#submitIncidentReport').click(function () {
        updateIncident();
        startAssist();
        $('#briefUpdate').hide();
        $('#submitIncidentReport').hide();
        $('#moveToAssist').hide();
        $('#assistReport').show();
        $('#stepTitle').text("Assist Report");
        $('#lps').html("<b>State:</b> CA");
        $('#submitFinalAssistReport').show();
        $('#submitAssistAdd').show();
        //change calsses of summary and off('click')
        $('#sumBtnIncidentReport').removeClass('btnNavDiv').addClass('NavDiv').off('click');

    });

    //$('#sumBtnIncidentReport').click(function () {
    //    //alert('Reload Incident Report');
    //    $('#briefUpdate').show();
    //    $('#assistReport').hide();
    //    $('#assistTransport').hide();
    //    $('#incidentNotes').hide();
    //    $('#submitIncidentReport').hide();
    //    $('#submitAssistAdd').hide();
    //    $('#submitFinalAssistReport').hide();
    //});

    //$('#sumBtnAssistReport').click(function () {
    //    //alert('Reload Assist Report')
    //    $('#briefUpdate').hide();
    //    $('#assistReport').show();
    //    $('#assistTransport').hide();
    //    $('#incidentNotes').hide();
    //    $('#submitIncidentReport').hide();
    //    $('#submitAssistAdd').show();
    //    $('#submitFinalAssistReport').show();
    //});

    $('#sumBtnNotes').click(function () {

        localStorage['editingField'] = "comments";
        var comments = $('#commentsDiv').html();

        if (comments == "")
        {
            $('#taNotes').val("");
            $('#taNotes').attr("placeholder", "Incident/Assist Comments");
            $('#KB_NT').modal('show');
        } else {
            $('#taNotes').val(comments);
            $('#KB_NT').modal('show');
        }

        //localStorage['editingField'] = "comments";

        //if (comments ==) ""
        //{
        //    $('#taNotes').val();
        //    $('#taNotes').attr("placeholder", "Incident/Assist Notes");
        //    $('#KB_NT').modal('show');
        //    jsKeyboard.init("virtualKeyboardNotes");

        //    //first input focus
        //    var $firstInput = $('#taNotes').first().focus();
        //    jsKeyboard.currentElement = $firstInput;
        //    jsKeyboard.currentElementCursorPosition = 0;
        //} else {
        //    $('#taNotes').val(comments);
        //    $('#taNotes').attr("placeholder", "Incident/Assist Notes");
        //    $('#KB_NT').modal('show');
        //    jsKeyboard.init("virtualKeyboardNotes");

        //    //first input focus
        //    var $firstInput = $('#taNotes').first().focus();
        //    jsKeyboard.currentElement = $firstInput;
        //    jsKeyboard.currentElementCursorPosition = comments.length;
        //}

    });

    $('#cursorLeft').click(function () {
        jsKeyboard.currentElementCursorPosition--;
    });

    $('#cursorRight').click(function () {
        jsKeyboard.currentElementCursorPosition++;
    });

    $('#moveToAssist').click(function () {
        $('#briefUpdate').hide();
        $('#submitIncidentReport').hide();
        $('#moveToAssist').hide();
        $('#assistReport').show();
        $('#stepTitle').text("Assist Report");
        $('#lps').html("<b>State:</b> CA");
    });

    $('#licensePlate').click(function () {
        localStorage['editingField'] = "LicensePlate";
        $('#kbTxt').val("");
        $('#kbTxt').attr("placeholder", "ABC-###");
        $('#KB_LP').modal('show');
        jsKeyboard.init("virtualKeyboard");

        //first input focus
        var $firstInput = $('#kbTxt').first().focus();
        jsKeyboard.currentElement = $firstInput;
        jsKeyboard.currentElementCursorPosition = 0;
        jsKeyboard.changeToNumber();
    });

    $('#startOdo').click(function () {
        localStorage['editingField'] = "startOdo";
        $('#kbTxt').val("");
        $('#kbTxt').attr("placeholder", "#####");
        $('#KB_LP').modal('show');
        jsKeyboard.init("virtualKeyboard");

        //first input focus
        var $firstInput = $('#kbTxt').first().focus();
        jsKeyboard.currentElement = $firstInput;
        jsKeyboard.currentElementCursorPosition = 0;
        jsKeyboard.changeToNumber();
    });

    $('#endOdo').click(function () {
        localStorage['editingField'] = "endOdo";
        $('#kbTxt').val("");
        $('#kbTxt').attr("placeholder", "#####");
        $('#KB_LP').modal('show');
        jsKeyboard.init("virtualKeyboard");

        //first input focus
        var $firstInput = $('#kbTxt').first().focus();
        jsKeyboard.currentElement = $firstInput;
        jsKeyboard.currentElementCursorPosition = 0;
        jsKeyboard.changeToNumber();
    });

    $('#KB_LP').on('hidden.bs.modal', function () {
        var editingField = localStorage.getItem('editingField');

        if ($('#kbTxt').val() != "") {
            if (editingField == "LicensePlate") {
                if ($('#kbTxt').val() != "") {
                    $('#licensePlate').text($('#kbTxt').val());
                    localStorage['licensePlate'] = $('#kbTxt').val();
                    $('#lp').html("<b>License Plate: </b>" + $('#kbTxt').val())
                }
            } else if (editingField == "CHPILN") {
                $('#CHPILN').text($('#kbTxt').val());
                localStorage['chpLogNumber'] = null;
                $('#chpiln').html("CHP Inclident Log #: " + $('#kbTxt').val())
            } else if (editingField == "surveyNumber") {
                $('#surveyNumber').text($('#kbTxt').val());
                localStorage['incidentSurveyNumber'] = $('#kbTxt').val();
                $('#survNumber').html("Incident Survery ID #: " + $('#kbTxt').val())
            } else if (editingField == "otherDS") {
                $('#otherDS').text($('#kbTxt').val());
                localStorage['otherDS'] = $('#kbTxt').val();
                $('#dropSiteDiv').html("<b>Drop Site: </b>" + $('#kbTxt').val());
            } else if (editingField == "otherDSBeat") {
                $('#otherDSBeat').text($('#kbTxt').val());
                localStorage['otherDSBeat'] = $('#kbTxt').val();
                $('#dropSiteDiv').append("<b> | Beat #: </b>" + $('#kbTxt').val());
            } else if (editingField == "Other") {
                localStorage['comments'] += $('#kbTxt').val();
                if ($('#commentsDiv').html() == "") {
                    $('#commentsDiv').append($('#kbTxt').val())
                } else {
                    $('#commentsDiv').append("<br />" + $('#kbTxt').val())
                }
                $('#commentsDiv').append("<br />" + $('#kbTxt').val())
            } else if (editingField == "Other1") {
                localStorage['comments'] += $('#kbTxt').val();
                if ($('#commentsDiv').html() == "") {
                    $('#commentsDiv').append($('#kbTxt').val())
                } else {
                    $('#commentsDiv').append("<br />" + $('#kbTxt').val())
                }
            } else if (editingField == "startOdo") {
                localStorage['startODO'] += $('#kbTxt').val();
                $('#startOdoDiv').html("<b>Starting Odomoter: </b>" + $('#kbTxt').val());
                $('#startOdo').text($('#kbTxt').val());
            } else if (editingField == "endOdo") {
                localStorage['endODO'] += $('#kbTxt').val();
                $('#endOdoDiv').html("<b>Ending Odomoter: </b>" + $('#kbTxt').val());
                $('#endOdo').text($('#kbTxt').val());
            } else if (editingField == "Other2") {
                $('#dropSiteDiv').html("Drop Site: " + $('#kbTxt').val());
            } else if (editingField == "OTAuth") {
                localStorage["OTAuth"] = $('#kbTxt').val();
                $('#OTAuth').text($('#kbTxt').val());
                $('#oTAuthDiv').html("<b>OT Authorization #: </b>" + $('#kbTxt').val());
            } else if (editingField == "comments") {
                $('#commentsDiv').html($('#kbTxt').val());
            }
        }
    });

    $('#KB_NT').on('hidden.bs.modal', function () {
        var editingField = localStorage.getItem('editingField');

        if ($('#taNotes').val() != "") {
            $('#commentsDiv').html($('#taNotes').val());
        } else {
            $('#commentsDiv').html("");
        }
    });

    $('#stateList li a').on('click', function () {
        localStorage["state"] = $(this).attr('id');
        $('#lps').html("<b>State: </b>" + $(this).attr('id'));
        $('#stateDropDown').text($(this).text());
    });

    $('#problemTypeList li a').on('click', function () {
        if ($(this).attr('id') == "TC") {
            $('#pt').html("<b>Problem Type: </b>" + $(this).text());
            $('#problemTypeDropDown').text($(this).text());
            localStorage["problemType"] = $(this).text();
            $('#trafficcollisiondropdownlist').show();
            $('#breakdowndropdownlist').hide();
            $('#debrisdropdownlist').hide();
            $('#otherdropdownlist').hide();
        } else if ($(this).attr('id') == "BD") {
            $('#pt').html("<b>Problem Type: </b>" + $(this).text());
            $('#problemTypeDropDown').text($(this).text());
            localStorage["problemType"] = $(this).text();
            $('#trafficcollisiondropdownlist').hide();
            $('#breakdowndropdownlist').show();
            $('#debrisdropdownlist').hide();
            $('#otherdropdownlist').hide();
        } else if ($(this).attr('id') == "DB") {
            $('#pt').html("<b>Problem Type: </b>" + $(this).text());
            $('#problemTypeDropDown').text($(this).text());
            localStorage["problemType"] = $(this).text();
            $('#trafficcollisiondropdownlist').hide();
            $('#breakdowndropdownlist').hide();
            $('#debrisdropdownlist').show();
            $('#otherdropdownlist').hide();
        } else if ($(this).attr('id') == "TO") {
            $('#pt').html("<b>Problem Type: </b>" + $(this).text());
            $('#problemTypeDropDown').text($(this).text());
            localStorage["problemType"] = $(this).text();
            $('#trafficcollisiondropdownlist').hide();
            $('#breakdowndropdownlist').hide();
            $('#debrisdropdownlist').hide();
            $('#otherdropdownlist').show();
        }
    });

    $('#collisionList li a').on('click', function () {
        localStorage["trafficCollision"] = $(this).attr('id');
        localStorage["detailNotes"] = $(this).text();
        $('#ptd').html("<b>Traffic Collision: </b>" + $(this).attr('id'));
        $('#trafficCollisionDropDown').text($(this).text());
    });

    $('#breakDownList li a').on('click', function () {
        localStorage["breakDown"] = $(this).attr('id');
        localStorage["detailNotes"] = $(this).text();
        $('#ptd').html("<b>Breakdown: </b>" + $(this).attr('id'));
        $('#breakDownDropDown').text($(this).attr('id'));
    });

    $('#debrisList li a').on('click', function () {
        localStorage["debris"] = $(this).attr('id');
        localStorage["detailNotes"] = $(this).text();
        $('#ptd').html("<b>Debris: </b>" + $(this).attr('id'));
        $('#debrisDropDown').text($(this).attr('id'));
    });

    $('#otherList li a').on('click', function () {
        localStorage["other"] = $(this).attr('id');
        localStorage["detailNotes"] = $(this).text();
        $('#ptd').html("<b>Other: </b>" + $(this).attr('id'));
        $('#otherDropDown').text($(this).attr('id'));
    });

    $('#assistDescList li a').on('click', function () {
        localStorage["actionTaken"] = $(this).attr('id');
        $('#actionTaken').html("<b>Action Taken: </b>" + $(this).attr('id'));
        $('#assistDescDropDown').text($(this).text());
    });

    $('#assistProvidedList li a').on('click', function () {
        localStorage["transportation"] = $(this).attr('id');
        $('#transDesc').html("<b>Transportation Provided: </b>" + $(this).attr('id'));
        $('#assistProvidedDropDown').text($(this).attr('id'));
        if ($(this).text() != "No Passenger (NPAS)") {
            $('#transportArea').slideDown();
        } else {
            $('#transportArea').slideUp();
        }
    });

    $('#dropsiteList').on('click', 'li', function () {
        localStorage["dropSite"] = $(this).text();
        if ($(this).text() == "Other (Add To Notes)") {
            $('#dropSiteArea').slideDown();
            $('#dropsiteDropDown').text($(this).text());
        } else {
            $('#dropSiteArea').slideUp();
            $('#dropsiteDropDown').text($(this).text());
            $('#dropSiteDiv').html("<b>Dropsite: </b>" + $(this).text());
        }
    });

    $('#problemType li a').on('click', function () {
        localStorage["chpProblemType"] = $(this).text().trim();
        $('#btnProblemType').text($(this).text());
        $('#btnProblemType').removeClass("btn-warning").addClass("btn-default");
        $('#PT').html("<b>Problem Type: </b>" + $(this).text());
        checkIncidentReport();
    });

    $('#direction li a').on('click', function () {
        localStorage["direction"] = $(this).attr('id');
        $('#incidentDirection').text($(this).text());
        $('#incidentDirection').removeClass("btn-warning").addClass("btn-default");
        $('#IDir').html("<b>Incident Direction: </b>" + $(this).text());
        checkIncidentReport();
    });

    $('#freeway').on('click', 'li', function () {
        localStorage["freeway"] = $(this).text();
        $('#incidentFreeway').text($(this).text());
        $('#incidentFreeway').removeClass("btn-warning").addClass("btn-default");
        $('#FW').html("<b>Freeway: </b>" + $(this).text());
        checkIncidentReport();
    });

    $('#position li a').on('click', function () {
        localStorage["position"] = $(this).attr('id');
        $('#incidentPosition').text($(this).text());
        $('#incidentPosition').removeClass("btn-warning").addClass("btn-default");
        $('#POS').html("<b>Position: </b>" + $(this).text());
        checkIncidentReport();

        if ($(this).text() == "In Lane") {
            $('#laneNumber').show();
        } else {
            $('#laneNumber').hide();
            $('#inLane').html("");
        }
    });

    $('#laneNumberList li a').on('click', function () {
        localStorage["laneNumber"] = $(this).attr('id');
        $('#laneNumber').text($(this).text());
        $('#inLane').html("<b>Lane #: </b>" + $(this).text());
    });

    $('#ptnList li a').on('click', function () {
        //localStorage["ptn"] = $(this).attr('id');
		localStorage["ptn"] = $(this).text();
        $('#ptnDropDown').text($(this).text());
        $('#ptn').html("<b>P/T/N: </b>" + $(this).text());
    });

    $('#vehicleTypeList li a').on('click', function () {
        localStorage["vehicleType"] = $(this).attr('id');
        $('#vehicleTypeDropDown').text($(this).text());
        $('#vehicleTypeDiv').html("<b>Vehicle Type: </b>" + $(this).text());
    });

    $('#OTAuth').click(function () {
        localStorage['editingField'] = "OTAuth";
        $('#kbTxt').val("");
        $('#kbTxt').attr("placeholder", "#####");
        $('#KB_LP').modal('show');
        jsKeyboard.init("virtualKeyboard");

        //first input focus
        var $firstInput = $('#kbTxt').first().focus();
        jsKeyboard.currentElement = $firstInput;
        jsKeyboard.currentElementCursorPosition = 0;
    });

    $('#beatList').on('click', 'li', function () {
        localStorage["otherDSBeat"] = $(this).text();
        $('#beatDropDown').text($(this).text());
        //$('#dropSiteDiv').html("Drop Site: " + $('#kbTxt').val());   Get DropSites for that beat
        getOtherDropSites($(this).text());
    });

    $('#ODSList').on('click', 'li', function () {
        localStorage["otherDS"] = $(this).text();
        $('#ODSDropDown').text($(this).text());
        $('#dropSiteDiv').html("<strong>Drop Site: </strong>" + $(this).text());
    });

    function checkIncidentReport() {
        if ($('#PT').html() != "" && $('#IDir').html() != "" && $('#FW').html() != "" && $('#POS').html() != "") {
            $('#submitIncidentReport').show();
            $('#moveToAssist').show();
        }
    }

    $('#btnCancel').click(function () {
        //$('#cancel').jqm({ modal: true });
        $('#cancel').modal('show');
    });

    //$('#btnOnTow').click(function(){
    //	setStatus('ON TOW');
    //});

    $('#btnModalOffLunch').click(function () {
        setStatus('ON PATROL');
    });

    $('#btnModalOffBreak').click(function () {
        setStatus('ON PATROL');
    });

    $('#rtm').click(function () {
        $('#btnReturn').show();
    });

    $('#btnReturn').click(function () {
        $('#incidentModal').modal('show');
        $('#btnReturn').hide();
    });

    $('#cancelIR').click(function () {
        //closeIncident();
		cancelIncident();
        clearVals();
        clearSummary();
        restButtons();
        $('#incidentModal').modal('hide');
        $('#assistReport').hide();
        $('#briefUpdate').hide();
        $('#Summary').hide();
        $('#laneNumber').hide();
        setStatus('ON PATROL');
        clearLocalIncidentStorage();
        clearLocalAssistStorage();
    });

    //Start Up Helper functions
    function startIncident() {
        localStorage['incidentID'] = null;
        var _incidentData;
        // most of this data will get overwritten at the service end.
        // technically the only reason this has to happen now is to
        //get the incident id and store it and get the incident ball rolling
        _incidentData = {
            incidentDatePosted: null,
            userPosted: null,
            callSign: null,
            fromTruck: true,
            lat: 0.0,
            lon: 0.0,
            canceled: false,
            reasonCanceled: 'na',
            beat: null,
            truckNumber: null,
            logID: null,
            wazeID: null,
            truckStatusID: null,
            FSPLocation: null,
            dispatchLocation: null,
            direction: null,
            positionIncident: null,
            laneNumber: null,
            chpIncidentType: null,
            briefUpdateLat: 0.0,
            briefUpdateLon: 0.0,
            freeway: '9',
            briefUpdatePosted: false,
            timeOfBriefUpdate: null,
            CHPLogNumber: null,
            incidentSurveyNumber: null,
            driverLastName: null,
            driverFirstName: null,
            driverID: null,
            runID: null
        };

        var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/createIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: postIncidentSuccess,
            error: postIncidentError
        });
    }

    function postIncidentSuccess(result) {
        localStorage["incidentID"] = result.d;
        $('#incidentModal').modal('show');
        $('#assistReport').hide();
        $('#briefUpdate').show();
        $('#Summary').show();
        getSurveyNumber();
        //alert("Incident Started: " + localStorage["incidentID"]);
    }

    function postIncidentError(error) {
        alert("Post Incident Error: " + error);
    }

    //Update Incident
    function updateIncidentBasicWaze() {
        localStorage['wazeComments']

        var _incidentData;
        _incidentData = {
            incidentID: localStorage['incidentID'],
            incidentDatePosted: null,
            userPosted: null,
            callSign: null,
            fromTruck: true,
            lat: 0.0,
            lon: 0.0,
            canceled: false,
            reasonCanceled: 'na',
            beat: null,
            truckNumber: null,
            logID: null,
            wazeID: null,
            truckStatusID: null,
            FSPLocation: null,
            dispatchLocation: null,
            direction: localStorage['direction'],
            positionIncident: localStorage['position'],
            laneNumber: localStorage['laneNumber'],
            chpIncidentType: localStorage['chpProblemType'],
            briefUpdateLat: 0.0,
            briefUpdateLon: 0.0,
            freeway: localStorage['freeway'],
            briefUpdatePosted: true,
            timeOfBriefUpdate: null,
            CHPLogNumber: localStorage['chpLogNumber'],
            incidentSurveyNumber: localStorage['incidentSurveyNumber'],
            driverLastName: null,
            driverFirstName: null,
            driverID: null,
            runID: null
        };

        var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/updateIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: updateIncidentSuccess,
            error: updateIncidentError
        });
    }

    function updateIncident() {
        var _incidentData;

        _incidentData = {
            incidentID: localStorage['incidentID'],
            incidentDatePosted: null,
            userPosted: null,
            callSign: null,
            fromTruck: true,
            lat: 0.0,
            lon: 0.0,
            canceled: false,
            reasonCanceled: 'na',
            beat: null,
            truckNumber: null,
            logID: null,
            wazeID: null,
            truckStatusID: null,
            FSPLocation: null,
            dispatchLocation: null,
            direction: localStorage['direction'],
            positionIncident: localStorage['position'],
            laneNumber: localStorage['laneNumber'],
            chpIncidentType: localStorage['chpProblemType'],
            briefUpdateLat: 0.0,
            briefUpdateLon: 0.0,
            freeway: localStorage['freeway'],
            briefUpdatePosted: true,
            timeOfBriefUpdate: null,
            CHPLogNumber: localStorage['chpLogNumber'],
            incidentSurveyNumber: localStorage['incidentSurveyNumber'],
            driverLastName: null,
            driverFirstName: null,
            driverID: null,
            runID: null
        };

        var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/updateIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: updateIncidentSuccess,
            error: updateIncidentError
        });
    }

    function updateIncidentSuccess(result) {
        //alert("Incident Updated, " + result.d);
    }

    function updateIncidentError(error) {
        alert("Update Incident Error: " + error);
    }

    /*********************************
	*
	* On Break Code
	*
	*********************************/

    var breakUsed = parseInt(localStorage["breakTimerTotal"]);
    var breakStart = '';
    var breakEnd = '';
    var breakTimer;
    var totalBreaks = parseInt(localStorage["allottedBreaks"]);
    var breaksUsed = 0;

    //$('#btnOnBreak').click(function () {

    //    //$('#break').jqm({ modal: true });

    //});

    //click event on menu button starts timer
    $('#btnOnBreak').click(function () {
        setStatus('ON BREAK');

        var breaksUsed = parseInt(localStorage["breaksUsed"]);
        if (breaksUsed >= totalBreaks) {
            $('#breakTimeRemaining').text('You have already had a break today.\r\nSwitching back to ON PATROL');
            $('#lblClickOKBreak').text('Your total break allotment for the day is ' + totalBreaks);
            setStatus('ON PATROL')
        }
        else {
            if (breaksUsed > 0) {
                //reset the used time for additional breaks
                breakUsed = 0;
            }
            breakStart = makeDateString();
            breakTimer = setInterval(function () { breakTimerTick() }, 60000);
            //set initial text
            var btRemaining = (15 * 60) - breakUsed;
            btRemaining = btRemaining / 60;
            $('#breakTimeRemaining').text('You have ' + btRemaining + ' minutes remaining');

            breaksUsed += 1;
            localStorage["breaksUsed"] = breaksUsed;
        }

        $('#break').modal('show');
    });

    //click even in modal window stops timer
    $('#btnModalOffBreak').click(function () {
        breakEnd = makeDateString();
        window.clearInterval(breakTimer);
        localStorage["breakTimerTotal"] = breakUsed;
        if (breakUsed < 60) {
            var breaksUsed = parseInt(localStorage["breaksUsed"]);
            localStorage["breakTimerTotal"] = 0;
            localStorage["breaksUsed"] = breaksUsed - 1;
        }
        var lblText = $('#lblClickOKBreak').text();
        if (lblText != "Get to work, you") {
            //add code to update break time at service
            localStorage["breakStarted"] = breakStart;
            localStorage["breakEnd"] = breakEnd;
            //showFallR('Updating FSP Service with used break time\r\nStart: ' + breakStart + '\r\nEnd:' + breakEnd);
        }
    });

    //timer tick event, updates label on modal break window
    function breakTimerTick() {
        breakUsed += 60;
        var btRemaining = (15 * 60) - breakUsed;
        btRemaining = btRemaining / 60;
        $('#breakTimeRemaining').text('You have ' + btRemaining + ' minutes remaining');
    }

    /***********************************
	*
	* ON LUNCH CODE
	*
	***********************************/

    var lunchUsed = parseInt(localStorage["lunchTimerTotal"]);
    var lunchStart = '';
    var lunchEnd = '';
    var lunchTimer;

    //click event on menu button starts timer
    $('#btnOnLunch').click(function () {
        if (lunchUsed > 0) {
            $('#lunchTimeRemaining').text('You have already had a lunch today.\r\nSwitching back to ON PATROL');
            $('#lblClickOKLunch').text('Get to work, you');
            setStatus('ON PATROL')
        }
        else {
            lunchStart = makeDateString();
            lunchTimer = setInterval(function () { lunchTimerTick() }, 30000);
            //set initial text
            var btRemaining = (30 * 60) - lunchUsed;
            btRemaining = btRemaining / 60;
            $('#lunchTimeRemaining').text('You have ' + btRemaining + ' minutes remaining');
        }
    });

    //click even in modal window stops timer
    $('#btnModalOffLunch').click(function () {
        window.clearInterval(lunchTimer);
        lunchEnd = makeDateString();
        localStorage["lunchTimerTotal"] = lunchUsed;
        var lblText = $('#lblClickOKLunch').text();
        if (lblText != "Get to work, you") {
            //add code to update lunch time at service
            localStorage["lunchStarted"] = lunchStart;
            localStorage["lunchEnd"] = lunchEnd;
            //showFallR('Updating FSP Service with used lunch time\r\nStart: ' + lunchStart + '\r\nEnd:' + lunchEnd);
        }
    });

    //timer tick event, updates label on modal break window
    function lunchTimerTick() {
        lunchUsed += 30;
        var btRemaining = (15 * 60) - lunchUsed;
        btRemaining = btRemaining / 60;
        $('#lunchTimeRemaining').text('You have ' + btRemaining + ' minutes remaining');
    }

    /***********************************
	*
	* Messages: Send and Receive
	*
	***********************************/

    function checkForMessages() {
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

    function GetTruckMessagesError(error) {
        alert("Get Truck Message Error: " + error);
    }

    function GetTruckMessagesSuccess(result) {
        var _data = result.d;
        if (_data.length > 2) {
            var _msg = $.parseJSON(_data);
            localStorage["MessageID"] = _msg[0].MessageID;
            var _msgType = _msg[0].MessageType;
            if (_msgType == 0) { //no response needed
                $('#MessageInfo0').empty();
                $('#MessageInfo0').append(_msg[0].MessageText);
                $('#message0').modal('show');
                //$('#message0').jqm({ modal: true});
                //$('#message0').jqmShow();
            }
            else if (_msgType == 1) { //need a response
                $('#MessageInfo1').empty();
                $('#MessageInfo1').append(_msg[0].MessageText);
                $('#message1').modal('show');
                //$('#message1').jqm({ modal: false});
                //$('#message1').jqmShow();
            }
        }
    }

    $('#btnAckMessage0').click(function () {
        //showFallR('Sending ack with no extra response');
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
            alert("Acknowlegde Error: " + error);
        }
    });

    $('#btnAckMessage1').click(function () {
        //showFallR('Sending ack with extra response');
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckMessageWithResponse';
        var _data = "MessageID=" + localStorage.getItem('MessageID') + "&Response=Acked";
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
        $('#message1').jqmHide();
    });

    $('#btnYesMessage1').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckMessageWithResponse';
        var _data = "MessageID=" + localStorage.getItem('MessageID') + "&Response=Yes";
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
        $('#message1').jqmHide();
    });

    $('#btnNoMessage1').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckMessageWithResponse';
        var _data = "MessageID=" + localStorage.getItem('MessageID') + "&Response=No";
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
        $('#message1').jqmHide();
    });

    $('#btnEnterNumber').click(function () {
        var numVal = $('#txtNum').val();
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckMessageWithResponse';
        var _data = "MessageID=" + localStorage.getItem('MessageID') + "&Response=" + numVal;
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
        $('#message1').jqmHide();
    });

    function AckMsgSuccess() {
        showFallR('Ack sent');
    }

    /*********************************
	*
	* Cancel Incident/Assist Code
	*
	*********************************/

    $('#btnUTL').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/cancelAssist';
        var _data = "incidentID=" + localStorage["IncidentID"] + "&reason=UTL";
        try {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: CancelIncidentSuccess
            });
        }
        catch (error) {
            alert("Cancel Assist Error: " + error);
        }
    });

    $('#btnCA').click(function () {
        var _url = ServiceLocation + 'AJAXFSPService.svc/cancelAssist';
        var _data = "incidentID=" + localStorage["IncidentID"] + "&reason=CANC";
        try {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: CancelIncidentSuccess
            });
        }
        catch (error) {
            alert("Cancel Assist Error: " + error);
        }
    });

    function CancelIncidentSuccess() {
        $//('#cancel').jqmHide();
        $('#cancel').modal('hide');
        //showFallR('Assist Canceled. Returning to \"On Patrol\"');
        alert('Assist Canceled. Returning to \"On Patrol\"');
        setStatus('ON PATROL')
    }

    /***********************************
	*
	* Helper Functions
	*
	***********************************/

    function makeDateString() {
        var dtNow = new Date();
        var chour = dtNow.getHours().toString();
        var cminute = dtNow.getMinutes().toString();
        var csecond = dtNow.getSeconds().toString();
        var cmonth = dtNow.getMonth().toString();;
        var cday = dtNow.getDate().toString();;
        var cyear = dtNow.getFullYear().toString();;
        var retdate = cmonth + '/' + cday + '/' + cyear + ' ' + chour + ':' + cminute + ':' + csecond;
        return retdate;
    }

    function showFallR(message) {
        $.fallr('show', {
            content: '<p>' + message + '</p>',
            position: 'center',
            icon: 'lamp'
        });
    }

    /***********************************
	*
	* GetFreeways
	*
	***********************************/

    function getFreeways(beat) {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetHighwaysByBeat?BeatNumber=' + beat;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getFreewaysSuccess,
            error: getFreewaysError
        });
    }

    function getFreewaysSuccess(result) {
        var _data = result.d;
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            if (_data[i] != "Select") {
                //<li><a href="#" id="NB">North Bound</a></li>
                _selCode += "<li><a href='#' id='" + _data[i] + "'>" + _data[i] + "</a></li>";
            }
        }
        $('#freeway').append(_selCode);
    }

    function getFreewaysError(error) {
        alert("Get Freeway Error: " + error);
    }

    /***********************************
    *
    * Close Incident
    *
    ***********************************/

    function closeIncident() {
        var _incidentData;

        _incidentData = {
            incidentID: localStorage['incidentID'],
            incidentDatePosted: null,
            userPosted: null,
            callSign: null,
            fromTruck: true,
            lat: 0.0,
            lon: 0.0,
            canceled: false,
            reasonCanceled: 'na',
            beat: null,
            truckNumber: null,
            logID: null,
            wazeID: null,
            truckStatusID: null,
            FSPLocation: null,
            dispatchLocation: null,
            direction: localStorage['direction'],
            positionIncident: localStorage['position'],
            laneNumber: localStorage['laneNumber'],
            chpIncidentType: localStorage['chpProblemType'],
            briefUpdateLat: 0.0,
            briefUpdateLon: 0.0,
            freeway: localStorage['freeway'],
            briefUpdatePosted: true,
            timeOfBriefUpdate: null,
            CHPLogNumber: localStorage['chpLogNumber'],
            incidentSurveyNumber: localStorage['incidentSurveyNumber'],
            driverLastName: null,
            driverFirstName: null,
            driverID: null,
			comment: $('#commentsDiv').text(),
            runID: null
        };

        var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeIncidentSuccess,
            error: closeIncidentError
        });
    }

    function closeIncidentSuccess(result) {
        //alert(result.d);
        clearVals();
        clearSummary();
        restButtons();
        $('#incidentModal').modal('hide');
        $('#assistReport').hide();
        $('#briefUpdate').hide();
        $('#Summary').hide();
        setStatus('ON PATROL');
        location.reload();
        localStorage['incidentID'] = null;
        clearLocalIncidentStorage();
        clearLocalAssistStorage();
    }

    function closeIncidentError(error) {
        alert("Close Incident Error: " + error);
    }

	function cancelIncident() {
        var _incidentData;

        _incidentData = {
            incidentID: localStorage['incidentID'],
            incidentDatePosted: null,
            userPosted: null,
            callSign: null,
            fromTruck: true,
            lat: 0.0,
            lon: 0.0,
            canceled: true,
            reasonCanceled: 'na',
            beat: null,
            truckNumber: null,
            logID: null,
            wazeID: null,
            truckStatusID: null,
            FSPLocation: null,
            dispatchLocation: null,
            direction: localStorage['direction'],
            positionIncident: localStorage['position'],
            laneNumber: localStorage['laneNumber'],
            chpIncidentType: localStorage['chpProblemType'],
            briefUpdateLat: 0.0,
            briefUpdateLon: 0.0,
            freeway: localStorage['freeway'],
            briefUpdatePosted: true,
            timeOfBriefUpdate: null,
            CHPLogNumber: localStorage['chpLogNumber'],
            incidentSurveyNumber: localStorage['incidentSurveyNumber'],
            driverLastName: null,
            driverFirstName: null,
            driverID: null,
			comment: $('#commentsDiv').text(),
            runID: null
        };

        var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/cancelIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: cancelIncidentSuccess,
            error: cancelIncidentError
        });
    }

    function cancelIncidentSuccess(result) {
        //alert(result.d);
        clearVals();
        clearSummary();
        restButtons();
        $('#incidentModal').modal('hide');
        $('#assistReport').hide();
        $('#briefUpdate').hide();
        $('#Summary').hide();
        setStatus('ON PATROL');
        //location.reload();
        localStorage['incidentID'] = null;
        clearLocalIncidentStorage();
        clearLocalAssistStorage();

        $('#btnFindMe').click();
    }

    function cancelIncidentError(error) {
        alert("Close Incident Error: " + error);
    }
});