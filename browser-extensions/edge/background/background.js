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
            const reportUrl = "https://hackathon2020-wus2.azurewebsites.net/api/report-now";
            fetch(reportUrl, {
                method: "POST",
                body: JSON.stringify({ ContentValue: request.text.toString() }),
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