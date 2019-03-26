function parseWithRefs(obj) {
    json = JSON.stringify(obj);
    var refMap = {};
    jsonObj = JSON.parse(json, function (k, v) {
        if (k === '$id') {
            refMap[v] = this;
            return void (0);
        }
        if (v && v.$ref) { return refMap[v.$ref]; }
        return v;
    });
    return jsonObj;
}