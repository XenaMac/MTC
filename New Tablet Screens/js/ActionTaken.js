$(document).ready(function () {

	var status = localStorage["logonStatus"];
	var logonTime = localStorage["logonTime"];
	var beat = localStorage["Beat"];
	var callSign = localStorage["CallSign"];
	
	if(localStorage["actionTaken"] != null && localStorage["actionTaken"] != "" && localStorage["actionTaken"] != 'NA'){
		//split the string and check the boxes
		var chk = localStorage["actionTaken"];
		var arr = chk.split(',');
		for(i=0;i<arr.length;i++){
			$('input:checkbox[id=' + arr[i] + ']').attr("checked", true);
		}
	}
	
	if(localStorage["atOther"] != "" && localStorage["atOther"] != 'NA'){
		$('#txtOther').val(localStorage["atOther"]);
	}
	
	if(status != "1"){
		window.location.href='Logon.html';
	}

	$('#btnPreAssistReport').click(function(){
		document.location.href = "PreIncident.html";
	});
	
	$('#btnStep1').click(function(){
		document.location.href = "ProblemType.html";
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
	
	/*
	localStorage["actionTaken"] = "";
	localStorage["atOther"] = "";
	*/
	
	$(window).on("beforeunload", function(){
		var chkValues = $('input[name=action]:checked').map(function(){
			//return this.labels[0].textContent;
			return this.id;
		}).get().join(',');
		
		if(chkValues != ""){
			localStorage["actionTaken"] = chkValues;
		}
		else{
			localStorage["actionTaken"] = 'NA';
		}
		
		var otherNotes = $('#txtOther').val();
		
		if(otherNotes != 'Further notes' && otherNotes != ''){
			localStorage["atOther"] = otherNotes;
		}
		else{
			localStorage["atOther"] = 'NA';
		}
	});
});