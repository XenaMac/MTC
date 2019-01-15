require([
    "Custom/segmentSelector",
    "dojo/_base/window",
    "dojo/dom-construct",
    "dojo/dom",
    "dojo/query",
    "dojo/domReady!"
], function (segmentSelector, win, domConstruct, dom, query) {
    describe('Segment Selector', function () {
        var testSelector,
            mainDiv;

        beforeEach(function () {
            mainDiv = domConstruct.create('div', { id: 'segmentSelector' }, win.body());
            testSelector = new segmentSelector({

            }, mainDiv);
        });

        afterEach(function () {
            testSelector.destroy();
            domConstruct.destroy(mainDiv);
        });

        it('should get the current list of segments from the server', function (done) {
            setTimeout(function () {
                expect(testSelector.features.length).toBeGreaterThan(0);
                done();
            }, 2000);
        });

        it('should have a check box for each segment', function (done) {
            setTimeout(function () {
                var featureLength = testSelector.features.length;
                var selectorNode = query('#segmentSelector')[0];
                var checkboxes = query('.segmentSelection', selectorNode).length;
                expect(featureLength).toEqual(checkboxes);
                done();
            }, 2000);
        });

        describe('when in edit mode', function () {
            beforeEach(function () {
                testSelector.edit = true;
                testSelector.beatNumber = 99;
            });

            afterEach(function () {
                testSelector.edit = false;
                testSelector.beatNumber = 0;
            });

            it('should have an edit state of "edit"', function () {
                expect(testSelector.edit).toEqual(true);
            });

            it('should have a beat number', function () {
                expect(testSelector.beatNumber).toBeGreaterThan(0);
            });

            it('gets the segments for the beat being edited', function () {
                expect(false).toEqual(true);
            });

            it('should mark the segments for the beat being edited', function () {
                expect(false).toEqual(true);
            });
        })

    });
});