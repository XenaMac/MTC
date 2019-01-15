/// <reference path="../Scripts/knockout-2.1.0.js" />

var SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
var viewModel;


var MyViewModel = function () {

    var self = this;
    self.currentSort = ko.observable("beatNumber");
    self.currentSortDirection = ko.observable("Asc");


    self.properties = ko.observableArray([
        new property(self, "First Name", "firstName"),
        new property(self, "Last Name", "lastName"),
        new property(self, "DOB", "dob"),
    ]);

    function property(root, name, value) {
        var self = this;

        self.name = ko.observable(name);
        self.value = ko.observable(value);

        self.sort = function (item) {

            alert(item.value())


            if (self.currentSortDirection() === 'Asc')
                self.currentSortDirection('Desc');
            else
                self.currentSortDirection('Asc');


            if (self.currentSortDirection() === 'Asc')
                self.records.sort(function (left, right) { return left.item.value() == right.item.value() ? 0 : (left.item.value() < right.item.value() ? -1 : 1) })
            else
                self.records.sort(function (left, right) { return left.item.value() == right.item.value() ? 0 : (left.item.value() > right.item.value() ? -1 : 1) })


        };
    }

    self.records = ko.observableArray([
       new record(self, "Tolga", "Koseoglu", "1/16/1976"),
       new record(self, "Eric", "Lahti", "1/16/1975"),
       new record(self, "John", "Newman", "1/16/1960"),
    ]);

    function record(root, firstName, lastName, dob) {

        var self = this;

        self.firstName = ko.observable(firstName);
        self.lastName = ko.observable(lastName);
        self.dob = ko.observable(dob);

    };

};

$(function () {

    viewModel = new MyViewModel();

    ko.applyBindings(viewModel);

});





