function updateBadWordsLastUpdated() {
    chrome.storage.local.get(['BadWordsLastUpdated'], function (result) {
        $('#badWordsLastUpdated').html('Last Updated: ' + result.BadWordsLastUpdated.DateString);
    });
}

function updateBadWords() {
    $.getJSON('https://hackathon2020-wus2.azurewebsites.net/api/bad-words', function (data) {
        chrome.storage.local.set({ 'BadWords': data }, function () { });

        var date = new Date();
        var dateObj = {
            Date: date,
            DateString: date.toString()
        };

        chrome.storage.local.set({ 'BadWordsLastUpdated': dateObj }, function () { });
    });

    updateBadWordsLastUpdated();
}

function updateBadWordsIfNone() {
    updateBadWordsLastUpdated();

    chrome.storage.local.get(['BadWords'], function (result) {
        if (Object.keys(result).length < 1) {
            updateBadWords();
        }
    });
}

//function getBadWords() {
//    chrome.storage.local.get(['BadWords'], function (result) {
//        var items = [];

//        $.each(result.BadWords.Content, function (idx, val) {
//            items.push(JSON.stringify(val.Value));
//        });

//        $('#BadWordsLastUpdated').append('<p>' + items.length + '</p>');

//        return items;
//    });
//}