﻿function onLikeClick(url, postId) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { postId: postId },
        success: function (data) {
            window.location.reload();
        },
        error: function (error) {
            console.error("Error: " + error);
        }
    })
}