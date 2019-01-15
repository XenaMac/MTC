'use strict';
(function () {
    mtcApp.factory('assistEntryService', function ($http, $log, $q, $rootScope, $location) {
        return {             
            postAssistEntry: function (vm, url) {
                console.time('Posting Assist Entry');

                return $http({
                    method: 'POST',
                    url: url,
                    data: JSON.stringify(vm)
                }).
                then(function (response) {
                    console.timeEnd('Posting Assist Entry');
                    return response.data;
                });


            }         
        }

    });
}());