var layer = window.parent.layer;
if (!layer) {
    $.getScript("../../../js/layer/layer.js");
    $("head").append("<link>");
    css = $("head").children(":last");
    css.attr({
        rel: "stylesheet",
        type: "text/css",
        href: "../../../js/layer/skin/layer.css"
    });
}