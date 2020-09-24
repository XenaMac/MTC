$(document).ready(function(){
	var map;
	var myLaLon;
	var marker;
	
	new GMaps({
		div: "#mapDiv",
		lat: -25.363,
		lng: 131.044
	})

	function initMap(){
		myLaLon = {lat: -25.363, lng: 131.044};
		map = new google.maps.Map(document.getElementById('mapDiv'), {
			zoom: 4,
			center: myLaLon
		});
		marker = new google.maps.Marker({
			position: myLaLon,
			map:map
		});
	}
	
});