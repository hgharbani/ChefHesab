!(function (e) {
    "use strict";
    e(".input-daterange-datepicker").daterangepicker({
        buttonClasses: ["btn", "btn-sm"],
        applyClass: "btn-danger",
        cancelClass: "btn-inverse",
        "locale": {
            "separator": " - ",
            "applyLabel": "ثبت",
            "cancelLabel": "لغو",
            "customRangeLabel": "بازه دلخواه",
            "daysOfWeek": [
                "ی", "د", "س", "چ", "پ", "ج", "ش"
            ],
            "monthNames": [
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
            ],
            "firstDay": 1
        }
    }),
        e(".input-daterange-timepicker").daterangepicker({
            timePicker: !0,
            format: "MM/DD/YYYY h:mm A",
            timePickerIncrement: 30,
            timePicker12Hour: !0,
            timePickerSeconds: !1,
            buttonClasses: ["btn", "btn-sm"],
            applyClass: "btn-danger",
            cancelClass: "btn-inverse",
            "locale": {
                "separator": " - ",
                "applyLabel": "ثبت",
                "cancelLabel": "لغو",
                "customRangeLabel": "بازه دلخواه",
                "daysOfWeek": [
                    "ی", "د", "س", "چ", "پ", "ج", "ش"
                ],
                "monthNames": [
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
                ],
                "firstDay": 1
            }
        }),
        e(".input-limit-datepicker").daterangepicker({
            format: "MM/DD/YYYY",
            minDate: "06/01/1402",
            maxDate: "06/30/1402",
            buttonClasses: ["btn", "btn-sm"],
            applyClass: "btn-danger",
            cancelClass: "btn-inverse",
            dateLimit: { days: 6 },
            "locale": {
                "separator": " - ",
                "applyLabel": "ثبت",
                "cancelLabel": "لغو",
                "customRangeLabel": "بازه دلخواه",
                "daysOfWeek": [
                    "ی", "د", "س", "چ", "پ", "ج", "ش"
                ],
                "monthNames": [
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
                ],
                "firstDay": 1
            }
        });
})(jQuery);
