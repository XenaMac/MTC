// this is the widget to create a new beat
define([
    "dijit/_Widget",
    "dijit/_OnDijitClickMixin",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/Dialog",
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/on",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/parser",
    "dojo/io-query",
    "dojo/request",
    "dojo/when",
    "dojo/text!Custom/Templates/beatCreator_template.html",
    "esri/tasks/query",
    "esri/tasks/QueryTask",
    "esri/tasks/GeometryService",
    "esri/graphic",
    "esri/symbols/SimpleFillSymbol",
    "esri/symbols/SimpleLineSymbol",
    "esri/Color",
    "Custom/segmentSelector",
    "Custom/beatAttributeInspector",
    "dijit/registry",
    "dijit/form/Button",
    "dojo/domReady!"],
    function (_Widget, _OnDijitClickMixin, _TemplatedMixin, _WidgetsInTemplateMixin, Dialog, Evented, declare, lang, arrayUtils, on, dom, domConstruct, domClass, domStyle, parser, ioQuery, request, when, dijitTemplate, Query, QueryTask, GeometryService, Graphic, SimpleFillSymbol, SimpleLineSymbol, Color, segmentSelector, beatAttributeInspector, registry) {
        return declare(Dialog, {
            baseClass: "beatCreator",
            title: "",
            beatPolygon: null,
            segments: null,
            map: null,
            beatLayer:null,
            myBeat: null,
            mySegments: null,
            beatNumber: 0,
            beatDescription: "",
            active: true,
            state: "add",
            selectedSegments: [],
            selectedFreeways: [],
            constructor: function (kwArgs) {
                lang.mixin(this, kwArgs);
                if (kwArgs.beatNumber) {
                    this.beatNumber = kwArgs.beatNumber;
                }
                var template = dijitTemplate;
                var contentWidget = new (declare(
                    [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin],
                        {
                            templateString: template
                        }
                    ));
                contentWidget.startup();
                this.content = contentWidget;
            },
            postCreate: function () {
                this.inherited(arguments);
                this.connect(this.content.cancelButton, "onClick", "onCancel");
                this.connect(this.content.submitButton, "onClick", "createBeat");
            },
            destroy: function() {
                this.destroyRecursive();
            },
            startup: function () {
                this.mySegments = new segmentSelector({

                }, "segmentSelector");

                if (this.beatNumber === 0) {
                    // we are creating a new beat
                    console.log("setting add state");
                    this.state = "add";
                    this.myBeat = new beatAttributeInspector({
                    }, "beatEditor");
                } else {
                    // we are editing a beat
                    console.log("setting edit state");
                    this.mySegments.edit = true;
                    this.mySegments.beatNumber = this.beatNumber;
                    this.state = "edit";
                    this.myBeat = new beatAttributeInspector({
                        beatNumber: this.beatNumber
                    }, "beatEditor");
                }
                this.myBeat.startup();
            },
            onExecute: function() {
                console.log("OK");
            },
            onCancel: function(){
                this.destroyRecursive();
            },
            createBeat: function () {
                // get the information from the form
                this.myBeat.getFormValues();
                //if (this.state === 'add') {
                //    this.beatNumber = this.myBeat.beatNumber;
                //}

                this.beatDescription = this.myBeat.description;
                this.active = this.myBeat.active;
                this.selectedFreeways = this.myBeat.freeways;

                // update the selected segments
                this.selectedSegments = this.mySegments.getMarkedSegments();

                if (this.selectedSegments.length > 0) {
                    this.generateServiceURL();

                    // get the selected segments
                    this._querySegments(this.selectedSegments);
                    this.generateBeatServiceURL();
                    this._closeForm();
                } else {
                    this._closeForm();
                }
            },
            _closeForm: function(){
                this.hide();
                this.destroyRecursive();
            },
            _querySegments: function (segments) {
                // function that takes an array of segments and queries to get the geometries
                var getSegmentID = lang.hitch(this, this._getSegmentID);
                var segmentIDs = [];
                arrayUtils.forEach(segments, function (segment) {
                    segmentIDs.push(getSegmentID(segment.id));
                });

                var queryTask = new QueryTask("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/");
                var query = new Query();
                query.objectIds = segmentIDs;
                query.returnGeometry = true;
                query.outFields = ['*'];
                queryTask.execute(query, lang.hitch(this, this._unionSegments));
            },
            _getSegmentID: function(segment){
                // function that takes the an html string and returns the segment object ID
                var returnID = 0;
                var segmentID = segment.replace(/\D/g, '');
                if (parseInt(segmentID)){
                    returnID = parseInt(segmentID);
                }

                return returnID;
            },
            _getSegmentName: function(segment){
                var returnName = "";
                returnName = segment.replace(/checkBox/g, '');
                return returnName;
            },
            _unionSegments: function (result) {
                // function that takes an array of segment data and unions them together into a single polygon
                var geometryService = new GeometryService("http://38.124.164.214:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer");
                var geometries = [];
                var localSegments = this.mySegments;
                var newBeatNumber = this.beatNumber;
                var newBeatDescription = this.beatDescription;
                var newBeatActive = this.active;
                var newBeatObjectNumber = 0;
                var submitToArcGIS = lang.hitch(this, this._submitBeatToArcGIS);
                arrayUtils.forEach(result.features, function (feature) {
                    geometries.push(feature.geometry);
                });
                geometryService.union(geometries, function (result) {
                    var sfs = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
                        new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                        new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25])
                        );

                    when(localSegments.getBeatObjectID(newBeatNumber), function (beatObjectNumber) {
                        newBeatObjectNumber = beatObjectNumber.features[0].attributes["OBJECTID"];

                        var attributes = {
                            "OBJECTID": newBeatObjectNumber,
                            "BEAT_ID_1": newBeatNumber,
                            "BeatDescript": newBeatDescription,
                            "Active": newBeatActive
                        };
                        var graphic = new Graphic(result, sfs, attributes);

                        // this show the graphic on the map for debugging purposes
                        //map.graphics.add(graphic);

                        submitToArcGIS(graphic);
                    });
                });
            },
            _submitBeatToArcGIS: function (polygon) {
                // function that takes a polygon and creates a new beat object in ArcGIS
                var localBeatLayer = this.beatLayer;
                var polygons = [polygon];
                if (this.state === "add") {
                    console.log("adding polygon");
                    localBeatLayer.applyEdits(polygons, null, null);
                } else if (this.state === "edit") {
                    console.log("editing polygon");
                    localBeatLayer.applyEdits(null, polygons, null);
                }
            },
            _submitBeatToSQL: function (serviceURL) {
                // function that takes beat information and submits it to the SQL services
                console.log(serviceURL);
                request.get(serviceURL, {
                    headers: {
                        "X-Requested-With": null
                    }
                }).then(function(response){
                    console.log(response);
                });
            },
            _submitBeatSegmentsToSQL: function(serviceURL){
                console.log(serviceURL);
                request.get(serviceURL, {
                    headers: {
                        "X-Requested-With": null
                    }
                }).then(function (response) {
                    console.log(response);
                });
            },
            generateServiceURL: function () {
                // takes the information from the beatAttributeInspector and creates a URL
                // to the service
                //var serviceURL = query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/updateBeatsFreeways";
                var serviceURL = "http://38.124.164.213:9017/AJAXFSPService.svc/updateBeatsFreeways";
                var query = {
                    _BeatID: this.myBeat.beatNumber,
                    _BeatDescription: this.myBeat.description,
                    _Active: this.myBeat.active,
                    _Freeways: this.myBeat.freeways.toString()
                };
                var queryString = ioQuery.objectToQuery(query);
                serviceURL = serviceURL + "?" + queryString;
                console.log(serviceURL);
                this._submitBeatToSQL(serviceURL);
                //return serviceURL;
            },
            generateBeatServiceURL: function () {
                // takes the segments that generated the beat and creates a URL to the service
                //var serviceURL = query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/reloadBeatBeatSegments";
                var serviceURL = "http://38.124.164.213:9017/AJAXFSPService.svc/reloadBeatBeatSegments";
                var getSegmentName = lang.hitch(this, this._getSegmentName);
                var segmentNames = [];
                arrayUtils.forEach(this.selectedSegments, function (segment) {
                    console.log(segment);
                    segmentNames.push(getSegmentName(segment.name));
                });

                var query = {
                    beatNumber: this.myBeat.beatNumber,
                    beatSegments: segmentNames.toString()
                };

                var queryString = ioQuery.objectToQuery(query);
                serviceURL = serviceURL + "?" + queryString;

                //console.log(serviceURL);
                this._submitBeatSegmentsToSQL(serviceURL);
            }
         });
    });

// http://38.124.164.213:9017/AJASXFSPService.svc/updateBeatsFreeways (string _BeatID, string _BeatDescription,
// string _Active(1/0 or True/False) and a list of strings containing all the freeways for that beat

//reloadBeatBeatSegments(string beatNumber, List<string> beatSegments) // delete and reload all segment/beat associations in beatbeatsegments
//getBeatNumbers() //return all beat numbers in beats
//getBeatSegmentsByBeat(string beatNumber) //return all beatsegments by beat in beatbeatsegments.
