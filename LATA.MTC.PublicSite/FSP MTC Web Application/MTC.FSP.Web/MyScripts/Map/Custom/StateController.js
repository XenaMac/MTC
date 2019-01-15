// controls the state of the map interface
define([
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/dom",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/dom-construct",
    "dijit/registry"
], function (arrayUtils, declare, lang, dom, domClass, domStyle, domConstruct, registry) {
    return declare(null, {
        currentState: '',
        map: null,
        stateNames: ['edit', 'create', 'contents', 'beat'],
        constructor: function (map) {
            this.map = map;
            this.findState("contents").startup(this);
            this.currentState = 'contents';
        },
        states: [
            {
                stateName: 'edit',
                startup: function (currentObject) {
                    currentObject.notifyUser("edit");
                    currentObject.map.setInfoWindowOnClick(false);
                },
                destroy: function (currentObject) {
                    currentObject.notifyUser("edit");
                    currentObject.map.setInfoWindowOnClick(true);
                    currentObject.destroyTemplatePicker();
                    domClass.remove(dom.byId("editButton"), "active");
                }
            },
            {
                stateName: 'create',
                startup: function (currentObject) {
                    currentObject.notifyUser("create");
                    currentObject.map.setInfoWindowOnClick(false);
                },
                destroy: function (currentObject) {
                    currentObject.notifyUser("create");
                    currentObject.map.setInfoWindowOnClick(true);
                    currentObject.destroyTemplatePicker();
                }
            },
            {
                stateName: 'contents',
                startup: function (currentObject) {
                    currentObject.notifyUser("contents");
                    currentObject.map.setInfoWindowOnClick(true);
                },
                destroy: function (currentObject) {
                    currentObject.notifyUser("contents");
                    currentObject.map.setInfoWindowOnClick(false);
                }
            },
            {
                stateName: 'beat',
                startup: function (currentObject) {
                    currentObject.notifyUser("beat");
                    currentObject.map.setInfoWindowOnClick(false);
                },
                destroy: function (currentObject) {
                    currentObject.notifyUser("beat");
                    currentObject.map.setInfoWindowOnClick(true);
                }
            }
        ],
        changeState: function (desiredState) {
            this.findState(this.currentState).destroy(this);
            this.findState(desiredState).startup(this);
            this.currentState = this.findState(desiredState).stateName;
        },
        findState: function (stateName) {
            var returnState = null;
            arrayUtils.forEach(this.states, function (state) {
                if (state.stateName == stateName) {
                    returnState = state;
                }
            });
            return returnState;
        },
        destroyTemplatePicker: function () {
            var widget = registry.byId("templatePickerDiv");
            if (widget) {
                widget.destroy();

                var parentNode = dom.byId("containerDiv");
                domConstruct.create("div", {
                    id: "templatePickerDiv"
                }, parentNode);
            }
        },
        notifyUser: function (state) {
            console.log(state + " changed");
        }
    });
});