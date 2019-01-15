$(document).ready(function(){

    /***************************************
	*
	* Start up, log on checks, control pops
	*
	***************************************/
	
    var status = localStorage["logonStatus"];
    var logonTime = localStorage["logonTime"];
    var beat = localStorage["Beat"];
    var callSign = localStorage["CallSign"];
    var closeWindow;
    $('#loadingGif').hide();
	
    //hide or show in-lane number on start
    if (localStorage["preAssistPosition"] == 'In-Lane') {
        $('#inlaneDiv').show();
    } else {
        $('#inlaneDiv').hide();
    }
	
    if(status != "1"){
        window.location.href='Logon.html';
    }
	
	getIncidentTypes();
    GetBeats();
    getFreeways(beat);
    getLocation();
    getSurveyNumber();

    $('#selBeat').on('change', function () {
        //HIDE AND SHOW
        $('#selFreeway').hide('fade');
        $('#loadingGif').show('fade');
        getFreeways($('#selBeat').val());
    });
	
	
    if(localStorage["preAssistDirection"] != null && localStorage["preAssistDirection"] != ""){
        $("input[name=Direction][value=" + localStorage["preAssistDirection"] + "]").prop('checked', true);
    }
	
    if(localStorage["preAssistFSPLocation"] != null && localStorage["preAssistFSPLocation"] != ""){
        $('#txtLocation').val(localStorage["preAssistFSPLocation"]);
    }
	
    if(localStorage["preAssistDispatchLocation"] != null && localStorage["preAssistDispatchLocation"] != ""){
        $('#txtDispatchLocation').val(localStorage["preAssistDispatchLocation"]);
    }
	
    if(localStorage["preAssistPosition"] != null && localStorage["preAssistPosition"] != ""){
        $("input[name=position][value=" + localStorage["preAssistPosition"] + "]").prop('checked', true);
    }
	
    if(localStorage["preAssistLane"] != null && localStorage["preAssistLane"] != ""){
        $('#txtLane').val(localStorage["preAssistLane"]);
    }
	
	if(localStorage["CHPLogNumber"] != null && localStorage["CHPLogNumber"] != ""){
		$('#txtCHPLogNum').val(localStorage["CHPLogNumber"]);
	}
	
    if(localStorage["preAssistIncidentType"] != null && localStorage["preAssistIncidentType"] != ""){
        $('#selIncidentType option').each(function(){
            if($(this).text() == localStorage["preAssistIncidentType"]){
                $(this).attr('selected', 'selected');
            }
        });
    }
	
    if(localStorage["preAsistCHPLogNum"] != null && localStorage["preAsistCHPLogNum"] != ""){
        $('#txtCHPLogNum').val(localStorage["preAsistCHPLogNum"]);
    }
	
    if(localStorage["preAssistSurveyNum"] != null && localStorage["preAssistSurveyNum"] != ""){
        $('#txtIncidentSurveyNum').val(localStorage["preAssistSurveyNum"]);
    }
	
	function getIncidentTypes(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getDispatchCodes';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			contentType: "application/json; charset=utf-8",
			success: getIncidentTypesSuccess,
			error: getIncidentTypesError
		});
	}
	
	function getIncidentTypesSuccess(result){
		var _data = $.parseJSON(result.d);
		var _selCode = '';
		for(var i = 0;i<_data.length;i++){
			_selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
		}
		_selCode += '<option id="Other">Other</option>';
		$('#selIncidentType').empty();
		$('#selIncidentType').append(_selCode);
	}
	
	function getIncidentTypesError(error){
		alert(error);
	}
	
    //disableButtons();
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
	
    function getFreewaysSuccess(result){
        $('#selFreeway').empty();
        var _data = result.d;
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
        }
        $('#selFreeway').append(_selCode);

        //HIDE AND SHOW
        $('#loadingGif').hide('fade');
        $('#selFreeway').show('fade');
    }
	
    function getFreewaysError(error){
        alert(error);
    }
	
    function getLocation(){
        var _url = ServiceLocation + 'AJAXFSPService.svc/findServiceLocation';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getLocationSuccess,
            error: getLocationError
        });
    }
	
    function getLocationSuccess(result){
        localStorage["preAssistFSPLocation"] = result.d;
        $('#txtLocation').val(result.d);
    }
	
    function getLocationError(error){
        alert(error);
    }
	
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
            beats.sort(function (a, b) { return a - b });
        }

        $('#selBeats').empty();
        var _selCode = '';
        for (var i = 0; i < beats.length; i++) {
            _selCode += '<option id="' + beats[i] + '">' + beats[i] + '</option>';
        }
        $('#selBeat').append(_selCode);
        setDefaultBeat();
    }

    function GetBeatsError(result, error) {
        var err = error;
        setError('A problem occurred getting the list of beats, please reload or contact the administrator');
    }

    function setDefaultBeat(){
        $('#selBeat option').each(function(){
            if($(this).text() == beat){
                $(this).attr('selected', 'selected');
            }
        });
    }
	
    function disableButtons(){
        if(localStorage["initAstSubmitted"] == "F"){
            $('#btnStep1').prop("disabled", true);
            $('#btnStep2').prop("disabled", true);
            $('#btnStep3').prop("disabled", true);
            $('#btnStep4').prop("disabled", true);
            $('#btnOnTow').prop("disabled", true);
        }
        else{
            $('#btnStep1').prop("disabled", false);
            $('#btnStep2').prop("disabled", false);
            $('#btnStep3').prop("disabled", false);
            $('#btnStep4').prop("disabled", false);
            $('#btnOnTow').prop("disabled", false);
        }
    }

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
        $('#txtIncidentSurveyNum').empty();
        $('#txtIncidentSurveyNum').val(result.d);
		localStorage["SurveyNumber"] = result.d;
		localStorage["preAssistSurveyNum"] = result.d;
    }

    function getSurveyNumberError(error) {
        alert(error);
    }
	
	
    /************************************
	*
	* Button Clicks
	*
	************************************/
	
    $('#btnLocate').click(function(){
        $.fallr('show', {
            buttons : {
                button1 : {text: 'Yes', danger: true, onclick: clickOK},
                button2 : {text: 'Cancel', onclick: cancelFallR}
            },
            content : '<p>Do you want to post unable to find?</p>',
            icon : 'error'
        });
    });
	
    var clickOK = function(){
        $.fallr('hide');
        closeWindow = 'T';
        setStatus('ON PATROL');
    }
	
    $('#btnOnTow').click(function(){
        $.fallr('show', {
            buttons : {
                button1 : {text: 'Yes', danger: true, onclick: clickTow},
                button2 : {text: 'Cancel', onclick: cancelFallR}
            },
            content : '<p>Start towing?</p>',
            icon : 'error'
        });
    });
	
    var clickTow = function(){
        $.fallr('hide');
        closeWindow = 'F';
        setStatus('ON TOW');
    }
	
    var clickSubmit = function(){
        $.fallr('hide');
        postPreAssist();

    }
	
    $('#btnSubmit').click(function(){
        $.fallr('show', {
            buttons : {
                button1 : {text: 'Yes', danger: true, onclick: clickSubmit},
                button2 : {text: 'Cancel', onclick: cancelFallR}
            },
            content : '<p>Submit Pre-Assist Report?</p>',
            icon : 'error'
        });
    });
	
    var clickCancel = function(){
        $.fallr('hide');
        closeWindow = 'T';
        setStatus('ON PATROL');
    }
	
    $('#btnCancelled').click(function(){
        $.fallr('show', {
            buttons : {
                button1 : {text: 'Yes', danger: true, onclick: clickCancel},
                button2 : {text: 'Cancel', onclick: cancelFallR}
            },
            content : '<p>Cancel out of assist?</p>',
            icon : 'error'
        });
    });
	
    function cancelFallR(){
        $.fallr('hide');
    };

    $("input:radio[name='position']").click(function () {
        //alert($(this).val());
        if ($(this).val() == "In-Lane") {
            $('#inlaneDiv').show('slideDown');
        } else {
            $('#inlaneDiv').hide('slideUp');
        }
    });
	
	/************************************
	*
	* Control Value Events
	*
	*************************************/
	
	$("input[type='text']").on("click", function(){
	    $(this).select();
	});
	
    $('#txtLocation').focusout(function(){
        if($(this).val() == ''){
            $(this).val('Please enter');
        }
    });
				
    $('#txtDispatchLocation').focusout(function(){
        if($(this).val() == ''){
            $(this).val('Please enter');
        }
    });
				
    $('#txtLane').focusout(function(){
        if($(this).val() == ''){
            $(this).val('Please enter');
        }
    });
				
    $('#txtCHPLogNum').focusout(function(){
        //if($(this).val() == ''){
        //    $(this).val('Please enter');
        //}
    });
				
    $('#txtIncidentSurveyNum').focusout(function(){
        //if($(this).val() == ''){
        //    $(this).val('Please enter');
        //}
    });
		
    $('#btnStep1').click(function(){
        document.location.href = "ProblemType.html";
    });
	
    $('#btnStep2').click(function(){
        document.location.href = "ActionTaken.html";
    });

    $('#btnStep3').click(function(){
        document.location.href = "Transport.html";
    });

    $('#btnStep4').click(function(){
        document.location.href = "Details.html";
    });	
	
    /************************
	*
	* Submit
	*
	************************/
	
    function postPreAssist(){
        localStorage["initAstSubmitted"] == "T";
        btnStep1
		
        //Direction
        var selectedDirection = $("input[type='radio'][name='Direction']:checked");
        if(selectedDirection.length > 0){
            localStorage["preAssistDirection"] = selectedDirection.val();
        }
        else{
            alert('You must select a highway direction');
            return;
        }
		
        //Beat
        localStorage["preAssistBeat"] = $('#selBeat option:selected').text();

        //Frreway
        localStorage["freeway"] = $('#selFreeway option:selected').text();

        if (localStorage["freeway"] == '' || localStorage["freeway"] == '-- Select a Highway --') {
            alert('You must enter a valid Highway');
            return;
        }
		
        //FSP Location
        localStorage["preAssistFSPLocation"] = $('#txtLocation').val();
		
        if(localStorage["preAssistFSPLocation"] == 'Please enter' || localStorage["preAssistFSPLocation"] == ""){
            alert('You must enter a valid FSP Location');
            return;
        }
		
        //Dispatch Location
        localStorage["preAssistDispatchLocation"] = $('#txtDispatchLocation').val();
		
        if(localStorage["preAssistDispatchLocation"] == 'Please enter' || localStorage["preAssistDispatchLocation"] == ""){
            alert('You must enter a valid dispatch location');
            return;
        }
				
        //Position
        var selectedPosition = $("input[type='radio'][name='position']:checked");
        if(selectedPosition.length > 0){
            localStorage["preAssistPosition"] = selectedPosition.val();
        }
        else{
            alert('You must select a postion');
            return;
        }
		
        if(localStorage["preAssistPosition"] == 'In-Lane'){
            localStorage["preAssistLane"] = $('#txtLane').val();
            if (localStorage["preAssistLane"] == "Enter Lane #" || localStorage["preAssistLane"] == "NA" || localStorage["preAssistLane"] == "") {
                alert('You must enter a valid lane number');
                return;
            }
        }
        else{
            localStorage["preAssistLane"] = 'NA';
        }
		
	
        //Incident Type
        localStorage["preAssistIncidentType"] = $('#selIncidentType option:selected').text();
		
        //CHP Log Number
        localStorage["preAsistCHPLogNum"] = $('#txtCHPLogNum').val();
		
        if(localStorage["preAsistCHPLogNum"] == 'CHP Log #' || localStorage["preAsistCHPLogNum"] == "Please enter" || localStorage["preAsistCHPLogNum"] == ""){
            //alert('You must enter a valid CHP Log Number');
            localStorage["preAsistCHPLogNum"] = "NA";
        }
		
        //Incident Survey Num
        localStorage["preAssistSurveyNum"] = $('#txtIncidentSurveyNum').val();
		
        if(localStorage["preAssistSurveyNum"] == 'Incident Survey #' || localStorage["preAssistSurveyNum"] == "Please enter" || localStorage["preAssistSurveyNum"] == ""){
            //alert('You must enter a valid survey number');
            localStorage["preAssistSurveyNum"] = "NA";
        }
        if(localStorage["IncidentID"] == null || localStorage["IncidentID"] == ""){
            localStorage["IncidentID"] = makeGuid();
        }
		
        var _assistData;
        _assistData = {
			
            Direction: localStorage["preAssistDirection"],
            Beat: localStorage["preAssistBeat"],
            FSPLocation: localStorage["preAssistFSPLocation"],
            LocationofInitialDispatch: localStorage["preAssistDispatchLocation"],
            Position: localStorage["preAssistPosition"],
            LaneNumber: localStorage["preAssistLane"],
            CHPIncidentType: localStorage["preAssistIncidentType"],
            CHPLogNumber: localStorage["preAsistCHPLogNum"],
            IncidentSurveyNumber: localStorage["preAssistSurveyNum"],
            Lat: localStorage["lat"],
            Lon: localStorage["lon"],
            Freeway: localStorage["freeway"]
        };
		
        //string _incidentID, string _driverName, MTCPreAssistData _data
        //var _data = "FSPIDNumber=" + FSPID + "&Password=" + Password + "&AssignedBeat=" + AssignedBeat;
        var assistString = JSON.stringify(_assistData);
		
        var _sendData = "incidentID=" + localStorage["IncidentID"] + "&driverName=" + localStorage["name"] + "&data=" + assistString;
		
        var _url = ServiceLocation + 'AJAXFSPService.svc/postMTCPreAssist';
		
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
	
    function postAssistSuccess(result){
        var incID = result.d;
        localStorage["IncidentID"] = incID;
        var msg = 'Direction: ' + localStorage["preAssistDirection"] + '\r\n';
        msg += 'Beat: ' + localStorage["preAssistBeat"] + '\r\n';
        msg += 'FSP Location: ' + localStorage["preAssistFSPLocation"] + '\r\n';
        msg += 'Dispatch: ' + localStorage["preAssistDispatchLocation"] + '\r\n';
        msg += 'Position: ' + localStorage["preAssistPosition"] + '\r\n';
        msg += 'Lane Num: ' + localStorage["preAssistLane"] + '\r\n';
        msg += 'Incident Type: ' + localStorage["preAssistIncidentType"] + '\r\n';
        msg += 'CHP Log Num: ' + localStorage["preAsistCHPLogNum"] + '\r\n';
        msg += 'Survey Num: ' + localStorage["preAssistSurveyNum"] + '\r\n';
        msg += 'IncidentID: ' + localStorage["IncidentID"];
        alert('Post submitted')//'\r\n' + msg);
        document.location.href = "ProblemType.html";
    }
	
    function postAssistError(error){
        alert(error);
    }
	
    /************************
	*
	* Status Changes
	*
	************************/
	
    function setStatus(status){
        var message = 'Selected ' + status;
        //$('#statusName').emtpy();
        localStorage["TruckStatus"] = status;
        $('#statusName').text(status);
        //tell the service truck status has changed
        var logonStatus = localStorage["logonStatus"];
        if(logonStatus != "1"){
            localStorage.clear();
            window.location.href='Logon.html';
        }
        else{
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
        if(closeWindow == 'T'){
            window.close();
        }
    }
	
    function SetStatusError(error) {
        //alert(error);
    }
	
    /****************************
	*
	* Helper Functions
	*
	****************************/
	
    function S4(){
        return (((1+Math.random())*0x10000)|0).toString(16).substring(1); 
    }
	
    function makeGuid(){
        var guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0,3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        return guid;
    }
});