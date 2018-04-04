angular.module("mobacs").directive('uiCnsCitizen', function($ionicPopup) {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _formatCnsCitizen = function(CnsCitizen) {
        CnsCitizen = CnsCitizen.replace(/[^0-9]+/g, "");
        if (CnsCitizen.length > 14) {
          CnsCitizen = CnsCitizen.substring(0, 15);
        }

        var initCns = CnsCitizen.substring(0, 1);

        if (CnsCitizen.length == 15) {

          if (initCns == 1 || initCns == 2) {
            var soma = 0,
              resto = 0,
              dv = 0,
              pis = '',
              resultado = '';

            pis = CnsCitizen.substring(0, 11);

            soma = ((parseInt(pis.substring(0, 1)) * 15) +
              (parseInt(pis.substring(1, 2)) * 14) +
              (parseInt(pis.substring(2, 3)) * 13) +
              (parseInt(pis.substring(3, 4)) * 12) +
              (parseInt(pis.substring(4, 5)) * 11) +
              (parseInt(pis.substring(5, 6)) * 10) +
              (parseInt(pis.substring(6, 7)) * 9) +
              (parseInt(pis.substring(7, 8)) * 8) +
              (parseInt(pis.substring(8, 9)) * 7) +
              (parseInt(pis.substring(9, 10)) * 6) +
              (parseInt(pis.substring(10, 11)) * 5));

            resto = soma % 11;
            dv = 11 - resto;

            if (dv == 11) {
              dv = 0;
            }

            if (dv == 10) {
              soma = ((parseInt(pis.substring(0, 1)) * 15) +
                (parseInt(pis.substring(1, 2)) * 14) +
                (parseInt(pis.substring(2, 3)) * 13) +
                (parseInt(pis.substring(3, 4)) * 12) +
                (parseInt(pis.substring(4, 5)) * 11) +
                (parseInt(pis.substring(5, 6)) * 10) +
                (parseInt(pis.substring(6, 7)) * 9) +
                (parseInt(pis.substring(7, 8)) * 8) +
                (parseInt(pis.substring(8, 9)) * 7) +
                (parseInt(pis.substring(9, 10)) * 6) +
                (parseInt(pis.substring(10, 11)) * 5) + 2);

              resto = soma % 11;
              dv = 11 - resto;
              resultado = pis + "001" + parseInt(dv).toString();
            } else {
              resultado = pis + "000" + parseInt(dv).toString();
            }

            if (!(CnsCitizen == resultado)) {
              CnsCitizen = null;
              $ionicPopup.alert({
                title: 'CNS Inválido',
                template: 'Por favor, digite um CNS válido!'
              })
            }

          } else if (initCns == 7 || initCns == 8 || initCns == 9) {

            var resto = 0,
              soma = 0;

            soma = ((parseInt(CnsCitizen.substring(0, 1)) * 15) +
              (parseInt(CnsCitizen.substring(1, 2)) * 14) +
              (parseInt(CnsCitizen.substring(2, 3)) * 13) +
              (parseInt(CnsCitizen.substring(3, 4)) * 12) +
              (parseInt(CnsCitizen.substring(4, 5)) * 11) +
              (parseInt(CnsCitizen.substring(5, 6)) * 10) +
              (parseInt(CnsCitizen.substring(6, 7)) * 9) +
              (parseInt(CnsCitizen.substring(7, 8)) * 8) +
              (parseInt(CnsCitizen.substring(8, 9)) * 7) +
              (parseInt(CnsCitizen.substring(9, 10)) * 6) +
              (parseInt(CnsCitizen.substring(10, 11)) * 5) +
              (parseInt(CnsCitizen.substring(11, 12)) * 4) +
              (parseInt(CnsCitizen.substring(12, 13)) * 3) +
              (parseInt(CnsCitizen.substring(13, 14)) * 2) +
              (parseInt(CnsCitizen.substring(14, 15)) * 1));

            resto = soma % 11;

            if (resto != 0) {
              CnsCitizen = null;
              $ionicPopup.alert({
                title: 'CNS Inválido',
                template: 'Por favor, digite um CNS válido!'
              })
            }
          } else if (initCns == 3 || initCns == 4 || initCns == 5 || initCns == 6) {
            CnsCitizen = null;
            $ionicPopup.alert({
              title: 'CNS Inválido',
              template: 'Números CNS válidos iniciam com 1,2,7,8 ou 9!'
            })
          } //Segunda condição
        } //CNS == 15
        return CnsCitizen;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_formatCnsCitizen(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
