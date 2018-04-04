angular.module("mobacs").directive('uiDateMonthYearMonthYear',function($filter,$ionicPopup,SystemService){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatDateMonthYear = function(DateMonthYear){
        if(DateMonthYear == undefined){
          DateMonthYear = null;
        }
        DateMonthYear = DateMonthYear.replace(/[^0-9]+/g,"");
        if(DateMonthYear.length > 2){
          DateMonthYear = DateMonthYear.substring(0,2) + "/" + DateMonthYear.substring(2);
        }
        if(DateMonthYear.length > 5){
          DateMonthYear = DateMonthYear.substring(0,7);
        }

        if(DateMonthYear.length == 7){
          var RegExp = /(((0[123456789]|10|11|12)([/])(([1][9][0-9][0-9])|([2][0-9][0-9][0-9]))))/;
          if(RegExp.test(DateMonthYear) != true){
            DateMonthYear = null;
            $ionicPopup.alert({
              title: 'Data Inválida',
              template: 'Por favor insira uma data valida!'
            })
          }
          if(SystemService.isSystemDate(DateMonthYear)){
            DateMonthYear = null;
            $ionicPopup.alert({
              title: 'Data Inválida',
              template: 'Por favor insira uma data valida!'
            })
          }
        }
        return DateMonthYear;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatDateMonthYear(ctrl.$viewValue));
        ctrl.$render();
      });

      ctrl.$parsers.push(function(value){
        if(value.length === 7){
          var DateMonthYearArray = value.split("/");
          return new Date(DateMonthYearArray[1],DateMonthYearArray[0]-1,01).getTime();
        }
      });


      ctrl.$formatters.push(function(value){
        return $filter("date")(value,"MM/yyyy");
      });

      // ctrl.$formatters.push(function(value){
      //   var date = $filter("date")(value,"dd/MM/yyyy");
      //   console.log(date);
      //   // return null;
      // });
    }
  };
});
