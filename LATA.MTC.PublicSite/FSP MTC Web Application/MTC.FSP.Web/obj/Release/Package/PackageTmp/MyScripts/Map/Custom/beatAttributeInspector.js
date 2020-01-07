define([
    "dijit/_WidgetBase",
    "dijit/_OnDijitClickMixin",
    "dijit/_TemplatedMixin",
    "dijit/form/CheckBox",
    "dijit/form/TextBox",
    "dijit/form/Textarea",
    "dojox/form/CheckedMultiSelect",
    "dojo/Evented",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/on",
    "dojo/when",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/request",
    "dojo/query",
    "dojo/json",
    "dojo/store/Memory",
    "dojo/data/ObjectStore",
    "dijit/registry",
    "esri/tasks/query",
    "esri/tasks/QueryTask",
    "dojo/text!Custom/Templates/beatAttributeInspector_template.html",
    "dojo/domReady!"
    ],
    function (_WidgetBase, _OnDijitClickMixin, _TemplatedMixin, CheckBox, TextBox, Textarea, CheckedMultiSelect, Evented, declare, lang, arrayUtil, on, when, dom, domConstruct, domClass, domStyle, request, query, JSON, Memory, DataStore, registry, Query, QueryTask, dijitTemplate) {
        return declare([_WidgetBase, _OnDijitClickMixin, _TemplatedMixin, Evented], {
            templateString: dijitTemplate,
            baseClass: "beatAttributeInspector",
            beatNumber: 0,
            description: "",
            active: true,
            freeways: [],
            //serviceAddress: query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/getBeatsFreewaysByBeat?beatNumber=",
            serviceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getBeatsFreewaysByBeat?beatNumber=",
            //fullServiceAddress: query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/getBeatsFreeways",
            fullServiceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getBeatsFreeways",
            //freewayServiceAddress: query(".websiteUrl")[0].innerHTML.trim() + ":9017/AJAXFSPService.svc/getFreeways",
            freewayServiceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getFreeways",
            constructor: function (params, srcRefNode) {
                this.domNode = srcRefNode;
                this.beatNumber = params.beatNumber;
            },
            startup: function () {
                this._addFormElements();
                if (this.beatNumber > 0) {
                    query("#beatId").style("display", "none");
                    query("#beatIdLabel").style("display", "none");
                    this._getBeatInformation();
                }
            },
            postCreate: function () {
            },
            _init: function () {

            },
            _addFormElements: function () {
                var newInput = dom.byId("activeCheckBox");
                var labelInput = dom.byId("activeCheckBoxLabel");
                var getFreeways = lang.hitch(this, this._getFreeways);
                var label = domConstruct.create("label", {
                    innerHTML: "Active:",
                    for: "activeCheckBox"
                }, labelInput);

                var checkBox = new CheckBox({
                    name: "activeCheckBox",
                    value: "active",
                    checked: true,
                    className: "activeCheckBox",
                    style: "margin-left: 5px;"
                }, newInput).startup();

                newInput = dom.byId("beatDescription");

                var textarea = new Textarea({
                    name: "beatDescription"
                }, newInput).startup();

                var freeways = dojo.byId("beatFreeways");
                when(getFreeways(), function (freewayStore) {
                    var freewaySelect = new CheckedMultiSelect({
                        name: "beatFreeways",
                        labelAttr: "label",
                        sortByLabel: false,
                        multiple: true,
                        dropDown: true,
                        store: freewayStore
                    }, freeways).startup();
                });
            },
            _setFormValues: function (inputData) {
                if (inputData !== 0) {
                    //dom.byId("beatId").value = inputData[0].BeatNumber;
                    dom.byId("beatDescription").value = inputData[0].BeatDescription;
                    registry.byId("beatDescription").resize();
                    dom.byId("activeCheckBox").value = inputData[0].Active;
                    var freeways = inputData[0].Freeways;
                    var freewaySelect = dijit.byId("beatFreeways");
                    if (freeways) {
                        freewaySelect.set('value', freeways);
                    }
                }
            },
            _checkBeatId: function(beatId) {
                // take beatId and check it against the existing ones to make sure
                // that it is unique

            },
            getFormValues: function () {
                // refreshes the local values to the current input form
                var checkBeatId = lang.hitch(this, this._checkBeatId);
                if (this.beatNumber === 0) {
                    this.beatNumber = dom.byId("beatId").value;
                }
                this.description = dom.byId("beatDescription").value;
                this.active = registry.byId("activeCheckBox").checked;
                this.freeways = registry.byId("beatFreeways").value;
            },
            _getBeatInformation: function () {
                var localService = this.serviceAddress + this.beatNumber;
                console.log(localService);
                var setFormValues = lang.hitch(this, this._setFormValues);
                request.get(localService, {
                    headers: {
                        "X-Requested-With": null
                    },
                    handleAs: "json"
                }).then(
                    function (response) {
                        var myData = JSON.parse(response.d);
                        console.log(myData);
                        setFormValues(myData);
                    }
                );
            },
            _getFreeways: function () {
                var dataStore;
                var memoryStore;
                return request.get(this.freewayServiceAddress, {
                    headers: {
                        // necessary to turn off this header or we can't see the service
                        "X-Requested-With": null
                    },
                    handleAs: "json"
                }).then(
                    function (response) {
                        var myData = JSON.parse(response.d);
                        //var selectList = dojo.byId("beatFreeways");
                        
                        var output = arrayUtil.map(myData, function (item, idx) {
                            return {
                                label: item,
                                id: toString(idx)
                            };
                        });
                        output.sort(function (a, b) { return parseInt(a.label) - parseInt(b.label) });

                        memoryStore = new Memory({
                            idProperty: "label",
                            data: output
                        });

                        dataStore = new DataStore({
                            objectStore: memoryStore
                        });
                        //arrayUtil.forEach(myData, function (freeway) {
                        //    var freeway = domConstruct.create("option", {
                        //        innerHTML: freeway,
                        //        value: freeway
                        //    }, selectList);
                        //});
                        return dataStore;
                    }
                );
            }
        });
});