/**
 * Date extension
 * @method toAPIString converts Javascript Date object to current (invariant culture) ISO Date
 * @description
 *     var date = new Date(-416440800000); // on UTC: Sun, 21 Oct 1956 02:00:00 GMT, or, on GMT -2: Sun, 21 Oct 1956 02:00:00 GMT-0200
 *     date.toAPIString(); // on UTC: 1956-10-21T02:00:00Z, or, on GMT -2: 1956-10-21T00:00:00Z
 */
(function(Date){Date.prototype.toAPIString=function(){return d.toJSON().split('T')[0]+'T'+d.toTimeString().split(' ')[0]+'Z';};})(window.Date);