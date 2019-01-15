$(function () {

    //// And now fire change event when the DOM is ready 
    //$('#cboFreeways').trigger('change');
    //$('#cboFreeways').change(function (e) {
    //    // Your event handler 
    //    var selectedFreewayId = $("#cboFreeways option:selected").val();
    //    var url = $("#websitePath").attr("data-websitePath") + "/Admin/GetBeatsByFreewayId";

    //    if (selectedFreewayId.length > 0) {
    //        $.ajax({
    //            url: url,
    //            dataType: "json",
    //            data: {
    //                freewayId: selectedFreewayId
    //            },
    //            success: function (data) {

    //                $("#SelectedBeat").removeAttr('disabled');

    //                $('#SelectedBeat').html('');

    //                //$('#cboBeats').append(
    //                //        $('<option></option>').val("").html("--Select Beat--")
    //                //    );

    //                $.each(data, function (index, template) {
    //                    $('#SelectedBeat').append(
    //                        $('<option></option>').val(template.BeatID).html(template.BeatNumber)
    //                    );
    //                });
    //            },
    //            error: function (data) {
    //                alert('Error: ' + data);
    //            }
    //        });
    //    }
    //    else {
    //        $("#SelectedBeat").attr("disabled", "disabled");
    //    }
    //});


    //// And now fire change event when the DOM is ready 
    //$('#SelectedBeat').trigger('change');
    //$('#SelectedBeat').change(function (e) {
    //    // Your event handler 
    //    var selectedBeatId = $("#SelectedBeat option:selected").val();
    //    var url = $("#websitePath").attr("data-websitePath") + "/Admin/GetSegmentsByBeatId";

    //    if (selectedBeatId.length > 0) {
    //        $.ajax({
    //            url: url,
    //            dataType: "json",
    //            data: {
    //                beatId: selectedBeatId
    //            },
    //            success: function (data) {

    //                $("#SelectedSegment").removeAttr('disabled');

    //                $('#SelectedSegment').html('');

    //                //$('#cboSegments').append(
    //                //        $('<option></option>').val("").html("--Select Segment--")
    //                //    );

    //                $.each(data, function (index, template) {
    //                    $('#SelectedSegment').append(
    //                        $('<option></option>').val(template.BeatSegmentID).html(template.BeatSegmentNumber)
    //                    );
    //                });
    //            },
    //            error: function (data) {
    //                alert('Error: ' + data);
    //            }
    //        });
    //    }
    //    else {
    //        $("#SelectedSegment").attr("disabled", "disabled");
    //    }
    //});

    ////admin tables    
    var tables = $('.tablesorter').dataTable({
        "sDom": '<"clear">f<"top"i>rt<"bottom"pl><"clear">R',
        "oLanguage": {
            "oPaginate": {
                "sPrevious": "‹",
                "sNext": "›"
            }
        },
        "aaSorting": [[0, "desc"]],
        "sPaginationType": "full_numbers",
        //"sPaginationType": "bootstrap",
        "iDisplayLength": 10,
        "bStateSave": false
    });

    //admin tables    
    //var tables = $('.tablesorter').dataTable();

});