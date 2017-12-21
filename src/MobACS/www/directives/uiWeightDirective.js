angular.module("mobacs").directive('uiWeight',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatWeight = function(Weight){
        Weight = Weight.replace(/[^0-9]+/g,"");
        console.log(Weight);
        if(Weight.length == 4){
          Weight = Weight.substring(0,1) + "," + Weight.substring(1,4);
        } else
        if(Weight.length == 5){
          Weight = Weight.substring(0,2) + "," + Weight.substring(2,5);
        }else
        if(Weight.length > 5){
          Weight = Weight.substring(0,3) + "," + Weight.substring(3,6);
        }
        return Weight;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatWeight(ctrl.$viewValue));
        ctrl.$render();
      });

      // ctrl.$parsers.push(function(value){
      //   if(value.length === 10){
      //     var dateArray = value.split("/");
      //     return new Date(dateArray[2],dateArray[1]-1,dateArray[0]).getTime();
      //   }
      // });
      //
      // ctrl.$formatters.push(function(value){
      //   return $filter("date")(value,"dd/MM/yyyy");
      // });
    }
  };
});
