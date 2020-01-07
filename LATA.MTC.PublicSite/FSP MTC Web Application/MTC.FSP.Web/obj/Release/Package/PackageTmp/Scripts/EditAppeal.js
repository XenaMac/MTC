$(function () {

    $('#BeatDD').change(function (e) {
        $('#Beatid').val($('#BeatDD').find(':selected').val());
    });

    $('#DriversDD').change(function (e) {
        $('#DriverId').val($('#DriversDD').find(':selected').val());
    });

    $('#BlocksInvoiced').change(function (e) {
        $('#O_BlocksInitGranted').val($('#BlocksInvoiced').find(':selected').val());
    });

    $('#BlocksAppealed').change(function (e) {
        $('#O_NumOfBlocks').val($('#BlocksAppealed').find(':selected').val());
    });

    //if form submties run validation
    $("#EditAppealForm").submit(function (e) {
        if ($('#AppealType').val() == "Violation") {
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
            } else if (!pattern.test($('#V_AppropriateCharge').val())) {
                alert("Please enter Appropriate Charge (Must be Decimal).")
                e.preventDefault();
            } else {
                $('#CreateAppealForm').submit();
            }
        } else if ($('#AppealType').val() == "Overtime") {
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
        } else if ($('#AppealType').val() == "Invoice") {
            if ($('#ContactName').val() == "") {
                alert("Please enter Contact Name.")
                e.preventDefault();
            } else if ($('#ContactPhone').val() == "") {
                alert("Please enter Contact Phone #.")
                e.preventDefault();
            } else if ($('#I_EventDate').val() == "") {
                alert("Please enter date of deduction/addition event.")
                e.preventDefault();
            } else if ($('#I_DriverDD').val() == "") {
                alert("Please select a driver.")
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
                $('#EditAppealForm').submit();
            }
        }
    });
});