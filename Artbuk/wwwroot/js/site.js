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