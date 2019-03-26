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
        url: '/api/building/' + $(this).data('id'),
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

$(document).delegate('#edit_building_btn', 'click', function (e) {
    let modal = $('#edit_building_modal');
    let building = {
        address: modal.find('input[name = "Address"]')[0].value,
        id: modal.find('input[name = "Id"]')[0].value
    };
    console.log(building);
    $.ajax({
        type: 'PUT',
        contentType: 'application/json',
        url: '/api/building/',
        data: JSON.stringify(building),
        success: function () {
            getBuildings();
            modal.modal('hide');
        }
    });
});

$('#create_building_modal').on('show.bs.modal', function (e) {
    $(this).find('input[name = "Address"]')[0].value = '';
});

$(document).delegate('#create_building_btn', 'click', function (e) {
    let modal = $('#create_building_modal');
    let building = {
        address: modal.find('input[name = "Address"]')[0].value
    };
    console.log(building);
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/api/building/',
        data: JSON.stringify(building),
        success: function () {
            getBuildings();
            modal.modal('hide');
        }
    });
});