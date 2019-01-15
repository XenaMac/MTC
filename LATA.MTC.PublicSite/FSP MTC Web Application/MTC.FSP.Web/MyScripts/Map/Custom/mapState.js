// defines a state for the map interface
define([
    "dojo/_base/declare",
    "dojo/_base/lang"
], function (declare, lang) {
    return declare(null, {
        statename: "",
        constructor: function(args){
            lang.mixin(this, args);
        }
    });
});