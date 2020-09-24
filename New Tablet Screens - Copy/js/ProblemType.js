$(document).ready(function(){

	var status = localStorage["logonStatus"];
	var logonTime = localStorage["logonTime"];
	var beat = localStorage["Beat"];
	var callSign = localStorage["CallSign"];
	if(status != "1"){
		window.location.href='Logon.html';
	}
	
	if(localStorage["trafficCollision"] != null && localStorage["trafficCollision"] != "" && localStorage["trafficCollision"] != "NA"){
		setTrafficCollision(localStorage["trafficCollision"]);
	}
	
	if(localStorage["breakdown"] != null && localStorage["breakdown"] != "" && localStorage["breakdown"] != "NA"){
		setBreakdown(localStorage["breakdown"]);
	}
	
	if(localStorage["debrisOnly"] != null && localStorage["debrisOnly"] != "" && localStorage["debrisOnly"] != "NA"){
		setDebrisOnly(localStorage["debrisOnly"]);
	}
	
	if(localStorage["otherProblem"] != null && localStorage["otherProblem"] != "" && localStorage["otherProblem"] != "NA"){
		setDebrisOnly(localStorage["otherProblem"]);
	}

	if(localStorage["problemNotes"] != null && localStorage["problemNotes"] != "" && localStorage["problemNotes"] != "NA"){
		setNoteText(localStorage["problemNotes"]);
	}

	/************************************
	*
	* Pre render selections
	*
	************************************/
	
	function setTrafficCollision(value){
		$('#selTrafficCollision option').each(function(){
			if($(this).text() == value){
				$(this).attr('selected', 'selected');
			}
		});
	}
	
	function setBreakdown(value){
		$('#selBreakdown option').each(function(){
			if($(this).text() == value){
				$(this).attr('selected', 'selected');
			}
		});
	}
	
	function setDebrisOnly(value){
		$('#selDebris option').each(function(){
			if($(this).text() == value){
				$(this).attr('selected', 'selected');
			}
		});
	}
	
	function setOther(value){
		$('#selOther option').each(function(){
			if($(this).text() == value){
				$(this).attr('selected', 'selected');
			}
		});
	}
	
	function setNoteText(value){
		$('#txtOther').val(value);
	}
	
	/************************************
	*
	* Control Events
	*
	************************************/
	
	$('#btnPreAssistReport').click(function(){
		/*
		$.fallr('show', {
			buttons : {
				button1 : {text: 'Yes', danger: true, onclick: navigateAway},
				button2 : {text: 'Cancel', onclick: cancelFallR}
			},
			content : '<p>Cancel out of assist?</p>',
			icon : 'error'
		})*/
		document.location.href = "PreIncident.html";
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
	
	$('#txtOther').on("click", function(){
		$(this).select();
	});
	
	/**********************************
	*
	* Window navigation/var storage
	*
	*********************************/
	
	$(window).on("beforeunload", function(){
		//Persist variables
		localStorage["trafficCollision"] = $('#selTrafficCollision option:selected').text();
		localStorage["breakdown"] = $('#selBreakdown option:selected').text();
		localStorage["debrisOnly"] = $('#selDebris option:selected').text();
		localStorage["otherProblems"] = $('#selOther option:selected').text();
		localStorage["problemNotes"] = $('#txtOther').val();
		if(localStorage["trafficCollision"] == 'Select'){
			localStorage["trafficCollision"] = 'NA';
		}
		if(localStorage["breakdown"] == 'Select'){
			localStorage["breakdown"] = 'NA';
		}
		if(localStorage["debrisOnly"] == 'Select'){
			localStorage["debrisOnly"] = 'NA';
		}
		if(localStorage["otherProblems"] == 'Select'){
			localStorage["otherProblems"] = 'NA';
		}
		if(localStorage["problemNotes"] == 'Further notes'){
			localStorage["problemNotes"] = 'NA';
		}
		var msg = 'Traffic Collision: ' + localStorage["trafficCollision"] + '\r\n';
		msg += 'Breakdown: ' + localStorage["breakdown"] + '\r\n';
		msg += 'Debris Only: ' + localStorage["debrisOnly"] + '\r\n';
		msg += 'Other Problems: ' + localStorage["otherProblems"] + '\r\n';
		msg += 'Problem Notes: ' + localStorage["problemNotes"];
		
		alert(msg);
	});
	
	function cancelFallR(){
		$.fallr('hide');
		return;
	}
	
});