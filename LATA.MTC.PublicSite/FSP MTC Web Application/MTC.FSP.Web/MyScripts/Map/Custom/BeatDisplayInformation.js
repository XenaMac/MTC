define(["dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/domReady!"],
    function (declare, lang) {
        return declare(null, {
            contractorName: "",
            truckCount: 0,
            callsigns: [],
            constructor: function (args) {
                declare.safeMixin(this.args);
            }
        });
    })