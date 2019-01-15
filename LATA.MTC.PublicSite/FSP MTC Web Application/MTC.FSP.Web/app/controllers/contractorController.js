(function () {
    'use strict';
    mtcApp.controller('contractorController',
        function contractorController($scope, $filter, $rootScope, $location, $routeParams, contractorService, fleetVehicleService,driverService) {

            $scope.header = 'Contractor';
            $scope.contractorId = 0;
            $scope.isBusy = false;
            $scope.isEdit = false;
            $scope.selectedFleetVehicleId;
            $scope.selectedDriverId;
            $scope.contractorTypes = [];
            $scope.selectedContractorType;

            $scope.init = function (contractorId) {
                $scope.contractorId = contractorId;
                if ($scope.contractorId != undefined && $scope.contractorId != "") {
                    $scope.header = "Edit";
                    $scope.isEdit = true;
                    $scope.getContractor();
                } else {
                    $scope.header = "Add";
                }
                $scope.getContractorTypes();
            };

            $scope.getContractor = function () {

                $scope.isBusy = true;

                contractorService.getContractor($scope.contractorId).then(function (results) {

                    $scope.header = "Edit " + "\"" + results[0].ContractCompanyName + "\"";

                    $scope.selectedContractorType = results[0].ContractorTypeId;
                    $scope.ContractCompanyName = results[0].ContractCompanyName;
                    $scope.ContactFirstName = results[0].ContactFirstName;
                    $scope.ContactLastName = results[0].ContactLastName;
                    $scope.Email = results[0].Email;
                    $scope.OfficeTelephone = results[0].OfficeTelephone;
                    $scope.MCPNumber = results[0].MCPNumber;
                    if (moment(results[0].MCPExpiration).isValid())
                        $scope.MCPExpiration = moment(results[0].MCPExpiration).format('MM/DD/YYYY');

                    $scope.Address = results[0].Address;
                    $scope.City = results[0].City;
                    $scope.State = results[0].State;
                    $scope.Zip = results[0].Zip;

                    $scope.Comments = results[0].Comments;

                    $scope.FleetVehicles = results[0].FleetVehicles;
                    $scope.Drivers = results[0].Drivers;

                    $scope.isBusy = false;

                });
            };

            $scope.getContractorTypes = function () {                
                contractorService.getContractorTypes().then(function (results) {                   
                    $scope.contractorTypes = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorTypes.push(results[i]);
                    }
                });
            };

            $scope.saveContractor = function () {

                $scope.isBusy = true;

                var vm = {
                    contractorTypeId:$scope.selectedContractorType,
                    contractorId: $scope.contractorId,
                    ContractCompanyName: $scope.ContractCompanyName,
                    ContactFirstName: $scope.ContactFirstName,
                    ContactLastName: $scope.ContactLastName,
                    Email: $scope.Email,
                    OfficeTelephone: $scope.OfficeTelephone,
                    MCPNumber: $scope.MCPNumber,
                    MCPExpiration: $scope.MCPExpiration,
                    Address: $scope.Address,
                    City: $scope.City,
                    State: $scope.State,
                    Zip: $scope.Zip,
                    Comments: $scope.Comments
                };

                contractorService.saveContractor(vm).then(function (result) {

                    $scope.isBusy = false;
                    $scope.isEdit = false;
                    $scope.contractorId = result;
                    toastr.success("Saved", "Contractor");
                    $scope.getContractor();

                });


            };


            $scope.manageFleetVehicle = function (fleetVehicle) {

                $scope.selectedFleetVehicleId = fleetVehicle.FleetVehicleId;
                $("#manageFleetVehicleModal").modal('show');

            };

            $scope.addFleetVehicle = function () {
                $scope.selectedFleetVehicleId = '';
                $("#manageFleetVehicleModal").modal('show');
            };

            $scope.removeFleetVehicle = function (fleetVehicle) {
              
                if (confirm("Are you sure to remove this fleet vehicle?")) {

                    $scope.isBusy = true;

                    fleetVehicleService.removeFleetVehicle(fleetVehicle.FleetVehicleId).then(function (result) {
                        toastr.success("Removed", "Fleet Vehicle");
                        $scope.getContractor();
                        $scope.isBusy = false;
                    });
                };

            };


            $scope.manageDriver = function (driver) {

                $scope.selectedDriverId = driver.DriverId;
                $("#manageDriverModal").modal('show');

            };

            $scope.addDriver = function () {
                $scope.selectedDriverId = '';
                $("#manageDriverModal").modal('show');
            };

            $scope.removeDriver = function (driver) {

                if (confirm("Are you sure to remove this driver?")) {

                    $scope.isBusy = true;

                    driverService.removeDriver(driver.DriverId).then(function (result) {
                        toastr.success("Removed", "Driver");
                        $scope.getContractor();
                        $scope.isBusy = false;
                    });
                };

            };

        }
    );
}());