$(document).ready(function () {

    $('#btnLogon').click(function () {
        /*
        var FSPID = $('#txtFSPID').val();
        var Password = $('#txtPassword').val();
        if ($('#txtFSPID').val() == '') {
        $.fallr('show', {
        content: '<p>Please enter your FSP ID</p>',
        position: 'center',
        icon: 'lamp'
        });
        return;
        }

        if ($('#txtPassword').val() == '') {
        $.fallr('show', {
        content: '<p>Please enter your password</p>',
        position: 'center',
        icon: 'lamp'
        });
        return;
        }

        if ($('#selBeats').find(':selected').text().toString() == '000-000') {
        $.fallr('show', {
        content: '<p>Please select your assigned beat</p>',
        position: 'center',
        icon: 'lamp'
        });
        return;
        }
        localStorage.clear();
        localStorage.setItem("assignedbeat", $('#selBeats').find(':selected').text().toString());
        localStorage.setItem("assignedbeatid", $('#selBeats').find(':selected').attr('id').toString());
        */
        Logon();
    });

    $('#txtPassword').live("keypress", function (e) {
        if (e.keyCode == 13) {
            Logon();
        }
    });

    GetBeats();

    function GetBeats() { //Get a list of current beats from the service, use the result to populate the beat list drop down
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetBeats';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: GetBeatsSuccess,
            error: GetBeatsError
        });
    }

    function GetBeatsSuccess(result) {
        var _data = result.d;
        $('#selBeats').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i].BeatID + '">' + _data[i].BeatName + '</option>';
        }
        $('#selBeats').append(_selCode);
    }

    function GetBeatsError(result, error) {
        //alert(error);
    }

    function Logon() {
        var FSPID = $('#txtFSPID').val();
        var Password = $('#txtPassword').val();
        if ($('#txtFSPID').val() == '') {
            $.fallr('show', {
                content: '<p>Please enter your FSP ID</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }

        if ($('#txtPassword').val() == '') {
            $.fallr('show', {
                content: '<p>Please enter your password</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }

        if ($('#selBeats').find(':selected').text().toString() == '000-000') {
            $.fallr('show', {
                content: '<p>Please select your assigned beat</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }
        localStorage.clear();
        localStorage.setItem("assignedbeat", $('#selBeats').find(':selected').text().toString());
        localStorage.setItem("assignedbeatid", $('#selBeats').find(':selected').attr('id').toString());

        var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogon';
        var FSPID = $('#txtFSPID').val();
        var Password = $('#txtPassword').val();
        var AssignedBeat = $('#selBeats').find(':selected').attr('id').toString();
        var _data = "FSPIDNumber=" + FSPID + "&Password=" + Password + "&AssignedBeat=" + AssignedBeat;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetLogonSuccess,
            error: GetLogonError
        });
    }

    function GetLogonSuccess(result) {
        $('#result').empty();
        var _input = result.d;
        var _splitcheck = _input.indexOf("|");
        if (_splitcheck != -1) {
            var splitData = _input.split("|");
            TruckNumber = splitData[1];
            DriverName = splitData[0];
            DriverID = splitData[2];
            TruckID = splitData[3];
            ContractorID = splitData[4];
            AssignedBeat = splitData[5];
            LateLogon = splitData[6];
            ClientStatus = "LoggedOn";
            $('#errLabel').empty();
            $('#errLabel').append('Driver: ' + DriverName + ' is now logged onto Truck ' + TruckNumber);
            localStorage.setItem("logon", "true");
            localStorage.setItem("name", DriverName);
            localStorage.setItem("trucknumber", TruckNumber);
            localStorage.setItem("driverid", DriverID);
            localStorage.setItem("truckid", TruckID);
            localStorage.setItem("contractorid", ContractorID);
            localStorage.setItem("assignedbeat", AssignedBeat);
            if (LateLogon == 'late') {
                document.location.href = "EarlyRollIn.html?Type=LateLogOn";
            }
            else {
                document.location.href = "Status.html";
            }
        }
        else {
            $('#errLabel').empty();
            $('#errLabel').append('<h1>' + _input + '</h1>');
        }
    }

    function GetLogonError(error) {
        ClientStatus = "LoggedOff";
        $('#errLabel').empty();
        $('#errLabel').append(error);
        //alert(error);
    }
});