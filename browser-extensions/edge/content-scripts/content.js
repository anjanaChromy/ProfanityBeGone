function profanityBegone() {
    var bodyElements = [...document.body.getElementsByTagName('*')];

    chrome.runtime.sendMessage(
        { type: "getBadWords" },
        badWords => {
            badWords.sort((a, b) => b.length - a.length)
            bodyElements.forEach(element => {
                element.childNodes.forEach(child => {
                    if (child.nodeType === Node.TEXT_NODE) {
                        replaceText(child, badWords);
                    }
                });
            });
        }
    );
}

function replaceText(node, badWords) {
    let value = node.nodeValue;
    badWords.forEach(w => {
            var regex = new RegExp('\\b' + w + '\\b', 'gi');
            value = value.replace(regex, '*'.repeat(w.length));
        });
    node.nodeValue = value;
}


chrome.runtime.onMessage.addListener(
    function (request, sender, sendResponse) {
        profanityBegone();
    }
);

chrome.runtime.sendMessage(
    {
        contentScriptQuery: "postData"
        , data: JSONdata
        , url: ApiUrl
    }, function (response) {
        debugger;
        if (response != undefined && response != "") {
            callback(response);
        }
        else {
            debugger;
            callback(null);
        }
    });