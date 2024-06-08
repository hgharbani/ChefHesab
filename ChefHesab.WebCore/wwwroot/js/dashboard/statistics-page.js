!(function (r) {
  var e = (function () {
    r(window).width();
    return {
      init: function () {},
      load: function () {
        var e, a, t;
        new ApexCharts(document.querySelector("#pieChart3"), {
          series: [90, 68, 85],
          chart: { type: "donut", height: 230 },
          dataLabels: { enabled: !1 },
          stroke: { width: 0 },
          colors: ["#F6AD2E", "var(--primary)", "#412EFF"],
          legend: { position: "bottom", show: !1 },
          responsive: [
            { breakpoint: 768, options: { chart: { height: 200 } } },
          ],
        }).render(),
          (e = {
            series: [
              { name: "سود", data: [15, 55, 90, 80, 25, 15, 70] },
              { name: "Revenue", data: [60, 65, 15, 35, 30, 5, 40] },
            ],
            chart: { type: "bar", height: 230, toolbar: { show: !1 } },
            plotOptions: {
              bar: {
                horizontal: !1,
                columnWidth: "25%",
                endingShape: "rounded",
              },
            },
            colors: ["#01111C", "var(--primary)"],
            dataLabels: { enabled: !1 },
            markers: { shape: "circle" },
            grid: { show: !0, strokeDashArray: 6 },
            legend: {
              show: !1,
              fontSize: "12px",
              labels: { colors: "#000000" },
              markers: {
                width: 18,
                height: 18,
                strokeWidth: 0,
                strokeColor: "#fff",
                fillColors: void 0,
                radius: 12,
              },
            },
            stroke: { show: !0, width: 1, colors: ["transparent"] },
            xaxis: {
              categories: ["شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه"],
              grid: { color: "rgba(233,236,255,0.5)", drawBorder: !0 },
              labels: {
                style: {
                  colors: "#787878",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 100,
                  cssClass: "apexcharts-xaxis-label",
                },
              },
              crosshairs: { show: !1 },
              axisTicks: { show: !1 },
              axisBorder: { show: !1 },
            },
            yaxis: {
              labels: {
                style: {
                  colors: "#787878",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 100,
                  cssClass: "apexcharts-xaxis-label",
                },
              },
            },
            fill: { opacity: 1 },
            tooltip: {
              y: {
                formatter: function (e) {
                  return "$ " + e + " thousands";
                },
              },
            },
          }),
          new ApexCharts(document.querySelector("#chartBar"), e).render(),
          (a = new ApexCharts(document.querySelector("#vacancyChart"), {
            series: [
              {
                name: "ارسال رزومه",
                data: [40, 60, 50, 65, 40, 65, 45, 56, 45, 60],
              },
              {
                name: "مصاحبه",
                data: [30, 27, 38, 35, 30, 35, 20, 35, 30, 40],
              },
              {
                name: "رد شده",
                data: [20, 25, 28, 20, 25, 28, 35, 25, 20, 30],
              },
            ],
            chart: { height: 300, type: "area", toolbar: { show: !1 } },
            colors: ["#35c556", "#3f4cfe", "#f34040"],
            dataLabels: { enabled: !1 },
            stroke: { curve: "smooth", width: 5 },
            legend: { show: !1 },
            grid: {
              show: !0,
              strokeDashArray: 6,
              borderColor: "var(--border)",
            },
            yaxis: {
              labels: {
                style: {
                  colors: "var(--text)",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 400,
                },
                formatter: function (e) {
                  return e;
                },
              },
            },
            xaxis: {
              categories: [
                    "فروردین",
                    "اردیبهشت",
                    "خرداد",
                    "تیر",
                    "مرداد",
                    "شهریور",
                    "مهر",
                    "آبان",
                    "آذر",
                    "دی",
              ],
              labels: {
                style: {
                  colors: "var(--text)",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 400,
                },
              },
              axisTicks: { show: !1 },
              axisBorder: { show: !1 },
            },
            fill: {
              type: "gradient",
              gradient: {
                colorStops: [
                  [
                    { offset: 0, color: "#35c556", opacity: 0.2 },
                    { offset: 50, color: "#35c556", opacity: 0 },
                    { offset: 100, color: "#35c556", opacity: 0 },
                  ],
                  [
                    { offset: 0, color: "#3f4cfe", opacity: 0.2 },
                    { offset: 50, color: "#3f4cfe", opacity: 0 },
                    { offset: 100, color: "#3f4cfe", opacity: 0 },
                  ],
                  [
                    { offset: 0, color: "#f34040", opacity: 0.2 },
                    { offset: 50, color: "#f34040", opacity: 0 },
                    { offset: 100, color: "#f34040", opacity: 0 },
                  ],
                ],
              },
            },
            tooltip: { x: { format: "dd/MM/yy HH:mm" } },
            responsive: [
              {
                breakpoint: 575,
                options: {
                  chart: { height: 200 },
                  stroke: { width: 3 },
                  yaxis: { labels: { style: { fontSize: "11px" } } },
                  xaxis: { labels: { style: { fontSize: "11px" } } },
                },
              },
            ],
          })).render(),
          r(".vacany-tabs .nav-link").on("click", function () {
            var e = [],
              t = [],
              o = [];
            switch (r(this).attr("data-series")) {
              case "Daily":
                (e = [60, 40, 50, 45, 60, 45, 35, 56, 45, 60]),
                  (t = [20, 35, 25, 35, 30, 20, 20, 35, 25, 40]),
                  (o = [10, 25, 30, 20, 25, 15, 35, 20, 20, 30]);
                break;
              case "Weekly":
                (e = [55, 35, 45, 35, 55, 45, 35, 60, 40, 55]),
                  (t = [35, 30, 40, 25, 44, 50, 20, 35, 30, 40]),
                  (o = [20, 20, 15, 10, 25, 28, 20, 25, 20, 30]);
                break;
              default:
                (e = [40, 60, 50, 65, 40, 65, 45, 56, 45, 60]),
                  (t = [30, 27, 38, 35, 30, 35, 20, 35, 30, 40]),
                  (o = [20, 25, 28, 20, 25, 28, 35, 25, 20, 30]);
            }
            a.updateSeries([
              { name: "ارسال رزومه", data: e },
              { name: "مصاحبه", data: t },
              { name: "رد شده", data: o },
            ]);
          }),
          (t = new ApexCharts(document.querySelector("#activity1"), {
            series: [
              {
                name: "سود",
                data: [50, 40, 55, 25, 45, 40, 35, 55, 50, 25, 42, 35, 50],
              },
            ],
            chart: { type: "bar", height: 350, toolbar: { show: !1 } },
            plotOptions: {
              bar: {
                horizontal: !1,
                columnWidth: "35%",
                endingShape: "rounded",
              },
            },
            dataLabels: { enabled: !1 },
            colors: ["var(--primary)"],
            stroke: { show: !0, width: 2, colors: ["transparent"] },
            xaxis: {
              categories: [
                "01",
                "02",
                "03",
                "04",
                "05",
                "06",
                "07",
                "08",
                "09",
                "10",
                "11",
                "12",
                "13",
              ],
              labels: {
                style: {
                  colors: "var(--text)",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 400,
                },
              },
              axisTicks: { show: !1 },
              axisBorder: { show: !1 },
            },
            yaxis: {
              labels: {
                style: {
                  colors: "var(--text)",
                  fontSize: "13px",
                   fontFamily: "iransans-light",
                  fontWeight: 400,
                },
                formatter: function (e) {
                  return e;
                },
              },
            },
            grid: {
              show: !0,
              strokeDashArray: 6,
              borderColor: "var(--border)",
            },
            fill: { opacity: 1 },
          })).render(),
          r(".chart-tab .nav-link").on("click", function () {
            var e = [];
            switch (r(this).attr("data-series")) {
              case "Daily":
                e = [60, 40, 50, 45, 60, 45, 35, 56, 45, 60, 36, 45, 60];
                break;
              case "Weekly":
                e = [55, 35, 45, 35, 55, 45, 35, 60, 40, 55, 45, 25, 45];
                break;
              default:
                e = [50, 40, 55, 25, 45, 40, 35, 55, 50, 25, 42, 35, 50];
            }
            t.updateSeries([{ name: "سود", data: e }]);
          });
      },
      resize: function () {},
    };
  })();
  jQuery(window).on("load", function () {
    setTimeout(function () {
      e.load();
    }, 1e3);
  });
})(jQuery);
