(function () {
    'use strict';
    mtcApp.controller('driverController',
        function driverController($scope, $filter, $rootScope, $location, $routeParams, driverService) {

            //from parent controller
            //selectedFleetVehicleId

            $scope.driverHeader = 'Driver';
            $scope.isBusy = false;
            $scope.isEdit = false;

            $scope.DriverId = '';
            //$scope.ContractorId = '';
            $scope.DateAdded = '';
            $scope.BeatID = '';

            $scope.LastName = '';
            $scope.FirstName = '';
            $scope.FSPIDNumber = '';
            $scope.ProgramStartDate = '';
            $scope.TrainingCompletionDate = '';
            $scope.DOB = '';
            $scope.LicenseExpirationDate = '';
            $scope.DL64ExpirationDate = '';
            $scope.MedicalCardExpirationDate = '';
            $scope.LastPullNoticeDate = '';
            $scope.UDF = '';
            $scope.Comments = '';
            $scope.ContractorEndDate = '';
            $scope.ProgramEndDate = '';
            $scope.ContractorStartDate = '';
            $scope.Password = '';
            $scope.DL64Number = '';
            $scope.DriversLicenseNumber = '';
            $scope.AddedtoC3Database = '';


            //initiation 1
            $scope.init = function (selectedDriverId) {

                $scope.selectedDriverId = selectedDriverId;
                $scope.reset();

                if ($scope.selectedDriverId != undefined && $scope.selectedDriverId != "") {

                    $scope.driverHeader = "Edit Driver";
                    $scope.isEdit = true;
                    $scope.getDriver();

                }
            };

            //initiation 2
            $('#manageDriverModal').on('shown.bs.modal', function () {

                $scope.reset();

                if ($scope.selectedDriverId != undefined && $scope.selectedDriverId != "") {

                    $scope.driverHeader = "Edit Driver";
                    $scope.isEdit = true;
                    $scope.getDriver();
                }
            })

            $scope.reset = function () {

                $scope.driverHeader = "Add Driver";
                //$scope.ContractorId = '';

                $scope.DateAdded = '';
                $scope.DriverId = '';
                $scope.BeatID = '';

                $scope.LastName = '';
                $scope.FirstName = '';
                $scope.FSPIDNumber = '';
                $scope.ProgramStartDate = '';
                $scope.TrainingCompletionDate = '';
                $scope.DOB = '';
                $scope.LicenseExpirationDate = '';
                $scope.DL64ExpirationDate = '';
                $scope.MedicalCardExpirationDate = '';
                $scope.LastPullNoticeDate = '';
              
                $scope.UDF = '';
                $scope.Comments = '';
                $scope.ContractorEndDate = '';
                $scope.ProgramEndDate = '';
                $scope.ContractorStartDate = '';
               
                $scope.Password = '';
                $scope.DL64Number = '';
                $scope.DriversLicenseNumber = '';
                $scope.AddedtoC3Database = '';


                $scope.$apply();
            };

            $scope.getDriver = function () {

                $scope.isBusy = true;

                driverService.getDriver($scope.selectedDriverId).then(function (result) {
                    
                    $scope.DateAdded = result.DateAdded;
                    $scope.BeatID = result.BeatID;

                    $scope.FSPIDNumber = result.FSPIDNumber;
                    $scope.Password = result.Password;
                    $scope.FirstName = result.FirstName;
                    $scope.LastName = result.LastName;

                    if (moment(result.DOB).isValid())
                        $scope.DOB = moment(result.DOB).format('MM/DD/YYYY');

                    if (moment(result.ProgramStartDate).isValid())
                        $scope.ProgramStartDate = moment(result.ProgramStartDate).format('MM/DD/YYYY');

                    if (moment(result.ProgramEndDate).isValid())
                        $scope.ProgramEndDate = moment(result.ProgramEndDate).format('MM/DD/YYYY');

                    if (moment(result.TrainingCompletionDate).isValid())
                        $scope.TrainingCompletionDate = moment(result.TrainingCompletionDate).format('MM/DD/YYYY');

                    if (moment(result.LicenseExpirationDate).isValid())
                        $scope.LicenseExpirationDate = moment(result.LicenseExpirationDate).format('MM/DD/YYYY');

                    if (moment(result.DL64ExpirationDate).isValid())
                        $scope.DL64ExpirationDate = moment(result.DL64ExpirationDate).format('MM/DD/YYYY');

                    if (moment(result.MedicalCardExpirationDate).isValid())
                        $scope.MedicalCardExpirationDate = moment(result.MedicalCardExpirationDate).format('MM/DD/YYYY');

                    if (moment(result.LastPullNoticeDate).isValid())
                        $scope.LastPullNoticeDate = moment(result.LastPullNoticeDate).format('MM/DD/YYYY');

                    if (moment(result.ContractorStartDate).isValid())
                        $scope.ContractorStartDate = moment(result.ContractorStartDate).format('MM/DD/YYYY');

                    if (moment(result.ContractorEndDate).isValid())
                        $scope.ContractorEndDate = moment(result.ContractorEndDate).format('MM/DD/YYYY');


                    if (moment(result.AddedtoC3Database).isValid())
                        $scope.AddedtoC3Database = moment(result.AddedtoC3Database).format('MM/DD/YYYY');

                    $scope.DriversLicenseNumber = result.DriversLicenseNumber;
                    $scope.DL64Number = result.DL64Number;
                    $scope.UDF = result.UDF;
                    $scope.Comments = result.Comments;

                    $scope.isBusy = false;

                });

            };

            $scope.saveDriver = function () {

                var vm = {
                    DriverID: $scope.selectedDriverId,
                    ContractorID: $scope.contractorId, //from parent
                    DateAdded: $scope.DateAdded,
                    BeatID: $scope.BeatID,

                    LastName: $scope.LastName,
                    FirstName: $scope.FirstName,
                    FSPIDNumber: $scope.FSPIDNumber,
                    ProgramStartDate: $scope.ProgramStartDate,
                    TrainingCompletionDate: $scope.TrainingCompletionDate,
                    DOB: $scope.DOB,
                    LicenseExpirationDate: $scope.LicenseExpirationDate,
                    DL64ExpirationDate: $scope.DL64ExpirationDate,
                    MedicalCardExpirationDate: $scope.MedicalCardExpirationDate,
                    LastPullNoticeDate: $scope.LastPullNoticeDate,
                    UDF: $scope.UDF,
                    Comments: $scope.Comments,
                    ContractorEndDate: $scope.ContractorEndDate,
                    ProgramEndDate: $scope.ProgramEndDate,
                    ContractorStartDate: $scope.ContractorStartDate,
                    Password: $scope.Password,
                    DL64Number: $scope.DL64Number,
                    DriversLicenseNumber: $scope.DriversLicenseNumber,
                    AddedtoC3Database: $scope.AddedtoC3Database,
                };

                driverService.saveDriver(vm).then(function (result) {

                    $scope.isBusy = false;
                    $scope.isEdit = true;
                    toastr.success("Saved", "Driver");

                    $scope.getContractor();//parent
                    $("#manageDriverModal").modal('hide');
                });
            };

        }
    );
}());