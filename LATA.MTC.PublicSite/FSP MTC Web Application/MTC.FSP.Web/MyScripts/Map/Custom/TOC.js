define([
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/has",
    "esri/kernel",
    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dojo/on",
    "dojo/text!Custom/Templates/TOC_template.html",
    "dojo/query",
    "dojo/dom",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/dom-construct",
    "dojo/_base/event",
    "dojo/_base/array",
    "esri/layers/FeatureLayer",
    "esri/tasks/query"
],
function (
    Evented,
    declare,
    lang,
    has,
    esriNS,
    _WidgetBase,
    _TemplatedMixin,
    on,
    dijitTemplate,
    query,
    dom,
    domClass,
    domStyle,
    domConstruct,
    event,
    array,
    FeatureLayer,
    Query
    ) {
    var Widget = declare("esri.dijit.TableOfContents", [_WidgetBase, _TemplatedMixin, Evented], {
        templateString: dijitTemplate,
        addedLayerName: "",
        options: {
            theme: "TableOfContents",
            map: null,
            layers: null,
            displayLayers: null,
            visible: true,
            userState: "full"
        },
        constructor: function (options, srcRefNode) {
            var defaults = lang.mixin({}, this.options, options);
            this.domNode = srcRefNode;

            // properties
            this.set("map", defaults.map);
            this.set("layers", defaults.layers);
            this.set("displayLayers", defaults.displayLayers);
            this.set("theme", defaults.theme);
            this.set("visible", defaults.visible);
            this.set("userState", defaults.userState);

            // listeners
            this.watch("theme", this._updateThemeWatch);
            this.watch("visible", this._visible);
            this.watch("layers", this._refreshLayers);
            this.watch("displayLayers", this._refreshDisplayLayers);
            this.watch("map", this.refresh);

            // classes
            this.css = {
                container: "toc-container",
                layer: "toc-layer",
                firstLayer: "toc-title",
                title: "toc-title",
                titleContainer: "toc-title-container",
                content: "toc-content",
                titleCheckbox: "toc-checkbox",
                checkboxCheck: "icon-check-1",
                titleText: "toc-text",
                visible: "toc-visible",
                featureList: "toc-feature-list",
                featureContainer: "toc-feature-holder",
                feature: "toc-feature",
                hidden: "hidden",
                displayLayer: "display-layer"
            };
        },
        startup: function () {
            if (!this.map) {
                this.destroy();
                console.log('no map found');
            }

            if (this.map.loaded) {
                this._init();
            } else {
                on.once(this.map, "load", lang.hitch(this, function () {
                    this._init();
                }));
            }
        },
        destroy: function () {
            this._removeEvents();
            this._removeDisplayEvents();
            this.inherited(arguments);
        },
        show: function () {
            this.set("visible", true);
        },
        hide: function () {
            this.set("visible", false);
        },
        refresh: function () {
            this.set("layers", this.layers);
            this.set("displayLayers", this.displayLayers);
            this._createList();
            this._addDisplayLayers();
        },
        _addDisplayLayers: function () {
            var displayLayers = this.get("displayLayers");
            this._displayNodes = [];
            this._removeDisplayEvents();
            //this._displayLayersNode.innerHTML = '';
            if (displayLayers && displayLayers.length) {
                for (var i = 0; i < displayLayers.length; i++) {
                    var displayLayer = displayLayers[i];
                    var titleCheckBoxClass = this.css.titleCheckbox;
                    var layerClass = this.css.layer;
                    if (displayLayer.visible) {
                        layerClass += ' ';
                        layerClass += this.css.visible;
                        titleCheckBoxClass += ' ';
                        titleCheckBoxClass += this.css.checkboxCheck;
                    }
                    var layerDiv = domConstruct.create("div", {
                        className: layerClass
                    });
                    domConstruct.place(layerDiv, this._layersNode, "last");

                    // title of layer
                    var titleDiv = domConstruct.create("div", {
                        className: this.css.title
                    });
                    domConstruct.place(titleDiv, layerDiv, "last");
                    // title container
                    var titleContainerDiv = domConstruct.create("div", {
                        className: this.css.titleContainer
                    });
                    domConstruct.place(titleContainerDiv, titleDiv, "last");
                    // title checkbox
                    var titleCheckBox = domConstruct.create("div", {
                        className: titleCheckBoxClass
                    });
                    domConstruct.place(titleCheckBox, titleContainerDiv, "last");
                    // title text
                    var displayText = this.css.titleText;
                    displayText += ' ';
                    displayText += this.css.displayLayer;
                    var name = "";
                    if (displayLayer.name) {
                        name = this._generateLayerName(displayLayer.name);
                    } else {
                        name = this._generateLayerName(displayLayer.url);
                    }

                    var titleText = domConstruct.create("div", {
                        className: displayText,
                        title: name,
                        innerHTML: name
                    });
                    domConstruct.place(titleText, titleContainerDiv, "last");

                    var displayNodesObj = {
                        checkbox: titleCheckBox,
                        title: titleDiv,
                        titleContainer: titleContainerDiv,
                        titleText: titleText,
                        layer: layerDiv
                    };
                    this._displayNodes.push(displayNodesObj);
                    this._displayCheckboxEvent(i);
                }
                this._setDisplayLayerEvents();
            }
        },
        _createList: function () {
            var layers = this.get("layers");
            this._nodes = [];
            // kill events
            this._removeEvents();
            // clear node
            this._layersNode.innerHTML = '';
            // if we got layers
            if (layers && layers.length) {
                for (var i = 0; i < layers.length; i++) {
                    var layer = layers[i];
                    var titleCheckBoxClass = this.css.titleCheckbox;
                    var layerClass = this.css.layer;
                    // first layer
                    if (i === (layers.length - 1)) {
                        layerClass += ' ';
                        layerClass += this.css.firstLayer;
                    }
                    if (layer.visible) {
                        layerClass += ' ';
                        layerClass += this.css.visible;
                        titleCheckBoxClass += ' ';
                        titleCheckBoxClass += this.css.checkboxCheck;
                    }
                    //layer node
                    var layerDiv = domConstruct.create("div", {
                        className: layerClass
                    });
                    domConstruct.place(layerDiv, this._layersNode, "first");
                    // title of layer
                    var titleDiv = domConstruct.create("div", {
                        className: this.css.title
                    });
                    domConstruct.place(titleDiv, layerDiv, "last");
                    // title container
                    var titleContainerDiv = domConstruct.create("div", {
                        className: this.css.titleContainer
                    });
                    domConstruct.place(titleContainerDiv, titleDiv, "last");
                    // title checkbox
                    var titleCheckBox = domConstruct.create("div", {
                        className: titleCheckBoxClass
                    });
                    domConstruct.place(titleCheckBox, titleContainerDiv, "last");
                    // title text
                    var titleText = domConstruct.create("div", {
                        className: this.css.titleText,
                        title: this._generateLayerName(layer.name),
                        innerHTML: this._generateLayerName(layer.name)
                    });
                    domConstruct.place(titleText, titleContainerDiv, "last");

                    // add in the features
                    var featureHolder = domConstruct.create("div", {
                        className: "toc-feature-holder"
                    });
                    domConstruct.place(featureHolder, titleDiv, "last");
                    var featureList = domConstruct.create("ul", {
                        className: this.css.featureList,
                        id: layer.name
                    });
                    domConstruct.place(featureList, featureHolder, "last");
                    this._generateFeatureList(layer.name, layer, featureList);

                    // if the layer is hidden, add the hidden class to the feature list
                    if (!layer.visible) {
                        domClass.add(featureHolder, this.css.hidden);
                    }

                    var nodesObj = {
                        checkbox: titleCheckBox,
                        title: titleDiv,
                        titleContainer: titleContainerDiv,
                        titleText: titleText,
                        features: featureList,
                        featureHolder: featureHolder,
                        layer: layerDiv
                    };
                    this._nodes.push(nodesObj);
                    this._checkboxEvent(i);
                }
                this._setLayerEvents();
            }
        },
        _generateFeatureList: function(layerName, inputLayer, listNode) {
            var query = new Query();
            query.where = "1=1";
            query.outFields = ["*"];
            query.orderByFields = this._getOrderByFields(layerName, inputLayer);
            var getFeatureName = lang.hitch(this, this._getFeatureName);
            var featureClass = this.css.feature;
            var setFeatureEvent = lang.hitch(this, this._setFeatureEvent);
            var features = inputLayer.queryFeatures(query, function (featureSet) {
                array.forEach(featureSet.features, function (feature, index) {
                    var name = getFeatureName(layerName, featureSet, index);
                    var featureNode = domConstruct.create("li", { innerHTML: name, className: featureClass }, listNode, "last");
                    setFeatureEvent(feature, featureNode, inputLayer);
                });
            });
        },
        _getOrderByFields: function(layerName, inputLayer){
            var returnField = [];
            var returnName = {
                "Drop Sites": "Name",
                "Beats": "BEAT_ID_1",
                "Segments": "Beatsegmen",
                "Tow Truck Yards": "Contract_1"
            }
            if (returnName[layerName]) {
                returnField.push(returnName[layerName]);
            }
            return returnField;
        },
        _getFeatureName: function (layerName, inputLayer, index) {
            var featureName = "feature";
            var getTruckYardAbbreviations = lang.hitch(this, this._getTruckYardAbbreviation);
            var nameHash = {
                "Drop Sites": inputLayer.features[index].attributes.Name,
                "Beats": inputLayer.features[index].attributes.BEAT_ID_1,
                "Segments": inputLayer.features[index].attributes.Beatsegmen,
                "Tow Truck Yards": getTruckYardAbbreviations(inputLayer.features[index].attributes.Contract_1)
            }
            if (nameHash[layerName]) {
                featureName = nameHash[layerName];
            }
            return featureName;
        },
        _getTruckYardAbbreviation: function(truckYardName) {
            var returnValue = truckYardName;
            var yardHash = {
                "Action Towing & Road Service Inc.": "Action",
                "All Counties": "All Counties",
                "American Tow": "American",
                "Atlas Towing Services, Inc.": "Atlas",
                "B&A Bodyworks/Towing": "B&A",
                "Bill's Towing": "Bill's",
                "Bob's Towing": "Bob's",
                "Campbell's Towing": "Campbell's",
                "Ken Betts' Towing": "Ken Betts'",
                "Lima Tow": "Lima",
                "Myers Towing Services": "Myers",
                "Palace Garage": "Palace Garage",
                "Redhill Towing & Autobody": "Redhill",
                "Roadrunner Tow": "Roadrunner",
                "Save Tow": "Save Tow",
                "Sierra Hart": "Sierra Hart",
                "Yarbrough Bros. Towing": "Yarbrough"
            }
            if (yardHash[truckYardName]) {
                returnValue = yardHash[truckYardName];
            }
            return returnValue;
        },
        _generateLayerName: function (inputName) {
            var nameHash = {
                "Drop Sites": "Drop Sites",
                "Beats": "Beats",
                "Segments": "Segments",
                "Tow Truck Yards": "Tow Truck Yards",
                "MTCFSPDevelopment.DBO.Callboxes": "Call Boxes",
                "http://maps.traffic.511.org/ArcGIS/rest/services/Speed_Service_REDGREEN/MapServer": "Congestion" 
            }
            if (nameHash[inputName]) {
                inputName = nameHash[inputName];
            } else if(this.addedLayerName !== "") {
                inputName = this.addedLayerName;
            }
            return inputName;
        },
        _refreshLayers: function () {
            this.refresh();
        },
        _refreshDisplayLayers: function () {
            this.refresh();
        },
        _removeDisplayEvents: function(){
            
        },
        _removeEvents: function () {
            var i;
            if (this._checkEvents && this._checkEvents.length) {
                for (i = 0; i < this._checkEvents.length; i++) {
                    this._checkEvents[i].remove();
                }
            }
            if (this._displayCheckEvents && this._displayCheckEvents.length) {
                for (i = 0; i < this._displayCheckEvents.length; i++) {
                    this._displayCheckEvents[i].remove();
                }
            }
            if (this._layerEvents && this._layerEvents.length) {
                for (i = 0; i < this._layerEvents.length; i++) {
                    this._layerEvents[i].remove();
                }
            }
            this._checkEvents = [];
            this._displayCheckEvents = [];
            this._layerEvents = [];
            this._featureEvents = [];
            this._displayLayerEvents = [];
        },
        _toggleVisible: function (index, visible) {
            domClass.toggle(this._nodes[index].layer, this.css.visible, visible);
            domClass.toggle(this._nodes[index].checkbox, this.css.checkboxCheck, visible);
            domClass.toggle(this._nodes[index].featureHolder, this.css.hidden, !visible)
            this.emit("toggle", {
                index: index,
                visible: visible
            });
        },
        _toggleDisplayVisible: function (index, visible) {
            domClass.toggle(this._displayNodes[index].layer, this.css.visible, visible);
            domClass.toggle(this._displayNodes[index].checkbox, this.css.checkboxCheck, visible);

            this.emit("toggle", {
                index: index,
                visible: visible
            });
        },
        _zoomToFeature: function(feature){
            this.map.setExtent(feature.geometry.getExtent(), true);
        },
        _displayLayerEvent: function(layer, index){
            var visChange = on(layer, 'visibility-change', lang.hitch(this, function(evt){
                this._toggleDisplayVisible(index, evt.visible);
            }));
            this._displayLayerEvents.push(visChange);
        },
        _layerEvent: function (layer, index) {
            var visChange = on(layer, 'visibility-change', lang.hitch(this, function (evt) {
                this._toggleVisible(index, evt.visible);
            }));
            this._layerEvents.push(visChange);
        },
        _hiliteFeature: function (featureNode) {
            query(".feature_active").removeClass("feature_active");
            domClass.toggle(featureNode, "feature_active");
        },
        _featureEvent: function (feature, featureNode, featureLayer){
            var featureClicked = on(featureNode, 'click', lang.hitch(this, function (evt) {
                this._zoomToFeature(feature);
                this._hiliteFeature(featureNode);
                var query = new Query();
                query.where = "OBJECTID = " + feature.attributes.OBJECTID;
                featureLayer.selectFeatures(query, FeatureLayer.SELECTION_NEW);
                var selected = featureLayer.getSelectedFeatures();
                //this.map.infoWindow.setFeatures(selected);
                //this.map.infoWindow.show(feature.geometry.getPoint(0, 0));
            }));
            this._featureEvents.push(featureClicked);
        },
        _featureCollectionVisible: function (layer, index, visible) {
            var equal;
            var visibleLayers = layer.visibleLayers;
            var layers = layer.featureCollection.layers;
            if (visibleLayers && visibleLayers.length) {
                equal = array.every(visibleLayers, function (item) {
                    return layers[item].layerObject.visible === visible;
                });
            } else {
                equal = array.every(layers, function (item) {
                    return item.layerObject.visible === visible;
                });
            }
            if (equal) {
                this._toggleVisible(index, visible);
            }
        },
        _createFeatureLayerEvent: function (layer, index, i) {
            var layers = layer.featureCollection.layers;
            var visChange = on(layers[i].layerObject, 'visiblity-change', lang.hitch(this, function (evt) {
                var visible = evt.visible;
                this._featureCollectionVisible(layer, index, visible);
            }));
            this._layerEvents.push(visChange);
        },
        _featureLayerEvent: function (layer, index) {
            var layers = layer.featureCollection.layers;
            if (layers && layers.length) {
                for (var i = 0; i < layers.length; i++) {
                    this._createFeatureLayerEvent(layer, index, i);
                }
            }
        },
        _setDisplayLayerEvents: function() {
            var displayLayers = this.get("displayLayers");
            var displayLayerObject;
            if (displayLayers && displayLayers.length) {
                for (var i = 0; i < displayLayers.length; i++) {
                    var displayLayer = displayLayers[i];
                    this._displayLayerEvent(displayLayer, i);
                }
            }
        },
        _setLayerEvents: function () {
            var layers = this.get("layers");
            var layerObject;
            if (layers && layers.length) {
                for (var i = 0; i < layers.length; i++) {
                    var layer = layers[i];
                    if (layer.featureCollection && layer.featureCollection.layers && layer.featureCollection.layers.length) {
                        this._featureLayerEvent(layer, i);
                    } else {
                        layerObject = layer;
                        this._layerEvent(layerObject, i);
                    }
                }
            }
        },
        _setFeatureEvent: function (feature, featureNode, featureLayer) {
            this._featureEvent(feature, featureNode, featureLayer);
        },
        _toggleDisplayLayer: function (displayLayerIndex) {
            if (this.displayLayers && this.displayLayers.length) {
                var newVis;
                var displayLayer = this.displayLayers[displayLayerIndex];
                if (displayLayer) {
                    newVis = !displayLayer.visible;
                    displayLayer.visibility = newVis;
                    if (newVis) {
                        displayLayer.show();
                    } else {
                        displayLayer.hide();
                    }
                }
            }
        },
        _toggleLayer: function (layerIndex) {
            if (this.layers && this.layers.length) {
                var newVis;
                var layer = this.layers[layerIndex];
                var layerObject = layer;
                var featureCollection = layer.featureCollection;
                var visibleLayers;
                var i;
                if (featureCollection) {
                    visibleLayers = layer.visibleLayers;
                    newVis = !layer.visibility;
                    layer.visibility = newVis;
                    if (visibleLayers && visibleLayers.length) {
                        for (i = 0; i < visibleLayers.length; i++) {
                            layerObject = featureCollection.layers[visibleLayers[i]].layerObject;
                            layerObject.setVisiblity(newVis);
                        }
                    } else {
                        for (i = 0; i < featureCollection.layers.length; i++) {
                            layerObject = featureCollection.layers[i].layerObject;
                            layerObject.setVisiblity(newVis);
                        }
                    }
                } else if (layerObject) {
                    newVis = !layerObject.visible;
                    layer.visibility = newVis;
                    if (newVis) {
                        layerObject.show();
                    } else {
                        layerObject.hide();
                    }
                    //layerObject.setVisibility(newVis);
                }
            }
        },
        _displayCheckboxEvent: function (index) {
            var displayCheckEvent = on(this._displayNodes[index].checkbox, 'click', lang.hitch(this, function (evt) {
                this._toggleDisplayLayer(index);
                event.stop(evt);
            }));
            this._displayCheckEvents.push(displayCheckEvent);
            var titleEvent = on(this._displayNodes[index].titleText, 'click', lang.hitch(this, function (evt) {
                this._toggleDisplayLayer(index);
                event.stop(evt);
            }));
            this._displayCheckEvents.push(titleEvent);
        },
        _checkboxEvent: function (index) {
            // when checkbox is clicked
            var checkEvent = on(this._nodes[index].checkbox, 'click', lang.hitch(this, function (evt) {
                this._toggleLayer(index);
                event.stop(evt);
            }));
            this._checkEvents.push(checkEvent);
            // when title is clicked
            var titleEvent = on(this._nodes[index].titleText, 'click', lang.hitch(this, function (evt) {
                this._toggleLayer(index);
                event.stop(evt);
            }));
            this._checkEvents.push(titleEvent);
        },
        _init: function () {
            this._visible();
            this._createList();
            this._addDisplayLayers();
            this.set("loaded", true);
            this.emit("load", {});
        },
        _updateThemeWatch: function () {
            var oldVal = arguments[1];
            var newVal = arguments[2];
            domClass.remove(this.domNode, oldVal);
            domClass.add(this.domNode, newVal);
        },
        _visible: function () {
            if (this.get("visible")) {
                domStyle.set(this.domNode, 'display', 'block');
            } else {
                domStyle.set(this.domNode, 'display', 'none');
            }
        }
    });
    if (has("extend-esri")) {
        lang.setObject("dijit.TableOfContents", Widget, esriNS);
    }
    return Widget;
});