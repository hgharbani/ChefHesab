!(function (a) {
  "use strict";
  var e = (function () {
    a(window).width();
    return {
      init: function () {},
      load: function () {
        var a, e, t, r, o, n, d, l, i, g, s, b;
        0 < jQuery("#barChart_1").length &&
          (((a = document
            .getElementById("barChart_1")
            .getContext("2d")).height = 100),
          new Chart(a, {
            type: "bar",
            data: {
              defaultFontFamily: "iransans-light",
              labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
              datasets: [
                {
                  label: "اولین مجموعه داده من",
                  data: [65, 59, 80, 81, 56, 55, 40],
                  borderColor: "rgba(249, 58, 11, 1)",
                  borderWidth: "0",
                  barPercentage: 0.5,
                  backgroundColor: "rgba(249, 58, 11, 1)",
                },
              ],
            },
            options: {
              plugins: { legend: !1 },
              scales: { y: { ticks: { beginAtZero: !0 } } },
            },
          })),
          0 < jQuery("#barChart_2").length &&
            ((e = (a = document
              .getElementById("barChart_2")
              .getContext("2d")).createLinearGradient(
              0,
              0,
              0,
              250
            )).addColorStop(0, "rgba(249, 58, 11, 1)"),
            e.addColorStop(1, "rgba(249, 58, 11, 0.5)"),
            (a.height = 100),
            new Chart(a, {
              type: "bar",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [65, 59, 80, 81, 56, 55, 40],
                    borderColor: e,
                    borderWidth: "0",
                    barPercentage: 0.5,
                    backgroundColor: e,
                    hoverBackgroundColor: e,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: { y: { ticks: { beginAtZero: !0 } } },
              },
            })),
          0 < jQuery("#barChart_3").length &&
            ((t = (e = document
              .getElementById("barChart_3")
              .getContext("2d")).createLinearGradient(
              50,
              100,
              50,
              50
            )).addColorStop(0, "rgba(249, 58, 11, 1)"),
            t.addColorStop(1, "rgba(249, 58, 11, 0.5)"),
            (r = e.createLinearGradient(50, 100, 50, 50)).addColorStop(
              0,
              "rgba(98, 126, 234, 1)"
            ),
            r.addColorStop(1, "rgba(98, 126, 234, 1)"),
            (o = e.createLinearGradient(50, 100, 50, 50)).addColorStop(
              0,
              "rgba(238, 60, 60, 1)"
            ),
            o.addColorStop(1, "rgba(238, 60, 60, 1)"),
            (e.height = 100),
            new Chart(e, {
              type: "bar",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [  "شنبه",
                "یکشنبه",
                "دوشنبه",
                "سه شنبه",
                "چهارشنبه",
                "پنج شنبه",
                "جمعه",],
                datasets: [
                  {
                    label: "قرمز",
                    backgroundColor: t,
                    hoverBackgroundColor: t,
                    data: ["12", "12", "12", "12", "12", "12", "12"],
                  },
                  {
                    label: "سبز",
                    backgroundColor: r,
                    hoverBackgroundColor: r,
                    data: ["12", "12", "12", "12", "12", "12", "12"],
                  },
                  {
                    label: "آبی",
                    backgroundColor: o,
                    hoverBackgroundColor: o,
                    data: ["12", "12", "12", "12", "12", "12", "12"],
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                legend: { display: !1 },
                title: { display: !1 },
                tooltips: { mode: "index", intersect: !1 },
                responsive: !0,
                scales: { x: { stacked: !0 }, y: { stacked: !0 } },
              },
            })),
          0 < jQuery("#lineChart_1").length &&
            (((t = document
              .getElementById("lineChart_1")
              .getContext("2d")).height = 100),
            new Chart(t, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: "rgba(249, 58, 11, 1)",
                    borderWidth: "2",
                    backgroundColor: "transparent",
                    pointBackgroundColor: "rgba(249, 58, 11, 1)",
                    tension: 0.5,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#lineChart_2").length &&
            ((o = (r = document
              .getElementById("lineChart_2")
              .getContext("2d")).createLinearGradient(
              500,
              0,
              100,
              0
            )).addColorStop(0, "rgba(249, 58, 11, 1)"),
            o.addColorStop(1, "rgba(249, 58, 11, 0.5)"),
            (r.height = 100),
            new Chart(r, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: o,
                    borderWidth: "2",
                    backgroundColor: "transparent",
                    pointBackgroundColor: "rgba(249, 58, 11, 0.5)",
                    tension: 0.5,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#lineChart_3").length &&
            ((d = (n = document
              .getElementById("lineChart_3")
              .getContext("2d")).createLinearGradient(
              500,
              0,
              100,
              0
            )).addColorStop(0, "rgba(249, 58, 11, 1)"),
            d.addColorStop(1, "rgba(249, 58, 11, 0.5)"),
            (l = n.createLinearGradient(500, 0, 100, 0)).addColorStop(
              0,
              "rgba(255, 92, 0, 1)"
            ),
            l.addColorStop(1, "rgba(255, 92, 0, 1)"),
            (n.height = 100),
            new Chart(n, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: d,
                    borderWidth: "2",
                    backgroundColor: "transparent",
                    pointBackgroundColor: "rgba(249, 58, 11, 0.5)",
                    tension: 0.5,
                  },
                  {
                    label: "اولین مجموعه داده من",
                    data: [5, 20, 15, 41, 35, 65, 80],
                    borderColor: l,
                    borderWidth: "2",
                    backgroundColor: "transparent",
                    pointBackgroundColor: "rgba(254, 176, 25, 1)",
                    tension: 0.5,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#lineChart_3Kk").length &&
            ((n = document.getElementById("lineChart_3Kk").getContext("2d")),
            (Chart.controllers.line = Chart.controllers.line.extend({
              draw: function () {
                draw.apply(this, arguments);
                let a = this.chart.chart.ctx,
                  e = a.stroke;
                a.stroke = function () {
                  a.save(),
                    (a.shadowColor = "rgba(0, 0, 0, 0)"),
                    (a.shadowBlur = 10),
                    (a.shadowOffsetX = 0),
                    (a.shadowOffsetY = 10),
                    e.apply(this, arguments),
                    a.restore();
                };
              },
            })),
            (n.height = 100),
            new Chart(n, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [90, 60, 80, 50, 60, 55, 80],
                    borderColor: "rgba(58,122,254,1)",
                    borderWidth: "3",
                    backgroundColor: "rgba(0,0,0,0)",
                    pointBackgroundColor: "rgba(0, 0, 0, 0)",
                    tension: 0.5,
                    fill: !0,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                elements: { point: { radius: 0 } },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                    borderWidth: 3,
                    display: !1,
                    lineTension: 0.4,
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#areaChart_1").length &&
            (((d = document
              .getElementById("areaChart_1")
              .getContext("2d")).height = 100),
            new Chart(d, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: "rgba(0, 0, 1128, .3)",
                    borderWidth: "1",
                    backgroundColor: "rgba(249, 58, 11, .5)",
                    pointBackgroundColor: "rgba(0, 0, 1128, .3)",
                    tension: 0.5,
                    fill: !0,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#areaChart_2").length &&
            ((i = (l = document
              .getElementById("areaChart_2")
              .getContext("2d")).createLinearGradient(
              0,
              1,
              0,
              500
            )).addColorStop(0, "rgba(238, 60, 60, 0.2)"),
            i.addColorStop(1, "rgba(238, 60, 60, 0)"),
            (l.height = 100),
            new Chart(l, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: "#ff2625",
                    borderWidth: "4",
                    backgroundColor: i,
                    tension: 0.5,
                    fill: !0,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 5,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#areaChart_3").length &&
            (((i = document
              .getElementById("areaChart_3")
              .getContext("2d")).height = 100),
            new Chart(i, {
              type: "line",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [ "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر"],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [25, 20, 60, 41, 66, 45, 80],
                    borderColor: "rgb(249, 58, 11)",
                    borderWidth: "1",
                    backgroundColor: "rgba(249, 58, 11, .5)",
                    tension: 0.5,
                    fill: !0,
                  },
                  {
                    label: "اولین مجموعه داده من",
                    data: [5, 25, 20, 41, 36, 75, 70],
                    borderColor: "rgb(255, 92, 0)",
                    borderWidth: "1",
                    backgroundColor: "rgba(255, 92, 0, .5)",
                    tension: 0.5,
                    fill: !0,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                scales: {
                  y: {
                    ticks: {
                      beginAtZero: !0,
                      max: 100,
                      min: 0,
                      stepSize: 20,
                      padding: 10,
                    },
                  },
                  x: { ticks: { padding: 5 } },
                },
              },
            })),
          0 < jQuery("#radar_chart").length &&
            ((b = (g = document
              .getElementById("radar_chart")
              .getContext("2d")).createLinearGradient(
              500,
              0,
              100,
              0
            )).addColorStop(0, "rgba(54, 185, 216, .5)"),
            b.addColorStop(1, "rgba(75, 255, 162, .5)"),
            (s = g.createLinearGradient(500, 0, 100, 0)).addColorStop(
              0,
              "rgba(68, 0, 235, .5"
            ),
            s.addColorStop(1, "rgba(68, 236, 245, .5"),
            new Chart(g, {
              type: "radar",
              data: {
                defaultFontFamily: "iransans-light",
                labels: [
                  ["غذا خوردن", "شام"],
                  ["نوشیدن", "اب"],
                  "خواب",
                  ["طراحی", "گرافیک"],
                  "برنامه نویسی",
                  "دوچرخه سواري",
                  "دویدن",
                ],
                datasets: [
                  {
                    label: "اولین مجموعه داده من",
                    data: [65, 59, 66, 45, 56, 55, 40],
                    borderColor: "#f21780",
                    borderWidth: "1",
                    backgroundColor: s,
                  },
                  {
                    label: "مجموعه داده دوم من",
                    data: [28, 12, 40, 19, 63, 27, 87],
                    borderColor: "#f21780",
                    borderWidth: "1",
                    backgroundColor: b,
                  },
                ],
              },
              options: {
                plugins: { legend: !1 },
                maintainAspectRatio: !1,
                scale: { ticks: { beginAtZero: !0 } },
              },
            })),
          0 < jQuery("#pie_chart").length &&
            ((g = document.getElementById("pie_chart").getContext("2d")),
            new Chart(g, {
              type: "pie",
              data: {
                defaultFontFamily: "iransans-light",
                datasets: [
                  {
                    data: [45, 25, 20, 10],
                    borderWidth: 0,
                    backgroundColor: [
                      "rgba(249, 58, 11, .9)",
                      "rgba(249, 58, 11, .7)",
                      "rgba(249, 58, 11, .5)",
                      "rgba(0,0,0,0.07)",
                    ],
                    hoverBackgroundColor: [
                      "rgba(249, 58, 11, .9)",
                      "rgba(249, 58, 11, .7)",
                      "rgba(249, 58, 11, .5)",
                      "rgba(0,0,0,0.07)",
                    ],
                  },
                ],
                labels: ["یکی", "دو", "سه", "چهار"],
              },
              options: {
                plugins: { legend: !1 },
                legend: !1,
                maintainAspectRatio: !1,
              },
            })),
          0 < jQuery("#doughnut_chart").length &&
            ((s = document.getElementById("doughnut_chart").getContext("2d")),
            new Chart(s, {
              type: "doughnut",
              data: {
                weight: 5,
                defaultFontFamily: "iransans-light",
                datasets: [
                  {
                    data: [45, 25, 20],
                    borderWidth: 3,
                    borderColor: "rgba(255,255,255,1)",
                    backgroundColor: [
                      "rgba(249, 58, 11, 1)",
                      "rgba(98, 126, 234, 1)",
                      "rgba(238, 60, 60, 1)",
                    ],
                    hoverBackgroundColor: [
                      "rgba(249, 58, 11, 0.9)",
                      "rgba(98, 126, 234, .9)",
                      "rgba(238, 60, 60, .9)",
                    ],
                  },
                ],
              },
              options: {
                weight: 1,
                cutoutPercentage: 70,
                responsive: !0,
                maintainAspectRatio: !1,
              },
            })),
          0 < jQuery("#polar_chart").length &&
            ((b = document.getElementById("polar_chart").getContext("2d")),
            new Chart(b, {
              type: "polarArea",
              data: {
                defaultFontFamily: "iransans-light",
                datasets: [
                  {
                    data: [15, 18, 9, 6, 19],
                    borderWidth: 0,
                    backgroundColor: [
                      "rgba(249, 58, 11, 1)",
                      "rgba(98, 126, 234, 1)",
                      "rgba(238, 60, 60, 1)",
                      "rgba(54, 147, 255, 1)",
                      "rgba(255, 92, 0, 1)",
                    ],
                  },
                ],
              },
              options: { responsive: !0, maintainAspectRatio: !1 },
            }));
      },
      resize: function () {},
    };
  })();
  jQuery(document).ready(function () {}),
    jQuery(window).on("load", function () {
      e.load();
    }),
    jQuery(window).on("resize", function () {
      setTimeout(function () {
        e.resize();
      }, 1e3);
    });
})(jQuery);
