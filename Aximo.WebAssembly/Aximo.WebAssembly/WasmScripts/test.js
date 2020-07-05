'use strict';

define(() => {
    document.body.innerHTML = "<div id='results' />";
});

var InteropHandles = {};
var CurrentHandle = 0;
var __InteropCallback = Module.mono_bind_static_method("[Aximo.WebAssembly] WebAssembly.Runtime:Callback");

var JsToken = class JsToken {
    constructor(type, value) {
        this.type = type;
        this._value = value;
    }

    get isRef() {
        return !(this.type === "unknown" || this.type === "string" || this.type === "number" || this.type === "boolean");
    }

    get value() {
        if (this.isRef)
            return InteropHandles[this._value];

        return this._value;
    }

    toString() {
        return this.type + "|" + this._value;
    }

};

function getHandleType(obj) {
    if (Array.isArray(obj))
        return "array";

    if (obj instanceof Element)
        return "element";

    return typeof (obj);
}

function RegisterHandle(obj) {
    if (!obj)
        return null;

    var handle = ++CurrentHandle;
    InteropHandles[handle] = obj;
    return handle;
}

function RegisterHandleWithType(obj) {
    if (!obj)
        return null;

    var type = getHandleType(obj);
    var handle = null;
    var t = typeof (obj);
    if (t == "string" || t == "number" || t == "boolean")
        handle = obj;
    else
        handle = RegisterHandle(obj);

    var handleStr = type + "|" + handle;
    if (type == "array")
        handleStr += "|" + obj.length;
    return handleStr;
}

var Interop = {

    appendResult: function (str) {
        var txt = document.createTextNode(str);
        var parent = document.getElementById('results');
        parent.appendChild(txt, parent.lastChild);
    },

    getTest: function (str) {
        return document.getElementById('results');
    },

    getNum: function (str) {
        return 55;
    },

    freeHandle: function (str) {
        //console.log(str);
        delete InteropHandles[str];
    },

    getElementById: function (str) {
        return RegisterHandleWithType(document.getElementById(str));
    },

    getObject: function (str) {
        var func = new Function(str);
        return RegisterHandleWithType(func());
    },

    getArrayElement: function (elHandle, index) {
        var ar = InteropHandles[elHandle];
        if (!ar)
            return null;

        var itm = ar[index];
        return RegisterHandleWithType(itm);
    },

    setArrayElement: function (elHandle, index, value) {
        var ar = InteropHandles[elHandle];
        if (!ar)
            return null;

        ar[index] = value.value;
    },

    setProperty: function (elHandle, name, value) {
        var ar = elHandle.value;
        if (!ar)
            return null;

        ar[name] = value.value;
    },

    getProperty: function (elHandle, name) {
        var ar = elHandle.value;
        if (!ar)
            return null;

        var itm = ar[name];
        return RegisterHandleWithType(itm);
    },

    callMethod: function (elHandle, name, args) {
        var ar = elHandle.value;
        if (!ar)
            return null;

        var values = args.map(function (e) { return e == null ? null : e.value });
        var func = ar[name];
        var itm = func.apply(ar, values)

        return RegisterHandleWithType(itm);
    },

    createDelegate: function (wasmHandle) {
        var func = function () {
            const args = Array.from(arguments);
            console.log(args);
            var argsHandle = RegisterHandle(args);
            __InteropCallback(wasmHandle._value, argsHandle, args.length);
        };
        func.__WASM_HANDLE__ = wasmHandle._value;
        return RegisterHandleWithType(func);
    },

};

