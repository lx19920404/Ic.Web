﻿//以下导入layui依赖的模块，加入到layout.cshtml中。
layui.use('element', function () {
    var element = layui.element;

    //…
});
layui.use('form', function () {
    var form = layui.form;

    //监听提交
    form.on('submit(formDemo)', function (data) {
        layer.msg(JSON.stringify(data.field));
        return false;
    });
});