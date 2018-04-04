angular.module("mobacs").directive('uiEmail',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatEmail = function(Email){
        Email = Email.replace(/[^a-zA-ZáéíóúàâêôãõüçÁÉÍÓÚÀÂÊÔÃÕÜÇ]+/g,"");
        if(Email.length > 99){
          Email = Email.substring(0,100);
        }
        return Email;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatEmail(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
