$(document).ready(function(){	

	var msgData = localStorage["assistMessage"];
	$('#AssistInfo').empty();
	$('#AssistInfo').append(msgData);
		
	$('#btnYesMessage1').click(function(){
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
	});
	
	$('#btnNoMessage1').click(function(){
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
	});
	
	$('#btnEnterNumber').click(function(){
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
	});
	
	$('#btnAckWaze').click(function () {
        localStorage["WazeMarker"] = true;
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckWazeMessage';
        var _data = "UUID=" + localStorage.getItem('WazeUUID') + "&Response=true&Accepted=true";
        try {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: AckMsgSuccess1
            });
        }
        catch (error) {
            alert(error);
        }
	});
	
	$('#ddIgnore').change(function () {
        var reason = this.value;
        var _url = ServiceLocation + 'AJAXFSPService.svc/AckWazeMessage';
        var _data = "UUID=" + localStorage.getItem('WazeUUID') + "&Response="+ reason + "&Accepted=false";
        try {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: AckMsgSuccess2
            });
        }
        catch (error) {
            alert(error);
        }
        $('#message1').jqmHide();
    });
	
	function AckMsgSuccess1(result){
		alert('WAZE Acked. Look for Waze icon on Map.');
    }
    
    function AckMsgSuccess2(result){
		alert('Wazw Ignored. Please close this tab.');           
        localStorage["WazeMarker"] = false;        
        localStorage["WazeUUID"] = null;
        localStorage["WazeLat"] = null;
        localStorage["WazeLon"] = null;
	}
});