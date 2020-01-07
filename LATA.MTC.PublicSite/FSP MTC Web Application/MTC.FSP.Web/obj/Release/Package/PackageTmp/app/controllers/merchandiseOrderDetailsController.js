(function () {
    "use strict";
    mtcApp.controller("merchandiseOrderDetailsController",
        function merchandiseOrderDetailsController($scope, $filter, $interval, $rootScope, $location, $routeParams, merchandiseService, generalService) {

            $scope.header = "Order Details";
            $scope.selectedOrder = "";
            $scope.isBusy = true;
            $scope.orderProducts = [];
            $scope.isEditMode = false;
            $scope.canCancel = false;
            $scope.canDecline = false;
            $scope.canFulFill = false;
            $scope.auditItems = [];

            $scope.getOrderDetails = function () {
                $scope.isBusy = true;
                merchandiseService.getOrderDetails($("#orderId").val()).then(function (result) {

                    var order = result.Order;
                    var products = result.Products;
                    var audits = result.Audits;

                    //Access
                    $scope.canCancel = result.CanCancel;
                    $scope.canDecline = result.CanDecline;
                    $scope.canFulFill = result.CanFulFill;

                    //Order
                    $scope.newContractor = result.Contractor;
                    $scope.newContactName = order.ContactName;
                    $scope.newContactNumber = order.ContactNumber;

                    if (moment(order.PickupDate).isValid())
                        $scope.newPickupDate = moment(order.PickupDate).format("MM/DD/YYYY");

                    $scope.newPickupTime = order.PickupTime;

                    $scope.merchandiseOrderStatus = order.MerchandiseOrderStatus;

                    if (order.MerchandiseOrderStatus == 0)
                        $scope.orderStatus = "Order Submitted";
                    else if (order.MerchandiseOrderStatus == 1)
                        $scope.orderStatus = "Order Filled";
                    else if (order.MerchandiseOrderStatus == 2)
                        $scope.orderStatus = "Order Cancelled";
                    else if (order.MerchandiseOrderStatus == 3)
                        $scope.orderStatus = "Order Declined";

                    if (order.PickupTime === "9")
                        $scope.newPickupTimeFriendly = "9 AM";
                    else if (order.PickupTime === "10")
                        $scope.newPickupTimeFriendly = "10 AM";
                    else if (order.PickupTime === "11")
                        $scope.newPickupTimeFriendly = "11 AM";
                    else if (order.PickupTime === "1")
                        $scope.newPickupTimeFriendly = "1 PM";
                    else if (order.PickupTime === "2")
                        $scope.newPickupTimeFriendly = "2 PM";
                    else if (order.PickupTime === "3")
                        $scope.newPickupTimeFriendly = "3 PM";
                    else if (order.PickupTime === "4")
                        $scope.newPickupTimeFriendly = "4 PM";

                    if (moment(order.ReceivedOn).isValid())
                        $scope.newReceivedOn = moment(order.ReceivedOn).format("MM/DD/YYYY");

                    $scope.newReceivedBy = order.ReceivedBy;

                    $scope.createdOn = order.CreatedOn;
                    $scope.createdBy = order.CreatedBy;

                    $scope.newComments = order.Comment;

                    //products
                    $scope.orderProducts = [];
                    for (var i = 0; i < products.length; i++) {
                        $scope.orderProducts.push(products[i]);
                    }

                    //audit
                    $scope.auditItems = [];
                    for (var i = 0; i < audits.length; i++) {
                        $scope.auditItems.push(audits[i]);
                    }

                    $scope.isBusy = false;
                });
            };
            $scope.getOrderDetails();

            $scope.canEdit = false;
            generalService.getCanEdit("Merchandise").then(function (result) {
                $scope.canEdit = result;
            });

            $scope.cancelOrder = function () {

                if (confirm("Are you sure to cancel this order?")) {

                    var vm = {
                        orderId: $("#orderId").val()
                    };
                    $scope.isBusy = true;
                    merchandiseService.cancelOrder(vm).then(function (result) {
                        toastr.success("Order", "Successfully cancelled and inventory restocked");
                        $scope.getOrderDetails();
                        $scope.isBusy = false;
                    });

                }

            };

            $scope.declineOrder = function () {

                if (confirm("Are you sure to decline this order?")) {

                    var vm = {
                        orderId: $("#orderId").val()
                    };
                    $scope.isBusy = true;
                    merchandiseService.declineOrder(vm).then(function (result) {

                        toastr.success("Order", "Successfully declined and inventory restocked");
                        $scope.getOrderDetails();
                        $scope.isBusy = false;
                    });

                }

            };

            $scope.fulFillOrder = function () {

                if (confirm("Are you sure to fulfill this order?")) {

                    var vm = {
                        orderId: $("#orderId").val()
                    };

                    $scope.isBusy = true;
                    merchandiseService.fulFillOrder(vm).then(function (result) {

                        toastr.success("Order", "Successfully fulfilled");
                        $scope.getOrderDetails();
                        $scope.isBusy = false;
                    });

                }
            };

            $scope.edit = function () {
                $scope.isEditMode = true;
            };

            $scope.cancel = function () {
                $scope.isEditMode = false;
            };

            $scope.save = function () {

                $scope.isBusy = true;

                var vm = {
                    Id: $("#orderId").val(),
                    ContactName: $scope.newContactName,
                    ContactNumber: $scope.newContactNumber,
                    PickupDate: $scope.newPickupDate,
                    PickupTime: $scope.newPickupTime,
                    Comment: $scope.newComments,
                    ReceivedBy: $scope.newReceivedBy,
                    ReceivedOn: $scope.newReceivedOn
                };

                merchandiseService.saveOrder(vm).then(function (results) {
                    toastr.success("Order", "Successfully saved");
                    $scope.getOrderDetails();
                    $("#editModal").modal("hide");
                    $scope.isEditMode = false;
                    $scope.isBusy = false;
                });

            };
        }
    );
}());