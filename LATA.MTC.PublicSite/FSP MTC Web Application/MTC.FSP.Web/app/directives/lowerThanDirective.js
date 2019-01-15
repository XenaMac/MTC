mtcApp.directive('lowerThan', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {

                var validate = function (viewValue) {
                    var comparisonModel = attrs.lowerThan;

                    if (!viewValue || !comparisonModel) {
                        // It's valid because we have nothing to compare against
                        ngModelCtrl.$setValidity('lowerThan', true);
                    }

                    // It's valid if model is lower than the model we're comparing against
                    ngModelCtrl.$setValidity('lowerThan', parseInt(viewValue, 10) < parseInt(comparisonModel, 10));
                    return viewValue;
                };

                ngModelCtrl.$parsers.unshift(validate);
                ngModelCtrl.$formatters.push(validate);

                attrs.$observe('lowerThan', function (comparisonModel) {
                    return validate(ngModelCtrl.$viewValue);
                });

            });            
        }
    }
});