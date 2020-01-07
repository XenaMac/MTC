angular.module('mtcAppFilters', []).filter('datefilter', function () {
    return function (input) {
        if (moment(input).isValid())
            return moment(input).format('MM/DD/YYYY');
        else
            return "";
    };
})
.filter('datetimefilter', function () {
    return function (input) {
        if (moment(input).isValid())
            return moment(input).format('MM/DD/YYYY hh:mm A');
        else
            return "";
    };
})
.filter('datetimeHourMinute', function () {
    return function (input) {
        if (moment(input).isValid())
            return moment(input).format('HH:mm');
        else
            return "";
    };
})
.filter('decLatLonToDegreeLatLon', function () {
    return function (input) {
        if (input) {
            var lat = eval(input.split("~")[0]);
            var lon = eval(input.split("~")[1]);
            var point = new GeoPoint(lon, lat);            
            return point.getLatDeg() + ' ' + point.getLonDeg()
        }
        else
            return "";
    };
})
;