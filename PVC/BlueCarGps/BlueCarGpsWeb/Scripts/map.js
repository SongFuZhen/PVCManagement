var Map = {};

var map,
    normalIcon,
    alarmIcon,
    shopIcon,
    myDis;

Map.mapInit = function (id) {
    //TODO:判断网络， 如果没有网络， 进行报错处理，并关闭网站
    //防止网站密钥被看到
    map = new BMap.Map(id);
    normalIcon = new BMap.Icon("../images/car.png", new BMap.Size(64, 40), { imageOffset: new BMap.Size(0, 0) });
    alarmIcon = new BMap.Icon("../images/alarm_car.png", new BMap.Size(64, 40), { imageOffset: new BMap.Size(0, 0) });
    shopIcon = new BMap.Icon("../../images/shop.png", new BMap.Size(64, 40), { imageOffset: new BMap.Size(0, 0) });
    gfenceIcon = new BMap.Icon("../../images/gfence.png", new BMap.Size(64, 64), { imageOffset: new BMap.Size(0, 0) });
    myDis = new BMapLib.DistanceTool(map);

    // 创建地图实例
    var point = new BMap.Point(118.591, 31.210);  // 创建点坐标, 上海张江    默认可以为 北京天安门
    map.centerAndZoom(point, 10);                 // 初始化地图，设置中心点坐标和地图级别
    map.enableScrollWheelZoom();// 设置滚轮滚动

    var ctrl = new BMapLib.TrafficControl({ showPanel: true });
    map.addControl(ctrl);
    ctrl.setAnchor(BMAP_ANCHOR_BOTTOM_RIGHT);

    //创建左上角 缩放
    var opts_nav = { type: BMAP_NAVIGATION_CONTROL_LARGE }
    map.addControl(new BMap.NavigationControl(opts_nav));

    //创建右上角 地图切换
    var mapType1 = new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] });
    //var overView = new BMap.OverviewMapControl();
    //var overViewOpen = new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT });
    map.addControl(mapType1);          //2D图，卫星图
    map.setCurrentCity("上海");        //由于有3D图，需要设置城市哦
    //map.addControl(overView);          //添加默认缩略地图控件
    //map.addControl(overViewOpen);      //右下角，打开

    //进入全景
    Map.enterPanorama();

    //选择地图Style
    Map.mapStyle();
}

//地图功能
Map.enterPanorama = function () {
    // 覆盖区域图层测试
    map.addTileLayer(new BMap.PanoramaCoverageLayer());

    var stCtrl = new BMap.PanoramaControl(); //构造全景控件
    stCtrl.setOffset(new BMap.Size(20, 60));
    map.addControl(stCtrl);//添加全景控件
}

//地图样式
Map.mapStyle = function () {
    //初始化模板选择的下拉框
    var sel = document.getElementById('stylelist');
    $('#stylelist').empty();

    for (var key in mapstyles) {
        var style = mapstyles[key];
        var item = new Option(style.title, key);
        sel.options.add(item);
    }

    changeMapStyle('normal')
    sel.value = 'normal';
}

function changeMapStyle(style) {
    map.setMapStyle({ style: style });
    $('#desc').html(mapstyles[style].desc);
}

//移除标注, 参数是"all" 将删除所有的标注
// 参数是label数组， 将删除对应label的标注

Map.RemoveAllMarker = function (labels) {
    var allOverlay = map.getOverlays();

    if (allOverlay.length > 0) {
        if (labels == "all") {
            //删除全部标记
            for (var i = 0; i < allOverlay.length; i++) {
                map.removeOverlay(allOverlay[i]);
            }
        } else {
            //删除特定的标记
            for (var label in labels) {
                for (var i = 0; i < allOverlay.length; i++) {
                    //防止异常
                    try {
                        if (allOverlay[i].getLabel().content == labels[label]) {
                            map.removeOverlay(allOverlay[i]);
                        }
                    } catch (e) {
                        //console.log(e);
                    }
                }
            }
        }
    }
}