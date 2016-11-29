(function ($) {
    //全局系统对象
    window['Dot'] = {};

    Dot.GetHandlerURL = function (options) {
        var p = options || {};

        var ajaxUrl = "/Ajax/ProcessRequest?";

        var url = ajaxUrl + $.param({
            type: p.type,
            method: p.method
        });

        return url;
    }

    Dot.ajax = function (options) {
        var p = options || {};
        var url = Admin.GetHandlerURL(options);

        var isAsync = true;
        if (typeof (p.async) != "undefined") {
            isAsync = p.async;
        }

        $.ajax({
            cache: false,
            async: isAsync,
            url: url,
            data: p.data,
            dataType: 'json',
            type: 'post',
            beforeSend: function () { },
            complete: function (g) {
                var type = g;
            },
            success: function (result) {
                if (!result) return;
                if (!result.IsError) {
                    if (p.success) {
                        p.success(result.data, result.Message);
                    }
                }
                if (p.error)
                    p.error(result.Message);
            },
            error: function (result) {
                alert('发现系统错误 <BR>错误码：' + result.status);
            }
        });
    }

    Dot.getPageMenuNo = function () {

    }

    //增加搜索按钮,并创建事件
    Dot.appendSearchButtons = function (form, table) {

    }

    //加载toolbar
    Dot.loadToolbar = function (grid, toolbarBtnItemClick) {
        var MenuNo = Dot.getPageMenuNo();
    }
})(jQuery);