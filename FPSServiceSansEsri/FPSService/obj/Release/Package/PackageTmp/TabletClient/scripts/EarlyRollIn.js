$(document).ready(function () {

    preloadTime();
    findType();
    function SetTruckStatus(statusMsg) {

        var logon = localStorage.getItem("logon");
        if (logon == "true") {
            var _data = "Status=" + statusMsg;
            var _url = ServiceLocation + 'AJAXFSPService.svc/SetTruckStatus';
            //TruckStatus = statusMsg;
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
        else {
            document.location.href = "Logon.html";
            return;
        }
    }

    function SetStatusSuccess(result) {
        //document.location.href = "Status.html"

        var chk = result.d;
        var _exceptionType = getQueryStringParams('Type');
        if (_exceptionType == '') {
            //SetTruckStatus('Roll In OK');
            localStorage.setItem('truckstatus', 'Roll In');
            document.location.href = "Status.html";
        }
        if (_exceptionType == undefined) {
            //SetTruckStatus('Roll In OK');
            localStorage.setItem('truckstatus', 'Roll In');
            document.location.href = "Status.html";
        }
        if (_exceptionType == 'LateLogOn') {
            //SetTruckStatus('Driver Logged On');
            document.location.href = "Status.html";
        }
        if (_exceptionType == 'LateRollOut') {
            //SetTruckStatus('Roll Out');
            document.location.href = "Status.html";
        }
        if (_exceptionType == 'EarlyRollIn') {
            //SetTruckStatus('Roll In OK');
            localStorage.setItem('truckstatus', 'Roll In');
            document.location.href = "Status.html";
        }
        if (_exceptionType == 'LateOnPatrol') {
            //SetTruckStatus('On Patrol');
            document.location.href = "Status.html";
        }
    }

    function SetStatusError(error) {
        //alert(error);
        var chk = error;
    }

    function getQueryStringParams(sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                return sParameterName[1];
            }
        }
    }

    function findType() {
        var _exceptionType = getQueryStringParams('Type');
        var strExceptionType = '';
        switch (_exceptionType) {
            case '':
                strExceptionType = 'Early Roll In';
                break;
            case undefined:
                strExceptionType = 'Early Roll In';
                break;
            case 'EarlyRollIn':
                strExceptionType = 'Early Roll In';
                break;
            case 'LateLogOn':
                strExceptionType = 'Late Log On';
                break;
            case 'LateOnPatrol':
                strExceptionType = 'Late On Patrol';
                break;
            case 'LateRollOut':
                strExceptionType = 'Late Roll Out';
                break;
            case 'EarlyLogOff':
                strExceptionType = 'Early Log Off';
                break;
            default:
                strExceptionType = 'Unknown';
                break;
        }
        $('#exceptionType').empty();
        //$('#exceptionType').replaceWith(strExceptionType);
        $('#exceptionType').html(strExceptionType);
    }





    $('#btnCancel').click(function () {

        var _exceptionType = getQueryStringParams('Type');
        if (_exceptionType == 'LateLogOn') {
            LogOff();
            setTimeout(function () { resetToWindow('Logon.html'); }, 2000);
            //document.location.href = "Logon.html";
        }
        else if (_exceptionType == 'Roll Out') {
            LogOff();
            setTimeout(function () { resetToWindow('Logon.html'); }, 2000);
            //document.location.href = "Logon.html";
        }
        else if (_exceptionType == "EarlyLogOff") {
            localStorage.setItem("logon", "true");
            SetTruckStatus('Roll In');
            setTimeout(function () { resetToWindow('Status.html'); }, 2000);
            //document.location.href = "Status.html";
        }
        else if (_exceptionType == "LateRollOut") {
            SetTruckStatus('Driver Logged On');
            setTimeout(function () { resetToWindow('Status.html'); }, 2000);
            //document.location.href = "Status.html";
        }
        else if (_exceptionType == "LateOnPatrol") {
            SetTruckStatus('Roll Out');
            setTimeout(function () { resetToWindow('Status.html'); }, 2000);
            //document.location.href = "Status.html";
        }
        else {
            SetTruckStatus('On Patrol');
            setTimeout(function () { resetToWindow('Status.html'); }, 2000);
            //document.location.href = "Status.html";
        }
        var logon = localStorage.getItem("logon");
        if (logon == "false") {
            document.location.href = "Logon.html";
            return;
        }
    });

    function resetToWindow(windowName) {
        document.location.href = windowName;
        //setTimeout(function () { fireLogoff(); }, 2000);
    }

    $('#btnComment').click(function () {
        if ($('#txtComments').val() == '' || $('#txtDateStamp').val() == '' || $('#txtLogNumber').val() == '') {
            $.fallr('show', {
                content: '<p>All fields are required</p>',
                position: 'center',
                icon: 'lamp'
            });
            return;
        }
        var _comment = $('#txtComments').val();
        var _dt = $('#txtDateStamp').val();
        var _log = $('#txtLogNumber').val();
        var _url = ServiceLocation + 'AJAXFSPService.svc/PostEarlyRollInReason';
        var _type = getQueryStringParams('Type');
        if (_type == undefined || _type == '') {
            _type = 'EarlyRollIn';
        }
        _data = "_reason=" + _comment + "&_dt=" + _dt + "&_log=" + _log + "&_type=" + _type;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: PostSuccess,
            error: PostError
        });
    });

    function PostSuccess(result) {
        updateStatusAndMoveOn();
    }

    function PostError(error) { }

    function updateStatusAndMoveOn() {
        var _exceptionType = getQueryStringParams('Type');
        if (_exceptionType == '') {
            SetTruckStatus('Roll In OK');
        }
        if (_exceptionType == undefined) {
            SetTruckStatus('Roll In OK');
        }
        if (_exceptionType == 'LateLogOn') {
            SetTruckStatus('Driver Logged On');
        }
        if (_exceptionType == 'LateRollOut') {
            SetTruckStatus('Roll Out');
        }
        if (_exceptionType == 'EarlyRollIn') {
            SetTruckStatus('Roll In OK');
            localStorage.setItem('truckstatus', 'Roll In');
        }
        if (_exceptionType == 'LateOnPatrol') {
            SetTruckStatus('On Patrol');
        }
        if (_exceptionType == "EarlyLogOff") {
            LogOff();
        }
    }

    function preloadTime() {
        var cDate = FixDate('now');
        $('#txtDateStamp').val(cDate);
    }

    //Log Off events
    function LogOff() {
        //add set logoff on service code, set truck status to logged off or something


        var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff';
        var _data = "_ok=force";
        $.ajax({
            type: "GET",
            dataType: "json",
            url: ServiceLocation + "AJAXFSPService.svc/DriverLogoff",
            data: "_ok=force",
            contentType: "application/json; charset=utf-8",
            success: RedirectToLogon,
            error: LogOffError
        });

        /*
        try {
        setTimeout(function () { fireLogoff(); }, 2000);
        }
        catch (err) {
        alert(err.responseText);
        }*/
    }

    function fireLogoff() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff';
        var _data = "_ok=force";
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _data,
            contentType: "application/json; charset=utf-8",
            success: RedirectToLogon,
            error: LogOffError
        });
    }

    function RedirectToLogon() {
        localStorage.setItem("name", "");
        localStorage.setItem("trucknumber", "");
        localStorage.setItem("truckid", "");
        localStorage.setItem("logon", "false");
        localStorage.setItem("truckstate", "loggedoff");
        document.location.href = "Logon.html";
    }
    function LogOffError(error) {
        var err = error;
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