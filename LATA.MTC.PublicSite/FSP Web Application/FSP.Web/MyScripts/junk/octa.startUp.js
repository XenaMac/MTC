/// <reference path="octa.constants.js" />
/// <reference path="octa.mapViewModel.js" />
/// <reference path="octa.truckCollection.js" />

$(function () {

	try {
		fsp = new octa.FSP();
		
		
		fsp.setPersons();
		fsp.initializeMap();


		//apply binding to ko
		ko.applyBindings(fsp);
	} catch (e) {

	}
	

});