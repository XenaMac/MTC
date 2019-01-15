define([
    "dojo/_base/declare",
    "dojo/domReady!"
], function (declare) {
    return declare("addresses",null, {
        devServer: "http://38.124.164.213",
        accServer: "http://38.124.164.212",
        prodServer: "http://38.124.164.211",
        arcServer: "http://38.124.164.214:6080",
        state: "dev",
        serviceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getBeatsFreewaysByBeat?beatNumber=",
        fullServiceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getBeatsFreeways",
        freewayServiceAddress: "http://38.124.164.213:9017/AJAXFSPService.svc/getFreeways",
        beats: "http://38.124.164.214:6080/arcgis/rest/services/Beat_and_Segements_2015/FeatureServer/0",
        segments: "http://38.124.164.214:6080/arcgis/rest/services/Beat_and_Segements_2015/FeatureServer/1",
        dropZones: "http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer/0",
        towTruckYards: "http://38.124.164.214:6080/arcgis/rest/services/TowTruckSites/FeatureServer/0",
        callBoxes: "http://38.124.164.214:6080/arcgis/rest/services/Callboxes/FeatureServer/0",
        currentUser: "http://localhost/MTC.FSP.Web/Home/GetCurrentUser",
        geometryServer: "http://38.124.164.214:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer",
        constructor: function (args) {
            this.state = args.state;
            this._getAddresses;
        },
        _getAddresses: function () {
            // function that takes generates the addresses for a given state
        }
    });
});