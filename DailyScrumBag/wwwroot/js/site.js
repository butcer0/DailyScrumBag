$(function () {
    $('#mainContent').on('click', '.pager a', function () {
        var url = $(this).attr('href');

        $('#mainContent').load(url);

        return false;
    });
});


/* comments.js */


$(function initializeCommentComponents() {

    $(document).on('click', '.show-comments', function (evt) {
        evt.stopPropagation();
        new Post(this).showComments();
        return false;
    });

    $(document).on('click', '.add-comment', function (evt) {
        evt.stopPropagation();
        new Post(this).showAddComment();
        return false;
    });

    $(document).on('submit', '.new-comment form', function (evt) {
        evt.stopPropagation();
        new Post(this).addComment();
        return false;
    });

    $(document).on('click', '.delete-comment', function (evt) {
        evt.stopPropagation();
        var deleteId = $(this).data('deleteid');
        new Post(this).removeComment(deleteId);
        return false;
    });

    //$(document).on('click', '.delete-comment', function (evt) {
    //    evt.stopPropagation();
    //    var deleteId = $(this).data('deleteid');
    //    new Post(this).removeComment(deleteId);
    //    return false;
    //});

    //$(document).on('click', '.edit-comment', function (evt) {
    //    evt.stopPropagation();
    //    new Post(this).showEditComments();
    //    return false;
    //});
});

/*  Post class as an object-oriented wrapper around DOM elements */
function Post(el) {
    //Depricated - Added Edit Comment
    var $el = $(el),
        postEl = $el.hasClass('blog-post') ? $el : $el.parents('.blog-post'),
        addCommentEl = postEl.find('.add-comment'),
        newCommentEl = postEl.find('.new-comment'),
        commentEl = newCommentEl.find('[name=Body]'),
        commentsContainer = postEl.find('.comments-container'),
        postKey = commentsContainer.data('post-key'),
        commentsEl = postEl.find('.comments'),
        showCommentsButton = postEl.find('.show-comments'),
        noCommentsEl = postEl.find('.no-comments');

    //var $el = $(el),
    //    postEl = $el.hasClass('blog-post') ? $el : $el.parents('.blog-post'),
    //    addCommentEl = postEl.find('.add-comment'),
    //    newCommentEl = postEl.find('.new-comment'),
    //    commentEl = newCommentEl.find('[name=Body]'),
    //    commentsContainer = postEl.find('.comments-container'),
    //    postKey = commentsContainer.data('post-key'),
    //    commentsEl = postEl.find('.comments'),
    //    showCommentsButton = postEl.find('.show-comments'),
    //    noCommentsEl = postEl.find('.no-comments'),

    //    //editCommentEl = commentsEl.find('edit-comment'),
    //    //updateCommentEl = commentsEl.find('update-comment'),
    //    //updatedCommentEl = updateCommentEl.find('[name=EditBody]')

    //;


    /*********  Web API Methods ***********/


    // RESTful Web API URL:  /api/posts/{postKey}/comments
    var webApiUrl = ['/api/posts', postKey, 'comments'].join('/');

    function addComment() {

        var comment = { Body: commentEl.val() };

        $.ajax({
            url: webApiUrl,
            type: 'POST',
            data: JSON.stringify(comment),
            contentType: 'application/json'
        }).then(renderComments);

    }

    function showComments() {

        $.ajax({
            url: webApiUrl,
            type: 'GET',
            contentType: 'application/json'
        }).then(renderComments);

    }

    function removeComment(deleteId) {
        var webApiDeleteUrl = [webApiUrl, deleteId].join('/');

        $.ajax({
            url: webApiDeleteUrl,
            type: 'Delete',
            contentType: 'application/json'
        }).then(clearThenRenderComments);
    }

    function deleteComment(deleteId) {
        var webApiDeleteUrl = [webApiUrl, deleteId].join('/');

        $.ajax({
            url: webApiDeleteUrl,
            type: 'Delete',
            contentType: 'application/json'
        }).then(clearThenRenderComments);
    }

    /****************************************/


    function showAddComment() {
        addCommentEl.addClass('hide');
        newCommentEl.removeClass('hide');
        commentEl.focus();
    }


    //function showEditComments() {

    //}
    
    return {
        addComment: addComment,
        renderComment: renderComments,
        showAddComment: showAddComment,
        showComments: showComments,
        removeComment: removeComment
    };


    //Depricated - Removed deleteComment
    //return {
    //    addComment: addComment,
    //    renderComment: renderComments,
    //    showAddComment: showAddComment,
    //    showComments: showComments,
    //    removeComment: removeComment,
    //    deleteComment: deleteComment
    //};


    ///*********  Private methods ****************/
    function createCommentElements(comments) {
        comments = [].concat(comments);

        if (!comments.length) {
            return $('<div class="no-comments">No comments</div>');
        }

        return comments.reduce(function (commentEls, comment) {
            var curId = '' + comment.id;
            var currentId = 'Id="' + curId + '"';
            //var curIdString = '<a href="#" class="delete-comment" Id="' + curId + '">Delete Comment</a>';
            //var curIdString = '<a href="#" class="delete-comment" onClick="deleteComment(' + comment.id + ')">Delete Comment</a>';
            //var curIdString = '<a class="delete-comment" asp-controller="Comments" asp-action="Delete" asp-route-id="'+comment.id+'" > Delete Comment</a > ';
            var curIdString = '<a href="#" class="delete-comment" data-deleteid="' + comment.id + '">Delete Comment</a>';

            //Depricated - Try ActionLink
            var el =
                $('<div class="comment">')
                    .append($('<p class="details">').append(comment.author || 'Anon').append(' at ' + new Date(comment.posted).toLocaleString()))
                    .append($('<p class="body">').append(comment.body))
                    .append($(curIdString));
        
                    //.append($('<a class="delete-comment" href="@Url.Action("Delete", "Comments", new { id =' + comment.id + '})">Delete Comment</a>'));
                    //.append($('<a class="delete-comment" asp-controller="Comments" asp-action="Delete" asp-route-id="'.append(comment.id).append('" > Delete Comment</a > ')));

            return commentEls.concat(el);
        }, []);
    }

    function clearThenRenderComments(comments) {
        $('.comments').html('');
        var commentEls = createCommentElements(comments);
        commentsEl.append(commentEls);
        commentsContainer.removeClass('hide');
        showCommentsButton.remove();
        noCommentsEl.remove();
        resetAddComment();
    }

    function renderComments(comments) {
        var commentEls = createCommentElements(comments);
        commentsEl.append(commentEls);
        commentsContainer.removeClass('hide');
        showCommentsButton.remove();
        noCommentsEl.remove();
        resetAddComment();
    }

    function resetAddComment() {
        addCommentEl.removeClass('hide');
        newCommentEl.addClass('hide');
        commentEl.val('');
    }
}