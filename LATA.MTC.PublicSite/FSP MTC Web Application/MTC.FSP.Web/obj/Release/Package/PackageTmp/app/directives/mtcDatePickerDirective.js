mtcApp.directive('mtcDatePicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {

                var minDateFromToday = -1000;
                var disableWeekends = false;

                if (attrs.minDateFromToday)
                    minDateFromToday = attrs.minDateFromToday;

                if (attrs.disableWeekends)
                    disableWeekends = eval(attrs.disableWeekends);

                element.datepicker({
                    minDate: '+' + minDateFromToday,
                    beforeShowDay: disableWeekends === true ? $.datepicker.noWeekends : ''
                });

                //if (attrs.minDateFromToday) {

                //} else {
                //    if (attrs.disableWeekends) {
                //        element.datepicker({
                //            beforeShowDay: $.datepicker.noWeekends
                //        });
                //    }
                //    else {
                //        element.datepicker();
                //    }
                //}
            });
        }
    }
});