angular.module("mobacs").directive('uiPhoneNumberResidencia', function($filter, $ionicPopup) {
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
        if (phoneNumber.length > 8) {
          phoneNumber = phoneNumber.substring(0, 8) + "-" + phoneNumber.substring(8, 12);
        }
        if (phoneNumber.length == 13) {
          console.log(tercDigito);
          console.log(phoneNumber);

          if (tercDigito == 8 || tercDigito == 9) {
            phoneNumber = null;
            $ionicPopup.alert({
              title: 'Número Inválido',
              template: 'Por favor, digite o DDD + Número de Telefone Fixo válido com 8 dígitos!'
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
