var ViewModel = function () {

    var self = this;
    self.mode = ko.observable('add');
    self.isBusy = ko.observable(false);
    self.alarm = ko.observable('');

    self.currentBeatID = ko.observable('');
    self.schedules = ko.observableArray([]);
    self.beats = ko.observableArray([]);
    self.beatSchedules = ko.observableArray([]);
    self.selectedBeat = ko.observable();   
    self.selectedSchedules = ko.observableArray([]);

    self.getSchedules = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AdminArea/BeatSchedule/GetSchedules';

        console.time('Getting Schedules');

        $.get(url,
              function (schedules) {

                  console.timeEnd('Getting Schedules');
                  self.schedules([]);

                  $.each(schedules, function (i, schedule) {
                      self.schedules.push(new uiSchedule(self, schedule));
                  });

              }, "json");
    }
    self.getBeats = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AdminArea/BeatSchedule/GetBeatsWithoutSchedules';
        console.time('Getting Beats');
        $.get(url,
              function (beats) {
                  console.timeEnd('Getting Beats');
                  self.beats([]);

                  $.each(beats, function (i, beat) {
                      self.beats.push(beat);
                  });

              }, "json");
    }
    self.getBeatSchedules = function () {

        var url = $("#websitePath").attr("data-websitePath") + '/AdminArea/BeatSchedule/GetBeatSchedules';
        console.time('Getting Beat Schedules');
        $.get(url,
              function (beatSchedules) {
                  console.timeEnd('Getting Beat Schedules');
                  self.beatSchedules([]);
                  console.log(beatSchedules);


                  $.each(beatSchedules, function (i, beatSchedule) {
                      self.beatSchedules.push(beatSchedule);
                  });

              }, "json");

    };

    self.editBeatSchedule = function (item) {
        self.selectedSchedules([]);

        for (var i = 0; i < self.schedules().length; i++) {
            self.schedules()[i].isSelected(false);
            self.schedules()[i].isEnabled(true);
        }

        if (item.BeatID != undefined) {
            console.log('Edit Mode');

            self.mode('edit');

            self.currentBeatID(item.BeatID);
            var currentSchedules = item.ScheduleList;

            for (var i = 0; i < currentSchedules.length; i++) {
                for (var ii = 0; ii < self.schedules().length; ii++) {
                    if (self.schedules()[ii].scheduleName() === currentSchedules[i]) {
                        self.schedules()[ii].isSelected(true);
                        self.selectedSchedules.push(self.schedules()[ii]);
                    }
                }
            }

            self.validate();
        }
        else {
            console.log('Add Mode');
            self.mode('add');
        }

        $("#modelBeatSchedule").modal('show');

    };
    self.deleteBeatSchedule = function (item) {

        var url = $("#websitePath").attr("data-websitePath") + '/AdminArea/BeatSchedule/Delete';

        $.ajax({
            url: url,
            type: "POST",
            data: {
                BeatID: item.BeatID
            },
            dataType: "json",
            success: function (result) {
                console.log('Deleting result %s', result);

                if (result === true) {
                    alert("Successfully deleted beat schedule");
                }

                self.schedules([]);
                self.beats([]);
                self.beatSchedules([]);
                self.selectedBeat();
                self.selectedSchedules();
                self.currentBeatID();

                self.isBusy(false);

                self.getBeatSchedules();
                self.getSchedules();
                self.getBeats();
                $("#modelBeatSchedule").modal('hide');
            },
            error: function (jqXHR, tranStatus, errorThrown) {
                console.error(jqXHR);

                self.isBusy(false);
            }
        });

    };
    self.save = function () {

        console.log('Number of selected schedules %s', self.selectedSchedules().length);

        var url = $("#websitePath").attr("data-websitePath") + '/AdminArea/BeatSchedule/Save';

        var selectedScheduleIds = ko.observableArray([]);

        for (var i = 0; i < self.selectedSchedules().length; i++) {
            selectedScheduleIds.push(self.selectedSchedules()[i].scheduleId);
        }

        var beatID;

        if (self.mode() === 'add')
            beatID = self.selectedBeat().BeatID;
        else
            beatID = self.currentBeatID();

        self.isBusy(true);

        $.ajax({
            url: url,
            type: "POST",
            data: {
                BeatID: beatID,
                SelectedSchedules: JSON.stringify(selectedScheduleIds()),               
            },
            dataType: "json",
            success: function (result) {
                console.log('Saving result %s', result);

                if (result === true) {
                    if (self.mode() === 'add')
                        alert("Successfully added beat schedules");
                    else
                        alert("Beat schedules successfully updaed");
                }

                self.schedules([]);
                self.beats([]);
                self.beatSchedules([]);
                self.selectedBeat();
                self.selectedSchedules();
                self.currentBeatID();

                self.isBusy(false);

                self.getBeatSchedules();
                self.getSchedules();
                self.getBeats();
                $("#modelBeatSchedule").modal('hide');
            },
            error: function (jqXHR, tranStatus, errorThrown) {
                console.error(jqXHR);

                self.isBusy(false);
            }
        });



    };

    //main busines logic to determin conflicting schedules
    self.validate = function () {

        //enable all first
        for (var iii = 0; iii < self.schedules().length; iii++) {
            self.schedules()[iii].isEnabled(true);
        }

        //let's disable all other schedule that would conflict or enable schedules that don't
        for (var i = 0; i < self.selectedSchedules().length; i++) {

            var selectedSavedSchedule = self.selectedSchedules()[i];

            for (var ii = 0; ii < self.schedules().length; ii++) {

                var schedule = self.schedules()[ii];

                if ((selectedSavedSchedule.rollIn.Hours < schedule.rollIn.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                    if ((selectedSavedSchedule.rollIn.Hours > schedule.onPatrol.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                        if (schedule.isSelected() === false) {
                            schedule.isEnabled(false);
                        }
                    }
                }
                else {
                    if ((selectedSavedSchedule.onPatrol.Hours < schedule.rollIn.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                        if (schedule.isSelected() === false) {
                            schedule.isEnabled(false);
                        }
                    }
                }
            }
        }
    };

    function uiSchedule(root, dbSchedule) {

        var self = this;
        self.scheduleId = dbSchedule.ScheduleID;
        self.scheduleName = ko.observable(dbSchedule.ScheduleName);
        self.onPatrol = dbSchedule.OnPatrol;
        self.rollIn = dbSchedule.RollIn;
        self.weekday = dbSchedule.Weekday;
        self.isSelected = ko.observable(false);
        self.isEnabled = ko.observable(true);
        self.start = dbSchedule.Start;
        self.end = dbSchedule.End;

        self.checked = function (selectedSchedule) {

            console.log('Checked Schedule: %s, OnPatrol: %s, RollIn: %s', selectedSchedule.scheduleName(), selectedSchedule.onPatrol.Hours, selectedSchedule.rollIn.Hours);

            if (selectedSchedule.isSelected()) {
                selectedSchedule.isSelected(false);
                root.selectedSchedules.remove(selectedSchedule);

            }
            else {
                selectedSchedule.isSelected(true);
                root.selectedSchedules.push(selectedSchedule);
            }

            console.log('Number of selected schedules %s', root.selectedSchedules().length);
            root.validate();


            //IMPORTANT that will allow to do the checkboxes checking and unchecking
            return true;
        };

    };

};

$(function () {

    var viewModel = new ViewModel();
    ko.applyBindings(viewModel);
    viewModel.getBeatSchedules();
    viewModel.getSchedules();
    viewModel.getBeats();

});