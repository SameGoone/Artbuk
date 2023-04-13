function onLikeClick(postId) {
    $.ajax({
        type: 'POST',
        url: '?handler=AddLike',
        data: { postId: postId },
        success: function (data) {
            alert(data);
        },
        error: function (error) {
            alert("Error: " + error);
        }
    })
}