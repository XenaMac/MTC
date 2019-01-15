'use strict';
var mtcApp = angular.module('mtcApp', ['ngRoute', 'ngSanitize', 'mtcAppFilters', 'ui.bootstrap', 'minicolors'])
    .config(function($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
    }).run(function($rootScope, $location, $interval) {
        $rootScope.ApplicationName = 'MTC Web 1.0';
        $rootScope.defaultTablePageSize = 50;
        $rootScope.showColumnConfig = function() {
            $("#columnConfigModal").modal('show');
        };
        $rootScope.mtcColumn = function(display, name, visible, sortable) {
            var self = this;
            self.display = display;
            self.name = name;
            self.visible = visible;
            self.sortable = sortable;
        };
        $rootScope.mtcTruck = function(dbTruck) {
            var self = this;
            self.id = 1;
            self.truckNumber = '1';
            self.ipAddress = '1';
            self.beatNumber = '100';
            self.update = function(dbTruck) {
                self.truckNumberOrginal = dbTruck.TruckNumber;
                self.callSign = dbTruck.CallSign;
                if (dbTruck.TruckNumber !== null) {
                    self.id = dbTruck.TruckNumber.replace(' ', '').replace('-', '').replace('_', '');
                    self.truckNumber = dbTruck.TruckNumber;
                }
                self.vehicleType = dbTruck.VehicleType;
                self.ipAddress = dbTruck.IPAddress;
                self.vehicleStateIconUrl = $rootScope.getTruckIcon(dbTruck.VehicleState, dbTruck.Alarms);
                self.vehicleState = dbTruck.VehicleState;
                self.heading = dbTruck.Heading;
                if (dbTruck.BeatNumber !== null) self.beatNumber = dbTruck.BeatNumber.substring(dbTruck.BeatNumber.indexOf("-") + 1);
                self.beatNumberString = self.beatNumber;
                self.beatSegmentNumber = dbTruck.BeatSegmentNumber;
                self.contractorId = dbTruck.ContractorId;
                self.lat = dbTruck.Lat;
                self.lon = dbTruck.Lon;
                self.speed = dbTruck.Speed;
                self.driverName = dbTruck.DriverName;
                self.contractorName = dbTruck.ContractorName;
                self.location = dbTruck.Location;
                self.lastMessage = moment(dbTruck.LastMessage).format('MM/DD/YY hh:mm:ss');
                self.speedingTime = moment(dbTruck.SpeedingTime).format('MM/DD/YY hh:mm:ss');
                self.speedingValue = dbTruck.SpeedingValue;
                self.outOfBoundsMessage = dbTruck.OutOfBoundsMessage;
                self.outOfBoundsTime = dbTruck.OutOfBoundsTime;
                self.hasAlarm = dbTruck.Alarms;
            };
            self.update(dbTruck);
        };
        $rootScope.getTruckIcon = function(vehicleState, hasAlarms) {
            var iconName = 'mtc_icons_v2_gray'; //waiting for driver login in + logged off
            //green
            if (vehicleState.toLowerCase() === 'on patrol') iconName = 'mtc_icons_v2_green';
            //light blue
            else if (vehicleState.toLowerCase() === 'on break') iconName = 'mtc_icons_v2_lt_blue';
            else if (vehicleState.toLowerCase() === 'roll out') iconName = 'mtc_icons_v2_lt_blue';
            else if (vehicleState.toLowerCase() === 'roll in') iconName = 'mtc_icons_v2_lt_blue';
            else if (vehicleState.toLowerCase() === 'on lunch') iconName = 'mtc_icons_v2_lt_blue';
            //purple
            else if (vehicleState.toLowerCase() === 'driver logged on') iconName = 'mtc_icons_v2_purple';
            //blue
            else if (vehicleState.toLowerCase() === 'on incident') iconName = 'mtc_icons_v2_blue';
            //yellow
            else if (vehicleState.toLowerCase() === 'on tow') iconName = 'mtc_icons_v2_yellow';
            //orange
            else if (vehicleState.toLowerCase() === 'enroute') iconName = 'mtc_icons_v2_orange';
            if (hasAlarms) iconName += "_alert";
            return $(".websiteUrl").text().trim() + '/Content/Images/' + iconName + '.png';
        };
        $rootScope.redrawTable = function() {
            var redrawInterval = $interval(function() {
                $interval.cancel(redrawInterval);
                $('.footable').trigger('footable_redraw');
            }, 100);
        };
        $rootScope.createDateFromTimespan = function(time) {
            var datetime = '2000-01-01 ' + time.Hours + ":" + time.Minutes;
            var date = new Date();
            date.setHours(moment(datetime).format('HH'));
            date.setMinutes(moment(datetime).format('mm'));
            return date;
        };
        $rootScope.createDate = function(hour, min) {
            var d = new Date();
            d.setHours(hour);
            d.setMinutes(min);
            return d;
        };
    });
$(function() {

    // toastr.options = {
    //     progressBar: false,
    //     preventDuplicates: true,
    //     positionClass: "toast-bottom-right",
    //     timeOut: 1000
    // };
    toastr.options = {
        closeButton: false,
        debug: false,
        newestOnTop: false,
        progressBar: false,
        positionClass: "toast-bottom-left",
        preventDuplicates: false,
        onclick: null,
        showDuration: 300,
        hideDuration: 1000,
        timeOut: 5000,
        extendedTimeOut: 1000,
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };

});