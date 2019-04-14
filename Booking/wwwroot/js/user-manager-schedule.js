$(document).ready(function () {
    const user_div = document.getElementById('user_schedule'),
        manager_div = document.getElementById('manager_schedule');
    user_div && getUserSchedule();
    manager_div && getManagerSchedule();
});

function getUserSchedule() {
    $.ajax({
        type: 'GET',
        url: '/api/reserve/user',
        cache: false,
        success: function (user_reserves) {
            user_reserves = parseWithRefs(user_reserves);
            $.each(user_reserves.$values, function (key, value) {
                value.dateTime = new Date(Date.parse(value.dateTime));
            });
            let block = $('#user_schedule');
            block.empty();
            checkListEmpty(user_reserves.$values, block, 'Нет событий');
            $('#user_event_tmpl').tmpl(user_reserves.$values).appendTo(block);
        }
    });
}

function getManagerSchedule() {
    $.ajax({
        type: 'GET',
        url: '/api/reserve/manager',
        cache: false,
        success: function (manager_reserves) {
            manager_reserves = parseWithRefs(manager_reserves);
            $.each(manager_reserves.$values, function (key, value) {
                value.dateTime = new Date(Date.parse(value.dateTime));
            });
            let block = $('#manager_schedule');
            block.empty();
            checkListEmpty(manager_reserves.$values, block, 'Нет событий');
            $('#manager_event_tmpl').tmpl(manager_reserves.$values).appendTo(block);
        }
    });
}

function checkListEmpty(list, block, text) {
    if (list.length === 0) {
        let p = document.createElement('p');
        p.innerText = text;
        p.className = 'empty';
        $(p).appendTo(block);
    }
    else {
        let empty_p = $(block).find('empty');
        if (empty_p.length !== 0)
            $(empty_p[0]).remove();
    }
}

$('#request_decline_reserve').on('show.bs.modal', function (e) {
    document.getElementById('decline_reserve_btn').dataset.rid = e.relatedTarget.dataset.rid;
});

$('#decline_reserve_btn').on('click', function () {
    const rid = this.dataset.rid;
    $.ajax({
        type: 'PUT',
        url: `/api/reserve/decline/${rid}`,
        success: function () {
            getUserSchedule();
            $('#request_decline_reserve').modal('hide');
        }
    });
});

$('#request_cancel_reserve').on('show.bs.modal', function (e) {
    document.getElementById('cancel_reserve_btn').dataset.rid = e.relatedTarget.dataset.rid;
});

$('#cancel_reserve_btn').on('click', function () {
    const rid = this.dataset.rid;
    $.ajax({
        type: 'DELETE',
        url: `/api/reserve/${rid}`,
        success: function () {
            getManagerSchedule();
            $('#request_cancel_reserve').modal('hide');
        }
    });
});