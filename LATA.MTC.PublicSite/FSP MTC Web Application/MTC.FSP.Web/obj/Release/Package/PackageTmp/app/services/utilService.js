'use strict';
(function() {
    mtcApp.factory('utilService', function() {
        return {
            findArrayElement: function(arr, propName, propValue) {
                for (var i = 0; i < arr.length; i++)
                    if (arr[i][propName] === propValue) return arr[i];

            },
            getPolygonCoords: function(polygon) {
                var coords = [];
                if (!polygon) return coords;

                var len = polygon.getPath().getLength();
                for (var i = 0; i < len; i++) {
                    var xy = polygon.getPath().getAt(i);
                    var coord = {
                        ExtensionData: {},
                        lat: xy.lat(),
                        lon: xy.lng()
                    };
                    coords.push(coord);
                }

                return coords;
            },
            clearPolygonCoords: function(polygon) {
                try {
                    if (!polygon) return;
                    var len = polygon.getPath().getLength();
                    for (var i = 0; i < len; i += 2) {
                        polygon.getPath().removeAt(i);
                    }
                } catch (e) {
                    console.log(e);
                }
            }
        };

    });
}());