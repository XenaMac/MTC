(function () {
    'use strict';
    mtcApp.controller('fleetVehicleController',
        function fleetVehicleController($scope, $filter, $rootScope, $location, $routeParams, fleetVehicleService) {

            //from parent controller
            //selectedFleetVehicleId

            $scope.fleetVehicleHeader = 'Fleet Vehicle';
            $scope.isBusy = false;
            $scope.isEdit = false;
            $scope.ProgramStartDate = '';
            $scope.FleetNumber = '';
            $scope.VehicleType = '';
            $scope.VehicleYear = '';
            $scope.VehicleMake = '';
            $scope.VehicleModel = '';
            $scope.VIN = '';
            $scope.LicensePlate = '';
            $scope.RegistrationExpireDate = '';
            $scope.InsuranceExpireDate = '';
            $scope.LastCHPInspection = '';
            $scope.Comments = '';
            $scope.ProgramEndDate = '';
            $scope.FAW = '';
            $scope.RAWR = '';
            $scope.RAW = '';
            $scope.GVW = '';
            $scope.GVWR = '';
            $scope.Wheelbase = '';
            $scope.Overhang = '';
            $scope.MAXTW = '';
            $scope.MAXTWCALCDATE = '';
            $scope.FuelType = '';
            $scope.VehicleNumber = '';
            $scope.IPAddress = '';
            $scope.TAIP = '';
            $scope.AgreementNumber = '';
            $scope.IsBackup = '';

            //initiation 1
            $scope.init = function (selectedFleetVehicleId) {

                $scope.selectedFleetVehicleId = selectedFleetVehicleId;
                $scope.reset();

                if ($scope.selectedFleetVehicleId != undefined && $scope.selectedFleetVehicleId != "") {

                    $scope.fleetVehicleHeader = "Edit Fleet Vehicle";
                    $scope.isEdit = true;
                    $scope.getFleetVehicle();

                }
            };

            //initiation 2
            $('#manageFleetVehicleModal').on('shown.bs.modal', function () {

                $scope.reset();

                if ($scope.selectedFleetVehicleId != undefined && $scope.selectedFleetVehicleId != "") {

                    $scope.fleetVehicleHeader = "Edit Fleet Vehicle";
                    $scope.isEdit = true;
                    $scope.getFleetVehicle();
                }
            })

            $scope.reset = function () {

                $scope.fleetVehicleHeader = "Add Fleet Vehicle";

                $scope.ProgramStartDate = '';
                $scope.FleetNumber = '';
                $scope.VehicleType = '';
                $scope.VehicleYear = '';
                $scope.VehicleMake = '';
                $scope.VehicleModel = '';
                $scope.VIN = '';
                $scope.LicensePlate = '';
                $scope.RegistrationExpireDate = '';
                $scope.InsuranceExpireDate = '';
                $scope.LastCHPInspection = '';
                $scope.Comments = '';
                $scope.ProgramEndDate = '';
                $scope.FAW = '';
                $scope.RAWR = '';
                $scope.RAW = '';
                $scope.GVW = '';
                $scope.GVWR = '';
                $scope.Wheelbase = '';
                $scope.Overhang = '';
                $scope.MAXTW = '';
                $scope.MAXTWCALCDATE = '';
                $scope.FuelType = '';
                $scope.VehicleNumber = '';
                $scope.IPAddress = '';
                $scope.TAIP = '';
                $scope.AgreementNumber = '';

                $scope.$apply();
            };

            $scope.getFleetVehicle = function () {

                $scope.isBusy = true;

                fleetVehicleService.getFleetVehicle($scope.selectedFleetVehicleId).then(function (result) {

                    if (moment(result.ProgramStartDate).isValid())
                        $scope.ProgramStartDate = moment(result.ProgramStartDate).format('MM/DD/YYYY');

                    $scope.FleetNumber = result.FleetNumber;

                    $scope.VehicleType = result.VehicleType;
                    $scope.VehicleYear = result.VehicleYear;
                    $scope.VehicleMake = result.VehicleMake;
                    $scope.VehicleModel = result.VehicleModel;
                    $scope.VIN = result.VIN;
                    $scope.LicensePlate = result.LicensePlate;

                    if (moment(result.RegistrationExpireDate).isValid())
                        $scope.RegistrationExpireDate = moment(result.RegistrationExpireDate).format('MM/DD/YYYY');

                    if (moment(result.InsuranceExpireDate).isValid())
                        $scope.InsuranceExpireDate = moment(result.InsuranceExpireDate).format('MM/DD/YYYY');

                    if (moment(result.LastCHPInspection).isValid())
                        $scope.LastCHPInspection = moment(result.LastCHPInspection).format('MM/DD/YYYY');

                    $scope.Comments = result.Comments;

                    if (moment(result.ProgramEndDate).isValid())
                        $scope.ProgramEndDate = moment(result.ProgramEndDate).format('MM/DD/YYYY');

                    $scope.FAW = result.FAW;
                    $scope.RAWR = result.RAWR;
                    $scope.RAW = result.RAW;
                    $scope.GVW = result.GVW;
                    $scope.GVWR = result.GVWR;
                    $scope.Wheelbase = result.Wheelbase;
                    $scope.Overhang = result.Overhang;
                    $scope.MAXTW = result.MAXTW;

                    if (moment(result.MAXTWCALCDATE).isValid())
                        $scope.MAXTWCALCDATE = moment(result.MAXTWCALCDATE).format('MM/DD/YYYY');

                    $scope.FuelType = result.FuelType;
                    $scope.VehicleNumber = result.VehicleNumber;
                    $scope.IPAddress = result.IPAddress;
                    $scope.TAIP = result.TAIP;
                    $scope.AgreementNumber = result.AgreementNumber;
                    $scope.IsBackup = result.IsBackup

                    $scope.isBusy = false;

                });

            };

            $scope.saveFleetVehicle = function () {

                var vm = {
                    FleetVehicleID: $scope.selectedFleetVehicleId,
                    ContractorID: $scope.contractorId, //from parent controller
                    ProgramStartDate: $scope.ProgramStartDate,
                    FleetNumber: $scope.FleetNumber,
                    VehicleType: $scope.VehicleType,
                    VehicleYear: $scope.VehicleYear,
                    VehicleMake: $scope.VehicleMake,
                    VehicleModel: $scope.VehicleModel,
                    VIN: $scope.VIN,
                    LicensePlate: $scope.LicensePlate,
                    RegistrationExpireDate: $scope.RegistrationExpireDate,
                    InsuranceExpireDate: $scope.InsuranceExpireDate,
                    LastCHPInspection: $scope.LastCHPInspection,
                    Comments: $scope.Comments,
                    ProgramEndDate: $scope.ProgramEndDate,
                    FAW: $scope.FAW,
                    RAW: $scope.RAW,
                    RAWR: $scope.RAWR,
                    GVW: $scope.GVW,
                    GVWR: $scope.GVWR,
                    Wheelbase: $scope.Wheelbase,
                    Overhang: $scope.Overhang,
                    MAXTW: $scope.MAXTW,
                    MAXTWCALCDATE: $scope.MAXTWCALCDATE,
                    FuelType: $scope.FuelType,
                    VehicleNumber: $scope.VehicleNumber,
                    IPAddress: $scope.IPAddress,
                    TAIP: $scope.TAIP,
                    AgreementNumber: $scope.AgreementNumber,
                    IsBackup: $scope.IsBackup
                };

                fleetVehicleService.saveFleetVehicle(vm).then(function (result) {

                    $scope.isBusy = false;
                    $scope.isEdit = true;
                    toastr.success("Saved", "Contractor");

                    $scope.getContractor();//parent
                    $("#manageFleetVehicleModal").modal('hide');
                });
            };

        }
    );
}());