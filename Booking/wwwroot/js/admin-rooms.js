$(document).ready(function () {
    getRooms();
});

function getRooms() {
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/api/room/',
        contentType: 'application/json',
        success: function (rooms) {
            rooms = parseWithRefs(rooms);
            $('#rooms_tbody').empty();
            $('#room_tmpl').tmpl(rooms.$values).appendTo('#rooms_tbody');
        }
    });
}

$('#create_room_modal').on('show.bs.modal', function (e) {
    $(this).find('form')[0].reset();
});

$('#edit_room_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Id"]')[0].value = e.relatedTarget.dataset.id;
    $(this).find('input[name = "Number"]')[0].value = e.relatedTarget.dataset.number;
    $(this).find('input[name = "Floor"]')[0].value = e.relatedTarget.dataset.floor;
    $(this).find('select')[0].value = e.relatedTarget.dataset.buildingid;
    $(this).find('textarea[name = "Info"]')[0].value = e.relatedTarget.dataset.info;
});

$('#request_delete_room').on('show.bs.modal', function (e) {
    $(this).find('#delete_room_btn')[0].dataset.id = e.relatedTarget.dataset.id;
});

$(document).delegate('#delete_room_btn', 'click', function (e) {
    $.ajax({
        type: 'DELETE',
        url: '/api/room/' + this.dataset.id,
        contentType: 'application/json',
        success: function () {
            $("#request_delete_room").modal('hide');
            getRooms();
        }
    });
});

$(document).delegate('#create_room_btn', 'click', function (e) {
    let modal = $('#create_room_modal');
    let room = {
        number: modal.find('input[name = "Number"]')[0].value,
        floor: modal.find('input[name = "Floor"]')[0].value,
        info: modal.find('textarea[name = "Info"]')[0].value,
        buildingId: modal.find('select')[0].value
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/api/room/',
        data: JSON.stringify(room),
        success: function () {
            getRooms();
            modal.modal('hide');
        }
    });
});

$(document).delegate('#edit_room_btn', 'click', function (e) {
    let modal = $('#edit_room_modal');
    let room = {
        id: modal.find('input[name = "Id"]')[0].value,
        number: modal.find('input[name = "Number"]')[0].value,
        floor: modal.find('input[name = "Floor"]')[0].value,
        info: modal.find('textarea[name = "Info"]')[0].value,
        buildingId: modal.find('select')[0].value
    };
    $.ajax({
        type: 'PUT',
        contentType: 'application/json',
        url: '/api/room/',
        data: JSON.stringify(room),
        success: function () {
            getRooms();
            modal.modal('hide');
        }
    });
});