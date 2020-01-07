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
    "dojo/text!Custom/Templates/EditPanel_template.html",
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
        dijitTemplate
    ) {
    var EditPanel = declare("esri.dijit.EditPanel", [_WidgetBase, _TemplatedMixin, Evented],
        {
            options: {
                map: null,
                layers: null,
                visible: true
            },
            constructor: function (options, srcRefNode) {
                var defaults = lang.mixin({}, this.options, options);
                this.domNode = srcRefNode;
                this.clickHandles = [];

                // properties

            },
            startup: function () {

            },
            destroy: function () {

            },
            _onClick: function () {

            },
            _init: function () {
               
            }
        });
    if (has("extend-esri")) {
        lang.setObject("dijit.EditPanel", EditPanel, esriNS);
    }
    return EditPanel;
});