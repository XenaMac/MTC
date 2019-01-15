define([
    "dojo/request",
    "dojo/json",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/_base/event",
    "dojox/timing",
    "dojo/Evented",
    "dojo/on",
    "dojo/query",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/dom-class",
    "Custom/Truck",
    "esri/layers/GraphicsLayer",
    "dojo/domReady!"],
    function (request, JSON, declare, lang, arrayUtil, event, timing, Evented, on, query, dom, domConstruct, domClass, Truck, GraphicsLayer) {
        return declare([Evented], {
            self: this,
            //serviceAddress: query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/GetAllTrucks",
            serviceAddress: "http://38.124.164.211:9017/AJAXFSPService.svc/GetAllTrucks",
            refreshRate: 1000,
            t: null,
            map: null,
            trucks: [],
            graphicsLayer: null,
            selectedTruckNumber: "",
            filterName: "",
            getTruckData: function () {
                var self = this;
                var graphicsLayer = this.graphicsLayer;
                var filterName = this.filterName;
                var showData = lang.hitch(this, this.showTrucks);
                var filter = lang.hitch(this, this.filter);
                var localMap = this.map;
                request.get(this.serviceAddress, {
                    headers: {
                        // necessary to turn off this header or we can't see the service
                        "X-Requested-With": null
                    },
                    handleAs: "json"
                }).then(
                    function (response) {
                        var myData = JSON.parse(response.d);
                        this.trucks = [];

                        arrayUtil.forEach(myData, function (truck, i) {
                            var currentTruck = new Truck({
                                map: localMap,
                                graphicsLayer : graphicsLayer
                            });
                            currentTruck.truckNumber = truck.TruckNumber;
                            currentTruck.beatNumber = truck.BeatNumber;
                            currentTruck.driverName = truck.DriverName;
                            currentTruck.heading = truck.Heading;
                            currentTruck.latitude = truck.Lat;
                            currentTruck.longitude = truck.Lon;
                            currentTruck.callsign = truck.CallSign;
                            currentTruck.contractorName = truck.ContractorName;
                            currentTruck.speed = truck.Speed;
                            currentTruck.vehicleState = truck.VehicleState;
                            currentTruck.alarms = truck.Alarms;
                            currentTruck.backup = truck.IsBackup;
                            currentTruck.determineState();
                            currentTruck.lastMessage = truck.LastMessage;
                            currentTruck.ipAddress = truck.IPAddress;
                            this.trucks.push(currentTruck);
                        });
                        if (filterName != "") {
                            filter();
                        }
                        showData(this.trucks);
                        self.emit("data-downloaded", {data: this.trucks});
                    },
                    function (error) {
                        console.log(error);
                    });
            },
            filter: function () {
                var localFilter = this.filterName;
                self.trucks = arrayUtil.filter(self.trucks, function (el) {
                    return el.contractorName == localFilter;
                });
            },
            getTruckFromIp: function(truckId){
                var localFilter = truckId;
                var returnArray = arrayUtil.filter(self.trucks, function(el){
                    return el.ipAddress == localFilter;
                });
                return returnArray;
            },
            getTruckList: function(){
                return this.trucks;
            },
            showTrucks: function (truckArray) {
                this.graphicsLayer.clear();
                arrayUtil.forEach(truckArray, function (truck) {
                    truck.plotTruckLocation();
                });
                this.emit("plot", {truckArray});
            },
            startService: function () {
                // this function calls the getTruckData function at the refresh rate
                this.t = new timing.Timer(1000);
                var getData = lang.hitch(this, this.getTruckData);
                this.t.onTick = function () {
                    getData();
                }

                this.t.start();
            },
            stopService: function () {
                this.t.stop();
            },
            constructor: function (args) {
                //declare.safeMixin(this.args);
                this.map = args.map;
                var getTruckNumber = lang.hitch(this, this._getTruckNumber);
                // set up a separate graphics layer for the service
                this.graphicsLayer = new GraphicsLayer();
                on(this.graphicsLayer, "click", getTruckNumber);
                this.map.addLayer(this.graphicsLayer);
            },
            _getTruckNumber: function (evt) {
                // handles when the graphic is clicked
                var infoTemplate = evt.graphic.infoTemplate.content;
                var node = domConstruct.toDom(infoTemplate);
                var truckNumber = query("[id='truckNumber']", node);
                var truckNumberString = truckNumber[0].innerHTML;
                var extractedNumber = truckNumberString.split(">");
                this.selectedTruckNumber = extractedNumber[2];
            },
            getCurrentTruck: function () {
                return this.selectedTruckNumber;
            }
            
        });
    });

/*
     service links being pushed out
     http://octa-dev:9017/AJAXFSPService.svc/GetAllTrucks (In Ajax)
     http://octa-dev:9017/TowTruckService.svc/CurrentTrucks (service reference)
     http://38.124.164.213:9017/EsriService.svc/GetEsriTrucks what Vasu was using
 */
