$(document).ready(function () {

    startTimer();
    CheckAssistID();
    GetCurrentVals();

    function CheckAssistID() {
        var AssistID = localStorage.getItem("assistid");
        var IncidentDescription = localStorage.getItem("incidentdescription");
        if (AssistID == null || AssistID == "") {
            $('#selectedAssistVal').empty();
            $('#selectedAssistVal').append('New Incident');
            //$('#txtx1097').val('Set on post');
            //$('#txtx1097').attr("disabled", "disabled");
        }
        else {
            $('#selectedAssistVal').empty();
            $('#selectedAssistVal').append(IncidentDescription);
            //$('#txtx1097').val(localStorage.getItem("dispatchtime"));
            //$('#txtx1097').attr("disabled", "disabled");
        }
    }

    //Status trigger methods are only here as an attempt to keep iPads connected, they provide no extended functionality
    function statusTrigger() {
        return window.setInterval(function () { GetDate(); }, 10000);
    }

    var counterStart;
    function startTimer() {
        counterStart = FixDate('now');
        $('#txtDispatchTime').val(counterStart);
        return window.setInterval(function () { CountUp(); }, 30000);
    }

    function CountUp() {
        var today = FixDate('now');
        var diffMS = Math.abs(new Date(counterStart) - new Date(today));
        var diffMins = Math.round(((diffMS % 86400000) % 3600000) / 60000); // minutes
        $('#lblTimeCount').text(diffMins.toString());
    }

    statusTrigger();

    function GetTruckStatus() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckStatus';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: GetStatusSuccess,
            error: GetStatusError
        });
    }

    function GetStatusSuccess(result) {
        var _data = result.d;
        if (_data == 'Waiting for Driver Login') {
            document.location.href = "Logon.html";
        }
        /*
        _data = $.parseJSON(_data);
        var _status = _data.TruckStatus;
        localStorage.setItem("truckstate", _status);
        var truckState = localStorage.getItem("truckstate");
        $('#currentStatus').empty();
        $('#currentStatus').append(truckState);
        */
    }

    function GetStatusError(result) {
        var error = result;
    }

    //set textboxes and text areas to auto select on click
    $('input:text').focus(function () { $(this).select(); });
    $('input:text').mouseup(function (e) { return false; });
    $('textarea').focus(function () { $(this).select(); });
    $('textarea').mouseup(function (e) { return false; });

    $('#btnStatus').click(function () {
        ResetAndLeave();
    });

    $('#accordion').accordion({
        //collapsible: true,
        //active: false,
        fillSpace: true
    });

    /*
    $('#txtx1097').ptTimeSelect({
    onClose: function (input, widget) {
    var dt = addDate($('#txtx1097').val());
    $('#txtx1097').val(dt);
    }
    });


    $('#txtx1098').ptTimeSelect({
    onClose: function (input, widget) {
    var dt = addDate($('#txtx1098').val());
    $('#txtx1098').val(dt);
    }
    });
    */

    function addDate(timeVal) {
        var dtNow = new Date();
        var mo = dtNow.getMonth();
        mo = mo + 1;
        var yr = dtNow.getFullYear();
        var da = dtNow.getDate();
        var val = mo + '/' + da + '/' + yr + ' ' + timeVal;
        return val;
    }

    //populate the drop-downs
    PopulateDropDowns('IncidentTypes');
    PopulateDropDowns('Freeways');
    PopulateDropDowns('ServiceTypes');
    //PopulateDropDowns('DropZones');
    PopulateDropDowns('TowLocations');
    //PopulateDropDowns('Contractors');
    PopulateDropDowns('VehiclePositions');
    PopulateDropDowns('LocationCodes');
    PopulateDropDowns('TrafficSpeeds');
    PopulateDropDowns('VehicleTypes');
    $('#txtComments').empty();
    $('#txtComments').append(localStorage.getItem('incidentdescription'));
    function PopulateDropDowns(Type) {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetPreloadedData';
        var _data = 'Type=' + Type;
        switch (Type) {
            case "IncidentTypes":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: IncidentTypesSuccess,
                    error: IncidentTypesError
                });
                break;
            case "Freeways":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: FreewaysSuccess,
                    error: FreewaysError
                });
                break;
            case "ServiceTypes":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: ServiceTypesSuccess,
                    error: ServiceTypesError
                });
                break;
            case "DropZones":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: DropZonesSuccess,
                    error: DropZonesError
                });
                break;
            case "TowLocations":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: TowLocationsSuccess,
                    error: TowLocationsError
                });
                break;
            case "Contractors":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: ContractorsSuccess,
                    error: ContractorsError
                });
                break;
            case "VehiclePositions":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: VehiclePositionsSuccess,
                    error: VehiclePositionsError
                });
                break;
            case "LocationCodes":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: LocationCodesSuccess,
                    error: LocationCodesError
                });
                break;
            case "TrafficSpeeds":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: TrafficSpeedsSuccess,
                    error: TrafficSpeedsError
                });
                break;
            case "VehicleTypes":
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: _url,
                    data: _data,
                    contentType: "application/json; charset=utf-8",
                    success: VehicleTypesSuccess,
                    error: VehicleTypesError
                });
        }

    }

    function IncidentTypesSuccess(result) {
        $('#ddlIncidentTypes').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].IncidentTypeID + '">' + _result[i].IncidentTypeCode + ' (' + _result[i].IncidentTypeName + ')</option>';
            }
            $('#ddlIncidentTypes').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);		
        }
    }

    function IncidentTypesError(error) {
        //alert(error);
    }

    function FreewaysSuccess(result) {
        $('#ddlFreeways').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].FreewayID + '">' + _result[i].FreewayName + '</option>';
            }
            $('#ddlFreeways').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);		
        }
    }

    function FreewaysError(error) {
        //alert(error);
    }

    function ServiceTypesSuccess(result) {
        $('#ddlServiceTypes').empty();
        try {
            var _result = $.parseJSON(result.d);
            //var selItem = '';
            var chkBoxes = '';
            for (var i = 0; i < _result.length; i++) {
                //selItem += '<option value="' + _result[i].ServiceTypeID + '">' + _result[i].ServiceTypeCode + ' (' + _result[i].ServiceTypeName + ')</option>';
                chkBoxes += '<input type="checkbox" id="' + _result[i].ServiceTypeID + '">' + _result[i].ServiceTypeCode + ' (' + _result[i].ServiceTypeName + ')<br/>';
            }
            $('#services').append(chkBoxes);
            //$('#ddlServiceTypes').append(selItem).selectmenu('refresh', true);

        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function ServiceTypesError(error) {
        //alert(error);
    }

    function DropZonesSuccess(result) {
        $('#ddlDropZones').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].DropZoneID + '">' + _result[i].Location + '</option>';
            }
            $('#ddlDropZones').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function DropZonesError(error) {
        //alert(error);
    }

    function TowLocationsSuccess(result) {
        $('#ddlTowLocations').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                //set NOT as default selection
                if (_result[i].TowLocationCode == "NOT") {
                    selItem += '<option value="' + _result[i].TowLocationID + '" selected>' + _result[i].TowLocationCode + ' (' + _result[i].TowLocationName + ')</option>';
                }
                else {
                    selItem += '<option value="' + _result[i].TowLocationID + '">' + _result[i].TowLocationCode + ' (' + _result[i].TowLocationName + ')</option>';
                }
            }
            $('#ddlTowLocations').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function TowLocationsError(error) {
        //alert(error);
    }

    function ContractorsSuccess(result) {
        $('#ddlContractors').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].ContractorID + '">' + _result[i].ContractCompanyName + '</option>';
            }
            $('#ddlContractors').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function ContractorsError(error) {
        //alert(error);
    }

    function VehiclePositionsSuccess(result) {
        $('#ddlVehiclePositions').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                if (_result[i].VehiclePositionCode != 'RGT') {
                    selItem += '<option value="' + _result[i].VehiclePositionID + '">' + _result[i].VehiclePositionCode + '</option>';
                }
                else {
                    selItem += '<option value="' + _result[i].VehiclePositionID + '" selected>' + _result[i].VehiclePositionCode + '</option>';
                }
            }
            $('#ddlVehiclePositions').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function VehiclePositionsError(error) {
        //alert(error);
    }

    function LocationCodesSuccess(result) {
        $('#ddlLocations').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].LocationID + '">' + _result[i].LocationCode + '</option>';
            }
            $('#ddlLocations').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function LocationCodesError(error) {
        //alert(error);
    }

    function TrafficSpeedsSuccess(result) {
        $('#ddlTrafficSpeeds').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].TrafficSpeedID + '">' + _result[i].TrafficSpeedCode + '</option>';
            }
            $('#ddlTrafficSpeeds').append(selItem).selectmenu('refresh', true);
        }
        catch (error) {
            //alert(error.statusText);
        }
    }

    function TrafficSpeedsError(error) {
        //alert(error);
    }

    function VehicleTypesSuccess(result) {
        $('ddlVehicleTypes').empty();
        try {
            var _result = $.parseJSON(result.d);
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].VehicleTypeID + '">' + _result[i].VehicleTypeCode + '</option>';
            }
            $('#ddlVehicleTypes').append(selItem).selectmenu('refresh', true);
        }
        catch (error) { }
    }

    function VehicleTypesError(error) {
        //alert(error);
    }

    function GetCurrentVals() {
        var AssistID = localStorage.getItem("assistid");
        if (AssistID == null || AssistID == "") {
            return;
        }
        else {
            //$('#txtDispatchTime').val(localStorage.getItem("dispatchtime"));
            localStorage.getItem("servicetype");
            localStorage.getItem("dropzone");
            $('#txtVehicleMake').val(localStorage.getItem("make"));
            $('#txtVehicleType').val(localStorage.getItem("vehicletype"));
            localStorage.getItem("vehicleposition");
            $('#txtVehicleColor').val(localStorage.getItem("color"));
            $('#txtLicensePlate').val(localStorage.getItem("licenseplate"));
            $('#txtState').val(localStorage.getItem("state"));
            localStorage.getItem("startod");
            localStorage.getItem("endod");
            localStorage.getItem("towlocation");
            localStorage.getItem("tip");
            localStorage.getItem("tipdetail");
            $('#txtCustomerLastName').val(localStorage.getItem("customerlastname"));
            localStorage.getItem("comments");
            localStorage.getItem("ismdc");
            //$('#txtx1097').val(FixDate(localStorage.getItem("x1097")));
            //$('#txtx1098').val(FixDate(localStorage.getItem("x1098")));
            localStorage.getItem("contractor");
            localStorage.getItem("lognumber");
            localStorage.getItem("beatsegmentid");
        }
    }

    //Post Assist Data
    var _assistData;

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
                //alert(error);
            }
        });
    }

    function PostAssistData() {
        //GetDate();
        var _assistID = localStorage.getItem("assistid");
        var _incidentID = localStorage.getItem("incidentid");
        var _createdbyID = localStorage.getItem("createdbyid");
        var _onSiteTime = localStorage.getItem("onsitetime");
        if (!_onSiteTime) {
            _onSiteTime = FixDate('now');
        }
        if (undefined == _createdbyID || _createdbyID == '' || _createdbyID == null) {
            _createdbyID = localStorage.getItem("DriverName");
            //localStorage.setItem("name", DriverName)
        }
        var _incidentdescription = localStorage.getItem("incidentdescription");
        //null or unassigned incidentid means we're generating the whole thing from scratch
        if (_incidentID == null || _incidentID == '' || undefined == _incidentID) {
            _incidentID = '00000000-0000-0000-0000-000000000000';
            _assistID = '00000000-0000-0000-0000-000000000000';
        }
        try {
            if (isNaN(parseFloat($('#txtStartOD').val())) == true) {
                $.fallr('show', {
                    content: '<p>Start odometer value is invalid</p>',
                    position: 'center',
                    icon: 'lamp'
                });
                //alert('Start odometer value is invalid');
                return;
            }

            if (isNaN(parseFloat($('#txtEndOD').val())) == true) {
                $.fallr('show', {
                    content: '<p>Start odometer value is invalid</p>',
                    position: 'center',
                    icon: 'lamp'
                });
                //alert('Start odometer value is invalid');
                return;
            }

            if (parseFloat($('#txtStartOD').val()) > parseFloat($('#txtEndOD').val())) {
                $.fallr('show', {
                    content: '<p>Start odometer must be less than or equal to end odometer</p>',
                    position: 'center',
                    icon: 'lamp'
                });
                //alert('Start Odometer must be less than or equeal to End Odometer');
                return;
            }

            // make check for drop zonesif($('#
            var TowLocation = $('#ddlTowLocations option:selected').text();
            var DropZone = $('#txtDropZone').val();
            if (TowLocation == 'DRP (Drop Zone (Towed to Drop Zone))' && DropZone == "") {
                $.fallr('show', {
                    content: '<p>You must enter the drop zone</p>',
                    position: 'center',
                    icon: 'lamp'
                });
                //alert('You must enter the drop zone');
                return;
            }
        }
        catch (error) {
            $.fallr('show', {
                content: '<p>Check your start and end odometer values</p>',
                position: 'center',
                icon: 'lamp'
            });
            //alert('Check your start and end odometer values');
            return;
        }
        //var currentDate = FixDate('now');
        var currentDate = localStorage.getItem("x1098time");
        //currentDate = FixDate(currentDate); Should aready be set
        var astID = localStorage.getItem("assistid");
        var x1097dt;
        var dispTime = localStorage.getItem("dispatchtime");
        var currenttbVal = $('#txtDispatchTime').val();

        if ($('#txtDispatchTime').val()) {
            //x1097dt = FixDate('now');
            var datetime = new Date();
            var month = datetime.getMonth() + 1;
            var day = datetime.getDate();
            var year = datetime.getFullYear();
            var cDate = month + '/' + day + '/' + year;
            cDate = $('#txtDispatchTime').val();
            x1097dt = cDate;
        }

        else {
            x1097dt = dispTime;
        }
        if (astID) {
            x1097dt = localStorage.getItem("dispatchtime");
        }

        if (x1097dt == '' || x1097dt == null) {
            x1097dt = FixDate('now');
        }

        var _SelectedServices = [];
        $(':checkbox:checked').each(function () {
            _SelectedServices.push(this.id);
        });
        var _location = $('#ddlFreewayDirection').val() + ' ' + $('#ddlFreeways').val() + ' ' + $('#ddlDirection').val() + ' ' + $('#txtStreet').val();
        _assistData = {
            thisNewIncidentAssist: {
                IncidentID: _incidentID,
                Location: _location,
                FreewayID: $('#ddlFreeways').val(),
                LocationID: $('#ddlLocations').val(),
                BeatSegmentID: '00000000-0000-0000-0000-000000000000',
                //add timestamp, add at service?
                CreatedBy: _createdbyID,
                Description: $('#txtComments').val(),
                IncidentNumber: localStorage.getItem("incidentnumber"),
                AssistID: _assistID,
                DriverID: localStorage.getItem("driverid"),
                FleetVehicleID: localStorage.getItem("truckid"),
                //add dispatch time at service
                CustomerWaitTime: $('#txtCustomerWaitTime').val(),
                VehiclePositionID: $('#ddlVehiclePositions').val(),
                TrafficSpeedID: $('#ddlTrafficSpeeds').val(),
                //ServiceTypeID: $('#ddlServiceTypes').val(),
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
                ContractorID: localStorage.getItem("contractorid"),
                LogNumber: $('#txtLogNumber').val(),
                IncidentTypeID: $('#ddlIncidentTypes').val(),
                TowLocationID: $('#ddlTowLocations').val(),
                VehicleTypeID: $('#ddlVehicleTypes').val(),
                x1097: dateToWcf(x1097dt),
                //x1098: dateToWcf($('#txtx1098').val())
                x1098: dateToWcf(currentDate),
                OnSiteTime: dateToWcf(_onSiteTime),
                Direction: 'na',
                CrossStreet1: 'na',
                CrossStreet2: 'na',
                SelectedServices: _SelectedServices,
                SurveyNum: $('#txtSurveyNum').val()
            }
        };
        /*
        _assistData = {
        thisAssist: {
        AssistID: _assistID,
        IncidentID: _incidentID,
        DriverID: localStorage.getItem("driverid"),
        FleetVehicleID: localStorage.getItem("truckid"),
        IncidentTypeID: $('#ddlIncidentTypes').val()
        }
        };*/
        var assistString = JSON.stringify(_assistData);
        var _url = ServiceLocation + 'AJAXFSPService.svc/PostClientAssist2';
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _url,
            data: assistString,
            contentType: "application/json; charset=utf-8",
            success: PostAssistSuccess,
            error: PostAssistError
        });
    }

    function PostAssistSuccess() {
        /* $.fallr('show', {
        content: '<p>Alert posted successfully</p>',
        buttons: {
        button1: { text: 'OK', onclick: PostAnotherAssistQuery }
        },
        position: 'center',
        icon: 'lamp'
        });*/
        $.fallr('show', {
            content: '<p>Post another assist?</p>',
            buttons: {
                button1: { text: 'Yes', onclick: PostAnotherAssist },
                button2: { text: 'No', danger: true, onclick: ResetAndLeave }
            },
            position: 'center',
            icon: 'lamp'
        });
        //alert("Assist Posted Successfully");
        //PostAnotherAssistQuery();
    }

    function PostAnotherAssistQuery() {
        $.fallr('hide');

    }

    function PostAnotherAssist() {
        $.fallr('hide');
        $('#txtCustomerWaitTime').val('0');
        $('#txtStreet').val('');
        $('#txtVehicleMake').val('');
        $('#txtVehicleColor').val('');
        $('#txtLicensePlate').val('');
        $('#txtStartOD').val('0.0');
        $('#txtEndOD').val('0.0');
        $('#txtDropZone').val('');
        $('#txtTip').val('');
        $('#txtTipDetail').val('');
        $('#txtCustomerLastName').val('');
        $('#txtLogNumber').val('');
        //$('#txtComments').val('');
        //Second assist on an incident, create new assistID
        localStorage.setItem("assistid", "00000000-0000-0000-0000-000000000000")
        return;
    }

    function ResetAndLeave() {
        $.fallr('hide');
        //assigned incident posted, clear memory and get ready for new data
        localStorage.setItem("assistid", "");
        localStorage.setItem("incidentid", "");
        localStorage.setItem("createdbyid", "");
        localStorage.setItem("incidentdescription", "");
        localStorage.setItem("dispatchtime", "");
        var tstVal = localStorage.getItem("truckstate");
        if (tstVal == "EARLY") {
            SetTruckStatus("Roll In");
        }
        else {
            SetTruckStatus("On Patrol");
        }
    }

    function PostAssistError(error) {
        $.fallr('show', {
            content: '<p>Error posting assist, please try again</p>',
            position: 'center',
            icon: 'lamp'
        });
        //alert('Error posting assist, please try again');
    }

    $('#btnPost').click(function () {
        PostAssistData();
    });

    //Helper Functions

    function SetTruckStatus(statusMsg) {
        var logon = localStorage.getItem("logon");
        if (logon == "true") {
            var _data = "Status=" + statusMsg;
            var _url = ServiceLocation + 'AJAXFSPService.svc/SetTruckStatus';
            TruckStatus = statusMsg;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: SetStatusSuccess,
                error: SetStatusError
            });
        }
    }

    function SetStatusSuccess(result) {
        //reset incident data since the driver cancelled out of an incident
        //localStorage.setItem("AssistID", "");
        window.location.href = "Status.html";
    }

    function SetStatusError(error) {
        //alert(error);
    }

    $('#btnSurveyNum').click(function () {
        MakeSurveyNum();
    });

    function MakeSurveyNum() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/MakeSurveyNum';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: MakeSurveyNumSucess,
            error: MakeSurveyNumError
        });
    }

    function MakeSurveyNumSucess(result) {
        var data = result.d;
        $('#txtSurveyNum').val(data);
    }

    function MakeSurveyNumError(error) {
        //alert(error);
    }


    function FixDate(dtVal) {
        try {
            if (dtVal != "now") {
                var valFix = dtVal;
                valFix = dtVal.replace("/Date(", "").replace(")/", "");
                var iMil = parseInt(valFix, 10);
                var d = new Date(iMil);
                var Month = (d.getMonth() + 1).toString();
                if (Month.length < 2)
                { Month = "0" + Month; }
                var _date = (d.getDate()).toString();
                if (_date.length < 2)
                { _date = "0" + _date; }
                var Year = (d.getFullYear()).toString();
                var Hour = (d.getHours()).toString();
                if (Hour.length < 2)
                { Hour = "0" + Hour; }
                var Minute = (d.getMinutes()).toString();
                if (Minute.length < 2)
                { Minute = "0" + Minute; }
                var Second = (d.getSeconds()).toString();
                if (Second.length < 2)
                { Second = "0" + Second; }
                return Month + "/" + _date + "/" + Year + " " + Hour + ":" + Minute + ":" + Second;
            }
            else {
                var d = new Date();
                var Month = (d.getMonth() + 1).toString();
                if (Month.length < 2)
                { Month = "0" + Month; }
                var _date = (d.getDate()).toString();
                if (_date.length < 2)
                { _date = "0" + _date; }
                var Year = (d.getFullYear()).toString();
                var Hour = (d.getHours()).toString();
                if (Hour.length < 2)
                { Hour = "0" + Hour; }
                var Minute = (d.getMinutes()).toString();
                if (Minute.length < 2)
                { Minute = "0" + Minute; }
                var Second = (d.getSeconds()).toString();
                if (Second.length < 2)
                { Second = "0" + Second; }
                return Month + "/" + _date + "/" + Year + " " + Hour + ":" + Minute + ":" + Second;
            }
        }
        catch (err) {
            //alert("Bad Input");
        }
    }

});