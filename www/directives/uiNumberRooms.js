angular.module("mobacs").directive('uiNumberRooms',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatNumberRooms = function(NumberRooms){
        NumberRooms = NumberRooms.replace(/[^0-9]+/g,"");
        if(NumberRooms.length > 1){
          NumberRooms = NumberRooms.substring(0,2);
        }
        return NumberRooms;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatNumberRooms(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
