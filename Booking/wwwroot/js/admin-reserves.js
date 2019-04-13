$(document).ready(function () {
    getReserves();
});

function getReserves() {
    let ue = $('#user_email').val() === '' ? 'null' : $('#user_email').val();
    let bid = $('#buildings_list option:selected').val();
    let rid = $('#rooms_list option:selected').val();
    let ed = $('#event_date').val() === '' ? 'null' : $('#event_date').val();
    $.ajax({
        type: 'GET',
        url: '/api/reserve/' + bid + '-' + rid + '/' + ue + '/' + ed,
        contentType: 'application/json',
        cache: false,
        success: function (reserves) {
            reserves = parseWithRefs(reserves);
            $.each(reserves.$values, function (key, value) {
                value.dateTime = new Date(Date.parse(value.dateTime));
            });
            $('#reserves_tbody').empty();
            $('#reserve_tmpl').tmpl(reserves.$values).appendTo("#reserves_tbody");
        }
    });
}

function getBuildingRooms(list) {
    let bid = list.options[list.selectedIndex].value;
    let roomList = $('#rooms_list');
    if (bid == 0) {
        roomList.empty()
            .append($(document.createElement('option')).val(0).text('Не выбрано'));
        return;
    }
    $.ajax({
        type: 'GET',
        url: '/api/building/' + bid,
        cache: false,
        success: function (building) {
            $.each(building.rooms.$values, function (k, v) {
                roomList.append($(document.createElement('option')).val(v.id).text(v.number));
            });
        }
    });
}

$('#request_delete_reserve').on('show.bs.modal', function (e) {
    $(this).find('#delete_reserve_btn')[0].dataset.id = $(e.relatedTarget).data('id');
});

$(document).delegate('#delete_reserve_btn', 'click', function (e) {
    $.ajax({
        type: 'DELETE',
        url: '/api/reserve/' + this.dataset.id,
        cache: false,
        success: function () {
            getReserves();
            $('#request_delete_reserve').modal('hide');
        }
    });
});