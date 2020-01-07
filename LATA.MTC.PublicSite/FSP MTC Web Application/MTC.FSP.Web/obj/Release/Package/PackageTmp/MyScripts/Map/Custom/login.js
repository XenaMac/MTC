define([
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/json",
    "dojo/request"
],
function (declare, lang, array, JSON, request) {
    return declare(null, {
        //serviceAddress: "http://38.124.164.213/MTCWebsite/Home/GetCurrentUser",
        serviceAddress: "http://localhost/MTC.FSP.Web/Home/GetCurrentUser",
        userRole: "",
        userID: "",
        getUserInfo: function () {
            request.get(this.serviceAddress, {
                headers: {
                    "X-Requested-With": null
                },
                handleAs: "json"
            }).then(
                function (response) {
                    this.userRole = response.SelectedRoleName;
                    this.userID = response.UserName;
                },
                function (error) {
                    console.log(error);
                }
            )
        }
    });
});