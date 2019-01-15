/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="octa.mapViewModel.js" />
/// <reference path="octa.constants.js" />

octa.FSP.prototype = {
    setPersons: function () {
        this.persons = ko.observableArray([
              new person(self, "Tolga", "Koseoglu"),
              new person(self, "Charli", "Koseoglu"),
              new person(self, "Jen", "Koseoglu"),
        ]);

        function person(root, firstName, lastName) {

            var self = this;

            self.firstName = ko.observable(firstName);
            self.lastName = ko.observable(lastName);

            self.updatePerson = function (firstName, lastName) {

                self.firstName(firstName);
                self.lastName(lastName);

            };

        }
    }
};





