chrome.runtime.onMessage.addListener(
	function (request, sender, sendResponse) {
		if (request.type == "getBadWords") {
			const url = "https://hackathon2020-wus2.azurewebsites.net/api/bad-words"
			fetch(url)
				.then(response => response.text())
				.then(text => parseBadWords(text))
				.then(badWords => sendResponse(badWords))
				.catch(error => console.error(error))
			return true;
		}
		else if (request.type == "sendTextReport") {
			const reportUrl = "https://hackathon2020-wus2.cognitiveservices.azure.com/contentmoderator/moderate/v1.0/ProcessText/Screen?autocorrect=true&classify=True"
			fetch(reportUrl, {
				method: "POST",
				body: request.text,
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'text/plain',
					'Ocp-Apim-Subscription-Key': 'todo:subscriptionkey_configuration'
				}
			})
			.then(response => sendResponse(response))
			.catch(error => console.error(error));
			return true;
		}
	});

function parseBadWords(text) {
	doc = JSON.parse(text);
	return doc.Content.map(function (item) {
		return item.Value;
	});
}

