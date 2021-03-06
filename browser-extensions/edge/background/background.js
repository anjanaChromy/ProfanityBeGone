chrome.runtime.onMessage.addListener(
	function (request, sender, sendResponse) {
        if (request.type == "getBadWords") {
            getBadWords()
                .then(function (response) {
                    var text = JSON.stringify(response);
                    var badWords = parseBadWords(text);
                    sendResponse(badWords);
                })
                .catch(error => console.error(error));

            return true;
        }
		else if (request.type == "sendTextReport") {
            var uuid = CreateUUID();
            const reportUrl = "https://hackathon2020-wus2.azurewebsites.net/api/report-now";
			fetch(reportUrl, {
				method: "POST",
				body: JSON.stringify({ContentValue: request.text.toString()}),
				headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'text/json'
                }
			})
				.then(response => sendResponse(response))
				.catch(error => console.error(error));
			return true;
		}
	});

function CreateUUID() {
	return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
		(c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
	)
}

function parseBadWords(text) {
    doc = JSON.parse(text);
    return doc.Content.map(function (item) {
        return item.Value;
    });
}

function getBadWords() {
    return new Promise(resolve => {
        chrome.storage.local.get(['BadWords'], function (result) {
            resolve(result.BadWords);
        });
    });
}