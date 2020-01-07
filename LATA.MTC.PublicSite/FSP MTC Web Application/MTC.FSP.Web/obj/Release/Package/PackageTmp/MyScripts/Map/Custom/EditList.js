var pageLocation = location.href.replace(/Map\/Index/, "");

define([
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/has",
    "esri/kernel",
    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dojo/on",
    "dojo/text!Custom/Templates/EditList_template.html",
    "dojo/query",
    "dojo/dom",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/dom-construct",
    "dojo/_base/event",
    "dojo/_base/array",
    "esri/layers/FeatureLayer",
    "esri/tasks/query",
    "esri/toolbars/draw",
    "esri/toolbars/edit",
    "esri/dijit/AttributeInspector",
    "dojo/domReady!"
], function (
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
        arrayUtil,
        FeatureLayer,
        Query,
        Draw,
        Edit,
        AttributeInspector
    ) {
    var Widget = declare("esri.dijit.EditList", [_WidgetBase, _TemplatedMixin, Evented], {
        templateString: dijitTemplate,
        options: {
            theme: "EditList",
            map: null,
            layers: null,
            infoLayers: null,
            infoLayer: "",
            visible: true
        },
        constructor: function (options, srcRefNode) {
            var defaults = lang.mixin({}, this.options, options);
            this.domNode = srcRefNode;
            this.clickHandles = [];

            // properties
            this.set("map", defaults.map);
            this.set("layers", defaults.layers);
            this.set("theme", defaults.theme);
            this.set("visible", defaults.visible);

            // listeners
            this.watch("visible", this._visible);
            this.watch("layers", this._refresh);
            this.watch("map", this.refresh);

            // classes
            this.css = {
                container: "el-container",
                layer: "el-layer",
                firstLayer: "el-title",
                title: "el-title",
                titleContainer: "el-title-container",
                content: "el-content",
                titleText: "el-text",
                titleIcon: "el-icon"
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
            this.inherited(arguments);
        },
        show: function () {
            this.set("visible", true);
        },
        hide: function () {
            this.set("visible", false);
        },
        _refresh: function() {
            this._createList();
        },
        _createList: function() {
            var layers = this.get("layers");
            this._nodes = [];
            // kill events
            this._removeEvents();
            // clear node
            this._layersNode.innerHTML = '';
            var getLayerName = lang.hitch(this, this._getLayerName);
            var getIconLink = lang.hitch(this, this._getIconLink);
            if (layers && layers.length) {
                for (var i = 0; i < layers.length; i++) {
                    var layer = layers[i];
                    var layerIconClass = this.css.titleIcon;
                    var layerClass = this.css.layer;
                    if (i === (layers.length - 1)) {
                        layerClass += ' ';
                        layerClass += this.css.firstLayer;
                    }
                    // layer node
                    var layerDiv = domConstruct.create("div", {
                        className: layerClass
                    });
                    domConstruct.place(layerDiv, this._layersNode, "first");
                    var titleDiv = domConstruct.create("div", {
                        className: this.css.title
                    });
                    domConstruct.place(titleDiv, layerDiv, "last");
                    var titleContainerDiv = domConstruct.create("div", {
                        className: this.css.titleContainer
                    });
                    domConstruct.place(titleContainerDiv, titleDiv, "last");
                    // title icon
                    var titleIcon = domConstruct.create("div", {
                        className: layerIconClass
                    });
                    domStyle.set(titleIcon, {
                        backgroundImage: getIconLink(layer.layer.name)
                    });
                    domConstruct.place(titleIcon, titleContainerDiv, "last");

                    // title text
                    var titleText = domConstruct.create("div", {
                        className: this.css.titleText,
                        title: getLayerName(layer.layer.name),
                        innerHTML: getLayerName(layer.layer.name)
                    });
                    domConstruct.place(titleText, titleContainerDiv, "last");

                    var nodesObj = {
                        icon: titleIcon,
                        title: titleDiv,
                        titleContainer: titleContainerDiv,
                        titleText: titleText,
                        layer: layerDiv,
                        graphicsId: layer.layer.id
                    };
                    this._nodes.push(nodesObj);
                    this._clickEvent(i);
                }
            }

        },
        _getIconLink: function (layerName) {
            var returnValue = "none";
            var iconHash = {
                "Drop Sites": "Content/Images/mtc_map_ui_v3_2_dropzone.png",
                "Beats": "Content/Images/mtc_map_ui_v3_2_beats.png",
                "Segments": "Content/Images/mtc_map_ui_v3_2_segments.png",
                "Tow Truck Yards": "Content/Images/mtc_map_ui_v3_2_tow%20truck%20yards.png"
            }
            if (iconHash[layerName]) {
                returnValue = "url(" + pageLocation + iconHash[layerName] + ")";
            }
            return returnValue;
        },
        _getInfoLayer: function(layerName){
            var returnValue = this.infoLayers[0];
            arrayUtil.forEach(this.infoLayers, function (layerInfo, i) {
                if (layerInfo.featureLayer.name === layerName.name) {
                    returnValue = this.infoLayers[i];
                }
            });
            
            return returnValue;
        },
        _getLayerName: function(layerName) {
            var returnValue = layerName;
            var layerHash = {
                "Drop Sites": "Drop Sites",
                "Beats": "Beats",
                "Segments": "Segments",
                "Tow Truck Yards": "Tow Truck Yards"
            }
            if (layerHash[layerName]) {
                returnValue = layerHash[layerName];
            }
            return returnValue;
        },
        _removeEvents: function() {
            var i;
            if (this._clickEvents && this._clickEvents.length) {
                for (i = 0; i < this._clickEvents.length; i++) {
                    this._clickEvents[i].remove();
                }
            }
            this._clickEvents = [];
        },
        _clickEvent: function (index){
            // the event that happens when the label or the icon are clicked
            var returnMessage = this._nodes[index].titleText.innerHTML + " was clicked.";
            var hilite = lang.hitch(this, this._hilite_choice);
            var select_layer = lang.hitch(this, this._select_layer);
            var refresh = lang.hitch(this, this._refresh);
            var iconEvent = on(this._nodes[index].icon, 'click', lang.hitch(this, function (evt) {
                refresh();
                hilite(index);
                select_layer(index);
            }));
            this._clickEvents.push(iconEvent);
            var titleEvent = on(this._nodes[index].titleText, 'click', lang.hitch(this, function (evt) {
                refresh();
                hilite(index);
                select_layer(index);
            }));
            this._clickEvents.push(titleEvent);
        },
        _select_layer: function(index){
            // activate the tools for the given layer
            var currentLayer = this.map.getLayer(this._nodes[index].graphicsId);
            console.log(currentLayer);
            this.infoLayer = this._getInfoLayer(currentLayer);
            this._setCurrentLayer(currentLayer);
        },
        _setCurrentLayer: function (currentLayer) {
            var localClicks = this.clickHandles;
            if (localClicks.length > 1) {
                arrayUtil.forEach(localClicks, function (clickHandle) {
                    clickHandle.remove();
                });
            }
            var setToolbars = lang.hitch(this, this._setupToolbars(currentLayer));
            // check for a feature first and if there is one enable the editing
            require(["esri/tasks/query"], function (Query) {
                var localClickHandle = currentLayer.on("click", function (evt) {
                    event.stop(evt);
                    var selectQuery = new Query();
                    selectQuery.geometry = evt.mapPoint;
                    currentLayer.selectFeatures(selectQuery, FeatureLayer.SELECTION_NEW, function (features) {
                        if (features.length > 0) {
                            setToolbars(currentLayer);
                        }
                    });
                });

                localClicks.push(localClickHandle);
            });
        },  
        _hilite_choice: function(index){
            // remove any existing hilites
            query(".hilite").forEach(function (node) {
                domClass.toggle(node, "hilite");
            });

            // toggle hilite class for the chosen button
            domClass.toggle(this._nodes[index].layer, "hilite");
        },
        _editInfoWindow: function(currentLayer, editToolbar, evt){
            var updateFeature;
            var infoLayer = this.infoLayer;
            require(["esri/tasks/query", "dijit/form/Button", ], function (Query, Button) {
                var selectQuery = new Query();
                selectQuery.geometry = evt.mapPoint;

                currentLayer.selectFeatures(selectQuery, FeatureLayer.SELECTION_NEW, function (features) {
                    if (features.length > 0) {
                        updateFeature = features[0];
                        map.infoWindow.setTitle(features[0].getLayer().name);
                        map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint));
                    } else {
                        map.infoWindow.hide();
                    }
                });

                map.infoWindow.on("hide", function () {
                    currentLayer.clearSelection();
                    editToolbar.deactivate();
                });

                var layerInfo = [
                    infoLayer
                ];

                var attInspector = new AttributeInspector({
                    layerInfos: layerInfo
                }, domConstruct.create("div"));

                attInspector.on("attribute-change", function (evt) {
                    updateFeature.attributes[evt.fieldName] = evt.fieldValue;
                })

                var saveButton = new Button({
                    label: "Save",
                    "class": "saveButton"
                }, domConstruct.create("div"));

                domConstruct.place(saveButton.domNode, attInspector.deleteBtn.domNode, "after");

                saveButton.on("click", function () {
                    console.log("save button clicked");
                    //this.currentLayer.applyEdits(null, [evt.graphic], null);
                    currentLayer.applyEdits(null, [updateFeature], null);
                });

                var cancelButton = new Button({
                    label: "Cancel",
                    "class": "cancelButton"
                }, domConstruct.create("div"));

                domConstruct.place(cancelButton.domNode, saveButton.domNode, "after");

                cancelButton.on("click", function () {
                    map.infoWindow.hide();
                    attInspector.destroy();
                    editToolbar.deactivate();
                });

                currentLayer.on("edits-complete", function () {
                    console.log("updates successful");
                    map.infoWindow.hide();
                    attInspector.destroy();
                    editToolbar.deactivate();
                });

                map.infoWindow.setContent(attInspector.domNode);
                map.infoWindow.resize(350, 400);
            });
        },
        _setupToolbars: function (currentLayer) {
            var editToolbar = new Edit(this.map);
            var editingEnabled = false;
            var editInfoWindow = lang.hitch(this, this._editInfoWindow);

            on(currentLayer, "click", function (evt) {
                event.stop(evt);
                if (editingEnabled === false) {
                    editingEnabled = true;
                    editToolbar.activate(Edit.EDIT_VERTICES | Edit.MOVE, evt.graphic);
                    editInfoWindow(currentLayer, editToolbar, evt);
                }
            });

            on(this.map, "click", function (evt) {
                map.infoWindow.hide();
                editToolbar.deactivate();
                editingEnabled = false;
            });
        },
        _init: function () {
            this._createList();
            this.set("loaded", true);
            this.emit("load", {});
        }
    });
    if (has("extend-esri")) {
        lang.setObject("dijit.EditList", Widget, esriNS);
    }
    return Widget;
});