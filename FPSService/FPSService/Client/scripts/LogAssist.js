$('#LogAssist').live('pageshow', function(){
	//pageshow does not appear to be used, leaving for the time being
	//$('#LogAssistHeader').empty();
	//$('#LogAssistHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckID);
});

$('#LogAssist').live('pageinit', function(event){
	$('#LogAssistHeader').empty();
	$('#LogAssistHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckID);
});