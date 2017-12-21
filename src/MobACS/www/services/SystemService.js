angular.module('mobacs').service('SystemService',function(){
  return{
    isTenYears : function(data){
      var systemDate = new Date().getTime();
      var tenYears = systemDate - 315360000000;

      data = parseInt(data);

      if(data > tenYears){
        return false;
      }else{
        return true;
      }
    },
    isNineSixty : function(data){
      var systemDate = new Date().getTime();
      var nineYears = systemDate - 283824000000;

      data = parseInt(data);

      if(data < nineYears){
        return false;
      }else{
        return true;
      }
    },
    isSystemDate : function(data){
      var systemDate = new Date().getTime();

      data = parseInt(data);

      if(data == null){
        data = new Date().getTime() + 10000;
      }else{
        if(data.length === 10){
          var dateArray = data.split("/");
          data =  new Date(dateArray[2],dateArray[1]-1,dateArray[0]).getTime();
        }
      }


      if(data > systemDate){
        return true;
      }else{
        return false;
      }
    }
  }
})
