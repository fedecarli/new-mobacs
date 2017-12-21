angular.module("mobacs").directive('uiMicroarea',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatMicroarea = function(microArea){
        microArea = microArea.replace(/[^0-9]+/g,"");
        if(microArea.length > 1){
          microArea = microArea.substring(0,2);
        }
        return microArea;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatMicroarea(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
