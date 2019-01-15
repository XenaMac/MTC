// this is the module for creating a segment selector widget
define([
    "dijit/_WidgetBase",
    "dijit/_OnDijitClickMixin",
    "dijit/_TemplatedMixin",
    "dijit/form/CheckBox",
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/on",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/when",
    "dojo/io-query",
    "dojo/request",
    "dojo/json",
    "dojo/query",
    "dijit/registry",
    "esri/tasks/query",
    "esri/tasks/QueryTask",
    "esri/tasks/RelationshipQuery",
    "esri/request",
    "dojo/text!Custom/Templates/segment_template.html",
    "dojo/domReady!"
    ],
    function (_WidgetBase, _OnDijitClickMixin, _TemplatedMixin, CheckBox, Evented, declare, lang, arrayUtils, on, dom, domConstruct, domClass, domStyle, when, ioQuery, request, JSON, query, registry, Query, QueryTask, RelationshipQuery, esriRequest, dijitTemplate) {
        return declare([_WidgetBase, _OnDijitClickMixin, _TemplatedMixin, Evented], {
            templateString: dijitTemplate,
            baseClass: "segmentSelector",
            title: "",
            features: null,
            selectedFeatures: null,
            markedFeatures: null,
            objectID: 0,
            beatNumber: 0,
            edit: false,
            constructor: function(params, srcRefNode){
                this.domNode = srcRefNode;
                this.title = params.title;
                this.features = [];
                this.selectedFeatures = [];
                this._queryFeatures();
            },
            startup: function () {
                this.titleNode.innerHTML = this.title;
            },
            postCreate: function(){
                domStyle.set(this.titleNode, 'display', 'none');
            },
            showFeatures: function() {
                console.log(this.features);
            },
            setSelectedSegments: function () {
                // make sure we have a return from adding features to the page first
                // queries the sql service for the segments in a given beat
                //this.beatNumber = beatNumber;
                var markSegments = lang.hitch(this, this._markSegments);
                var segments = this.selectedFeatures;

                //var serviceURL = query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/getBeatSegmentsByBeat"
                var serviceURL = "http://38.124.164.213:9017/AJAXFSPService.svc/getBeatSegmentsByBeat";
                var stringNumber = this.beatNumber.toString();
                var query = {
                    beatNumber: stringNumber
                };
                var queryString = ioQuery.objectToQuery(query);
                serviceURL = serviceURL + "?" + queryString;
                request.get(serviceURL, {
                    headers: {
                        "X-Requested-With": null
                    }
                }).then(function (response) {
                    var myData = JSON.parse(response);
                    myData = JSON.parse(myData.d);
                    segments = myData;
                    markSegments(segments);
                });
            },
            _onClick: function () {

            },
            _init: function () {

            },
            _markSegments: function (segments) {
                //var verify = lang.hitch(this, this._verifyCheckBox);
                var getSegmentId = lang.hitch(this, this._getSegmentId);
                arrayUtils.forEach(segments, function (segment) {
                    var segmentNumber = segment.beatSegmentNumber;
                    when(getSegmentId(segmentNumber), function (result) {
                        var segmentObjectNumber = result.features[0].attributes["OBJECTID"];
                        var checkboxName = "checkBox" + segmentObjectNumber;
                        var checkbox = dom.byId(checkboxName);
                        if (checkbox) {
                            checkbox.checked = true;
                        }
                    });
                });
            },
            getMarkedSegments: function(){
                var currentFeatures = [];
                arrayUtils.forEach(this.features, function (segment) {
                    // make the checkbox name
                    var checkbox_name = "checkBox" + segment.oID;
                    var current_checkbox = dom.byId(checkbox_name);
                    if (current_checkbox && current_checkbox.checked) {
                        currentFeatures.push(current_checkbox);
                    }
                });

                return currentFeatures;
            },
            _switchServer: function () {
                // change to the map server if we are querying related records
                esriRequest.setRequestPreCallback(function (ioArgs) {
                    if (ioArgs.url.indexOf("queryRelatedRecords") !== -1) {
                        ioArgs.url = ioArgs.url.replace("FeatureServer", "MapServer");
                    }
                    return ioArgs;
                });
            },
            setObjectID: function (result) {
                return result.features[0].attributes["OBJECTID"];
            },
            getBeatObjectID: function(beatNumber){
                var queryTask = new QueryTask("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/0");
                var query = new Query();
                query.returnGeometry = false;
                query.outFields = ["OBJECTID"];
                query.where = "BEAT_ID_1=" + beatNumber;
                var result = queryTask.execute(query, lang.hitch(this, this.setObjectID));
                return result;
            },
            _queryFeatures: function () {
                // query to get all the segments
                var queryTask = new QueryTask("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/1");
                var query = new Query();
                query.returnGeometry = false;
                query.outFields = ["Beatsegmen", "OBJECTID"];
                query.where = "1=1";
                queryTask.execute(query, lang.hitch(this, this._addFeatures));
            },
            _getSegmentId: function(beatSegmentNumber){
                // query to get the object id for a given segment number
                var queryTask = new QueryTask("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/1");
                var query = new Query();
                query.returnGeometry = false;
                query.outFields = ["Beatsegmen", "OBJECTID"];
                query.where = "Beatsegmen = '" + beatSegmentNumber + "'";
                var result = queryTask.execute(query, this.setObjectId);
                return result;
            },
            _addFeatures: function (results) {
                var resultCount = results.features.length;
                var segmentID = "";
                var objectID = "";
                var segmentObject = null;
                for (var i = 0; i < resultCount; i++) {
                    objectID = results.features[i].attributes["OBJECTID"];
                    segmentID = results.features[i].attributes["Beatsegmen"];
                    segmentObject = {
                        oID: objectID,
                        sID: segmentID
                    };
                    this.features.push(segmentObject);
                }
                this.features.sort(this._sortFunction);
                this._addFeaturesToPage();
            },
            _sortFunction: function(a,b){
                if (a.sID < b.sID) {
                    return -1;
                } else if ( b.sID < a.sID) {
                    return 1;
                } else {
                    return 0;
                }
            },
            _clearRegistry: function() {
                var startsWith = function (wholeString, lookFor) {
                    return wholeString.slice(0, lookFor.length) == lookFor;
                };

                var toDestroy = arrayUtils.filter(registry.toArray(),
                    function (w) { return startsWith(w.id, 'checkBox'); });

                arrayUtils.forEach(toDestroy, function (w) { w.destroyRecursive(false); });
            },
            _addFeaturesToPage: function () {
                var segmentSelector = dom.byId("segmentSelector");
                this._clearRegistry();
                var length = this.features.length;
                for (var i = 0; i < length; i++) {
                    var segmentName = this.features[i].sID;
                    var segmentObjectID = this.features[i].oID;
                    var newSegment = domConstruct.create("div", {
                        className: "segmentSelection"
                    }, segmentSelector);

                    var checkBox = new CheckBox({
                        name: "checkBox" + segmentName,
                        id: "checkBox" + segmentObjectID,
                        value: segmentObjectID,
                        checked: false,
                        className: "segmentCheckBox"
                    });

                    checkBox.placeAt(newSegment);

                    var label = domConstruct.create("label", {
                        innerHTML: segmentName,
                        className: "checkBoxLabel",
                        'for': "checkBox" + segmentName
                    }, newSegment);

                }
                if (this.edit) {
                    this.setSelectedSegments();
                }
            }
        });
});
