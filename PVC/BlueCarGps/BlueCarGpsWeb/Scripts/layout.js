var Layout = {};

Layout.init = function () {
    //IE 提示console找不到 解决办法
    window.console = window.console || (function () {
        var c = {}; c.log = c.warn = c.debug = c.info = c.error = c.time = c.dir = c.profile = c.clear = c.exception = c.trace = c.assert = function () { };
        return c;
    })();

    $('.sidebar-menu li').removeClass('active');
    var pathname = window.location.pathname.split('/');

    switch (pathname[1]) {
        case "PVC":
            $('.nav-pvc').addClass('active');
            PageAction('#pvc', '新建PVC记录', '编辑PVC记录', 'PVC记录详情', '创建', '更新', '删除');
            break;
        default:
            $('.nav-pvc').addClass('active');
            break;
    }

    function PageAction(id, newAction, editAction, deleteAction, newBtn, editBtn, deleteBtn) {
        var vueName = new Vue({
            el: id,
            data: {
                action: newAction,
                actionBtn: newBtn
            }
        });

        if (pathname[2] == "Edit") {
            vueName.action = editAction;
            vueName.actionBtn = editBtn;
        } else if (pathname[2] == "Delete") {
            vueName.action = deleteAction;
            vueName.actionBtn = deleteBtn;
        }
    }
}

Layout.show_loading = function () {
    $('#loading').css({ "display": "block" });
}

Layout.hide_loading = function () {
    $('#loading').css({ "display": "none" });
}

Layout.popMsg = function (cls, content) {
    var Html = "<div class='" + cls + "'><div class='popMsg-body'> " + content + "</div></div>";

    $(Html).appendTo($('body'));

    window.setTimeout(function () {
        $("." + cls).slideUp();
        window.setTimeout(function () {
            $('.' + cls).remove();
        }, 410);
    }, 4000);
    
}

Layout.GetUrlString = function(name){
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

Layout.datepicker = function (date_picker) {
    $(date_picker).datetimepicker({
        format: 'Y-m-d',
        scrollInput: false,
        timepicker: false
    });
}

Layout.timepicker = function (time_picker) {
    $(time_picker).datetimepicker({
        format: 'H:i',
        defaultTime: '00:00',
        datepicker: false
    });
}

Layout.datetimepicker = function (date_time_picker) {
    $(date_time_picker).datetimepicker({
        scrollInput: false
    });
}

Layout.rangedatepicker = function (date_picker_start, date_picker_end) {
    $(date_picker_start).datetimepicker({
        format: 'Y-m-d',
        onShow: function (ct) {
            this.setOptions({
                maxDate: $(date_picker_end).val() ? $(date_picker_end).val() : false
            })
        },
        scrollInput: false,
        timepicker: false
    });

    $(date_picker_end).datetimepicker({
        format: 'Y-m-d ',
        onShow: function (ct) {
            this.setOptions({
                minDate: $(date_picker_start).val() ? $(date_picker_start).val() : false
            })
        },
        scrollInput: false,
        timepicker: false
    });
}

Layout.rangedatetimepicker = function (date_picker_start, date_picker_end) {
    $(date_picker_start).datetimepicker({
        onShow: function (ct) {
            this.setOptions({
                maxDate: $(date_picker_end).val() ? $(date_picker_end).val() : false
            })
        },
        scrollInput: false,
    });

    $(date_picker_end).datetimepicker({
        onShow: function (ct) {
            this.setOptions({
                minDate: $(date_picker_start).val() ? $(date_picker_start).val() : false
            })
        },
        scrollInput: false,
    });
}

//可以扩展， 使用typeof 进行判断然后进行判断
Layout.IsStringNull = function (str) {
    if (str == null || str == "") {
        return true;
    } else {
        return false;
    }
}

Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function FutureDate(day) {
    var now = new Date();
    var FutureDate = new Date(now.getTime() - day * 24 * 60 * 60 * 1000).Format("yyyy-MM-dd hh:mm");
    return FutureDate;
}

var console = console || {
    log: function () {
        return;
    }
};//兼容IE,当IE不支持console.log时，自定义一个包含log方法的对象给他

$.ajaxSetup({
    statusCode: {
        401: function () {
            Layout.popMsg("popMsg-danger", "登陆过期，请重新登陆");
            if (window.location.pathname.split('/')[1] == "Error") {
            } else {
                location.href = '/Error/error401';
            }
        },
        404: function () {
            Layout.popMsg("popMsg-danger", "请求API失败");
            if (window.location.pathname.split('/')[1] == "Error") {
            } else {
                //location.href = '/Error/error404';
            }
        },
        500: function () {
            Layout.popMsg("popMsg-danger", "服务器错误");
            if (window.location.pathname.split('/')[1] == "Error") {
            } else {
                location.href = '/Error/error500';
            }
        }
    }
});


$(document).ready(function () {
    if ($("#msg").val() != "" && $("#msg").val() != undefined) {
        Layout.popMsg("popMsg-danger", $("#msg").val());
    }
 
    $('select').not('#carTypeId').not('#IsOpen').not('#InOutMode').not('#AlarmMode').comboSelect();

    var pathname = window.location.pathname.split('/');

    if (pathname[2] == "Delete") {
        $('input').attr('disabled', 'disabled');
        $('select').attr('disabled', 'disabled');
       $('.combo-input').attr('disabled', 'disabled');
    }
});