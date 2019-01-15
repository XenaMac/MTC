$(document).ready(function(){
	var msgData = localStorage["assistMessage"];
	if(msgData == null || msgData.length == 0)
	{
		msgData = 'NO ASSIST TO REPORT';
	}
	$('#AssistInfo').empty();
	$('#AssistInfo').append(msgData);
	$('#btnAckAssist').click(function(){
		ackAssistRequest();
	});
	
	function ackAssistRequest(){
		if(localStorage["IncidentID"] == null){
			alert('No Incident to report');
			return;
		}
	
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
	
	function ackSuccess(result){
		setStatus('ENROUTE')
		alert('Request Acknowledged.\n\nClose this window when you are finished with it');
	}
	
	function ackError(error){
		alert('An error has been detected');
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
		//checkStatus();
		//showFallR(message);
	}
	
	function SetStatusSuccess(result) {
        $('#result').empty();
        var _input = result.d;
        //GetTruckStatus();
    }
	
	function SetStatusError(error) {
        //alert(error);
    }
});