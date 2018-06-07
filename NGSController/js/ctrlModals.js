app.controller('ModalInstanceCtrl', ['$scope', '$http', '$location', '$uibModalInstance', '$uibModal', function ($scope, $http, $location, $uibModalInstance, $uibModal) {
    $scope.close = function (data) {
        $uibModalInstance.close(data);
    };
    $scope.addAndClose = function (nameController, nameItem, nameLevel) {
        // var e = $scope.$resolve.myFileChange;
        $scope.progOk = true;
        $scope.hiddenFile = false;
        $scope.add(nameController, nameItem, nameLevel);
        $scope.cancel();
        var myEl = angular.element(document.querySelector('#uploadFile'));
        myEl.addClass('wait');
    };
    $scope.saveDownloadLevel0AndClose = function (nameFun) {
        $scope.progOk = true;
        $scope.hiddenFile = false;
        $scope[nameFun]("uploadLevel0", {
            NumRun: $scope.Run,
            BarcodeCantrige: $scope.BarcodeCantrige,
            BarcodeP1: $scope.BarcodeP1,
            BarcodeP2: $scope.BarcodeP2,
            BarcodeP3: $scope.BarcodeP3,
            BarcodeP4: $scope.BarcodeP4,
        });
        //$scope.SaveLevel0("uploadLevel0", {
        //    BarcodeCantrige: $scope.BarcodeCantrige,
        //    BarcodeP1: $scope.BarcodeP1,
        //    BarcodeP2: $scope.BarcodeP2,
        //    BarcodeP3: $scope.BarcodeP3,
        //    BarcodeP4: $scope.BarcodeP4,
        //});
        $scope.cancel();
        var myEl = angular.element(document.querySelector('#uploadFile'));
        myEl.addClass('wait');
    };
    $scope.Save = function (PrintM) {
        $scope.mySave();
     //   $scope.mySave(nameController, misItem);
        $scope.close({
            SaveSuccess: $scope.$resolve.saveSuccess,
            BtnUpdate: $scope.$resolve.updateOk,
            BtnInsert: $scope.$resolve.insertOk,
            BtnUpdateCencel: $scope.$resolve.updateBitul,
            Progress: $scope.$resolve.progOk,
            // FileName: $scope.$resolve.fileName,
            'printModal': PrintM// 'ModalSave'
        });
    };
    $scope.Send = function (PrintM) {
        $scope.sendEmail();
        $scope.close({
            SaveSuccess: $scope.$resolve.saveSuccess,
            BtnUpdate: $scope.$resolve.updateOk,
            BtnInsert: $scope.$resolve.insertOk,
            BtnUpdateCencel: $scope.$resolve.updateBitul,
            Progress: $scope.$resolve.progOk,
            // FileName: $scope.$resolve.fileName,
            'printModal': PrintM// 'ModalSave'
        });
    };
    $scope.updateAnd = function (nameCtroll, nameItem) {
        $scope.saveSuccess = " ";
        $scope.myUpdate(nameCtroll, nameItem);
    };
    $scope.updateAndSend = function () {
        $scope.saveSuccess = " ";
        $scope.myUpdateAndSend();
    };
    
    $scope.myClear = function () {
        $scope.saveSuccess = " ";
        $scope.cancel();
    }
    $scope.printModal = function (divName) {
        $scope.close({ 'printModal': divName })
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.ifBarcode = function (nameModal) {
        if ($scope.csvFileBarcode == null) {
            //alert("Barcode not inserted, you want to proceed anyway click Continue");
            // $scope.close({ csvFileBarcode: $scope.csvFileBarcode, 'printModal': 'ModalEmptyBarcode' });
        }
        else {
            $scope.close({ csvFileBarcode: $scope.csvFileBarcode, 'printModal': nameModal });
        }
    }
    function validFileType(file, typeFile) {
        $scope.t = file.name.split('.');
        switch (typeFile) {
            case 1: if (angular.equals($scope.t[1], 'csv'))
                    return true;
            case 2: {/*if (file.type === 'application/vnd.ms-excel') */
                if (angular.equals($scope.t[1], 'csv'))
                    return true;
            }
            case 3: {
                if (angular.equals($scope.t[1], 'xlsm'))
                    return true;
            }
            case 4: {
                if (angular.equals($scope.t[1], 'xlsm'))
                    return true;
            }
        }
        return false;
    }
    $scope.ifRunsWithBarcode = function () {
        var request = $http({
            method: 'POST',
            url: "api/uploadLevel0?id=1",
            data: '=' + JSON.stringify({
                NumRun: $scope.Run,
                BarcodeCantrige: $scope.BarcodeCantrige,
                BarcodeP1: $scope.BarcodeP1,
                BarcodeP2: $scope.BarcodeP2,
                BarcodeP3: $scope.BarcodeP3,
                BarcodeP4: $scope.BarcodeP4,
            }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' }
            //responseType: "arraybuffer"
        });
        request.then(
            function (response) {
                $scope.Run = response.data;
            },
            function (error) {
                $rootScope.progOk = false;
                $rootScope.insertOk = false;
                $rootScope.SaveSuccess = error.statusText;
                if (angular.equals(error.statusText, 'Bad Request'))
                    $window.location.href = 'index.html';
            });
    };
    //$scope.myTest = function (mis) {
    //    var csvFileBarcode = $scope.$resolve.csvFileBarcode.replace(/ /g, ''); //מוריד רווחים...trim();
    //    var AllnameFile = document.forms.myFileChange.file.files[0].name;
    //    var stepOk = stepFile(mis, 'tooltipIsOpenStep' + mis, AllnameFile);
    //    if (stepOk == false)
    //        {
    //        if (validFileType(document.forms.myFileChange.file.files[0], mis)) {
    //            var nameFile = [];
    //            //if (mis == 2)
    //            //    if (AllnameFile.indexOf('(') !== -1)
    //            //    { 
    //            //        nameFile = AllnameFile.split('(');
    //            //    }
    //            nameFile = AllnameFile.split('-');
    //            nameFile[0] = nameFile[0].replace(/ /g, '');//document.getElementById('file').value; //נתיב
    //            //var AllnameFile = document.getElementById('file').files[0].name;
    //            if (!angular.equals(csvFileBarcode, nameFile[0])) {
    //                $scope.ddd = true;
    //                alert('The file does not match the barcode name, if you are sure you want to continue? Click continue and if not search for another file');
    //            }
    //            $scope.PayoutEnabled = false;
    //            $scope['tooltipIsOpenType' + mis] = false;      
    //            $scope['tooltipIsOpenStep' + mis] = false;      
    //        }
    //        else {
    //            $scope['tooltipIsOpenType' + mis] = !$scope['tooltipIsOpenType' + mis];      
    //            $scope.PayoutEnabled = true;
    //        }
    //    }
    //}
    //function stepFile(mis, nameTool, namefile) {
    //    if (namefile.indexOf('s' + mis) < 0){
    //        $scope[nameTool] = !$scope[nameTool];
    //        return true;
    //    }
    //    return false;
    //}
}]);