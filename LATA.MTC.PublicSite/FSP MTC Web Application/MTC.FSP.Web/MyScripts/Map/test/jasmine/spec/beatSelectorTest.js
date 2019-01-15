require(["Custom/beatSelector"], function (BeatSelector) {
    describe("Beat Selector", function () {
        var beatSelector;
        var beatLayer;

        beforeEach(function () {
            beatSelector = new BeatSelector({
                title: "",
                class: "nonModal",
                beatLayer: beatLayer
            });
            beatSelector.show();
        });

        afterEach(function () {
            beatSelector.destroy();
        });

       it("should list out all of the active beats", function () {

        });

        it("should let the user pick only one beat", function () {

        });

        it('should return the selected beat', function () {

        });
    });
});
