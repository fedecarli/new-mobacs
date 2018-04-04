angular.module( 'mobacs')
.factory('SecurityService', function() {
    var key = CryptoJS.enc.Utf8.parse('7CCB5E624FD29283');
    var iv = CryptoJS.enc.Utf8.parse('7061737323313233');
    
    function encrypt(text) {
        return CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(text), key, {
            keySize: 128 / 8, 
            iv: iv, 
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7 
        });
    }
    
    function decrypt(encrypted) {
        return CryptoJS.AES.decrypt(encrypted, key, { 
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
    }
    
    return {
        encrypt: encrypt,
        decrypt: decrypt
    };
});