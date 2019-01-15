mtcApp.directive('autoCompleteLocation', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {

                element.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: $(".websiteUrl").text().trim() + "/Home/GetLocations",
                            dataType: "json",
                            data: {
                                featureClass: "P",
                                style: "full",
                                maxRows: 12,
                                name_startsWith: request.term
                            },
                            success: function (data) {
                                try {
                                    console.log('Locations ', data);
                                    response($.map(data, function (item) {
                                        return {
                                            id: item,
                                            value: item
                                        }
                                    }));
                                } catch (e) {

                                }

                            },
                            error: function (data) {
                                console.error('Autocomplete Locations', data);
                            }
                        });

                    },
                    minLength: 0,
                    select: function (event, ui) {

                        scope.location = ui.item.id;

                    },
                    open: function () {
                        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                    },
                    close: function () {
                        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                    }
                });

            });
        }
    }
});