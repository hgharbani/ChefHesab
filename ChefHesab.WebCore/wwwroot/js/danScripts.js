﻿
/// <reference path="jquery-1.10.2.min.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
// <![CDATA[
function defrm() {
    document.write = '';
    window.top.location = window.self.location;
    setTimeout(function () {
        document.body.innerHTML = '';
    }, 0);
    window.self.onload = function (evt) {
        document.body.innerHTML = '';
    };
}
if (window.top !== window.self) {
    try {
        if (window.top.location.host)
        { /* will throw */ }
        else {
            defrm(); /* chrome */
        }
    } catch (ex) {
        defrm(); /* everyone else */
    }
}

function cancelEvent(element, e) {
    if (window.event)
        event.cancel = true;
    else
        e.preventDefault();
    return false;
}

function addToken(data) {
    data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
    return data;
}

function validateThisForm(formId) {
    if ($("#" + formId).data('validator'))
        $("#" + formId).data('validator').settings.ignore = '';
    var val = $("#" + formId).validate();
    val.form();
    return val.valid();
}
function customSubmit(formId, options) {
    if (!validateThisForm(formId)) return;
    var defaults = {
        disableButtons: null
    };
    options = $.extend(defaults, options);
    $.each(options.disableButtons, function (i, val) {
        $(val).attr("disabled", "disabled").prop('disabled', true).prop("onclick", null).attr("onclick", null);
    });
    showLoading();
    $("#" + formId).submit();
}
function disableButtons(buttons) {
    $.each(buttons, function (i, val) {
        $(val).attr("disabled", "disabled").prop('disabled', true).prop("onclick", null).attr("onclick", null);
    });
}
function enableButtons(buttons) {
    $.each(buttons, function (i, val) {
        $(val).prop('disabled', false).removeAttr("disabled").removeProp('disabled').removeProp("onclick").removeAttr("onclick");
    });
}
function showLoading() {
    $("#showLoading").show();
}
function hideLoading() {
    $("#showLoading").hide();
}
function customDelete(e, formId, options) {
    var defaults = {
        disableButtons: null,
        content: 'آیا مایل به حذف رکورد میباشید?'
    };
    options = $.extend(defaults, options);
    e.preventDefault();
    if (!confirm(options.content))
        return;
    customSubmit(formId, options);
}
//custom validators
jQuery(document).ready(function () {
    jQuery.validator.unobtrusive.adapters.add('requiredwithinitialvalue',
                                            ['initialvalue'],
                                            function (options) {
                                                // simply pass the options.params here       
                                                options.rules['requiredwithinitialvalue'] = options.params;
                                                options.messages['requiredwithinitialvalue'] = options.message;
                                            });
    jQuery.validator.addMethod('requiredwithinitialvalue',
                                              function (value, element, params) {
                                                  // params here will equal { param1: 'value1', param2: 'value2' } 
                                                  var initial = params["initialvalue"];
                                                  if (value == "") {
                                                      return false;
                                                  }
                                                  if (value == null) {
                                                      return false;
                                                  }
                                                  if (value == initial) {
                                                      return false;
                                                  }
                                                  else {
                                                      return true;
                                                  }
                                              });
});
///
//color switcher script
//$(function () {
//    $("#custom").spectrum({
//        showPalette: true,
//        localStorageKey: "spectrum.homepage",
//        showInitial: true,
//        palette: [
//        ],
//        move: function (color) {
//            var selectedColor = color.toHexString(); // #ff0000
//            $('.sidebar').css('background-color', selectedColor);

//        }
//    });
//});
function display_kendoui_grid_error(e) {
    if (e.errors) {
        if ((typeof e.errors) == 'string') {
            //single error
            //display the message
            alert(e.errors);
        } else {
            //array of errors
            //source: http://docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/faq#how-do-i-display-model-state-errors?
            //var message = "با عرض پوزش ،خطای زیر رخ داده است:";
            ////create a message containing all errors.
            //$.each(e.errors, function (key, value) {
            //    if (value.errors) {
            //        message += "\n";
            //        message += value.errors.join("\n");
            //    }
            //});
            ////display the message
            //alert(message);
            ShowNotifications(e.errors);
        }
    } else {
        alert('با عرض پوزش ،خطایی رخ داده است.لطفا دوباره تلاش نمایید.');
    }
}

function GetView(options) {
    var defaults = {
        getUrl: '/',
        loginUrl: '/login',
        errorHandler: null,
        UpdateTargetId: ''
    };
    options = $.extend(defaults, options);
    if (this.event)
        this.event.preventDefault();
    //در اينجا مي‌توان مثلا دكمه‌اي را غيرفعال كرد
    if (options.beforePostHandler)
        options.beforePostHandler(this);
    showLoading();
    //اطلاعات نبايد كش شوند
    $.ajaxSetup({ cache: false });
    $.ajax({
        type: "GET",
        url: options.getUrl,
        data: null,
        complete: function (xhr, status) {
            var data = xhr.responseText;
            if (xhr.status == 403) {
                window.location = options.loginUrl; //در حالت لاگين نبودن شخص اجرا مي‌شود
            }
            else if (status === 'error' || !data) {
                if (options.errorHandler)
                    options.errorHandler(this);
            }
            else {
                $("#" + options.UpdateTargetId).html(data);
                $('form').removeData('validator');
                $('form').removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse('form');
            }
            hideLoading();
        },
        error: function () {
            alert("خطایی رخ داده است");
            if (options.errorHandler)
                options.errorHandler(this);
            hideLoading();
        }
    });
};
// ]]>

function ShowNotifications(data, form, options) {
    //data = $.parseJSON(data);
    var notification = $("#notification").data("kendoNotification");
    if (data.DanActionStaus == 0) {
        $.each(data.Notifications, function (i, ival) {
            if (ival.NotifyType == 0) {//Success
                notification.show({ title: ival.Title, message: ival.Message }, "success");
            }
            else if (ival.NotifyType == 1) {//Warning
                notification.show({ title: ival.Title, message: ival.Message }, "warning");
            }
            else if (ival.NotifyType == 2) {//Info
                notification.show({ title: ival.Title, message: ival.Message }, "info");
            }
            else if (ival.NotifyType == 3) {//Error
                notification.show({ title: ival.Title, message: ival.Message }, "error");
            }
        });
    } else if (data.DanActionStaus == 1) {
        if (form) {
            var validator = form.data("validator");
            var errors = new Array();
            $.each(data.ValidationErrors, function (i, ival) {
                var errKey = ival.Key;
                $.each(ival.Errors, function (j, jval) {
                    if (errors[errKey]) {
                        errors[errKey] = errors[errKey] + "-" + jval;
                    } else {
                        errors[errKey] = jval;
                    }
                });
            });
            validator.showErrors(errors);
        } else {
            var errStr = "";
            $.each(data.ValidationErrors, function (i, ival) {
                var errKey = ival.Key;
                $.each(ival.Errors, function (j, jval) {
                    if (errStr == "") {
                        errStr = jval;
                    } else {
                        errStr = errStr + "\n" + jval;
                    }
                });
            });
            if (notification) {
                notification.show({ title: "خطای سیستم", message: errStr.replace("\n", "<br />") }, "error");
            } else {
                alert(errStr);
            }
        }
    }
    else if (data.DanActionStaus == 2) {
        if (confirm(data.ConfirmOptions.ConfirmMessage)) {
            var postData;
            if (form) {
                postData = form.serialize();
            } else {
                postData = options.data;
            }
            $.PostMvcDataAjax({
                baseEvent: options.event,
                data: postData,
                postUrl: data.ConfirmOptions.ConfirmUrl,
                loginUrl: options.loginUrl,
                beforePostHandler: options.beforePostHandler,
                completeHandler: options.completeHandler,
                errorHandler: options.errorHandler
            });
        }
        return;
    }
    if (data.HasRedirect) {
        setTimeout("window.location.assign('" + data.RedirectOptions.RedirectUrl + "')", data.RedirectOptions.RedirectTimeout);
    }
    if (data.CloseWindows) {
        setTimeout("parent.CloseWindow()", 3000);
    }
}

//normalize width & height
function NormalizeWidth(width) {
    if (width == null) {
        width = $(window).width() - 50;
    }
    return width;
}

function NormalizeHeight(height) {
    if (height == null) {
        height = $(window).height() - 100;
    }
    var docHeight = $(window).height();
    if (docHeight < 650) {
        height = docHeight - 50;
    }
    return height;
}
//end normalize width & height


// Public Functions

function isNum(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;
    if (keycode > 31 && (keycode < 48 || keycode > 57))
        return false;
    return true;
}

(function ($) {
    var inlineObjectDataSource;
    var methods = {
        toInlineObjectDataSource: function (treeDatasource) {
            for (i in treeDatasource) {
                inlineObjectDataSource.push(treeDatasource[i]);
            }
            for (i in treeDatasource) {
                if (treeDatasource[i].items != null)
                    methods.toInlineObjectDataSource(treeDatasource[i].items);
            }
        }
    };
    $.hierarchyTreeDataSourceToInlineObject = function (data) {
        inlineObjectDataSource = new Array();
        methods.toInlineObjectDataSource(data);
        return inlineObjectDataSource;
    };
    /**********Total Menu**********/
    $(function () {
        $(".wsmenu-click").click(function () {
            $(this).children(".fa-angle-up").toggleClass("wsmenu-rotate");
            $(this).siblings('.wsmenu-submenu').slideToggle('slow');
            $(this).siblings('.wsmenu-submenu-sub').slideToggle('slow');
            $(this).siblings('.wsmenu-submenu-sub-sub').slideToggle('slow');
            $(this).siblings('.meguMenu').slideToggle('slow');
        });
    });
})(jQuery);

function CloseAfterSuccess(data) {
    if ($("#closeWindowAfterSuccess").length == 0) {
        return;
    }
    if ($("#closeWindowAfterSuccess").is(":checked")) {
        if (data.DanActionStaus == 0) {
            $.each(data.Notifications, function (i, ival) {
                if (ival.NotifyType == 0) {//Success
                    CloseCurrentWindow();
                    return;
                }
            });
        }
    }
}
function CloseCurrentWindow() {
    if (parent.CloseWindow) {
        parent.CloseWindow();
    }
}
function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function removeCommas(nStr) {
    //if (nStr.length > 3) {
    return nStr.toString().replace(/,/g, "");
    //} else {
    //    return nStr;
    //}
}