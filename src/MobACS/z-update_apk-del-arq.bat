DIR /b /a "%ANDROID_HOME%\build-tools" >> test.txt
FOR /F %f IN (aversions.txt) DO SET ANDROID_VERSION=%f
ERASE aversions.txt

FOR /L %%A IN (1,1,200) DO (
  pause
  ECHO %%A
  adb -d shell "pm uninstall -k br.com.softpark.mobacs" && ionic cordova build --release android && jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore "mobacs.keystore" -storepass "softpark@2017" "platforms\android\build\outputs\apk\android-release-unsigned.apk" MobACS && del "MobACS.apk" && "%ANDROID_HOME%\build-tools\%ANDROID_VERSION%\zipalign" -v 4 "platforms\android\build\outputs\apk\android-release-unsigned.apk" MobACS.apk && adb install MobACS.apk && adb shell am start -n br.com.softpark.mobacs/.MainActivity)