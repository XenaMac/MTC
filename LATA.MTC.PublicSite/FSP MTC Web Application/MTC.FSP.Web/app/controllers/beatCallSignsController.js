(function () {
    'use strict';
    mtcApp.controller('beatCallSignsController',
        function beatCallSignsController($scope, $rootScope, beatsService) {

            $scope.header = 'Beats & Call Signs';
            $scope.beatCallSigns = [];

            $scope.getBeatCallSigns = function () {

                beatsService.getBeatCallSigns().then(function (results) {
                    $scope.beatCallSigns = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.beatCallSigns.push(results[i]);
                    }

                    $rootScope.redrawTable();
                });

            };
            $scope.getBeatCallSigns();
        }
    );
}());