var DriverName = "";
var TruckID = "";
var TruckStatus = "";
var ServiceLocation = "http://38.124.164.211:9007/";

var ClientStatus = "LoggedOff";

var ViewModel = function () {

    var self = this;
    self.isBusy = ko.observable(false);

    self.init = function () {
        self.getDrivers();
        self.getContractors();
        self.getVehicles();
        self.getBeatNumbers();
        self.getIncidentTypes();
        self.getTrafficSpeeds();
        self.getFreeways();
        self.getVehiclePositions();
        self.getVehicleTypes();
        self.getTowLocations();
        self.getServiceTypes();
    };

    self.postAssist = function () {
        console.log('Posting Assist...');
        self.isBusy(true);


        var x1097dt;
        var x1098dt;
        var onsitedt;
        var assistData;

        //validation
        try {
            if (isNaN(parseFloat($('#txtStartOD').val())) == true) {
                self.isBusy(false);
                alert('Start odometer value is invalid');
                return;
            }

            if (isNaN(parseFloat($('#txtEndOD').val())) == true) {
                self.isBusy(false);
                alert('Start odometer value is invalid');
                return;
            }

            if (parseFloat($('#txtStartOD').val()) > parseFloat($('#txtEndOD').val())) {
                self.isBusy(false);
                alert('Start Odometer must be less than or equeal to End Odometer');
                return;
            }

            var TowLocation = $('#ddlTowLocations option:selected').text();
            var DropZone = $('#txtDropZone').val();
            if (TowLocation == 'DRP (Drop Zone (Towed to Drop Zone))' && DropZone == "") {
                alert('You must enter the drop zone');
                self.isBusy(false);
                return;
            }

            if ($('#txtAssistStartTime').val()) {
                onsitedt = dateToWcf($('#txtServiceDate').val() + ' ' + $('#txtAssistStartTime').val());
            }
            else {
                alert('Must provide an assist start time of format DD/MM/YY HH:MM:SS', 'aborting');
                self.isBusy(false);
                return;
            }

            if ($('#txtAssistEndTime').val()) {
                x1098dt = dateToWcf($('#txtServiceDate').val() + ' ' + $('#txtAssistEndTime').val());
            }
            else {
                alert('Must provide an assist end time of format DD/MM/YY HH:MM:SS', 'aborting');
                self.isBusy(false);
                return;
            }

            if ($('#txtServiceDate').val() && $('#txtDispatchTime').val()) {
                x1097dt = dateToWcf($('#txtServiceDate').val() + ' ' + $('#txtDispatchTime').val());
            }
            else {
                alert('Must provide a service time of format DD/MM/YY HH:MM:SS', 'aborting');
                self.isBusy(false);
                return;
            }

            if ($('#ddlDrivers').val() === "-1") {
                alert('Please select a driver');
                self.isBusy(false);
                return;
            }

            if ($('#ddlContractors').val() === "-1") {
                alert('Please select a contractor');
                self.isBusy(false);
                return;
            }

            if ($('#ddlVehicles').val() === "-1") {
                alert('Please select a vehicle');
                self.isBusy(false);
                return;
            }

            if ($('#ddlBeatNumbers').val() === "-1") {
                alert('Please select a driver');
                self.isBusy(false);
                return;
            }

            if ($('#ddlIncidentTypes').val() === "-1") {
                alert('Please select an incident type');
                self.isBusy(false);
                return;
            }

            if ($('#ddlTrafficSpeeds').val() === "-1") {
                alert('Please select traffic speed');
                self.isBusy(false);
                return;
            }

            if ($('#ddlFreeways').val() === "-1") {
                alert('Please select freeway');
                self.isBusy(false);
                return;
            }

            if ($('#ddlVehiclePositions').val() === "-1") {
                alert('Please select the vehicle position');
                self.isBusy(false);
                return;
            }

            if ($('#ddlVehicleTypes').val() === "-1") {
                alert('Please select the vehicle type');
                self.isBusy(false);
                return;
            }


            if ($('#ddlTowLocations').val() === "-1") {
                alert('Please select the tow location');
                self.isBusy(false);
                return;
            }



            var _SelectedServices = [];
            $(':checkbox:checked').each(function () {
                _SelectedServices.push(this.id);
            });


        }
        catch (error) {
            alert('Check your start and end odometer values');
            return;
        }

        assistData = {
            thisNewIncidentAssist: {
                IncidentID: '00000000-0000-0000-0000-000000000000',
                Location: $('#ddlFreewayDirection').val() + ' ' + $('#ddlFreeways').val() + ' ' + $('#ddlDirection').val() + ' ' + $('#txtStreet').val(),
                FreewayID: $('#ddlFreeways').val(),
                LocationID: $('#ddlLocations').val(),
                BeatSegmentID: '00000000-0000-0000-0000-000000000000',
                BeatNumber: $('#ddlBeatNumbers').val(),
                CreatedBy: '00000000-0000-0000-0000-000000000000',
                Description: $('#txtComments').val(),
                IncidentNumber: localStorage.getItem("incidentnumber"),
                AssistID: '00000000-0000-0000-0000-000000000000',
                DriverID: $('#ddlDrivers').val(),
                FleetVehicleID: $('#ddlVehicles').val(),
                CustomerWaitTime: $('#txtCustomerWaitTime').val(),
                VehiclePositionID: $('#ddlVehiclePositions').val(),
                TrafficSpeedID: $('#ddlTrafficSpeeds').val(),
                DropZone: $('#txtDropZone').val(),
                Make: $('#txtVehicleMake').val(),
                Color: $('#txtVehicleColor').val(),
                LicensePlate: $('#txtLicensePlate').val(),
                State: $('#selState').find(':selected').text().toString(),
                StartOD: $('#txtStartOD').val(), //default val
                EndOD: $('#txtEndOD').val(),
                TowLocation: $('#ddlTowLocations').val(),
                Tip: $('#txtTip').val(),
                TipDetail: $('#txtTipDetail').val(),
                CustomerLastName: $('#txtCustomerLastName').val(),
                Comments: $('#txtComments').val(),
                IsMDF: false,
                ContractorID: $('#ddlContractors').val(),
                LogNumber: $('#txtLogNumber').val(),
                IncidentTypeID: $('#ddlIncidentTypes').val(),
                TowLocationID: $('#ddlTowLocations').val(),
                VehicleTypeID: $('#ddlVehicleTypes').val(),
                x1097: x1097dt,
                x1098: x1098dt,
                OnSiteTime: onsitedt,
                Direction: 'na',
                CrossStreet1: 'na',
                CrossStreet2: 'na',
                SelectedServices: _SelectedServices,
                SurveyNum: ''
            }
        };

        var assistString = JSON.stringify(assistData);

        $.ajax({
            type: "POST",
            dataType: "json",
            url: ServiceLocation + 'AJAXFSPService.svc/PostClientAssist2',
            data: assistString,
            contentType: "application/json; charset=utf-8",
            success: function () {
                self.init();
                self.isBusy(false);
                alert("Assist Posted Successfully");
            },
            error: function (jqXHR, tranStatus, errorThrown) {
                self.init();
                self.isBusy(false);
                alert('Error posting assist, please try again');
            }
        });

    };

    self.getDrivers = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetDrivers';

        $.get(url,
               function (drivers) {
                   var selItem = '<option value="-1">--Select--</option>';

                   $('#ddlDrivers').html('');

                   $.each(drivers, function (i, driver) {
                       selItem += '<option value="' + driver.Id + '">' + driver.Name + '</option>';
                   });

                   $('#ddlDrivers').append(selItem);


               }, "json");
    };
    self.getContractors = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetContractors';
        $.get(url,
               function (contractors) {

                   $('#ddlContractors').html('');

                   var selItem = '<option value="-1">--Select--</option>';

                   $.each(contractors, function (i, contractor) {
                       selItem += '<option value="' + contractor.Id + '">' + contractor.Name + '</option>';
                   });

                   $('#ddlContractors').append(selItem);

               }, "json");
    };
    self.getVehicles = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetVehicles';
        $.get(url,
                function (vehicles) {

                    $('#ddlVehicles').html('');

                    var selItem = '<option value="-1">--Select--</option>';
                    $.each(vehicles, function (i, vehicle) {
                        selItem += '<option value="' + vehicle.Id + '">' + vehicle.Name + '</option>';
                    });

                    $('#ddlVehicles').append(selItem);



                }, "json");
    };
    self.getBeatNumbers = function () {
        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetBeatNumbers';
        $.get(url,
                function (beatNumbers) {

                    $('#ddlBeatNumbers').html('');

                    var selItem = '<option value="-1">--Select--</option>';

                    $.each(beatNumbers, function (i, beatNumber) {
                        selItem += '<option value="' + beatNumber.Id + '">' + beatNumber.Name + '</option>';
                    });

                    $('#ddlBeatNumbers').append(selItem);


                }, "json");
    }
    self.getIncidentTypes = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetIncidentTypes';

        $.get(url,
               function (results) {

                   $('#ddlIncidentTypes').html('');

                   var selItem = '<option value="-1">--Select--</option>';

                   $.each(results, function (i, result) {
                       selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlIncidentTypes').append(selItem);

               }, "json");

    };
    self.getTrafficSpeeds = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetTrafficSpeeds';

        $.get(url,
               function (results) {

                   $('#ddlTrafficSpeeds').html('');

                   var selItem = '<option value="-1">--Select--</option>';

                   $.each(results, function (i, result) {
                       selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlTrafficSpeeds').append(selItem);

               }, "json");

    };
    self.getFreeways = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetFreeways';

        $.get(url,
               function (results) {

                   $('#ddlFreeways').html('');

                   var selItem = '<option value="-1">--Select--</option>';

                   $.each(results, function (i, result) {
                       selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlFreeways').append(selItem);

               }, "json");

    };
    self.getVehiclePositions = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetVehiclePositions';

        $.get(url,
               function (results) {

                   $('#ddlVehiclePositions').html('');

                   var selItem = '<option value="-1">--Select--</option>';

                   $.each(results, function (i, result) {
                       selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlVehiclePositions').append(selItem);

               }, "json");

    };
    self.getVehicleTypes = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetVehicleTypes';

        $.get(url,
               function (results) {

                   $('#ddlVehicleTypes').html('');

                   var selItem = '';

                   $.each(results, function (i, result) {
                       selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlVehicleTypes').append(selItem);

               }, "json");

    };
    self.getTowLocations = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetTowLocations';

        $.get(url,
               function (results) {

                   $('#ddlTowLocations').html('');

                   var selItem = '';

                   $.each(results, function (i, result) {
                       if (result.Name === 'NOT (No Tow (Not Towed))')
                           selItem += '<option selected value="' + result.Id + '">' + result.Name + '</option>';
                       else
                           selItem += '<option value="' + result.Id + '">' + result.Name + '</option>';
                   });

                   $('#ddlTowLocations').append(selItem);

               }, "json");

    };
    self.getServiceTypes = function () {
        var url = $("#websitePath").attr("data-websitePath") + '/AssistAdmin/GetServiceTypes';

        $.get(url,
               function (results) {

                   $('#services').html('');

                   var chkBoxes = '';
                   $.each(results, function (i, result) {
                       chkBoxes += '<input type="checkbox" id="' + result.Id + '">&nbsp;<b>' + result.Code + '</b> (' + result.Name + ')<br/>';
                   });
                   $('#services').append(chkBoxes);

               }, "json");

    };

    function GetGuidFromWCF() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/MakeGuid';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                return result.d;
            },
            error: GetGuidError
        });
    }

    function GetGuidError(error) {
        //alert(error);
    }

    function GenerateGuid() {
        var S4 = function () {
            return Math.floor(
					Math.random() * 0x10000 /* 65536 */
				).toString(16);
        };

        return (
				S4() + S4() + "-" +
				S4() + "-" +
				S4() + "-" +
				S4() + "-" +
				S4() + S4() + S4()
			);
    }

    function dateToWcf(input) {
        var d = new Date(input);
        if (isNaN(d)) {
            return null;
        }
        return '\/Date(' + d.getTime() + '-0000)\/';
    }

    function GetDate() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetDate';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var _data = $.parseJSON(data.d);
                _data = FixDate(_data);
                localStorage.setItem("x1098time", _data);
            },
            error: function (error) {
                alert(error);
            }
        });
    }

};


$(function () {

    var viewModel = new ViewModel();
    ko.applyBindings(viewModel);
    viewModel.init();

    $(".datepicker").datepicker();

    $('.timerpicker').timepicker({
        minuteStep: 1,
        showSeconds: false,
        showMeridian: false
    });

    $("#txtServiceDate").val(moment().format('MM/DD/YYYY'));
    $("#txtDispatchTime").val(moment().format('HH:mm'));
    $("#txtAssistStartTime").val(moment().format('HH:mm'));
    $("#txtAssistEndTime").val(moment().format('HH:mm'));
});