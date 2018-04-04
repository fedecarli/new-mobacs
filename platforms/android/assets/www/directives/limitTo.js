angular.module("mobacs").directive('limitTo', function() {
  return {
    // restrict: "A",
    // link: function(scope, elem, attrs) {
    //   console.log(attrs);
    //   console.log(attrs.limitTo);
    //   var limit = parseInt(attrs.limitTo);
    //   angular.element(elem).on("keypress", function(e) {
    //     console.log(this.value.length);
    //     if (this.value.length == limit) e.preventDefault();
    //   });
    // }

    restrict: "A",
    require: 'ngModel',
    link: function(scope, element, attrs, ngModel) {
      attrs.$set("ngTrim", "false");
      var limitLength = parseInt(attrs.awLimitLength, 10); // 
      console.log(attrs);
      scope.$watch(attrs.ngModel, function(newValue) {
        if (ngModel.$viewValue.length > limitLength) {
          ngModel.$setViewValue(ngModel.$viewValue.substring(0, limitLength));
          ngModel.$render();
        }
      });
    }

    // require: "ngModel",
    // link: function(scope, element, attrs, ctrl) {

    //   var _maxCarac = function(ComplementoEnd) {
    //     ComplementoEnd = ComplementoEnd.replace(/[^0-9]+/g, "");
    //     if (ComplementoEnd.length > 29) {
    //       ComplementoEnd = ComplementoEnd.substring(0, 30);
    //     }
    //     return ComplementoEnd;
    //   }

    //   element.bind("keyup", function() {
    //     ctrl.$setViewValue(_maxCarac(ctrl.$viewValue));
    //     ctrl.$render();
    //   });
    // }
  };
});
