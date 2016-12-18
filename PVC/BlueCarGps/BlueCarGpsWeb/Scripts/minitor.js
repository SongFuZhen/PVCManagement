var Minitor = {};

//定义全局变量
var All_Device = new Array(),
    Alarm_Device = new Array(),
    Working_Device = new Array(),
    LowBattery_Device = new Array(),
    Offline_Device = new Array(),
    Idol_Device = new Array();

Minitor.mapInit = function () {
    $('.tab-pane ul').css({ height: $(window).height() - 210 + 'px' });

    if ($('.device_show').css("display").trim() == "none") {
        $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width() - 30 + 'px' });
        $(window).resize(function () {
            $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width() - 30 + 'px' });
        });
    } else {
        $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width() - 330 + 'px' });
        $(window).resize(function () {
            $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width() - 330 + 'px' });
        });
    }

    Map.mapInit('baiduMap');

    Minitor.SetDataToSelect();

    var ShopID = $('#search_shop_type').val();
    var BankID = $('#search_bank_type').val();
    //VIN
    //Brand
    var BrandID = $('#search_brand_type').val();
    var VIN = $('#vin').val();

    //绘制地图数据
    Minitor.DrawMap(ShopID, BankID,BrandID,VIN);

    //点击左边的Car
    Minitor.ClickCar();
}

Minitor.SetDataToSelect = function () {
    $.ajax({
        url: '/api/brands/GetAll',
        type: 'get',
        success: function (data) {
            $('#search_brand_type').empty();

            $('#search_brand_type').append("<option value=''></option>");
            for (var msg in data) {
                var BrandMsg = data[msg];
                $('#search_brand_type').append("<option value=" + BrandMsg.id + " BankOrShop='bank'>" + BrandMsg.name + "</option>");
            }

            $('#search_brand_type').comboSelect();
        },
        error: function () {
            Layout.popMsg('popMsg-danger', "汽车品牌API请求失败");
        }
    });

    $.ajax({
        url: '/api/banks/GetAll',
        type: 'get',
        success: function (data) {
            $('#search_bank_type').empty();

            $('#search_bank_type').append("<option value=''></option>");
            for (var msg in data) {
                var BankMsg = data[msg];
                $('#search_bank_type').append("<option value=" + BankMsg.id + " BankOrShop='bank'>" + BankMsg.name + "</option>");
            }

            $('#search_bank_type').comboSelect();
        },
        error: function () {
            Layout.popMsg('popMsg-danger', "银行API请求失败");
        }
    });


    $.ajax({
        url: '/api/shops/GetAll',
        type: 'get',
        success: function (data) {
            $('#search_shop_type').empty();

            $('#search_shop_type').append("<option value=''></option>");

            for (var msg in data) {
                var ShopMsg = data[msg];
                $('#search_shop_type').append("<option value=" + ShopMsg.id + " BankOrShop='shop'>" + ShopMsg.name + "</option>");
            }

            $('#search_shop_type').comboSelect();
        },
        error: function () {
            Layout.popMsg('popMsg-danger', "4S店API请求失败");
        }
    });
}

$('#Btn_Search').click(function () {
    //绘制地图数据
    Minitor.DrawSearchMap();
    //点击左边的Car
    Minitor.ClickCar();
});

$('#toggle-left-menu').click(function () {
    if ($('.device_show').css("display").trim() == "none") {
        $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width() - 330 + 'px' });
    } else {
        $('#baiduMap').css({ height: $(window).height() - 190 + 'px', width: $(window).width()-30 + 'px' });
    }
    $(".device_show").fadeToggle();
})

Minitor.DrawSearchMap = function () {
    var BankID = $('#search_bank_type').val();
    var ShopID = $('#search_shop_type').val();
    var BrandID = $('#search_brand_type').val();
    var VIN = $('#vin').val();
    Minitor.DrawMap(ShopID, BankID,BrandID,VIN);

    $('#search_bank_type').find('option').each(function (i) {
        if ($(this).val() === BankID) {
            $(this).attr("selected", "selected");
        }
    });
    $('#search_brand_type').find('option').each(function (i) {
        if ($(this).val() === BrandID) {
            $(this).attr("selected", "selected");
        }
    });

    $('#search_shop_type').find('option').each(function (i) {
        if ($(this).val() === ShopID) {
            $(this).attr("selected", "selected");
        }
    });
}

Minitor.DrawMap = function (shopId, bankId,brandId,vin) {
    $('#loading').css("top", "160px");
    Layout.show_loading();

    $.ajax({
        url: '/api/Devices/GetAll',
        type: 'get',
        dataType: 'json',
        data:{
            shopId: shopId,
            bankId: bankId,
            brandId: brandId,
            vin:vin
        },
        success: function (data) {
            for (var status in data) {
                switch (data[status].state) {
                    case 100:
                        //返回 全部信息
                            All_Device = data[status].devices;
                        break;
                    case 200:
                        //返回 警报
                            Alarm_Device = data[status].devices;
                        break;
                    case 300:
                        //返回 工作中
                            Working_Device = data[status].devices;
                        break;
                    case 400:
                        //返回 低电量
                            LowBattery_Device = data[status].devices;
                        break;
                    case 500:
                        //返回 离线
                            Offline_Device = data[status].devices;
                        break;
                    case 600:
                        //返回 Idol
                            Idol_Device = data[status].devices;
                        break;
                    default:
                        //返回 ...
                        break;
                }
            }

            //调用填充数据函数
            Minitor.FillDate();

            Layout.hide_loading();
        },
        error: function () {
            //console.log("Something Error!");
            Layout.popMsg('popMsg-danger', '请求设备API失败');
        }
    })
}

Minitor.FillDate = function () {
    //刚进入界面就加载所有信息
    Minitor.AppendContent(All_Device, '#all_device_ul');

    //填充Badge
    Minitor.AppendBadge(All_Device.length, '.all_tab');
    Minitor.AppendBadge(Alarm_Device.length, '.alarm_tab');
    Minitor.AppendBadge(Working_Device.length, '.working_tab');
    Minitor.AppendBadge(LowBattery_Device.length, '.low_battery_tab');
    Minitor.AppendBadge(Offline_Device.length, '.offline_tab');
    Minitor.AppendBadge(Idol_Device.length, '.idol_tab');

    $('.all_tab').click(function () {
        Minitor.AppendContent(All_Device, '#all_device_ul');
    })

    $('.alarm_tab').click(function () {
        Minitor.AppendContent(Alarm_Device, '#alarm_device_ul');
    })

    $('.working_tab').click(function () {
        Minitor.AppendContent(Working_Device, '#working_device_ul');
    })

    $('.low_battery_tab').click(function () {
        Minitor.AppendContent(LowBattery_Device, '#low_battery_device_ul');
    })

    $('.offline_tab').click(function () {
        Minitor.AppendContent(Offline_Device, '#offline_device_ul');
    })

    $('.idol_tab').click(function () {
        Minitor.AppendContent(Idol_Device, '#idol_device_ul');
    })
}

Minitor.AppendContent = function (DeviceArr, cls) {
    Map.RemoveAllMarker("all");
    $(cls).empty();

    //此处进行循环
    if (DeviceArr.length > 0) {
        var deviceShow="";
        for (var device = 0; device < DeviceArr.length; device++) {
            var OldDevice = DeviceArr[device];
            var ggPoint = new BMap.Point(OldDevice.lng, OldDevice.lat);
            //进行坐标转化
            
            if (device == 0) {
                //自动定位到第一个坐标处
                map.centerAndZoom(new BMap.Point(OldDevice.lng, OldDevice.lat), 10);
            }

            //此处可以添加隐藏控件 或者选择直接使用ajax调用
            deviceShow += "<li id='" + OldDevice.id + "' device_code='" + OldDevice.nr + "' " +
                "gps_lat='" + OldDevice.lat + "' gps_lng='" + OldDevice.lng + "'>" +
                "<span class='pull-left'><i class='fa fa-car'></i>  " + OldDevice.vin+" / " +OldDevice.imei + "</span>" +
                "<br /><span>" + (OldDevice.bankName == "" ? "无" : OldDevice.bankName) + "</span>" +
                "&nbsp; &nbsp;/&nbsp; &nbsp;<span'>" + (OldDevice.shopName == "" ? "无" : OldDevice.shopName) + "&nbsp;&nbsp;</span>" +
                "</li>";

            //添加标注 不超过 50个
            //if (device < 50) {
            Minitor.AddMarker(OldDevice);
            //} else {
            //提示信息
            //数据量太大， 将显示前五十个， 如果要看，请点击左侧
            //}
        }

        $(cls).append(deviceShow);
    } else {
        //没有数据， 可以进行填充提示
        //在ul tab 上或者弹窗进行提示
    }
}

Minitor.AppendBadge = function (count, tabCls) {
    //填充信息, 在第一次的时候就完成
    $(tabCls).find('.badge').empty();
    $(tabCls).append('<span class="badge">(' + count + ')</span>');
}

//单个点进行添加标注
Minitor.AddMarker = function (Device) {
    //是先删除标记 然后做判断 还是 先做判断在删除？
    //TODO: 先删除，后判断： 每次切换都将会删除标记，如果遇到新的tab中没有值，则显示空
    // 先判断，后删除： 每次判断之后在删除， 如果 选择的tab中为空值，则显示上一个有值的，容易造成误导

    if (Device.CurrentBDLng == 0 && Device.CurrentBDLat == 0) {
        var ggPoint = new BMap.Point(Device.lng, Device.lat);
        var convertor = new BMap.Convertor();
        var pointArr = [];
        pointArr.push(ggPoint);
        convertor.translate(pointArr, 1, 5, function (translateData) {
            if (translateData.status === 0) {
                var Icon = normalIcon;

                if (Device.fenceStateDisplay == "围栏外") {
                    Icon = alarmIcon;
                }

                var marker = new BMap.Marker(translateData.points[0], { icon: Icon });  // 创建标注
                map.addOverlay(marker);              // 将标注添加到地图中

                var label = new BMap.Label(Device.nr, { offset: new BMap.Size(20, -9) });
                marker.setLabel(label);

                var href_link = '<a href="/Home/RealTimeTrace?device_nr=' + Device.nr + '&id=' + Device.id + '" target="_blank">实时跟踪 </a>' +
                    '<a href="/Home/History?device_nr=' + Device.nr + '&id=' + Device.id + '" target="_blank">历史轨迹 </a>';

                var content = "<b>" + Device.nr + "</b><hr /><b>IMEI号: </b>" +
                    Device.imei + "<br /><b>状态: </b>" + Device.workStateDisplay + "<br /><b>电量: </b>" +
                    Device.batteryStateDisplay + "<br /><b>信号时间: </b>" + Device.signalTime + "<br /><b>定位方式: </b>" +
                    Device.singleType + "<br /><b>VIN号: </b>" + Device.vin + "<br /><b>Shop: </b>" + Device.shopName + "<br /><b>围栏状态: </b>" + Device.fenceStateDisplay + "<hr />" + href_link;

                //添加信息窗口
                addClickHandler(content, marker);

                // 开始写入数据库
                $.ajax({
                    url: "/api/devices/WriteBDDevice",
                    type: 'get',
                    data: {
                        id: Device.id,
                        currentBDlng: translateData.points[0].lng,
                        currentBDlat: translateData.points[0].lat
                    },
                    success: function (data) {
                        //console.log(data);
                    },
                    error: function () {
                        //console.log("不需要进行转换坐标");
                    }
                });
            }
        })
    } else {
        var Icon = normalIcon;

        if (Device.fenceStateDisplay == "围栏外") {
            Icon = alarmIcon;
        }

        var marker = new BMap.Marker(new BMap.Point(Device.CurrentBDLng, Device.CurrentBDLat), { icon: Icon });  // 创建标注
        map.addOverlay(marker);              // 将标注添加到地图中

        var label = new BMap.Label(Device.nr, { offset: new BMap.Size(20, -9) });
        marker.setLabel(label);

        var href_link = '<a href="/Home/RealTimeTrace?device_nr=' + Device.nr + '&id=' + Device.id + '" target="_blank">实时跟踪 </a>' +
            '<a href="/Home/History?device_nr=' + Device.nr + '&id=' + Device.id + '" target="_blank">历史轨迹 </a>';

        var content = "<b>" + Device.nr + "</b><hr /><b>IMEI号: </b>" +
            Device.imei + "<br /><b>状态: </b>" + Device.workStateDisplay + "<br /><b>电量: </b>" +
            Device.batteryStateDisplay + "<br /><b>信号时间: </b>" + Device.signalTime + "<br /><b>定位方式: </b>" +
            Device.singleType + "<br /><b>VIN号: </b>" + Device.vin + "<br /><b>Shop: </b>" + Device.shopName + "<br /><b>围栏状态: </b>" + Device.fenceStateDisplay + "<hr />" + href_link;

        //添加信息窗口
        addClickHandler(content, marker);
    }
}
 
//显示信息窗口
var opts = {
    width: 250,     // 信息窗口宽度
    height: 0,     // 信息窗口高度
    title: "", // 信息窗口标题
    enableMessage: true//设置允许信息窗发送短息
};

function addClickHandler(content, marker) {
    marker.addEventListener("click", function (e) {
        openInfo(content, e)
    });
}

function openInfo(content, e) {
    var p = e.target;
    var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
    var infoWindow = new BMap.InfoWindow(content, opts);  // 创建信息窗口对象
    map.openInfoWindow(infoWindow, point); //开启信息窗口
}

//点击左边
Minitor.ClickCar = function () {
    //点击之后Add 标注, 去除其他标注
    $('.tab-pane ul').unbind('click').on('click', 'li', function () {
        $('.tab-pane li').css({ color: 'grey' });
        $(this).css({ color: '#CF2526' });
        Map.RemoveAllMarker([""]);

        var ggPoint = new BMap.Point($(this).attr('gps_lng'), $(this).attr('gps_lat'));  // 创建点坐标, 上海张江    默认可以为 北京天安门

        var convertor = new BMap.Convertor();
        var pointArr = [];
        pointArr.push(ggPoint);
        convertor.translate(pointArr, 1, 5, function (translateData) {
            if (translateData.status === 0) {
                map.centerAndZoom(translateData.points[0], 15);

                var marker2 = new BMap.Marker(translateData.points[0]);  // 创建标注
                map.addOverlay(marker2);              // 将标注添加到地图中
                marker2.setAnimation(BMAP_ANIMATION_DROP); //设置点的坠落动画 BMAP_ANIMATION_BOUNCE 跳动动画

                var label = new BMap.Label("", { offset: new BMap.Size(100000, 1000000), setZIndex: -10 });
                marker2.setLabel(label);
                marker2.enableDragging();
            }
        });
    });
}