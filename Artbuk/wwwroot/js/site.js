function onLikeClick(url, postId, likeCheckboxState) {
    let likeCheckbox = document.getElementById("like-add");

    $.ajax({
        type: 'POST',
        url: url,
        data: { postId: postId, likeCheckboxState: likeCheckbox.checked },
        success: function (likesInfoJson) {
            let likesInfo = JSON.parse(likesInfoJson)
            $("#likes-count").text(likesInfo.likesCount);
            likeCheckbox.checked = likesInfo.isLiked;
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