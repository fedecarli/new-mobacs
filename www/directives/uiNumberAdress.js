angular.module("mobacs").directive('uiNumberAdress',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatNumberAdress = function(NumberAdress){
        NumberAdress = NumberAdress.replace(/[^0-9]+/g,"");
        if(NumberAdress.length > 9){
          NumberAdress = NumberAdress.substring(0,10);
        }
        return NumberAdress;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatNumberAdress(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
