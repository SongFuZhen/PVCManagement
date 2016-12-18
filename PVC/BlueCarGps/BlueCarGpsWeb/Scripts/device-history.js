var History = {};
var lushu;

History.Init = function () {
    var urlStr = Layout.GetUrlString(location.href);
    $('#device_nr').val(urlStr.device_nr);
    $('#device_id').val(urlStr.id);

    var now = new Date().Format("yyyy-MM-dd hh:mm");
    var WeekAgo = FutureDate(14);
    $('.datetime-picker-start').val(WeekAgo);
    $('.datetime-picker-end').val(now);
}

History.Draw = function () {
    $('.draw-history').click(function () {
        Layout.show_loading();

        //TODO: circle-right circle-left CSS Style 要重置

        $(".circle-left").css("background", "#B6B2B2");
        $(".circle-left").css("transform", "0deg");
        $(".circle-right").css("background", "#B6B2B2");
        $(".circle-right").css("transform", "0deg");

        $("#loading").css("top", "110px");

        $('#play-history').attr('disabled', 'disabled');
        $('#play-history').html("加载中...");

        var dateStart = $('.datetime-picker-start').val();
        var dateEnd = $('.datetime-picker-end').val();

        if (dateStart == null || dateStart == "") {
            Layout.popMsg('popMsg-danger', "请选择开始时间");
            return;
        }

        if (dateEnd == null || dateEnd == "") {
            Layout.popMsg('popMsg-danger', "请选择结束时间");
            return;
        }

        Map.RemoveAllMarker("all");
        $("#plays-history").attr("disabled", "disabled");
        $("#pause-history").attr("disabled", "disabled");
        $("#stop-history").attr("disabled", "disabled");

        $.ajax({
            url: '/api/devices/GetGpsList',
            type: 'get',
            dataType: 'json',
            data: {
                id: $('#device_id').val(),
                startTime: dateStart,
                endTime: dateEnd
            },
            success: function (data) {
                History.Strack(data);
            },
            error: function () {
                Layout.popMsg('popMsg-danger', '请求GPS历史记录API失败');
            }
        })
    });
}

History.mapInit = function () {
    //设置高度
    $('#history_map').css({ height: $(window).height() - 130 + 'px', width: $(window).width() });

    $(window).resize(function () {
        $('#history_map').css({ height: $(window).height() - 130 + 'px', width: $(window).width() });
    });

    Map.mapInit('history_map');
}

History.Strack = function (data) {
    //设备信息
    var Device = data.device;

    if($("#speed").val()==""){
        $("#speed").val("5000");
    }

    var CurrentSpeed = $("#speed").val();

    var Device_Track = data.gps;

    if (Device_Track.length == 0) {
        Layout.popMsg('popMsg-danger', "所查时间段没有数据");
        Layout.hide_loading();
        return false;
    }

    $('#pause-history').removeAttr("disabled");

    var Icon = normalIcon;
    if (Device.fenceState == 200) {
        Icon = alarmIcon;
    }

    var href_link = '<a href="/Home/RealTimeTrace?device_nr=' + Device.nr + '&id=' + Device.id + '">实时跟踪 </a>';

    var content = "<b>" + Device.nr + "</b> <i class='fa fa-minus pull-right' id='info-drop' style='cursor:pointer;'></i> <div class='info-body'> <hr /><b>IMEI号: </b>" +
                Device.imei + "<br /><b>状态: </b>" + Device.workStateDisplay + "<br /><b>电量状态: </b>" +
                Device.batteryStateDisplay + "<br /><b>电量</b> " + Device.batteryPercent + "<br /><b>信号时间: </b>" + Device.signalTime + "<br /><b>定位方式: </b>" +
                Device.singleType + "<br /><b>VIN号: </b>" + Device.vin + "<br /><b>Shop: </b>" + Device.shopName + "<br /><b>围栏状态: </b>" + Device.fenceStateDisplay + "<hr /></div>" + href_link;

    $('#device_info').empty();
    $('#device_info').append(content);

    var points = new Array();
    var infoWindow;
    var convertor = new BMap.Convertor();
    var pointArr = [];

    var num = 0;
    var sum = Device_Track.length;
    var deg = 0;
    var s, p;

    if (sum > 500) {
        Layout.popMsg("popMsg-danger", "数据太多了，第一次转换有点慢，请耐心等待");
    }

    $("#process").css("display", "block");

    $("#totalNum").html(sum);

    makeLine();

    function makeLine() {
        //开始加载进度条
        // 如果 是最后一个，进行绘制折线
        if (num == Device_Track.length) {
            var polyline = new BMap.Polyline(points);
            map.addOverlay(polyline); //绘制折线  

            lushu = new BMapLib.LuShu(map, points, {
                defaultContent: CurrentSpeed + " 米/秒",
                autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                icon: Icon,
                speed: CurrentSpeed,// 50米速度
                enableRotation: true, //是否设置marker随着道路的走向进行旋转
            });

            Layout.hide_loading();

            $("#process").css("display", "none");

            lushu.start();

            map.setViewport(points, 15);  //调整到最佳视野
        } else {
            //如果不是最后一个， 进行判断转换
            //一个个点转换的过程中记录已转换点的个数   
            $("#nowNum").html(num);

            var Percentage = (num / sum).toFixed(2) * 100;

            s = Percentage + "";

            p = s.split(".")[0];

            $("#percentage").html(p + "%");

            deg = deg + 360 / sum;

            if (num <= sum / 2) {
                $(".circle-right").css("-o-transform", "rotate(" + deg + "deg)");
                $(".circle-right").css("-moz-transform", "rotate(" + deg + "deg)");
                $(".circle-right").css("-webkit-transform", "rotate(" + deg + "deg)");
            } else {
                $(".circle-left").css("backgroundColor", "#CF2526");
                $(".circle-left").css("-o-transform", "rotate(" + (deg) + "deg)");
                $(".circle-left").css("-moz-transform", "rotate(" + (deg) + "deg)");
                $(".circle-left").css("-webkit-transform", "rotate(" + (deg) + "deg)");
            }

            // 已经转换过和没有转换过的都存在，需要进行处理
            if (Device_Track[num].BDlng == 0 && Device_Track[num].BDlat == 0) {
                var gpsPoint = new BMap.Point(Device_Track[num].lng, Device_Track[num].lat);
                //方法参数必须是数组，一个一个转时需要数组pointArr = []占位，其输出永远为空；result则越来越多
                var pointArr = [];
                pointArr.push(gpsPoint);
                convertor.translate(pointArr, 1, 5, function (translateData) {
                    if (translateData.status === 0) {
                        points.push(translateData.points[0]);
                        var GpsBDRecordData = {
                            id: Device_Track[num].id,
                            BDlng: translateData.points[0].lng,
                            BDlat: translateData.points[0].lat
                        };
                        $.ajax({
                            url: '/api/devices/WriteGpsBDRecord',
                            type: 'post',
                            data: GpsBDRecordData,
                            success: function (data) {
                                //console.log(data);
                            },
                            error: function () {
                                //Layout.popMsg("popMsg-danger", "请求转化API失败");
                            }
                        })
                    }
                });

                setTimeout(function () {
                    makeLine();  //循环坐标转换，并不画线     
                }, 100)

                num++;
            } else {
                points.push({ lng: Device_Track[num].BDlng, lat: Device_Track[num].BDlat });

                setTimeout(function () {
                    makeLine();
                }, 5);

                num++;
            }
        }
    }

    $('#speed').blur(function () {
        lushu._opts.speed = $(this).val();

        lushu._opts.defaultContent = $(this).val() + " 米/秒";

        Layout.popMsg("popMsg-success", "下一段的车速 " + $(this).val() + " m / s");
    });
}

$("#plays-history").click(function () {
    try {
        lushu.start();
    } catch (e) {
        //console.log(e);
    }
});

$("#pause-history").click(function () {
    try {
        lushu.pause();
        $("#plays-history").removeAttr("disabled");
        $("#stop-history").removeAttr("disabled");
    } catch (e) {
        //console.log(e);
    }
});

$("#stop-history").click(function () {
    try {
        lushu.stop();
        Map.RemoveAllMarker("all");
        $("#plays-history").attr("disabled", "disabled");
        $("#pause-history").attr("disabled", "disabled");
        $("#stop-history").attr("disabled", "disabled");
    } catch (e) {
        //console.log(e);
        Map.RemoveAllMarker("all");
        $("#plays-history").attr("disabled", "disabled");
        $("#pause-history").attr("disabled", "disabled");
        $("#stop-history").attr("disabled", "disabled");
    }
});

