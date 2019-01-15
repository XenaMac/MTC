$(document).ready(function () {

    var _ip = localStorage.getItem("ip");
    GetData();
    window.setInterval(function () { GetData(); }, 10000);

    function GetData() {
        $.ajax({
            type: "GET",
            dataType: "json",
            data: "ipaddr=" + _ip,
            url: "../AJAXFSPService.svc/GetTruckData",
            contentType: "application/json; charset=utf-8",
            success: GetTruckSuccess,
            error: GetTruckError
        });
    }

    function GetTruckSuccess(result) {
        var _data = result.d;
        var _truckData = $.parseJSON(_data);
        ClearData();
        $('#lblTruckNumber').append(_truckData.TruckNumber);
        $('#lblIPAddress').append(_truckData.IPAddress);
        $('#lblDirection').append(_truckData.Direction);
        $('#lblSpeed').append(_truckData.Speed);
        $('#lblLat').append(_truckData.Lat);
        $('#lblLon').append(_truckData.Lon);
        $('#lblVehicleState').append(_truckData.VehicleState);
        $('#lblAlarms').append(_truckData.Alarms.toString());
        $('#lblSpeedingAlarm').append(_truckData.SpeedingAlarm.toString());
        $('#lblSpeedingValue').append(_truckData.SpeedingValue);
        $('#lblSpeedingTime').append(FixDate(_truckData.SpeedingTime));
        $('#lblOutOfBoundsAlarm').append(_truckData.OutOfBoundsAlarm.toString());
        $('#lblOutOfBoundsMessage').append(_truckData.OutOfBoundsMessage);
        $('#lblOutOfBoundsTime').append(FixDate(_truckData.OutOfBoundsTime));
        $('#lblHeading').append(_truckData.Heading);
        $('#lblLastMessage').append(FixDate(_truckData.LastMessage));
        $('#lblContractorName').append(_truckData.ContractorName);
        $('#lblBeatNumber').append(_truckData.BeatNumber);
        $('#lblGPSRate').append(_truckData.GPSRate);
        $('#lblLog').append(_truckData.Log);
        $('#lblVersion').append(_truckData.Version);
        $('#lblServerIP').append(_truckData.ServerIP);
        $('#lblSFTPServerIP').append(_truckData.SFTPServerIP);
        $('#lblGPSStatus').append(_truckData.GPSStatus);
        $('#lblGPSDOP').append(_truckData.GPSDOP);

    }

    function GetTruckError(result) {
        alert('Bad mojo, dude');
    }

    function ClearData() {
        $('#lblTruckNumber').empty();
        $('#lblIPAddress').empty();
        $('#lblDirection').empty();
        $('#lblSpeed').empty();
        $('#lblLat').empty();
        $('#lblLon').empty();
        $('#lblVehicleState').empty();
        $('#lblAlarms').empty();
        $('#lblSpeedingAlarm').empty();
        $('#lblSpeedingValue').empty();
        $('#lblSpeedingTime').empty();
        $('#lblOutOfBoundsAlarm').empty();
        $('#lblOutOfBoundsMessage').empty();
        $('#lblOutOfBoundsTime').empty();
        $('#lblHeading').empty();
        $('#lblLastMessage').empty();
        $('#lblContractorName').empty();
        $('#lblBeatNumber').empty();
        $('#lblGPSRate').empty();
        $('#lblLog').empty();
        $('#lblVersion').empty();
        $('#lblServerIP').empty();
        $('#lblSFTPServerIP').empty();
        $('#lblGPSStatus').empty();
        $('#lblGPSDOP').empty();
    }

    function FixDate(dtVal) {
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
            //return "Bad Input";
        }
    }
});