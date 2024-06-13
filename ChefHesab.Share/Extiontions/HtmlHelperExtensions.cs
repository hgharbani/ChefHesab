using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public static class HtmlHelperExtensions
    {
        public static string EncryptName(this HtmlHelper htmlHelper, string name)
        {
            return ConfigurationBasedStringEncrypter.Prefix + name;
        }

        public static HtmlString DanWindow(this HtmlHelper helper, string name = "danWindow")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<div id=\"{name}\"></div>");
            stringBuilder.AppendLine("<script type=\"text/javascript\">");
            stringBuilder.AppendLine("var afterWindowsClose;");
            stringBuilder.AppendLine("function WindowOnClose(e) {");
            stringBuilder.AppendLine("if (afterWindowsClose !=null){afterWindowsClose();}}");
            stringBuilder.AppendLine("function CloseWindow(parentWindow) {");
            stringBuilder.AppendLine("if (parentWindow == null) {");
            stringBuilder.AppendLine($"parentWindow = \"{name}\";");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("var windowcontainerId = parentWindow + \"windowcontainer\";");
            stringBuilder.AppendLine("var popupWindow = $(\"#\" + windowcontainerId).data(\"kendoWindow\");");
            stringBuilder.AppendLine("popupWindow.close();");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("function ShowPopup(parentWindow, content, title, width, height,afterWindowsClose) {");
            stringBuilder.AppendLine("if (parentWindow == null) {");
            stringBuilder.AppendLine($"parentWindow = \"{name}\";");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("var windowcontainerId = parentWindow + \"windowcontainer\";");
            stringBuilder.AppendLine("$(\"#\" + parentWindow).append(\"<div id='\" + windowcontainerId + \"'></div>\");");
            stringBuilder.AppendLine("var popupWindow = $(\"#\" + windowcontainerId).kendoWindow({");
            stringBuilder.AppendLine("actions: [\"Close\", \"Maximize\"],");
            stringBuilder.AppendLine("modal: true,");
            stringBuilder.AppendLine("iframe: true,");
            stringBuilder.AppendLine("content: content,");
            stringBuilder.AppendLine("draggable: true,");
            stringBuilder.AppendLine("minHeight: 300,");
            stringBuilder.AppendLine("minWidth: 300,");
            stringBuilder.AppendLine("top: 50,");
            stringBuilder.AppendLine("pinned: false,");
            stringBuilder.AppendLine("visible: false,");
            stringBuilder.AppendLine("resizable: true,");
            stringBuilder.AppendLine("close: WindowOnClose,");
            stringBuilder.AppendLine("deactivate: function () {");
            stringBuilder.AppendLine("this.destroy();");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}).data(\"kendoWindow\");");
            stringBuilder.AppendLine("this.afterWindowsClose = afterWindowsClose;");
            stringBuilder.AppendLine("if (width != null || height != null) {");
            stringBuilder.AppendLine("if (width == null) {");
            stringBuilder.AppendLine("width = $(window).width() - 50;");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("if (height == null) {");
            stringBuilder.AppendLine("height = $(window).height() - 100;");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("var docHeight = $(window).height();");
            stringBuilder.AppendLine("if (docHeight < 650) {");
            stringBuilder.AppendLine("height = docHeight - 50;");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("popupWindow.setOptions({ title: title, width: width, height: height });");
            stringBuilder.AppendLine("popupWindow.center();");
            stringBuilder.AppendLine("} else {");
            stringBuilder.AppendLine("popupWindow.setOptions({ title: title });");
            stringBuilder.AppendLine("popupWindow.maximize();");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("popupWindow.open();");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("</script>");
            return new HtmlString(stringBuilder.ToString());
        }

        public static HtmlString DanLinkWindow(this HtmlHelper helper, string name, string text, string content, string title, string parentWindow = "danWindow", string width = null, string height = null, string afterWindowsClose = null, string cssClass = "btn")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("<a href=\"javascript:void(0);\" id=\"btn{0}\" class=\"{2}\">{1}</a>", name, text, cssClass));
            stringBuilder.Append("<script type=\"text/javascript\">");
            stringBuilder.Append("$(function () {");
            stringBuilder.Append($"$(\"#btn{name}\").click(function () {{");
            stringBuilder.Append(string.Format("ShowPopup(\"{0}\", \"{1}\", \"{2}\", {3}, {4},{5});", parentWindow, content, title, width ?? "null", height ?? "null", afterWindowsClose ?? "null"));
            stringBuilder.Append("});});</script>");
            return new HtmlString(stringBuilder.ToString());
        }
    }
}
