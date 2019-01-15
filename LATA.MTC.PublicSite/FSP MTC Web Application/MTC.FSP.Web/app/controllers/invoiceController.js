(function () {
    'use strict';
    mtcApp.controller('invoiceController',
        function invoiceController($scope, $filter, $interval, $rootScope, $location, $routeParams, invoiceService, generalService) {
            //refresh looses everything
            $(window).bind('beforeunload', function () {
                return "Refreshing will erase all un-saved invoice data.";
            });

            //Hide all divs
            $('#Month').hide();
            $('#Beat').hide();
            $('#Contractors').hide();
            $('#choices').hide();
            $('#invoiceDiv').hide();
            $('#IE').hide();

            $scope.header = 'Monthly Invoice';
            $scope.isBusy = false;
            $scope.isBuilding = false;
            var TotalContractHours = 0.00;
            var BasePay = 0.00;
            var totalAdditions = 0.00;
            var totalDeductions = 0.00;
            $scope.InvoiceTotal = 0.00;
            $scope.Notes;

            //Contractor Info
            $scope.contractnumber;
            $scope.contractexp;
            $scope.companyname;
            $scope.address1;
            $scope.city;
            $scope.state;
            $scope.zip;
            $scope.contact;
            $scope.email;
            $scope.phone;
            $scope.invoiceNumber;
            $scope.MTCAddress;

            //Fill Month DropDown
            $scope._month;
            $scope.month;
            $scope.months = [];
            $scope.monthsArray = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
            $scope.fillMonthDropDown = function () {
                $scope.isBusy = true;
                invoiceService.fillMonthDropDown().then(function (results) {
                    if (results) {
                        for (var i = 0; i < results.length; i++) {
                            $scope.months.push(results[i]);
                        }

                        $('#Month').show("slideIn");
                    }
                });

                $scope.isBusy = false;
            };
            $scope.fillMonthDropDown();

            //Fill Beat Dropdown
            $scope._beat;
            $scope.beatNum;
            $scope.Beats = [];
            $scope.fillBeatDropDown = function () {
                $scope.isBusy = true;
                generalService.getBeatNumbers().then(function (results) {
                    if (results) {
                        for (var i = 0; i < results.length; i++) {
                            $scope.Beats.push(results[i]);
                        }
                        $scope.Beats.sort(function (a, b) { return a - b });
                        $scope.isBusy = false;
                        $('#Beat').show("slideIn");
                    }
                });

            };
            $scope.fillBeatDropDown();

            //Fill Contractors Dropdown
            $scope.contractorid;
            $scope.Contractors = [];

            //additions info
            $scope.addition;
            $scope.additions = [];
            $scope.TotalAdditions = 0.00;
            $scope.TotalTimeAdded = 0.00;
            $scope.category;
            $scope.date;
            $scope.description; 
            $scope.timeadded;
            $scope.rate;
            $scope.cost;

            //deductions info
            $scope.deduction;
            $scope.deductions = [];
            $scope.TotalDeductions = 0.00;
            $scope.TotalDedTimeAdded = 0.00;

            //Anamolies Info
            $scope.anamolies = [];
            $scope.anamolieaddressed = 'N/A';

            //Month picked
            $(document.body).on('click', '.month', function (event) {
                //alert($(this).text());

                $('#invoiceDiv').hide("slideOut");
                $('#MonthDDText').text($(this).text());
                $scope.month = $(this).text();
                $scope._month = $scope.monthsArray.indexOf($(this).text()) + 1;
            });

            //Beat Picked
            $(document.body).on('click', '.beat', function (event) {
                //alert($(this).attr('id'));

                $scope.isBusy = true;
                $('#invoiceDiv').hide("slideOut");
                $('#Contractors').hide("slideOut");
                $('#choices').hide("slideOut");

                $('#BeatDDText').text($(this).text());
                $scope._beat = $(this).attr('id');
                $scope.beatNum = $(this).text();

                $scope.Contractors = [];
                

                invoiceService.getBeatContractors($scope._beat).then(function (results) {
                    if (results.length > 0) {
                        for (var i = 0; i < results.length; i++) {
                            $scope.Contractors.push(results[i]);
                        }

                        if (results.length > 1) {
                            $('#ContractorText').text('Choose Contractor');
                            $('#Contractors').show("slideIn");
                            $scope.isBusy = false;
                        } else {
                            $('#ContractorText').text($scope.Contractors[0].ContractCompanyName);
                            $('#Contractors').show("slideIn");
                            $('#choices').show("slideIn");
                            $scope.contractorid = $scope.Contractors[0].ContractorID;
                            //$scope.loadInvoice();
                            $scope.isBusy = false;
                        }
                    } else {
                        alert('No Contractor was found assigned to the chosen beat. Please choose another beat.');
                        $scope.isBusy = false;
                    }
                });
            });

            //Contractor Picked
            $(document.body).on('click', '.contractor', function (event) {
                //alert($(this).attr('id'));

                $('#contractor').text($(this).text());
                $scope.contractorid = $(this).attr('id');

                //alert($('#choices').is(':hidden'));
                if ($('#choices').is(':hidden')) {
                    $('#choices').show("slideIn");
                }
            });

            //LOAD INVOICE
            $scope.loadInvoice = function () {
                //alert($scope.contractorid);

                $scope.isBusy = true;
                $scope.isBuilding = true;
                $scope.deductions = [];
                $scope.TotalDeductions = 0.00;
                $scope.additions = [];
                $scope.TotalAdditions = 0.00;
                totalDeductions = 0.00;
                totalAdditions = 0.00;
                $scope.TotalTimeAdded = 0.00;
                $scope.TotalTimeDeducted = 0.00;

                if ($scope._month == null || $scope._beat == null) {
                    alert("Please choose and month and beat");
                    $scope.isBusy = false;
                } else {
                    $('#invoiceDiv').hide("fadeOut");

                    //Step 1 get contractor information
                    invoiceService.getBeatContractorInfo($scope._beat, $scope._month, $scope.contractorid).then(function (results) {
                        if (results) {
                            $scope.contractnumber = results.contractnumber;
                            $scope.contractexp = results.contractexp;
                            $scope.contractbeg = results.contractbeg;
                            $scope.companyname = results.companyname;
                            $scope.address1 = results.address1;
                            $scope.city = results.city;
                            $scope.state = results.state;
                            $scope.zip = results.zip;
                            $scope.contact = results.contact;
                            $scope.email = results.email;
                            $scope.phone = results.phone;
                            $scope.invoiceNumber = results.invoiceNumber;
                            $scope.MTCAddress = results.MTCAddress;

                            //Move progress bar
                            $(".progress-bar").css("width", "25%");
                            $(".progress-bar").html("Building Month's Base")

                            //Step 2 get summary
                            invoiceService.getServiceSummary2($scope._beat, $scope._month, $scope.contractorid).then(function (results2) {
                                if (results2) {
                                    $scope.TotalWeekdays = results2.TotalWeekDaysInMonth;
                                    $scope.TotalSaturdays = results2.TotalSaturDaysInMonth;
                                    $scope.TotalSundays = results2.TotalSunDaysInMonth;
                                    $scope.TotalHolidays = results2.TotalHolidaysInMonth;
                                    $scope.Holidays = "";
                                    if (results2.HolidaysInMonth.length > 0) {
                                        $scope.Holidays = " (";

                                        for (var i = 0; i < results2.HolidaysInMonth.length; i++) {
                                            var parsedDate = new Date(parseInt(results2.HolidaysInMonth[i].substr(6)))
                                            $scope.Holidays += parsedDate.toLocaleDateString()
                                            if (i < results2.HolidaysInMonth.length - 1) { $scope.Holidays += ", "; }
                                        }
                                        $scope.Holidays += ")";
                                    }
                                    $scope.TotalCustomDays = results2.TotalCustomDaysInMonth;
                                    $scope.CustomDaysInMonth = "";
                                    if (results2.CustomDaysInMonth.length > 0) {
                                        $scope.CustomDaysInMonth = " (";

                                        for (var i = 0; i < results2.CustomDaysInMonth.length; i++) {
                                            var parsedDate = new Date(parseInt(results2.CustomDaysInMonth[i].substr(6)))
                                            $scope.CustomDaysInMonth += parsedDate.toLocaleDateString()
                                            if (i < results2.CustomDaysInMonth.length - 1) { $scope.CustomDaysInMonth += ", "; }
                                        }
                                        $scope.CustomDaysInMonth += ")";
                                    }

                                    //Weekdays
                                    $scope.WeekDays = results2.TotalWeekDaysWorkedInMonth;
                                    $scope.WeekShifts = results2.TotalWeekdaysWorkedShifts;
                                    $scope.WeekContractHours = results2.TotalWeekDaysWorkedHours;
                                    $scope.WeekOnPatrolHours = results2.TotalWeekDaysWorkedHours;

                                    //Saturdays
                                    $scope.SatDays = results2.TotalSaturDaysWorkedInMonth;
                                    $scope.SatShifts = results2.TotalSaturDaysWorkedShifts;
                                    $scope.SatContractHours = results2.TotalSaturDaysWorkedHours;
                                    $scope.SatOnPatrolHours = results2.TotalSaturDaysWorkedHours;

                                    //Sundays
                                    $scope.SunDays = results2.TotalSunDaysWorkedInMonth;
                                    $scope.SunShifts = results2.TotalSunDaysWorkedShifts;
                                    $scope.SunContractHours = results2.TotalSunDaysWorkedHours;
                                    $scope.SunOnPatrolHours = results2.TotalSunDaysWorkedHours;

                                    //Holidays
                                    $scope.HolDays = results2.TotalHolidaysWorkedInMonth;
                                    $scope.HolShifts = results2.TotalHolidaysWorkedShifts;
                                    $scope.HolContractHours = results2.TotalHolidaysWorkedHours;
                                    $scope.HolOnPatrolHours = results2.TotalHolidaysWorkedHours;

                                    //Custom Days
                                    $scope.CustomDays = results2.TotalCustomDaysWorkedInMonth;
                                    $scope.CustomShifts = results2.TotalCustomDaysWorkedShifts;
                                    $scope.CustomContractHours = results2.TotalCustomDaysWorkedHours;
                                    $scope.CustomOnPatrolHours = results2.TotalCustomDaysWorkedHours;

                                    //Totals 
                                    $scope.TotalDays = results2.TotalDays;
                                    $scope.TotalShifts = results2.TotalShifts;
                                    $scope.TotalContractHours = results2.TotalHoursWorked;
                                    TotalContractHours = results2.TotalHoursWorked;
                                    $scope.TotalOnPatrolHours = results2.TotalHoursWorked;

                                    //move progress bar
                                    $(".progress-bar").css("width", "45%");
                                    $(".progress-bar").html("Retreiving Base Rate and Pay")

                                    //Step 3 get PayRate and FuelRate
                                    invoiceService.getBaseRate($scope._beat).then(function (results3) {
                                        if (results3) {
                                            $scope.BaseRate = parseFloat(results3.PayRate).toFixed(2);
                                            $scope.FuelRate = parseFloat(results3.FuelRate).toFixed(2);
                                            $scope.CMBasePay = parseFloat($scope.BaseRate * TotalContractHours).toFixed(2);
                                            BasePay = parseFloat($scope.BaseRate * TotalContractHours).toFixed(2);

                                            //move progress bar
                                            $(".progress-bar").css("width", "65%");
                                            $(".progress-bar").html("Finding Additions")

                                            //Step 4: Get Additions
                                            invoiceService.getInvoiceAdditions($scope._beat, $scope._month, $scope.contractorid).then(function (AdditionResults) {
                                                if (AdditionResults) {
                                                    for (var i = 0; i < AdditionResults.length; i++) {
                                                        AdditionResults[i].Cost = parseFloat(AdditionResults[i].Cost).toFixed(2);
                                                        $scope.TotalAdditions += parseFloat(AdditionResults[i].Cost);
                                                        $scope.TotalTimeAdded += AdditionResults[i].TimeAdded;
                                                        totalAdditions += parseFloat(AdditionResults[i].Cost);;
                                                        $scope.additions.push(AdditionResults[i]);

                                                        if (AdditionResults[i].category == "Overtime" || AdditionResults[i].category == "Backup Resolved") {
                                                            if (AdditionResults[i].shiftDay == "WEEKDAY") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "SUN") {
                                                                $scope.SunOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "MON") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "TUE") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "WED") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "THU") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "FRI") {
                                                                $scope.WeekOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "SAT") {
                                                                $scope.SatOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "CUS") {
                                                                $scope.CustomOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            } else if (AdditionResults[i].shiftDay == "HOL") {
                                                                $scope.HolOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                                $scope.TotalOnPatrolHours += (AdditionResults[i].TimeAdded / 4);
                                                            }
                                                        }
                                                    }

                                                    //sort additions by date
                                                    $scope.additions.sort(function (a, b) {
                                                        return new Date(b.date) - new Date(a.date);
                                                    });

                                                    //refresh additions table
                                                    var additionsTable = $('#additionsTable');
                                                    additionsTable.footable();
                                                    additionsTable.trigger('footable_initialize'); //Initialize the table
                                                    additionsTable.trigger('footable_redraw'); //Redraw the table

                                                    //move progress bar
                                                    $(".progress-bar").css("width", "80%");
                                                    $(".progress-bar").html("Finding Deductions")

                                                    //Step 5: Get Deductions
                                                    invoiceService.getInvoiceDeductions($scope._beat, $scope._month, $scope.contractorid).then(function (DeductionsResults) {
                                                        if (DeductionsResults) {
                                                            for (var i = 0; i < DeductionsResults.length; i++) {
                                                                if (DeductionsResults[i].Rate == 0.0062) {
                                                                    DeductionsResults[i].Rate = "Supplies";
                                                                } else if (DeductionsResults[i].Rate == 0.0033) {
                                                                    DeductionsResults[i].Rate = "Fine";
                                                                } else {
                                                                    DeductionsResults[i].Rate = "$" + parseFloat(DeductionsResults[i].Rate).toFixed(2);
                                                                }
                                                                DeductionsResults[i].Cost = parseFloat(DeductionsResults[i].Cost).toFixed(2);
                                                                totalDeductions += parseFloat(DeductionsResults[i].Cost);
                                                                $scope.TotalDedTimeAdded += DeductionsResults[i].TimeAdded;
                                                                $scope.deductions.push(DeductionsResults[i]);

                                                                if (DeductionsResults[i].category == "Overtime" || DeductionsResults[i].category == "Backup Resolved") {
                                                                    if (DeductionsResults[i].shiftDay == "WEEKDAY") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "SUN") {
                                                                        $scope.SunOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "MON") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "TUE") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "WED") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "THU") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "FRI") {
                                                                        $scope.WeekOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "SAT") {
                                                                        $scope.SatOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "CUS") {
                                                                        $scope.CustomOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    } else if (DeductionsResults[i].shiftDay == "HOL") {
                                                                        $scope.HolOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                        $scope.TotalOnPatrolHours -= (DeductionsResults[i].TimeAdded / 4);
                                                                    }
                                                                }
                                                            }

                                                            //sort deductions by date
                                                            $scope.deductions.sort(function (a, b) {
                                                                return new Date(b.date) - new Date(a.date);
                                                            });

                                                            $scope.TotalDeductions = parseFloat(totalDeductions).toFixed(2);

                                                            //move progress bar
                                                            $(".progress-bar").css("width", "90%");
                                                            $(".progress-bar").html("Finding Anomalies")

                                                            $scope.InvoiceTotal = Number(Number(BasePay) + Number(totalAdditions) - Number(totalDeductions)).toFixed(2);

                                                            //Step 6: Getting Anamolies
                                                            invoiceService.getInvoiceAnamolies($scope._beat, $scope._month, $scope.contractorid).then(function (Anamolies) {
                                                                if (Anamolies) { 
                                                                    $scope.anamolies = Anamolies;

                                                                    //refresh table
                                                                    var anamoliesTable = $('#anamoliesTable');
                                                                    anamoliesTable.footable();
                                                                    anamoliesTable.trigger('footable_initialize '); //Initialize the table
                                                                    anamoliesTable.trigger('footable_redraw'); //Redraw the table

                                                                    //move progress bar
                                                                    $(".progress-bar").css("width", "100%");
                                                                    $(".progress-bar").html("Invoice Compelted")

                                                                    $scope.isBusy = false;
                                                                    $scope.isBuilding = false;
                                                                    $('#invoiceDiv').show("fadeIn");
                                                                    $(".progress-bar").css("width", "5%");
                                                                } else {
                                                                    $('#MonthDDText').text('Choose Month');
                                                                    $('#BeatDDText').text('Choose Beat');
                                                                    $scope.isBusy = false;
                                                                    alert("No Invoice Was Found. Please try again.");
                                                                }
                                                            });
                                                        } else {
                                                            $('#MonthDDText').text('Choose Month');
                                                            $('#BeatDDText').text('Choose Beat');
                                                            $scope.isBusy = false;
                                                            alert("No Invoice Was Found. Please try again.");
                                                        }
                                                    });

                                                } else {
                                                    $('#MonthDDText').text('Choose Month');
                                                    $('#BeatDDText').text('Choose Beat');
                                                    $scope.isBusy = false;
                                                    alert("No Invoice Was Found. Please try again.");
                                                }
                                            });
                                        } else {
                                            $('#MonthDDText').text('Choose Month');
                                            $('#BeatDDText').text('Choose Beat');
                                            $scope.isBusy = false;
                                            alert("No Invoice Was Found. Please try again.");
                                        }
                                    });
                                } else {
                                    $('#MonthDDText').text('Choose Month');
                                    $('#BeatDDText').text('Choose Beat');
                                    $scope.isBusy = false;
                                    alert("No Invoice Was Found. Please try again.");
                                }
                            });
                        } else {
                            $('#MonthDDText').text('Choose Month');
                            $('#BeatDDText').text('Choose Beat');
                            $scope.isBusy = false;
                            alert("No Invoice Was Found. Please try again.");
                        }
                    });
                }
            }; 

            //Deduction Costs are updated
            $scope.updateDeductsTotals = function (id) {
                //Change the array item cost
                var cost = $('#' + id).val();
                for (var i = 0; i <= $scope.deductions.length - 1; i++) {
                    if ($scope.deductions[i].id == id) {
                        $scope.deductions[i].Cost = cost;
                    }
                }

                //total it up
                $scope.TotalDeductions = 0.00;
                totalDeductions = 0.00;

                $('.deducts').each(function (i, obj) {

                    if ($(this).val() != 0) {
                        if (parseFloat($(this).val())) {
                            $scope.TotalDeductions += parseFloat($(this).val());
                            totalDeductions += parseFloat($(this).val());
                        } else {
                            alert('Deduction Costs Can Only Be Money Amounts.')
                        }
                    }
                });

                //refresh totals
                $scope.InvoiceTotal = Number(Number(BasePay) + Number(totalAdditions) - Number(totalDeductions)).toFixed(2);
            }

            //Open addition add modal
            $('#openAdditionModal').click(function (e) {
                $('#additionModal').modal("show");
            });

            //Open deduction add modal
            $('#openDeductionModal').click(function (e) {
                $('#deductionModal').modal("show");
            });

            //Open deduction add modal
            $(document.body).on('click', '.createDeductionModal', function (event) {
                $scope.anamolieaddressed = this.id;
                $('#deductionModal').modal("show");
            });

            //Add Addition
            $('#additionForm').submit(function (e) {
                totalAdditions = 0.00;
                $scope.TotalAdditions = 0.00;
                $scope.TotalTimeAdded = 0.00;

                var $form = $(this);
                
                if ($form.valid()) {

                    if ($scope.MENum != null) {
                        for (var i = 0; i <= $scope.additions.length - 1; i++) {
                            if ($scope.additions[i].MENum == $scope.MENum) {
                                $scope.additions.splice(i, 1);
                            }
                        }
                    };

                    var addition = {
                        "category": $scope.category,
                        "date": new Date($scope.date).toLocaleDateString(),
                        "description": $scope.description,
                        "id": Math.floor(Math.random() * 256),
                        "TimeAdded": $scope.timeadded,
                        "Rate": $scope.rate,
                        "Cost": $scope.cost,
                        "MENum": Math.floor(Math.random() * 256)
                    };

                    $scope.additions.push(addition);
                    //Sort the additions array
                    $scope.additions.sort(function (a, b) {
                        return new Date(a.date) - new Date(b.date);
                    });
                    for (var i = 0; i < $scope.additions.length; i++) {
                        $scope.additions[i].Cost = parseFloat($scope.additions[i].Cost).toFixed(2);
                        $scope.TotalAdditions += parseFloat($scope.additions[i].Cost);
                        $scope.TotalTimeAdded += parseFloat($scope.additions[i].TimeAdded);
                        totalAdditions += parseFloat($scope.additions[i].Cost);
                    }

                    //refresh totals
                    $scope.InvoiceTotal = Number(Number(BasePay) + Number(totalAdditions) - Number(totalDeductions)).toFixed(2);

                    //refresh table
                    //var additionsTable = $('#additionsTable');
                    //additionsTable.footable();
                    //additionsTable.trigger('footable_initialize'); //Reinitialize
                    //additionsTable.trigger('footable_redraw'); //Redraw the table
                    //additionsTable.trigger('footable_resize'); //Resize the table

                    $('#additionModal').modal("hide");

                    $scope.category = null;
                    $scope.date = null;
                    $scope.description = null;
                    $scope.timeadded = null;
                    $scope.rate = null;
                    $scope.cost = null;
                } else {
                    e.preventDefault();
                }
            });

            //Delete Addition
            $scope.deleteAddition = function (ID) {
                //alert("Delete: " + MENum);
                for(var i = 0; i <= $scope.additions.length - 1; i++)
                {
                    if ($scope.additions[i].id == ID) {
                        $scope.TotalAdditions -= parseFloat($scope.additions[i].Cost);
                        totalAdditions -= parseFloat($scope.additions[i].Cost);
                        $scope.TotalTimeAdded -= parseFloat($scope.additions[i].TimeAdded);
                        $scope.additions.splice(i, 1);
                    }
                }
            };

            //Modify Addition
            $scope.modifyAddition = function (MENum ) {
                for (var i = 0; i <= $scope.additions.length - 1; i++) {
                    if ($scope.additions[i].MENum == MENum) {
                        $scope.category = $scope.additions[i].category;
                        $scope.date = $scope.additions[i].date;
                        $scope.description = $scope.additions[i].description;
                        $scope.timeadded = $scope.additions[i].TimeAdded;
                        $scope.rate = $scope.additions[i].Rate;
                        $scope.cost = $scope.additions[i].Cost;
                        $scope.MENum = $scope.additions[i].MENum;

                        $('#additionModal').modal("show");
                    }
                }
            };

            //Add Deduction
            $('#deductionForm').submit(function (e) {
                totalDeductions = 0.00;
                $scope.TotalDeductions = 0.00;
                $scope.TotalDedTimeAdded = 0.00;

                var $form = $(this);

                if ($form.valid()) {
                    if ($scope.MENum != null) {
                        for (var i = 0; i <= $scope.deductions.length - 1; i++) {
                            if ($scope.deductions[i].MENum == $scope.MENum) {
                              $scope.deductions.splice(i, 1);
                            }
                        }
                    };

                    var deduction = {
                        "category": $scope.category,
                        "date": new Date($scope.date).toLocaleDateString(),
                        "description": $scope.description,
                        "id": Math.floor(Math.random() * 256),
                        "TimeAdded": $scope.timeadded,
                        "Rate": $scope.rate,
                        "Cost": $scope.cost,
                        "MENum": Math.floor(Math.random() * 256)
                    };

                    $scope.deductions.push(deduction);
                    //Sort the deductions array
                    $scope.deductions.sort(function (a, b) {
                        return new Date(a.date) - new Date(b.date);
                    });

                    for (var i = 0; i < $scope.deductions.length; i++) {
                        $scope.deductions[i].Cost = parseFloat($scope.deductions[i].Cost).toFixed(2);
                        $scope.TotalDeductions += parseFloat($scope.deductions[i].Cost);
                        $scope.TotalDedTimeAdded += parseFloat($scope.deductions[i].TimeAdded);
                        totalDeductions += parseFloat($scope.deductions[i].Cost);
                    }

                    //refresh totals
                    $scope.InvoiceTotal = Number(Number(BasePay) + Number(totalAdditions) - Number(totalDeductions)).toFixed(2);

                    //refresh table
                    //var deductionsTable = $('#deductionsTable');
                    //deductionsTable.footable();
                    //deductionsTable.trigger('footable_initialize'); //Reinitialize
                    //deductionsTable.trigger('footable_redraw'); //Redraw the table
                    //deductionsTable.trigger('footable_resize'); //Resize the table

                    //hidemodal
                    $('#deductionModal').modal("hide");

                    //redo inputs
                    $scope.category = null;
                    $scope.date = null;
                    $scope.description = null;
                    $scope.timeadded = null;
                    $scope.rate = null;
                    $scope.cost = null;

                    //disable anomolie button if exists
                    if ($scope.anamolieaddressed != 'N/A') {
                        var btnName = '#' + $scope.anamolieaddressed;
                        $(btnName).text('Deduction Created');
                        $(btnName).prop('disabled', true);
                        $scope.anamolieaddressed = 'N/A';
                    }
                } else {
                    e.preventDefault();
                }
            });

            //Delete Deduction
            $scope.deleteDeduction = function (ID) {
                for (var i = 0; i <= $scope.deductions.length - 1; i++) {
                    if ($scope.deductions[i].id == ID) {
                        $scope.TotalDeductions -= parseFloat($scope.deductions[i].Cost);
                        totalDeductions -= parseFloat($scope.deductions[i].Cost);
                        $scope.TotalDedTimeAdded -= parseFloat($scope.deductions[i].TimeAdded);
                        $scope.deductions.splice(i, 1);
                    }
                }
            };

            //Modify Deduction
            $scope.modifyDeduction = function (MENum) {
                for (var i = 0; i <= $scope.deductions.length - 1; i++) {
                    if ($scope.deductions[i].MENum == MENum) {
                        $scope.category = $scope.deductions[i].category;
                        $scope.date = $scope.deductions[i].date;
                        $scope.description = $scope.deductions[i].description;
                        $scope.timeadded = $scope.deductions[i].TimeAdded;
                        $scope.rate = $scope.deductions[i].Rate;
                        $scope.cost = $scope.deductions[i].Cost;
                        $scope.MENum = $scope.deductions[i].MENum;

                        $('#deductionModal').modal("show");
                    }
                }
            };

            //Delete Anamolie
            $scope.deleteAnamolie = function (id) {
                for (var i = 0; i <= $scope.anamolies.length - 1; i++) {
                    if ($scope.anamolies[i].ID == id) {
                        $scope.anamolies.splice(i, 1);
                    }
                }
            }

            //Check for saved invoice
            $scope.SaveInvoice = function () {
                $scope.isBusy = true;

                invoiceService.invoiceExists($scope.invoiceNumber).then(function (results) {
                    if (results == "True") {
                        $('#IE').show();
                    } else {
                        $('#IE').hide();
                    }
                    $('#SaveModal').modal('show');
                });

                $scope.isBusy = false;
            };
             
            //Save Invoice
            $('#saveInvoiceForm').submit(function (e) {
                $scope.isBusy = true;

                var $form = $(this);

                if ($form.valid()) {

                    var summaries = [];

                    //Custom
                    var summary = {
                        Row: "Custom",
                        Days: $scope.CustomDays,
                        Shifts: $scope.CustomShifts,
                        ContractHours: $scope.CustomContractHours,
                        OnPatrolHours: $scope.CustomOnPatrolHours
                    }
                    summaries.push(summary);

                    //Holidays
                    var summary = {
                        Row: "Holidays",
                        Days: $scope.HolDays,
                        Shifts: $scope.HolShifts,
                        ContractHours: $scope.HolContractHours,
                        OnPatrolHours: $scope.HolOnPatrolHours
                    }
                    summaries.push(summary);

                    //Sundays
                    var summary = {
                        Row: "Sundays",
                        Days: $scope.SunDays,
                        Shifts: $scope.SunShifts,
                        ContractHours: $scope.SunContractHours,
                        OnPatrolHours: $scope.SunOnPatrolHours
                    }
                    summaries.push(summary);

                    //Saturdays
                    var summary = {
                        Row: "Saturdays",
                        Days: $scope.SatDays,
                        Shifts: $scope.SatShifts,
                        ContractHours: $scope.SatContractHours,
                        OnPatrolHours: $scope.SatOnPatrolHours
                    }
                    summaries.push(summary);

                    //Weekdays
                    var summary = {
                        Row: "Weekdays",
                        Days: $scope.WeekDays,
                        Shifts: $scope.WeekShifts,
                        ContractHours: $scope.WeekContractHours,
                        OnPatrolHours: $scope.WeekOnPatrolHours
                    }
                    summaries.push(summary);

                    //Totals
                    var summary = {
                        Row: "Totals",
                        Days: $scope.TotalDays,
                        Shifts: $scope.TotalShifts,
                        ContractHours: $scope.TotalContractHours,
                        OnPatrolHours: $scope.TotalOnPatrolHours
                    }
                    summaries.push(summary);

                    var invoice = {
                        InvoiceID: $scope.invoiceNumber,
                        ContractorID: $scope.contractorid,
                        Month: $scope._month,
                        BeatID: $scope._beat,
                        FuelRate: $scope.FuelRate,
                        BaseRate: $scope.BaseRate,
                        Notes: $scope.Notes,
                        Summaries: summaries,
                        Additions: $scope.additions,
                        Deductions: $scope.deductions,
                        Anamolies: $scope.anamolies
                    };

                    invoiceService.saveInvoice(invoice).then(function (save_results) {
                        if (save_results) {
                            if (save_results == "Success") {
                                alert('Invoice Saved Successfully');
                            } else {
                                alert('Invoice Save Error: ' + save_results);
                            }
                        }

                        $('#SaveModal').modal('hide');
                    });
                }

                $scope.isBusy = false;
            });

            //Open Invoices Window
            $scope.openInvoices = function () {
                $scope.isBusy = true;
                $scope.invoiceNumbers = [];

                invoiceService.getInvoices().then(function (results) {
                    if (results) {
                        for (var i = 0; i < results.length; i++) {
                            $scope.invoiceNumbers.push(results[i]);
                        }
                    }

                    $scope.isBusy = false;
                    $('#openInvoicesModal').modal('show');
                });

                $scope.isBusy = false;
            };

            //Delete Invoice
            $scope.deleteInvoice = function (invoiceNumber) {
                $scope.isBusy = true;
                $scope.isBuilding = true;

                if(confirm("This will remove the saved invoice permanently from the database. Are you sure?"))
                {
                    //delete Invoice
                    invoiceService.deleteInvoice(invoiceNumber).then(function (results) {
                        if (results == true) {
                            alert("Invoice Deleted");

                            invoiceService.getInvoices().then(function (results) {
                                if (results) {
                                    for (var i = 0; i < results.length; i++) {
                                        $scope.invoiceNumbers.push(results[i]);
                                    }
                                }

                                $scope.isBusy = false;
                                $('#openInvoicesModal').modal('hide');
                            });
                        }
                    });
                }
            }

            //Open Invoice
            $scope.openInvoice = function (invoiceNumber) {
                $scope.isBusy = true;
                $scope.isBuilding = true;
                $scope.deductions = [];
                $scope.TotalDeductions = 0.00;
                $scope.TotalDedTimeAdded = 0.00;
                $scope.additions = [];
                $scope.TotalTimeAdded = 0.00;
                $scope.TotalAdditions = 0.00;
                $scope.TotalDays = 0.00;
                $scope.TotalShifts = 0.00;
                $scope.TotalContractHours = 0.00;
                $scope.TotalOnPatrolHours = 0.00;

                $('#openInvoicesModal').modal('hide');
                $('#invoiceDiv').hide("fadeOut");

                //Move progress bar
                $(".progress-bar").css("width", "15%");
                $(".progress-bar").html("Opening ...")

                //open invoice
                invoiceService.getInvoice(invoiceNumber).then(function (results) {
                    if (results) {
                        //Step 1 get contractor information
                        invoiceService.getBeatContractorInfo(results.BeatID, results.Month, results.ContractorID).then(function (Cont_results) {
                            if (Cont_results) {
                                $scope.month = $scope.monthsArray[results.Month - 1];
                                $scope._month = results.Month;
                                $scope.beatNum = results.BeatNum;
                                $scope.contractnumber = Cont_results.contractnumber;
                                $scope.contractorid = results.ContractorID;
                                $scope.contractexp = Cont_results.contractexp;
                                $scope.contractbeg = Cont_results.contractbeg;
                                $scope.companyname = Cont_results.companyname;
                                $scope.address1 = Cont_results.address1;
                                $scope.city = Cont_results.city;
                                $scope.state = Cont_results.state;
                                $scope.zip = Cont_results.zip;
                                $scope.contact = Cont_results.contact;
                                $scope.email = Cont_results.email;
                                $scope.phone = Cont_results.phone;
                                $scope.invoiceNumber = Cont_results.invoiceNumber;
                                $scope.CMBasePay = 0.00;
                                $scope.TotalContractHours = 0.00;
                                TotalContractHours = 0.00;

                                //Move progress bar
                                $(".progress-bar").css("width", "25%");
                                $(".progress-bar").html("Building Month's Base")

                                //Monthly Totals
                                $scope.TotalWeekdays = results.TotalWeekDaysInMonth;
                                $scope.TotalSaturdays = results.TotalSaturDaysInMonth;
                                $scope.TotalSundays = results.TotalSunDaysInMonth;
                                $scope.TotalHolidays = results.TotalHolidaysInMonth;
                                $scope.Holidays = "";
                                if (results.HolidaysInMonth.length > 0) {
                                    $scope.Holidays = " (";

                                    for (var i = 0; i < results.HolidaysInMonth.length; i++) {
                                        var parsedDate = new Date(parseInt(results.HolidaysInMonth[i].substr(6)))
                                        $scope.Holidays += parsedDate.toLocaleDateString()
                                        if (i < results.HolidaysInMonth.length - 1) { $scope.Holidays += ", "; }
                                    }
                                    $scope.Holidays += ")";
                                }
                                $scope.TotalCustomDays = results.TotalCustomDaysInMonth;
                                $scope.CustomDaysInMonth = "";
                                if (results.CustomDaysInMonth.length > 0) {
                                    $scope.CustomDaysInMonth = " (";

                                    for (var i = 0; i < results.CustomDaysInMonth.length; i++) {
                                        var parsedDate = new Date(parseInt(results.CustomDaysInMonth[i].substr(6)))
                                        $scope.CustomDaysInMonth += parsedDate.toLocaleDateString()
                                        if (i < results.CustomDaysInMonth.length - 1) { $scope.CustomDaysInMonth += ", "; }
                                    }
                                    $scope.CustomDaysInMonth += ")";
                                }

                                //Get Summary
                                for (var i = 0; i <= results.Summaries.length-1; i++) {

                                    if (results.Summaries[i].Row == "Custom") {
                                        $scope.CustomDays = results.Summaries[i].Days;
                                        $scope.CustomShifts = results.Summaries[i].Shifts;
                                        $scope.CustomContractHours = results.Summaries[i].ContractHours;
                                        $scope.CustomOnPatrolHours = results.Summaries[i].OnPatrolHours;

                                    } else if (results.Summaries[i].Row == "Holidays") {
                                        $scope.HolDays = results.Summaries[i].Days;
                                        $scope.HolShifts = results.Summaries[i].Shifts;
                                        $scope.HolContractHours = results.Summaries[i].ContractHours;
                                        $scope.HolOnPatrolHours = results.Summaries[i].OnPatrolHours;

                                    } else if (results.Summaries[i].Row == "Sundays") {
                                        $scope.SunDays = results.Summaries[i].Days;
                                        $scope.SunShifts = results.Summaries[i].Shifts;
                                        $scope.SunContractHours = results.Summaries[i].ContractHours;
                                        $scope.SunOnPatrolHours = results.Summaries[i].OnPatrolHours;

                                    } else if (results.Summaries[i].Row == "Saturdays") {
                                        $scope.SatDays = results.Summaries[i].Days;
                                        $scope.SatShifts = results.Summaries[i].Shifts;
                                        $scope.SatContractHours = results.Summaries[i].ContractHours;
                                        $scope.SatOnPatrolHours = results.Summaries[i].OnPatrolHours;

                                    } else if (results.Summaries[i].Row == "Weekdays") {
                                        $scope.WeekDays = results.Summaries[i].Days;
                                        $scope.WeekShifts = results.Summaries[i].Shifts;
                                        $scope.WeekContractHours = results.Summaries[i].ContractHours;
                                        $scope.WeekOnPatrolHours = results.Summaries[i].OnPatrolHours;

                                    } else if (results.Summaries[i].Row == "Totals") {
                                        $scope.TotalDays = results.Summaries[i].Days;
                                        $scope.TotalShifts = results.Summaries[i].Shifts;
                                        $scope.TotalContractHours = results.Summaries[i].ContractHours;
                                        TotalContractHours = results.Summaries[i].ContractHours;
                                        $scope.TotalOnPatrolHours = results.Summaries[i].OnPatrolHours;
                                    }
                                }

                                //move progress bar to base pay
                                $(".progress-bar").css("width", "45%");
                                $(".progress-bar").html("Retreiving Base Rate and Pay")
                                $scope.FuelRate = parseFloat(results.FuelRate).toFixed(2);
                                $scope.BaseRate = parseFloat(results.BaseRate).toFixed(2);
                                $scope.CMBasePay = ($scope.TotalContractHours * $scope.BaseRate).toFixed(2);

                                //move progress bar to additions
                                $(".progress-bar").css("width", "65%");
                                $(".progress-bar").html("Finding Additions")
                                $scope.additions = results.Additions;
                                for (var i = 0; i <= results.Additions.length - 1; i++) {
                                    $scope.TotalAdditions += parseFloat(results.Additions[i].Cost);
                                    $scope.TotalTimeAdded += parseFloat(results.Additions[i].TimeAdded);
                                    totalAdditions += parseFloat(results.Additions[i].Cost);
                                }
                                
                                //move progress bar to deductions
                                $(".progress-bar").css("width", "65%");
                                $(".progress-bar").html("Finding Additions")
                                $scope.deductions = results.Deductions;
                                for (var j = 0; j <= results.Deductions.length - 1; j++) {
                                    $scope.TotalDeductions += parseFloat(results.Deductions[j].Cost);
                                    $scope.TotalDedTimeAdded += parseFloat(results.Deductions[j].TimeAdded);
                                    totalDeductions += parseFloat(results.Deductions[j].Cost);
                                }

                                //move progress bar to anomalies
                                $(".progress-bar").css("width", "75%");
                                $(".progress-bar").html("Finding Anomalies")
                                $scope.anamolies = results.Anamolies;

                                //move progress bar to compelted
                                $(".progress-bar").css("width", "100%");
                                $(".progress-bar").html("Invoice Compelted")

                                $scope.InvoiceTotal = Number(Number($scope.CMBasePay) + Number($scope.TotalAdditions) - Number($scope.TotalDeductions)).toFixed(2);
                                $scope.isBusy = false;
                                $scope.isBuilding = false;
                                $('#openInvoicesModal').modal('hide');
                                $('#invoiceDiv').show("fadeIn");
                                $(".progress-bar").css("width", "5%");
                            } else {
                                $('#MonthDDText').text('Choose Month');
                                $('#BeatDDText').text('Choose Beat');
                                $scope.isBusy = false;
                                $scope.isBuilding = false;
                                $('#openInvoicesModal').modal('hide');
                                alert("No Invoice Was Found. Please try again.");
                            }
                        });
                    } else {
                        $('#MonthDDText').text('Choose Month');
                        $('#BeatDDText').text('Choose Beat');
                        $scope.isBusy = false;
                        $scope.isBuilding = false;
                        $('#openInvoicesModal').modal('hide');
                        alert("No Invoice Was Found. Please try again.");
                    }
                    
                });

                $('.footable').footable();
            };

            //Print Invoice
            $scope.printInvoice = function () {
                window.print();
            };

            $('.footable').footable();
        }
    );
}());