$(document).ready(function () {
    getBuildings();
});

function getBuildings() {
    $.ajax({
        type: 'GET',
        url: '/api/building/',
        contentType: 'application/json',
        cache: false,
        success: function (buildings) {
            buildings = parseWithRefs(buildings);
            checkEmpty(buildings.$values);
            $('#buildings_tbody').empty();
            $('#building_tmpl').tmpl(buildings.$values).appendTo("#buildings_tbody");
        }
    });
}

$('#request_delete_building').on('show.bs.modal', function (e) {
    $(this).find('#delete_building_btn')[0].dataset.id = $(e.relatedTarget).data('id');
});

$(document).delegate('#delete_building_btn', 'click', function (e) {
    $.ajax({
        type: 'DELETE',
        url: '/api/building/' + this.dataset.id,
        success: function () {
            getBuildings();
            $('#request_delete_building').modal('hide');
        }
    });
});

$('#edit_building_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Address"]')[0].value = e.relatedTarget.dataset.address;
    $(this).find('input[name = "Id"]')[0].value = e.relatedTarget.dataset.id;
});

$('#edit_building_modal').on('hide.bs.modal', function (e) {
    let eblock = $('#edit_building_errors');
    if (!eblock.hasClass('collapse'))
        eblock.addClass('collapse');
});

$(document).delegate('#edit_building_btn', 'click', function (e) {
    let modal = $('#edit_building_modal');
    let building = {
        address: modal.find('input[name = "Address"]')[0].value,
        id: modal.find('input[name = "Id"]')[0].value
    };
    $.ajax({
        type: 'PUT',
        contentType: 'application/json',
        url: '/api/building/',
        data: JSON.stringify(building),
        success: function () {
            getBuildings();
            modal.modal('hide');
        },
        error: function (xhr, status, error) {
            let eblock = $('#edit_booking_errors');
            eblock.text(xhr.responseText);
            eblock.removeClass('collapse');
        },
    });
});

$('#create_building_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Address"]')[0].value = '';
});

$('#create_building_modal').on('hide.bs.modal', function (e) {
    let eblock = $('#create_building_errors');
    if (!eblock.hasClass('collapse'))
        eblock.addClass('collapse');
});

$(document).delegate('#create_building_btn', 'click', function (e) {
    let modal = $('#create_building_modal');
    let building = {
        address: modal.find('input[name = "Address"]')[0].value
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/api/building/',
        data: JSON.stringify(building),
        success: function () {
            getBuildings();
            modal.modal('hide');
        },
        error: function (xhr, status, error) {
            let eblock = $('#create_building_errors');
            eblock.text(xhr.responseText);
            eblock.removeClass('collapse');
        },
    });
});