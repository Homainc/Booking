$(document).ready(function () {
    getDevices();
});

function getDevices() {
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/api/device/',
        contentType: 'application/json',
        success: function (devices) {
            devices = parseWithRefs(devices);
            $('#devices_tbody').empty();
            $('#device_tmpl').tmpl(devices.$values).appendTo('#devices_tbody');
        }
    });
}

$('#request_delete_device').on('show.bs.modal', function (e) {
    $(this).find('#delete_device_btn')[0].dataset.id = $(e.relatedTarget).data('id');
});

$(document).delegate('#delete_device_btn', 'click', function (e) {
    $.ajax({
        type: 'DELETE',
        url: '/api/device/' + $(this).data('id'),
        success: function () {
            getDevices();
            $('#request_delete_device').modal('hide');
        }
    });
});

$('#create_device_modal').on('show.bs.modal', function (e) {
    $(this).find('form')[0].reset();
});

$(document).delegate('#create_device_btn', 'click', function (e) {
    let modal = $('#create_device_modal');
    let device = {
        name: modal.find('input[name = "Name"]')[0].value
    }
    $.ajax({
        type: 'POST',
        url: '/api/device/',
        contentType: 'application/json',
        data: JSON.stringify(device),
        success: function () {
            getDevices();
            modal.modal('hide');
        }
    });
});

$('#edit_device_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Id"]')[0].value = e.relatedTarget.dataset.id;
    $(this).find('input[name = "Name"]')[0].value = e.relatedTarget.dataset.name;
});

$(document).delegate('#edit_device_btn', 'click', function (e) {
    let modal = $('#edit_device_modal');
    let device = {
        id: modal.find('input[name = "Id"]')[0].value,
        name: modal.find('input[name = "Name"]')[0].value
    }
    $.ajax({
        type: 'PUT',
        url: '/api/device/',
        contentType: 'application/json',
        data: JSON.stringify(device),
        success: function () {
            getDevices();
            modal.modal('hide');
        }
    });
});