@ECHO OFF

  ECHO RELEASE 
  ionic cordova build --release android && "%JAVA_HOME%\bin\jarsigner" -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore "mobacs.keystore" -storepass "softpark@2017" "platforms\android\build\outputs\apk\android-release-unsigned.apk" MobACS && del "MobACS.apk" && "%ANDROID_HOME%\build-tools\23.0.3\zipalign" -v 4 "platforms\android\build\outputs\apk\android-release-unsigned.apk" MobACS.apk