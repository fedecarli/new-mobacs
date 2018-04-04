angular.module("mobacs").directive('uiName',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatName = function(Name){
        Name = Name.replace(/[^a-zA-ZéúíóáÉÚÍÓÁèùìòàçÇÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄ\-\ \s]+$/,"");
        Name = Name.toUpperCase();
        if(Name.length > 69){
          Name = Name.substring(0,70);
        }
        return Name;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatName(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
