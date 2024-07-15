"use strict"

var themeOptionArr = {
    typography: '',
    version: '',
    layout: '',
    primary: '',
    headerBg: '',
    navheaderBg: '',
    sidebarBg: '',
    sidebarStyle: '',
    sidebarPosition: '',
    headerPosition: '',
    containerLayout: '',
    direction: 'rtl',
};


(function ($) {

    "use strict"

    var direction = getUrlParams('dir');
    var theme = getUrlParams('theme');

    /* dlab Theme Demo Settings  */

    /* dlab Theme Demo Settings  */

    var dlabThemeSet0 = { /* Default Theme */
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_1",
        sidebarBg: "color_1",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };

    var dlabThemeSet1 = {
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_1",
        sidebarBg: "color_1",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };

    var dlabThemeSet2 = {
        typography: "poppins",
        version: "dark",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_1",
        sidebarBg: "color_1",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };


    var dlabThemeSet3 = {
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_3",
        sidebarBg: "color_3",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };

    var dlabThemeSet4 = {
        typography: "poppins",
        version: "dark",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_8",
        sidebarBg: "color_8",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };

    var dlabThemeSet5 = {
        typography: "poppins",
        version: "light",
        layout: "horizontal",
        primary: "color_5",
        headerBg: "color_5",
        navheaderBg: "color_5",
        sidebarBg: "color_1",
        sidebarStyle: "full",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };
    var dlabThemeSet6 = {
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_11",
        headerBg: "color_1",
        navheaderBg: "color_11",
        sidebarBg: "color_11",
        sidebarStyle: "compact",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };
    var dlabThemeSet7 = {
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_1",
        headerBg: "color_1",
        navheaderBg: "color_13",
        sidebarBg: "color_13",
        sidebarStyle: "mini",
        sidebarPosition: "fixed",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };
    var dlabThemeSet8 = {
        typography: "poppins",
        version: "light",
        layout: "vertical",
        primary: "color_14",
        headerBg: "color_1",
        navheaderBg: "color_14",
        sidebarBg: "color_14",
        sidebarStyle: "modern",
        sidebarPosition: "static",
        headerPosition: "fixed",
        containerLayout: "full",
        direction: 'rtl',
    };


    function themeChange(theme, direction) {
        var themeSettings = {};
        themeSettings = eval('dlabThemeSet' + theme);
        themeSettings.direction = direction;
        dlabSettingsOptions = themeSettings; /* For Screen Resize */
        new dlabSettings(themeSettings);

        setThemeInCookie(themeSettings);
    }

    function setThemeInCookie(themeSettings) {
        //console.log(themeSettings);
        jQuery.each(themeSettings, function (optionKey, optionValue) {
            setCookie(optionKey, optionValue);
        });
    }

    function setThemeLogo() {
        var logo = getCookie('logo_src');

        var logo2 = getCookie('logo_src2');

        if (logo != '') {
            jQuery('.nav-header .logo-abbr').attr("src", logo);
        }

        if (logo2 != '') {
            jQuery('.nav-header .logo-compact, .nav-header .brand-title').attr("src", logo2);
        }
    }

    /*  set switcher option start  */
    function getElementAttrs(el) {
        return [].slice.call(el.attributes).map((attr) => {
            return {
                name: attr.name,
                value: attr.value
            }
        });
    }

    function handleSetThemeOption(item, index, arr) {

        var attrName = item.name.replace('data-', '').replace('-', '_');

        if (attrName === "sidebarbg" || attrName === "primary" || attrName === "headerbg" || attrName === "nav_headerbg") {
            if (item.value === "color_1") {
                return false;
            }
            var attrNameColor = attrName.replace("bg", "")
            document.getElementById(attrNameColor + "_" + item.value).checked = true;
        } else if (attrName === "navigationbarimg") {
            document.getElementById("sidebar_img_" + item.value.split('sidebar-img/index.html')[1].split('.')[0]).checked = true;
        } else if (attrName === "sidebartext") {
            if ($("#sidebar_text_" + item.value).length > 0) {
                document.getElementById("sidebar_text_" + item.value).checked = true;
            }
        } else if (attrName === "direction" || attrName === "nav_headerbg" || attrName === "headerbg") {
            if ($("#theme_direction").length > 0) {
                document.getElementById("theme_direction").value = item.value;
            }
        } else if (attrName === "layout") {
            if (item.value === "vertical") { return false }
            document.getElementById("theme_layout").value = item.value;
        }
        else if (attrName === "container") {
            if (item.value === "wide") { return false }
            document.getElementById("container_layout").value = item.value;
        }

    }
    /* / set switcher option end / */

    function setThemeOptionOnPage() {
        if (getCookie('version') != '') {
            jQuery.each(themeOptionArr, function (optionKey, optionValue) {
                var optionData = getCookie(optionKey);
                themeOptionArr[optionKey] = (optionData != '') ? optionData : dlabSettingsOptions[optionKey];
            });
            //console.log(themeOptionArr);
            dlabSettingsOptions = themeOptionArr;
            new dlabSettings(dlabSettingsOptions);

            setThemeLogo();
        }
    }

    jQuery(document).on('click', '.dlab_theme_demo', function () {
        setTimeout(() => {
            var allAttrs = getElementAttrs(document.querySelector('body'));
            allAttrs.forEach(handleSetThemeOption);
        }, 1500);

        var demoTheme = jQuery(this).data('theme');
        themeChange(demoTheme, 'rtl');
        $('.dlab-demo-panel').removeClass('show');

    });


    jQuery(document).on('click', '.dlab_theme_demo_rtl', function () {
        var demoTheme = jQuery(this).data('theme');
        themeChange(demoTheme, 'rtl');
        $('.dlab-demo-panel').removeClass('show');

    });


    jQuery(window).on('load', function () {
        direction = (direction != undefined) ? direction : 'rtl';

        if (getCookie('direction') == 'rtl') {

        }

        if (theme != undefined) {
            if (theme == 'rtl') {
                themeChange(0, 'rtl');

            } else {
                themeChange(theme, direction);
            }
        }
        else if (direction != undefined) {
            if (getCookie('version') == '') {
                themeChange(0, direction);
            }
        }

        setTimeout(() => {
            var allAttrs = getElementAttrs(document.querySelector('body'));
            allAttrs.forEach(handleSetThemeOption);
        }, 1500);

        /* Set Theme On Page From Cookie */
        setThemeOptionOnPage();
    });
    jQuery(window).on('resize', function () {
        setThemeOptionOnPage();
    });


})(jQuery); "use strict"; var themeOptionArr = { typography: "", version: "", layout: "", primary: "", headerBg: "", navheaderBg: "", sidebarBg: "", sidebarStyle: "", sidebarPosition: "", headerPosition: "", containerLayout: "", direction: "" }; !function ($) {
    var direction = getUrlParams("dir"), theme = getUrlParams("theme"), dlabThemeSet0 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_1", sidebarBg: "color_1", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet1 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_1", sidebarBg: "color_1", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet2 = { typography: "poppins", version: "dark", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_1", sidebarBg: "color_1", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet3 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_3", sidebarBg: "color_3", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet4 = { typography: "poppins", version: "dark", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_8", sidebarBg: "color_8", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet5 = { typography: "poppins", version: "light", layout: "horizontal", primary: "color_5", headerBg: "color_5", navheaderBg: "color_5", sidebarBg: "color_1", sidebarStyle: "full", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet6 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_11", headerBg: "color_1", navheaderBg: "color_11", sidebarBg: "color_11", sidebarStyle: "compact", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet7 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_1", headerBg: "color_1", navheaderBg: "color_13", sidebarBg: "color_13", sidebarStyle: "mini", sidebarPosition: "fixed", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }, dlabThemeSet8 = { typography: "poppins", version: "light", layout: "vertical", primary: "color_14", headerBg: "color_1", navheaderBg: "color_14", sidebarBg: "color_14", sidebarStyle: "modern", sidebarPosition: "static", headerPosition: "fixed", containerLayout: "full", direction: "rtl" }; function themeChange(theme, direction) { var themeSettings = {}, themeSettings = eval("dlabThemeSet" + theme); themeSettings.direction = direction, dlabSettingsOptions = themeSettings, new dlabSettings(themeSettings), setThemeInCookie(themeSettings) } function setThemeInCookie(e) { jQuery.each(e, function (e, t) { setCookie(e, t) }) } function setThemeLogo() { var e = getCookie("logo_src"), t = getCookie("logo_src2"); "" != e && jQuery(".nav-header .logo-abbr").attr("src", e), "" != t && jQuery(".nav-header .logo-compact, .nav-header .brand-title").attr("src", t) } function getElementAttrs(e) { return [].slice.call(e.attributes).map(e => ({ name: e.name, value: e.value })) } function handleSetThemeOption(e, t, o) {
        var r = e.name.replace("data-", "").replace("-", "_"); if ("sidebarbg" === r || "primary" === r || "headerbg" === r || "nav_headerbg" === r) { if ("color_1" === e.value) return !1; var i = r.replace("bg", ""); document.getElementById(i + "_" + e.value).checked = !0 } else if ("navigationbarimg" === r) document.getElementById("sidebar_img_" + e.value.split("sidebar-img/index.html")[1].split(".")[0]).checked = !0; else if ("sidebartext" === r) 0 < $("#sidebar_text_" + e.value).length && (document.getElementById("sidebar_text_" + e.value).checked = !0); else if ("direction" === r || "nav_headerbg" === r || "headerbg" === r) 0 < $("#theme_direction").length && (document.getElementById("theme_direction").value = e.value);
        else if ("layout" === r) { if ("vertical" === e.value) return !1; document.getElementById("theme_layout").value = e.value } else if ("container" === r) { if ("wide" === e.value) return !1; document.getElementById("container_layout").value = e.value }
    }
    function setThemeOptionOnPage() { "" != getCookie("version") && (jQuery.each(themeOptionArr, function (e, t) { var o = getCookie(e); themeOptionArr[e] = "" != o ? o : dlabSettingsOptions[e] }), dlabSettingsOptions = themeOptionArr, new dlabSettings(dlabSettingsOptions), setThemeLogo()) } jQuery(document).on("click", ".dlab_theme_demo", function () { setTimeout(() => { getElementAttrs(document.querySelector("body")).forEach(handleSetThemeOption) }, 1500), themeChange(jQuery(this).data("theme"), "rtl"), $(".dlab-demo-panel").removeClass("show"), jQuery(".main-css").attr("href", "https://jobick.dexignlab.com/xhtml/page-error-404.html") }), jQuery(document).on("click", ".dlab_theme_demo_rtl", function () { themeChange(jQuery(this).data("theme"), "rtl"), $(".dlab-demo-panel").removeClass("show"), jQuery(".main-css").attr("href", "https://jobick.dexignlab.com/xhtml/page-error-404.html") }), jQuery(window).on("load", function () { direction = null != direction ? direction : "rtl", "rtl" == getCookie("direction") && jQuery(".main-css").attr("href", "https://jobick.dexignlab.com/xhtml/page-error-404.html"), null != theme ? "rtl" == theme ? (themeChange(0, "rtl"), jQuery(".main-css").attr("href", "https://jobick.dexignlab.com/xhtml/page-error-404.html")) : themeChange(theme, direction) : null != direction && "" == getCookie("version") && themeChange(0, direction), setTimeout(() => { getElementAttrs(document.querySelector("body")).forEach(handleSetThemeOption) }, 1500), setThemeOptionOnPage() }), jQuery(window).on("resize", function () { setThemeOptionOnPage() })
}(jQuery);