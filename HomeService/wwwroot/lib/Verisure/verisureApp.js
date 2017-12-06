
var config = {
	username: '',
	password: ''
};

var verisureApi = require('./verisure-api').setup(config);

// climate measurement changes
module.exports.sendClimateData = function (callback) {
	verisureApi.on('climateChange', sendClimateData);

	function sendClimateData(data) {
		console.log("i SendClimateData", data);
		var parsed = JSON.parse(JSON.stringify(data));
		objJSON = parsed;
		for (var i = 0, len = objJSON.length; i < len; ++i) {
			var climateData = objJSON[i];
			console.log(climateData.location);
			console.log(climateData.temperature);
			console.log(climateData.humidity);
			console.log(climateData.timestamp);
			console.log("----------------------");
			callback(null, objJSON);

		}
	}
}

module.exports.sendAlarmStatus = function (callback) {
	// alarm state changes
	verisureApi.on('alarmChange', sendAlarmStatus);
	//{ date: 'Today 6:45 AM', status: 'unarmed' }
	function sendAlarmStatus(data) {
		console.log("I sendAlarmStatus", data);
		var parsed = JSON.parse(JSON.stringify(data));
		objJSON = parsed;
		callback(null, parsed);
	}
}
