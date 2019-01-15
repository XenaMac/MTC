$(document).ready(function () {
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