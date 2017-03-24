(function (window, $) {
    var esus = function () {
        var _self = this;

        var containers = {
            page: function () { },
            pre: [],
            post: [],
            modal: {},
            others: {}
        };

        this.register = function (container, code, name) {
            if (container === 'page') {
                containers.page = code;
            } else if (container === 'modal') {
                containers.modal[name] = code;
            } else if (container === 'pre') {
                containers.pre.push(code);
            } else if (container === 'post') {
                containers.post.push(code);
            } else {
                containers.others[container] = code;
            }
        };

        this.modal = function (name) {
            return containers.modal[name];
        };

        this.component = function (name) {
            return containers.others[name];
        }

        this.init = function () {
            containers.pre.forEach(function (f) {
                if ($.isFunction(f)) {
                    f.call(_self);
                }
            });

            if ($.isFunction(containers.page)) {
                containers.page.call(_self);
            }
        };

        $(window).load(function () {
            containers.post.forEach(function (f) {
                if ($.isFunction(f)) {
                    f.call(_self);
                }
            });
        });
    };

    window.ESUS = new esus();

    $(window.document).ready(function () {
        window.ESUS.init();
    });
})(window, $);