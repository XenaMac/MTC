$(document).ready(function () {

    LoadCurrentTrucks();
    //Load data from AJAXFSPService.GetAllTrucks into an HTML Table and append the table to a div with id=CurrentConnects
    window.setInterval(function () { LoadCurrentTrucks(); }, 10000);


    function LoadCurrentTrucks() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetAllTrucks",
            contentType: "application/json; charset=utf-8",
            success: GetTrucksSuccess,
            error: GetTrucksError
        });
    }



    function GetTrucksSuccess(result) {
        var _data = result.d;
        var _truckList = $.parseJSON(_data);
        //var _tblCode = '<div class="subTitle"><h2>Currently Connected Trucks</h2></div>';
        var _tblCode = '<center><br/>';
        _tblCode += '<table class="dashboardTable">';
        _tblCode += '<thead><th>Select</th><th>Remove</th><th>Reboot</th><th>Alarm Status</th><th>Truck Number</th><th>Beat Number</th>' +
            '<th>Call Sign</th><th>Cell</th><th>IP Address</th><th>Heading</th><th>Speed</th><th>Lat</th><th>Lon</th><th>Vehicle State</th>' +
            '<th>Alarms</th><th>Location</th><th>Last Message</th><th>Contractor Name</th></thead><tbody>';

        $('#CurrentConnects').empty();
        for (var i = 0; i < _truckList.length; i++) {
            var SpeedingTime = FixDate(_truckList[i].SpeedingTime);
            var OutOfBoundsTime = FixDate(_truckList[i].OutOfBoundsTime);
            var LastMessage = FixDate(_truckList[i].LastMessage);
            var StatusStarted = FixDate(_truckList[i].StatusStarted);
            _tblCode += '<tr>';
            _tblCode += '<td><input type="button" id="' + _truckList[i].IPAddress + '" value="Detail" class="detailClass"/></td>';
            _tblCode += '<td><input type="button" id="' + _truckList[i].IPAddress + '" value="Kill" class="killClass"/></td>';
            _tblCode += '<td><input type="button" id="' + _truckList[i].IPAddress + '" value="Reboot" class="rebootClass"/></td>';
            _tblCode += '<td><input type="button" id="' + _truckList[i].IPAddress + '" value="Alarms" class="alarmsClass"/></td>';
            _tblCode += '<td>' + _truckList[i].TruckNumber + '</td>';
            _tblCode += '<td>' + _truckList[i].BeatNumber + '</td>';
            _tblCode += '<td>' + _truckList[i].CallSign + '</td>';
            _tblCode += '<td>' + _truckList[i].Cell + '</td>';
            _tblCode += '<td>' + _truckList[i].IPAddress + '</td>';
            _tblCode += '<td>' + _truckList[i].Heading + '</td>';
            _tblCode += '<td>' + _truckList[i].Speed + '</td>';
            _tblCode += '<td>' + _truckList[i].Lat + '</td>';
            _tblCode += '<td>' + _truckList[i].Lon + '</td>';
            _tblCode += '<td>' + _truckList[i].VehicleState + '</td>';
            _tblCode += '<td>' + _truckList[i].Alarms + '</td>';
            _tblCode += '<td>' + _truckList[i].Location + '</td>';
            _tblCode += '<td>' + LastMessage + '</td>';
            _tblCode += '<td>' + _truckList[i].ContractorName + '</td>';
            
            _tblCode += '</tr>';
        }
        _tblCode += '</tbody></table>';
        _tblCode += '</center>';
        $('#CurrentConnects').append(_tblCode);
        $('.detailClass').on("click", GetDetail);
        $('.killClass').on("click", KillTruck);
        $('.rebootClass').on("click", RebootTruck);
        $('.alarmsClass').on("click", AlarmStatus);
        $('#conCount').text("Connection Count: " +_truckList.length);
    }

    function GetDetail() {
        localStorage.setItem("ip", this.id);
        document.location.href = "TruckDetail.aspx?ip=" + this.id;
    }

    function AlarmStatus() {
        document.location.href = "TruckAlarmStatus.aspx?ip=" + this.id;
    }

    function KillTruck() {
        var ip = this.id;
        _data = "ip=" + ip;
        $.ajax({
            type: "GET",
            dataType: "json",
            data: _data,
            url: "../AJAXFSPService.svc/KillTruck",
            contentType: "application/json; charset=utf-8",
            success: alert("Truck removed, wait for next update")
        });
    }

    function RebootTruck() {
        var ip = this.id;
        _data = "IPAddr=" + ip;
        var ok = confirm("This will force the truck to reboot, are you sure you want to do this?");
        if (ok == true) {
            $.ajax({
                type: "GET",
                dataType: "json",
                data: _data,
                url: "../AJAXFSPService.svc/SendMessage",
                contentType: "application/json; charset=utf-8",
                success: alert("Truck rebooting, please be patient")
            });
        }
        else {
            alert("Opertation aborted, truck will not reboot");
        }
    }

    function GetTrucksError(error) {
        //alert(error.responseText);
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
            return "Bad Input";
        }
    }
});