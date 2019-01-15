(function() {
    'use strict';
    mtcApp.controller('shiftEntryController',
        function shiftEntryController($scope, $filter, $rootScope, $location, $routeParams, shiftEntryService, generalService, dispatchingService) {

            $scope.header = "Incident Entry";
            $scope.isBusy = false;
            $scope.contractorDrivers = [];
            $scope.contractorDriverID = null;
            $scope.Beats = [];
            $scope.BeatID = null;
            $scope.callSign = null;
            $scope.callSigns = [];
            $scope.truckNum = null;
            $scope.contractorTrucks = [];
            $scope.assists = [];
            $scope.assist;
            $scope.vehicleTypes = [];
            $scope.vehicleType = null;
            $scope.HWYS = [];
            $scope.HWY = null;
            $scope.actions = [];
            $scope.ten - 8;

            $scope.isBusyGettingCurrentUser = false;
            $scope.isBusyGettingDrivers = false;
            $scope.isBusyGettingVehicles = false;
            $scope.isBusyGettingBeats = false;
            $scope.isBusyGettingVehicleTypes = false;

            $(window).bind('beforeunload', function() {
                var assistnum = $('#tblAssists tr').length;
                if (assistnum > 1) {
                    return "Incident and Assists have not been saved. Refreshing will loose all data.";
                }
            });

            $scope.addAssist = function() {
                $('#edit').val('-1');
                $('#myModalLabel').text('Add Assist');
                $("#addEditAssist").modal('show');
            };

            //getContractorDrivers            
            $scope.isBusyGettingCurrentUser = true;
            generalService.getCurrentUser().then(function(results) {
                if (results) {
                    $scope.isBusyGettingCurrentUser = false;

                    var currentUserRole = results.SelectedRoleName;

                    if (currentUserRole === 'Admin' || currentUserRole === 'DataConsultant') {

                        $scope.isBusyGettingDrivers = true;
                        generalService.getDrivers().then(function(results2) {
                            for (var i = 0; i < results2.length; i++) {
                                $scope.contractorDrivers.push(results2[i]);
                            }
                            $scope.isBusyGettingDrivers = false;
                        });

                        $scope.isBusyGettingVehicles = true;
                        generalService.getVehicles().then(function(results3) {
                            for (var i = 0; i < results3.length; i++) {
                                $scope.contractorTrucks.push(results3[i]);
                            }
                            $scope.isBusyGettingVehicles = false;
                        });

                    } else {
                        if (results.ContractorId) {
                            $scope.isBusyGettingDrivers = true;
                            generalService.getContractorDrivers(results.ContractorId).then(function(results2) {
                                for (var i = 0; i < results2.length; i++) {
                                    $scope.contractorDrivers.push(results2[i]);
                                }
                                $scope.isBusyGettingDrivers = false;
                            });

                            $scope.isBusyGettingVehicles = true;
                            generalService.getContractorVehicles(results.ContractorId).then(function(results3) {
                                for (var i = 0; i < results3.length; i++) {
                                    $scope.contractorTrucks.push(results3[i]);
                                }
                                $scope.isBusyGettingVehicles = false;
                            });
                        }
                    }
                }

            });

            //getBeatNumbers
            $scope.isBusyGettingBeats = true;
            generalService.getBeatNumbers().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.Beats.push(results[i]);
                }
                $scope.isBusyGettingBeats = false;
            });

            //Get Vehicle Type for DropDowns
            $scope.isBusyGettingVehicleTypes = true;
            generalService.getVehicleTypes().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.vehicleTypes.push(results[i]);
                }
                $scope.isBusyGettingVehicleTypes = false;
            });

            //change Incident Type Label
            $('input:radio[name="IC"]').change(function() {
                $('#lblIC').text($(this).val());
            });

            //actions array change
            $(document).on("change", "input[name='AP']", function() {

                if (this.checked) {
                    $scope.actions.push($(this).val());
                } else {
                    var index = $scope.actions.indexOf($(this).val());
                    if (index > -1) {
                        $scope.actions.splice(index, 1);
                    }
                }

                if ($(this).val() !== "") {
                    //alert($(this).val());
                    $('#lblAPNONE').removeClass('active');
                    $('#APNONE').prop('checked', false);
                } else {
                    //remove alla cive classes
                    $('.Acode').removeClass('active');
                }
            });

            //Add assist to Array
            $scope.AddThisAssist = function() {
                $scope.isBusy = true;
                var am = {
                    //Time1097: $('#OnPatrol').val(),
                    //Time1098: $('#RollIn').val(),
                    IC: $("input:radio[name='IC']:checked").val() || "NULL",
                    TC: $("input:radio[name='TC']:checked", '#addEditAssistForm').val() || "NULL",
                    BD: $("input:radio[name='BD']:checked", '#addEditAssistForm').val() || "NULL",
                    DO: $("input:radio[name='DO']:checked", '#addEditAssistForm').val() || "NULL",
                    O: $("input:radio[name='O']:checked", '#addEditAssistForm').val() || "NULL",
                    AP: $scope.actions.toString(),
                    DIR: $("input:radio[name='DIR']:checked").val() || "NULL",
                    Highway: $('#Highway').val() || "NULL",
                    Area: $('#Area').val() || "NULL",
                    POS: $("input:radio[name='POS']:checked", '#addEditAssistForm').val() || "NULL",
                    PTN: $("input:radio[name='PTN']:checked", '#addEditAssistForm').val() || "NULL",
                    TransCode: $('#selTransCode').val() || "NULL",
                    VehType: $('#VehType').val() || "NULL",
                    LicPlateState: $('#LicPlateState').val() || "NULL",
                    LicPlateNum: $('#LicPlateNum').val() || "NULL",
                    CHPIncLogNum: $('#CHPIncLogNum').val() || "NULL",
                    DetailNote: $('#GenNotes').val() || "NULL",
                    TimeOnInc: $('#TimeOnInc').val() || "NULL",
                    TimeOffInc: $('#TimeOffInc').val() || "NULL",
                };

                //is edit
                if ($('#edit').val() === "-1") {
                    $scope.assists.push(am);
                } else {
                    $scope.assists[$('#edit').val()] = am;
                }

                localStorage.setItem("LSassists", JSON.stringify($scope.assists));
                $("#addEditAssist").modal('hide');
                $('#selTransCode').val('');
                $('#VehType').val('');
                $('#LicPlateState').val('CA');
                $('#LicPlateNum').val('');
                $('#GenNotes').val('');

                //remove alla live classes
                //$('.active').removeClass('active');

                //redo the NONE classes
                $('#ICNone').addClass('active');
                $('#IC_NA').prop('checked', true);
                $('#0VehLbl').addClass('active');
                $('#0Veh').prop('checked', true);
                $('#lblMECH').addClass('active');
                $('#NONEMECH').prop('checked', true);
                $('#lblO').addClass('active');
                $('#ONone').prop('checked', true);
                $('#lblAPNONE').addClass('active');
                $('#APNONE').prop('checked', true);
                $('#lblPOSNONE').addClass('active');
                $('#POSNONE').prop('checked', true);
                $('#lblPTNNONE').addClass('active');
                $('#PTNNONE').prop('checked', true);

                $('#lblIC').text('');
                updateFootable();
                $scope.actions = [];
                if ($('#edit').val() === "-1") {
                    alert('New assist was added');
                } else {
                    alert('Assist was edited');
                }
            };

            //Update footable after assista dded
            function updateFootable() {
                $("#tblAssists").remove();
                $('<table>').attr({ 'id': 'tblAssists', 'class': 'footable table table-hover toggle-circle col-lg-12' }).appendTo('#assistsDiv');
                $("#tblAssists").append(
                    "<thead>" +
                    "<tr>" +
                    "<th data-toggle='true' data-sort-initial='true'>CHP Incident Code</th>" +
                    "<th>Problem Code(s)</th>" +
                    "<th>Action Code(s)</th>" +
                    "<th>Dir.</th>" +
                    "<th>Hwy</th>" +
                    "<th>Area</th>" +
                    "<th>Edit</th>" +
                    "<th data-hide='all'>Time On Incident</th>" +
                    "<th data-hide='all'>Time Off Incident</th>" +
                    "<th data-hide='all'>Position</th>" +
                    "<th data-hide='all'>P/T/N</th>" +
                    "<th data-hide='all'>Transport Code</th>" +
                    "<th data-hide='all'>Vehicle Type</th>" +
                    "<th data-hide='all'>License Plate State</th>" +
                    "<th data-hide='all'>License Plate #</th>" +
                    "<th data-hide='all'>CHP Incident Log #</th>" +
                    "<th data-hide='all'>Assist Notes</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>"
                );
                for (var indx = 0; indx < $scope.assists.length; indx++) {
                    $("#tblAssists").append(
                        "<tr>" +
                        "<td ng-bind='assist.IC'>" + $scope.assists[indx].IC + "</td>" +
                        "<td ng-bind='assist.TC'>" + $scope.assists[indx].TC + ", " + $scope.assists[indx].BD + ", " + $scope.assists[indx].DO + ", " + $scope.assists[indx].O + "</td>" +
                        "<td ng-bind='assist.AP'>" + $scope.assists[indx].AP + "</td>" +
                        "<td ng-bind='assist.DIR'>" + $scope.assists[indx].DIR + "</td>" +
                        "<td ng-bind='assist.Highway'>" + $scope.assists[indx].Highway + "</td>" +
                        "<td ng-bind='assist.Area'>" + $scope.assists[indx].Area + "</td>" +
                        "<td><button type='button' class='btn btn-primary btn-sm edit' id='editAssist' value='" + indx + "'><i class='fa fa-edit'></i></button>  <button type='button' class='btn btn-danger btn-sm del' id='deleteAssist' value='" + indx + "'><i class='fa fa-trash'></i></button></td>" +
                        "<td ng-bind='assist.Time1097'>" + $scope.assists[indx].TimeOnInc + "</td>" +
                        "<td ng-bind='assist.Time1098'>" + $scope.assists[indx].TimeOffInc + "</td>" +
                        "<td ng-bind='assist.POS'>" + $scope.assists[indx].POS + "</td>" +
                        "<td ng-bind='assist.PTN'>" + $scope.assists[indx].PTN + "</td>" +
                        "<td ng-bind='assist.TransCode'>" + $scope.assists[indx].TransCode + "</td>" +
                        "<td ng-bind='assist.VehType'>" + $scope.assists[indx].VehType + "</td>" +
                        "<td ng-bind='assist.LicPlateState'>" + $scope.assists[indx].LicPlateState + "</td>" +
                        "<td ng-bind='assist.LicPlateNum'>" + $scope.assists[indx].LicPlateNum + "</td>" +
                        "<td ng-bind='assist.CHPIncLogNum'>" + $scope.assists[indx].CHPIncLogNum + "</td>" +
                        "<td ng-bind='assist.DetailNote'>" + $scope.assists[indx].DetailNote + "</td>" +
                        "</tr>"
                    );
                }
                $("#tblAssists").append("</tbody></table>");
                $('#tblAssists').footable();

                //attach click event to delete.
                $('#tblAssists').on('click', '.del', function() {
                    //alert(this.value);
                    if (confirm('Are you sure you wish to delete this assist?')) {
                        $scope.assists.splice(this.value, 1);
                        updateFootable();
                    }
                });

                //attach click event to edit. 
                $('#tblAssists').on('click', '.edit', function() {

                    var parsed = JSON.parse(localStorage.getItem("LSassists"));

                    var eassist = [];

                    for (var x in parsed) {
                        eassist.push(parsed[x]);
                    }

                    var assistEdit = eassist[$(this).val()];
                    $('#TimeOnInc').val(assistEdit.TimeOnInc);
                    $('#TimeOffINc').val(assistEdit.TimeOffInc);
                    //CHP Incident Code
                    if (assistEdit.IC === "Accident: No Details") {
                        $('#IC_11-83').parent().addClass('active');
                        $('#IC_11-83').prop("checked", true);
                        $('#lblIC').text(assistEdit.IC);
                    };
                    if (assistEdit.IC === "Accident: Property Damage") {
                        $('#IC_11-82').parent().addClass('active');
                        $('#IC_11-82').prop("checked", true);
                        $('#lblIC').text(assistEdit.IC);
                    };
                    if (assistEdit.IC === "Abandoned Vehicle") {
                        $('#IC_11-24').parent().addClass('active');
                        $('#IC_11-24').prop("checked", true);
                        $('#lblIC').text(assistEdit.IC);
                    };
                    if (assistEdit.IC === "Disabled Vehicle: Occupied") {
                        $('#IC_11-26').parent().addClass('active');
                        $('#IC_11-26').prop("checked", true);
                        $('#lblIC').text(assistEdit.IC);
                    };
                    if (assistEdit.IC === "Traffic Hazard") {
                        $('#IC_11-25').parent().addClass('active');
                        $('#IC_11-25').prop("checked", true);
                        $('#lblIC').text(assistEdit.IC);
                    };
                    //PC Traffic Collision  
                    if (assistEdit.TC === "1VEH") {
                        $('#1VEH').parent().addClass('active');
                        $('#1VEH').prop("checked", true);
                    };
                    if (assistEdit.TC === "2VEH") {
                        $('#2VEH').parent().addClass('active');
                        $('#2VEH').prop("checked", true);
                    };
                    if (assistEdit.TC === "3VEH") {
                        $('#3VEH').parent().addClass('active');
                        $('#3VEH').prop("checked", true);
                    };
                    //PC Breakdown  
                    if (assistEdit.BD === "BATT") {
                        $('#BATT').parent().addClass('active');
                        $('#BATT').prop("checked", true);
                    };
                    if (assistEdit.BD === "TIRE") {
                        $('#TIRE').parent().addClass('active');
                        $('#TIRE').prop("checked", true);
                    };
                    if (assistEdit.BD === "GAS") {
                        $('#GAS').parent().addClass('active');
                        $('#GAS').prop("checked", true);
                    };
                    if (assistEdit.BD === "ABAN") {
                        $('#ABAN').parent().addClass('active');
                        $('#ABAN').prop("checked", true);
                    };
                    if (assistEdit.BD === "HEAT") {
                        $('#HEAT').parent().addClass('active');
                        $('#HEAT').prop("checked", true);
                    };
                    if (assistEdit.BD === "MECH") {
                        $('#MECH').parent().addClass('active');
                        $('#MECH').prop("checked", true);
                    };
                    //PC Debri Only  
                    if (assistEdit.DO === "DIL") {
                        $('#DIL').parent().addClass('active');
                        $('#DIL').prop("checked", true);
                    };
                    if (assistEdit.DO === "DOS") {
                        $('#DOS').parent().addClass('active');
                        $('#DOS').prop("checked", true);
                    };
                    //PC Other  
                    if (assistEdit.O === "SMNA") {
                        $('#SMNA').parent().addClass('active');
                        $('#SMNA').prop("checked", true);
                    };
                    if (assistEdit.O === "Other") {
                        $('#Other').parent().addClass('active');
                        $('#Other').prop("checked", true);
                    };
                    //Action Codes
                    if (assistEdit.AP === "ASST") {
                        $('#ASST').parent().addClass('active');
                        $('#ASST').prop("checked", true);
                    };
                    if (assistEdit.AP === "PART") {
                        $('#PART').parent().addClass('active');
                        $('#PART').prop("checked", true);
                    };
                    if (assistEdit.AP === "OCFR") {
                        $('#OCFR').parent().addClass('active');
                        $('#OCFR').prop("checked", true);
                    };
                    if (assistEdit.AP === "RFSD") {
                        $('#RFSD').parent().addClass('active');
                        $('#RFSD').prop("checked", true);
                    };
                    if (assistEdit.AP === "HLPW") {
                        $('#HLPW').parent().addClass('active');
                        $('#HLPW').prop("checked", true);
                    };
                    if (assistEdit.AP === "HLPN") {
                        $('#HLPN').parent().addClass('active');
                        $('#HLPN').prop("checked", true);
                    };
                    if (assistEdit.AP === "DOFF") {
                        $('#DOFF').parent().addClass('active');
                        $('#DOFF').prop("checked", true);
                    };
                    if (assistEdit.AP === "OTHER") {
                        $('#OTHER').parent().addClass('active');
                        $('#OTHER').prop("checked", true);
                    };
                    //Direction
                    if (assistEdit.DIR === "NB") {
                        $('#NB').parent().addClass('active');
                        $('#NB').prop("checked", true);
                    };
                    if (assistEdit.DIR === "SB") {
                        $('#SB').parent().addClass('active');
                        $('#SB').prop("checked", true);
                    };
                    if (assistEdit.DIR === "EB") {
                        $('#EB').parent().addClass('active');
                        $('#EB').prop("checked", true);
                    };
                    if (assistEdit.DIR === "WB") {
                        $('#WB').parent().addClass('active');
                        $('#WB').prop("checked", true);
                    };
                    //freeway
                    $('#Highway').val(assistEdit.Highway);
                    //Area
                    $('#Area').val(assistEdit.Area);
                    //Position
                    if (assistEdit.POS === "RS") {
                        $('#RS').parent().addClass('active');
                        $('#RS').prop("checked", true);
                    };
                    if (assistEdit.POS === "CD") {
                        $('#RS').parent().addClass('active');
                        $('#RS').prop("checked", true);
                    };
                    if (assistEdit.POS === "RAMP") {
                        $('#RS').parent().addClass('active');
                        $('#RS').prop("checked", true);
                    };
                    //PTN
                    if (assistEdit.PTN === "P") {
                        $('#P').parent().addClass('active');
                        $('#P').prop("checked", true);
                    };
                    if (assistEdit.PTN === "T") {
                        $('#T').parent().addClass('active');
                        $('#T').prop("checked", true);
                    };
                    if (assistEdit.PTN === "N") {
                        $('#N').parent().addClass('active');
                        $('#N').prop("checked", true);
                    };
                    //Transport Code
                    $('#selTransCode').val(assistEdit.TransCode);
                    //Vehicle Type
                    $("#VehType > option").each(function() {
                        //alert(this.text + ' ' + this.value);
                        if (this.text.indexOf(assistEdit.VehType) >= 0) {
                            $(this).prop('selected', true);
                        };
                    });
                    //License Plate State
                    $('#LicPlateState').val(assistEdit.LicPlateState);
                    //License Plate Number
                    $('#LicPlateNum').val(assistEdit.LicPlateNum);
                    //License Plate State
                    $('#CHPIncLogNum').val(assistEdit.CHPIncLogNum);

                    $('#edit').val($(this).val());
                    $('#myModalLabel').text('Edit Assist');
                    $("#addEditAssist").modal('show');
                });
            };

            //AddIncidentToDB
            $scope.addIncident = function() {
                $scope.isBusy = true;

                var validated = 1;
                var message = null;
                var rowCount = $('#tblAssists tr').length;

                if ($('#Driver').val() === "") {
                    validated = 0;
                    message = 'Please enter a driver.';
                } else if ($('#Beats').val() === "") {
                    validated = 0;
                    message = 'Please enter a Beat.';
                } else if ($('#CallSign').val() === "") {
                    validated = 0;
                    message = 'Please enter a CallSign.';
                } else if ($('#TruckNum').val() === "") {
                    validated = 0;
                    message = 'Please enter a Truck.';
                } else if ($('#OnPatrol').val() === "") {
                    validated = 0;
                    message = 'Please enter an On Patrol Time.';
                } else if ($('#LogOnOd').val() === "") {
                    validated = 0;
                    message = 'Please enter a Log On Odometer Reading.';
                } else if ($('#RollIn').val() === "") {
                    validated = 0;
                    message = 'Please enter an Roll In Time.';
                } else if ($('#LogOffOd').val() === "") {
                    validated = 0;
                    message = 'Please enter a Log Off Odometer Reading.';
                } else if (rowCount <= 1) {
                    validated = 0;
                    message = 'Please enter an assist for this incident.';
                }

                //else if ($('#CHPIncLogNum').val() == "") {
                //    validated = 0;
                //    message = 'Please enter a CHP Incident Log Number.';
                //}

                if (validated === 0) {
                    alert(message);
                    $scope.isBusy = false;
                } else {
                    var incident = {
                        Id: "",
                        Date: $('#Date').val(),
                        Driver: $('#Driver').val(),
                        Beat: $('#Beats').find(':selected').text().toString(),
                        CallSign: $('#CallSign').val(),
                        Truck: $('#TruckNum').find(':selected').text().toString(),
                        OnPatrol: $('#OnPatrol').val(),
                        RollIn: $('#RollIn').val(),
                        LogOnOd: $('#LogOnOd').val(),
                        LogOffOd: $('LogOffOd').val(),
                        Notes: $('#GenNotes').val(),
                        CHPTOTLogNum: $('#OTLogNum').val(),
                        DropSiteNum: $('#DSNum').val(),
                        IncidentSurvNum: $('#IncSurveyNum').val(),
                        Assists: localStorage.getItem("LSassists")
                    };

                    shiftEntryService.saveIncident(incident).then(function(result) {
                        alert('Incident Saved');
                        $("#tblAssists").remove();
                        //empty assists
                        localStorage.removeItem(key);
                        ("LSassists");
                        updateFootable();
                    });

                    $scope.isBusy = false;
                }

            };

            //On beats select change get callsigns
            $scope.getCallSigns = function() {
                $scope.isBusy = true;
                var beat = $('#Beats').find(':selected').text().toString();
                $('#CallSign').empty();
                var _selCode = '';
                shiftEntryService.getCallSigns(beat).then(function(results) {
                    if (results) {
                        for (var i = 0; i < results.length; i++) {
                            _selCode += '<option id="' + results[i].Text + '">' + results[i].Text + '</option>';
                        }
                        $('#CallSign').append(_selCode);
                        $('#CallSign').prop('disabled', false);
                    } else {
                        $scope.header = 'Error. We were not able to retrieve your callsigns. Please come back later.';
                    }
                });
                $scope.isBusy = false;
            };

            //On beats select change get freeways
            $scope.getFreeways = function() {
                $scope.isBusy = true;
                var beat = $('#Beats').find(':selected').val().toString();
                $('#Highway').empty();
                var _selCode = '';
                if (beat !== "") {
                    generalService.getBeatsFreewaysByBeat(beat).then(function(results) {
                        if (results) {
                            for (var i = 0; i < results.length; i++) {
                                _selCode += '<option id="' + results[i].Id + '">' + results[i].Text + '</option>';
                            }
                            $('#Highway').append(_selCode);
                            $('#Highway').prop('disabled', false);
                        } else {
                            $scope.header = 'Error. We were not able to retrieve your callsigns. Please come back later.';
                        }
                        $('#Highway').prop('disabled', false);
                    });
                }
                $scope.isBusy = false;
            };

            //print page
            $scope.print = function() {
                window.print();
            };
        }
    );
}());