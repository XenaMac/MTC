﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    public class MetraTemplate
    {
        private String[] AlertTemplateName = 
        {
/*
            0-"Trains Stopped (Line)", 
            1-"Trains Stopped Extensive Delays Expected",
            2-"Trains Remain Stopped",
            3-"Inbound and Outbound Trains Delayed",
            4-"Inbound and Outbound Train Movement Halted",
            5-"Inbound and Outbound Train Movement Remains Halted",
            6-"Inbound and Outbound Train Movement Halted Extensive Delays Expected",
            7-"Train Movement Remains Halted Seek Alternate Transportation",
            8-"CTA Honoring Metra Tickets",
            9-"CTA No Longer Honors Metra Tickets",
            10-"Train Delay (single Train)",
            11-"Multiple Trains Delayed",
            12-"Multiple Trains Stopped (specific trains)",
            13-"Trains Stopped, board alternate train",
            14-"Single track, board alternate train",
            15-"Trains on the move",
            16-"Delayed train expressing",
            17-"Bussing",
            18-"Schedule Modification Alert",
            19-"Train Expressing",
            20-"Train Making Extra Stops",
            21-"Train Expressing, Extra train Will Accommodate",
            22-"Train Making Flag Stops",
            23-"Train Changing its Origin Station",
            24-"Train Terminating",
            25-"Train will not operate",
            26-"Train Canceled Then Uncanceled",
            27-"Delayed Departure, Duration Unknown",
            28-"Delayed Departure, Anticipated Duration",
            29-"Delayed departure, On the move.",
            30-"Train Delay (single Train) auto-alert",
            31-"Station/Platform Construction",
            32-"Elevator out of service",
            33-"Elevator back in service",
            34-"Elevator out of service for scheduled time frame",
            35-"Winter Weather Advisory",
            36-"Heat Advisory",
            37-"Alcohol Restrictions",
            38-"Alcohol Prohibited ",
            39-"Chicago Union Station Overcrowding (Great Hall Open)",
            40-"Chicago Union Station Overcrowding (Great Hall Closed)"
*/
            "Trains Stopped (Line)", 
            "Trains Stopped Extensive Delays Expected",
            "Trains Remain Stopped",
            "Inbound and Outbound Trains Delayed",
            "Inbound and Outbound Train Movement Halted",
            "Inbound and Outbound Train Movement Remains Halted",
            "Inbound and Outbound Train Movement Halted Extensive Delays Expected",
            "Train Movement Remains Halted Seek Alternate Transportation",
            "CTA Honoring Metra Tickets",
            "CTA No Longer Honors Metra Tickets",
            "Train Delay (single Train)",
            "Multiple Trains Delayed",
            "Multiple Trains Stopped (specific trains)",
            "Trains Stopped, board alternate train",
            "Single track, board alternate train",
            "Trains on the move",
            "Delayed train expressing",
            "Bussing",
            "Schedule Modification Alert",
            "Train Expressing",
            "Train Making Extra Stops",
            "Train Expressing, Extra train Will Accommodate",
            "Train Making Flag Stops",
            "Train Changing its Origin Station",
            "Train Terminating",
            "Train will not operate",
            "Train Canceled Then Uncanceled",
            "Delayed Departure, Duration Unknown",
            "Delayed Departure, Anticipated Duration",
            "Delayed departure, On the move.",
            "Train Delay (single Train) auto-alert",
            "Station/Platform Construction",
            "Elevator out of service",
            "Elevator back in service",
            "Elevator out of service for scheduled time frame",
            "Winter Weather Advisory",
            "Heat Advisory",
            "Alcohol Restrictions",
            "Alcohol Prohibited ",
            "Chicago Union Station Overcrowding (Great  Hall Open)",
            "Chicago Union Station Overcrowding (Great Hall Closed)"
        };

        private String[] AlertTemplateHeader = 
        {
/*
            0-"{1, '<LINE>'} Inbound and Outbound Trains Stopped Near </LOCATION>{0, ' due to <DELAY_REASON>'}",
            1-"Inbound and Outbound Trains Stopped Near </LOCATION> due to <DELAY_REASON>",
            2-"Inbound and Outbound Trains Remain Stopped Near </LOCATION> due to <DELAY_REASON>",
            3-"Inbound and Outbound Trains May Be Operating </XX> to </XX> minutes behind Schedule",
            4-"Inbound and Outbound Train Movement Halted Near </LOCATION>",
            5-"Inbound and Outbound Train Movement Remains Halted",
            6-"Inbound and Outbound Train Movement Halted",
            7-"Inbound and Outbound Train Movement Halted",
            8-"CTA Will Honor Metra Tickets on the </SPECIFIC_CTA_LINES> Until Further Notice",
            9-"CTA Will No Longer Honor Metra Tickets.",
            10-"<DIRECTION> Train <TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> -- </XX> to </XX> Minute Delay",
            11-"<DIRECTION> <S/P, 'train/trains'> {1, '#<TRAIN_NUM>'}- Delayed",
            12-"<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} are stopped",
            13-"<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} <S/P, 'is/are'> stopped, board </TRAIN_NUM> at </STATION> station",
            14-"<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} commuters, board </TRAIN_NUM> at </STATION> station",
            15-"Inbound and Outbound Trains Are On The Move Through The Incident Site </XX> to </XX> Minutes Late.",
            16-"Train #<TRAIN_NUM> delayed </XX> minutes -- Expressing.",
            17-"<DIRECTION> Train {1, '#<TRAIN_NUM>'} -Will Originate at </NEW_ORIGIN>. Bus Service Will Be Provided",
            18-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing",
            19-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing",
            20-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Extra Stops",
            21-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing.",
            22-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Flag Stops",
            23-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Will Originate at </STATION>",
            24-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive </STATION> at <SCHED_DEST_TIME> ­ Terminating at </STATION>",
            25-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart </STATION> at <SCHED_ORIGIN_TIME> - Will Not Operate",
            26-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Will Operate",
            27-"<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Delayed Departure",
            28-"<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Delayed Departure",
            29-"<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - </XX> Minutes Delayed",
            30-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> is delayed <DELAY_TIME> minutes",
            31-"<STATION> Platform Rehabilitation",
            32-"The <STATION> Elevator Temporarily Out Of Service.",
            33-"<STATION> Elevator Back In Service.",
            34-"<STATION> Elevator Will Be Out Of Service from </START_TIME> to </END_TIME>",
            35-"The National Weather Service forecast includes a Winter Weather Advisory and Hazardous Weather Outlook for the Chicago area that may adversely affect portions of the region served by Metra.  Please refer to:  National Weather Service Forecast.",
            36-"Heat Advisory Reminder",
            37-"Alcohol Resctricted After 7 P.M.",
            38-"Alcohol Prohibited All Day",
            39-"CUS Service Disruption - Proceed To Great Hall Prior To Boarding Trains",
            40-"CUS Service Disruption - Proceed To Area Near Ticket Office or Breezeway Between Great Hall and Amtrak"
*/
            //"Inbound and Outbound Trains Stopped Near </LOCATION> due to <DELAY_REASON>",
            "{1, '<LINE>'} Inbound and Outbound Trains Stopped Near </LOCATION>{0, ' due to <DELAY_REASON>'}",
            "Inbound and Outbound Trains Stopped Near </LOCATION> due to <DELAY_REASON>",
            "Inbound and Outbound Trains Remain Stopped Near </LOCATION> due to <DELAY_REASON>",
            "Inbound and Outbound Trains May Be Operating </XX> to </XX> minutes behind Schedule",
            "Inbound and Outbound Train Movement Halted Near </LOCATION>",
            "Inbound and Outbound Train Movement Remains Halted",
            "Inbound and Outbound Train Movement Halted",
            "Inbound and Outbound Train Movement Halted",
            "CTA Will Honor Metra Tickets on the </SPECIFIC_CTA_LINES> Until Further Notice",
            "CTA Will No Longer Honor Metra Tickets.",
            "<DIRECTION> Train <TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> -- </XX> to </XX> Minute Delay",
            "<DIRECTION> <S/P, 'train/trains'> {1, '#<TRAIN_NUM>'}- Delayed",
            "<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} are stopped",
            "<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} <S/P, 'is/are'> stopped, board </TRAIN_NUM> at </STATION> station",
            "<S/P, 'Train/Trains'> {1, '#<TRAIN_NUM>'} commuters, board </TRAIN_NUM> at </STATION> station",
            "Inbound and Outbound Trains Are On The Move Through The Incident Site </XX> to </XX> Minutes Late.",
            "Train #<TRAIN_NUM> delayed </XX> minutes -- Expressing.",
            "<DIRECTION> Train {1, '#<TRAIN_NUM>'} -Will Originate at </NEW_ORIGIN>. Bus Service Will Be Provided",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Extra Stops",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Expressing.",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> - Flag Stops",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Will Originate at </STATION>",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive </STATION> at <SCHED_DEST_TIME> ­ Terminating at </STATION>",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart </STATION> at <SCHED_ORIGIN_TIME> - Will Not Operate",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Will Operate",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Delayed Departure",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - Delayed Departure",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled To Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> - </XX> Minutes Delayed",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> delayed <DELAY_TIME> minutes",
            "<STATION> Platform Rehabilitation",
            "The <STATION> Elevator Temporarily Out Of Service.",
            "<STATION> Elevator Back In Service.",
            "<STATION> Elevator Will Be Out Of Service from </START_TIME> to </END_TIME>",
            "The National Weather Service forecast includes a Winter Weather Advisory and Hazardous Weather Outlook for the Chicago area that may adversely affect portions of the region served by Metra.  Please refer to:  National Weather Service Forecast.",
            "Heat Advisory Reminder",
            "Alcohol Resctricted After 7 P.M.",
            "Alcohol Prohibited All Day",
            "CUS Service Disruption - Proceed To Great Hall Prior To Boarding Trains",
            "CUS Service Disruption - Proceed To Area Near Ticket Office or Breezeway Between Great Hall and Amtrak"
        };

        private String[] AlertTemplateText = 
        {
/*
            0-"{1, '<LINE>'} inbound and outbound trains are stopped near </LOCATION>{0, ' due to <DELAY_REASON>'}. The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            1-"Inbound and outbound trains are stopped near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Extensive delays are expected. Please listen to platform announcements for delay times.  Updates will be provided.",
            2-"Inbound and outbound trains remain stopped near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            3-"Inbound and outbound trains may be operating </XX> to </XX> minutes behind schedule due to <DELAY_REASON>.  Please listen to platform announcements for the location of your train.",
            4-"Inbound and outbound train movement  has been halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown. Please listen to platform announcements for delay times.  Updates will be provided.",
            5-"Inbound and outbound train movement remains halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            6-"Inbound and outbound train movement  has been halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Extensive delays are expected.  Please listen to platform announcements for delay times.  Updates will be provided.",
            7-"Inbound and outbound train movement has been halted near </LOCATION> due to <DELAY_REASON>. The duration of this delay is unknown.  Passengers may wish to utilize alternate transportation.  Please listen to platform announcements for delay times.  Updates will be provided.",
            8-"CTA will honor Metra tickets on the </SPECIFIC_CTA_LINES> due to <DELAY_REASON> at </LOCATION> until further notice.",
            9-"The CTA will no longer honor Metra tickets on the </SPECIFIC_CTA_LINES>.",
            10-"Train <TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is operating </XX> To </XX> minutes behind schedule due to <DELAY_REASON>.",
            11-"<DIRECTION> {1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> operating </XX> to </XX> minutes behind schedule due to <DELAY_REASON>.",
            12-"{1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> stopped near </LOCATION> due to police activity.",
            13-"{1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> stopped near </STATION> station due to police activity. Commuters on board both trains will board train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, and continue into </SCHED_DESTINATION>.",
            14-"Due to one track being open to train service, commuters on board {1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} will detrain at the </STATION> station and board train #</TRAIN_NUM> scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>  and continue their commute.",
            15-"The following trains are on the move through the <DELAY_REASON> site at </LOCATION> with Some Schedule Modifications. {1, 'train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} are operating approximately </XX> minutes late and will make all scheduled station stops.",
            16-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> operating approximately </XX> minutes late will detrain commuters between </STATION> station and </STATION> station and express into <SCHED_DESTINATION>   Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate commuters between </STATION> station and </STATION> station and make all scheduled station stops.",
            17-"{1, 'train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>'} will originate at </NEW_ORIGIN> due to <DELAY_REASON>.  Passengers from </STATION>  to </STATION>, will be bussed to </STATION> where a waiting train will make all scheduled stops to <SCHED_DESTINATION>. The bus will arrive </STATION> at approximately </X:XXAM/PM>. ",
            18-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </STATION> to </STATION>.  Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate the passengers at intermediate stops.",
            19-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </STATION> to </STATION>.  Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate the passengers at intermediate stops.",
            20-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will make extra stop at </STATIONS>.  Please listen to platform announcements",
            21-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </LOCATION> to </LOCATION>.  An extra train following behind this train will accommodate passengers.  Please listen to platform announcements for the location of the extra train.",
            22-"Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will make flag stops enroute to <SCHED_DESTINATION>.  Please stand behind the yellow line and remain visible to the Engineer as this train approaches your station.",
            23-"<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will Originate at </STATION>.  The next train making station stops from </STATION> to </STATION> will be train #</TRAIN_NUM>.",
            24-"Train #<TRAIN_NUM> scheduled to arrive </STATION> at <SCHED_DEST_TIME>, will terminate at </LOCATION>, due to <DELAY_REASON>.  Train #</TRAIN_NUM> will accommodate passengers to </STATION>.",
            25-"Train #<TRAIN_NUM> scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will not operate due to <DELAY_REASON>.  Train #</TRAIN_NUM> will be the next train making all scheduled stops to <SCHED_DESTINATION>.",
            26-"Train #<TRAIN_NUM> scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will operate.  This is a revised operational change to an earlier website posting.",
            27-"Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will be delayed in departure due to <DELAY_REASON>. Estimated time of delay is unknown. Metra will provide updated information as it becomes available.",
            28-"Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will be delayed in departure due to <DELAY_REASON>. Estimated time of delay will be approximately </XX> to </XX> minutes.",
            29-"Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is on the move operating </XX> minutes behind schedule due to <DELAY_REASON>.",
            30-"Train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is operating <DELAY_TIME> minutes behind schedule{0, ' due to <DELAY_REASON>'}."
            31-"The portion of the platform located </LOCATION> will be out of service due to platform rehabilitation.  Please board your inbound trains from the in-service portion of the platform located </LOCATION>. Metra apologizes for any inconvenience and thanks you for your patience as we make these station improvements. Please do not go around barricades and allow yourself additional time to reach your boarding platform safely.",
            32-"The <STATION> elevator is temporarily out of service. Recommended alternative accessible locations are </STATION> and </STATION>. Metra will provide  updated information once service has been restored.",
            33-"<STATION> elevator is back in service. Metra apologizes for the inconvenience.",
            34-"<STATION> elevator will be out of service from </START_TIME> to </END_TIME> due to scheduled maintenance. Recommended alternative accessible locations are </STATION> and </STATION>. Metra will provide updated information once service has been restored.",
            35-"Please be advised that, as always, we will make every reasonable effort to assure your timely and comfortable commute.  Unfortunately, weather conditions beyond our control will likely create unanticipated delays or service disruptions.  Depending on the severity of conditions in specific areas your train may experience delays due to weather-related conditions. Metra will continue to utilize all possible  measures available to combat extraordinary  weather conditions.  Please allow extra travel time to assure your safe passage to and from your destination. We regret any unanticipated weather related delays that you may experience and appreciate your patience.  Thank you for choosing Metra for your travel needs.  Please refer to Service Updates for line- specific information.",
            36-"As a reminder, a heat advisory is now in effect for all inbound and outbound trains. Trains may operate behind schedule due to heat related restrictions. Metra apologizes for any inconvenience.",
            37-"Due to upcoming festival alcohol will be restricted after 7pm.",
            38-"Due to upcoming festival alcohol will be prohibited all day.",
            39-"Due to overcrowding on the </NORTH_OR_SOUTH> concourse for your safety commuters are advised to proceed to The Great Hall prior to boarding trains.",
            40-"Due to overcrowding on the </NORTH_OR_SOUTH> concourse for your safety commuters are advised to proceed to area near ticket office or Breezeway between The Great Hall and Amtrak prior to boarding trains."
*/
            //"Inbound and outbound trains are stopped near </LOCATION> due to <DELAY_REASON>. The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "{1, '<LINE>'} inbound and outbound trains are stopped near </LOCATION>{0, ' due to <DELAY_REASON>'}. The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound trains are stopped near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Extensive delays are expected. Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound trains remain stopped near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound trains may be operating </XX> to </XX> minutes behind schedule due to <DELAY_REASON>.  Please listen to platform announcements for the location of your train.",
            "Inbound and outbound train movement  has been halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown. Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound train movement remains halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound train movement  has been halted near </LOCATION> due to <DELAY_REASON>.  The duration of this delay is unknown.  Extensive delays are expected.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "Inbound and outbound train movement has been halted near </LOCATION> due to <DELAY_REASON>. The duration of this delay is unknown.  Passengers may wish to utilize alternate transportation.  Please listen to platform announcements for delay times.  Updates will be provided.",
            "CTA will honor Metra tickets on the </SPECIFIC_CTA_LINES> due to <DELAY_REASON> at </LOCATION> until further notice.",
            "The CTA will no longer honor Metra tickets on the </SPECIFIC_CTA_LINES>.",
            "Train <TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is operating </XX> To </XX> minutes behind schedule due to <DELAY_REASON>.",
            "<DIRECTION> {1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> operating </XX> to </XX> minutes behind schedule due to <DELAY_REASON>.",
            "{1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> stopped near </LOCATION> due to police activity.",
            "{1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} <S/P, 'is/are'> stopped near </STATION> station due to police activity. Commuters on board both trains will board train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, and continue into </SCHED_DESTINATION>.",
            "Due to one track being open to train service, commuters on board {1, 'train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} will detrain at the </STATION> station and board train #</TRAIN_NUM> scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>  and continue their commute.",
            "The following trains are on the move through the <DELAY_REASON> site at </LOCATION> with Some Schedule Modifications. {1, 'train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>'} are operating approximately </XX> minutes late and will make all scheduled station stops.",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME> operating approximately </XX> minutes late will detrain commuters between </STATION> station and </STATION> station and express into <SCHED_DESTINATION>   Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate commuters between </STATION> station and </STATION> station and make all scheduled station stops.",
            "{1, 'train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>'} will originate at </NEW_ORIGIN> due to <DELAY_REASON>.  Passengers from </STATION>  to </STATION>, will be bussed to </STATION> where a waiting train will make all scheduled stops to <SCHED_DESTINATION>. The bus will arrive </STATION> at approximately </X:XXAM/PM>. ",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </STATION> to </STATION>.  Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate the passengers at intermediate stops.",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </STATION> to </STATION>.  Train #</TRAIN_NUM>, scheduled to arrive </SCHED_DESTINATION> at </SCHED_DEST_TIME>, will accommodate the passengers at intermediate stops.",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will make extra stop at </STATIONS>.  Please listen to platform announcements",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will express from </LOCATION> to </LOCATION>.  An extra train following behind this train will accommodate passengers.  Please listen to platform announcements for the location of the extra train.",
            "Train #<TRAIN_NUM> scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will make flag stops enroute to <SCHED_DESTINATION>.  Please stand behind the yellow line and remain visible to the Engineer as this train approaches your station.",
            "<DIRECTION> Train #<TRAIN_NUM> Scheduled to Depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will Originate at </STATION>.  The next train making station stops from </STATION> to </STATION> will be train #</TRAIN_NUM>.",
            "Train #<TRAIN_NUM> scheduled to arrive </STATION> at <SCHED_DEST_TIME>, will terminate at </LOCATION>, due to <DELAY_REASON>.  Train #</TRAIN_NUM> will accommodate passengers to </STATION>.",
            "Train #<TRAIN_NUM> scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will not operate due to <DELAY_REASON>.  Train #</TRAIN_NUM> will be the next train making all scheduled stops to <SCHED_DESTINATION>.",
            "Train #<TRAIN_NUM> scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME>, will operate.  This is a revised operational change to an earlier website posting.",
            "Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will be delayed in departure due to <DELAY_REASON>. Estimated time of delay is unknown. Metra will provide updated information as it becomes available.",
            "Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, will be delayed in departure due to <DELAY_REASON>. Estimated time of delay will be approximately </XX> to </XX> minutes.",
            "Train #<TRAIN_NUM>, scheduled to depart <SCHED_ORIGIN> at <SCHED_ORIGIN_TIME> and arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is on the move operating </XX> minutes behind schedule due to <DELAY_REASON>.",
            "Train #<TRAIN_NUM>, scheduled to arrive <SCHED_DESTINATION> at <SCHED_DEST_TIME>, is operating <DELAY_TIME> minutes behind schedule{0, ' due to <DELAY_REASON>'}.",
            "The portion of the platform located </LOCATION> will be out of service due to platform rehabilitation.  Please board your inbound trains from the in-service portion of the platform located </LOCATION>. Metra apologizes for any inconvenience and thanks you for your patience as we make these station improvements. Please do not go around barricades and allow yourself additional time to reach your boarding platform safely.",
            "The <STATION> elevator is temporarily out of service. Recommended alternative accessible locations are </STATION> and </STATION>. Metra will provide  updated information once service has been restored.",
            "<STATION> elevator is back in service. Metra apologizes for the inconvenience.",
            "<STATION> elevator will be out of service from </START_TIME> to </END_TIME> due to scheduled maintenance. Recommended alternative accessible locations are </STATION> and </STATION>. Metra will provide updated information once service has been restored.",
            "Please be advised that, as always, we will make every reasonable effort to assure your timely and comfortable commute.  Unfortunately, weather conditions beyond our control will likely create unanticipated delays or service disruptions.  Depending on the severity of conditions in specific areas your train may experience delays due to weather-related conditions. Metra will continue to utilize all possible  measures available to combat extraordinary  weather conditions.  Please allow extra travel time to assure your safe passage to and from your destination. We regret any unanticipated weather related delays that you may experience and appreciate your patience.  Thank you for choosing Metra for your travel needs.  Please refer to Service Updates for line- specific information.",
            "As a reminder, a heat advisory is now in effect for all inbound and outbound trains. Trains may operate behind schedule due to heat related restrictions. Metra apologizes for any inconvenience.",
            "Due to upcoming festival alcohol will be restricted after 7pm.",
            "Due to upcoming festival alcohol will be prohibited all day.",
            "Due to overcrowding on the </NORTH_OR_SOUTH> concourse for your safety commuters are advised to proceed to The Great Hall prior to boarding trains.",
            "Due to overcrowding on the </NORTH_OR_SOUTH> concourse for your safety commuters are advised to proceed to area near ticket office or Breezeway between The Great Hall and Amtrak prior to boarding trains."
        };

        public String[] GetAlertTemplateName()
        {
            return AlertTemplateName;
        }

        public String[] GetAlertTemplateHeader()
        {
            return AlertTemplateHeader;
        }

        public String[] GetAlertTemplateText()
        {
            return AlertTemplateText;
        }

    }
}
