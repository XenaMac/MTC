/// <reference path="../../Views/Alerts/Index.cshtml" />
var map;

require([
    "esri/config",
    "esri/map",
    "esri/tasks/GeometryService",
    "esri/layers/FeatureLayer",
    "esri/layers/ArcGISDynamicMapServiceLayer",
    "esri/symbols/PictureMarkerSymbol",
    "esri/toolbars/draw",
    "esri/dijit/editing/Editor",
    "esri/dijit/editing/TemplatePicker",
    "esri/dijit/HomeButton",
    "esri/dijit/BasemapGallery",
    "esri/dijit/Legend",
    "dojo/parser",
    "dojo/dom",
    "dojo/dom-class",
    "dojo/on",
    "dojo/query",
    "dojo/_base/array",
    "dojo/keys",
    "dijit/form/CheckBox",
    "dijit/layout/AccordionContainer",
    "dijit/layout/BorderContainer",
    "dijit/layout/ContentPane",
    "dojo/domReady!"],
    function (
        esriConfig, Map, GeometryService, FeatureLayer, ArcGISDynamicMapServiceLayer, PictureMarkerSymbol, Draw, Editor, TemplatePicker,
        HomeButton, BasemapGallery, Legend, parser, dom, domClass, on, query, arrayUtils, keys, CheckBox
    ) {
    parser.parse();
    var legendLayers = [];

    //esriConfig.defaults.geometryService = new GeometryService("http://tasks.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
    esriConfig.defaults.geometryService = new GeometryService("http://services4.geopowered.com/arcgis/rest/services/LATA/DropSitesProduction/FeatureServer/");

    /* create the original base map and set up functionality */
    map = new Map("map", {
        basemap: "topo",
        //center: [-106.572, 35.101], // for centering on Albuquerque
        center: [-122.000, 37.880],
        zoom: 9,
        showAttribution: false,
        sliderStyle: "small"
    });

    /* Editing Functionality */
    map.on("layers-add-result", initEditing);

    /* Basemap Gallery */
    var basemapGallery = new BasemapGallery({
        showArcGISBasemaps: true,
        map: map,
    }, "basemapGallery");
    basemapGallery.startup();

    basemapGallery.on("error", function (msg) {
        console.log("basemap gallery error: ", msg);
    });

    /* Home Button */
    var home = new HomeButton({
        map: map
    }, "HomeButton");
    home.startup();

    /* Menu Button Functionality */
    var mapGalleryNode = dom.byId("basemapButton");
    on(mapGalleryNode, "click", function (evt) {
        domClass.toggle("basemapHolder", "hidden");
    });

    /* Alerts button functionality */
    var alertNode = dom.byId("alertButton");
    on(alertNode, "click", function (evt) {
        window.location = "Alerts/Index";
    });

    var statusNode = dom.byId("statusButton");
    on(statusNode, "click", function (evt) {
        window.location = "TruckStatus/Index";
    });
/*
    // show the zoom factor
    map.on("zoom", function (event) {
        console.log(event.zoomFactor);
    });
*/

    /* Beats Layer */
    var beatLayer = new FeatureLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegmentsProduction/FeatureServer/1", {
        mode: FeatureLayer.MODE_ONDEMAND,
        showAttribution: false,
        outFields: ["*"]
    });

    var beatCheckBox = new CheckBox({
        name: "beatCheck",
        value: "layerOn",
        checked: true,
        onChange: function () {
            if (beatLayer.visible) {
                beatLayer.hide();
            } else {
                beatLayer.show();
            }
        }
    }, "BeatCheck");

    legendLayers.push({ layer: beatLayer, title: "Beats" });

    /* Segment Layer */
    var segmentLayer = new FeatureLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegmentsProduction/FeatureServer/0", {
        mode: FeatureLayer.MODE_ONDEMAND,
        showAttribution: false,
        outFields: ["*"]
        //outFields: ["BeatSegmentNumber", "BeatSegmentDescription", "LastUpdate", "LastUpdateBy", "Active"]
    });

    var segmentCheckBox = new CheckBox({
        name: "SegmentCheck",
        value: "layerOn",
        checked: true,
        onChange: function () {
            if (segmentLayer.visible) {
                segmentLayer.hide();
            } else {
                segmentLayer.show();
            }
        }
    }, "SegmentCheck");

    legendLayers.push({ layer: segmentLayer, title: "Segments" });

    /* Callbox Layer */
    var callBoxLayer = new FeatureLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegmentsProduction/FeatureServer/0", {
        mode: FeatureLayer.MODE_ONDEMAND,
        showAttribution: false,
        visible: false,
        outFields: ["*"]
    });

    var callBoxCheckBox = new CheckBox({
        name: "CallBoxCheck",
        value: "layerOn",
        checked: false,
        onChange: function () {
            if (callBoxLayer.visible) {
                callBoxLayer.hide();
            } else {
                callBoxLayer.show();
            }
        }
    }, "CallBoxCheck");

    legendLayers.push({ layer: callBoxLayer, title: "Call Boxes" });

    /* Temporary Beat Service Layer */
    var temporaryBeatLayer = new FeatureLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegmentsProduction/FeatureServer/1", {
        mode: FeatureLayer.MODE_SNAPSHOT,
        showAttribution: false,
        visible: false,
        outFields: ["*"]
    });

    var tempBeatCheckBox = new CheckBox({
        name: "tempBeatCheck",
        value: "layerOn",
        checked: false,
        onChange: function () {
            if (temporaryBeatLayer.visible) {
                temporaryBeatLayer.hide();
            } else {
                temporaryBeatLayer.show();
            }
        }
    }, "tempBeatCheck");

    legendLayers.push({ layer: temporaryBeatLayer, title: "Temporary Beat Layer" });

    /* Congestion Layer */
    var congestionLayer = new ArcGISDynamicMapServiceLayer("http://maps.traffic.511.org/ArcGIS/rest/services/Speed_Service_REDGREEN/MapServer", {
        "opacity": 0.5,
        "visible": false,
        "showAttribution": false

    });

    var congestionCheckBox = new CheckBox({
        name: "congestionCheck",
        value: "layerOn",
        checked: false,
        onChange: function () {
            if (congestionLayer.visible) {
                congestionLayer.hide();
            } else {
                congestionLayer.show();
            }
        }
    }, "congestionCheck");

    legendLayers.push({ layer: congestionLayer, title: "Congestion Layer" });

    /* Truck Service Layer */
    // connect up to the truck service
    var truckService = new FeatureLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/TowTruckSitesProduction/FeatureServer/0", {
        mode: FeatureLayer.MODE_ONDEMAND,
        outFields: ["*"],
        showAttribution: false,
        refreshInterval: 0.1
    });

    // setup the service layer checkbox
    var serviceCheckBox = new CheckBox({
        name: "ServiceCheck",
        value: "layerOn",
        checked: true,
        onChange: function () {
            if (truckService.visible) {
                truckService.hide();
            } else {
                truckService.show();
            }
        }
    }, "ServiceCheck");

    legendLayers.push({ layer: truckService, title: "Truck Service" });

    // add in the feature layers
    map.addLayer(truckService);
    map.addLayer(callBoxLayer);
    map.addLayer(congestionLayer);
    map.addLayers([beatLayer, segmentLayer]);

    function initEditing(event) {
        /* legend */
        var legend = new Legend({
            map: map,
            layerInfos: legendLayers
        }, "legendPane");
        legend.startup();

        /* template picker */
        var templateLayers = arrayUtils.map(event.layers, function (result) {
            return result.layer;
        });

        var templatePicker = new TemplatePicker({
            featureLayers: templateLayers,
            grouping: false,
            rows: "auto",
            columns: 3
        }, "templateDiv");
        templatePicker.startup();

        /* Accordion Functionality */
        var accordion = dijit.byId("mainToolset");
        accordion.on("click", function (event) {
            var childPane = accordion.selectedChildWidget;
            if (childPane.id === "editor") {
                templatePicker.update();
            }
        });

        /* editing functionality */
        var featureLayerInfos = arrayUtils.map(event.layers, function (layer) {
            return {
                featureLayer: layer.layer
            };
        });

        var settings = {
            map: map,
            templatePicker: templatePicker,
            layerInfos: featureLayerInfos,
            toolbarVisible: true,
            createOptions: {
                polygonDrawTools: [
                    Editor.CREATE_TOOL_RECTANGLE,
                    Editor.CREATE_TOOL_POLYGON
                ]
            }
        };

        var params = {
            settings: settings
        };

        var editorWidget = new Editor(params, "featureEditorDiv");
        editorWidget.startup();
    }
});