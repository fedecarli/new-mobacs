angular.module("mobacs").directive('uiMedicalRecords', function() {
  return {

    require: "ngModel",
    link: function(scope, element, attrs, ctrl) {

      var _formatMedicalRecords = function(MedicalRecords) {
        MedicalRecords = MedicalRecords.replace(/[^0-9]+/g, "");
        if (MedicalRecords != null) {
          var primeiroDig = MedicalRecords.substring(0, 1);
          if (primeiroDig == 0) {
            MedicalRecords = MedicalRecords.replace(0, "");
          }
          //   MedicalRecords = MedicalRecords.toUpperCase();
          // }
          // MedicalRecords = MedicalRecords.replace(/[^0-9A-Z]+/g,"");
          // if(MedicalRecords.length > 29){
          //   MedicalRecords = MedicalRecords.substring(0,30);
        }
        return MedicalRecords;
      }

      element.bind("keyup", function() {
        ctrl.$setViewValue(_formatMedicalRecords(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
