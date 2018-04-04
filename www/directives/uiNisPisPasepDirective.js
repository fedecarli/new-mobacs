angular.module("mobacs").directive('uiNisPisPasep',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatNisPisPasep = function(NisPisPasep){
        NisPisPasep = NisPisPasep.replace(/[^0-9]+/g,"");
        if(NisPisPasep.length > 10){
          NisPisPasep = NisPisPasep.substring(0,11);
        }
        return NisPisPasep;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatNisPisPasep(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
