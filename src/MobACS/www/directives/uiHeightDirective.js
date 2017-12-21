angular.module("mobacs").directive('uiHeight',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatHeight = function(Height){
        Height = Height.replace(/[^0-9]+/g,"");
        if((Height.length == 2)){
          Height = Height.substring(0,1) + "," + Height.substring(1,2);
        } else
        if(Height.length == 3){
          Height = Height.substring(0,2) + "," + Height.substring(2,3);
        } else
        if(Height.length == 4){
          Height = Height.substring(0,3) + "," + Height.substring(3,4);
        } else
        if(Height.length > 4){
          Height = Height.substring(0,4) + "," + Height.substring(4,5);
        }
        return Height;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatHeight(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
