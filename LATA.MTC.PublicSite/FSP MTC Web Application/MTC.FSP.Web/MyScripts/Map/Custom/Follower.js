define([
    "dojo/request",
    "dojo/_base/declare",
    "dojo/Evented",
    "dojo/query",
    "dojo/dom-class",
    "dojo/dom-construct",
    "dojo/_base/array",
    "esri/geometry/Point",
    "esri/geometry/Extent",
    "esri/SpatialReference",
    "dojo/domReady!"],
    function (request, declare, Evented, query, domClass, domConstruct, arrayUtils, Point, Extent, SpatialReference) {
        return declare([Evented], {
            baseClass: "follower",
            self: this,
            truckNumber: "",
            map: null,
            followState: false,
            truckService: null,
            following: null,
            currentTruck: null,
            currentPosition: null,
            externalCaller: false,
            currentFeature: null,
            currentExtent: null,
            constructor: function (args) {
                this.externalCaller = args.externalCaller;
                if (this.externalCaller) {
                    this.map = args.map;
                    this.truckService = truckService;
                    this.currentTruck = args.currentTruck;
                    this._zoomToTruck();
                } else {
                    this._getTruckNumber(args.currentTruck);
                    this.currentPosition = new Point(args.currentTruck.geometry.x, args.currentTruck.geometry.y);
                    this.map = args.map;
                    this.truckService = truckService;
                    this._getCurrentExtent();
                }
            },
            _getCurrentExtent: function() {
                // gets the current extent of the map so that we can reset
                // it after zooming in
                this.currentExtent = this.map.extent;
            },
            _zoomToExtent: function () {
                self = this;
                var localMap = this.map;
                self.currentFeature = localMap.infoWindow.getSelectedFeature();
                if (self.currentFeature) {
                    self.currentPosition = new Point(self.currentFeature.geometry.x, self.currentFeature.geometry.y);
                } else {
                    this.currentPosition = new Point(-122.000, 37.880, new SpatialReference({ wkid: 102100}));
                }
                localMap.centerAndZoom(self.currentPosition, 18);
                localMap.on("zoom-end", function () {
                    self.emit("followerDone", {});
                });
            },
            _zoomToTruck: function(){
                var localMap = this.map;
                if (this.currentTruck) {
                    var position = new Point(this.currentTruck.longitude, this.currentTruck.latitude);
                    localMap.centerAndZoom(position, 18);
                }
            },
            _getTruckNumber: function(inputWindow){
                var infoTemplate = inputWindow.infoTemplate.content;
                var node = domConstruct.toDom(infoTemplate);
                var truckNumber = query("[id='truckNumber']", node);
                var truckNumberString = truckNumber[0].innerHTML;
                var extractedNumber = truckNumberString.split(">");
                self.truckNumber = extractedNumber[2];
            },
            _closeInfoWindow: function() {
                this.map.setInfoWindowOnClick(false);
            },
            _openInfoWindow: function(){
                this.map.setInfoWindowOnClick(true);
            },
            _showStopFollowingButton: function () {
                query('#followButton').removeClass("hidden");
            },
            _hideStopFollowingButton: function() {
                query('#followButton').addClass("hidden");
            },
            _returnToOriginalExtent: function () {
                var self = this;
                this.map.setExtent(this.currentExtent);
                this.map.on("zoom-end", function () {
                    self.emit("returnDone", {});
                });
            },
            _startFollowing: function () {
                this.followState = true;
                this._closeInfoWindow();
                this._showStopFollowingButton();
                this._zoomToExtent();
                this.following = this.truckService.on("plot", this.recenter);
            },
            _stopFollowing: function() {
                this.followState = false;
                this._openInfoWindow();
                this._hideStopFollowingButton();
                this._returnToOriginalExtent();
                this.following.remove();
            },
            getFollowState: function () {
                return this.followState;
            },
            getCurrentPosition: function () {
                return this.currentPosition.y, this.currentPosition.x;
            },
            setCurrentPosition: function (longitude, latitude) {
                this.currentPosition = new Point(longitude, latitude);
            },
            getPositionUpdate: function () {
                // should get the data for the current truck
            },
            recenter: function (evt) {
                var localMap = this.map;
                var truck = arrayUtils.filter(evt.truckArray, function (truck) {
                    return truck.truckNumber == this.truckNumber;
                });
                var currentTruck = truck[0];
                self.currentPosition = new Point(currentTruck.longitude, currentTruck.latitude);
                localMap.centerAndZoom(self.currentPosition, 18);
            },
            followClickHandler: function () {
                this._startFollowing();
            },
            stopFollowingClickHandler: function () {
                self._stopFollowing();
            }
        });
    });