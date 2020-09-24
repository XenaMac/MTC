$(document).ready(function(){
	$('#AssistInfo').empty();
	$('#AssistInfo').append(localStorage["MessageText"]);
	
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
	
	function AckMsgSuccess(result){
		alert('Acked');
	}
});