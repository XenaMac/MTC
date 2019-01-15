beforeEach(function () {
    jasmine.addMatchers({
        toBeFollowing: function () {
            return {
                compare: function (actual, expected) {
                    var follower = actual;

                    return {
                        pass: follower.getFollowState() === expected
                    }
                }
            };
        },
        compareExtent: function () {
            // written to handle differences in real numbers inside the extent
            return {
                compare: function (actual, expected) {
                    var extent1 = actual;
                    var extent2 = expected;
                    // make sure that the x, y and spatial references are the same
                    return {
                        pass: (actual.y.toPrecision(4) == expected.y.toPrecision(4)) && (actual.x.toPrecision(4) == expected.x.toPrecision(4))
                    }
                }
            };
        }
    });
});