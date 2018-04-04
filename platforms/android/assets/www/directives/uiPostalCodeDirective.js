angular.module("mobacs").directive('uiPostalCode',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatPostalCode = function(PostalCode){
        PostalCode = PostalCode.replace(/[^0-9]+/g,"");
        if(PostalCode.length > 5){
          PostalCode = PostalCode.substring(0,5) + '-' +PostalCode.substring(5,8);
        }
        return PostalCode;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatPostalCode(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
