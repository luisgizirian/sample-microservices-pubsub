const postContact = (model) => {
    var defer = $.Deferred();
    $.ajax({
        type: 'POST',
        url: '/actionshandler/postcontact',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(model),
        success: function (data) {
            defer.resolve(data);
        }
    });
    return defer.promise();
}

export {
    postContact
};