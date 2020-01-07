define(["dojo/request",
    "dojo/json",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "esri/InfoTemplate",
    "Custom/BeatDisplayInformation",
    "dojo/domReady!"],
    function (request, JSON, declare, lang, arrayUtil, InfoTemplate, BeatInfo) {
        return declare(null, {
            serviceAddress: "http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer/getBeatDataByBeat?BeatNumber=",
            fullServiceAddress : "http://services4.geopowered.com/arcgis/rest/services/LATA/BeatsandSegments2015/FeatureServer//getBeatData",
            beatID: "",
            beatInfo: null,
            getBeatData: function () {
                var address = this._generateServiceAddress();
                var getTime = lang.hitch(this, this._generateDate);
                request.get(address, {
                    headers: {
                        // necessary to turn off this header or we can't see the service
                        "X-Requested-With": null
                    },
                    handleAs: "json"
                }).then(
                    function (response) {
                        var beatData = JSON.parse(response.d);
                        var currentBeat = new BeatInfo();
                        currentBeat.contractorName = beatData[0].ContractorCompanyName;
                        currentBeat.truckCount = beatData[0].TruckCount;
                        arrayUtil.forEach(beatData, function (item, i) {
                            currentBeat.callsigns.push({
                                callsign: item.CallSign,
                                schedule: item.ScheduleName,
                                starttime: getTime(item.StartTime),
                                endtime: getTime(item.EndTime)
                            });
                        });
                        this.beatInfo = currentBeat;
                    },
                    function (error) {
                        console.log(error);
                    }
                )
            },
            _generateDate: function (inputDate) {
                var dateNumber = parseInt(inputDate.replace(/[^0-9.]/g, ""));
                var returnDate = new Date(dateNumber);
                var returnString = "";
                var hours = returnDate.getHours().toString();
                if (hours.length == 1) {
                    hours = "0" + hours;
                }
                var minutes = returnDate.getMinutes().toString();
                if (minutes == "0") {
                    minutes = "00";
                }
                returnString = hours + ":" + minutes;
                return returnString;
            },
            _generateServiceAddress: function() {
                var returnValue = "";
                if (this.beatID === "") {
                    returnValue = this.fullServiceAddress;
                } else {
                    returnValue = this.serviceAddress;
                    returnValue += this.beatID;
                }
                return returnValue;
            },
            _generateContentString: function() {
                var contentString = "";
                contentString += "<div id='beatInformation'>";
                contentString += "<div class='beatPopupContent'><span class='beatContentTitle'>Call Signs</span>" + this.getCallSigns() + "</div>";
                contentString += "<div class='beatPopupContent'><span class='beatContentTitle'>Tow Contractor</span>" + this.beatInfo.contractorName + "</div>";
                contentString += "<div class='beatPopupContent'><span class='beatContentTitle'>Service Hours</span>" + this.getServiceHours() + "</div>";
                contentString += "<div class='beatPopupContent'><span class='beatContentTitle'>Number Of Trucks</span>" + this.beatInfo.truckCount + "</div>";
                contentString += "</div>";
                return contentString;
            },
            _generateContentTitle: function() {
                var returnString = '';

                returnString = "Beat";
                if (this.beatNumber) {
                    returnString += ' ';
                    returnString += this.beatID;
                }

                return returnString;
            },
            getCallSigns: function() {
                var returnString = "";
                returnString += "<ul>";
                arrayUtil.forEach(this.beatInfo.callsigns, function (item, i) {
                    returnString += "<li>";
                    returnString += item.callsign;
                    returnString += "</li>";
                });
                returnString += "</ul>";

                return returnString;
            },
            getServiceHours: function() {
                var returnString = "";
                returnString += "<ul>";
                arrayUtil.forEach(this.beatInfo.callsigns, function (item, i) {
                    returnString += "<li>";
                    returnString += item.starttime
                    returnString += " - ";
                    returnString += item.endtime;
                    returnString += "</li>";
                });
                returnString += "</ul>";
                return returnString;
            },
            generatePopup: function() {
                var templateTitle = this._generateContentTitle();
                var contentString = this._generateContentString();
                var beatTemplate = new InfoTemplate({
                    title: templateTitle,
                    content: contentString
                });
            },
            constructor: function (args) {
                declare.safeMixin(this.args);
                this.beatID = args.beatID;
            },
            startup: function () {
                this.getBeatData();
            }
        });
    });

// write function to generate the popup information