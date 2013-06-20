//This function reads the cost value from stores and returns a percentage.
//Handles the Bills grid, PhoneCalls grid, History page, and Delegees PhoneCalls grid
function RoundCost(value, meta, record, rowIndex, colIndex, store) {
    if (record.data.PersonalCallsCost != undefined) {
        return Math.round(record.data.PersonalCallsCost * 100) / 100;
    }
    else if (record.data.Marker_CallCost != undefined) {
        return Math.round(record.data.Marker_CallCost * 100) / 100;
    }
}


//This handles the PhoneCalls grid, History page, and Delegees PhoneCalls grid
var DateRenderer = function (value) {
    if (typeof value != undefined && value != 0) {
        if (BrowserDetect.browser != "Explorer") {
            value = Ext.util.Format.date(value, "d M Y h:i A");
            return value;
        } else {
            var my_date = {};
            var value_array = value.split(' ');
            var months = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

            my_date["date"] = value_array[0];
            my_date["time"] = value_array[1];

            var date_parts = my_date["date"].split('-');
            my_date["date"] = {
                year: date_parts[0],
                month: months[parseInt(date_parts[1])],
                day: date_parts[2]
            }

            var time_parts = my_date["time"].split(':');
            my_date["time"] = {
                hours: time_parts[0],
                minutes: time_parts[1],
                period: (time_parts[0] < 12 ? 'AM' : 'PM')
            }

            //var date_format = Date(my_date["date"].year, my_date["date"].month, my_date["date"].day, my_date["time"].hours, my_date["time"].minutes);
            return (
                my_date.date.day + " " + my_date.date.month + " " + my_date.date.year + " " +
                my_date.time.hours + ":" + my_date.time.minutes + " " + my_date.time.period
            );
        }//END ELSE
    }//END OUTER IF
}


//This function handles a special case of server-side generated date-and-time value in the Bills History page
var SpecialDateRenderer = function (value) {
    if (typeof value != undefined && value != 0) {
        var months_array = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var months_long_names = {
            'Jan': 'January',
            'Feb': 'February',
            'Mar': 'March',
            'Apr': 'April',
            'May': 'May',
            'Jun': 'June',
            'Jul': 'July',
            'Aug': 'August',
            'Sep': 'September',
            'Oct': 'October',
            'Nov': 'November',
            'Dec': 'December'
        };

        var date = value.toString();
        var date_array = date.split(' ');

        //The following is a weird IE bugfix
        //@date string appears in IE like this: "Thu Jan 31 00:00:00 UTC+0200 2013"
        //@date string appears in other browsers like this: "Thu Jan 31 2013 00:00:00 GMT+0200 (GTB Standard Time)"
        //So by splitting the string on different browsers, you get different index of the year substring!
        if (BrowserDetect.browser == "Explorer") {
            return (months_long_names[date_array[1]] + ", " + date_array[date_array.length - 1]); //year is at last index
        } else {
            return (months_long_names[date_array[1]] + ", " + date_array[3]); //year is at 4th index
        }
    }
}


//This is used in the PhoneCalls page, History page, and Delegees page
function getRowClassForIsPersonal(value, meta, record, rowIndex, colIndex, store) {
    if (record.data != null) {
        if (record.data.UI_CallType == 'Personal') {
            meta.style = "color: rgb(201, 20, 20);";
        }
        if (record.data.UI_CallType == 'Business') {
            meta.style = "color: rgb(46, 143, 42);";
        }
        if (record.data.UI_CallType == 'Dispute') {
            meta.style = "color: rgb(31, 115, 164);";
        }

        return value
    }
}


//This is used in the PhoneCalls page, History page, and Delegees page
function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
    if (record.data.AC_IsInvoiced == 'NO') {
        meta.style = "color: rgb(201, 20, 20);";
    }
    if (record.data.AC_IsInvoiced == 'YES') {
        meta.style = "color: rgb(46, 143, 42);";
    }
    return value
}


//This is used in the PhoneCalls page, Bills page, History page, and Delegees page
function GetMinutes(value, meta, record, rowIndex, colIndex, store) {
    var sec_num = 0;

    //Handles the case of Mangage Phone Calls Grid in the Phone Calls page
    if (record.data.Duration != undefined) {
        sec_num = parseInt(record.data.Duration, 10);
    }
    //Handles the case of Bills History Grid in the Bills page
    else if (record.data.PersonalCallsDuration != undefined) {
        sec_num = parseInt(record.data.PersonalCallsDuration, 10);
    }
    
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) {
        hours = "0" + hours;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    if (seconds < 10) {
        seconds = "0" + seconds;
    }

    return hours + ':' + minutes + ':' + seconds;;
}

//This is used in the PhoneCalls page, User Statistics, and User Dashboard page
var GetHoursFromMinutes = function (value) {
    var sec_num = parseInt(value, 10);
    var hours = Math.floor(sec_num / 60);
    var minutes = Math.floor((sec_num - (hours * 60)));
    return hours + "." + minutes;
};


var submitValue = function (grid, hiddenFormat, format) {
    grid.submitData(false, { isUpload: true });
};


var onShow = function (toolTip, grid) {
    var view = grid.getView(),
        store = grid.getStore(),
        record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
        column = view.getHeaderByCell(toolTip.triggerElement),
        data = record.get(column.dataIndex);

    if (column.id == "main_content_place_holder_DestinationNumberUri") {
        data = record.get("PhoneBookName");
    }

    toolTip.update(data);
};


//This function is used in the User Dashboard page, and User Statistics page for handling the duration value in the charts
var chartsDurationFormat = function (seconds) {
    var sec_num = parseInt(seconds, 10);
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) hours = "0" + hours;
    if (minutes < 10) minutes = "0" + minutes;
    if (seconds < 10) seconds = "0" + seconds;

    return hours + ':' + minutes + ':' + seconds;
}