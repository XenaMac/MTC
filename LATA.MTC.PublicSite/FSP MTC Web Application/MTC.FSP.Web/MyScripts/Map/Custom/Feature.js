// this is a base class for the features that are being added to the map

define([
    "dojo/_base/declare",
    "esri/dijit/PopupTemplate",
    "esri/layers/FeatureLayer",
    "dijit/form/CheckBox",
    "esri/renderers/SimpleRenderer"
], function (declare, PopupTemplate, FeatureLayer, CheckBox, SimpleRenderer) {
    return declare(null, {
        template: null,
        layer: null,
        renderer: null,
        visible: true,
        constructor: function (kwArgs) {

        }
    });
});