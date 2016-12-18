var PseudoData = {};

PseudoData.Monitor = function () {
    var data = new Array();
    var state = 100;
    for (var j = 0; j < 7; j++) {
        var devices = new Array();
        if (j == 1) state = 200;
        else if (j == 2) state = 300;
        else if (j == 3) state = 400;
        else if (j == 4) state = 500;
        else if (j == 5) state = 600;
        else if (j == 6) state = 700;

        //TODO:海量数据的处理
        for (var i = Math.round(Math.random() * 10) ; i < Math.round(Math.random() * 700000) ; i++) {
            //创建数据
            devices.push(
                {
                    id: state + i,
                    nr: 'GT300S-4558' + state + i,
                    imei: '86812014284558' + i + state,
                    status: "离线",
                    batteryPercent: Math.random() * 100 + '%',
                    signalTime: '2016-09-2' + Math.round(Math.random() * 9) + ' 15:12:09',
                    lat: 30 + Math.random(),
                    lng: 120 + Math.random(),
                    signalType: "卫星",
                    workStateDisplay: "在线",
                    batteryStateDisplay: "低电量"
                });
        }
        data.push({ state: state, devices: devices });
    }

    return data;
}

PseudoData.History = function () {
    var data;
    var gps = new Array();
    for (var i = Math.round(Math.random() * 10) ; i < Math.round(Math.random() * 70) ; i++) {
        gps.push({
            lng: 100 +  Math.random(),
            lat:20+ Math.random(),
            signalTime: '2016-09-2' + Math.round(Math.random() * 9) + ' 15:12:09',
            signalType: "卫星"
        })
    }

    data={
        device: {
            id: 144,
            nr: 'GT300S-4558100144',
            imei: '86812014284558100144',
            status: "离线",
            batteryPercent: Math.random() * 100 + '%',
            signalTime: '2016-09-2' + Math.round(Math.random() * 9) + ' 15:12:09',
            lng: 120 + Math.random(),
            lat: 30 + Math.random(),
            signalType: "卫星",
            workStateDisplay: "在线",
            batteryStateDisplay: "低电量"
        },
        gps: gps
    };

    return data;
}

var i = 0.01111111, j = 0.01111111;
PseudoData.RealTimeTrace = function () {
    var data = {
        id: 144,
        nr: 'GT300S-4558100144',
        imei: '86812014284558100144',
        status: "离线",
        batteryPercent: Math.random() * 100 + '%',
        signalTime: '2016-09-2' + Math.round(Math.random() * 9) + ' 15:12:09',
        lng: 120 + i,
        lat: 30 + j,
        signalType: "卫星",
        workStateDisplay: "在线",
        batteryStateDisplay: "低电量"
    };

    i += 0.005;
    j += 0.004;
    return data;
}