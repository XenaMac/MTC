mtcApp.directive('wgTablePager', function () {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mtcTablePagerTemplate.html',
        controller: ['$scope', function ($scope) {
            $scope.range = function () {
                var step = 2;
                var doubleStep = step * 2;
                var start = Math.max(0, $scope.page - step);
                var end = start + 1 + doubleStep;
                if (end > $scope.pagesCount) { end = $scope.pagesCount; }

                var ret = [];
                for (var i = start; i != end; ++i) {
                    ret.push(i);
                }

                return ret;
            };
        }]
    }
});