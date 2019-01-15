"use strict";
(function () {
    mtcApp.controller("merchandiseOrderController",
        function merchandiseOrderController($scope, $filter, $interval, $rootScope, $location, $routeParams, merchandiseService, generalService) {

            $scope.header = "Merchandise Order Form";
            $scope.predicate = "DisplayName";
            $scope.reverse = false;
            $scope.isBusy = false;
            $scope.contactName = "";
            $scope.contactNumber = "";
            $scope.pickupDate = "";
            $scope.pickupTime = 9;
            $scope.paymentType = "";
            $scope.selectedContractorId = "";
            $scope.contractors = [];
            $scope.busyGettingContractors = true;

            $scope.canEdit = false;
            generalService.getCanEdit("Merchandise").then(function (result) {
                $scope.canEdit = result;
            });

            generalService.getContractors().then(function (results) {

                for (var i = 0; i < results.length; i++) {
                    $scope.contractors.push(results[i]);
                };
                $scope.busyGettingContractors = false;

                generalService.getCurrentUser().then(function (results) {
                    if (results) {
                        if (results.SelectedRoleName === "TowContractor" && results.ContractorTypeName == "Tow-Contractor") {
                            $scope.selectedContractorId = results.ContractorId;
                            $scope.userIsContractor = true;

                        }
                        else {
                            $scope.userIsContractor = false;
                        }
                    }
                    else {
                        $scope.header = "Error. We were not able to retrieve your current user context. Please come back later.";
                    }
                });

            });

            $scope.products = [];
            $scope.isBusyGettingProducts = true;

            $scope.getProducts = function () {
                merchandiseService.getProducts().then(function (results) {
                    $scope.products = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.products.push(new product(results[i]));
                    }
                    $scope.isBusyGettingProducts = false;
                });
            };
            $scope.getProducts();

            $scope.TotalCost = 0;
            $scope.updateTotalCost = function () {

                $scope.TotalCost = 0;

                for (var i = 0; i < $scope.products.length; i++) {
                    var product = $scope.products[i];
                    if (product.Quantity > 0)
                        $scope.TotalCost += product.Quantity * product.UnitCost;
                }

            };

            $scope.submit = function () {



                var selectedProducts = [];
                for (var i = 0; i < $scope.products.length; i++) {
                    var product = $scope.products[i];
                    if (product.Quantity > 0)
                        selectedProducts.push(product);
                }

                if (selectedProducts.length > 0) {

                    $scope.isBusy = true;

                    var vm = {
                        ContractorId: $scope.selectedContractorId,
                        ContactName: $scope.contactName,
                        ContactNumber: $scope.contactNumber,
                        Products: selectedProducts,
                        PickupDate: $scope.pickupDate,
                        PickupTime: $scope.pickupTime,
                        PaymentType: $scope.paymentType
                    };

                    merchandiseService.submitOrder(vm).then(function (orderId) {

                        console.log("Order Number/Id: " + orderId);
                        toastr.success("Order", "Successfully submitted");

                        $scope.contactName = "";
                        $scope.contactNumber = "";
                        $scope.pickupDate = "";
                        $scope.pickupTime = "";
                        $scope.paymentType = "";

                        $scope.getProducts();
                        $scope.isBusy = false;

                        $scope.orderId = orderId;

                        $("#orderConfirmationModal").modal("show");

                    });


                }
                else
                    alert("You must pick at least one merchandise.")



                console.log(vm);

            };

            function product(dbProduct) {
                var self = this;

                self.Id = dbProduct.Id;
                self.DisplayName = dbProduct.DisplayName;
                self.UnitCost = dbProduct.UnitCost;
                self.UnitsInStock = dbProduct.UnitsInStock;
                self.Quantity = 0;
                self.TotalCost = 0;

                self.updateTotalCost = function () {

                    if (self.Quantity >= 0) {
                        if (self.Quantity <= self.UnitsInStock) {
                            self.TotalCost = self.Quantity * self.UnitCost;
                            $scope.updateTotalCost();
                        }
                        else
                            alert("Out of stock. Please select lower quantity.");
                    }
                    else {
                        self.Quantity = 0;
                        alert("Positive numbers only please.");
                    }


                }

            }
        }
    );
}());