// this is the custom class that defines a dropzone
define([
    "dojo/_base/declare",
    "esri/dijit/PopupTemplate",
    "esri/layers/FeatureLayer",
    "dijit/form/CheckBox",
    "esri/renderers/SimpleRenderer"
], function (declare, PopupTemplate, FeatureLayer, CheckBox, SimpleRenderer) {
    return declare(null, {
        layerAddress: "http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer/0",
        constructor: function (kwArgs) {
            this.template = new PopupTemplate({
                title: "Drop Site Information",
                fieldInfos: [{
                    fieldName: "Name",
                    label: "Drop Site Name",
                    visible: true
                }, {
                    fieldName: "DPSDSC",
                    label: "Drop Site Description",
                    visible: true
                }]
            });

            this.dropZoneLayer = new FeatureLayer(this.layerAddress, {
                mode: FeatureLayer.MODE_ONDEMAND,
                showAttribution: false,
                outFields: ["*"],
                infoTemplate: this.template
            });

        },
        editTemplate: {
            'featureLayer': this.dropZoneLayer,
            'showAttachments': false,
            'isEditable': true,
            'showDeleteButton': false,
            'fieldInfos': [
                { 'fieldName': 'OBJECTID', 'isEditable': true, 'tooltip': 'This is the name of the drop zone.', 'label': 'Drop Zone Name' },
                { 'fieldName': 'Name', 'isEditable': true, 'tooltip': 'This is a description of the drop zone', 'label': 'Drop Zone Description' },
                { 'fieldName': 'created_user', 'isEditable': true, 'tooltip': 'This is the user who created the drop zone.', 'label': 'Created By' },
                { 'fieldName': 'created_date', 'isEditable': true, 'tooltip': 'This is the date that the drop zone was created.', 'label': 'Created On' },
                { 'fieldName': 'last_edited_user', 'isEditable': true, 'tooltip': 'This is the last user who edited this drop zone.', 'label': 'Last Edited By' },
                { 'fieldName': 'last_edited_date', 'isEditable': true, 'tooltip': 'This is the last date the drop zone was edited.', 'label': 'Last Edited On' }
            ]
        },
        makeCheckBox: function (element) {
            this.checkBox = new CheckBox({
                name: "DropZoneCheck",
                value: "layerOn",
                checked: true,
                onChange: function () {
                    if (this.dropZoneLayer.visible) {
                        this.dropZoneLayer.hide();
                    } else {
                        this.dropZoneLayer.show();
                    }
                }
            }, element);
        }

    });
});