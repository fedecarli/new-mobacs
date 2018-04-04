angular.module("mobacs").directive('uiPhoneNumberCelular', function($filter, $ionicPopup) {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _formatphoneNumber = function(phoneNumber) {
        phoneNumber = phoneNumber.replace(/[^0-9]+/g, "");
        if (phoneNumber.length > 2) {
          phoneNumber = '(' + phoneNumber;
        }
        if (phoneNumber.length > 3) {
          phoneNumber = phoneNumber.substring(0, 3) + ")" + phoneNumber.substring(3);
          var tercDigito = phoneNumber.substring(4, 5);
        }
        if (phoneNumber.length > 9) {
          phoneNumber = phoneNumber.substring(0, 9) + "-" + phoneNumber.substring(9, 13);
        }
        if (phoneNumber.length == 14) {
          console.log(tercDigito);
          console.log(phoneNumber);

          if (tercDigito != 9) {
            phoneNumber = null;
            $ionicPopup.alert({
              title: 'Número Inválido',
              template: 'Por favor, digite o DDD + Número de Celular válido com 9 dígitos!'
            })
          }

        }
        return phoneNumber;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_formatphoneNumber(ctrl.$viewValue));
        ctrl.$render();
      });

      // ctrl.$parsers.push(function(value){
      //   if(value.length === 10){
      //     var phoneNumberArray = value.split("/");
      //     return new phoneNumber(phoneNumberArray[2],phoneNumberArray[1]-1,phoneNumberArray[0]).getTime();
      //   }
      // });
      //
      // ctrl.$formatters.push(function(value){
      //   return $filter("phoneNumber")(value,"dd/MM/yyyy");
      // });
    }
  };
});
