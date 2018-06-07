var app = angular.module('app', ["ngRoute", 'ui.bootstrap']).run(['$anchorScroll', function ($anchorScroll) {/*'ui.router'*/
    $anchorScroll.yOffset = 100;   // always scroll by 50 extra pixels
}]);
app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        //.when("/", {
        //    templateUrl: "main.htm"
        //})
        .when("/Level1", {
            templateUrl: "Levels/Level1.html",
            controller: "ctrl2"
        })
        .when("/Level2", {
            templateUrl: "Levels/Level2.html",
            controller: "ctrl2"
        })
        .when("/Level3", {
            templateUrl: "Levels/Level3.html",
            controller: "ctrl2"
        })
        .when("/Level4", {
            templateUrl: "Levels/Level4.html",
            controller: "ctrl2"
        })
        .when("/MpileupInsertions", {
            templateUrl: "Levels/LevelMpileupInsertions.html",
            controller: "ctrl2"
        })
        .when("/Level5", {
            templateUrl: "Levels/Level5.html",
            controller: "ctrl2"
        })
        .when("/Level6", {
            templateUrl: "Levels/Level6.html",
            controller: "ctrl2"
        })
        .when("/Level7", {
            templateUrl: "Levels/Level7.html",
            controller: "ctrl2"
        })
        .when("/Level8", {
            templateUrl: "Levels/Level8.html",
            controller: "ctrl2"
        })
        .when("/Level9", {
            templateUrl: "Levels/Level9.html",
            controller: "ctrl2"
        })
       .when("/file3", {
        templateUrl: "Levels/Level1.html",
        controller: "ctrl2"
    });
}]);


//app.config(function ($stateProvider, $urlRouterProvider) {

//    $urlRouterProvider.when("", "/page1");

//    $stateProvider
//        .state("Level1", {
//            url: "/Level1",
//            templateUrl: "Levels/Level1.html",
//            controller: "ctrl2"
//        })
//        .state("Level2", {
//            url: "/Level2",
//            templateUrl: "Levels/Level2.html",
//            controller: "ctrl2"
//        })
//        .state("page3", {
//            url: "/page3",
//            templateUrl: "Page-3.html"
//        });
//});