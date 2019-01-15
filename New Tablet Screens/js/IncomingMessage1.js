$(document).ready(function(){
	
	$('#AssistInfo').empty();
	$('#AssistInfo').append(localStorage["MessageText"]);
	
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
	
	/*unused
	$('#btnAckMessage').click(function(){
		//alert('Acked');
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
            alert(error);
        }
	});
	*/
	function AckMsgSuccess(result){
		alert('Acked');
	}
});