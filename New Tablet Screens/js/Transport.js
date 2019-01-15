$(document).ready(function(){

	/**************************************
	*
	* Startup and initialization
	*
	**************************************/
	var status = localStorage["logonStatus"];
	var logonTime = localStorage["logonTime"];
	var beat = localStorage["Beat"];
	var callSign = localStorage["CallSign"];
	if(status != "1"){
		window.location.href='Logon.html';
	}

	//pre pop transportation
	if(localStorage["transportation"] != null && localStorage["transportation"] != "" && localStorage["transportation"] != 'NA'){
		var chk = localStorage["transportation"];
		var arr = chk.split(',');
		for(i=0;i<arr.length;i++){
			$('input:checkbox[id=' + arr[i] + ']').attr("checked", true);
		}
	}
	
	//pre pop start odo
	if(localStorage["startODO"] != null && localStorage["startODO"] != "" && localStorage["startODO"] != null && localStorage["startODO"] != 0.0){
		$('#txtStartODO').val(localStorage["startODO"]);
	}
	
	//pre pop end odo
	if(localStorage["endODO"] != null && localStorage["endODO"] != "" && localStorage["endODO"] != null && localStorage["endODO"] != 0.0){
		$('#txtEndODO').val(localStorage["endODO"]);
	}
	
	GetBeats();
	getDropSites(beat);
	
	//override selected beat if necessary
	if(localStorage["dropZoneBeat"] != null && localStorage["dropZoneBeat"] != "" && localStorage["dropZoneBeat"] != null && localStorage["dropZoneBeat"] != 'NA'){
		$('#selBeat option').each(function(){
			if($(this).text() == localStorage["dropZoneBeat"]){
				$(this).attr('selected', 'selected');
			}
		});
	}
	
	//set drop site
	if(localStorage["dropZone"] != null && localStorage["dropZone"] != "" && localStorage["dropZone"] != null && localStorage["dropZone"] != 'NA'){
		$('#selDrop option').each(function(){
			if($(this).text() == localStorage["dropZoneBeat"]){
				$(this).attr('selected', 'selected');
			}
		});
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
        $('#selBeats').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i].BeatID + '">' + _data[i].BeatName + '</option>';
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
	
	/******************************************
	*
	* Button Clicks
	*
	******************************************/
	
	$('#btnPreAssistReport').click(function(){
		document.location.href = "PreIncident.html";
	});
	
	$('#btnStep1').click(function(){
		document.location.href = "ProblemType.html";
	});
	
	$('#btnStep2').click(function(){
		document.location.href = "ActionTaken.html";
	});

	$('#btnStep4').click(function(){
		document.location.href = "Details.html";
	});	
	
	/****************************************
	*
	* Select changes
	*
	****************************************/
	
	$('#selBeat').on('change', function(e){
		var selected = this.value;
		getDropSites(selected);
	});
	
	/****************************************
	*
	* Control focus checks
	*
	****************************************/
	
	$("input[type='text']").on("click", function(){
		$(this).select();
	});
	
	$("input[type='text']").focusout(function(){
		if($(this).val() == ''){
			$(this).val('Please enter');
			}
		});

	/**********************************
	*
	* Load Drop Sites
	*
	**********************************/
	
	function getDropSites(BeatNumber){
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

	function GetDropsSuccess(result){
		var _data = result.d;
        $('#selDrop').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
        }
        $('#selDrop').append(_selCode);
	}
	
	function GetDropsError(error){
		alert(error);
	}
	
	/**********************************
	*
	* Window navigation/var storage
	*
	*********************************/
	
	$(window).on("beforeunload", function(){
	
		var chkValues = $('input[name=action]:checked').map(function(){
			//return this.labels[0].textContent;
			return this.id;
		}).get().join(',');
	
		if(chkValues != ""){
			localStorage["transportation"] = chkValues;
		}
		else{
			localStorage["transportation"] = 'NA';
		}
		
		var stOdo = $('#txtStartODO').val();
		var endOdo = $('#txtEndODO').val();
		
		if(stOdo == null || stOdo == "" || stOdo == 'Please enter'){
			localStorage["startODO"] = 0.0;
		}
		else{
			localStorage["startODO"] = stOdo;
		}
		
		if(endOdo == null || endOdo == "" || endOdo == 'Please enter'){
			localStorage["endODO"] = 0.0;
		}
		else{
			localStorage["endODO"] = endOdo;
		}
		
		//Drop Zone Beat
		localStorage["dropZoneBeat"] = $('#selBeat option:selected').text();
		//Drop Site
		localStorage["dropZone"] = $('#selDrop option:selected').text();
		if(localStorage["dropZone"] == 'Select'){
			localStorage["dropZoneBeat"] = 'NA';
			localStorage["dropZone"] = 'NA';
			localStorage["dropZoneOther"] = 'NA';
		}
		if(localStorage["dropZone"] == 'Other'){
			localStorage["dropZoneOther"] = $('#txtOtherDrop').val();
			if(localStorage["dropZoneOther"] == ''){
				localStorage["dropZoneOther"] = 'Not Entered';
			}
		}
	});
});