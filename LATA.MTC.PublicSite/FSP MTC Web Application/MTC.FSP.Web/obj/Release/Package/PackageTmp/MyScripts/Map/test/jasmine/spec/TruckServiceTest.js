require([
    "dojo/_base/window",
    "dojo/dom-construct",
    "esri/map",
    "dojo/dom-class",
    "dojo/on",
    "Custom/TruckService",
    "dojo/domReady!"
    ], function (win, domConstruct, Map, domClass, on, TruckService) {
    describe('Truck Service', function () {
        var testTruckService,
            mainDiv,
            div,
            testMap,
            truckList;

        beforeEach(function (done) {
            // make sure to have trucks running on the server so that all the tests will pass
            mainDiv = domConstruct.create('div', { id: "mainDiv" }, win.body());
            div = domConstruct.create('div', { style: { width: "300px", height: "200px" }});
            testMap = new Map(div, {
                basemap: "streets",
                center: [-122.000, 37.880],
                zoom: 9
            });

            testMap.on('load', function () {
                testTruckService = new TruckService({
                    map: testMap
                });
                testTruckService.startService();
                testTruckService.on("data-downloaded", function(evt){
                    truckList = evt.data;
                    done();
                });
            });
        }, 6000);

        afterEach(function (done) {
            testTruckService.stopService();
            done();
            //testMap.destroy();
            //domConstruct.destroy(div);
            //domConstruct.destroy(mainDiv);
            //testTruckService = null;
        }, 10000);

        xit('should get truck data from the service', function () {
            expect(truckList.length).toBeGreaterThan(0);
        });

        xit('should be able to filter the list of trucks by contractor', function () {
            testTruckService.filterName = "ZZ - Los Alamos Technical Associates (LATA)";
            var expectedTrucks = "LATA-VIRT";
            expect(truckList[0].truckNumber).toEqual(expectedTrucks);
        });

        it('should add a graphics layer to the map to plot the trucks on', function () {
            expect(testTruckService.graphicsLayer).not.toBe(null);
        });

        xit('should plot the trucks on the map', function () {
            expect(testTruckService.graphicsLayer.graphics.length).toBeGreaterThan(0);
        });

        describe('when a truck is clicked', function () {
            it('should return the current truck number', function () {
                var currentTruck = testTruckService.getCurrentTruck();

            });
        });
    });
});