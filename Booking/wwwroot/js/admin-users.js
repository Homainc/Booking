$(document).ready(function () {
    getUsers();
});

function getUsers() {
    $.ajax({
        type: 'GET',
        url: '/api/user',
        cache: false,
        success: function (users) {
            users = parseWithRefs(users.$values);
            let block = $('#users_tbody');
            block.empty();
            $('#user_tmpl').tmpl(users).appendTo(block);
        }
    });
}

$('#request_delete_user').on('show.bs.modal', function (e) {
    $(this).find('#delete_user_btn')[0].dataset.id = $(e.relatedTarget).data('id');
});

$(document).delegate('#delete_user_btn', 'click', function (e) {
    $.ajax({
        type: 'DELETE',
        url: `/api/user/${this.dataset.id}`,
        success: function () {
            getUsers();
            $('#request_delete_user').modal('hide');
        }
    });
});

$('#edit_user_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Id"]')[0].value = e.relatedTarget.dataset.id;
    $(this).find('input[name = "Name"]')[0].value = e.relatedTarget.dataset.name;
    $(this).find('input[name = "Surname"]')[0].value = e.relatedTarget.dataset.surname;
    $(this).find('select')[0].value = e.relatedTarget.dataset.rname;
    $(this).find('input[name = "Email"]')[0].value = e.relatedTarget.dataset.email;
});

$(document).delegate('#edit_user_btn', 'click', function (e) {
    let modal = $('#edit_user_modal');
    let user = {
        id: modal.find('input[name = "Id"]')[0].value,
        name: modal.find('input[name = "Name"]')[0].value,
        surname: modal.find('input[name = "Surname"]')[0].value,
        email: modal.find('input[name = "Email"]')[0].value,
        roleName: modal.find('select')[0].value
    };
    $.ajax({
        type: 'PUT',
        contentType: 'application/json',
        url: '/api/user/',
        data: JSON.stringify(user),
        success: function () {
            getUsers();
            modal.modal('hide');
        }
    });
});