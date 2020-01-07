require([
    "dojo/dom-construct",
    "esri/map",
    "esri/geometry/Point",
    "dojo/_base/window",
    "dojo/on",
    "dojo/dom-class",
    "dojo/query",
    "Custom/Follower",
    "dojo/domReady!"
    ], function (domConstruct, Map, Point, win, on, domClass, query, follower) {
    describe('Follower', function () {
        var testFollower,
            currentTruck,
            testMap,
            mainDiv, 
            div,
            buttonDiv;

        beforeEach(function (done) {
            mainDiv = domConstruct.create('div', { id: "mainDiv" }, win.body());
            div = domConstruct.create('div', { style: {width: "300px", height: "200px" }});
            buttonDiv = domConstruct.create('div', { id: "followButton" }, mainDiv);
            domClass.add(buttonDiv, "hidden");
            currentTruck = {
                truckNumber: "sometruck",
                latitude: -100.000,
                longitude: 37.000
            };
            testMap = new Map(div, {
                basemap: "streets",
                center: [-122.000, 37.880],
                zoom: 9
            });

            testMap.on('load', function () {
                testFollower = new follower({
                    currentTruck: currentTruck,
                    map: testMap
                });
                done();
            },1);
        });

        afterEach(function () {
            testMap.destroy();
            domConstruct.destroy(div);
            domConstruct.destroy(buttonDiv);
            domConstruct.destroy(mainDiv);
            testFollower = null;
        });
            
        describe('when follow link is clicked', function () {
            beforeEach(function (done) {
                testFollower.followClickHandler();
                testFollower.on('followerDone', function () {
                    done();
                });
            }, 1000);

            afterEach(function (done) {
                testFollower.stopFollowingClickHandler();
                testFollower.on('returnDone', function () {
                    done();
                });
            }, 1000);

            it('should enter follow state', function () {
                expect(testFollower).toBeFollowing(true);
            });

            it('should zoom in on the selected trucks extent', function () {
                var zoomLevel = testFollower.map.getZoom();
                expect(zoomLevel).toEqual(18);
            });

            it('should close the infowindow', function () {
                var infoWindowState = testMap.infoWindow.isShowing;
                expect(infoWindowState).toEqual(false);
            });

            it('should activate the stop following button', function () {
                var buttonHidden = domClass.contains(buttonDiv, "hidden");
                expect(buttonHidden).toEqual(false);
            });

            it('truck position should update with new data', function () {
                testFollower.setCurrentPosition(-122.000, 37.880);
                expect(testFollower.getCurrentPosition()).toBe(-122.000, 37.880);
            });

            it('should recenter the map on selected truck position', function () {
                var mapCenter = testMap.extent.getCenter();
                expect(mapCenter).compareExtent(testFollower.currentPosition);
            });

            describe('when Stop Following button is clicked', function () {
                beforeEach(function (done) {
                    testFollower.stopFollowingClickHandler();
                    testFollower.on('returnDone', function () {
                        done();
                    });
                }, 1000);

                afterEach(function (done) {
                    testFollower.followClickHandler();
                    testFollower.on('followerDone', function () {
                        done();
                    });
                }, 1000);

                it('should leave follow state', function () {
                    expect(testFollower).toBeFollowing(false);
                });

                it('should return the zoom level to its original setting', function () {
                    var zoomLevel = testFollower.map.getZoom();
                    expect(zoomLevel).toEqual(9);
                });

                it('should return the view to the original extent', function () {
                    var expectedExtent = testFollower.currentExtent;
                    var currentExtent = testFollower.map.extent;
                    expect(currentExtent).toEqual(expectedExtent);
                });

                it('should deactivate the stop following button', function () {
                    var buttonHidden = domClass.contains(buttonDiv, "hidden");
                    expect(buttonHidden).toEqual(true);
                });
            });

            });
        });
    });
