angular.module("mobacs").directive('uiNumeroDo', function() {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _formatNumeroDo = function(NumeroDo) {
        NumeroDo = NumeroDo.replace(/[^0-9]+/g, "");
        if (NumeroDo.length > 8) {
          NumeroDo = NumeroDo.substring(0, 9);
        }
        return NumeroDo;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_formatNumeroDo(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
