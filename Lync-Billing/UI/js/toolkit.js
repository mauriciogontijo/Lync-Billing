function object_skeleton() {
    this.api = function () { };
    this.api_data = function () { };
}

var obj = Object.create(object_skeleton.prototype);

obj = function () {

}

window.billing_main = b;