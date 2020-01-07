require([
    "Custom/Truck",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/_base/window",
    "dojo/domReady!"
], function (Truck, domConstruct, domClass, win) {
    describe('Truck', function () {
        var testTruck,
            websiteURL;

        beforeEach(function () {
            websiteURL = domConstruct.create('div', { innerHTML: "" }, win.body());
            domClass.add(websiteURL, "websiteUrl");
            domClass.add(websiteURL, "hidden");
            var map = {};
            var graphicsLayer = {};
            testTruck = new Truck({
                map: map,
                graphicsLayer: graphicsLayer
            });
            testTruck.truckNumber = 1;
            testTruck.beatNumber = 777;
            testTruck.driverName = 'some driver';
            testTruck.heading = 0;
            testTruck.latitude = -122.000;
            testTruck.longitude = 37.000;
            testTruck.callsign = 'rubber ducky';
            testTruck.contractorName = 'ABC Towing';
            testTruck.speed = 100;
            testTruck.vehicleState = "OnPatrol";
            testTruck.determineState();
        });

        afterEach(function () {
            testTruck = null;
        });

        it('should have a call sign', function () {
            expect(testTruck.callsign).toEqual('rubber ducky');
        });

        it('should have a heading', function () {
            expect(testTruck.heading).toEqual(0);
        });

        it('should have a status', function () {
            expect(testTruck.statusString).toEqual("On Patrol");
        });

        it('should have an icon', function () {
            expect(testTruck.icon).toEqual('/Content/Images/mtc_icons_v2_green.png');
        });

        it('should have a driver', function () {
            expect(testTruck.driverName).toEqual('some driver');
        });

        it('should have a truckNumber', function () {
            expect(testTruck.truckNumber).toEqual(1);
        });

        it('should have a beat number', function () {
            expect(testTruck.beatNumber).toEqual(777);
        });

        it('should have a latitude and longitude', function () {
            expect(testTruck.latitude).toEqual(-122.000);
            expect(testTruck.longitude).toEqual(37.000);
        });

        it('should have a contractor', function () {
            expect(testTruck.contractorName).toEqual('ABC Towing');
        });

        it('should have a speed', function () {
            expect(testTruck.speed).toEqual(100);
        });

        it('should have a vehicle state', function () {
            expect(testTruck.vehicleState).toEqual("OnPatrol");
        });

        it('should update its position once a second', function () {

        });
    });
});