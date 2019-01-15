$(document).ready(function(){
	var mobileDemo = { 'center': '57.7973333,12.0502107', 'zoom': 10 };
	try{

	//$('#map_canvas').gmap({ 'center': '42.345573,-71.098326' });
	}
	catch(error){
		alert(error);
	}
	/*
	$('#HomeScreen').live('pageinit', function() {
				demo.add('basic_map', function() {
					$('#map_canvas').gmap({'center': mobileDemo.center, 'zoom': mobileDemo.zoom, 'disableDefaultUI':true, 'callback': function() {
						var self = this;
						self.addMarker({'position': this.get('map').getCenter() }).click(function() {
							self.openInfoWindow({ 'content': 'Hello World!' }, this);
						});
					}}); 
				}).load('basic_map');
			});
			
			$('#HomeScreen').live('pageshow', function() {
				demo.add('basic_map', function() { $('#map_canvas').gmap('refresh'); }).load('basic_map');
			});
	*/
	
	$(function() {
        // Also works with: var yourStartLatLng = '59.3426606750, 18.0736160278';
        var yourStartLatLng = new google.maps.LatLng(59.3426606750, 18.0736160278);
        $('#map_canvas').gmap({'center': yourStartLatLng});
     });
	 /*
	$('#HomeScreen').live("pageshow", function() {
		$('#map_canvas').gmap('refresh');
    });
    $('#HomeScreen').live("pageinit", function() {
        $('#map_canvas').gmap({'center': '59.3426606750, 18.0736160278'});
    });
	*/
});