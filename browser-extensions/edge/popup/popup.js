updateBadWordsIfNone();

const sendMessageId = document.querySelector("#filterButton");
if (sendMessageId) {
  console.log("hi");
    sendMessageId.onclick = function () {
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            chrome.tabs.sendMessage(
                tabs[0].id,
                {
                    tabId: tabs[0].id
                },
                function (response) {
                    console.log("message with url sent");
                    window.close();
                }
            );
            function guidGenerator() {
                const S4 = function () {
                    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
                };
                return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
            }
        });
    };
}

const reportButtonId = document.querySelector("#reportButton");
if (reportButtonId) {
    reportButtonId.onclick = function () {
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            chrome.tabs.executeScript(tabs[0].id, {
                code: "window.getSelection().toString();"
            }, function (selection) {
                chrome.runtime.sendMessage(
                    { type: "sendTextReport", text: selection },
                    response => {
                        alert("Request has been sent for Review");
                    });
                });
        });
    };
}

$(document).ready(function () {
    $("#updateFilterButton").click(function () {
        updateBadWords();
    });
});