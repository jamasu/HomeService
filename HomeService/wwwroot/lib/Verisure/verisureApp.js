
	var config = {
		username: 'suleiman.jama@icloud.com',
		password: 'SLamdu&6'
	};


	var verisureApi = require('./verisure-api').setup(config);


	// alarm state changes
	verisureApi.on('alarmChange', log);

	// climate measurement changes
	verisureApi.on('climateChange', log);

	function log(data) {
		console.log(data);
	}
