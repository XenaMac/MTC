'use strict';
(function () {
    mtcApp.controller('contractsController',
        function inspectionsController($scope, $filter, $rootScope, $location, $routeParams, contractsService) {

            $scope.header = 'Contracts';
            $scope.contracts = [];
                     
            var getContracts = function () {

                $scope.isBusyGettingContracts = true;

                contractsService.getContracts().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.contracts.push(new contract(results[i]));
                    }

                    $scope.isBusyGettingContracts = false;

                });
            };

            getContracts();

            function contract(dbContract) {

                var self = this;

                self.ContractId = dbContract.ContractId;
                self.AgreementNumber = dbContract.AgreementNumber;
                self.StartDate = moment(dbContract.StartDate).format('MM/DD/YY');
                self.EndDate = moment(dbContract.EndDate).format('MM/DD/YY');
                self.MaxObligation = dbContract.MaxObligation;
                self.Beat = dbContract.Beat;
                self.ContractorId = dbContract.ContractorId;
                self.ContractorCompanyName = dbContract.ContractorCompanyName;
                
            };
        }
    );
}());