﻿let buildingUri = "/api/building";
let roomUri = "/api/room";
let reserveUri = "/api/reserve";
let userUri = "/api/user";
var roomSelected = false;
var dateSelected = false;
var scheduleDate;

function showDeviceMenu() {
    document.getElementById("deviceList").classList.toggle("search-show");
    document.getElementById("addDeviceBtn").classList.toggle("search-btn-index");
}
function filterDevices() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("searchDevice");
    filter = input.value.toUpperCase();
    div = document.getElementById("deviceList");
    a = div.getElementsByTagName("a");
    for (i = 0; i < a.length; i++) {
        txtValue = a[i].textContent || a[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}
function addToDeviceList(caller) {
    var list;
    list = document.getElementById("currentDevices");
    span = document.createElement("span");
    span.className = "badge badge-secondary";
    span.textContent = caller.textContent;
    span.dataset.id = caller.dataset.id;
    span.setAttribute("onclick", "removeFromDeviceList(this);");
    caller.setAttribute("hidden", "hidden");
    list.appendChild(span);
    if (list.childElementCount == 2) {
        list.firstElementChild.setAttribute("hidden", "hidden");
    }
}
function removeFromDeviceList(caller) {
    var list, devicesList;
    list = document.getElementById("currentDevices");
    devicesList = document.getElementById("deviceList");
    devices = devicesList.getElementsByTagName("a");
    for (i = 0; i < devices.length; i++) {
        if (devices[i].dataset.id == caller.dataset.id) {
            devices[i].removeAttribute("hidden", "hidden");
        }
    }
    if (list.childElementCount == 2) {
        list.firstElementChild.removeAttribute("hidden");
    }
    list.removeChild(caller);
}
function getBuildingRooms(list) {
    bid = list.options[list.selectedIndex].value;
    if (bid == 0) {
        clearList(document.getElementById("roomList"));
        return;
    }
    var request = new XMLHttpRequest();
    request.open('GET', buildingUri + "/" + bid);
    request.responseType = "json";
    request.send();
    request.onload = function () {
        refreshRoomList(request.response);
    }
}
function clearList(select) {
    for (i = select.options.length - 1; i >= 0; i--) {
        select.remove(i);
    }
    notchoose = document.createElement("option");
    notchoose.innerText = "Не выбрано";
    notchoose.value = 0;
    select.appendChild(notchoose);
    roomSelected = false;
}
function roomChoosed(select) {
    if (select.options[select.selectedIndex].value != 0)
        roomSelected = true;
    if (dateSelected)
        showSchedule();
}
function dateChoosed(fieldDate) {
    dateSelected = true;
    scheduleDate = new Date(fieldDate.value);
    if (roomSelected)
        showSchedule();
}
function refreshRoomList(building) {
    select = document.getElementById("roomList");
    clearList(select);
    building["rooms"]["$values"].forEach(function (room) {
        option = document.createElement("option");
        option.innerText = room["number"];
        option.value = room["id"];
        select.appendChild(option);
    });
}
function showSchedule() {
    document.getElementById("loadingAnimation").classList.remove("collapse");
    let roomList = document.getElementById("roomList");
    let rid = roomList.options[roomList.selectedIndex].value;
    var request = new XMLHttpRequest();
    request.open('GET', roomUri + '/' + rid);
    request.responseType = "json";
    request.send();
    let date = getDateStrFromDate(scheduleDate);
    request.onload = function () {
        result = parseWithRefs(request.response);
        populateScheduleWithRoom(result, date);
        request.open('GET', reserveUri + '/' + rid + '/' + date);
        request.send();
        request.onload = function () {
            result = parseWithRefs(request.response);
            populateScheduleWithTimeLine(result);
            let manSchedule = document.getElementById("managerSchedule");
            manSchedule.classList.remove("collapse");
            document.getElementById("loadingAnimation").classList.add("collapse");
        }
    }
}
function populateScheduleWithRoom(result, date) {
    let manSchedule = document.getElementById("managerSchedule");
    let block = manSchedule.getElementsByClassName("room-block")[0];
    block.getElementsByClassName("room-floor")[0].innerText = result["floor"];
    block.getElementsByClassName("room-number")[0].innerText = result["number"];
    block.getElementsByClassName("room-info")[0].innerText = result["info"];
    block.getElementsByClassName("schedule-date")[0].innerText = date;
}
function populateScheduleWithTimeLine(reserves) {
    let timeline = document.getElementById("timeline");
    timeline.innerHTML = '';
    reservesList = reserves["$values"];
    if (reservesList.length == 0) {
        addToTimeLine(timeline, "8:00", "22:00", "free");
        return;
    }
    var i = 0;
    currRDate = new Date(Date.parse(reservesList[i]["dateTime"]));
    lastRDate = new Date(currRDate);
    lastRDate.setHours(8);
    lastRDate.setMinutes(0);
    while (lastRDate.getHours() != 22) {
        if (lastRDate.getHours() < currRDate.getHours() ||
            (lastRDate.getHours() == currRDate.getHours() && lastRDate.getMinutes() < currRDate.getMinutes())) {
            lDate = getTimeStrFromDate(lastRDate);
            rDate = getTimeStrFromDate(currRDate);
            addToTimeLine(timeline, lDate, rDate, "free", null);
            lastRDate = new Date(currRDate);
        }
        else {
            lastRDate.setMinutes(currRDate.getMinutes() + reservesList[i]["hours"]*60);
            lDate = getTimeStrFromDate(currRDate);
            rDate = getTimeStrFromDate(lastRDate);
            user = reservesList[i]["user"]
            addToTimeLine(timeline, lDate, rDate, "busy", user);
            i++
            if (i < reservesList.length) {
                currRDate = new Date(Date.parse(reservesList[i]["dateTime"]));
            }
            else {
                currRDate.setHours(22);
                currRDate.setMinutes(0);
            }
        }
    }
}
function addToTimeLine(timeline, lDate, rDate, divClass, user) {
    var div = document.createElement("div");
    div.classList = "base-timeline " + divClass + "-timeline";
    div.innerHTML = lDate + "-" + rDate + (user != null ? "<p>" + user["name"] + " " + user["surname"] + "</p>" : '');
    if (divClass == "free") {
        div.dataset.toggle = "modal";
        div.dataset.target = "#bookingWindow";
        div.dataset.start = lDate;
        div.dataset.end = rDate;
    }
    timeline.appendChild(div);
}
function getTimeStrFromDate(d) {
    var h = ("0" + d.getHours()).slice(-2);
    var m = ("0" + d.getMinutes()).slice(-2);
    return h + ":" + m;
}
function getDateStrFromDate(d) {
    var day = ("0" + d.getDate()).slice(-2);
    var mon = ("0" + (d.getMonth()+1)).slice(-2);
    var year = d.getFullYear();
    return day + '.' + mon + '.' + year;
}
$("#bookingWindow").on("show.bs.modal", function (e) {
    let form = document.getElementById("reserveForm");
    let startTime = form.getElementsByClassName("time-start")[0];
    let endTime = form.getElementsByClassName("time-end")[0];
    startTime.value = e.relatedTarget.dataset.start;
    endTime.value = e.relatedTarget.dataset.end;
})
function addToTeamBtn() {
    let emailfield = $("#reserveForm").find($('[name = "Email"]'));
    if (emailfield.valid()) {
        addToTeam(emailfield.val());
        emailfield.val('');
    }
}
function addToTeam(email) {
    let list = document.getElementById("parcipiantsList");
    if (Array.from(list.getElementsByTagName("a"))
        .some(function (item) { return item.innerHTML == email; }))
        return;
    let a = document.createElement("a");
    a.classList = "list-group-item list-group-item-action";
    a.innerHTML = email;
    a.setAttribute("onclick", "removeFromTeam(this);");
    a.setAttribute("style", "cursor: pointer;");
    if (list.childElementCount == 1 && list.firstElementChild.innerHTML == "Нету участников")
        list.removeChild(list.firstElementChild);
    list.appendChild(a);
}
function removeFromTeam(caller) {
    let list = document.getElementById("parcipiantsList");
    list.removeChild(caller);
    if (list.childElementCount == 0) {
        let a = document.createElement("a");
        a.classList = "list-group-item list-group-item-action";
        a.innerHTML = "Нету участников";
        list.appendChild(a);
    }
}
function getParticipantEmails() {
    let list = document.getElementById("parcipiantsList");
    if (list.firstElementChild.innerHTML == "Нету участников")
        return [];
    let items = Array.from(list.getElementsByTagName("a"));
    var result = [];
    items.forEach(function (item) {
        result.push(item.innerHTML);
    });
    return result;
}
function saveReserve() {
    let rL = document.getElementById("roomList");
    let form = document.getElementById("reserveForm");
    let startTime = form.getElementsByClassName("time-start")[0].value;
    let endTime = form.getElementsByClassName("time-end")[0].value;
    var reserveAPI = {
        StartDate: getDateTimeStr(scheduleDate, startTime),
        EndDate: getDateTimeStr(scheduleDate, endTime),
        Participants: getParticipantEmails(),
        RoomId: rL.options[rL.selectedIndex].value,
        Devices: getDevicesArray()
    };
    console.log(reserveAPI);
    $.ajax({
        type: "POST",
        url: reserveUri,
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(reserveAPI),
        success: function (result) {
            showSchedule();
            $("#bookingWindow").modal('hide');
        },
        error: function (xhr, status, error) {
            alert("Not access!");
        },
    });
}
function getDateTimeStr(d,t) {
    var day = ("0" + d.getDate()).slice(-2);
    var mon = ("0" + (d.getMonth() + 1)).slice(-2);
    var year = d.getFullYear();
    return mon + '.' + day + '.' + year + ' ' + t + ':00';
}
function getDevicesArray() {
    let list = document.getElementById("currentDevices");
    let array = [];
    Array.from(list.getElementsByTagName("span")).forEach(function (item) {
        array.push(item.dataset.id);
    });
    return array;
}