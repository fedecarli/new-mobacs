angular.module("mobacs").directive('uiDigito', function() {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _formatDigito = function(Digito) {
        Digito = Digito.replace(/[^a-zA-Z0-9]+/g, "");
        if (Digito.length > 1) {
          Digito = Digito.substring(0, 1);
        }

        return Digito;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_formatDigito(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
