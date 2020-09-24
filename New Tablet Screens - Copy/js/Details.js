$(document).ready(function () {

    $('#txtState').val('CA');
    $('#NPAR').hide();
    var lb = document.getElementById("txtState");
    if (lb != null) {
        var options = lb.options;
        for (var i = options.length - 1; i > 0; i--) {
            if (i == 4) {
                options[i].selected == true
            }
        }
    }

    /***************************
	*
	* Start Up Functions
	*
	***************************/

    var closeWindow;
    var status = localStorage["logonStatus"];
    var logonTime = localStorage["logonTime"];
    var beat = localStorage["Beat"];
    var callSign = localStorage["CallSign"];
    if (status != "1") {
        window.location.href = 'Logon.html';
    }

    if (localStorage["IncidentID"] == null || localStorage["IncidentID"] == '' || localStorage["IncidentID"] == undefined) {
        $('#submitbtns').hide();
        $('#NPAR').show();
    }

    if (localStorage["state"] != null && localStorage["state"] != 'NA') {
        $('#txtState').val(localStorage["state"]);
    }

    if (localStorage["licensePlate"] != null && localStorage["licensePlate"] != 'NA') {
        $('#txtLicense').val(localStorage["licensePlate"]);
    }

    if (localStorage["vehicleType"] != null && localStorage["vehicleType"] != 'NA') {
        $('#selVehicleType option').each(function () {
            if ($(this).text() == localStorage["vehicleType"]) {
                $(this).attr('selected', 'selected');
            }
        });
    }

    if (localStorage["otAuthNum"] != null && localStorage["otAuthNum"] != 'NA') {
        $('#txtOvertime').val(localStorage["otAuthNum"]);
    }

    if (localStorage["detailNotes"] != null && localStorage["detailNotes"] != 'NA') {
        $('#txtOther').val(localStorage["detailNotes"]);
    }

    /*************************************
	*
	* Button Clicks
	*
	***********************************/

    $('#btnPreAssistReport').click(function () {
        document.location.href = "PreIncident.html";
    });

    $('#btnStep1').click(function () {
        document.location.href = "ProblemType.html";
    });

    $('#btnStep2').click(function () {
        document.location.href = "ActionTaken.html";
    });

    $('#btnStep3').click(function () {
        document.location.href = "Transport.html";
    });

    $('#btnDone').click(function () {
        setStatus('ON PATROL');
		setTimeout(function () { close_window() }, 1250);
        clearAllLocalStorage();
        
    });

    $('#btnAddMore').click(function () {
        clearAssistLocalStorage();
        document.location.href = "PreIncident.html";
    });

    $('#btnSubmit').click(function () {
        if (localStorage["trafficCollision"] == "NA" && localStorage["breakdown"] == "NA" && localStorage["debrisOnly"] == "NA" && localStorage["otherProblems"] == "NA" && localStorage["problemNotes"] == "NA") {
            alert('You have not entered any Problem Types.');
            return;
        } else if (localStorage["actionTaken"] == null || localStorage["actionTaken"] == "" || localStorage["actionTaken"] == "NA") {
            alert('You have not entered any Actions Taken.');
            return;
        } else if (localStorage["transportation"] == null || localStorage["transportation"] == "") {
            localStorage["actionTaken"] == "NA"
        } else {
            $.fallr('show', {
                buttons: {
                    button1: { text: 'Yes', danger: true, onclick: clickSubmit },
                    button2: { text: 'Cancel', onclick: cancelFallR }
                },
                content: '<p>Submit Assist Report?</p>',
                icon: 'error'
            });
        }
    });

    /*********************************
	*
	* Post Assist Information
	*
	*********************************/

    function postAssist() {
        setVars();
        //split out Actions Taken:
        var chk = localStorage["actionTaken"];
        var arr = chk.split(',');
        var actionsTaken = '[';
        for (i = 0; i < arr.length; i++) {
            actionsTaken += '"' + arr[i] + '",';
        }
        actionsTaken = actionsTaken.substring(0, actionsTaken.length - 1);
        actionsTaken += ']';

        chk = localStorage["transportation"];
        arr = chk.split(',');
        var transports = '[';
        for (i = 0; i < arr.length; i++) {
            transports += '"' + arr[i] + '",';
        }
        transports = transports.substring(0, transports.length - 1);
        transports += ']';

        var _assistData;
        _assistData = {

            TrafficCollision: localStorage["trafficCollision"],
            Breakdown: localStorage["breakdown"],
            DebrisOnly: localStorage["debrisOnly"],
            Other: localStorage["otherProblems"],
            ProblemNote: localStorage["problemNotes"],
            //ActionTaken: actionsTaken,
            OtherNote: localStorage["atOther"],
            //TransportType: transports,
            PStartODO: localStorage["startODO"],
            PEndODO: localStorage["endODO"],
            DropSiteBeat: localStorage["dropZoneBeat"],
            DropSite: localStorage["dropZone"],
            DropSiteOther: localStorage["dropZoneOther"],
            State: localStorage["state"],
            LicPlateNum: localStorage["licensePlate"],
            VehicleType: localStorage["vehicleType"],
            OTAuthNum: localStorage["otAuthNum"],
            DetailNote: localStorage["detailNotes"],
            Lat: localStorage["lat"],
            Lon: localStorage["lon"],
            Freeway: localStorage["freeway"]

        };

        var assistString = JSON.stringify(_assistData);
        assistString = assistString.substring(0, assistString.length - 1);
        assistString += ',"ActionTaken":' + actionsTaken + ',';
        assistString += '"TransportType":' + transports + '}';
        var _sendData = "incidentID=" + localStorage["IncidentID"] + "&driverName=" + localStorage["name"] + "&data=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/postMTCAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: postAssistSuccess,
            error: postAssistError
        });
    }

    function postAssistSuccess(result) {
        var incID = result.d;
        localStorage["IncidentID"] = incID;
        alert('Post submitted');
        $('#btnDone').toggleClass("assistDisabled", "remove");
        $('#btnDone').toggleClass("assistDone", "add");
        $('#btnDone').prop("disabled", "");
    }

    function postAssistError(error) {
        alert(error);
    }

    /*********************************
	*
	* Helper functions
	*
	*********************************/

    function setStatus(status) {
        var message = 'Selected ' + status;
        //$('#statusName').emtpy();
        localStorage["TruckStatus"] = status;

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
        //showFallR(message);
    }

    function SetStatusSuccess(result) {
        $('#result').empty();
        var _input = result.d;
        if (closeWindow == 'T') {
            window.close();
        }
    }

    function SetStatusError(error) {
        //alert(error);
    }

    function cancelFallR() {
        alert('operation aborted');
        $.fallr('hide');
    }

    var clickSubmit = function () {
        $.fallr('hide');
        postAssist();
        alert('Assist Report Posted, use the buttons at the bottom to finish or add another assist report');
    }

    function clearAllLocalStorage() {

        localStorage["preAssistDirection"] = "";
        localStorage["preAssistBeat"] = "";
        localStorage["preAssistFSPLocation"] = "";
        localStorage["preAssistDispatchLocation"] = "";
        localStorage["preAssistPosition"] = "";
        localStorage["preAssistLane"] = "";
        localStorage["preAssistIncidentType"] = "";
        localStorage["preAsistCHPLogNum"] = "";
        localStorage["preAssistSurveyNum"] = "";
        localStorage["IncidentID"] = "";

        localStorage["trafficCollision"] = "";
        localStorage["breakdown"] = "";
        localStorage["debrisOnly"] = "";
        localStorage["otherProblems"] = "";
        localStorage["problemNotes"] = "";

        localStorage["actionTaken"] = "";
        localStorage["atOther"] = "";

        localStorage["transportation"] = "";
        localStorage["startODO"] = 0.0;
        localStorage["endODO"] = 0.0;
        localStorage["dropZoneBeat"] = "";
        localStorage["dropZone"] = "";
        localStorage["dropZoneOther"] = "";

        localStorage["state"] = "";
        localStorage["licensePlate"] = "";
        localStorage["vehicleType"] = "";
        localStorage["otAuthNum"] = "";
        localStorage["detailNotes"] = "";
		localStorage["CHPLogNumber"] = "";

        $('#txtState').val("");
        $('#txtLicense').val("");
        $('#selVehicleType option:selected').text('Select');
        $('#txtOvertime').val("");
        $('#txtOther').val('Further notes')
        var astReqWindow = window.open("AssistRequest.html", "ASSISTREQUEST");
        astReqWindow.close();
        close_window();
    }

    function findWindow(name) {

    }

    function clearAssistLocalStorage() {

        localStorage["trafficCollision"] = "";
        localStorage["breakdown"] = "";
        localStorage["debrisOnly"] = "";
        localStorage["otherProblems"] = "";
        localStorage["problemNotes"] = "";

        localStorage["actionTaken"] = "";
        localStorage["atOther"] = "";

        localStorage["transportation"] = "";
        localStorage["startODO"] = 0.0;
        localStorage["endODO"] = 0.0;
        localStorage["dropZoneBeat"] = "";
        localStorage["dropZone"] = "";
        localStorage["dropZoneOther"] = "";

        localStorage["state"] = "";
        localStorage["licensePlate"] = "";
        localStorage["vehicleType"] = "";
        localStorage["otAuthNum"] = "";
        localStorage["detailNotes"] = "";
		localStorage["CHPLogNumber"] = "";

        $('#txtState').val("");
        $('#txtLicense').val("");
        $('#selVehicleType option:selected').text('Select');
        $('#txtOvertime').val("");
        $('#txtOther').val('Further notes')

        document.location.href = "PreIncident.html";
    }

    function close_window() {
        var newWindow = window.open('', '_self', ''); //open the current window
        window.close();
    }

    function setVars() {
        localStorage["state"] = $('#txtState').val();
        localStorage["licensePlate"] = $('#txtLicense').val();
        localStorage["vehicleType"] = $('#selVehicleType option:selected').text();
        localStorage["otAuthNum"] = $('#txtOvertime').val();
        localStorage["detailNotes"] = $('#txtOther').val();

        if (localStorage["state"] == "") {
            localStorage["state"] = 'NA';
        }

        if (localStorage["licensePlate"] == "") {
            localStorage["licensePlate"] = 'NA';
        }

        if (localStorage["vehicleType"] == "Select") {
            localStorage["vehicleType"] = 'NA';
        }

        if (localStorage["otAuthNum"] == "" || localStorage["otAuthNum"] == 'Please enter') {
            localStorage["otAuthNum"] = 'NA';
        }
        if (localStorage["detailNotes"] == 'Further notes' || localStorage["detailNotes"] == '') {
            localStorage["detailNotes"] = 'NA';
        }
    }

    /****************************************
	*
	* Control focus checks
	*
	****************************************/

    $("input[type='text']").on("click", function () {
        $(this).select();
    });

    $("input[type='text']").focusout(function () {
        if ($(this).val() == '') {
            $(this).val('Please enter');
        }
    });

    $('#txtOther').on("click", function () {
        $(this).select();
    });

    /**********************************
	*
	* Window navigation/var storage
	*
	*********************************/

    $(window).on("beforeunload", function () {
        setVars();
    });
});