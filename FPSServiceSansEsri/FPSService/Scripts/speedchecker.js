/*	SWFObject v2.0 <http://code.google.com/p/swfobject/>
Copyright (c) 2007 Geoff Stearns, Michael Williams, and Bobby van der Sluis
This software is released under the MIT License <http://www.opensource.org/licenses/mit-license.php>
*/

var sc_swfobject = function () { var Z = "undefined", P = "object", B = "Shockwave Flash", h = "ShockwaveFlash.ShockwaveFlash", W = "application/x-shockwave-flash", K = "SWFObjectExprInst", G = window, g = document, N = navigator, f = [], H = [], Q = null, L = null, T = null, S = false, C = false; var a = function () { var l = typeof g.getElementById != Z && typeof g.getElementsByTagName != Z && typeof g.createElement != Z && typeof g.appendChild != Z && typeof g.replaceChild != Z && typeof g.removeChild != Z && typeof g.cloneNode != Z, t = [0, 0, 0], n = null; if (typeof N.plugins != Z && typeof N.plugins[B] == P) { n = N.plugins[B].description; if (n) { n = n.replace(/^.*\s+(\S+\s+\S+$)/, "$1"); t[0] = parseInt(n.replace(/^(.*)\..*$/, "$1"), 10); t[1] = parseInt(n.replace(/^.*\.(.*)\s.*$/, "$1"), 10); t[2] = /r/.test(n) ? parseInt(n.replace(/^.*r(.*)$/, "$1"), 10) : 0 } } else { if (typeof G.ActiveXObject != Z) { var o = null, s = false; try { o = new ActiveXObject(h + ".7") } catch (k) { try { o = new ActiveXObject(h + ".6"); t = [6, 0, 21]; o.AllowScriptAccess = "always" } catch (k) { if (t[0] == 6) { s = true } } if (!s) { try { o = new ActiveXObject(h) } catch (k) { } } } if (!s && o) { try { n = o.GetVariable("$version"); if (n) { n = n.split(" ")[1].split(","); t = [parseInt(n[0], 10), parseInt(n[1], 10), parseInt(n[2], 10)] } } catch (k) { } } } } var v = N.userAgent.toLowerCase(), j = N.platform.toLowerCase(), r = /webkit/.test(v) ? parseFloat(v.replace(/^.*webkit\/(\d+(\.\d+)?).*$/, "$1")) : false, i = false, q = j ? /win/.test(j) : /win/.test(v), m = j ? /mac/.test(j) : /mac/.test(v); /*@cc_oni = true; @if(@_win32)q = true; @elif(@_mac)m=true;@end@*/return { w3cdom: l, pv: t, webkit: r, ie: i, win: q, mac: m} } (); var e = function () { if (!a.w3cdom) { return } J(I); if (a.ie && a.win) { try { g.write("<script id=__ie_ondomload defer=true src=//:><\/script>"); var i = c("__ie_ondomload"); if (i) { i.onreadystatechange = function () { if (this.readyState == "complete") { this.parentNode.removeChild(this); V() } } } } catch (j) { } } if (a.webkit && typeof g.readyState != Z) { Q = setInterval(function () { if (/loaded|complete/.test(g.readyState)) { V() } }, 10) } if (typeof g.addEventListener != Z) { g.addEventListener("DOMContentLoaded", V, null) } M(V) } (); function V() { if (S) { return } if (a.ie && a.win) { var m = Y("span"); try { var l = g.getElementsByTagName("body")[0].appendChild(m); l.parentNode.removeChild(l) } catch (n) { return } } S = true; if (Q) { clearInterval(Q); Q = null } var j = f.length; for (var k = 0; k < j; k++) { f[k]() } } function J(i) { if (S) { i() } else { f[f.length] = i } } function M(j) { if (typeof G.addEventListener != Z) { G.addEventListener("load", j, false) } else { if (typeof g.addEventListener != Z) { g.addEventListener("load", j, false) } else { if (typeof G.attachEvent != Z) { G.attachEvent("onload", j) } else { if (typeof G.onload == "function") { var i = G.onload; G.onload = function () { i(); j() } } else { G.onload = j } } } } } function I() { var l = H.length; for (var j = 0; j < l; j++) { var m = H[j].id; if (a.pv[0] > 0) { var k = c(m); if (k) { H[j].width = k.getAttribute("width") ? k.getAttribute("width") : "0"; H[j].height = k.getAttribute("height") ? k.getAttribute("height") : "0"; if (O(H[j].swfVersion)) { if (a.webkit && a.webkit < 312) { U(k) } X(m, true) } else { if (H[j].expressInstall && !C && O("6.0.65") && (a.win || a.mac)) { D(H[j]) } else { d(k) } } } } else { X(m, true) } } } function U(m) { var k = m.getElementsByTagName(P)[0]; if (k) { var p = Y("embed"), r = k.attributes; if (r) { var o = r.length; for (var n = 0; n < o; n++) { if (r[n].nodeName.toLowerCase() == "data") { p.setAttribute("src", r[n].nodeValue) } else { p.setAttribute(r[n].nodeName, r[n].nodeValue) } } } var q = k.childNodes; if (q) { var s = q.length; for (var l = 0; l < s; l++) { if (q[l].nodeType == 1 && q[l].nodeName.toLowerCase() == "param") { p.setAttribute(q[l].getAttribute("name"), q[l].getAttribute("value")) } } } m.parentNode.replaceChild(p, m) } } function F(i) { if (a.ie && a.win && O("8.0.0")) { G.attachEvent("onunload", function () { var k = c(i); if (k) { for (var j in k) { if (typeof k[j] == "function") { k[j] = function () { } } } k.parentNode.removeChild(k) } }) } } function D(j) { C = true; var o = c(j.id); if (o) { if (j.altContentId) { var l = c(j.altContentId); if (l) { L = l; T = j.altContentId } } else { L = b(o) } if (!(/%$/.test(j.width)) && parseInt(j.width, 10) < 310) { j.width = "310" } if (!(/%$/.test(j.height)) && parseInt(j.height, 10) < 137) { j.height = "137" } g.title = g.title.slice(0, 47) + " - Flash Player Installation"; var n = a.ie && a.win ? "ActiveX" : "PlugIn", k = g.title, m = "MMredirectURL=" + G.location + "&MMplayerType=" + n + "&MMdoctitle=" + k, p = j.id; if (a.ie && a.win && o.readyState != 4) { var i = Y("div"); p += "SWFObjectNew"; i.setAttribute("id", p); o.parentNode.insertBefore(i, o); o.style.display = "none"; G.attachEvent("onload", function () { o.parentNode.removeChild(o) }) } R({ data: j.expressInstall, id: K, width: j.width, height: j.height }, { flashvars: m }, p) } } function d(j) { if (a.ie && a.win && j.readyState != 4) { var i = Y("div"); j.parentNode.insertBefore(i, j); i.parentNode.replaceChild(b(j), i); j.style.display = "none"; G.attachEvent("onload", function () { j.parentNode.removeChild(j) }) } else { j.parentNode.replaceChild(b(j), j) } } function b(n) { var m = Y("div"); if (a.win && a.ie) { m.innerHTML = n.innerHTML } else { var k = n.getElementsByTagName(P)[0]; if (k) { var o = k.childNodes; if (o) { var j = o.length; for (var l = 0; l < j; l++) { if (!(o[l].nodeType == 1 && o[l].nodeName.toLowerCase() == "param") && !(o[l].nodeType == 8)) { m.appendChild(o[l].cloneNode(true)) } } } } } return m } function R(AE, AC, q) { var p, t = c(q); if (typeof AE.id == Z) { AE.id = q } if (a.ie && a.win) { var AD = ""; for (var z in AE) { if (AE[z] != Object.prototype[z]) { if (z == "data") { AC.movie = AE[z] } else { if (z.toLowerCase() == "styleclass") { AD += ' class="' + AE[z] + '"' } else { if (z != "classid") { AD += " " + z + '="' + AE[z] + '"' } } } } } var AB = ""; for (var y in AC) { if (AC[y] != Object.prototype[y]) { AB += '<param name="' + y + '" value="' + AC[y] + '" />' } } t.outerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"' + AD + ">" + AB + "</object>"; F(AE.id); p = c(AE.id) } else { if (a.webkit && a.webkit < 312) { var AA = Y("embed"); AA.setAttribute("type", W); for (var x in AE) { if (AE[x] != Object.prototype[x]) { if (x == "data") { AA.setAttribute("src", AE[x]) } else { if (x.toLowerCase() == "styleclass") { AA.setAttribute("class", AE[x]) } else { if (x != "classid") { AA.setAttribute(x, AE[x]) } } } } } for (var w in AC) { if (AC[w] != Object.prototype[w]) { if (w != "movie") { AA.setAttribute(w, AC[w]) } } } t.parentNode.replaceChild(AA, t); p = AA } else { var s = Y(P); s.setAttribute("type", W); for (var v in AE) { if (AE[v] != Object.prototype[v]) { if (v.toLowerCase() == "styleclass") { s.setAttribute("class", AE[v]) } else { if (v != "classid") { s.setAttribute(v, AE[v]) } } } } for (var u in AC) { if (AC[u] != Object.prototype[u] && u != "movie") { E(s, u, AC[u]) } } t.parentNode.replaceChild(s, t); p = s } } return p } function E(k, i, j) { var l = Y("param"); l.setAttribute("name", i); l.setAttribute("value", j); k.appendChild(l) } function c(i) { return g.getElementById(i) } function Y(i) { return g.createElement(i) } function O(k) { var j = a.pv, i = k.split("."); i[0] = parseInt(i[0], 10); i[1] = parseInt(i[1], 10); i[2] = parseInt(i[2], 10); return (j[0] > i[0] || (j[0] == i[0] && j[1] > i[1]) || (j[0] == i[0] && j[1] == i[1] && j[2] >= i[2])) ? true : false } function A(m, j) { if (a.ie && a.mac) { return } var l = g.getElementsByTagName("head")[0], k = Y("style"); k.setAttribute("type", "text/css"); k.setAttribute("media", "screen"); if (!(a.ie && a.win) && typeof g.createTextNode != Z) { k.appendChild(g.createTextNode(m + " {" + j + "}")) } l.appendChild(k); if (a.ie && a.win && typeof g.styleSheets != Z && g.styleSheets.length > 0) { var i = g.styleSheets[g.styleSheets.length - 1]; if (typeof i.addRule == P) { i.addRule(m, j) } } } function X(k, i) { var j = i ? "visible" : "hidden"; if (S) { c(k).style.visibility = j } else { A("#" + k, "visibility:" + j) } } return { registerObject: function (l, i, k) { if (!a.w3cdom || !l || !i) { return } var j = {}; j.id = l; j.swfVersion = i; j.expressInstall = k ? k : false; H[H.length] = j; X(l, false) }, getObjectById: function (l) { var i = null; if (a.w3cdom && S) { var j = c(l); if (j) { var k = j.getElementsByTagName(P)[0]; if (!k || (k && typeof j.SetVariable != Z)) { i = j } else { if (typeof k.SetVariable != Z) { i = k } } } } return i }, embedSWF: function (n, u, r, t, j, m, k, p, s) { if (!a.w3cdom || !n || !u || !r || !t || !j) { return } r += ""; t += ""; if (O(j)) { X(u, false); var q = (typeof s == P) ? s : {}; q.data = n; q.width = r; q.height = t; var o = (typeof p == P) ? p : {}; if (typeof k == P) { for (var l in k) { if (k[l] != Object.prototype[l]) { if (typeof o.flashvars != Z) { o.flashvars += "&" + l + "=" + k[l] } else { o.flashvars = l + "=" + k[l] } } } } J(function () { R(q, o, u); if (q.id == u) { X(u, true) } }) } else { if (m && !C && O("6.0.65") && (a.win || a.mac)) { X(u, false); J(function () { var i = {}; i.id = i.altContentId = u; i.width = r; i.height = t; i.expressInstall = m; D(i) }) } } }, getFlashPlayerVersion: function () { return { major: a.pv[0], minor: a.pv[1], release: a.pv[2]} }, hasFlashPlayerVersion: O, createSWF: function (k, j, i) { if (a.w3cdom && S) { return R(k, j, i) } else { return undefined } }, createCSS: function (j, i) { if (a.w3cdom) { A(j, i) } }, addDomLoadEvent: J, addLoadEvent: M, getQueryParamValue: function (m) { var l = g.location.search || g.location.hash; if (m == null) { return l } if (l) { var k = l.substring(1).split("&"); for (var j = 0; j < k.length; j++) { if (k[j].substring(0, k[j].indexOf("=")) == m) { return k[j].substring((k[j].indexOf("=") + 1)) } } } return "" }, expressInstallCallback: function () { if (C && L) { var i = c(K); if (i) { i.parentNode.replaceChild(L, i); if (T) { X(T, true); if (a.ie && a.win) { L.style.display = "block" } } L = null; T = null; C = false } } } } } ();

window.Modernizr = function (a, b, c) { function A(a, b) { for (var d in a) if (k[a[d]] !== c) return b == "pfx" ? a[d] : !0; return !1 } function z(a, b) { return !! ~("" + a).indexOf(b) } function y(a, b) { return typeof a === b } function x(a, b) { return w(n.join(a + ";") + (b || "")) } function w(a) { k.cssText = a } var d = "2.0.6", e = {}, f = !0, g = b.documentElement, h = b.head || b.getElementsByTagName("head")[0], i = "modernizr", j = b.createElement(i), k = j.style, l, m = Object.prototype.toString, n = " -webkit- -moz- -o- -ms- -khtml- ".split(" "), o = {}, p = {}, q = {}, r = [], s = function (a, c, d, e) { var f, h, j, k = b.createElement("div"); if (parseInt(d, 10)) while (d--) j = b.createElement("div"), j.id = e ? e[d] : i + (d + 1), k.appendChild(j); f = ["&shy;", "<style>", a, "</style>"].join(""), k.id = i, k.innerHTML += f, g.appendChild(k), h = c(k, a), k.parentNode.removeChild(k); return !!h }, t, u = {}.hasOwnProperty, v; !y(u, c) && !y(u.call, c) ? v = function (a, b) { return u.call(a, b) } : v = function (a, b) { return b in a && y(a.constructor.prototype[b], c) }; var B = function (a, c) { var d = a.join(""), f = c.length; s(d, function (a, c) { var d = b.styleSheets[b.styleSheets.length - 1], g = d.cssRules && d.cssRules[0] ? d.cssRules[0].cssText : d.cssText || "", h = a.childNodes, i = {}; while (f--) i[h[f].id] = h[f]; e.csstransforms3d = i.csstransforms3d.offsetLeft === 9 }, f, c) } ([, ["@media (", n.join("transform-3d),("), i, ")", "{#csstransforms3d{left:9px;position:absolute}}"].join("")], [, "csstransforms3d"]); o.canvas = function () { var a = b.createElement("canvas"); return !!a.getContext && !!a.getContext("2d") }, o.csstransforms = function () { return !!A(["transformProperty", "WebkitTransform", "MozTransform", "OTransform", "msTransform"]) }, o.csstransforms3d = function () { var a = !!A(["perspectiveProperty", "WebkitPerspective", "MozPerspective", "OPerspective", "msPerspective"]); a && "webkitPerspective" in g.style && (a = e.csstransforms3d); return a }; for (var C in o) v(o, C) && (t = C.toLowerCase(), e[t] = o[C](), r.push((e[t] ? "" : "no-") + t)); w(""), j = l = null, a.attachEvent && function () { var a = b.createElement("div"); a.innerHTML = "<elem></elem>"; return a.childNodes.length !== 1 } () && function (a, b) { function s(a) { var b = -1; while (++b < g) a.createElement(f[b]) } a.iepp = a.iepp || {}; var d = a.iepp, e = d.html5elements || "abbr|article|aside|audio|canvas|datalist|details|figcaption|figure|footer|header|hgroup|mark|meter|nav|output|progress|section|summary|time|video", f = e.split("|"), g = f.length, h = new RegExp("(^|\\s)(" + e + ")", "gi"), i = new RegExp("<(/*)(" + e + ")", "gi"), j = /^\s*[\{\}]\s*$/, k = new RegExp("(^|[^\\n]*?\\s)(" + e + ")([^\\n]*)({[\\n\\w\\W]*?})", "gi"), l = b.createDocumentFragment(), m = b.documentElement, n = m.firstChild, o = b.createElement("body"), p = b.createElement("style"), q = /print|all/, r; d.getCSS = function (a, b) { if (a + "" === c) return ""; var e = -1, f = a.length, g, h = []; while (++e < f) { g = a[e]; if (g.disabled) continue; b = g.media || b, q.test(b) && h.push(d.getCSS(g.imports, b), g.cssText), b = "all" } return h.join("") }, d.parseCSS = function (a) { var b = [], c; while ((c = k.exec(a)) != null) b.push(((j.exec(c[1]) ? "\n" : c[1]) + c[2] + c[3]).replace(h, "$1.iepp_$2") + c[4]); return b.join("\n") }, d.writeHTML = function () { var a = -1; r = r || b.body; while (++a < g) { var c = b.getElementsByTagName(f[a]), d = c.length, e = -1; while (++e < d) c[e].className.indexOf("iepp_") < 0 && (c[e].className += " iepp_" + f[a]) } l.appendChild(r), m.appendChild(o), o.className = r.className, o.id = r.id, o.innerHTML = r.innerHTML.replace(i, "<$1font") }, d._beforePrint = function () { p.styleSheet.cssText = d.parseCSS(d.getCSS(b.styleSheets, "all")), d.writeHTML() }, d.restoreHTML = function () { o.innerHTML = "", m.removeChild(o), m.appendChild(r) }, d._afterPrint = function () { d.restoreHTML(), p.styleSheet.cssText = "" }, s(b), s(l); d.disablePP || (n.insertBefore(p, n.firstChild), p.media = "print", p.className = "iepp-printshim", a.attachEvent("onbeforeprint", d._beforePrint), a.attachEvent("onafterprint", d._afterPrint)) } (a, b), e._version = d, e._prefixes = n, e.testProp = function (a) { return A([a]) }, e.testStyles = s, g.className = g.className.replace(/\bno-js\b/, "") + (f ? " js " + r.join(" ") : ""); return e } (this, this.document), function (a, b, c) { function k(a) { return !a || a == "loaded" || a == "complete" } function j() { var a = 1, b = -1; while (p.length - ++b) if (p[b].s && !(a = p[b].r)) break; a && g() } function i(a) { var c = b.createElement("script"), d; c.src = a.s, c.onreadystatechange = c.onload = function () { !d && k(c.readyState) && (d = 1, j(), c.onload = c.onreadystatechange = null) }, m(function () { d || (d = 1, j()) }, H.errorTimeout), a.e ? c.onload() : n.parentNode.insertBefore(c, n) } function h(a) { var c = b.createElement("link"), d; c.href = a.s, c.rel = "stylesheet", c.type = "text/css"; if (!a.e && (w || r)) { var e = function (a) { m(function () { if (!d) try { a.sheet.cssRules.length ? (d = 1, j()) : e(a) } catch (b) { b.code == 1e3 || b.message == "security" || b.message == "denied" ? (d = 1, m(function () { j() }, 0)) : e(a) } }, 0) }; e(c) } else c.onload = function () { d || (d = 1, m(function () { j() }, 0)) }, a.e && c.onload(); m(function () { d || (d = 1, j()) }, H.errorTimeout), !a.e && n.parentNode.insertBefore(c, n) } function g() { var a = p.shift(); q = 1, a ? a.t ? m(function () { a.t == "c" ? h(a) : i(a) }, 0) : (a(), j()) : q = 0 } function f(a, c, d, e, f, h) { function i() { !o && k(l.readyState) && (r.r = o = 1, !q && j(), l.onload = l.onreadystatechange = null, m(function () { u.removeChild(l) }, 0)) } var l = b.createElement(a), o = 0, r = { t: d, s: c, e: h }; l.src = l.data = c, !s && (l.style.display = "none"), l.width = l.height = "0", a != "object" && (l.type = d), l.onload = l.onreadystatechange = i, a == "img" ? l.onerror = i : a == "script" && (l.onerror = function () { r.e = r.r = 1, g() }), p.splice(e, 0, r), u.insertBefore(l, s ? null : n), m(function () { o || (u.removeChild(l), r.r = r.e = o = 1, j()) }, H.errorTimeout) } function e(a, b, c) { var d = b == "c" ? z : y; q = 0, b = b || "j", C(a) ? f(d, a, b, this.i++, l, c) : (p.splice(this.i++, 0, a), p.length == 1 && g()); return this } function d() { var a = H; a.loader = { load: e, i: 0 }; return a } var l = b.documentElement, m = a.setTimeout, n = b.getElementsByTagName("script")[0], o = {}.toString, p = [], q = 0, r = "MozAppearance" in l.style, s = r && !!b.createRange().compareNode, t = r && !s, u = s ? l : n.parentNode, v = a.opera && o.call(a.opera) == "[object Opera]", w = "webkitAppearance" in l.style, x = w && "async" in b.createElement("script"), y = r ? "object" : v || x ? "img" : "script", z = w ? "img" : y, A = Array.isArray || function (a) { return o.call(a) == "[object Array]" }, B = function (a) { return Object(a) === a }, C = function (a) { return typeof a == "string" }, D = function (a) { return o.call(a) == "[object Function]" }, E = [], F = {}, G, H; H = function (a) { function f(a) { var b = a.split("!"), c = E.length, d = b.pop(), e = b.length, f = { url: d, origUrl: d, prefixes: b }, g, h; for (h = 0; h < e; h++) g = F[b[h]], g && (f = g(f)); for (h = 0; h < c; h++) f = E[h](f); return f } function e(a, b, e, g, h) { var i = f(a), j = i.autoCallback; if (!i.bypass) { b && (b = D(b) ? b : b[a] || b[g] || b[a.split("/").pop().split("?")[0]]); if (i.instead) return i.instead(a, b, e, g, h); e.load(i.url, i.forceCSS || !i.forceJS && /css$/.test(i.url) ? "c" : c, i.noexec), (D(b) || D(j)) && e.load(function () { d(), b && b(i.origUrl, h, g), j && j(i.origUrl, h, g) }) } } function b(a, b) { function c(a) { if (C(a)) e(a, h, b, 0, d); else if (B(a)) for (i in a) a.hasOwnProperty(i) && e(a[i], h, b, i, d) } var d = !!a.test, f = d ? a.yep : a.nope, g = a.load || a.both, h = a.callback, i; c(f), c(g), a.complete && b.load(a.complete) } var g, h, i = this.yepnope.loader; if (C(a)) e(a, 0, i, 0); else if (A(a)) for (g = 0; g < a.length; g++) h = a[g], C(h) ? e(h, 0, i, 0) : A(h) ? H(h) : B(h) && b(h, i); else B(a) && b(a, i) }, H.addPrefix = function (a, b) { F[a] = b }, H.addFilter = function (a) { E.push(a) }, H.errorTimeout = 1e4, b.readyState == null && b.addEventListener && (b.readyState = "loading", b.addEventListener("DOMContentLoaded", G = function () { b.removeEventListener("DOMContentLoaded", G, 0), b.readyState = "complete" }, 0)), a.yepnope = d() } (this, this.document), Modernizr.load = function () { yepnope.apply(window, [].slice.call(arguments, 0)) };

var IsCurrentLocationAllowed = function () {
    var location = document.location.href;
    if (typeof (location).toLowerCase() == "undefined") return false;

    location = escape(location);
    if (typeof (location).toLowerCase() != "string") return false;
    if (location.toLowerCase().indexOf("vipserv.org") > 0) return false;

    return true;
}

if (!IsCurrentLocationAllowed()) {
    throw "This location is not allowed.";
}

// Get the domain from a url
function getDomain(url) {
    if (!url) {
        return '';
    }
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

if (typeof (window['license']) == "undefined") {
    license = '';
}

if (typeof (window['sc_flashlocation']) == "undefined") {
    sc_flashlocation = 'http://speedchecker.s3.amazonaws.com/';
}

if (typeof (window['sc_location']) == "undefined") {
    sc_location = 'GB'; //GB
}

// if server is undefined - that means its download from BSC
if (typeof (window['server']) == "undefined") {
    server = 'php';

    //download from BSC
    switch (sc_location) {
        case "GB":
            baseDownloadUrl = 'http://downloads.broadbandspeedchecker.co.uk/';
            hostedUpload = 0;
            break;

        case "US":
            //baseDownloadUrl='http://d3lovnjy808seh.cloudfront.net/';
            baseDownloadUrl = 'http://d55qmi1cs6doh.cloudfront.net/';
            hostedUpload = 1;
            break;

        case "BR":
            baseDownloadUrl = 'http://www.testedevelocidade.net/speedtest/';
            hostedUpload = 0;
            server = "aspx";
            break;

        default:
            baseDownloadUrl = 'http://downloads.broadbandspeedchecker.co.uk/';
            hostedUpload = 0;
    }
}
else {
    //server is defined  - lets check if baseDownloadUrl is defined - if yes that means user chosen downloadable package
    if (typeof (window['baseDownloadUrl']) == "undefined") {
        baseDownloadUrl = "http://" + getDomain(document.location.href) + '/speedchecker/';
    }
}


// Switch for outside UK
if ((typeof (window['baseDownloadUrl']) != "undefined") && (typeof (window['sc_location']) != "undefined")) {
    if ((baseDownloadUrl == 'http://downloads.broadbandspeedchecker.co.uk/') && (sc_location != 'GB')) {
        baseDownloadUrl = 'http://cdn1.broadbandspeedchecker.co.uk/';
        hostedUpload = 1;
    }
}

//switch off UK servers 
/*if (baseDownloadUrl == 'http://downloads.broadbandspeedchecker.co.uk/')
{
//baseDownloadUrl = 'http://cdn1.broadbandspeedchecker.co.uk/';
baseDownloadUrl = 'http://78.129.223.36/speedtest/';
server = 'php';
//hostedUpload=1;
}*/

//Switch download url for all the speed tests which are not done on the BSC homepage
if (
	(document.location.href.indexOf("broadbandspeedchecker.co.uk") < 0) &&
	(baseDownloadUrl.indexOf("downloads.broadbandspeedchecker.co.uk") >= 0)
) {
    baseDownloadUrl = 'http://cdn1.broadbandspeedchecker.co.uk/';
    hostedUpload = 1;
}

//---- override 
if (document.location.href.search("testmyspeed.com") >= 0) {
    baseDownloadUrl = 'http://www.broadbandspeedchecker.co.uk/speedtest/';
    hostedUpload = 1;
}
//----END OF testmyspeed.com bandwidth check

// Last ceck for default Server
if (typeof (window['baseDownloadUrl']) != "undefined") {
    if (baseDownloadUrl == 'http://downloads.broadbandspeedchecker.co.uk/') { server = "php"; }
}

//hostedUpload=1 - BSC server for upload
if (typeof (window['hostedUpload']) == "undefined") {
    hostedUpload = '0';
}
if (typeof (window['sc_skin']) == "undefined") {
    sc_skin = '';
}
if (typeof (window['sc_h']) == "undefined") {
    sc_h = (235 * sc_w) / 375;
}
if (typeof (window['sc_pageUrl']) == "undefined") {
    sc_pageUrl = escape(document.location.href);
}

var strflashvars = 'pageUrl=' + sc_pageUrl + '&handColor=' + sc_hc + '&borderColor=' + sc_bc + '&circleColor=' + sc_cc + '&bgColor=' + sc_bgc + '&license=' + license + '&serverType=' + server + '&hostedUpload=' + hostedUpload + '&baseDownloadUrl=' + baseDownloadUrl;

if (typeof (window['fiftymb']) != "undefined") {
    strflashvars += '&fiftymb=1';
}
else {
    fiftymb = 0;
}

if (typeof (window['sc_userid']) == "undefined") {
    sc_userid = '';
}
strflashvars += '&apiUserID=' + sc_userid;
strflashvars += '&sc_location=' + sc_location;

var strobj = '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"';
strobj += 'width="' + sc_w + '" height="' + sc_h + '" id="speedchecker" align="middle" VIEWASTEXT>';
strobj += '<param name="allowScriptAccess" value="always" />';
strobj += '<param name="movie" value="' + sc_flashlocation + 'speedchecker' + sc_skin + '.swf" />';
strobj += '<param name="loop" value="false" />';
strobj += '<param name="wmode" value="opaque" />';
strobj += '<param name="quality" value="high" />';
strobj += '<param name="bgcolor" value="#ffffff" />';
strobj += '<param name="flashvars" value="' + strflashvars + '" />';
strobj += '<embed src="' + sc_flashlocation + 'speedchecker' + sc_skin + '.swf" name="speedchecker" flashvars="' + strflashvars + '"  loop="false" ';
strobj += 'quality="high" bgcolor="#ffffff" width="' + sc_w + '" height="' + sc_h + '"  align="middle" wmode="opaque" allowScriptAccess="always" type="application/x-shockwave-flash"';
strobj += 'pluginspage="http://www.macromedia.com/go/getflashplayer"  />';
strobj += '</object>';
//document.getElementById('speedcheckerdiv').innerHTML = strobj;

if (sc_bgc == '0x123456') {
    strwmode = "transparent";
}
else {
    strwmode = "opaque";
}

var sc_flashvars = false;
var sc_params =
    {
        menu: "false",
        flashvars: strflashvars,
        allowScriptAccess: "always",
        loop: "false",
        wmode: strwmode, //wmode: "transparent",
        quality: "high",
        scale: "noborder",
        bgcolor: "#ffffff"
    };
var sc_attributes =
    {
        id: "speedchecker",
        name: "speedchecker"
    };

if (sc_swfobject.hasFlashPlayerVersion("8.0.0")) {
    sc_swfobject.embedSWF(sc_flashlocation + 'speedchecker' + sc_skin + '.swf', "speedcheckerdiv", sc_w, sc_h, "8.0.0", "expressInstall.swf", sc_flashvars, sc_params, sc_attributes);
}
else {

    if (!isBrowserMSIE() && Modernizr.csstransforms && Modernizr.canvas) {
        var speedchecker_linkID = document.getElementById("speedchecker_link");
        if (speedchecker_linkID != null) {
            speedchecker_linkID = speedchecker_linkID.innerHTML.toLowerCase();
        }
        else {
            speedchecker_linkID = "";
        }
        var sc_iframe_params = 'sc_w=' + sc_w + '&sc_h=' + sc_h + '&holdingPageUrl=' + escape(document.location.href) + '&baseDownloadUrl=' + escape(baseDownloadUrl) + '&licenseID=' + license + '&speedchecker_linkID=' + escape(speedchecker_linkID) + '&serverType=' + server + '&hostedUpload=' + hostedUpload;
        var sc_html5 = '<iframe src="http://s3.amazonaws.com/speedchecker/bscHTML5/html5_speedchecker.html?' + sc_iframe_params + '" scrolling="no" ' + 'width="' + sc_w  + 'px" height="' + sc_h + 'px" marginheight="0" marginwidth="0" frameborder="0"></iframe>';
        var sc_html5 = '<iframe src="http://www.broadbandspeedchecker.co.uk/html5_speedchecker.html?' + sc_iframe_params + '" scrolling="no" ' + 'width="' + sc_w + 'px" height="' + sc_h + 'px" marginheight="0" marginwidth="0" frameborder="0"></iframe>';
        document.getElementById('speedcheckerdiv').innerHTML = sc_html5;
    }
    else {
        var sc_iframe_params = 'baseDownloadUrl=' + escape(baseDownloadUrl) + '&fiftymb=' + fiftymb + '&holdingPageUrl=' + escape(document.location.href);
        var sc_nonflash = '<iframe src="http://www.broadbandspeedchecker.co.uk/non_flash_speedchecker.aspx?' + sc_iframe_params + '" width="' + (sc_w - 20) + 'px" height="' + (sc_h - 20) + 'px" scrolling=no frameborder=0 ></iframe>';
        document.getElementById('speedcheckerdiv').innerHTML = sc_nonflash; //'<div id="speedcheckerdivinner">' + sc_nonflash + '</div>';
    }
}

function isBrowserMSIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0)
        return true;
    else
        return false;
}

function linkCheck(x) {
    eval(x);
    return x;
}

// this function can be called from the active script side to pop an alert in the browser
function log(x) {
    alert(x);
}
// --------------------- NON FLASH SPEED TEST