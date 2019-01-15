'use strict';
(function() {
    mtcApp.factory('mapService', function($http, $log, $q, $rootScope, $location) {
        return {
            getBeats: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetBeats'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getBeatSegmentPolygons: function(beatId) {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetBeatSegmentPolygons?beatId=' + beatId
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getSegmentPolygons: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetSegmentPolygons'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getYardPolygons: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetYardPolygons'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getDropSitePolygons: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetDropSitePolygons'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getCallSigns: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Map/GetCallSignLocations'
                }).
                then(function(response) {
                    return response.data;
                });
            },

            saveYard: function(yard) {
                return $http({
                    method: 'POST',
                    data: yard,
                    url: $(".websiteUrl").text().trim() + '/Map/SaveYardPolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            saveSegment: function(segment) {
                return $http({
                    method: 'POST',
                    data: segment,
                    url: $(".websiteUrl").text().trim() + '/Map/SaveSegmentPolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            saveBeat: function(beat) {
                return $http({
                    method: 'POST',
                    data: beat,
                    url: $(".websiteUrl").text().trim() + '/Map/SaveBeat'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            saveDropSite: function(dropSite) {
                return $http({
                    method: 'POST',
                    data: dropSite,
                    url: $(".websiteUrl").text().trim() + '/Map/SaveDropSitePolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },

            addYard: function(yard) {
                return $http({
                    method: 'POST',
                    data: yard,
                    url: $(".websiteUrl").text().trim() + '/Map/AddYardPolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            addSegment: function(segment) {
                return $http({
                    method: 'POST',
                    data: segment,
                    url: $(".websiteUrl").text().trim() + '/Map/AddSegmentPolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            addBeat: function(beat) {
                return $http({
                    method: 'POST',
                    data: beat,
                    url: $(".websiteUrl").text().trim() + '/Map/AddBeat'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            addDropSite: function(dropSite) {
                return $http({
                    method: 'POST',
                    data: dropSite,
                    url: $(".websiteUrl").text().trim() + '/Map/AddDropSitePolygon'
                }).
                then(function(response) {
                    return response.data;
                });
            },


            deleteYard: function(yardId) {
                return $http({
                    method: 'POST',
                    data: {
                        id: yardId
                    },
                    url: $(".websiteUrl").text().trim() + '/Map/DeleteYard'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            deleteSegment: function(segmentId) {
                return $http({
                    method: 'POST',
                    data: {
                        id: segmentId
                    },
                    url: $(".websiteUrl").text().trim() + '/Map/DeleteSegment'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            deleteBeat: function(beatId) {
                return $http({
                    method: 'POST',
                    data: {
                        id: beatId
                    },
                    url: $(".websiteUrl").text().trim() + '/Map/DeleteBeat'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            deleteDropSite: function(dropSiteId) {
                return $http({
                    method: 'POST',
                    data: {
                        id: dropSiteId
                    },
                    url: $(".websiteUrl").text().trim() + '/Map/DeleteDropSite'
                }).
                then(function(response) {
                    return response.data;
                });
            }
        };
    });
}());