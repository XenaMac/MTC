$(function () {
    $('#violationDiv').hide();
    $('#overtimeDiv').hide();
    $('#invoiceDiv').hide();
    $('#ViolationInfoDiv').hide();
    var AppealType = "";
    var pattern = new RegExp("^\d+\.\d{0,2}$");

    $(document).on('change', 'input:radio[name="AppealType"]', function (event) {
        //alert($(this).val());
        var value = $(this).val();
        if (value == "Violation") {
            AppealType = "Violation";
            //Drive the divs
            $('#overtimeDiv').slideUp("slow", function () {
                $('#invoiceDiv').slideUp("slow", function () {
                    $('#ViolationInfoDiv').html('');
                    $('#ViolationDD').val($("#target option:first").val());
                    $('#ViolationInfoDiv').slideUp('slow');
                    $('#violationDiv').slideDown("slow");
                });
            });
        } else if (value == "Overtime") {
            //Drive the divs
            AppealType = "Overtime";
            $('#invoiceDiv').slideUp("slow", function () {
                $('#violationDiv').slideUp("slow", function () {
                    $('#overtimeDiv').slideDown("slow");
                });
            });
        } else if (value == "Invoice") {
            //Drive the divs
            AppealType = "Invoice";
            $('#violationDiv').slideUp("slow", function () {
                $('#overtimeDiv').slideUp("slow", function () {
                    $('#invoiceDiv').slideDown("slow");
                });
            });
        }
    })

    $('#ViolationDD').change(function () {
        $.ajax({
            url: '../Home/GetViolationDetails?VID=' + $('#ViolationDD').val(),
            data: {
                format: 'json'
            },

            error: function (request, error) {
                console.log(arguments);
                alert(" Bad call: " + error);
            },
            beforeSend: function () {
                $('#ViolationInfoDiv').slideUp("slow");
                $('#ViolationInfoDiv').html('');
            },
            success: function (data) {
                var htmlString = "--- Violation Details ---- <br />" +
                    "<strong>Severity: </strong>" + data.Severity + "<br />" +
                    "<strong>Length Of Violation: </strong>" + data.LengthOfViolation + "<br />" +
                    "<strong>Driver: </strong>" + data.Driver + "<br />" +
                    "<strong>Fleet Vehicle: </strong>" + data.FleetVehicle + "<br />" +
                    "<strong>Beat: </strong>" + data.Beat + "<br />" +
                    "<strong>Call Sign: </strong>" + data.Callsign + "<br />" +
                    "<strong>Offense Number: </strong>" + data.OffenseNumb + "<br />" +
                    "<strong>Violation Status: </strong>" + data.Status + "<br />" +
                    "<strong>Deduction Amount: </strong>" + data.Deduction + "<br />" +
                    "<strong>Notes: </strong>" + data.Notes;
                $('#DriverId').val(data.DriverId);
                $('#Beatid').val(data.BeatId)
                $('#ViolationId').val($('#ViolationDD').val());
                $('#ViolationInfoDiv').html(htmlString);
                $('#ViolationInfoDiv').slideDown("slow");
            },
            type: 'GET'
        });
    });

    $('#DriverDD').change(function (e) {
        $('#DriverId').val($('#DriverDD').find(':selected').val());
    });

    $('#Beat').change(function () {
        $.ajax({
            url: '../Home/GetCallSigns?BeatID=' + $('#Beat').find(':selected').text().toString(),
            data: {
                format: 'json'
            },

            error: function (request, error) {
                console.log(arguments);
                alert(" Bad call: " + error);
            },
            beforeSend: function () {
                //$('#CallSign').empty();
            },
            success: function (data) {
                var _selCode = '';
                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        _selCode += '<option id="' + data[i].Id + '">' + data[i].Text + '</option>';
                    }
                    $('#CallSign').append(_selCode);
                    $('#CallSign').prop('disabled', false);
                    $('#Beatid').val($('#Beat').find(':selected').val().toString());
                }
                else {
                    $scope.header = 'Error. We were not able to retrieve your callsigns. Please come back later.';
                }
            },
            type: 'GET'
        });
    });

    $('#I_DriverDD').change(function (e) {
        $('#DriverId').val($('#I_DriverDD').find(':selected').val());
    });

    $('#I_Beat').change(function (e) {
        $('#Beatid').val($('#I_Beat').find(':selected').val());
    });

    $('input#V_AppropriateCharge').blur(function(){
        var num = parseFloat($(this).val());
        var cleanNum = num.toFixed(2);
        $(this).val(cleanNum);
        if(num/cleanNum < 1){
            $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
        }
    });

    $('input#I_AppealDeduction').blur(function () {
        var num = parseFloat($(this).val());
        var cleanNum = num.toFixed(2);
        $(this).val(cleanNum);
        if (num / cleanNum < 1) {
            $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
        }
    });
    
    $('input#I_InvoiceDeduction').blur(function () {
        var num = parseFloat($(this).val());
        var cleanNum = num.toFixed(2);
        $(this).val(cleanNum);
        if (num / cleanNum < 1) {
            $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
        }
    });

    $('#btnCreateAppeal').click(function (e) {
        if (AppealType == "Violation") {
            if ($('#ContactName').val() == "") {
                alert("Please enter Contact Name.")
                e.preventDefault();
            } else if ($('#ContactPhone').val() == "") {
                alert("Please enter Contact Phone #.")
                e.preventDefault();
            } else if ($('#V_ReasonForAppeal').val() == "") {
                alert("Please enter Reason For Appeal.")
                e.preventDefault();
            } else if ($('#V_AppropriateCharge').val() == "") {
                alert("Please enter Appropriate Charge.")
                e.preventDefault();
            } else if ($('#ViolationDD').val() == "") {
                alert("Please select Violation.")
                e.preventDefault();
            } else {
                $('#CreateAppealForm').submit();
            }
        } else if (AppealType == "Overtime") {
            if ($('#ContactName').val() == "") {
                alert("Please enter Contact Name.")
                e.preventDefault();
            } else if ($('#ContactPhone').val() == "") {
                alert("Please enter Contact Phone #.")
                e.preventDefault();
            } else if ($('#O_Datetime').val() == "") {
                alert("Please enter date and time of claimed overtime.")
                e.preventDefault();
            } else if ($('#Beat').val() == "") {
                alert("Please enter a beat #.")
                e.preventDefault();
            } else if ($('#CallSign').val() == "") {
                alert("Please enter a callsign.")
                e.preventDefault();
            } else {
                $('#CreateAppealForm').submit();
            }
        } else if (AppealType == "Invoice") {
            if ($('#ContactName').val() == "") {
                alert("Please enter Contact Name.")
                e.preventDefault();
            } else if ($('#ContactPhone').val() == "") {
                alert("Please enter Contact Phone #.")
                e.preventDefault();
            } else if ($('#I_EventDate').val() == "") {
                alert("Please enter date of deduction/addition event.")
                e.preventDefault();
            } else if ($('#I_InvoiceReason').val() == "") {
                alert("Please enter the invoice reason.")
                e.preventDefault();
            } else if ($('#I_AppealReason').val() == "") {
                alert("Please enter the appeal reason.")
                e.preventDefault();
            } else if ($('#I_InvoiceDeduction').val() == "") {
                alert("Please enter the invoice amount.")
                e.preventDefault();
            } else if ($('#I_AppealDeduction').val() == "") {
                alert("Please enter the appeal amount.")
                e.preventDefault();
            } else {
                $('#CreateAppealForm').submit();
            }
        }
    });
});