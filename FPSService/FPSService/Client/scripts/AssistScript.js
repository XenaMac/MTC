$(document).ready(function () {

    //CheckAssistID();
    GetCurrentVals();
    function CheckAssistID() {
        var AssistID = localStorage.getItem("assistid");
        if (AssistID == null || AssistID == "") {
            $('#selectedAssistVal').empty();
            $('#selectedAssistVal').append('New Assist');
        }
        else {
            $('#selectedAssistVal').empty();
            $('#selectedAssistVal').append(AssistID);
        }
    }

    $('#btnStatus').click(function () {
        document.location.href = "Status.html";
    });

    //populate the drop-downs
    PopulateDropDowns('IncidentTypes');
    PopulateDropDowns('Freeways');
    PopulateDropDowns('ServiceTypes');
    //PopulateDropDowns('DropZones');
    PopulateDropDowns('TowLocations');
    PopulateDropDowns('Contractors');
    PopulateDropDowns('VehiclePositions');
    PopulateDropDowns('LocationCodes');
    PopulateDropDowns('TrafficSpeeds');
    PopulateDropDowns('VehicleTypes');
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
            var selItem = '';
            for (var i = 0; i < _result.length; i++) {
                selItem += '<option value="' + _result[i].ServiceTypeID + '">' + _result[i].ServiceTypeCode + ' (' + _result[i].ServiceTypeName + ')</option>';
            }
            $('#ddlServiceTypes').append(selItem).selectmenu('refresh', true);
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
                selItem += '<option value="' + _result[i].TowLocationID + '">' + _result[i].TowLocationCode + ' (' + _result[i].TowLocationName + ')</option>';
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
                selItem += '<option value="' + _result[i].VehiclePositionID + '">' + _result[i].VehiclePositionCode + '</option>';
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
            $('#txtDispatchTime').val(localStorage.getItem("dispatchtime"));
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
            $('#txtx1097').val(FixDate(localStorage.getItem("x1097")));
            $('#txtx1098').val(FixDate(localStorage.getItem("x1098")));
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
        alert(error);
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

    function PostAssistData() {
        var _assistID = localStorage.getItem("assistid");
        var _incidentID = localStorage.getItem("incidentid");
        //null or unassigned incidentid means we're generating the whole thing from scratch
        if (_incidentID == null || _incidentID == '' || undefined == _incidentID) {
            _incidentID = '00000000-0000-0000-0000-000000000000';
            _assistID = '00000000-0000-0000-0000-000000000000';
            _assistData = {
                thisNewIncidentAssist: {
                    IncidentID: _incidentID,
                    Direction: $('#txtDirection').val(),
                    FreewayID: $('#ddlFreeways').val(),
                    LocationID: $('#ddlLocations').val(),
                    BeatSegmentID: '00000000-0000-0000-0000-000000000000',
                    //add timestamp, add at service?
                    CreatedBy: localStorage.getItem("driverid"),
                    Description: $('#txtComments').val(),
                    IncidentNumber: localStorage.getItem("incidentnumber"),
                    AssistID: _assistID,
                    DriverID: localStorage.getItem("driverid"),
                    FleetVehicleID: localStorage.getItem("truckid"),
                    //add dispatch time at service
                    CustomerWaitTime: $('#txtCustomerWaitTime').val(),
                    VehiclePositionID: $('#ddlVehiclePositions').val(),
                    TrafficSpeedID: $('#ddlTrafficSpeeds').val(),
                    ServiceTypeID: $('#ddlServiceTypes').val(),
                    DropZone: $('#txtDropZone').val(),
                    Make: $('#txtVehicleMake').val(),
                    Color: $('#txtVehicleColor').val(),
                    LicensePlate: $('#txtLicensePlate').val(),
                    State: $('#txtState').val(), //default val
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
                    VehicleTypeID: $('#ddlVehicleTypes').val()
                }
            };
        }
        else {
            //incident has been assigned, need to generate assist data
        }

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
        var _url = ServiceLocation + 'AJAXFSPService.svc/PostClientAssist';
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

    function PostAssistSuccess() { alert("Assist Posted Successfully"); }

    function PostAssistError(error) {
        alert(error.responseText);
    }

    $('#btnPost').click(function () {
        PostAssistData();
    });

    //Helper Functions

    function FixDate(dtVal) {
        if (dtVal == null || dtVal == "") {
            return;
        }
        try {
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
        catch (err) {
            alert("Bad Input");
        }
    }
});