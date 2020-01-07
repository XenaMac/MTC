// this is the module for creating a beat selector widget
define([
    "dijit/_Widget",
    "dijit/_OnDijitClickMixin",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/Dialog",
    "dijit/form/CheckBox",
    "dijit/form/RadioButton",
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/on",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/query",
    "esri/tasks/query",
    "esri/tasks/QueryTask",
    "dojo/text!Custom/Templates/beat_template.html",
    "Custom/BeatCreator",
    "dojo/domReady!"
],
function (_Widget, _OnDijitClickMixin, _TemplatedMixin, _WidgetsInTemplateMixin, Dialog, CheckBox, RadioButton, Evented, declare, lang, arrayUtils, on, dom, domConstruct, domClass, domStyle, query, Query, QueryTask, dijitTemplate, BeatCreator) {
    return declare(Dialog, {
        baseClass: "beatSelector",
        title: "",
        features: null,
        selectedFeatures: null,
        beatLayer: null,
        constructor: function (params) {
            var template = dijitTemplate;
            this.title = params.title;
            this.features = [];
            this.selectedFeatures = [];
            var contentWidget = new (declare(
                [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin],
                    {
                        templateString: template
                    }
                ));
            contentWidget.startup();
            this.content = contentWidget;
            this._queryFeatures();
        },
        destroy: function() {
            this.destroyRecursive();
        },
        startup: function () {
            //this.titleNode.innerHTML = this.title;
        },
        postCreate: function () {
            this.inherited(arguments);
            this.connect(this.content.cancelButton, "onClick", "onCancel");
            this.connect(this.content.submitButton, "onClick", "getChosenFeature");
        },
        showFeatures: function () {
            console.log(this.features);
        },
        onCancel: function () {
            this.destroyRecursive();
        },
        onExecute: function () {
            this.getChosenFeature();
            this.destroyRecursive();
        },
        onHide: function(){
            this.destroyRecursive();
        },
        getChosenFeature: function () {
            var checkBoxes = query(".beatRadio");
            var selection;
            arrayUtils.forEach(checkBoxes, function (checkBox) {
                if (checkBox.checked) {
                    var widget = dijit.getEnclosingWidget(checkBox);
                    selection = widget.get("value");
                }
            });

            this.startEditor(selection);
        },
        startEditor: function (selection) {
            this.hide();
            var myBeatCreator = new BeatCreator({
                title: "Editing Beat " + selection,
                class: "nonModal",
                beatLayer: this.beatLayer,
                beatNumber: selection
            });
            myBeatCreator.show();
        },
        _init: function () {

        },
        _queryFeatures: function () {
            var queryTask = new QueryTask("http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/0");
            var query = new Query();
            query.returnGeometry = false;
            query.outFields = ["BEAT_ID_1"];
            query.where = "1=1";
            queryTask.execute(query, lang.hitch(this, this._addFeatures));
        },
        _addFeatures: function (results) {
            var resultCount = results.features.length;
            var beatId = "";
            for (var i = 0; i < resultCount; i++) {
                beatId = results.features[i].attributes["BEAT_ID_1"];
                this.features.push(beatId);
            }
            this.features.sort(function (a, b) { return a - b });
            this._addFeaturesToPage();
        },
        _addFeaturesToPage: function() {
            var beatSelector = dom.byId("beatSelectorContainer");
            if (beatSelector != null) {
                var length = this.features.length;
                for (var i = 0; i < length; i++) {
                    var beatName = this.features[i];
                    var newBeat = domConstruct.create("div", {
                        className: "beatSelection",
                    }, beatSelector);

                    var radioButton = new RadioButton({
                        checked: false,
                        value: beatName,
                        name: "radioButton",
                        className: "beatRadio",
                        id: "radioButton" + i
                    });

                    radioButton.placeAt(newBeat);

                    var label = domConstruct.create("label", {
                        innerHTML: beatName,
                        className: "radioLabel",
                        'for': "radioButton" + i
                    }, newBeat);
                }
            }
        }
    });
});