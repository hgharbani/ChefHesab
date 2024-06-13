// <![CDATA[
(function ($) {
    $.fn.PostMvcFormAjax = function (options) {
        var defaults = {
            baseEvent: null,
            postUrl: '/',
            loginUrl: '/login',
            beforePostHandler: null,
            completeHandler: null,
            errorHandler: null
        };
        options = $.extend(defaults, options);

        var validateForm = function (form) {
            //زمانیکه از کندو استفاده میکنیم.
            if ($(form).data('validator'))
                $(form).data('validator').settings.ignore = '';
            //فعال سازي دستي اعتبار سنجي جي‌كوئري
            var val = form.validate();
            val.form();
            return val.valid();
        };

        return this.each(function () {
            if (options.baseEvent)
                options.baseEvent.preventDefault();
            var form = $(this);
            //اگر فرم اعتبار سنجي نشده، اطلاعات آن ارسال نشود
            if (!validateForm(form)) return;

            //در اينجا مي‌توان مثلا دكمه‌اي را غيرفعال كرد
            if (options.beforePostHandler)
                options.beforePostHandler(this);
            //showLoading();
            //اطلاعات نبايد كش شوند
            $.ajaxSetup({ cache: false });
            $.ajax({
                type: "POST",
                url: options.postUrl,
                data: form.serialize(), //تمام فيلدهاي فرم منجمله آنتي فرجري توكن آن‌را ارسال مي‌كند
                complete: function (xhr, status) {
                    //hideLoading();
                   
                    var data = xhr.responseText;
                    if (xhr.status == 403) {
                        window.location = options.loginUrl; //در حالت لاگين نبودن شخص اجرا مي‌شود
                    }
                    else if (status === 'error' || !data) {
                        if (options.errorHandler)
                            options.errorHandler(this);
                    }
                    else {
                        if (options.completeHandler) {
                            data = $.parseJSON(data);
                            ShowNotifications(data, form, options);
                        }
                        options.completeHandler(data);
                    }
                },
                error: function (a) {
                    //hideLoading();
                    alert("خطایی رخ داده است");
                    options.errorHandler();
                }
            });
        });
    };
})(jQuery);
// ]]>
// <![CDATA[
(function ($) {
    $.PostMvcDataAjax = function (options) {
        var defaults = {
            data: null,
            baseEvent: null,
            postUrl: '/',
            loginUrl: '/login',
            beforePostHandler: null,
            completeHandler: null,
            errorHandler: null
        };
        options = $.extend(defaults, options);
        if (options.baseEvent)
            options.baseEvent.preventDefault();
        //در اينجا مي‌توان مثلا دكمه‌اي را غيرفعال كرد
        if (options.beforePostHandler)
            options.beforePostHandler(this);
        //showLoading();
        //اطلاعات نبايد كش شوند
        $.ajaxSetup({ cache: false });
        $.ajax({
            type: "POST",
            url: options.postUrl,
            data: options.data, //تمام فيلدهاي فرم منجمله آنتي فرجري توكن آن‌را ارسال مي‌كند
            complete: function (xhr, status) {
                //hideLoading();
                var data = xhr.responseText;
                if (xhr.status == 403) {
                    window.location = options.loginUrl; //در حالت لاگين نبودن شخص اجرا مي‌شود
                }
                else if (status === 'error' || !data) {
                    if (options.errorHandler)
                        options.errorHandler(this);
                }
                else {
                    if (options.completeHandler) {
                        data = $.parseJSON(data);
                        ShowNotifications(data, null, options);
                    }
                    options.completeHandler(data);
                }
            },
            error: function (a) {
                //hideLoading();
                alert("خطایی رخ داده است");
                options.errorHandler();
            }
        });
    };
})(jQuery);
// ]]>