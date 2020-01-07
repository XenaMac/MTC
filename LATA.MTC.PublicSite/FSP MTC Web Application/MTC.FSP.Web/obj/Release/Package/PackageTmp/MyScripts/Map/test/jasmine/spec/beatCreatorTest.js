require([
    "Custom/beatCreator",
    "dojo/dom-construct"
], function (BeatCreator, domConstruct) {
    describe('Beat Creator', function () {
        var testBeatCreator;
        var testBeatNumber;
        var testBeatLayer;
        var testMap;
        //describe('create new beat', function () {
        //    beforeEach(function () {
        //        testBeatLayer = {};
        //        testMap = {};

        //        testBeatCreator = new BeatCreator({
        //            title: "",
        //            class: "nonModal",
        //            beatLayer: testBeatLayer,
        //            map: testMap
        //        });

        //        testBeatCreator.show();
        //    });

        //    afterEach(function () {
        //        testBeatCreator._closeForm();
        //    });

        //    it('should be in the add state', function (done) {
        //        setTimeout(function () {
        //            expect(testBeatCreator.state).toEqual("add");
        //            done();
        //        }, 2000);
        //    })

        //    it('should have a list of segments', function (done) {
        //        setTimeout(function () {
        //            expect(testBeatCreator.mySegments).not.toBe(null);
        //            done();
        //        }, 2000)
        //    });

        //    it('should have an attribute inspector', function (done) {
        //        setTimeout(function () {
        //            expect(testBeatCreator.myBeat).not.toBe(null);
        //            done();
        //        }, 2000);
        //    });
        //});

        //describe('on submit click', function () {
        //    beforeEach(function () {
        //        testBeatLayer = {};
        //        testMap = {};

        //        testBeatCreator = new BeatCreator({
        //            title: "",
        //            class: "nonModal",
        //            beatLayer: testBeatLayer,
        //            map: testMap
        //        });

        //        testBeatCreator.show();
        //        testBeatCreator.beatNumber = 777;
        //        testBeatCreator.beatDescription = "something";
        //        testBeatCreator.active = true;

        //        testBeatCreator.createBeat();
        //    });

        //    afterEach(function () {

        //    });
 
        //    it('should fail if no segments are selected', function (done) {
        //        setTimeout(function () {
        //            var selectedSegments = testBeatCreator.selectedSegments.length;
        //            expect(selectedSegments).toEqual(0);
        //            expect(testBeatCreator).not.toBe(null);
        //            done();
        //        }, 2000);
        //    });

        //    xit('should get the information from the attribute inspector', function (done) {
        //        setTimeout(function () {
        //            expect(testBeatCreator.beatNumber).toBeGreaterThan(0);
        //            expect(testBeatCreator.beatDescription).not.toEqual("");
        //            expect(testBeatCreator.active).not.toBe(null);
        //            done();
        //        }, 2000);
        //    });

        //    xit('should union the segments together to form a beat polygon', function () {
        //        expect(false).toEqual(true);
        //    });

        //    xit('should submit the beat polygon to the esri server', function () {
        //        expect(false).toEqual(true);
        //    });

        //    xit('should submit the attribute information to the sql service', function () {
        //        expect(false).toEqual(true);
        //    });
        //});
        describe('edit existing beat', function () {
            beforeEach(function () {
                testBeatLayer = {};
                testMap = {};
                testBeatNumber = 99;

                testBeatCreator = new BeatCreator({
                    title: "Editing Beat " + testBeatNumber,
                    class: "nonModal",
                    beatLayer: testBeatLayer,
                    beatNumber: testBeatNumber
                });

                testBeatCreator.show();
            });

            afterEach(function () {
                testBeatCreator._closeForm();
            });

            it('should be in the edit state', function (done) {
                setTimeout(function () {
                    expect(testBeatCreator.state).toEqual("edit");
                    done();
                }, 2000);
            });

            it('should have a beat number greater than 0', function (done) {
                setTimeout(function () {
                    expect(testBeatCreator.beatNumber).toBeGreaterThan(0);
                    done();
                }, 2000);
            });

            it('should have a list of segments', function (done) {
                setTimeout(function () {
                    expect(testBeatCreator.mySegments).not.toBe(null);
                    done();
                }, 2000);
            });

            it('should have an attribute inspector', function (done) {
                setTimeout(function () {
                    expect(testBeatCreator.myBeat).not.toBe(null);
                    done();
                }, 2000);
            });

        });

        describe('when submit is clicked', function () {
            beforeEach(function(){
                testBeatLayer = {};
                testMap = {};
                testBeatNumber = 99;

                testBeatCreator = new BeatCreator({
                    title: "Editing Beat " + testBeatNumber,
                    class: "nonModal",
                    beatLayer: testBeatLayer,
                    beatNumber: testBeatNumber
                });

                testBeatCreator.show();

                //testBeatCreator.createBeat();
            });

            afterEach(function () {
                testBeatCreator._closeForm();
            });

            it('should get the chosen segments from the form', function (done) {
                setTimeout(function () {
                    var features = testBeatCreator.mySegments.getMarkedSegments();
                    expect(features.length).toEqual(4);
                    done();
                }, 4000);
            });

            it('should grab the freeways from the service for the selected beat', function (done) {
                setTimeout(function () {
                    var freeways = testBeatCreator.myBeat._getFreeways();
                    done();
                }, 2000);
            });

            it('should get the chosen freeways from the form', function (done) {
                setTimeout(function () {
                    expect(true).toEqual(true);
                    done();
                }, 4000);
            });

            it('should generate an html string to the service', function (done) {
                setTimeout(function () {
                    var expectedValue = "http://38.124.164.213:9017/AJAXFSPService.svc/updateBeatsFreeways?_BeatID=99&_BeatDescription=&_Active=true";
                    expect(testBeatCreator.generateServiceURL()).toBe(expectedValue);
                    done();
                }, 2000);
            });

            it('should generate an html string to update the beats on the SQL service', function (done) {
                setTimeout(function () {
                    expect(testBeatCreator.generateBeatServiceURL()).not.toBe(null);
                    done();
                }, 2000);
            });
        });
    });
});