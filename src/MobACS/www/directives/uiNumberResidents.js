angular.module("mobacs").directive('uiNumberResidents',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatNumberResidents = function(NumberResidents){
        NumberResidents = NumberResidents.replace(/[^0-9]+/g,"");
        if(NumberResidents.length > 3){
          NumberResidents = NumberResidents.substring(0,4);
        }
        return NumberResidents;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatNumberResidents(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
