angular.module("mobacs").directive('uiPortariaNaturalizacao',function(){
  return{

    require: "ngModel",
    link: function(scope,element,attrs,ctrl){

      var _formatPortariaNaturalizacao = function(PortariaNaturalizacao){
        PortariaNaturalizacao = PortariaNaturalizacao.replace(/[^0-9a-z]+/g,"");
        if(PortariaNaturalizacao.length > 15){
          PortariaNaturalizacao = PortariaNaturalizacao.substring(0,16);
        }
        return PortariaNaturalizacao;
      }

      element.bind("keyup", function(){
        ctrl.$setViewValue(_formatPortariaNaturalizacao(ctrl.$viewValue));
        ctrl.$render();
      });
    }
  };
});
