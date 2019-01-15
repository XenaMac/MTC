require([
    "Custom/beatAttributeInspector",
    "dojo/_base/window",
    "dojo/dom-construct",
    "dojo/domReady!"
], function (beatAttributeInspector, win, domConstruct) {
    describe('Beat Attribute Inspector', function () {
        var mainDiv,
            testInspector,
            testBeatNumber;

        beforeEach(function () {
            testBeatNumber = 99;
            mainDiv = domConstruct.create('div', { id: "mainDiv" }, win.body());
            testInspector = new beatAttributeInspector({
                beatNumber: testBeatNumber
            }, mainDiv);
        });

        afterEach(function () {
            testInspector.destroy();
            domConstruct.destroy(mainDiv);
        });

        it('should have a beat id box', function () {

        });

        it('should have a checkbox for active status of the beat', function () {

        });

        it('should have a textbox for the beat description', function () {

        });

        it('should have list of freeways for the beat', function () {
            testInspector.getFormValues();
            expect(testInspector.freeways.length).toBe(4);
        });

        it('should return an object of the entered data', function () {

        });

        describe('when in edit mode', function () {
            beforeEach(function () {

            });

            afterEach(function () {

            });

            it('should have a beat number', function () {

            });

            it('should remove the beat id box', function () {

            });

            it('should fill in the active checkbox for the chosen beat', function () {

            });

            it('should fill in the description for the chosen beat', function () {

            });

            it('should fill in the freeways for the chosen beat', function () {

            });
        });
    });
});