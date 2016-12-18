var RealTimeTrace = {};
var IsFirstLoad = true;

RealTimeTrace.Init = function () {
    var urlStr = Layout.GetUrlString(location.href);
    $('#device_nr').val(urlStr.device_nr);
    $('#device_id').val(urlStr.id);
}

RealTimeTrace.mapInit = function () {
    //设置高度
    $('#real_time_map').css({ height: $(window).height() - 110 + 'px', width: $(window).width()+'px' });

    $(window).resize(function () {
        $('#real_time_map').css({ height: $(window).height() - 110 + 'px', width: $(window).width() + 'px' });
    });

    Map.mapInit('real_time_map');

    //绘制地图数据
    RealTimeTrace.DrawMap();
}

RealTimeTrace.DrawMap = function () {
    RealTimeTrace.GetPoint();

    setInterval(function () {
        RealTimeTrace.GetPoint();
    }, 10000);
}

RealTimeTrace.GetPoint = function () {
    $.ajax({
        url: '/api/devices/GetLastest',
        type: 'get',
        dataType: 'json',
        data: {
            id: $('#device_id').val()
        },
        success: function (data) {
            console.log(data);
            //假数据处理
            //var data = PseudoData.RealTimeTrace();
            Map.RemoveAllMarker("all");
            var pt = new BMap.Point(data.lng, data.lat);
            var convertor = new BMap.Convertor();
            var pointArr = [];
            pointArr.push(pt);
            convertor.translate(pointArr, 1, 5, function (translateData) {
                if (translateData.status === 0) {
                    var Icon = normalIcon;
                    if (data.fenceStateDisplay == "围栏外") {
                        Icon = alarmIcon;
                    }

                    var marker = new BMap.Marker(translateData.points[0], { icon: Icon });
                    map.addOverlay(marker);

                    if (IsFirstLoad) {
                        map.centerAndZoom(translateData.points[0], 10);
                        IsFirstLoad = false;
                    }

                    var href_link = '<a href="/Home/History?device_nr=' + data.nr + '&id=' + data.id + '" target="_blank">历史轨迹 </a>';

                    var content = "<b>" + data.nr + "</b> <i class='fa fa-minus pull-right' id='info-drop' style='cursor:pointer;padding:5px 6px; color:black;'></i> <div class='info-body' style='display:none;'> <hr /><b>IMEI号: </b>" +
                                data.imei + "<br /><b>状态: </b>" + data.workStateDisplay + "<br /><b>电量状态: </b>" +
                                data.batteryStateDisplay + "<br /><b>电量: </b>" + data.batteryPercent + "<br /><b>信号时间: </b>" + data.signalTime + "<br /><b>定位方式: </b>" +
                                data.singleType + "<br /><b>VIN号: </b>" + data.vin + "<br /><b>Shop: </b>" + data.shopName + "<br /><b>围栏状态: </b>" + data.fenceStateDisplay + "<hr /></div>" + href_link;

                    $('#device_info').empty();
                    $('#device_info').append(content);

                    var WhereFrom = $('#from_where').val();

                    if (WhereFrom == 'wx') {
                        $('.row').css({ display: 'none' });
                        $('.navbar').css({ display: 'none' });
                        $('#real_time_map').css({ height: $(window).height() + 'px' });
                        $("a").remove();

                        //取消警报
                        clearInterval(AlarmInterval);

                        $('#alarm_audio').remove();
                    }
                }
            });

            $('body').unbind('click').on('click', '#info-drop', function () {
                $('.info-body').slideToggle();
            });
        },
        error: function () {
            //console.log("Something Error!");
            Layout.popMsg('popMsg-danger', '请求实时跟踪API失败');
        }
    });
}
