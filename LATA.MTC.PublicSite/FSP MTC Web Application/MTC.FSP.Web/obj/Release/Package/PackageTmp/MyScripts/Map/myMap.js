var map;
var dropZoneLayer;
var updateFeature;
var layers = [];
var displayLayers = [];
var addedLayerName = "";
var infoLayers = [];
var featureLayers = [];
var beatLayer = null;
var loginInfo = null;
var userState = "full";
var truckService = null;
var contractorName = "";
var localUserId;

//DEV vars for conectivity
var ESRIBeatandSegmentURL = "http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/";
var ESRICallBoxUrl = "http://services4.geopowered.com/arcgis/rest/services/LATA/Callbox/FeatureServer/";
var ESRIDropSites = "http://services4.geopowered.com/arcgis/rest/services/LATA/DropSites2015/FeatureServer/";
var ESRITowTruckSites = "http://services4.geopowered.com/arcgis/rest/services/LATA/TowTruckSites/FeatureServer/";

//PROD vars for conectivity
//var ESRIBeatandSegmentURL = "http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegmentsProduction/FeatureServer/";
//var ESRICallBoxUrl = "http://services4.geopowered.com/arcgis/rest/services/LATA/CallboxProduction/FeatureServer/";
//var ESRIDropSites = "http://services4.geopowered.com/arcgis/rest/services/LATA/DropSitesProduction/FeatureServer/";
//var ESRITowTruckSites = "http://services4.geopowered.com/arcgis/rest/services/LATA/TowTruckSitesProduction/FeatureServer/";

require([
    "esri/map",
    "esri/dijit/Popup",
    "esri/dijit/PopupTemplate",
    "esri/Color",
    "esri/symbols/SimpleFillSymbol",
    "esri/symbols/SimpleLineSymbol",
    "esri/renderers/SimpleRenderer",
    "esri/tasks/GeometryService",
    "esri/tasks/query",
    "esri/tasks/ProjectParameters",
    "esri/geometry/Extent",
    "esri/layers/ArcGISDynamicMapServiceLayer",
    "esri/layers/FeatureLayer",
    "esri/layers/LayerDrawingOptions",
    "esri/SpatialReference",
    "esri/toolbars/draw",
    "esri/dijit/AttributeInspector",
    "esri/dijit/editing/Editor",
    "esri/dijit/HomeButton",
    "esri/dijit/BasemapGallery",
    "esri/config",
    "dojo/_base/array",
    "dojo/_base/lang",
    "dojo/parser",
    "dojo/query",
    "dojo/dom",
    "dojo/dom-class",
    "dojo/dom-construct",
    "dojo/dom-style",
    "dojo/on",
    "dijit/form/CheckBox",
    "dijit/form/Button",
    "dijit/Dialog",
    "dojo/request",
    "dojo/domReady!"],
    function (
        Map, Popup, PopupTemplate, Color, SimpleFillSymbol, SimpleLineSymbol, SimpleRenderer, GeometryService, Query, ProjectParameters, Extent, ArcGISDynamicMapServiceLayer, FeatureLayer, LayerDrawingOptions, SpatialReference, Draw, AttributeInspector, Editor,
        HomeButton, BasemapGallery, esriConfig, arrayUtils, lang, parser, query, dom, domClass, domConstruct, domStyle, on, CheckBox, Button, Dialog, request
    ) {
        // parse the form to get all the widgets working
        parser.parse();

        request.get(query(".websiteUrl")[0].innerHTML.trim() + "/Home/GetCurrentUser", {
            headers: {
                "X-Requested-With": null
            },
            handleAs: "json"
        }).then(
            function (response) {
                var hideFields = [];
                if (response.SelectedRoleName === "TowContractor") {
                    var contractorId = response.ContractorId;
                    var contractorType = response.ContractorTypeName;
                    request.get(query(".websiteUrl")[0].innerHTML.trim() + "/Home/GetContractors?=" + contractorType, {
                        headers: {
                            "X-Requested-With": null
                        },
                        handleAs: "json"
                    }).then(
                        function (response) {
                            contractorName = arrayUtils.filter(response, function (item) {
                                return item.Id == contractorId;
                            })[0].Text;

                            userState = "partial";
                            hideFields = [
                                "editButton",
                                "createButton",
                                "beatButton",
                                "AGOButton",
                                "globalAddButton",
                                "alertButton",
                                "statusButton",
                                "shiftLogButton"
                            ];

                            arrayUtils.forEach(hideFields, function (field) {
                                var button = dom.byId(field);
                                domStyle.set(button, 'display', 'none');
                            });

                            if (this.truckService) {
                                this.truckService.filterName = contractorName;
                            }
                        });
                } else if (response.SelectedRoleName === "CHPOfficer" ||
                    response.SelectedRoleName === "DataConsultant" ||
                    response.SelectedRoleName === "FSPPartner" ||
                    response.SelectedRoleName === "GeneralUser" ||
                    response.SelectedRoleName === "Guest" ||
                    response.SelectedRoleName === "InVehicleContractor" ||
                    response.SelectedRoleName === "MTC") {
                    userState = "full";
                    hideFields = [
                        "editButton",
                        "createButton",
                        "beatButton"
                    ];

                    arrayUtils.forEach(hideFields, function (field) {
                        var button = dom.byId(field);
                        domStyle.set(button, 'display', 'none');
                    });

                }

                /* ZOOM REQUEST
                    handle zoom/truck select request from "Truck Status" page
                    make sure that this only happens for the user issueing the request.
                */

                $(function () {
                    $.get($(".websiteUrl").text().trim() + '/Home/GetCurrentUser',
                           function (value) {
                               console.group("Get Current User");
                               console.log(value);
                               console.log('Map Page. Current User ' + value.UserName);
                               var localUserName = value.UserName;
                               var mtcHub = $.connection.mtcHub;

                               mtcHub.client.setSelectedTruck = function (truckId, userId) {
                                   console.group("Zoom Request");
                                   console.log('Map Page. Truck selected: ' + truckId + ' by user ' + userId);

                                   if (userId === localUserName) {
                                       //only execute if requesting user is the same. In other words, don't center truck for other users.
                                       console.log('Map Page. Center on truck %s', truckId);

                                       //CLAY. INSERT A LINE OF CODE HERE THAT EXECUTES YOUR LOCAL "CENTERING" METHOD.
                                       zoomTo(truckId);
                                   }
                                   console.groupEnd("Zoom Request");
                               };
                               console.groupEnd("Get Current User");
                               $.connection.hub.start().done(function () {
                                   console.log('Map Page. hub started..');
                               });
                           }, "json");
                });


                esriConfig.defaults.geometryService = new GeometryService("http://38.124.164.214:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer");

                // add the popup for clicking on features on the map
                var fill = new SimpleFillSymbol("null", null, new Color("#000000"));
                var popup = new Popup({
                    fillSymbol: fill,
                    titleInBody: false,
                    highlight: false
                }, domConstruct.create("div"));
                domClass.add(popup.domNode, "light");

                /* create the original base map and set up functionality */
                map = new Map("map", {
                    basemap: "streets",
                    //center: [-106.572, 35.101], // for centering on Albuquerque
                    center: [-122.000, 37.880], // for centering on the bay area
                    zoom: 9,
                    smartNavigation: false,
                    infoWindow: popup
                });
                map.on("load", loadLayers);

                // setup the TOC and the edit windows in the accordion
                map.on("layers-add-result", setLayers);
                map.on("layers-add-result", setupAccordion);

                popup.on("SelectionChange", function () {
                    if (userState == "full") {
                        removeLinks();
                        addLinks();
                    }
                });

                /* Basemap Gallery */
                var basemapGallery = new BasemapGallery({
                    showArcGISBasemaps: true,
                    map: map,
                }, "basemapGallery");
                basemapGallery.startup();

                basemapGallery.on("error", function (msg) {
                    console.log("basemap gallery error: ", msg);
                });

                map.on("click", function () {
                    var basemapNode = dom.byId("basemapHolder");
                    domClass.add(basemapNode, "hidden")
                });

                /* Home Button */
                var home = new HomeButton({
                    map: map
                }, "HomeButton");
                home.startup();

                /* Basemap Button functionality */
                var mapGalleryNode = dom.byId("basemapButton");
                on(mapGalleryNode, "click", function (evt) {
                    domClass.toggle("basemapHolder", "hidden");
                });

                /* Alerts button functionality */
                var alertNode = dom.byId("alertButton");
                on(alertNode, "click", function (evt) {
                    window.open(query(".websiteUrl")[0].innerHTML.trim() + "/Alerts/Index");
                });

                /* Truck Status button functionality */
                var statusNode = dom.byId("statusButton");
                on(statusNode, "click", function (evt) {
                    window.open(query(".websiteUrl")[0].innerHTML.trim() + "/TruckStatus/Index");
                });

                /* Add Feature button functionality */
                var addButtonNode = dom.byId("toolsContainer");
                on(addButtonNode, "click", setAddState);

                /* Truck Legend Button functionality */
                var truckLegendNode = dom.byId("TruckLegend");
                on(truckLegendNode, "click", function (evt) {
                    require(["dijit/Dialog", "dojo/domReady!"], function (Dialog) {
                        var myDialog = new Dialog({
                            class: 'nonModal',
                            id: 'truckLegendPopup',
                            title: 'Tow Truck Legend',
                            onHide: function () { dijit.byId('truckLegendPopup').destroy(); }
                        });

                        myDialog.show();
                    });
                });

                /* Congestion Legend Button functionality */
                var congestionNode = dom.byId("CongestionLegend");
                on(congestionNode, "click", function (evt) {
                    require(["dijit/Dialog", "dojo/domReady!"], function (Dialog) {
                        var myDialog = new Dialog({
                            class: 'nonModal',
                            id: 'congestionLegendPopup',
                            title: 'Congestion Legend',
                            onHide: function () { dijit.byId('congestionLegendPopup').destroy(); }
                        });

                        myDialog.show();
                    });
                });

                /* ArcGIS Online Button functionality */
                var agoNode = dom.byId("AGOButton");
                on(agoNode, "click", function (evt) {
                    window.open("http://octafps.maps.arcgis.com/home/webmap/viewer.html?webmap=21d83aeabb214cb5b4df64f2b369c933");
                });

                /* Bookmarks button functionality */
                //var bookmarksNode = dom.byId("bookmarks");
                //var bookmark;
                //on(bookmarksNode, "click", function (evt) {
                //    require(["esri/dijit/Bookmarks"], function (Bookmarks) {
                //        bookmark = new Bookmarks({
                //            map: map,
                //            bookmarks: [],
                //            editable: true
                //        }, dojo.byId('bookmarks'));

                //    });
                //});

                /* Add Layer button functionality */
                function addLayerFromURL(inputURL, layerName) {
                    var addedLayer = new ArcGISDynamicMapServiceLayer(inputURL, {
                        "opacity": 0.5,
                        "visible": true,
                        "showAttribution": false
                    });
                    addedLayerName = layerName;
                    displayLayers.push(addedLayer);
                    map.addLayer(addedLayer);
                }

                var addButton = dom.byId("globalAddButton");
                on(addButton, "click", function (evt) {
                    require(["dijit/Dialog", "dijit/form/Form", "dijit/form/TextBox", "dijit/form/Button", "dojo/domReady!"],
                        function (Dialog, Form, TextBox, Button) {
                            var form = new Form();

                            var newLabel = new TextBox({
                                placeHolder: "Layer Name",
                                id: 'layerName'
                            }).placeAt(form.containerNode);

                            var newURL = new TextBox({
                                placeHolder: "Service URL",
                                id: 'serviceURL'
                            }).placeAt(form.containerNode);

                            new Button({
                                label: "OK",
                                onClick: function () {
                                    var inputURL = "";
                                    var inputName = "";

                                    inputName = newLabel.get("value");
                                    inputURL = newURL.get("value");

                                    // make sure it is valid

                                    addLayerFromURL(inputURL, inputName);
                                    dijit.byId('addLayerDialog').destroyRecursive();
                                }
                            }).placeAt(form.containerNode);

                            new Button({
                                label: "Cancel",
                                onClick: function () {
                                    dijit.byId('addLayerDialog').destroyRecursive();
                                }
                            }).placeAt(form.containerNode);

                            var myDialog = new Dialog({
                                content: form,
                                class: 'nonModal',
                                id: 'addLayerDialog',
                                title: 'Add Layer From Server',
                                onHide: function () { dijit.byId('addLayerDialog').destroyRecursive(); }
                            });
                            form.startup();
                            myDialog.show();
                        });
                });
            });

        function loadLayers(evt) {
            var dropZoneTemplate = new PopupTemplate({
                title: "Drop Site {Name}",
                fieldInfos: [{
                    fieldName: "Beat",
                    label: "Beat",
                    visible: true
                },
                {
                    fieldName: "Freeway",
                    label: "Freeway",
                    visible: true
                },
                {
                    fieldName: "FULLNAME",
                    label: "Cross Street",
                    visible: true
                },
                {
                    fieldName: "DPSDSC",
                    label: "Description",
                    visible: true
                },
                {
                    fieldName: "last_edited_user",
                    label: "Edited By",
                    visible: true
                },
                {
                    fieldName: "last_edited_date",
                    label: "Date Edited",
                    visible: true
                }
                ]
            });

            dropZoneLayer = new FeatureLayer(ESRIDropSites + "0", {
                mode: FeatureLayer.MODE_ONDEMAND,
                showAttribution: false,
                outFields: ["*"],
                visible: false,
                infoTemplate: dropZoneTemplate
            });

            var dropZoneRenderer = new SimpleRenderer(
            new SimpleFillSymbol(
                SimpleFillSymbol.STYLE_SOLID,
                    new SimpleLineSymbol(
                    SimpleLineSymbol.STYLE_SOLID,
                    new Color([0, 200, 0]), 2),
                    new Color([0, 255, 0, 0.2])
                ));
            dropZoneLayer.setRenderer(dropZoneRenderer);
            infoLayers.push({
                'featureLayer': dropZoneLayer,
                'showAttachments': false,
                'isEditable': true,
                'showDeleteButton': false,
                'fieldInfos': [
                    { 'fieldName': 'OBJECTID', 'isEditable': true, 'tooltip': 'This is the name of the drop zone.', 'label': 'Drop Zone Name' },
                    { 'fieldName': 'Name', 'isEditable': true, 'tooltip': 'This is a description of the drop zone', 'label': 'Drop Zone Description' }
                ]
            });

            /* Beats Layer */
            var beatTemplate = new PopupTemplate({
                title: "Beat {BEAT_ID_1}",
                fieldInfos: [{
                    fieldName: "BeatDescript",
                    label: "Beat Description",
                    visible: true
                }]
            });

            beatLayer = new FeatureLayer(ESRIBeatandSegmentURL + "0", {
                mode: FeatureLayer.MODE_ONDEMAND,
                showAttribution: false,
                outFields: ["*"],
                visible: false,
                infoTemplate: beatTemplate
            });

            infoLayers.push({
                'featureLayer': beatLayer,
                'showDeleteButton': false,
                'fieldInfos': [
                    { 'fieldName': 'BEAT_ID_1', 'isEditable': true, 'tooltip': 'This is the name the beat is known by.', 'label': 'Beat ID' },
                    { 'fieldName': 'BeatDescript', 'isEditable': true, 'tooltip': 'This is a physical description of the beat.', 'label': 'Beat Description' },
                    { 'fieldName': 'Active', 'isEditable': true, 'tooltip': 'Set a value of 1 for active or 0 for inactive.', 'label': 'Active' }
                ]
            });

            var beatSelectionSymbol =
                    new SimpleFillSymbol(
                        SimpleFillSymbol.STYLE_SOLID,
                        new SimpleLineSymbol(
                            SimpleLineSymbol.STYLE_SOLID,
                            new Color([255, 0, 0]), 2),
                            new Color([255, 0, 0, 0.2])
                );
            beatLayer.setSelectionSymbol(beatSelectionSymbol);

            var beatRenderer = new SimpleRenderer(
                new SimpleFillSymbol(
                SimpleFillSymbol.STYLE_SOLID,
                    new SimpleLineSymbol(
                    SimpleLineSymbol.STYLE_SOLID,
                    new Color([0, 0, 0]), 1),
                    new Color([0, 0, 255, 0.2])
            ));
            beatLayer.setRenderer(beatRenderer);
            map.on("click", function (evt) {
                query(".feature_active").removeClass("feature_active");
                beatLayer.clearSelection();
            });

            /* Segment Layer */
            var segmentTemplate = new PopupTemplate({
                title: "Segment {Beatsegmen}",
                fieldInfos: [{
                    fieldName: "BeatSegeme",
                    label: "Segment Description",
                    visible: true
                }]
            });

            var segmentLayer = new FeatureLayer(ESRIBeatandSegmentURL + "1", {
                mode: FeatureLayer.MODE_ONDEMAND,
                outFields: ["*"],
                visible: false,
                infoTemplate: segmentTemplate
            });

            var segmentRenderer = new SimpleRenderer(
                new SimpleFillSymbol(
                    SimpleFillSymbol.STYLE_SOLID,
                    new SimpleLineSymbol(
                        SimpleLineSymbol.STYLE_SOLID,
                        new Color([0, 0, 0]), 2),
                        new Color([0, 0, 0, 0.2])
            ));
            segmentLayer.setRenderer(segmentRenderer);

            var segmentSelectionSymbol = new SimpleFillSymbol(
                SimpleFillSymbol.STYLE_SOLID,
                new SimpleLineSymbol(
                    SimpleLineSymbol.STYLE_SOLID,
                    new Color([255, 0, 0]), 3),
                    new Color([255, 0, 0, 0.5])
            );
            segmentLayer.setSelectionSymbol(segmentSelectionSymbol);
            map.on("click", function (evt) {
                query(".feature_active").removeClass("feature_active");
                segmentLayer.clearSelection();
            });

            infoLayers.push({
                'featureLayer': segmentLayer,
                'showAttachments': false,
                'isEditable': true,
                'showDeleteButton': false,
                'fieldInfos': [
                    { 'fieldName': 'SegmentID', 'isEditable': true, 'tooltip': 'This is the name the segment is known by.', 'label': 'Segment ID' },
                    { 'fieldName': 'SegmentDescription', 'isEditable': true, 'tooltip': 'This is a physical description of the segment', 'label': 'Segment Description' },
                    { 'fieldName': 'Active', 'isEditable': true, 'tooltip': 'Set a value of 1 for active or 0 for inactive.', 'label': 'Active' }
                ]
            });

            /* Tow Truck Yard Layer */
            var towTruckYardTemplate = new PopupTemplate({
                title: "Truck Yard : {Contract_1}",
                description: "{Address_1} <br />{City_1_1}<br />{Zip_1}<br />{Phone1}"
            });

            var TruckYardLayer = new FeatureLayer(ESRITowTruckSites+"0", {
                mode: FeatureLayer.MODE_ONDEMAND,
                showAttribution: false,
                outFields: ["*"],
                infoTemplate: towTruckYardTemplate,
                visible: false
            });

            var truckYardRenderer = new SimpleRenderer(
                new SimpleFillSymbol(
                    SimpleFillSymbol.STYLE_SOLID,
                    new SimpleLineSymbol(
                        SimpleLineSymbol.STYLE_SOLID,
                        new Color([132, 0, 168]), 2),
                    new Color([0, 0, 0, 0.1])
            ));

            TruckYardLayer.setRenderer(truckYardRenderer);
            infoLayers.push({
                'featureLayer': TruckYardLayer,
                'showAttachments': false,
                'isEditable': true,
                'showDeleteButton': false,
                'fieldInfos': [
                    { 'fieldName': 'ID_1', 'isEditable': true, 'tooltip': 'This is the name of the Tow Truck Yard.', 'label': 'Tow Truck Yard Name' },
                    { 'fieldName': 'Contract_1', 'isEditable': true, 'tooltip': 'This is the contract for the Tow Truck Yard.', 'label': 'Tow Truck Yard Contract' },
                    { 'fieldName': 'Address_1', 'isEditable': true, 'tooltip': 'This is the address for the Tow Truck Yard', 'label': 'Address' },
                    { 'fieldName': 'City_1_1', 'isEditable': true, 'tooltip': 'This is the city where the tow truck yard is located.', 'label': 'City' },
                    { 'fieldName': 'Zip_1', 'isEditable': true, 'tooltip': 'This is the zip code where the tow truck yard is located.', 'label': 'Zip' },
                    { 'fieldName': 'Contacts', 'isEditable': true, 'tooltip': 'This is the contact information for the tow truck yard.', 'label': 'Contact Information' }
                ]
            });

            /* Callbox Layer */
            var callboxTemplate = new PopupTemplate({
                title: "Call Box : {SignNumber}",
                fieldInfos: [{
                    fieldName: "Location",
                    label: "Location",
                    visible: true
                }, {
                    fieldName: "Notes",
                    label: "Notes",
                    visible: true
                }]
            });

            var callBoxLayer = new FeatureLayer(ESRICallBoxUrl+"0", {
                mode: FeatureLayer.MODE_ONDEMAND,
                showAttribution: false,
                visible: false,
                outFields: ["*"],
                infoTemplate: callboxTemplate
            });
            displayLayers.push(callBoxLayer);

            /* Congestion Layer */
            var congestionLayer = new ArcGISDynamicMapServiceLayer("http://maps.traffic.511.org/ArcGIS/rest/services/Speed_Service_REDGREEN/MapServer", {
                "opacity": 0.5,
                "visible": true,
                "showAttribution": false
            });
            displayLayers.push(congestionLayer);

            /* truck service */
            require(["Custom/TruckService"],
                function (TruckService) {
                    var myTruckService = new TruckService({ map: this.map });
                    this.truckService = myTruckService;
                    myTruckService.filterName = contractorName;
                    myTruckService.startService();
                    //setTimeout(function () { myTruckService.stopService(); }, 3000); // leave in for debugging

                    /* shift log button functionality */
                    var shiftLogNode = dom.byId("shiftLogButton");
                    on(shiftLogNode, "click", function (evt) {
                        var truckNumber = myTruckService.getCurrentTruck();
                        if (truckNumber !== "") {
                            window.open(query(".websiteUrl")[0].innerHTML.trim() + "/ShiftLog/Index?truckNumber=" + truckNumber);
                        } else {
                            alert("Please select a truck.");
                        }
                    });
                });

            // add in the feature layers
            map.addLayer(callBoxLayer);
            map.addLayer(congestionLayer);
            map.addLayers([dropZoneLayer, segmentLayer, TruckYardLayer, beatLayer]);

            // this is done so that the map will reset after loading the layers
            map.resize();
        }

        /* Following Truck functions */
        function addLinks() {
            require(["Custom/Follower"],
                function (follower) {
                    var localMap = this.map;
                    var featureType = query("#truckInformation", window.map.infoWindow.domNode[0]);
                    if (featureType.length > 0) {
                        var followLink = domConstruct.create("a", {
                            "class": "action",
                            "id": "followLink",
                            "innerHTML": "Follow",
                            "href": "javascript: void(0);"
                        }, query(".actionList", window.map.infoWindow.domNode)[0]);

                        var currentTruck = localMap.infoWindow.getSelectedFeature();

                        var myFollower = new follower({
                            map: localMap,
                            currentTruck: currentTruck,
                            truckService: this.truckService
                        });

                        on(followLink, "click", function () {
                            myFollower.followClickHandler();
                        });

                        var stopButton = query("#followButton");
                        on(stopButton, "click", myFollower.stopFollowingClickHandler);
                    }
                });
        }

        function removeLinks() {
            var followLink = dom.byId("followLink");
            domConstruct.destroy(followLink);
        }

        function zoomTo(truckId) {
            require(["Custom/Follower"],
                function (follower) {
                    var localMap = this.map;
                    var currentTruck = this.truckService.getTruckFromIp(truckId)[0];
                    var myFollower = new follower({
                        externalCaller: true,
                        map: localMap,
                        currentTruck: currentTruck,
                        truckService: this.truckService
                    });
                });
        };

        /* setup functions */
        function setLayers(event) {
            require(["dojo/_base/array", "dojo/domReady!"],
                function (arrayUtils) {
                    featureLayers = arrayUtils.map(event.layers, function (result) {
                        return result.layer;
                    });
                });
        }

        function getVisibleLayers(inputLayers) {
            // returns an array of visible layers
            var returnLayers = [];
            require(["dojo/_base/array"],
                function (arrayUtils) {
                    arrayUtils.forEach(inputLayers, function (layer) {
                        if (layer.visible) {
                            returnLayers.push(layer);
                        }
                    });
                });
            return returnLayers;
        }

        function getVisibleInfos(inputLayers) {
            var returnObject
            require(["dojo/_base/array"],
                function (arrayUtils) {
                    var mapped = arrayUtils.map(inputLayers, function (layer) {
                        return {
                            "featureLayer": layer
                        };
                    });

                    // get rid of the beats layer if it is in there
                    mapped = arrayUtils.filter(mapped, function (layer) {
                        return layer.featureLayer.name != "Beats";
                    });

                    returnObject = arrayUtils.filter(mapped, function (layer) {
                        return layer.featureLayer.visible == true;
                    });
                });
            return returnObject;
        }

        function removeLayers() {
            var filtered = arrayUtils.filter(featureLayers, function (layer) {
                return (layer.name != "Beats") && (layer.name != "Segments");
            });

            return filtered;
        }

        function setupTOC() {
            var localLayers = featureLayers;
            if (userState === "partial") {
                localLayers = removeLayers();
            }
            require(["Custom/TOC", "dijit/registry", "dojo/domReady!"],
                function (TOC, registry) {
                    var widget = registry.byId("TableOfContents");
                    if (!widget) {
                        var tableOfContents = new TOC({
                            map: this.map,
                            layers: localLayers,
                            displayLayers: displayLayers,
                            visible: true
                        }, "TableOfContents");

                        tableOfContents.startup();
                        this.map.on("layer-add", function () {
                            tableOfContents.addedLayerName = addedLayerName;
                            tableOfContents.displayLayers = displayLayers;
                            tableOfContents.refresh();
                        });
                    }
                });
        }

        function displayPopupContent(feature) {
            require(["dijit/registry"], function (registry) {
                console.log("displaying feature", feature);
                if (feature) {
                    var content = feature.getContent();
                    registry.byId("attributeInspectorDiv").set("content", content);
                }
            });
        }

        function initializeSidebar(map) {
            require(["dojo/on", "dojo/_base/connect", "esri/dijit/Popup", "dojo/domReady!"], function (on, connect, Popup) {
                var popup = map.infoWindow;
                connect.connect(popup, "onSelectionChange", function () {
                    displayPopupContent(popup.getSelectedFeature());
                });
            });
        }

        function createNewBeat() {
            require(["Custom/beatCreator", "dojo/domReady!"], function (beatCreator) {
                var myBeatCreator = new beatCreator({
                    title: "Beat Creator",
                    class: "nonModal",
                    beatLayer: beatLayer,
                    map: map
                });
                myBeatCreator.show();
            });
        }

        function editBeat() {
            require(["Custom/beatSelector", "dojo/domReady!"], function (beatSelector) {
                var myBeatSelector = new beatSelector({
                    title: "Select a Beat to Edit",
                    class: "nonModal",
                    beatLayer: beatLayer
                });
                myBeatSelector.show();
            });
        }

        function setupBeatPanel() {
            //var createBeatNode = dom.byId("createBeatsButton");
            //on(createBeatNode, "click", createNewBeat);

            var editBeatNode = dom.byId("editBeatsButton");
            on(editBeatNode, "click", editBeat);
        }

        function setupCreatePanel() {
            require(["esri/dijit/editing/TemplatePicker", "esri/dijit/editing/Editor", "dijit/registry"],
                function (TemplatePicker, Editor, registry) {
                    var visibleFeatureInfos = getVisibleInfos(featureLayers);

                    var widget = registry.byId("templatePickerDiv");
                    if (widget) {
                        widget.destroy();

                        var parentNode = dom.byId("containerDiv");
                        domConstruct.create("div", {
                            id: "templatePcikerDiv"
                        }, parentNode);
                    }

                    var settings = {
                        map: map,
                        layerInfos: visibleFeatureInfos
                    };

                    var params = {
                        settings: settings
                    };

                    var createWidget = new Editor(params, "templatePickerDiv");
                    createWidget.startup();

                    on(map, "update-end", function () {
                        console.log("creation complete");
                        updateService();
                    });
                });
        }

        function updateService() {
            var address = query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/AJAXFSPService.svc/reload";
            var services = [
                "Beats",
                "BeatSegments",
                "Yards",
                "Drops"
            ];
            arrayUtils.forEach(services, function (service) {
                var serviceAddress = address + service;
                console.log(serviceAddress);
                request.get(serviceAddress, {
                    headers: {
                        "X-Requested-With": null
                    },
                    handleAs: "json"
                });
            });
        }

        function setupEditPanel() {
            require(["esri/dijit/AttributeInspector", "esri/dijit/editing/Editor", "dijit/registry", "dojo/dom", "dojo/dom-construct", "dojo/domReady!"],
                function (AttributeInspector, Editor, registry, dom, domConstruct) {
                    var visibleFeatureInfos = getVisibleInfos(featureLayers);

                    var widget = registry.byId("templatePickerDiv");
                    // destory the widget if it exists
                    if (widget) {
                        widget.destroy();

                        // put back in the dom node
                        var parentNode = dom.byId("containerDiv");
                        domConstruct.create("div", {
                            id: "templatePickerDiv"
                        }, parentNode);
                    }

                    // add in the editor
                    var settings = {
                        map: map,
                        layerInfos: visibleFeatureInfos
                    };

                    var params = {
                        settings: settings
                    };

                    var editorWidget = new Editor(params, "templatePickerDiv");
                    editorWidget.startup();

                    on(map, "update-end", function () {
                        console.log("update complete");
                        updateService();
                    });

                });
        }

        var drawToolbar;
        function setAddState() {
            require(["esri/toolbars/draw"],
                function (Draw) {
                    drawToolbar = new Draw(map);
                    drawToolbar.on("draw-end", addGraphic);
                    map.disableMapNavigation();
                    drawToolbar.activate(Draw.POLYGON);
                });
        }

        function addGraphic(evt) {
            var newGraphic;
            var newFeature;
            require(["esri/symbols/SimpleFillSymbol", "esri/symbols/SimpleLineSymbol", "esri/graphic"], function (SimpleFillSymbol, SimpleLineSymbol, Graphic) {
                var fillSymbol = new SimpleFillSymbol(
                    SimpleFillSymbol.STYLE_SOLID,
                    new SimpleLineSymbol(
                        SimpleLineSymbol.STYLE_SOLID,
                        new Color([255, 0, 0]), 2),
                    new Color([0, 0, 0, 0.1])
                );
                newGraphic = new Graphic(evt.geometry, fillSymbol);
                map.graphics.add(newGraphic);
            });
            dropZoneLayer.applyEdits([newGraphic], null, null);
            drawToolbar.deactivate();
            map.enableMapNavigation();
        }

        function setupAccordion(event) {
            require(["Custom/StateController"], function (stateController) {
                /* Accordion Functionality */
                var accordion = dijit.byId("mainToolset");
                var mapController = new stateController(map);
                setupTOC();
                map.setInfoWindowOnClick(true);

                /* Contents Button functionality */
                var contentNode = dom.byId("contentsButton");
                on(contentNode, "click", function (evt) {
                    accordion.selectChild("contents");
                    mapController.changeState("contents");
                    setupTOC();
                });

                /* Create Button functionality */
                var createNode = dom.byId("createButton");
                on(createNode, "click", function (evt) {
                    accordion.selectChild("editor");
                    mapController.changeState("create");
                    setupCreatePanel();
                });

                /* Beats Button functionaility */
                var beatsNode = dom.byId("beatButton");
                on(beatsNode, "click", function (evt) {
                    accordion.selectChild("beats");
                    mapController.changeState("beat");
                    setupBeatPanel();
                });

                /* Edit button functionality */
                var editNode = dom.byId("editButton");
                on(editNode, "click", function () {
                    if (domClass.contains(editNode, "active")) {
                        domClass.remove(editNode, "active");
                        mapController.changeState("contents");
                    } else {
                        domClass.add(editNode, "active");
                        accordion.selectChild("contents");
                        mapController.changeState("edit");
                        setupEditPanel();
                    }
                });
            });
        }
    });
