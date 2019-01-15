
window.Modernizr = function (a, b, c) { function A(a, b) { for (var d in a) if (k[a[d]] !== c) return b == "pfx" ? a[d] : !0; return !1 } function z(a, b) { return !! ~("" + a).indexOf(b) } function y(a, b) { return typeof a === b } function x(a, b) { return w(n.join(a + ";") + (b || "")) } function w(a) { k.cssText = a } var d = "2.0.6", e = {}, f = !0, g = b.documentElement, h = b.head || b.getElementsByTagName("head")[0], i = "modernizr", j = b.createElement(i), k = j.style, l, m = Object.prototype.toString, n = " -webkit- -moz- -o- -ms- -khtml- ".split(" "), o = {}, p = {}, q = {}, r = [], s = function (a, c, d, e) { var f, h, j, k = b.createElement("div"); if (parseInt(d, 10)) while (d--) j = b.createElement("div"), j.id = e ? e[d] : i + (d + 1), k.appendChild(j); f = ["&shy;", "<style>", a, "</style>"].join(""), k.id = i, k.innerHTML += f, g.appendChild(k), h = c(k, a), k.parentNode.removeChild(k); return !!h }, t, u = {}.hasOwnProperty, v; !y(u, c) && !y(u.call, c) ? v = function (a, b) { return u.call(a, b) } : v = function (a, b) { return b in a && y(a.constructor.prototype[b], c) }; var B = function (a, c) { var d = a.join(""), f = c.length; s(d, function (a, c) { var d = b.styleSheets[b.styleSheets.length - 1], g = d.cssRules && d.cssRules[0] ? d.cssRules[0].cssText : d.cssText || "", h = a.childNodes, i = {}; while (f--) i[h[f].id] = h[f]; e.csstransforms3d = i.csstransforms3d.offsetLeft === 9 }, f, c) } ([, ["@media (", n.join("transform-3d),("), i, ")", "{#csstransforms3d{left:9px;position:absolute}}"].join("")], [, "csstransforms3d"]); o.canvas = function () { var a = b.createElement("canvas"); return !!a.getContext && !!a.getContext("2d") }, o.csstransforms = function () { return !!A(["transformProperty", "WebkitTransform", "MozTransform", "OTransform", "msTransform"]) }, o.csstransforms3d = function () { var a = !!A(["perspectiveProperty", "WebkitPerspective", "MozPerspective", "OPerspective", "msPerspective"]); a && "webkitPerspective" in g.style && (a = e.csstransforms3d); return a }; for (var C in o) v(o, C) && (t = C.toLowerCase(), e[t] = o[C](), r.push((e[t] ? "" : "no-") + t)); w(""), j = l = null, a.attachEvent && function () { var a = b.createElement("div"); a.innerHTML = "<elem></elem>"; return a.childNodes.length !== 1 } () && function (a, b) { function s(a) { var b = -1; while (++b < g) a.createElement(f[b]) } a.iepp = a.iepp || {}; var d = a.iepp, e = d.html5elements || "abbr|article|aside|audio|canvas|datalist|details|figcaption|figure|footer|header|hgroup|mark|meter|nav|output|progress|section|summary|time|video", f = e.split("|"), g = f.length, h = new RegExp("(^|\\s)(" + e + ")", "gi"), i = new RegExp("<(/*)(" + e + ")", "gi"), j = /^\s*[\{\}]\s*$/, k = new RegExp("(^|[^\\n]*?\\s)(" + e + ")([^\\n]*)({[\\n\\w\\W]*?})", "gi"), l = b.createDocumentFragment(), m = b.documentElement, n = m.firstChild, o = b.createElement("body"), p = b.createElement("style"), q = /print|all/, r; d.getCSS = function (a, b) { if (a + "" === c) return ""; var e = -1, f = a.length, g, h = []; while (++e < f) { g = a[e]; if (g.disabled) continue; b = g.media || b, q.test(b) && h.push(d.getCSS(g.imports, b), g.cssText), b = "all" } return h.join("") }, d.parseCSS = function (a) { var b = [], c; while ((c = k.exec(a)) != null) b.push(((j.exec(c[1]) ? "\n" : c[1]) + c[2] + c[3]).replace(h, "$1.iepp_$2") + c[4]); return b.join("\n") }, d.writeHTML = function () { var a = -1; r = r || b.body; while (++a < g) { var c = b.getElementsByTagName(f[a]), d = c.length, e = -1; while (++e < d) c[e].className.indexOf("iepp_") < 0 && (c[e].className += " iepp_" + f[a]) } l.appendChild(r), m.appendChild(o), o.className = r.className, o.id = r.id, o.innerHTML = r.innerHTML.replace(i, "<$1font") }, d._beforePrint = function () { p.styleSheet.cssText = d.parseCSS(d.getCSS(b.styleSheets, "all")), d.writeHTML() }, d.restoreHTML = function () { o.innerHTML = "", m.removeChild(o), m.appendChild(r) }, d._afterPrint = function () { d.restoreHTML(), p.styleSheet.cssText = "" }, s(b), s(l); d.disablePP || (n.insertBefore(p, n.firstChild), p.media = "print", p.className = "iepp-printshim", a.attachEvent("onbeforeprint", d._beforePrint), a.attachEvent("onafterprint", d._afterPrint)) } (a, b), e._version = d, e._prefixes = n, e.testProp = function (a) { return A([a]) }, e.testStyles = s, g.className = g.className.replace(/\bno-js\b/, "") + (f ? " js " + r.join(" ") : ""); return e } (this, this.document), function (a, b, c) { function k(a) { return !a || a == "loaded" || a == "complete" } function j() { var a = 1, b = -1; while (p.length - ++b) if (p[b].s && !(a = p[b].r)) break; a && g() } function i(a) { var c = b.createElement("script"), d; c.src = a.s, c.onreadystatechange = c.onload = function () { !d && k(c.readyState) && (d = 1, j(), c.onload = c.onreadystatechange = null) }, m(function () { d || (d = 1, j()) }, H.errorTimeout), a.e ? c.onload() : n.parentNode.insertBefore(c, n) } function h(a) { var c = b.createElement("link"), d; c.href = a.s, c.rel = "stylesheet", c.type = "text/css"; if (!a.e && (w || r)) { var e = function (a) { m(function () { if (!d) try { a.sheet.cssRules.length ? (d = 1, j()) : e(a) } catch (b) { b.code == 1e3 || b.message == "security" || b.message == "denied" ? (d = 1, m(function () { j() }, 0)) : e(a) } }, 0) }; e(c) } else c.onload = function () { d || (d = 1, m(function () { j() }, 0)) }, a.e && c.onload(); m(function () { d || (d = 1, j()) }, H.errorTimeout), !a.e && n.parentNode.insertBefore(c, n) } function g() { var a = p.shift(); q = 1, a ? a.t ? m(function () { a.t == "c" ? h(a) : i(a) }, 0) : (a(), j()) : q = 0 } function f(a, c, d, e, f, h) { function i() { !o && k(l.readyState) && (r.r = o = 1, !q && j(), l.onload = l.onreadystatechange = null, m(function () { u.removeChild(l) }, 0)) } var l = b.createElement(a), o = 0, r = { t: d, s: c, e: h }; l.src = l.data = c, !s && (l.style.display = "none"), l.width = l.height = "0", a != "object" && (l.type = d), l.onload = l.onreadystatechange = i, a == "img" ? l.onerror = i : a == "script" && (l.onerror = function () { r.e = r.r = 1, g() }), p.splice(e, 0, r), u.insertBefore(l, s ? null : n), m(function () { o || (u.removeChild(l), r.r = r.e = o = 1, j()) }, H.errorTimeout) } function e(a, b, c) { var d = b == "c" ? z : y; q = 0, b = b || "j", C(a) ? f(d, a, b, this.i++, l, c) : (p.splice(this.i++, 0, a), p.length == 1 && g()); return this } function d() { var a = H; a.loader = { load: e, i: 0 }; return a } var l = b.documentElement, m = a.setTimeout, n = b.getElementsByTagName("script")[0], o = {}.toString, p = [], q = 0, r = "MozAppearance" in l.style, s = r && !!b.createRange().compareNode, t = r && !s, u = s ? l : n.parentNode, v = a.opera && o.call(a.opera) == "[object Opera]", w = "webkitAppearance" in l.style, x = w && "async" in b.createElement("script"), y = r ? "object" : v || x ? "img" : "script", z = w ? "img" : y, A = Array.isArray || function (a) { return o.call(a) == "[object Array]" }, B = function (a) { return Object(a) === a }, C = function (a) { return typeof a == "string" }, D = function (a) { return o.call(a) == "[object Function]" }, E = [], F = {}, G, H; H = function (a) { function f(a) { var b = a.split("!"), c = E.length, d = b.pop(), e = b.length, f = { url: d, origUrl: d, prefixes: b }, g, h; for (h = 0; h < e; h++) g = F[b[h]], g && (f = g(f)); for (h = 0; h < c; h++) f = E[h](f); return f } function e(a, b, e, g, h) { var i = f(a), j = i.autoCallback; if (!i.bypass) { b && (b = D(b) ? b : b[a] || b[g] || b[a.split("/").pop().split("?")[0]]); if (i.instead) return i.instead(a, b, e, g, h); e.load(i.url, i.forceCSS || !i.forceJS && /css$/.test(i.url) ? "c" : c, i.noexec), (D(b) || D(j)) && e.load(function () { d(), b && b(i.origUrl, h, g), j && j(i.origUrl, h, g) }) } } function b(a, b) { function c(a) { if (C(a)) e(a, h, b, 0, d); else if (B(a)) for (i in a) a.hasOwnProperty(i) && e(a[i], h, b, i, d) } var d = !!a.test, f = d ? a.yep : a.nope, g = a.load || a.both, h = a.callback, i; c(f), c(g), a.complete && b.load(a.complete) } var g, h, i = this.yepnope.loader; if (C(a)) e(a, 0, i, 0); else if (A(a)) for (g = 0; g < a.length; g++) h = a[g], C(h) ? e(h, 0, i, 0) : A(h) ? H(h) : B(h) && b(h, i); else B(a) && b(a, i) }, H.addPrefix = function (a, b) { F[a] = b }, H.addFilter = function (a) { E.push(a) }, H.errorTimeout = 1e4, b.readyState == null && b.addEventListener && (b.readyState = "loading", b.addEventListener("DOMContentLoaded", G = function () { b.removeEventListener("DOMContentLoaded", G, 0), b.readyState = "complete" }, 0)), a.yepnope = d() } (this, this.document), Modernizr.load = function () { yepnope.apply(window, [].slice.call(arguments, 0)) };

var serverType = "";
var hosted = true;
var hostedUpload = "0";
var baseDownloadUrl = "";
var holdingPageUrl = "";
var holdingPageDomain = "";
var start;
var timeSpan;
var req;
var totalSize = 0;
var upload = false;
var roundedSpeed = 0;
var downloadTestCount = 0;
var predictedDownloadTestCount = 2;
var g_uploadSpeed = 0;

var currentPayloadId = 0;
var first = true;
var speeds = new Array();
var speedIdx = 0;

var maxDownloadSpeed = 0;
var maxUploadSpeed = 0;
var needlesBackToZeroIdAfterDownload = 0;
var needlesBackToZeroIdAfterUpload = 0;

var ISP;
var speedTestId;

var speedCheckerEnabled = false;
var startUploadTimeout;
var drawStartSpeedTestAgainTimeout;

var residual = 0;

function browserHasNeededHTML5Features() {
    if (Modernizr.csstransforms && Modernizr.canvas) {
        // HTML5 features needed ARE available
        return true;
    }
    else {
        // HTML5 features needed NOT available
        return false;
    }
}

function firstTimeInit() {

    drawFrame(0, 0, "Download test in progress", true, -1, -1);

    upload = false;
    currentPayloadId = 0;
}

function resetForTest() {
    first = true;
    totalSize = 0;
    residual = 0;
    roundedSpeed = 0;
    downloadTestCount = 0;
    predictedDownloadTestCount = 2;
    speeds = new Array();
    speedIdx = 0;
}

function startNextDownload(payloadId) {

    // we want to avoid the biggest file for ipads and iphones
    if (payloadId >= 6) {
        payloadId = 5;
    }

    currentPayloadId = payloadId;
    if (payloadId == 0) {
        return;
    }

    if (req != null) {
        req.removeEventListener("progress", updateProgress, false);
        req.removeEventListener("load", loaded, false);
        req.removeEventListener("error", downloadFailed, false);
        req.removeEventListener("abort", downloadCanceled, false);
    }
    req = new XMLHttpRequest();
    if (req != null) {
        req.addEventListener("progress", updateProgress, false);
        req.addEventListener("load", loaded, false);
        req.addEventListener("error", downloadFailed, false);
        req.addEventListener("abort", downloadCanceled, false);
    }

    var url = baseDownloadUrl;
    if (url == "") {
        url = "http://downloads.broadbandspeedchecker.co.uk/";
    }

    req.open("GET", url + "random" + currentPayloadId + ".jpg?" + Math.random());
    start = (new Date()).getTime();
    req.send();
}

function getNextPayloadSize(payloadID) {

    switch (payloadID) {
        case 2:
            return 4;
        case 3:
            return 2;
        case 4:
            return 5;
        case 5:
            return 6;
        case 6:
            return 0;
    }
    return 0;
}

function generateRandomData(payloadID) {

    switch (payloadID) {
        case 2:
            len = 50000 * 7;
            break;
        case 3:
            len = 100000 * 7;
            break;
        case 4:
            len = 200000 * 6;
            break;
        case 5:
            len = 300000 * 5;
            break;
        case 6:
            len = 400000 * 4;
            break;
    }

    var data = "";
    while (data.length < len) {
        data = data + (Math.floor(Math.random() * 100000000) + 1);
    }
    return data;
}

function downloadFailed(evt) {
}

function downloadCanceled(evt) {
}

function uploadFailed(evt) {
}

function uploadCanceled(evt) {
}

function updateProgress(evt) {

    var percentage = 0;
    if (evt.type == "readystatechange" && req.readyState < 2) {
        return;
    }
    var loaded, total;
    if (evt.lengthComputable) {
        total = totalSize = evt.total;
        loaded = evt.loaded;
    }
    else {
        total = loaded = totalSize;
    }

    // this is a workaround for upload only when it reports wrong progress
    if (upload) {
        if (speedIdx > 0) {
            if (loaded < residual) {
                loaded = loaded + residual;
            }
        }
        residual = loaded;
    }
    percentage = loaded / total; // we keep it as a value between 0..1
    if (percentage >= 1) {
        percentage = 1;
    }

    var now = (new Date()).getTime();
    timeSpan = now - start;
    var speed = ((loaded * 1000 / timeSpan) / 1024 / 1024) * 8;

    roundedSpeed = Math.round(speed * 100) / 100;
    speeds[speedIdx] = roundedSpeed;
    speedIdx = speedIdx + 1;

    var speedAngle = angleForSpeed(roundedSpeed);
    var percentageAngle = angleForPercentage(percentage);

    if (upload) {
        if (percentage >= 0.98) {
            UploadDone();
        }
        else {
            drawFrame(speedAngle, percentageAngle, "Upload test in progress", true, -1, -1);
        }
    }
    else {
        if (!first) {
            drawFrame(speedAngle, percentageAngle, "Download test in progress", true, -1, -1);
        }
    }
} // function updateProgress

function loaded(evt) {

    if (evt.type == "readystatechange" && req.readyState < 2) {
        return;
    }

    if (upload) {
        req.upload.removeEventListener("progress", updateProgress, false);
        req.upload.removeEventListener("load", loaded, false);
        req.removeEventListener("readystatechange", updateProgress, false);
    }
    else {
        downloadTestCount = downloadTestCount + 1;
        req.removeEventListener("progress", updateProgress, false);
        req.removeEventListener("load", loaded, false);
        ret = null;
    }

    //after 231 kb first payload size we decide what's next payload size
    if (!upload && first) {
        first = false;
        // Make 2nd request
        if (timeSpan <= 600) {
            startNextDownload(6);
        }
        else if (timeSpan <= 1500) {
            startNextDownload(5);
        }
        else if (timeSpan <= 4000) {
            startNextDownload(2);
        }
        else {
            startNextDownload(3);
        }
        return;
    }
    else if (!upload && downloadTestCount < predictedDownloadTestCount) {
        // if we have finished the  first test in less than 2 seconds
        // increase the number of test to be ran
        if (timeSpan <= 2000 && predictedDownloadTestCount < 3) {
            predictedDownloadTestCount = 3;
        }
        startNextDownload(getNextPayloadSize(currentPayloadId));
        return;
    }
    if (!upload) {

        // Download test completed, do upload test
        speeds.sort(function (a, b) { return a - b });
        var maxSpeed = speeds[Math.round(speedIdx - (speedIdx / 6))];

        maxDownloadSpeed = maxSpeed;

        upload = false;
        downloadTestCount = 0;
        first = true;

        lastSpeed = roundedSpeed;
        needlesBackToZeroIdAfterDownload = setInterval("setNeedlesBackToZeroAfterDownload()", step);

    } // else if (!upload)
    else {
        UploadDone();
    } // else
} // function loaded

function UploadDone() {
    req.upload.removeEventListener("progress", updateProgress, false);
    req.upload.removeEventListener("load", loaded, false);
    req.removeEventListener("readystatechange", updateProgress, false);

    lastSpeed = roundedSpeed;

    speeds.sort(function (a, b) { return a - b });
    var maxSpeed = 0;
    if (currentPayloadId >= 4) {
        var delta = Math.round(speedIdx / 5);
        if (delta < 2) {
            delta = delta + 1;
        }
        maxSpeed = speeds[speedIdx - delta];
    }
    else {
        maxSpeed = speeds[Math.round(speedIdx / 6)];
    }

    maxUploadSpeed = maxSpeed;

    needlesBackToZeroIdAfterUpload = setInterval("setNeedlesBackToZeroAfterUpload()", step);
}

// Calculate the needle angle for a particular transfer speed
function angleForSpeed(speed) {
    var PI = 3.1415; // 2 * PI = 360
    var degree = 0;

    if (speed <= 1) { // 0 - 0.27
        degree = (0 * 0.27) + (speed * 0.27);
    }
    else if (speed <= 4) { // 0.27 - 0.54
        degree = (1 * 0.27) + ((speed - 1) / 3 * 0.27);
    }
    else if (speed <= 8) { // 0.54 - 0.81
        degree = (2 * 0.27) + ((speed - 4) / 4 * 0.27);
    }
    else if (speed <= 24) { // 0.81 - 1.08
        degree = (3 * 0.27) + ((speed - 8) / 16 * 0.27);
    }
    else if (speed <= 50) { // 1.08 - 1.35
        degree = (4 * 0.27) + ((speed - 24) / 26 * 0.27);
    }
    else { // 1.35 - 1.61
        degree = (5 * 0.27) + ((speed - 50) / 50 * 0.26);
    }

    return degree * PI;
}
// Calculate the needle angle for a particular percentage (0-1)
function angleForPercentage(percentage) {
    var PI = 3.1415; // 2 * PI = 360
    return (percentage * 1.61 * PI);
}

var lastSpeed = 100;
var step = 20;
function setNeedlesBackToZeroAfterDownload() {
    if (lastSpeed > 0.01) {
        var percentage = lastSpeed / roundedSpeed;
        lastSpeed = lastSpeed - (roundedSpeed / (1000 / step));
        drawFrame(angleForSpeed(lastSpeed), angleForPercentage(percentage),
                "Download test completed", true, -1, -1);
    }
    else {
        clearInterval(needlesBackToZeroIdAfterDownload);
        drawFrame(0.0, 0.0, "Download test completed", true, maxDownloadSpeed, -1);

        startUploadTimeout = setTimeout("startUpload()", 1500);
    }
}

function startUpload() {
    clearTimeout(startUploadTimeout);

    upload = true;
    resetForTest();

    req = new XMLHttpRequest();
    req.upload.addEventListener("progress", updateProgress, false);
    req.upload.addEventListener("load", loaded, false);
    req.addEventListener("readystatechange", updateProgress, false);
    req.addEventListener("error", uploadFailed, false);
    req.addEventListener("abort", uploadCanceled, false);

    var url = "";
    //    if (hosted || hostedUpload == "1") {
    //        url = "http://www.broadbandspeedchecker.co.uk/upload.aspx";
    //    }
    //    else {
    //        url = baseDownloadUrl + "upload." + serverType;
    //    }
    url = "http://www.broadbandspeedchecker.co.uk/upload.aspx";

    var data = generateRandomData(currentPayloadId);
    totalSize = data.length;

    req.open("POST", url);
    start = (new Date()).getTime();
    req.send(data);
}

function setNeedlesBackToZeroAfterUpload() {
    if (lastSpeed > 0.01) {
        var percentage = lastSpeed / roundedSpeed;
        lastSpeed = lastSpeed - (roundedSpeed / (1000 / step));
        drawFrame(angleForSpeed(lastSpeed), angleForPercentage(percentage),
                "Speed test completed", true, -1, -1);
    }
    else {
        clearInterval(needlesBackToZeroIdAfterUpload);

        drawFrame(0, 0, "Speed test completed", true, maxDownloadSpeed, maxUploadSpeed);

        saveSpeedTest();

        drawStartSpeedTestAgainTimeout = setTimeout("drawStartSpeedTestAgain()", 1500);
    }
}

function drawStartSpeedTestAgain() {
    clearTimeout(drawStartSpeedTestAgainTimeout);

    drawFrame(0, 0, "Start speed test again", false, maxDownloadSpeed, maxUploadSpeed);

    upload = false;
    downloadTestCount = 0;
    first = true;
}

// ---------------------------------------------------------------------------------
var canvas;
var ctx;
var fullScaleWidth = 385;
var fullScaleHeight = 253;
var widthScale = 1;
var heightScale = 1;
var queryStringList = null;

function readQueryParams() {
    var url = window.location.toString();
    url.match(/\?(.+)$/);
    var params = RegExp.$1;
    // split up the query string and store in an associative array
    var params = params.split("&");
    queryStringList = {};

    for (var i = 0; i < params.length; i++) {
        var tmp = params[i].split("=");
        queryStringList[tmp[0]] = unescape(tmp[1]);
    }
}

function getHoldingPageUrl() {
    if (queryStringList != null) {
        if (queryStringList['holdingPageUrl'] != null) {
            holdingPageUrl = queryStringList['holdingPageUrl'];
        }
    }
}

function getBaseDownloadUrl() {
    if (queryStringList != null) {
        if (queryStringList['baseDownloadUrl'] != null) {
            baseDownloadUrl = queryStringList['baseDownloadUrl'];
        }
    }
    if (baseDownloadUrl != null) {
        if (baseDownloadUrl.indexOf('cdn1') > 0) {
            baseDownloadUrl = baseDownloadUrl.replace('cdn1', 'cdn2');
        }
    }
}

function getLicense() {
    var license = null;
    if (queryStringList != null) {
        if (queryStringList['licenseID'] != null) {
            license = queryStringList['licenseID'];
        }
    }
    return license;
}

function getSpeedcheckerLink() {
    var link = null;
    if (queryStringList != null) {
        if (queryStringList['speedchecker_linkID'] != null) {
            link = queryStringList['speedchecker_linkID'];
        }
    }

    //    if (link == null || link == "") {
    //        link = parent.linkCheck("x = document.getElementById('speedchecker_link').innerHTML.toLowerCase();");
    //    }

    return link;
}

function getServerType() {
    if (queryStringList != null) {
        if (queryStringList['serverType'] != null) {
            serverType = queryStringList['serverType'];
        }
    }
}

function getHostedUpload() {
    if (queryStringList != null) {
        if (queryStringList['hostedUpload'] != null) {
            hostedUpload = queryStringList['hostedUpload'];
        }
    }
}

function getWidth(w) {
    var width = null;
    if (queryStringList != null) {
        if (queryStringList['sc_w'] != null) {
            width = queryStringList['sc_w'];
        }
    }
    if (width == null) {
        width = w;
    }
    return width;
}

function getHeight(h) {
    var height = null;
    if (queryStringList != null) {
        if (queryStringList['sc_h'] != null) {
            height = queryStringList['sc_h'];
        }
    }
    if (height == null) {
        height = h;
    }
    return height;
}

function init(width, height) {

    if (browserHasNeededHTML5Features()) {

        readQueryParams();
        getBaseDownloadUrl();
        getHostedUpload();
        getServerType();
        getHoldingPageUrl();

        var startButtonText = getLinkCheckCode();

        canvas = document.getElementById("canvas");
        ctx = canvas.getContext("2d");

        width = getWidth(width);
        height = getHeight(height);

        widthScale = width / fullScaleWidth;
        heightScale = height / fullScaleHeight;
        canvas.width = width;
        canvas.height = height;

        if (serverType == "asp" || serverType == "php") {
            hosted = false;
        }

        if (speedCheckerEnabled) {
            drawFrame(0.0, 0.0, "Start speed test", false, -1, -1);
        }
        else {
            drawFrame(0.0, 0.0, startButtonText, true, -1, -1);
        }
    }
    else {
    }
}

function drawFrame(needle1Dgr, needle2Dgr, startButtonText, disabled, maxDSpeed, maxUSpeed) {

    var downSpeed = 0;
    var upSpeed = 0;

    // Clear canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    ctx.save();
    ctx.scale(widthScale, heightScale);
    ctx.drawImage(document.getElementById("basicProgressBarImg"), 2.0, 208.0);
    ctx.drawImage(document.getElementById("speedTestDialImg"), 0.0, 0.0);

    drawStartButton(ctx, startButtonText, disabled);

    ctx.drawImage(document.getElementById("speedTestProgressImg"), 200.0, 41.6);

    if (upload) {
        if (maxUSpeed != -1) {
            upSpeed = maxUSpeed;
        }
        else {
            upSpeed = roundedSpeed;
        }
        if (maxDSpeed != -1) {
            downSpeed = maxDSpeed;
        }
        else {
            downSpeed = maxDownloadSpeed;
        }
    }
    else {
        if (maxDSpeed != -1) {
            downSpeed = maxDSpeed;
        }
        else {
            downSpeed = roundedSpeed;
        }
        upSpeed = 0;
    }

    if (startButtonText != "Start speed test") {
        ctx.drawImage(document.getElementById("scoreBoardImg"), 175, 0, 210, 42);
        drawSpeeds(downSpeed, upSpeed);
    }
    ctx.restore();

    ctx.save();
    ctx.scale(widthScale, heightScale);
    ctx.translate(102.3, 94.6);
    ctx.rotate(needle1Dgr);
    ctx.drawImage(document.getElementById("needle1Img"), -47.0, -14.7);
    ctx.restore();

    ctx.save();
    ctx.scale(widthScale, heightScale);
    ctx.translate(279.1, 115.1);
    ctx.rotate(needle2Dgr);
    ctx.drawImage(document.getElementById("needle2Img"), -32.4, -11.4);
    ctx.restore();
}

function drawSpeeds(downloadSpeed, uploadSpeed) {
    ctx.font = "15px verdana";
    ctx.fillStyle = "white";
    ctx.fillText(downloadSpeed + " Mb/s", 182, 34);
    ctx.fillText(uploadSpeed + " Mb/s", 293, 34);
}

function drawStartButton(ctx, buttonText, disabled) {
    if (disabled) {
        canvas.removeEventListener("click", startButtonClicked, false);
        ctx.drawImage(document.getElementById("startButtonImg"), 84.0, 200.0);
    }
    else {
        canvas.addEventListener("click", startButtonClicked, false);
        ctx.drawImage(document.getElementById("startButtonImg"), 84.0, 200.0);
    }
    ctx.font = "bold 13px verdana";
    ctx.fillStyle = "white";
    var textWidth = ctx.measureText(buttonText).width;
    ctx.fillText(buttonText, 84 + (211 - textWidth) / 2, 220);
}

function startButtonClicked(e) {
    ctx.save();
    ctx.scale(widthScale, heightScale);
    ctx.beginPath();
    ctx.moveTo(84, 200);
    ctx.lineTo(84, 228);
    ctx.lineTo(295, 228);
    ctx.lineTo(295, 200);
    ctx.lineTo(84, 200);
    ctx.closePath();
    ctx.restore();

    var x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft - canvas.offsetLeft;
    var y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop - canvas.offsetTop;

    if (ctx.isPointInPath(x, y)) {
        firstTimeInit();
        resetForTest();
        startNextDownload(3);
    }
}

//---------------- JS-----------------
//V3.01.A - http://www.openjs.com/scripts/jx/
jx = {
    //Create a xmlHttpRequest object - this is the constructor. 
    getHTTPObject: function () {
        var http = false;
        //Use IE's ActiveX items to load the file.
        if (typeof ActiveXObject != 'undefined') {
            try { http = new ActiveXObject("Msxml2.XMLHTTP"); }
            catch (e) {
                try { http = new ActiveXObject("Microsoft.XMLHTTP"); }
                catch (E) { http = false; }
            }
            //If ActiveX is not available, use the XMLHttpRequest of Firefox/Mozilla etc. to load the document.
        } else if (window.XMLHttpRequest) {
            try { http = new XMLHttpRequest(); }
            catch (e) { http = false; }
        }

        return http;
    },
    // This function is called from the user's script. 
    //Arguments - 
    //	url	- The url of the serverside script that is to be called. Append all the arguments to 
    //			this url - eg. 'get_data.php?id=5&car=benz'
    //	callback - Function that must be called once the data is ready.
    //	format - The return type for this function. Could be 'xml','json' or 'text'. If it is json, 
    //			the string will be 'eval'ed before returning it. Default:'text'
    load: function (url, callback, format) {
        var http = this.init(); //The XMLHttpRequest object is recreated at every call - to defeat Cache problem in IE
        if (!http || !url) return;
        if (http.overrideMimeType) http.overrideMimeType('text/xml');

        if (!format) var format = "text"; //Default return type is 'text'
        format = format.toLowerCase();

        //Kill the Cache problem in IE.
        var now = "uid=" + new Date().getTime();
        url += (url.indexOf("?") + 1) ? "&" : "?";
        url += now;

        http.open("GET", url, true);

        http.onreadystatechange = function () {//Call a function when the state changes.
            if (http.readyState == 4) {//Ready State will be 4 when the document is loaded.
                if (http.status == 200) {
                    var result = "";
                    if (http.responseText) result = http.responseText;

                    //If the return is in JSON format, eval the result before returning it.
                    if (format.charAt(0) == "j") {
                        //\n's in JSON string, when evaluated will create errors in IE
                        result = result.replace(/[\n\r]/g, "");
                        result = eval('(' + result + ')');
                    }

                    //Give the data to the callback function.
                    if (callback) callback(result);
                } else { //An error occured
                    if (error) error(http.status);
                }
            }
        }
        http.send(null);
    },
    init: function () { return this.getHTTPObject(); }
}

function getDomain(url) {
    if (!url) return '';
    var temp = url.replace('http://', '');
    temp = temp.replace('https://', '');
    var temp = temp.split('/');
    return getTLD(temp[0]);
}

// Get the top-level domain from a fully-qualified domain name
function getTLD(fqdn) {
    fqdn = fqdn.toLowerCase(); // force lowercase for parsing
    var labels = fqdn.split('.');
    if (labels.length >= 3) {
        var www = labels[0] == 'www' ? 1 : 0;
        // assume that any domain having at least 3 labels ending with a 2 byte label, also contains a country code for its last two labels
        labels.splice(www, (labels.slice(-1).toString().length == 2 ? labels.length - 3 - www : labels.length - 2 - www)); // chop subdomains
    }
    return labels.join('.');
}

function saveSpeedTest() {
    var http = new jx.getHTTPObject();
    var url = "http://www.broadbandspeedchecker.co.uk/saveSpeedtest.aspx";
    var dl = (1024 * maxDownloadSpeed);
    var ul = (1024 * maxUploadSpeed);
    var params = "dl=" + dl + "&ul=" + ul + "&page=" + holdingPageUrl + "&site=" + holdingPageDomain;

    http.open("GET", url + "?" + params, true);
    http.onreadystatechange = function () {//Call a function when the state changes.
        if (http.readyState == 4 && http.status == 200) {
            outputParams = http.responseText;

            ispPos = outputParams.search("isp=");
            idPos = outputParams.search("speedtestid=");
            isp = outputParams.substring(ispPos + 4, idPos);
            speedtestid = outputParams.substring(idPos + 12);
            try {
                parent.speedCheckerCompleted(dl + "," + ul + ",," + speedtestid);
            }
            catch (e) {
                var es = e;
            }
        }
    }
    http.send(null);
}

var hexcase = 0; function hex_md5(a) { return rstr2hex(rstr_md5(str2rstr_utf8(a))) } function hex_hmac_md5(a, b) { return rstr2hex(rstr_hmac_md5(str2rstr_utf8(a), str2rstr_utf8(b))) } function md5_vm_test() { return hex_md5("abc").toLowerCase() == "900150983cd24fb0d6963f7d28e17f72" } function rstr_md5(a) { return binl2rstr(binl_md5(rstr2binl(a), a.length * 8)) } function rstr_hmac_md5(c, f) { var e = rstr2binl(c); if (e.length > 16) { e = binl_md5(e, c.length * 8) } var a = Array(16), d = Array(16); for (var b = 0; b < 16; b++) { a[b] = e[b] ^ 909522486; d[b] = e[b] ^ 1549556828 } var g = binl_md5(a.concat(rstr2binl(f)), 512 + f.length * 8); return binl2rstr(binl_md5(d.concat(g), 512 + 128)) } function rstr2hex(c) { try { hexcase } catch (g) { hexcase = 0 } var f = hexcase ? "0123456789ABCDEF" : "0123456789abcdef"; var b = ""; var a; for (var d = 0; d < c.length; d++) { a = c.charCodeAt(d); b += f.charAt((a >>> 4) & 15) + f.charAt(a & 15) } return b } function str2rstr_utf8(c) { var b = ""; var d = -1; var a, e; while (++d < c.length) { a = c.charCodeAt(d); e = d + 1 < c.length ? c.charCodeAt(d + 1) : 0; if (55296 <= a && a <= 56319 && 56320 <= e && e <= 57343) { a = 65536 + ((a & 1023) << 10) + (e & 1023); d++ } if (a <= 127) { b += String.fromCharCode(a) } else { if (a <= 2047) { b += String.fromCharCode(192 | ((a >>> 6) & 31), 128 | (a & 63)) } else { if (a <= 65535) { b += String.fromCharCode(224 | ((a >>> 12) & 15), 128 | ((a >>> 6) & 63), 128 | (a & 63)) } else { if (a <= 2097151) { b += String.fromCharCode(240 | ((a >>> 18) & 7), 128 | ((a >>> 12) & 63), 128 | ((a >>> 6) & 63), 128 | (a & 63)) } } } } } return b } function rstr2binl(b) { var a = Array(b.length >> 2); for (var c = 0; c < a.length; c++) { a[c] = 0 } for (var c = 0; c < b.length * 8; c += 8) { a[c >> 5] |= (b.charCodeAt(c / 8) & 255) << (c % 32) } return a } function binl2rstr(b) { var a = ""; for (var c = 0; c < b.length * 32; c += 8) { a += String.fromCharCode((b[c >> 5] >>> (c % 32)) & 255) } return a } function binl_md5(p, k) { p[k >> 5] |= 128 << ((k) % 32); p[(((k + 64) >>> 9) << 4) + 14] = k; var o = 1732584193; var n = -271733879; var m = -1732584194; var l = 271733878; for (var g = 0; g < p.length; g += 16) { var j = o; var h = n; var f = m; var e = l; o = md5_ff(o, n, m, l, p[g + 0], 7, -680876936); l = md5_ff(l, o, n, m, p[g + 1], 12, -389564586); m = md5_ff(m, l, o, n, p[g + 2], 17, 606105819); n = md5_ff(n, m, l, o, p[g + 3], 22, -1044525330); o = md5_ff(o, n, m, l, p[g + 4], 7, -176418897); l = md5_ff(l, o, n, m, p[g + 5], 12, 1200080426); m = md5_ff(m, l, o, n, p[g + 6], 17, -1473231341); n = md5_ff(n, m, l, o, p[g + 7], 22, -45705983); o = md5_ff(o, n, m, l, p[g + 8], 7, 1770035416); l = md5_ff(l, o, n, m, p[g + 9], 12, -1958414417); m = md5_ff(m, l, o, n, p[g + 10], 17, -42063); n = md5_ff(n, m, l, o, p[g + 11], 22, -1990404162); o = md5_ff(o, n, m, l, p[g + 12], 7, 1804603682); l = md5_ff(l, o, n, m, p[g + 13], 12, -40341101); m = md5_ff(m, l, o, n, p[g + 14], 17, -1502002290); n = md5_ff(n, m, l, o, p[g + 15], 22, 1236535329); o = md5_gg(o, n, m, l, p[g + 1], 5, -165796510); l = md5_gg(l, o, n, m, p[g + 6], 9, -1069501632); m = md5_gg(m, l, o, n, p[g + 11], 14, 643717713); n = md5_gg(n, m, l, o, p[g + 0], 20, -373897302); o = md5_gg(o, n, m, l, p[g + 5], 5, -701558691); l = md5_gg(l, o, n, m, p[g + 10], 9, 38016083); m = md5_gg(m, l, o, n, p[g + 15], 14, -660478335); n = md5_gg(n, m, l, o, p[g + 4], 20, -405537848); o = md5_gg(o, n, m, l, p[g + 9], 5, 568446438); l = md5_gg(l, o, n, m, p[g + 14], 9, -1019803690); m = md5_gg(m, l, o, n, p[g + 3], 14, -187363961); n = md5_gg(n, m, l, o, p[g + 8], 20, 1163531501); o = md5_gg(o, n, m, l, p[g + 13], 5, -1444681467); l = md5_gg(l, o, n, m, p[g + 2], 9, -51403784); m = md5_gg(m, l, o, n, p[g + 7], 14, 1735328473); n = md5_gg(n, m, l, o, p[g + 12], 20, -1926607734); o = md5_hh(o, n, m, l, p[g + 5], 4, -378558); l = md5_hh(l, o, n, m, p[g + 8], 11, -2022574463); m = md5_hh(m, l, o, n, p[g + 11], 16, 1839030562); n = md5_hh(n, m, l, o, p[g + 14], 23, -35309556); o = md5_hh(o, n, m, l, p[g + 1], 4, -1530992060); l = md5_hh(l, o, n, m, p[g + 4], 11, 1272893353); m = md5_hh(m, l, o, n, p[g + 7], 16, -155497632); n = md5_hh(n, m, l, o, p[g + 10], 23, -1094730640); o = md5_hh(o, n, m, l, p[g + 13], 4, 681279174); l = md5_hh(l, o, n, m, p[g + 0], 11, -358537222); m = md5_hh(m, l, o, n, p[g + 3], 16, -722521979); n = md5_hh(n, m, l, o, p[g + 6], 23, 76029189); o = md5_hh(o, n, m, l, p[g + 9], 4, -640364487); l = md5_hh(l, o, n, m, p[g + 12], 11, -421815835); m = md5_hh(m, l, o, n, p[g + 15], 16, 530742520); n = md5_hh(n, m, l, o, p[g + 2], 23, -995338651); o = md5_ii(o, n, m, l, p[g + 0], 6, -198630844); l = md5_ii(l, o, n, m, p[g + 7], 10, 1126891415); m = md5_ii(m, l, o, n, p[g + 14], 15, -1416354905); n = md5_ii(n, m, l, o, p[g + 5], 21, -57434055); o = md5_ii(o, n, m, l, p[g + 12], 6, 1700485571); l = md5_ii(l, o, n, m, p[g + 3], 10, -1894986606); m = md5_ii(m, l, o, n, p[g + 10], 15, -1051523); n = md5_ii(n, m, l, o, p[g + 1], 21, -2054922799); o = md5_ii(o, n, m, l, p[g + 8], 6, 1873313359); l = md5_ii(l, o, n, m, p[g + 15], 10, -30611744); m = md5_ii(m, l, o, n, p[g + 6], 15, -1560198380); n = md5_ii(n, m, l, o, p[g + 13], 21, 1309151649); o = md5_ii(o, n, m, l, p[g + 4], 6, -145523070); l = md5_ii(l, o, n, m, p[g + 11], 10, -1120210379); m = md5_ii(m, l, o, n, p[g + 2], 15, 718787259); n = md5_ii(n, m, l, o, p[g + 9], 21, -343485551); o = safe_add(o, j); n = safe_add(n, h); m = safe_add(m, f); l = safe_add(l, e) } return Array(o, n, m, l) } function md5_cmn(h, e, d, c, g, f) { return safe_add(bit_rol(safe_add(safe_add(e, h), safe_add(c, f)), g), d) } function md5_ff(g, f, k, j, e, i, h) { return md5_cmn((f & k) | ((~f) & j), g, f, e, i, h) } function md5_gg(g, f, k, j, e, i, h) { return md5_cmn((f & j) | (k & (~j)), g, f, e, i, h) } function md5_hh(g, f, k, j, e, i, h) { return md5_cmn(f ^ k ^ j, g, f, e, i, h) } function md5_ii(g, f, k, j, e, i, h) { return md5_cmn(k ^ (f | (~j)), g, f, e, i, h) } function safe_add(a, d) { var c = (a & 65535) + (d & 65535); var b = (a >> 16) + (d >> 16) + (c >> 16); return (b << 16) | (c & 65535) } function bit_rol(a, b) { return (a << b) | (a >>> (32 - b)) };
function getLinkCheckCode() {

    holdingPageDomain = getDomain(holdingPageUrl);

    var license = getLicense();

    if (license != hex_md5("md5" + holdingPageDomain)) {
        var linkValid = false;

        var str = getSpeedcheckerLink();
        if (str.indexOf('<a href=\"http://www.broadbandspeedchecker.co.uk\">') < 0 || !str) {
            // link not found
        }
        else {
            //test if anchor text is there
            str = replace(str, '<a href=\"http://www.broadbandspeedchecker.co.uk\">', '');
            if (str.indexOf('speed') >= 0 || str.indexOf('broadband') >= 0) {
                linkValid = true;
            }
        }

        if (!linkValid) {
            //lets check with server code 

            var http = new jx.getHTTPObject();
            var url = "http://www.broadbandspeedchecker.co.uk/licenseCheck.aspx?page=" + holdingPageUrl;

            http.open("GET", url, true);
            http.onreadystatechange = function () {//Call a function when the state changes.
                if (http.readyState == 4 && http.status == 200) {
                    outputParams = http.responseText;

                    serverlicensecheckPos = outputParams.search("serverlicensecheck=");
                    serverlicensecheck = outputParams.substring(serverlicensecheckPos + 19);

                    if (serverlicensecheck == 'INVALID') {
                        speedCheckerEnabled = false;
                    }
                    else {
                        speedCheckerEnabled = true;
                        drawFrame(0.0, 0.0, "Start speed test", false, -1, -1);
                    }
                }
            }
            http.send(null);

            return "Preparing download test";
        }
        else {
            speedCheckerEnabled = true;
        }
    }
    else {
        speedCheckerEnabled = true;
    }
    return "License error";
}

function replace(holder, searchfor, replacement) {
    var temparray = holder.split(searchfor);
    holder = temparray.join(replacement);
    return (holder);
}

// ---------------------------------------------------------------------------------