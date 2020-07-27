chrome.runtime.onMessage.addListener(
	function (request, sender, sendResponse) {
		if (request.contentScriptQuery == "badWords") {
			const url = "https://hackathon2020-wus2.azurewebsites.net/api/bad-words"
			fetch(url)
				.then(response => response.text())
				.then(text => parseBadWords(text))
				.then(badWords => sendResponse(badWords))
				.catch(error => console.error(error))
			return true;
		}
	});

function parseBadWords(text) {
	doc = JSON.parse(text);
	return doc.Content.map(function (item) {
		return item.Value;
	});
}

