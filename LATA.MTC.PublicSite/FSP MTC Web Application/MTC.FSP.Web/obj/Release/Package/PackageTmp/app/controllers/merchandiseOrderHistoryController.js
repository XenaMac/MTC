"use strict";
(function () {
    mtcApp.controller("merchandiseOrderHistoryController",
        function merchandiseOrderHistoryController($scope, $filter, $interval, $rootScope, $location, $routeParams, merchandiseService, generalService) {

            $scope.header = "Orders";
            $scope.orders = [];
            $scope.isBusyGettingOrders = true;

            $scope.getOrders = function () {

                merchandiseService.getOrders().then(function (results) {
                    $scope.orders = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.orders.push(new order(results[i]));
                    }
                    $scope.isBusyGettingOrders = false;
                });

            };
            $scope.getOrders();
         
            $scope.canEdit = false;
            generalService.getCanEdit("Merchandise").then(function (result) {
                $scope.canEdit = result;
            });

            function order(dbOrder) {

                var self = this;

                self.Id = dbOrder.Id;
                self.FriendlyId = dbOrder.FriendlyId,
                self.Contractor = dbOrder.Contractor;
                self.ContactName = dbOrder.ContactName;
                self.ContactNumber = dbOrder.ContactNumber;
                self.PickupDate = moment(dbOrder.PickupDate).format("MM/DD/YYYY");
                self.PickupTime = dbOrder.PickupTime;

                if (dbOrder.PickupTime === "9")
                    self.PickupTimeFriendly = "9 AM";
                else if (dbOrder.PickupTime === "10")
                    self.PickupTimeFriendly = "10 AM";
                else if (dbOrder.PickupTime === "11")
                    self.PickupTimeFriendly = "11 AM";
                else if (dbOrder.PickupTime === "1")
                    self.PickupTimeFriendly = "1 PM";
                else if (dbOrder.PickupTime === "2")
                    self.PickupTimeFriendly = "2 PM";
                else if (dbOrder.PickupTime === "3")
                    self.PickupTimeFriendly = "3 PM";
                else if (dbOrder.PickupTime === "4")
                    self.PickupTimeFriendly = "4 PM";

                self.CreatedBy = dbOrder.CreatedBy;
                self.CreatedOn = moment(dbOrder.CreatedOn).format("MM/DD/YYYY hh:mm");

                self.MerchandiseOrderStatus = dbOrder.MerchandiseOrderStatus;

                if (dbOrder.MerchandiseOrderStatus == 0)
                    self.OrderStatus = "Submitted";
                else if (dbOrder.MerchandiseOrderStatus == 1)
                    self.OrderStatus = "Filled";
                else if (dbOrder.MerchandiseOrderStatus == 2)
                    self.OrderStatus = "Cancelled";
                else if (dbOrder.MerchandiseOrderStatus == 3)
                    self.OrderStatus = "Declined";

                self.TotalCost = 0;
                self.OrderDetails = dbOrder.OrderDetails;
                if (dbOrder.OrderDetails) {
                    for (var i = 0; i < dbOrder.OrderDetails.length; i++) {

                        var det = dbOrder.OrderDetails[i];
                        self.TotalCost += (det.UnitCost * det.Quantity);
                    }
                }
            }

        }
    );
}());