angular.module("mobacs").directive('uiDate',function($filter,$ionicPopup,SystemService){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatDate = function(date){
        if(date == undefined){
          date = null;
        }
        date = date.replace(/[^0-9]+/g,"");
        if(date.length > 2){
          date = date.substring(0,2) + "/" + date.substring(2);
        }
        if(date.length > 5){
          date = date.substring(0,5) + "/" + date.substring(5,9);
        }

        if(date.length == 10){
          var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
          if(RegExp.test(date) != true){
            date = null;
            $ionicPopup.alert({
              title: 'Data Inválida',
              template: 'Por favor insira uma data valida!'
            })
          }
          if(SystemService.isSystemDate(date)){
            date = null;
            $ionicPopup.alert({
              title: 'Data Inválida',
              template: 'Por favor insira uma data valida!'
            })
          }
        }
        return date;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatDate(ctrl.$viewValue));
        ctrl.$render();
      });

      ctrl.$parsers.push(function(value){
        if(value.length === 10){
          var dateArray = value.split("/");
          return new Date(dateArray[2],dateArray[1]-1,dateArray[0]).getTime();
        }
      });

      ctrl.$formatters.push(function(value){
        return $filter("date")(value,"dd/MM/yyyy");
      });
    }
  };
});
