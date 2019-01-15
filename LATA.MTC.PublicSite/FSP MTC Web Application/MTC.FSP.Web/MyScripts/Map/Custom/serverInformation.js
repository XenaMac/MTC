// class to hold the different layer configurations for development,
// acceptance, and production
define([
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array"
], function (declare, lang, arrayUtils) {
    return declare(null, {
        serverType: "development",
        server: {
            callBoxLayer: "",
            towTruckLayer: "",
            beatsLayer: "",
            segmentsLayer: "",
            dropZoneLayer: ""
        },
        constructor: function (args) {
            this._setLayerAddresses();
        },
        _setLayerAddresses: function () {
            // development addresses
            this.server.callBoxLayer = "";
            this.server.towTruckLayer = "";
            this.server.beatsLayer = "";
            this.server.segmentsLayer = "";
            this.server.dropZoneLayer = "";

            // acceptance addresses
            this.server.callBoxLayer = "";
            this.server.towTruckLayer = "";
            this.server.beatsLayer = "";
            this.server.segmentsLayer = "";
            this.server.dropZoneLayer = "";

            // production addresses
            this.server.callBoxLayer = "";
            this.server.towTruckLayer = "";
            this.server.beatsLayer = "";
            this.server.segmentsLayer = "";
            this.server.dropZoneLayer = "";
        }
    });
})