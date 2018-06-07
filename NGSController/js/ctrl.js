
app.controller('ctrl', ['$scope', '$rootScope', '$http', '$location', '$uibModal', '$window', '$filter', '$compile', '$anchorScroll', function ($scope, $rootScope, $http, $location, $uibModal, $window, $filter, $compile, orderByArrResultsFilter, $anchorScroll) {
    $scope.csvFileBarcode = '';
    $scope.csvFileBarcode1 = null;
    $scope.csvFileBarcode2 = null;
    $scope.csvFileBarcode3 = null;
    $scope.animationsEnabled = false;
    $scope.printModal = function (divName) {
        var ModalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'Modals/' + divName + '.html',
            controller: 'ModalInstanceCtrl',
            backdrop: false,
            //   windowClass: 'app-modal-window',
            resolve: {
                BarcodeP1: function () {
                    return $scope.BarcodeP1;
                },
                BarcodeP2: function () {
                    return $scope.BarcodeP2;
                },
                BarcodeP3: function () {
                    return $scope.BarcodeP3;
                },
                BarcodeP4: function () {
                    return $scope.BarcodeP4;
                },
                BarcodeCantrige: function () {
                    return $scope.BarcodeCantrige;
                },

                csvFileBarcode: function () {
                    return $scope.csvFileBarcode;
                },
                csvFileBarcode1: function () {
                    return $scope.csvFileBarcode1;
                },
                myFileChange: function () {
                    return $scope.myFileChange;
                },
                SaveSuccess: function () {
                    return $rootScope.SaveSuccess;
                },
                BtnUpdate: function () {
                    return $rootScope.updateOk;
                },
                BtnInsert: function () {
                    return $rootScope.insertOk;
                },
                BtnUpdateCencel: function () {
                    return $rootScope.updateBitul;
                },
                Progress: function () {
                    return $rootScope.progOk;
                },
                HistoryItems: function () {
                    return $rootScope.HistoryItem;
                },
                ResultsEnums: function () {
                    return $rootScope.ResultsEnum;
                },
                GetManualCommentsEnum3: function () {
                    return $rootScope.ManualCommentsEnum3;
                },
                SampleNames: function () {
                    return $rootScope.SampleName;
                }
            }
        });
        ModalInstance.result.then(function (result) {
            $scope.BarcodeP1 = result.BarcodeP1;
            $scope.BarcodeP2 = result.BarcodeP2;
            $scope.BarcodeP3 = result.BarcodeP3;
            $scope.BarcodeP4 = result.BarcodeP4;
            $scope.BarcodeCantrige = result.BarcodeCantrige;
            $scope.csvFileBarcode = result.csvFileBarcode;
            $scope.csvFileBarcode1 = result.csvFileBarcode1;
            $scope.myFileChange = result.myFileChange;
            $rootScope.SaveSuccess = result.SaveSuccess;
            $rootScope.updateOk = result.BtnUpdate;
            $rootScope.insertOk = result.BtnInsert;
            $rootScope.updateBitul = result.BtnUpdateCencel;
            $rootScope.progOk = result.Progress;
            $rootScope.HistoryItem = result.HistoryItem;
            $rootScope.ResultsEnum = result.ResultsEnum;
            $rootScope.ManualCommentsEnum3 = result.GetManualCommentsEnum3;
            $rootScope.SampleName = result.SampleNames;
            // $rootScope.fileName = result.FileName;
            if (result.printModal) {
                $scope.printModal(result.printModal)
            }
        }, function () {
        });
    }
    $scope.printDivModal = function (divName, sizeM = 'md') {
        var ModalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            animation: true,
            templateUrl: 'Modals/' + divName + '.html',
            controller: 'ModalInstanceCtrl',
            size: sizeM,//lg
            //controllerAs: 'ctrl',
            backdrop: false,
            windowClass: 'modal-lg-History'
        });
        ModalInstance.result.then(function (result) {
            if (result.BarcodeP1) {
                $scope.BarcodeP1 = result.BarcodeP1;
            }
            if (result.BarcodeP2) {
                $scope.BarcodeP2 = result.BarcodeP2;
            }
            if (result.BarcodeP3) {
                $scope.BarcodeP3 = result.BarcodeP3;
            }
            if (result.BarcodeP4) {
                $scope.BarcodeP4 = result.BarcodeP4;
            }
            if (result.BarcodeCantrige) {
                $scope.BarcodeCantrige = result.BarcodeCantrige;
            }
            if (result.csvFileBarcode) {
                $scope.csvFileBarcode = result.csvFileBarcode;
            }
            if (result.csvFileBarcode1) {
                $scope.csvFileBarcode1 = result.csvFileBarcode1;
            }
            if (result.SaveSuccess) {
                $rootScope.SaveSuccess = result.SaveSuccess;
            }
            if (result.BtnUpdate) {
                $rootScope.updateOk = result.BtnUpdate;
            }
            if (result.BtnInsert) {
                $rootScope.insertOk = result.BtnInsert;
            }
            if (result.BtnUpdateCencel) {
                $rootScope.updateBitul = result.BtnUpdateCencel;
            }
            if (result.Progress) {
                $rootScope.progOk = result.Progress;
            }
            if (result.HistoryItems) {
                $rootScope.HistoryItem = result.HistoryItems;
            }
            if (result.ResultsEnums) {
                $rootScope.ResultsEnum = result.ResultsEnums;
            }
            //if (result.FileName) {
            //    $rootScope.fileName = result.FileName;
            //}
            if (result.GetManualCommentsEnum3) {
                $rootScope.ManualCommentsEnum3 = result.GetManualCommentsEnum3;
            }
            if (result.SampleNames) {
                $rootScope.SampleName = result.SampleNames;
            }
            if (result.printModal) {
                $scope.printModal(result.printModal)
            }
        }, function () {
        });
    }
    $rootScope.SearchQuery = '';

    $rootScope.ResultsEnum = ['REP', 'AFF', 'NEG', 'POS'];

    $rootScope.items1 = [];
    $scope.addWithBarcode = function (nameController, idPath, barcodePlate, nameLevel) {
        $rootScope.MassegeSamplesNotInStep1 = "";
        var request = $http({
            method: 'POST',
            url: "api/" + nameController + "?idPath=" + idPath + "&barcodePlate=" + barcodePlate,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' }/*,*/
            //responseType: "arraybuffer"
        });
        request.then(
            function (response) {
                if (response.data != null) {
                    $scope.ifHide = 0;
                   
                    $rootScope.MassegeSamplesNotInStep1 = response.data.MassegeSamplesNotInStep1;
                    alert($rootScope.MassegeSamplesNotInStep1);
                    $rootScope.Production = response.data.ResultProduction;
                    $rootScope.items1 = response.data.ResultProduction.ListLevel1;
                    $rootScope.items9 = response.data.ResultProduction.ListLevel9;
                    $rootScope.items2 = response.data.ResultProduction.ListLevel2;
                    $rootScope.items3 = response.data.ResultProduction.ListLevel3;
                    $rootScope.items4 = response.data.ResultProduction.ListLevel4;
                    $rootScope.MpileupInsertions = response.data.ResultProduction.ListMpileupInsertions;
                    $rootScope.items5 = response.data.ResultProduction.ListLevel5;
                    $rootScope.items6 = response.data.ResultProduction.ListLevel6;
                    $rootScope.items8 = response.data.ResultProduction.ListLevel8;

                    //angular.element(document.getElementsByClassName('wait')).removeClass('wait');
                    //if ($scope.ReadOnlyPlate === true) {
                    //    alert("The file has already been sent to the office and can not be changed but read-only.");
                    //    $scope.readOnly = true;
                    //}
                    //else $scope.readOnly = false;
                    $scope.goNext("/" + nameLevel);//Level2 
                    $rootScope.progOk = false;
                    //$location.hash("tableFor" + nameLevel);
                    //$anchorScroll();
                }

                else alert("data is null");
            },
            function (error, s, a, b) {
                //$scope[nameItem] = "";
                alert(error.statusText);
                angular.element(document.getElementsByClassName('wait')).removeClass('wait');
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });

    }
    $rootScope.add = function (nameController, nameItem, nameLevel) {
        //initItems(nameLevel);
        var payload = new FormData();
        payload.append("title", "Dvori");
        var l = document.forms.myFileChange.file.files.length;
        $rootScope.NameFile = document.forms.myFileChange.file.value;
        var i = 0;
        for (; i < l; i++) {
            payload.append(file.name + i, document.forms.myFileChange.file.files[i]);
        }
        if (i !== 6) {
            alert("There are only " + i + " files and you need 6")
        }
           
        else {
            /*payload.append('file', document.forms.myFileChange.file.files);*///[0]*document.getElementById('file').files[0]*/
            var request = $http.post("api/" + nameController, payload, {//Upload
                withCredentials: false,
                headers: {
                    'Content-Type': undefined /*'multipart/form-data'*/
                },
                transformRequest: angular.identity,
                params: {
                    payload
                },
                //data: '=' + JSON.stringify("111111"),
            })
            request.then(
                function (response) {
                    if (response.data != null) {
                        $rootScope.idPath = response.data;

                        $scope.addWithBarcode(nameController, $rootScope.idPath, $scope.csvFileBarcode, nameLevel);// "123456" barcodePlate
                        //$scope.ifHide = 0;
                        //$rootScope.Production = response.data;
                        //$rootScope.items1 = response.data.ListLevel1;
                        //$rootScope.items9 = response.data.ListLevel9;
                        //$rootScope.items2 = response.data.ListLevel2;
                        //$rootScope.items3 = response.data.ListLevel3;
                        //$rootScope.items4 = response.data.ListLevel4;
                        //$rootScope.items5 = response.data.ListLevel5;
                        //$rootScope.items6 = response.data.ListLevel6;
                        //$rootScope.items8 = response.data.ListLevel8;

                        //angular.element(document.getElementsByClassName('wait')).removeClass('wait');
                        //if ($scope.ReadOnlyPlate === true) {
                        //    alert("The file has already been sent to the office and can not be changed but read-only.");
                        //    $scope.readOnly = true;
                        //}
                        //else $scope.readOnly = false;
                        $scope.goNext("/" + nameLevel);//Level2 
                        $rootScope.progOk = false;
                        //$location.hash("tableFor" + nameLevel);
                        //$anchorScroll();
                    }

                    else alert("data is null");
                },
                function (error, s, a, b) {
                    $scope[nameItem] = "";
                    alert(error.statusText);
                    angular.element(document.getElementsByClassName('wait')).removeClass('wait');
                    if (angular.equals(error.statusText, 'Bad Request'))
                        $window.location.href = 'index.html';
                });
        }
    }
   
    $rootScope.goNext = function (hash) {
        $location.path(hash);
    }

    //$scope.GetItems2 = function () {
    //    var request = $http({
    //        method: 'POST',
    //        url: 'api/uploadLevel2',
    //    });
    //    request.then(
    //        function (response) {
    //            $scope.ifHide = 0;
    //            $rootScope.items2 = response.data;
    //        },
    //        function (error) {
    //                alert(error.statusText);
    //            if (angular.equals(error.statusText, 'Bad Request'))
    //                $window.location.href = 'index.html';
    //        });
    //}
    //$scope.GetItems2();
    $rootScope.getCSSClass = function (color) {
        return color + '-class';
    };

    //$scope.ifHide = 0;
    //$rootScope.hideRowDooplicate = function (index) {
    //    if ( index - $scope.ifHide == 1) {/*<*/
    //        $scope.ifHide = index;
    //        return true;
    //    }
    //    $scope.ifHide = index;
    //    return false;
    //};


    $rootScope.SaveLevel0 = function (nameController, data) {
        //$rootScope.SaveSuccess = "";
        //$rootScope.progOk = true;
        //$rootScope.updateOk = false;
        //$rootScope.updateBitul = false;[$scope.BarcodeCantrige, $scope.BarcodeP1, $scope.BarcodeP2, $scope.BarcodeP3, $scope.BarcodeP4]
        var request = $http({
            method: 'POST',
            url: "api/uploadLevel0",
            data: '=' + JSON.stringify(data),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' },
            responseType: "arraybuffer"
        });
        request.then(
            function (response) {
                //var myEl = angular.element(document.querySelector('#save1'));
                //myEl.addClass('glyphicon glyphicon-saved');
                //$rootScope.progOk = false;
                //$rootScope.insertOk = false;
                $scope.downloadData = response.data;
                $scope.nameFileXlsx = $scope.nameFile(response);
                var ifDown = $rootScope.downloadXlsx();
                if (ifDown != 0)
                    $rootScope.SaveSuccess = "The data was updated in the table and send to the office.";
                else {
                    $rootScope.SaveSuccess = "Data updated but download failed";
                }
                //$scope.readOnly = true;
            },
            function (error) {
                $rootScope.progOk = false;
                $rootScope.insertOk = false;
                $rootScope.SaveSuccess = error.statusText;
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });
    };
    $rootScope.mySave = function () {
        $rootScope.SaveSuccess = "";
        $rootScope.progOk = true;
        $scope.okBarcode = false;
        var request = $http({
            method: 'POST',
            url: "api/Save",
            data: '=' + JSON.stringify($rootScope.Production),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' }
        });
        request.then(
            function (response) {
                $rootScope.SaveSuccess = response.data;/* "the data saved  ."*/
                $rootScope.progOk = false;
            },
            function (error) {
                $rootScope.SaveSuccess = "";
                $rootScope.progOk = false;
                $rootScope.insertOk = false;
                $rootScope.SaveSuccess = error.statusText + ", so it can not be saved";
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });
    };
    $rootScope.myUpdateAndSend = function (typeFile) {
        //$rootScope.SaveSuccess = "";
        //$rootScope.progOk = true;
        //$rootScope.updateOk = false;
        //$rootScope.updateBitul = false;
        var request = $http({
            method: 'POST',
            url: "api/SaveLevel2?id=" + typeFile + "&idRun=" + $rootScope.Production.RunPlates.NumRun,
            data: '=' + JSON.stringify($rootScope.items6),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' },
            responseType: "arraybuffer"
        });
        request.then(
            function (response) {
                //var myEl = angular.element(document.querySelector('#save1'));
                //myEl.addClass('glyphicon glyphicon-saved');
                //$rootScope.progOk = false;
                //$rootScope.insertOk = false;
                $scope.downloadData = response.data;
                $scope.nameFileXlsx = $scope.nameFile(response);
                var ifDown = $rootScope.downloadXlsx();
                if (ifDown != 0)
                    $rootScope.SaveSuccess = "The data was updated in the table and send to the office.";
                else {
                    $rootScope.SaveSuccess = "Data updated but download failed";
                }
                //$scope.readOnly = true;
            },
            function (error) {
                $rootScope.progOk = false;
                $rootScope.insertOk = false;
                $rootScope.SaveSuccess = error.statusText;
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });
    };
    $rootScope.myUpdateAndSendFinal9 = function () {
        var request = $http({
            method: 'POST',
            url: "api/SaveLevel9Final?id=" + $rootScope.Production.RunPlates.NumRun,
            data: '=' + JSON.stringify($rootScope.items9),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' },
            responseType: "arraybuffer"
        });
        request.then(
            function (response) {
                $scope.downloadData = response.data;
                $scope.nameFileXlsx = $scope.nameFile(response);
                var ifDown = $rootScope.downloadXlsx();
                if (ifDown != 0)
                    $rootScope.SaveSuccess = "The data was updated in the table and send to the office.";
                else {
                    $rootScope.SaveSuccess = "Data updated but download failed";
                }
                //$scope.readOnly = true;
            },
            function (error) {
                $rootScope.progOk = false;
                $rootScope.insertOk = false;
                $rootScope.SaveSuccess = error.statusText;
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });
    };

    $scope.OnlyDownloadXlsx = function () {
        var ifDown = $scope.downloadXlsx();
        if (ifDown == 0)
            alert("file not found! ,the file can only be downloaded after saving");
    }
    $scope.downloadData = null;
    $scope.nameFileXlsx = "";
    $scope.nameFile = function (response) {
        var contentDispositionHeader = response.headers('Content-Disposition');
        var result = contentDispositionHeader.split(';')[1].trim().split('=')[1];
        return result.replace(/"/g, '');
    }
    $rootScope.downloadXlsx = function (response) {
        if ($scope.downloadData != null && $scope.nameFileXlsx != null && $scope.downloadData != "" && $scope.nameFileXlsx != "") {
            $scope.download($scope.downloadData, "vnd.openxmlformats-officedocument.spreadsheetml.sheet", $scope.nameFileXlsx);
        }
        else return 0;
    }
    $scope.download = function (data, type, nameFile) {
        var blob = new Blob([data], { type: type });
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, nameFile);
        }
        else {
            var e = document.createEvent('MouseEvents'),
                a = document.createElement('a');
            a.download = nameFile;
            a.href = window.URL.createObjectURL(blob);
            a.dataset.downloadurl = [type, a.download, a.href].join(':');
            e.initEvent('click', true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            a.dispatchEvent(e);
        }
    }
}]);
app.directive('tooltip', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.hover(function () {
                // on mouseenter
                element.tooltip('show');
            }, function () {
                // on mouseleave
                element.tooltip('hide');
            });
        }
    };
});
//app.filter('orderByArrResults', function () {
//    return function (input, Result, reverse) {//,items4Copy
//        if (angular.equals(Result, '')) {
//            return input;
//        }
//          items4Copy = angular.copy(input);
//        var Results = ['IBS', 'INC', 'REP', 'UTA', 'PBS', 'AFF', 'POS', 'NEG'];
//        var out = [];
//        angular.forEach(Results, function (r) {
//            angular.forEach(input, function (language) {
//                if (language[Result] === r) {
//                    out.push(language);
//                }
//            });
//        });
//        if (reverse) out.reverse();
//        return out;
//    }
//})