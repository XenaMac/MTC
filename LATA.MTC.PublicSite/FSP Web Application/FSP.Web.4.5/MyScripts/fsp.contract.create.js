/// <reference path="../Scripts/knockout-2.2.1.js" />
/// <reference path="../Scripts/jquery.validate-vsdoc.js" />

var ContractCreateViewModel = function () {

    var self = this;

    self.beats = ko.observableArray([]);
    self.selectedBeats = ko.observableArray([]);

    self.selectedBeatId = ko.observable();

    self.loadBeats = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/Beats/GetAll';

        self.beats([]);
        
        $.getJSON(url, function (dbbeats) {
            console.log(dbbeats);            
            $.each(dbbeats, function (i, dbbeat) {
                self.beats.push(new UIBeat(dbbeat.Id, dbbeat.Text));
            });
        });
    }

    self.loadSelectedBeats = function () {
        var contractId = $("#Contract_ContractID").val();
        var url = $("#websitePath").attr("data-websitePath") + '/Beats/GetBeatsByContractId?id=' + contractId;

        self.selectedBeats([]);

        $.getJSON(url, function (dbbeats) {
            console.log('Selected Beats: ' + dbbeats);           
            $.each(dbbeats, function (i, dbbeat) {
                self.selectedBeats.push(new UIBeat(dbbeat.Id, dbbeat.Text));
            });
        });
    }

    self.addBeat = function () {

        //check if user has this beat already selected
        var selectedBeatIdExists = ko.utils.arrayFirst(self.selectedBeats(), function (i) { return i.id === self.selectedBeatId(); });

        if (!selectedBeatIdExists) {
            //get the actual beat object from the main list
            var uiBeat = ko.utils.arrayFirst(self.beats(), function (i) { return i.id === self.selectedBeatId(); });
            self.selectedBeats.push(uiBeat);
        }
    }

    self.removeSelectedBeat = function (beat) {

        var uiBeat = ko.utils.arrayFirst(self.selectedBeats(), function (i) { return i.id === beat.id; });
        self.selectedBeats.remove(function (i) { return i.id === beat.id; });
    }

    function UIBeat(id, text) {

        var self = this;

        self.id = id;
        self.text = text;
      
    }

}

$(function () {

    var contractCreateViewModel = new ContractCreateViewModel();

    //apply binding to ko
    ko.applyBindings(contractCreateViewModel);

    contractCreateViewModel.loadBeats();
    contractCreateViewModel.loadSelectedBeats();

    $("#contractForm").validate({
        rules: {
            "Contract.AgreementNumber": {
                required: true
            },
            "Contract.ContractorID": {
                required: true
            }
        },
        errorPlacement: function (error, element) {
            error.insertBefore(element)
            error.addClass('error');  // add a class to the wrapper       
        }

    });

});