angular.module("mobacs").directive('uiAnimalsNumber',function($ionicPopup){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatAnimalsNumber = function(AnimalsNumber){
        AnimalsNumber = AnimalsNumber.replace(/[^0-9]+/g,"");

        if(AnimalsNumber.length > 1){
          AnimalsNumber = AnimalsNumber.substring(0,2);
        }

        if(AnimalsNumber == 0){
          AnimalsNumber = '';
          $ionicPopup.alert({
            title: 'Valor inválido',
            template: 'Existem animais, não pode ser inserido o valor 0!'
          })
        }

        return AnimalsNumber;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatAnimalsNumber(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
