(function () {
    'use strict';
    mtcApp.controller('reportSurveyResultsController',
        function reportSurveyResultsController($scope, $rootScope, reportService) {

            $scope.header = "Survey Results";
            $scope.isBusyGettingSurveys = false;
            $scope.isBusyGettingSurveyQuestions = false;
            $scope.isBusy = false;
            $scope.isBusyExporting = false;
            $scope.surveys = [];
            $scope.questions = [];
            $scope.answers = [];
            $scope.questionsAndAnswers = [];


            $scope.query = {
                surveyId: '',
                questionId: '',
                format: 'json'
            };

            $scope.getSurveys = function () {
                $scope.isBusyGettingSurveys = true;
                reportService.getSurveys().then(function (results) {
                    $scope.surveys = results;
                    $scope.isBusyGettingSurveys = false;
                });
            };
            $scope.getSurveys();

            $scope.getSurveyQuestions = function () {
                $scope.isBusyGettingSurveyQuestions = true;
                reportService.getSurveyQuestions($scope.query.surveyId).then(function (results) {
                    $scope.questions = results;
                    $scope.isBusyGettingSurveyQuestions = false;
                });
            };

            $scope.getSurveyQuestionAnswers = function () {
                $scope.isBusy = true;
                reportService.getSurveyQuestionAnswers($scope.query.questionId).then(function (results) {
                    $scope.answers = results;
                    $scope.isBusy = false;
                    $rootScope.redrawTable();
                });
            };

            $scope.getSurveyQuestionsAndAnswers = function () {
                $scope.isBusy = true;
                reportService.getSurveyQuestionsAndAnswers($scope.query).then(function (results) {
                    $scope.questionsAndAnswers = results;
                    $scope.isBusy = false;
                    $rootScope.redrawTable();
                });
            };

            $scope.exportReportData = function () {
                $scope.isBusyExporting = true;

                //requesting excel and not json
                $scope.query.format = 'excel';

                reportService.getSurveyQuestionsAndAnswers($scope.query).then(function (results) {
                    $scope.isBusyExporting = false;
                    var element = angular.element('<a/>');
                    element.attr({
                        href: 'data:attachment/csv;charset=utf-8,' + encodeURI(results),
                        target: '_blank',
                        download: 'SurveyResults.csv'
                    })[0].click();
                });
            };

        }
    );
}());