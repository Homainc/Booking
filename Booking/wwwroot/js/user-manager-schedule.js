$(document).ready(function () {
    const user_div = document.getElementById('user_schedule'),
        manager_div = document.getElementById('#manager_schedule');
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
            $('#user_schedule').empty();
            $('#user_event_tmpl').tmpl(user_reserves.$values).appendTo('#user_schedule');
        }
    });
}

function getManagerSchedule() {

}