(function () {
    'use strict';
    mtcApp.factory('reportService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getAssistsLogged: function (vm) {
                console.time('Getting Assists Logged');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Reports/GetAssistsLogged'
                }).
                then(function (response) {
                    console.timeEnd('Getting Assists Logged');
                    return response.data;
                });
            },
            getSurveys: function () {
                console.time('Getting Surveys');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Reports/GetSurveys'
                }).
                then(function (response) {
                    console.timeEnd('Getting Surveys');
                    return response.data;
                });
            },
            getSurveyQuestions: function (surveyId) {
                console.time('Getting Survey Questions');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Reports/GetSurveyQuestions?surveyId=' + surveyId
                }).
                then(function (response) {
                    console.timeEnd('Getting Survey Questions');
                    return response.data;
                });
            },
            getSurveyQuestionAnswers: function (questionId) {
                console.time('Getting Survey Question Answers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Reports/GetSurveyQuestionAnswers?questionId=' + questionId
                }).
                then(function (response) {
                    console.timeEnd('Getting Survey Question Answers');
                    return response.data;
                });
            },
            getSurveyQuestionsAndAnswers: function (vm) {
                console.time('Getting Survey Questions and Answers');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Reports/GetSurveyQuestionsAndAnswers'
                }).
                then(function (response) {
                    console.timeEnd('Getting Survey Questions and Answers');
                    return response.data;
                });
            },
            getAlarmReport: function (vm) {
                console.time('Getting Alarm Report');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Reports/GetAlarmReport'
                }).
                then(function (response) {
                    console.timeEnd('Getting Alarm Report');
                    return response.data;
                });
            },
            getInvoiceSummaryReport: function (m, e) {
                console.time('Getting Invoice Summary Report');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Reports/GetInvoiceSummary?month=' + m + '&export=' + e
                }).
                then(function (response) {
                    console.timeEnd('Retreived Invoice Summary Report');
                    return response.data;
                });
            },
            GetWazeMessagesReport: function () {
                console.time('Getting Waze Messages Report');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Reports/GetWazeMessagesReport'
                }).
                    then(function (response) {
                        console.timeEnd('Retreived Waze Messages Report');
                        return response.data;
                    });
            }
        }

    });
}());