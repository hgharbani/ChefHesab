"use strict";
function fullCalender() {
    var e = document.getElementById("external-events"),
        e =
            (new FullCalendar.Draggable(e, {
                itemSelector: ".external-event",
                eventData: function (e) {
                    return { title: e.innerText.trim() };
                },
            }),
                document.getElementById("calendar")),
        a = new FullCalendar.Calendar(e, {
            headerToolbar: {
                left: "prev,next today",
                center: "title",
                right: "dayGridMonth,timeGridWeek,timeGridDay",
            },
            isJalaali: true,
            locale: 'fa',
            lang: 'fa',
            datepickerLang: "fa",
            buttonText: {
                prev: 'قبلی',
                next: 'بعدی',
                today: 'امروز',
                month: 'ماه',
                week: 'هفته',
                day: 'روز',
                list: 'برنامه',
            },
            weekText: 'هف',
            allDayText: 'تمام روز',
            isJalaali: true,
            selectable: !0,
            selectable: !0,
            selectMirror: !0,
            select: function (e) {
                var t = prompt("Event Title:");
                t &&
                    a.addEvent({
                        title: t,
                        start: e.start,
                        end: e.end,
                        allDay: e.allDay,
                    }),
                    a.unselect();
            },
            editable: !0,
            editable: !0,
            droppable: !0,
            drop: function (e) {
                document.getElementById("drop-remove").checked &&
                    e.draggedEl.parentNode.removeChild(e.draggedEl);
            },
            initialDate: "2021-02-13",
            weekNumbers: !0,
            navLinks: !0,
            nowIndicator: !0,
           
            events: [
                { title: "رویداد یک روز کامل",
                 start: "2021-02-01" },
                {
                    title: "حادثه طولانی",
                    start: "2021-02-07",
                    end: "2021-02-10",
                    className: "bg-danger",
                },
                {
                    groupId: 999,
                    title: "تکرار رویداد",
                    start: "2021-02-09T16:00:00",
                },
                {
                    groupId: 999,
                    title: "تکرار رویداد",
                    start: "2021-02-16T16:00:00",
                },
                {
                    title: "کنفرانس",
                    start: "2021-02-11",
                    end: "2021-02-13",
                    className: "bg-danger",
                },
                { title: "ناهار ", start:" 2021-02-12T12: 00: 00 "},
                { title: "جلسه ", start:" 2021-04-12T14: 30: 00 "},
                { title: "ساعت مبارک ", start:" 2021-07-12T17: 30: 00 "},
                {
                    title: "شام",
                    start: "2021-02-12T20:00:00",
                    className: "bg-warning",
                },
                {
                    title: "جشن تولد",
                    start: "2021-02-13T07:00:00",
                    className: "bg-secondary",
                },
                {
                    title: "  کلیک کنید",
                    url: "http://google.com/",
                    start: "2021-02-28",
                },
            ],
        });
    a.render();
}
jQuery(window).on("load", function () {
    setTimeout(function () {
        fullCalender();
    }, 1e3);
});
