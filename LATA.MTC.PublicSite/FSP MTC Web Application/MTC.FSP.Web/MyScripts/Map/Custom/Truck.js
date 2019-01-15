define(["dojo/_base/declare",
    "dojo/_base/lang",
    "esri/symbols/SimpleMarkerSymbol",
    "esri/symbols/PictureMarkerSymbol",
    "esri/Color",
    "esri/symbols/SimpleLineSymbol",
    "esri/graphic",
    "esri/geometry/Point",
    "esri/InfoTemplate",
    "dojo/query",
    "dojo/domReady!"],
    function (declare, lang, SimpleMarkerSymbol, PictureMarkerSymbol, Color, SimpleLineSymbol, Graphic, Point, InfoTemplate, query) {
        return declare(null, {
            map: null,
            self: this,
            graphicsLayer: null,
            graphic: null,
            truckNumber: 0,
            beatNumber: 0,
            driverName: "",
            heading: 0,
            latitude: 0.0,
            longitude: 0.0,
            callsign: "",
            ipAddress: "",
            contractorName: "",
            speed: 0.0,
            vehicleState: "",
            alarms: "",
            backup: false,
            icon: "",
            statusString: "No Status",
            lastMessage: "",
            constructor: function (args) {
                declare.safeMixin(this.args);
                this.map = args.map;
                this.graphicsLayer = args.graphicsLayer;
            },
            determineState: function () {
                // checks the input information and sets the correct icon based on state
                var statusString = "";
                var iconName = "";
                switch (this.vehicleState) {
                    case "RollOut":
                        statusString = "Roll Out";
                        iconName = "mtc_icons_v2_lt_blue";
                        break;
                    case "OnPatrol":
                        statusString = "On Patrol";
                        iconName = "mtc_icons_v2_green";
                        break;
                    case "OnBreak":
                        statusString = "On Break";
                        iconName = "mtc_icons_v2_lt_blue";
                        break;
                    case "OnLunch":
                        statusString = "On Lunch";
                        iconName = "mtc_icons_v2_lt_blue";
                        break;
                    case "EnRoute":
                        statusString = "En Route";
                        iconName = "mtc_icons_v2_orange";
                        break;
                    case "OnIncident":
                        statusString = "On Incident";
                        iconName = "mtc_icons_v2_blue";
                        break;
                    case "OnTow":
                        statusString = "On Tow";
                        iconName = "mtc_icons_v2_yellow";
                        break;
                    case "RollIn":
                        statusString = "Roll In";
                        iconName = "mtc_icons_v2_lt_blue";
                        break;
                    case "LOGGED ON":
                        statusString = "Logged On";
                        iconName = "mtc_icons_v2_purple";
                        break;
                    case "NA":
                        statusString = "No Status";
                        iconName = "mtc_icons_v2_gray";
                        break;
                    default:
                        statusString = "No Status";
                        iconName = "mtc_icons_v2_gray";
                }
                if (this.backup) {
                    iconName += "_backup";
                }

                if (this.alarms) {
                    iconName += "_alert";
                } 

                iconName += ".png";
                
                iconName = this.generateIconName(iconName);
                this.icon = iconName;
                this.statusString = statusString;
            },
            generateIconName: function (iconName) {
                var outputPath = query(".websiteUrl")[0].innerHTML.trim() + "/Content/Images/" + iconName 
                return outputPath;
            },
            generateMessageDate: function () {
                var dateNumber = parseInt(this.lastMessage.replace(/[^0-9.]/g, ""));
                var returnDate = new Date(dateNumber);

                return returnDate.toTimeString().replace(/GMT-[0-9]+/g, "");
            },
            generateDriverName: function () {
                var returnName = this.driverName;
                var nameParts = returnName.split(",");
                returnName = nameParts[1] + " " + nameParts[0];
                return returnName;
            },
            generateContentString: function () {
                var contentString = "";
                contentString += "<div id='truckInformation'>";
                contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>Last Msg: </span>" + this.generateMessageDate() + "</div>";
                if (this.callsign == "NA") {
                    contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>CS: </span> No Callsign Assigned</div>";
                } else {
                    contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>CS: </span>" + this.callsign + "</div>";
                }
                if (this.driverName == "No Driver, No Driver") {
                    contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>Driver: </span>No Driver</div>";
                } else {
                    contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>Driver: </span>" + this.generateDriverName() + "</div>";
                }
                contentString += "<div id='truckNumber' class='truckPopupContent'><span class='truckContentTitle'>Truck Number: </span>" + this.truckNumber + "</div>";
                contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>Contractor: </span>" + this.contractorName + "</div>";
                contentString += "<div class='truckPopupContent'><span class='truckContentTitle'>Status: </span>" + this.statusString + ",  <span class='truckContentTitle'>Speed: </span>" + this.speed + " mph  </div>";
                contentString += "</div>";
                return contentString;
            },
            generateContentTitle: function(){
                var returnString = '';

                returnString = "Beat ";
                if (this.beatNumber === 'NOBEAT') {
                    returnString += ": None Selected";
                } else {
                    returnString += this.beatNumber;
                }

                return returnString;
            },
            plotTruckLocation: function () {
                this.firstPlot();
                //if (self.graphic) {
                //    this.updatePoint();
                //} else {
                //    this.firstPlot()
                //}
            },
            updatePoint: function () {
                console.log("updating");
                self.graphic.geometry.update(this.longitude, this.latitude);
                this.graphicsLayer.add(self.graphic);
            },
            firstPlot: function () {
                var zoomLevel = this.map.getZoom();
                var scale = 1;
                if (zoomLevel > 9) {
                    scale = 1 + ((zoomLevel - 9)/10)
                }

                var height = 20 * scale;
                var width = 20 * scale;
                
                var marker = new PictureMarkerSymbol(this.icon, height, width);
                this.heading -= 90;
                if (this.speed > 0) {
                    marker.setAngle(this.heading);
                }

                var point = new Point(this.longitude, this.latitude);
                var templateTitle = this.generateContentTitle();
                var contentString = this.generateContentString();
                var truckTemplate = new InfoTemplate({
                    title: templateTitle,
                    content: contentString
                });

                self.graphic = new Graphic(point, marker, null, truckTemplate);
                this.graphicsLayer.add(self.graphic);
            }
        });
    });