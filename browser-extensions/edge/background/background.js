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

			var uuid = CreateUUID();
			const reportUrl = `https://westus2.api.cognitive.microsoft.com/contentmoderator/review/v1.0/teams/hackathon2020wus2/jobs?ContentType=Text&ContentId=${uuid}&WorkflowName=hackathontext`
			fetch(reportUrl, {
				method: "POST",
				body: JSON.stringify({ContentValue: request.text}),
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'text/json',
					'Ocp-Apim-Subscription-Key': 'todo:subscriptionkey_configuration'
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

