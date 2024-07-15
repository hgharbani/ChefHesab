!(function (a) {
  var e = (function () {
    a(window).width();
    return {
      init: function () {},
      load: function () {
        var e, r, o;
        (e = {
          series: [
            { name: "Net Profit", data: [100, 300, 100, 400, 200, 400] },
          ],
          chart: {
            type: "area",
            height: 50,
            width: 100,
            toolbar: { show: !1 },
            zoom: { enabled: !1 },
            sparkline: { enabled: !0 },
          },
          colors: ["var(--primary)"],
          dataLabels: { enabled: !1 },
          legend: { show: !1 },
          stroke: {
            show: !0,
            width: 3,
            curve: "smooth",
            colors: ["var(--primary)"],
          },
          grid: {
            show: !1,
            borderColor: "#eee",
            padding: { top: 0, right: 0, bottom: 0, left: 0 },
          },
          states: {
            normal: { filter: { type: "none", value: 0 } },
            hover: { filter: { type: "none", value: 0 } },
            active: {
              allowMultipleDataPointsSelection: !1,
              filter: { type: "none", value: 0 },
            },
          },
          x: {
            categories: ["Jan", "feb", "Mar", "Apr", "May"],
            axisBorder: { show: !1 },
            axisTicks: { show: !1 },
            labels: { show: !1, style: { fontSize: "12px" } },
            crosshairs: {
              show: !1,
              position: "front",
              stroke: { width: 1, dashArray: 3 },
            },
            tooltip: {
              enabled: !0,
              formatter: void 0,
              offsetY: 0,
              style: { fontSize: "12px" },
            },
          },
          y: { show: !1 },
          fill: {
            type: "gradient",
            gradient: {
              colorStops: [
                { offset: 0, color: "var(--primary)", opacity: 1 },
                { offset: 0.3, color: "var(--primary)", opacity: 0.4 },
                { offset: 100, color: "var(--primary)", opacity: 0 },
              ],
            },
          },
          tooltip: {
            enabled: !1,
            style: { fontSize: "12px" },
            y: {
              formatter: function (e) {
                return "$" + e + " thousands";
              },
            },
          },
          responsive: [
            { breakpoint: 1400, options: { chart: { width: 70, height: 40 } } },
          ],
        }),
          new ApexCharts(document.querySelector("#NewCustomers"), e).render(),
          (e = {
            series: [
              { name: "Net Profit", data: [100, 300, 200, 400, 100, 400] },
            ],
            chart: {
              type: "area",
              height: 50,
              width: 100,
              toolbar: { show: !1 },
              zoom: { enabled: !1 },
              sparkline: { enabled: !0 },
            },
            colors: ["#0E8A74"],
            dataLabels: { enabled: !1 },
            legend: { show: !1 },
            stroke: {
              show: !0,
              width: 3,
              curve: "smooth",
              colors: ["#145650"],
            },
            grid: {
              show: !1,
              borderColor: "#eee",
              padding: { top: 0, right: 0, bottom: 0, left: 0 },
            },
            states: {
              normal: { filter: { type: "none", value: 0 } },
              hover: { filter: { type: "none", value: 0 } },
              active: {
                allowMultipleDataPointsSelection: !1,
                filter: { type: "none", value: 0 },
              },
            },
            x: {
              categories: ["Jan", "feb", "Mar", "Apr", "May"],
              axisBorder: { show: !1 },
              axisTicks: { show: !1 },
              labels: { show: !1, style: { fontSize: "12px" } },
              crosshairs: {
                show: !1,
                position: "front",
                stroke: { width: 1, dashArray: 3 },
              },
              tooltip: {
                enabled: !0,
                formatter: void 0,
                offsetY: 0,
                style: { fontSize: "12px" },
              },
            },
            y: { show: !1 },
            fill: {
              type: "gradient",
              gradient: {
                colorStops: [
                  { offset: 0, color: "#0E8A74", opacity: 1 },
                  { offset: 0.3, color: "#0E8A74", opacity: 0.4 },
                  { offset: 100, color: "#0E8A74", opacity: 0 },
                ],
              },
            },
            tooltip: {
              enabled: !1,
              style: { fontSize: "12px" },
              y: {
                formatter: function (e) {
                  return "$" + e + " thousands";
                },
              },
            },
            responsive: [
              {
                breakpoint: 1400,
                options: { chart: { width: 70, height: 40 } },
              },
            ],
          }),
          new ApexCharts(document.querySelector("#NewCustomers1"), e).render(),
          (e = {
            series: [
              { name: "Net Profit", data: [100, 200, 100, 300, 200, 400] },
            ],
            chart: {
              type: "area",
              height: 50,
              width: 100,
              toolbar: { show: !1 },
              zoom: { enabled: !1 },
              sparkline: { enabled: !0 },
            },
            colors: ["#0E8A74"],
            dataLabels: { enabled: !1 },
            legend: { show: !1 },
            stroke: {
              show: !0,
              width: 3,
              curve: "smooth",
              colors: ["#3385D6"],
            },
            grid: {
              show: !1,
              borderColor: "#eee",
              padding: { top: 0, right: 0, bottom: 0, left: 0 },
            },
            states: {
              normal: { filter: { type: "none", value: 0 } },
              hover: { filter: { type: "none", value: 0 } },
              active: {
                allowMultipleDataPointsSelection: !1,
                filter: { type: "none", value: 0 },
              },
            },
            x: {
              categories: ["Jan", "feb", "Mar", "Apr", "May"],
              axisBorder: { show: !1 },
              axisTicks: { show: !1 },
              labels: { show: !1, style: { fontSize: "12px" } },
              crosshairs: {
                show: !1,
                position: "front",
                stroke: { width: 1, dashArray: 3 },
              },
              tooltip: {
                enabled: !0,
                formatter: void 0,
                offsetY: 0,
                style: { fontSize: "12px" },
              },
            },
            y: { show: !1 },
            fill: {
              type: "gradient",
              gradient: {
                colorStops: [
                  { offset: 0, color: "#3385D6", opacity: 1 },
                  { offset: 0.3, color: "#3385D6", opacity: 0.4 },
                  { offset: 100, color: "#3385D6", opacity: 0 },
                ],
              },
            },
            tooltip: {
              enabled: !1,
              style: { fontSize: "12px" },
              y: {
                formatter: function (e) {
                  return "$" + e + " thousands";
                },
              },
            },
            responsive: [
              {
                breakpoint: 1400,
                options: { chart: { width: 70, height: 40 } },
              },
            ],
          }),
          new ApexCharts(document.querySelector("#NewCustomers2"), e).render(),
          (e = {
            series: [
              { name: "Net Profit", data: [100, 200, 100, 300, 200, 400] },
            ],
            chart: {
              type: "area",
              height: 50,
              width: 100,
              toolbar: { show: !1 },
              zoom: { enabled: !1 },
              sparkline: { enabled: !0 },
            },
            colors: ["#0E8A74"],
            dataLabels: { enabled: !1 },
            legend: { show: !1 },
            stroke: {
              show: !0,
              width: 3,
              curve: "smooth",
              colors: ["#B723AD"],
            },
            grid: {
              show: !1,
              borderColor: "#eee",
              padding: { top: 0, right: 0, bottom: 0, left: 0 },
            },
            states: {
              normal: { filter: { type: "none", value: 0 } },
              hover: { filter: { type: "none", value: 0 } },
              active: {
                allowMultipleDataPointsSelection: !1,
                filter: { type: "none", value: 0 },
              },
            },
            x: {
              categories: ["Jan", "feb", "Mar", "Apr", "May"],
              axisBorder: { show: !1 },
              axisTicks: { show: !1 },
              labels: { show: !1, style: { fontSize: "12px" } },
              crosshairs: {
                show: !1,
                position: "front",
                stroke: { width: 1, dashArray: 3 },
              },
              tooltip: {
                enabled: !0,
                formatter: void 0,
                offsetY: 0,
                style: { fontSize: "12px" },
              },
            },
            y: { show: !1 },
            fill: {
              type: "gradient",
              gradient: {
                colorStops: [
                  { offset: 0, color: "#B723AD", opacity: 1 },
                  { offset: 0.3, color: "#B723AD", opacity: 0.4 },
                  { offset: 100, color: "#B723AD", opacity: 0 },
                ],
              },
            },
            tooltip: {
              enabled: !1,
              style: { fontSize: "12px" },
              y: {
                formatter: function (e) {
                  return "$" + e + " thousands";
                },
              },
            },
            responsive: [
              {
                breakpoint: 1400,
                options: { chart: { width: 70, height: 40 } },
              },
            ],
          }),
          new ApexCharts(document.querySelector("#NewCustomers3"), e).render(),
          (r = new ApexCharts(document.querySelector("#vacancyChart"), {
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
          a(".vacany-tabs .nav-link").on("click", function () {
            var e = [],
              o = [],
              t = [];
            switch (a(this).attr("data-series")) {
              case "Daily":
                (e = [60, 40, 50, 45, 60, 45, 35, 56, 45, 60]),
                  (o = [20, 35, 25, 35, 30, 20, 20, 35, 25, 40]),
                  (t = [10, 25, 30, 20, 25, 15, 35, 20, 20, 30]);
                break;
              case "Weekly":
                (e = [55, 35, 45, 35, 55, 45, 35, 60, 40, 55]),
                  (o = [35, 30, 40, 25, 44, 50, 20, 35, 30, 40]),
                  (t = [20, 20, 15, 10, 25, 28, 20, 25, 20, 30]);
                break;
              default:
                (e = [40, 60, 50, 65, 40, 65, 45, 56, 45, 60]),
                  (o = [30, 27, 38, 35, 30, 35, 20, 35, 30, 40]),
                  (t = [20, 25, 28, 20, 25, 28, 35, 25, 20, 30]);
            }
            r.updateSeries([
              { name: "ارسال رزومه", data: e },
              { name: "مصاحبه", data: o },
              { name: "رد شده", data: t },
            ]);
          }),
          (o = new ApexCharts(document.querySelector("#activity1"), {
            series: [
              {
                name: "Net Profit",
                data: [50, 40, 55, 25, 45, 40, 35, 55, 50, 25, 42, 35, 50],
              },
            ],
            chart: { type: "bar", height: 280, toolbar: { show: !1 } },
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
          a(".chart-tab .nav-link").on("click", function () {
            var e = [];
            switch (a(this).attr("data-series")) {
              case "Daily":
                e = [60, 40, 50, 45, 60, 45, 35, 56, 45, 60, 36, 45, 60];
                break;
              case "Weekly":
                e = [55, 35, 45, 35, 55, 45, 35, 60, 40, 55, 45, 25, 45];
                break;
              default:
                e = [50, 40, 55, 25, 45, 40, 35, 55, 50, 25, 42, 35, 50];
            }
            o.updateSeries([{ name: "Net Profit", data: e }]);
          }),
          new ApexCharts(document.querySelector("#pieChart1"), {
            series: [90, 68, 85],
            chart: { type: "donut", height: 220 },
            dataLabels: { enabled: !1 },
            stroke: { width: 0 },
            colors: ["#1D92DF", "#4754CB", "#D55BC1"],
            legend: { position: "bottom", show: !1 },
            responsive: [
              { breakpoint: 1400, options: { chart: { height: 200 } } },
            ],
          }).render(),
          a("span.donut").peity("donut", { width: "100", height: "100" });
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
