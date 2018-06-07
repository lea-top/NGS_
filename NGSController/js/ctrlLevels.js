app.controller('ctrl2', ['$scope', '$http', '$filter','$compile', function ($scope, $http,$filter,$compile) {
    $scope.mySelect = function (x, id, nameItem) {
        this.items[id - 1].ManualComments += x + " ";
    };
    $scope.mySelectFromLevel3 = function (x, id) {
        this.items3[id - 1].ManualComments += x + " ";
        if (angular.equals(x, 'ref')) {
            this.items3[id - 1].Color = "lightgreen";
            this.items3[id - 1].ColorResults = "lightgreen";
            if (!angular.equals(this.items3[id - 1].ManualComments, "ref "))
                alert("Sampling with notes, it is recommended to choose another sample");
        }
        if (this.items3[id - 1].ManualComments.indexOf("ref")==-1) {
            this.items3[id - 1].Color = "";
            this.items3[id - 1].ColorResults = "";
        }
    };
    $scope.ifClearManual = function (id) {
        if (this.items3[id - 1].ManualComments.indexOf("ref") == -1) {
            if (this.items3[id - 1].Exon_CN_8==1){
                this.items3[id - 1].Color = "red";
                this.items3[id - 1].ColorResults = "red";
            }
            else{
            this.items3[id - 1].Color = "";
            this.items3[id - 1].ColorResults = "";
            }
        }
       else if (this.items3[id - 1].ManualComments.indexOf("ref") > -1) {
            this.items3[id - 1].Color = "lightgreen";
            this.items3[id - 1].ColorResults = "lightgreen";
       }
    };
    $scope.mySelectFromLevel4 = function (x, id) {
        this.items4[id - 1].ManualComments += x + "; ";
    };
    $scope.showExon7 = true;
    $scope.ifChange = function (id) {
        var x = this.items4[id - 1].Results;
        if (this.items4Draft[id - 1].Results.indexOf(x)) {//לא כמו הקודם
           $scope["readonly" + id] = false;

            if (this.items4[id - 1].ManualComments.indexOf("Manual Results") ==-1){//לא שינו כבר לרשום הודעה
                this.items4[id - 1].ManualComments += " Manual Results; ";
            }
            if (angular.equals(x, 'PBS')){
                this.items4[id - 1].ColorResults = "yellow";
                this.items4[id - 1].Exon_7 = '2';
            }
            if (angular.equals(x, 'UTA')) {
                this.items4[id - 1].ColorResults = "mediumpurple";
                this.items4[id - 1].Exon_7 = '';
            }
            if (angular.equals(x, 'REP')) {
                this.items4[id - 1].ColorResults = "skyblue";
                this.items4[id - 1].Exon_7 = '8';
            }  
            if (angular.equals(x, 'IBS')) {
                this.items4[id - 1].ColorResults = "palegreen";
                this.items4[id - 1].Exon_7 = '2.5';
            } 
            if (angular.equals(x, 'INC')) {
                this.items4[id - 1].ColorResults = "lightgrey";
                this.items4[id - 1].Exon_7 = '1.5';
            }
            if (angular.equals(x, 'POS')) {
                this.items4[id - 1].ColorResults = "red";
                this.items4[id - 1].Exon_7 = '1';
            }
            if (angular.equals(x, 'NEG')) {
                this.items4[id - 1].ColorResults = "";
                this.items4[id - 1].Exon_7 = '';
            } 
            if (angular.equals(x, 'AFF')) {
                this.items4[id - 1].ColorResults = "pink";
                this.items4[id - 1].Exon_7 = '0';
            }    

        }   
        else {/*if (angular.equals(x, 'REP')) */
            if (this.items4[id - 1].ManualComments.indexOf("Manual Results") != -1)
            {
                this.items4[id - 1].ColorResults = "skyblue";
                this.items4[id - 1].Exon_7 = 8;
                var index = this.items4[id - 1].ManualComments.indexOf("Manual Results");
                var first = this.items4[id - 1].ManualComments.substring(0, index );
                var end = this.items4[id - 1].ManualComments.substring(index + 14, this.items4[id - 1].ManualComments.lenght);
                this.items4[id - 1].ManualComments = first + end;
            }
               
        }
    };
    $scope.myClose = function () {
        $scope.table = true;
        $scope.showAgain = true;
    }
    $scope.showAgainF = function () {
        $scope.table = false;
        $scope.showAgain = false;
    }
    $scope.showAddSelect1 = function () {
        $scope.showAddSelect = !$scope.showAddSelect;
        $scope.newSelect = " ";
        $scope.massageErrorSelect = false;
        $scope.onDeleteSelect = false;
        $scope.onSelect = false;
    }
    function massageToFalse() {
        $scope.massageErrorSelect = false;
        $scope.onDeleteSelect = false;
        $scope.onSelect = false;
    }
    $scope.addNewSelect = function (nameController, nameItem) {
        massageToFalse();
        var request = $http({
            method: 'POST',
            url: "api/" + nameController,/*Select*/
            data: '=' + $scope.newSelect,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' }
        });
        request.then(
            function (response) {
                if (angular.equals(response.data, "הנתונים נוספו לטבלה")) {
                    $scope.onSelect = true;
                    $scope.GetManualCommentsEnum(nameController, nameItem);
                }
                else {
                    $scope.errorSelect = response.data;
                    $scope.massageErrorSelect = true;
                }
            },
            function (error) {
                if (error.data.ExceptionMessage != null) {
                    $scope.errorSelect = error.data.ExceptionMessage;
                    $scope.massageErrorSelect = true;
                }
               else alert(error.statusText);
            });
    };
    $scope.deleteSelect = function (nameController, nameItem) {
        massageToFalse();
        var request = $http({
            method: 'POST',
            url: "api/" + nameController + "?id=1",/*DeleteSelect*/
            data: '=' + $scope.newSelect,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8' }
        });
        request.then(
            function (response) {
                if (angular.equals(response.data, "הנתונים נמחקו")) {
                    $scope.onDeleteSelect = true;
                    $scope.GetManualCommentsEnum(nameController, nameItem);
                }
                else {
                    $scope.errorSelect = response.data;
                    $scope.massageErrorSelect = true;
                }
            },
            function (error) {
                if (error.data.ExceptionMessage != null) {
                    $scope.errorSelect = error.data.ExceptionMessage;
                    $scope.massageErrorSelect = true;
                }
                else  alert(error.statusText);
            });
    }; 
}]);


