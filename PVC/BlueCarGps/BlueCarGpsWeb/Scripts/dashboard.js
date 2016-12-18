var Dashboard={};

Dashboard.Init = function () {
    Dashboard.GetUserCount();
    Dashboard.GetDeviceCount();
    Dashboard.GetBanksCount();
    Dashboard.GetShopsCount();
    Dashboard.GetDeviceBatteryLevel();
    Dashboard.GetRoleType();

    $(document).ready(function () {
        Layout.show_loading();
    });
}



Dashboard.GetUserCount = function () {
    $.ajax({
        url: '/api/Users/GetUserCount',
        type: 'get',
        success: function (data) {
            $('#users_number').attr("dataValue", data);
            Dashboard.AutoNumber("users_number");
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求用户数量API失败");
        }
    })
}

Dashboard.GetDeviceCount = function () {
    $.ajax({
        url: '/api/Devices/GetDeviceCount',
        type: 'get',
        success: function (data) {
            Layout.hide_loading();

            $('#devices_number').attr("dataValue", data);
            Dashboard.AutoNumber("devices_number");
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求设备数量API失败");
        }
    })
}

Dashboard.GetShopsCount = function () {
    $.ajax({
        url: '/api/Shops/GetShopsCount',
        type: 'get',
        success: function (data) {
            $('#shops_number').attr("dataValue", data);
            Dashboard.AutoNumber("shops_number");
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求4S店数量API失败");
        }
    })
}

Dashboard.GetBanksCount = function () {
    $.ajax({
        url: '/api/Banks/GetBanksCount',
        type: 'get',
        success: function (data) {
            $('#banks_number').attr("dataValue", data);
            Dashboard.AutoNumber("banks_number");
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求银行数量API失败");
        }
    })
}

Dashboard.GetDeviceBatteryLevel=function () {
    $.ajax({
        url: '/api/Devices/GetDeviceBatteryLevel',
        type: 'get',
        success: function (data) {

            var XAxis = new Array();
            var SeriesData = new Array();

            var colors = ["#CF2526", "#f1c40f", "#e67e22", "#34495e", "#9b59b6", "#3498db", "#1abc9c"];

            for (var i in data) {
                XAxis.push(i);
                SeriesData.push({ name: i, y: parseInt(data[i]), color: colors[i] });
            }

            console.log(SeriesData);

            $('#device_battery_level_charts').highcharts({
                chart: {
                    type: 'pie',
                    borderRadius:"5",
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    },
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                credits:{
                    enabled:false
                },
                title: {
                    text: '电量等级分布图'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return "设备数: " + this.point.y + "<br />占比" + this.point.percentage.toFixed(2) + '%';
                            },
                            connectorColor: 'silver'
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    type: 'pie',
                    name: '占比',
                    data: SeriesData
                }]
            });
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求设备分类数量API失败");
        }
    });
}

Dashboard.GetRoleType = function () {
    $.ajax({
        url: '/api/Users/GetRoleType',
        type: 'get',
        success: function (data) {
            var XAxis = new Array();
            var SeriesData = new Array();
      
            for (var i in data) {
                XAxis.push(i);
                SeriesData.push({ name: i, y: parseInt(data[i]) });
            }
           
            $('#role_type_charts').highcharts({
                chart: {
                    borderRadius:'5',
                    type: 'column',
                    margin: 75
                },
                colors:["red", "blue"],
                title: {
                    text: '用户角色分布图'
                },
                credits: {
                    text: 'IF TECH',
                    href: 'http://www.if-industry.com'
                },
                tooltip: {
                    formatter: function () {
                        return '角色 <b> ' + this.x +
                            '</b> 有 <b>' + this.y + '</b> 位用户';
                    }
                },
                plotOptions: {
                    column: {
                        depth: 25
                    }
                    
                },
              
                xAxis: {
                    categories: XAxis
                },
                yAxis: {
                    title: {
                        text: '用户 (人)'
                    }
                },
                
                series: [{
                    name: '用户分布',
                    colorByPoint:true,
                    data: SeriesData
                }]
            });
        },
        error: function () {
            Layout.popMsg("popMsg-danger", "请求用户角色分类API失败");
        }
    });
}

//数字递增效果
Dashboard.AutoNumber = function (id) {
    var DataTo = $("#" + id).attr("dataValue");
    var i = 0;
    var count = Math.ceil(DataTo / 2) + 1;

    var NumberInterval = setInterval(function () {
        i += count;
        if (i <= DataTo) {
            $('#' + id).html(i);
        } else {
            //防止 i 不是目标值，显示错误
            $('#' + id).html(DataTo);
            //清除计时器
            clearInterval(NumberInterval);
            return;
        }
    }, 100);
}