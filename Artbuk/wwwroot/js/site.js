function onLikeClick(url, postId) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { postId: postId },
        success: function (likesCount) {
            $("#likes-count").text(likesCount);
        },
        error: function (error) {
            console.error("Error: " + error);
        }
    })
}

function onSubscribeClick(url, subcribeToId) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { subcribeToId: subcribeToId },
        success: function () {
            //$(`#subscribe-${subcribeToId}`).hide();
            //$(`#unsubscribe-${subcribeToId}`).show();
            window.location.reload();
        },
        error: function (error) {
            console.error("Error: " + error);
        }
    })
}

function onUnsubscribeClick(url, unsubcribeToId) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { unsubcribeToId: unsubcribeToId },
        success: function () {
            //$(`#unsubscribe-${unsubcribeToId}`).hide();
            //$(`#subscribe-${unsubcribeToId}`).show();
            window.location.reload();
        },
        error: function (error) {
            console.error("Error: " + error);
        }
    })
}