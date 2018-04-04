angular.module("mobacs").directive('uiComplementoEnd', function() {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _maxCarac = function(ComplementoEnd) {
        ComplementoEnd = ComplementoEnd.replace(/[^0-9]+/g, "");
        if (ComplementoEnd.length > 30) {
          // ComplementoEnd = ComplementoEnd.substring(0,5) + '-' +ComplementoEnd.substring(5,8);
          ComplementoEnd = ComplementoEnd.substring(0, 30);
        }
        return ComplementoEnd;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_maxCarac(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
