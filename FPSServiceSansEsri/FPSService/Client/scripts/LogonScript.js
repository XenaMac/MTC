$(document).ready(function(){

	$('#btnLogon').click(function(){
		localStorage.clear();
		Logon();
	});
	
	$('#txtPassword').live("keypress", function(e){
		if(e.keyCode == 13){
			Logon();
		}
	});

	function Logon(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogon';
		var FSPID = $('#txtFSPID').val();
		var Password = $('#txtPassword').val();
		var _data = "FSPIDNumber=" + FSPID + "&Password=" + Password;
		$.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
			data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetLogonSuccess,
            error: GetLogonError
        });
	}
	
	function GetLogonSuccess(result){
		$('#result').empty();
		var _input = result.d;
		var _splitcheck = _input.indexOf("|");
		if (_splitcheck != -1){
			var splitData = _input.split("|");
			TruckNumber = splitData[1];
			DriverName = splitData[0];
			DriverID = splitData[2];
			TruckID = splitData[3];
			ContractorID = splitData[4];
			ClientStatus = "LoggedOn";
			$('#errLabel').empty();
			$('#errLabel').append('Driver: ' + DriverName + ' is now logged onto Truck ' + TruckNumber);
			localStorage.setItem("logon", "true");
			localStorage.setItem("name", DriverName);
			localStorage.setItem("trucknumber", TruckNumber);
			localStorage.setItem("driverid", DriverID);
			localStorage.setItem("truckid", TruckID);
			localStorage.setItem("contractorid", ContractorID);
			document.location.href = "Status.html";
		}
		else{
			$('#errLabel').empty();
			$('#errLabel').append('<h1>' + _input + '</h1>');
		}
		
	}
	
	function GetLogonError(error){
		ClientStatus = "LoggedOff";
		$('#errLabel').empty();
		$('#errLabel').append(error);
		//alert(error);
	}
});