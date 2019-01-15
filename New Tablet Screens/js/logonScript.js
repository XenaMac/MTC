$(document).ready(function(){

/***************************************
*
* Stuff that auto runs
*
***************************************/
GetBeats();
//GetCallSigns();

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

        $('#selBeats').empty();
        var _selCode = '';
        for (var i = 0; i < beats.length; i++) {
            _selCode += '<option id="' + beats[i] + '">' + beats[i] + '</option>';
        }
        $('#selBeats').append(_selCode);
    }

    function GetBeatsError(result, error) {
		var err = error;
        setError('A problem occurred getting the list of beats, please reload or contact the administrator');
    }
	
//Call Signs - OLD MECHANISM
	
	
	/***************************************
	*
	* Alert Handling
	*
	***************************************/
	
	function setError(error){
		$('errLabel').empty();
		$('errLabel').text(error);
	}
	
	/****************************************
	*
	* Call Sign Data - fire on select change
	*
	****************************************/
	
	$('#selBeats').change(function(){
		var beat = $('#selBeats').find(':selected').text().toString();
		GetSchedules(beat);
	});

    /****************************************
	*
	* Schedules Data - fire on select change from beats
	*
	****************************************/

	function GetSchedules(beat) {
	    var _url = ServiceLocation + 'AJAXFSPService.svc/getschedulenames';
	    var _data = 'beatNumber=' + beat;
	    $.ajax({
	        type: "GET",
	        dataType: "json",
	        url: _url,
	        data: _data,
	        contentType: "application/json; charset=utf-8",
	        success: GetSchedulesSuccess,
	        error: GetSchedulesError
	    });
	}

	function GetSchedulesSuccess(result) {
	    var _data = result.d;
	    $('#selShifts').empty();
	    _data = $.parseJSON(_data);
	    var _selCode = '<option id="">--Select Shift --</option>';
	    for (var i = 0; i < _data.length; i++) {
	        _selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
	    }
	    $('#selShifts').append(_selCode);
	}

	function GetSchedulesError(result, error) {
	    var err = error;
	    setError('A problem occurred getting the list of schedules, please reload or contact the administrator');
	}

	$('#selShifts').change(function () {
	    var beat = $('#selBeats').find(':selected').text().toString();
	    var shift = $('#selShifts').find(':selected').text().toString();
	    GetCallSigns(beat, shift);
	});
	
    /****************************************
	*
	* Callsign Data - fire on select change from schedule
	*
	****************************************/

	function GetCallSigns(beat, shift){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getCallSigns';
		var _data = 'beat=' + beat + "&scheduleName=" + shift;
		$.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetCallSignsSuccess,
            error: GetCallSignsError
        });
	}
	
	function GetCallSignsSuccess(result){
		var _data = result.d;
        $('#selCallSigns').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
        }
        $('#selCallSigns').append(_selCode);
	}
	
	function GetCallSignsError(result, error) {
        var err = error;
		setError('A problem occurred getting the list of call signs, please reload or contact the administrator');
	}

	
	/**************************************
	*
	* Log on process
	*
	**************************************/
	
	$('#btnLogon').click(function(){
		//check for ID
		if ($('#txtFSPID').val() == '') {
            $.fallr('show', {
                content: '<p>Please enter your FSP ID</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }

		//check for password
        if ($('#txtPassword').val() == '') {
            $.fallr('show', {
                content: '<p>Please enter your password</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }

		//check for selected beat
        if ($('#selBeats').find(':selected').text().toString() == '000-000') {
            $.fallr('show', {
                content: '<p>Please select your assigned beat</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }
		
		//check for selected schedule
				//check for selected beat
		var check = $('#selShifts').find(':selected').text().toString();
        if ($('#selShifts').find(':selected').text().toString() == '--Select Shift --') {
            $.fallr('show', {
                content: '<p>Please select your assigned shift</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }
		
		//check for selected call sign
        if ($('#selCallSigns').find(':selected').text().toString() == null) {
            $.fallr('show', {
                content: '<p>Please select your assigned call sign</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }
		
		//check ODO reading
		if($('#txtOdometer').val() == ''){
			$.fallr('show', {
                content: '<p>Please enter your current odometer reading</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
		}
		
		if(isNaN($('#txtOdometer').val())){
			$.fallr('show', {
				content: 'Provided odometer reading is not a valid number</p>',
				position: 'center',
				icon: 'lamp'
			});
			return;
		}
	
		var FSPID = $('#txtFSPID').val();
		var Password = $('#txtPassword').val();
		var Shift = $('#selShifts').find(':selected').text().toString();
		var Beat = $('#selBeats').find(':selected').text().toString();
		var CallSign = $('#selCallSigns').find(':selected').text().toString();
		var Odo = $('#txtOdometer').val();
			
		//Log on to service
		var _data = "FSPIDNumber=" + FSPID + "&Password=" + Password + "&AssignedBeat=" + Beat + "&AssignedShift=" + Shift + "&CallSign=" + CallSign + "&StartODO=" + Odo;
		var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogon';
		$.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetLogonSuccess,
            error: GetLogonError
        });
		
		
		//window.location.href='Status.html';
	});
	
	
	
	function GetLogonSuccess(result){
		$('#result').empty();
		//checkCallSign();
		
        var _input = result.d;
        var _splitcheck = _input.indexOf("|");
        if (_splitcheck != -1) {
            var splitData = _input.split("|");
            TruckNumber = splitData[1];
            DriverName = splitData[0];
            DriverID = splitData[2];
            TruckID = splitData[3];
            ContractorID = splitData[4];
            AssignedBeat = splitData[5];
            LateLogon = splitData[6];
            ClientStatus = "LoggedOn";
            $('#errLabel').empty();
            $('#errLabel').append('Driver: ' + DriverName + ' is now logged onto Truck ' + TruckNumber);
            //localStorage.setItem("logon", "true");
			
			localStorage["logonStatus"] = "1";
			localStorage["logonTime"] = new Date();
			localStorage["FSPID"] = $('#txtFSPID').val();
			localStorage["Beat"] = $('#selBeats').find(':selected').text().toString();
			localStorage["CallSign"] = $('#selCallSigns').find(':selected').text().toString();
			localStorage["TruckStatus"] = "LOGGED ON";
			localStorage["breakTimerTotal"] = 0;
			localStorage["lunchTimerTotal"] = 0;
			var today = new Date();
			var totalBreaks = 0;
			var dow = today.getDay();
			if(today.getDay() == 6 || today.getDay() == 7){
				totalBreaks = 2;
			}
			else{
				totalBreaks = 1;
			}
			localStorage["allottedBreaks"] = totalBreaks;
			localStorage["breaksUsed"] = 0;
			localStorage["breakStarted"] = new Date();
			localStorage["breakEnded"] = new Date();
			localStorage["lunchStarted"] = new Date();
			localStorage["lunchEnded"] = new Date();
			
			localStorage.setItem("name", DriverName);
            localStorage.setItem("trucknumber", TruckNumber);
            localStorage.setItem("driverid", DriverID);
            localStorage.setItem("truckid", TruckID);
            localStorage.setItem("contractorid", ContractorID);
            localStorage.setItem("assignedbeat", AssignedBeat);
			
            if (LateLogon == 'late') {
                window.setTimeout(document.location.href = "Status.html", 2000);
            }
            else {
                window.setTimeout(document.location.href = "Status.html", 2000);
            }
        }
        else {
            $('#errLabel').empty();
            $('#errLabel').append('<h1>' + _input + '</h1>');
        }
	}
	
	function GetLogonError(error){
		$('#errLabel').empty();
		$('#errLabel').append(error);
	}
	
	function checkCallSign(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/makeCallSign';
		var beat = $('#selBeats').find(':selected').text().toString();
		var _data = "beat=" + beat;
		$.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: checkCallSignSuccess,
            error: checkCallSignError
        });
	}
	
	function checkCallSignSuccess(result){
		localStorage["CallSign"] = result.d;
	}
	
	function checkCallSignError(error){
		alert(error);
	}
});