mtcApp.directive('showErrors', function () {
    return {
        restrict: 'A',
        require: '^form',
        link: function (scope, el, attrs, formCtrl) {
            // find the text box element, which has the 'name' attribute
            var inputEl = el[0].querySelector("[name]");
            // convert the native text box element to an angular element
            var inputNgEl = angular.element(inputEl);
            // get the name on the text box so we know the property to check
            // on the form controller
            var inputName = inputNgEl.attr('name');

            // only apply the has-error class after the user leaves the text box
            inputNgEl.bind('blur', function () {
                el.toggleClass('has-error', formCtrl[inputName].$invalid);
                el.toggleClass('has-success', formCtrl[inputName].$valid);

                el.toggleClass('has-feedback', formCtrl[inputName].$valid);
                el.toggleClass('has-feedback', formCtrl[inputName].$invalid);

                var spanEl = el[0].querySelector("span");
                var spanNgEl = angular.element(spanEl);

                spanNgEl.toggleClass('glyphicon-remove', formCtrl[inputName].$invalid);
                spanNgEl.toggleClass('glyphicon-ok', formCtrl[inputName].$valid);

            })
        }
    }
});