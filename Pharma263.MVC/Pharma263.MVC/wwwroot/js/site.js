if (function(n) {
function tt(n, t) {
    return function (i) {
        return r(n.call(this, i), t)
    }
}
function wt(n, t) {
    return function (i) {
        return this.lang().ordinal(n.call(this, i), t)
    }
}
function it() { }
function l(n) {
    s(this, n)
}
function v(n) {
    var t = n.years || n.year || n.y || 0
        , i = n.months || n.month || n.M || 0
        , r = n.weeks || n.week || n.w || 0
        , u = n.days || n.day || n.d || 0
        , f = n.hours || n.hour || n.h || 0
        , e = n.minutes || n.minute || n.m || 0
        , o = n.seconds || n.second || n.s || 0
        , s = n.milliseconds || n.millisecond || n.ms || 0;
    this._input = n;
    this._milliseconds = +s + 1e3 * o + 6e4 * e + 36e5 * f;
    this._days = +u + 7 * r;
    this._months = +i + 12 * t;
    this._data = {};
    this._bubble()
}
function s(n, t) {
    for (var i in t)
        t.hasOwnProperty(i) && (n[i] = t[i]);
    return n
}
function e(n) {
    return 0 > n ? Math.ceil(n) : Math.floor(n)
}
function r(n, t) {
    for (var i = n + ""; i.length < t;)
        i = "0" + i;
    return i
}
function y(n, i, r, u) {
    var s, h, o = i._milliseconds, f = i._days, e = i._months;
    o && n._d.setTime(+n._d + o * r);
    (f || e) && (s = n.minute(),
        h = n.hour());
    f && n.date(n.date() + f * r);
    e && n.month(n.month() + e * r);
    o && !u && t.updateOffset(n);
    (f || e) && (n.minute(s),
        n.hour(h))
}
function rt(n) {
    return "[object Array]" === Object.prototype.toString.call(n)
}
function ut(n, t) {
    for (var u = Math.min(n.length, t.length), f = Math.abs(n.length - t.length), r = 0, i = 0; u > i; i++)
        ~~n[i] != ~~t[i] && r++;
    return r + f
}
function h(n) {
    return n ? tr[n] || n.toLowerCase().replace(/(.)s$/, "$1") : n
}
function bt(n, t) {
    return t.abbr = n,
        o[n] || (o[n] = new it),
        o[n].set(t),
        o[n]
}
function kt(n) {
    delete o[n]
}
function u(n) {
    if (!n)
        return t.fn._lang;
    if (!o[n] && lt)
        try {
            require("./lang/" + n)
        } catch (i) {
            return t.fn._lang
        }
    return o[n] || t.fn._lang
}
function dt(n) {
    return n.match(/\[.*\]/) ? n.replace(/^\[|\]$/g, "") : n.replace(/\\/g, "")
}
function gt(n) {
    for (var i = n.match(at), t = 0, r = i.length; r > t; t++)
        i[t] = f[i[t]] ? f[i[t]] : dt(i[t]);
    return function (u) {
        var f = "";
        for (t = 0; r > t; t++)
            f += i[t] instanceof Function ? i[t].call(u, n) : i[t];
        return f
    }
}
function ft(n, t) {
    return t = et(t, n.lang()),
        nt[t] || (nt[t] = gt(t)),
        nt[t](n)
}
function et(n, t) {
    function i(n) {
        return t.longDateFormat(n) || n
    }
    for (var r = 5; r-- && (b.lastIndex = 0,
        b.test(n));)
        n = n.replace(b, i);
    return n
}
function ni(n, t) {
    switch (n) {
        case "DDDD":
            return yi;
        case "YYYY":
            return pi;
        case "YYYYY":
            return wi;
        case "S":
        case "SS":
        case "SSS":
        case "DDD":
            return vi;
        case "MMM":
        case "MMMM":
        case "dd":
        case "ddd":
        case "dddd":
            return bi;
        case "a":
        case "A":
            return u(t._l)._meridiemParse;
        case "X":
            return di;
        case "Z":
        case "ZZ":
            return k;
        case "T":
            return ki;
        case "MM":
        case "DD":
        case "YY":
        case "HH":
        case "hh":
        case "mm":
        case "ss":
        case "M":
        case "D":
        case "d":
        case "H":
        case "h":
        case "m":
        case "s":
            return ai;
        default:
            return new RegExp(n.replace("\\", ""))
    }
}
function ot(n) {
    var r = (k.exec(n) || [])[0]
        , t = (r + "").match(nr) || ["-", 0, 0]
        , i = +(60 * t[1]) + ~~t[2];
    return "+" === t[0] ? -i : i
}
function ti(n, t, i) {
    var f, r = i._a;
    switch (n) {
        case "M":
        case "MM":
            null != t && (r[1] = ~~t - 1);
            break;
        case "MMM":
        case "MMMM":
            f = u(i._l).monthsParse(t);
            null != f ? r[1] = f : i._isValid = !1;
            break;
        case "D":
        case "DD":
            null != t && (r[2] = ~~t);
            break;
        case "DDD":
        case "DDDD":
            null != t && (r[1] = 0,
                r[2] = ~~t);
            break;
        case "YY":
            r[0] = ~~t + (~~t > 68 ? 1900 : 2e3);
            break;
        case "YYYY":
        case "YYYYY":
            r[0] = ~~t;
            break;
        case "a":
        case "A":
            i._isPm = u(i._l).isPM(t);
            break;
        case "H":
        case "HH":
        case "h":
        case "hh":
            r[3] = ~~t;
            break;
        case "m":
        case "mm":
            r[4] = ~~t;
            break;
        case "s":
        case "ss":
            r[5] = ~~t;
            break;
        case "S":
        case "SS":
        case "SSS":
            r[6] = ~~(1e3 * ("0." + t));
            break;
        case "X":
            i._d = new Date(1e3 * parseFloat(t));
            break;
        case "Z":
        case "ZZ":
            i._useUTC = !0;
            i._tzm = ot(t)
    }
    null == t && (i._isValid = !1)
}
function p(n) {
    var i, r, u, t = [];
    if (!n._d) {
        for (u = ri(n),
            i = 0; 3 > i && null == n._a[i]; ++i)
            n._a[i] = t[i] = u[i];
        for (; 7 > i; i++)
            n._a[i] = t[i] = null == n._a[i] ? 2 === i ? 1 : 0 : n._a[i];
        t[3] += ~~((n._tzm || 0) / 60);
        t[4] += ~~((n._tzm || 0) % 60);
        r = new Date(0);
        n._useUTC ? (r.setUTCFullYear(t[0], t[1], t[2]),
            r.setUTCHours(t[3], t[4], t[5], t[6])) : (r.setFullYear(t[0], t[1], t[2]),
                r.setHours(t[3], t[4], t[5], t[6]));
        n._d = r
    }
}
function ii(n) {
    var t = n._i;
    n._d || (n._a = [t.years || t.year || t.y, t.months || t.month || t.M, t.days || t.day || t.d, t.hours || t.hour || t.h, t.minutes || t.minute || t.m, t.seconds || t.second || t.s, t.milliseconds || t.millisecond || t.ms],
        p(n))
}
function ri(n) {
    var t = new Date;
    return n._useUTC ? [t.getUTCFullYear(), t.getUTCMonth(), t.getUTCDate()] : [t.getFullYear(), t.getMonth(), t.getDate()]
}
function w(n) {
    var t, r, e, o = u(n._l), i = "" + n._i;
    for (e = et(n._f, o).match(at),
        n._a = [],
        t = 0; t < e.length; t++)
        r = (ni(e[t], n).exec(i) || [])[0],
            r && (i = i.slice(i.indexOf(r) + r.length)),
            f[e[t]] && ti(e[t], r, n);
    i && (n._il = i);
    n._isPm && n._a[3] < 12 && (n._a[3] += 12);
    n._isPm === !1 && 12 === n._a[3] && (n._a[3] = 0);
    p(n)
}
function ui(n) {
    for (var t, i, f, u, e = 99, r = 0; r < n._f.length; r++)
        t = s({}, n),
            t._f = n._f[r],
            w(t),
            i = new l(t),
            u = ut(t._a, i.toArray()),
            i._il && (u += i._il.length),
            e > u && (e = u,
                f = i);
    s(n, f)
}
function fi(n) {
    var t, i = n._i, r = gi.exec(i);
    if (r) {
        for (n._f = "YYYY-MM-DD" + (r[2] || " "),
            t = 0; 4 > t; t++)
            if (vt[t][1].exec(i)) {
                n._f += vt[t][0];
                break
            }
        k.exec(i) && (n._f += " Z");
        w(n)
    } else
        n._d = new Date(i)
}
function ei(t) {
    var i = t._i
        , r = ci.exec(i);
    i === n ? t._d = new Date : r ? t._d = new Date(+r[1]) : "string" == typeof i ? fi(t) : rt(i) ? (t._a = i.slice(0),
        p(t)) : i instanceof Date ? t._d = new Date(+i) : "object" == typeof i ? ii(t) : t._d = new Date(i)
}
function oi(n, t, i, r, u) {
    return u.relativeTime(t || 1, !!i, n, r)
}
function si(n, t, i) {
    var o = c(Math.abs(n) / 1e3)
        , u = c(o / 60)
        , f = c(u / 60)
        , r = c(f / 24)
        , s = c(r / 365)
        , e = 45 > o && ["s", o] || 1 === u && ["m"] || 45 > u && ["mm", u] || 1 === f && ["h"] || 22 > f && ["hh", f] || 1 === r && ["d"] || 25 >= r && ["dd", r] || 45 >= r && ["M"] || 345 > r && ["MM", c(r / 30)] || 1 === s && ["y"] || ["yy", s];
    return e[2] = t,
        e[3] = n > 0,
        e[4] = i,
        oi.apply({}, e)
}
function a(n, i, r) {
    var f, e = r - i, u = r - n.day();
    return u > e && (u -= 7),
        e - 7 > u && (u += 7),
        f = t(n).add("d", u),
    {
        week: Math.ceil(f.dayOfYear() / 7),
        year: f.year()
    }
}
function st(n) {
    var i = n._i
        , r = n._f;
    return null === i || "" === i ? null : ("string" == typeof i && (n._i = i = u().preparse(i)),
        t.isMoment(i) ? (n = s({}, i),
            n._d = new Date(+i._d)) : r ? rt(r) ? ui(n) : w(n) : ei(n),
        new l(n))
}
function ht(n, i) {
    t.fn[n] = t.fn[n + "s"] = function (n) {
        var r = this._isUTC ? "UTC" : "";
        return null != n ? (this._d["set" + r + i](n),
            t.updateOffset(this),
            this) : this._d["get" + r + i]()
    }
}
function hi(n) {
    t.duration.fn[n] = function () {
        return this._data[n]
    }
}
function ct(n, i) {
    t.duration.fn["as" + n] = function () {
        return +this / i
    }
}
for (var t, i, c = Math.round, o = {}, lt = "undefined" != typeof module && module.exports, ci = /^\/?Date\((\-?\d+)/i, li = /(\-)?(?:(\d*)\.)?(\d+)\:(\d+)\:(\d+)\.?(\d{3})?/, at = /(\[[^\[]*\])|(\\)?(Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|YYYYY|YYYY|YY|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|mm?|ss?|SS?S?|X|zz?|ZZ?|.)/g, b = /(\[[^\[]*\])|(\\)?(LT|LL?L?L?|l{1,4})/g, ai = /\d\d?/, vi = /\d{1,3}/, yi = /\d{3}/, pi = /\d{1,4}/, wi = /[+\-]?\d{1,6}/, bi = /[0-9]*['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+|[\u0600-\u06FF\/]+(\s*?[\u0600-\u06FF]+){1,2}/i, k = /Z|[\+\-]\d\d:?\d\d/i, ki = /T/i, di = /[\+\-]?\d+(\.\d{1,3})?/, gi = /^\s*\d{4}-\d\d-\d\d((T| )(\d\d(:\d\d(:\d\d(\.\d\d?\d?)?)?)?)?([\+\-]\d\d:?\d\d)?)?/, vt = [["HH:mm:ss.S", /(T| )\d\d:\d\d:\d\d\.\d{1,3}/], ["HH:mm:ss", /(T| )\d\d:\d\d:\d\d/], ["HH:mm", /(T| )\d\d:\d\d/], ["HH", /(T| )\d\d/]], nr = /([\+\-]|\d\d)/gi, d = "Date|Hours|Minutes|Seconds|Milliseconds".split("|"), g = {
    Milliseconds: 1,
    Seconds: 1e3,
    Minutes: 6e4,
    Hours: 36e5,
    Days: 864e5,
    Months: 2592e6,
    Years: 31536e6
}, tr = {
    ms: "millisecond",
    s: "second",
    m: "minute",
    h: "hour",
    d: "day",
    w: "week",
    W: "isoweek",
    M: "month",
    y: "year"
}, nt = {}, yt = "DDD w W M D d".split(" "), pt = "M D H h m s w W".split(" "), f = {
    M: function () {
        return this.month() + 1
    },
    MMM: function (n) {
        return this.lang().monthsShort(this, n)
    },
    MMMM: function (n) {
        return this.lang().months(this, n)
    },
    D: function () {
        return this.date()
    },
    DDD: function () {
        return this.dayOfYear()
    },
    d: function () {
        return this.day()
    },
    dd: function (n) {
        return this.lang().weekdaysMin(this, n)
    },
    ddd: function (n) {
        return this.lang().weekdaysShort(this, n)
    },
    dddd: function (n) {
        return this.lang().weekdays(this, n)
    },
    w: function () {
        return this.week()
    },
    W: function () {
        return this.isoWeek()
    },
    YY: function () {
        return r(this.year() % 100, 2)
    },
    YYYY: function () {
        return r(this.year(), 4)
    },
    YYYYY: function () {
        return r(this.year(), 5)
    },
    gg: function () {
        return r(this.weekYear() % 100, 2)
    },
    gggg: function () {
        return this.weekYear()
    },
    ggggg: function () {
        return r(this.weekYear(), 5)
    },
    GG: function () {
        return r(this.isoWeekYear() % 100, 2)
    },
    GGGG: function () {
        return this.isoWeekYear()
    },
    GGGGG: function () {
        return r(this.isoWeekYear(), 5)
    },
    e: function () {
        return this.weekday()
    },
    E: function () {
        return this.isoWeekday()
    },
    a: function () {
        return this.lang().meridiem(this.hours(), this.minutes(), !0)
    },
    A: function () {
        return this.lang().meridiem(this.hours(), this.minutes(), !1)
    },
    H: function () {
        return this.hours()
    },
    h: function () {
        return this.hours() % 12 || 12
    },
    m: function () {
        return this.minutes()
    },
    s: function () {
        return this.seconds()
    },
    S: function () {
        return ~~(this.milliseconds() / 100)
    },
    SS: function () {
        return r(~~(this.milliseconds() / 10), 2)
    },
    SSS: function () {
        return r(this.milliseconds(), 3)
    },
    Z: function () {
        var n = -this.zone()
            , t = "+";
        return 0 > n && (n = -n,
            t = "-"),
            t + r(~~(n / 60), 2) + ":" + r(~~n % 60, 2)
    },
    ZZ: function () {
        var n = -this.zone()
            , t = "+";
        return 0 > n && (n = -n,
            t = "-"),
            t + r(~~(10 * n / 6), 4)
    },
    z: function () {
        return this.zoneAbbr()
    },
    zz: function () {
        return this.zoneName()
    },
    X: function () {
        return this.unix()
    }
}; yt.length;)
    i = yt.pop(),
        f[i + "o"] = wt(f[i], i);
for (; pt.length;)
    i = pt.pop(),
        f[i + i] = tt(f[i], 2);
for (f.DDDD = tt(f.DDD, 3),
    s(it.prototype, {
        set: function (n) {
            var t, i;
            for (i in n)
                t = n[i],
                    "function" == typeof t ? this[i] = t : this["_" + i] = t
        },
        _months: "January_February_March_April_May_June_July_August_September_October_November_December".split("_"),
        months: function (n) {
            return this._months[n.month()]
        },
        _monthsShort: "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_"),
        monthsShort: function (n) {
            return this._monthsShort[n.month()]
        },
        monthsParse: function (n) {
            var i, r, u;
            for (this._monthsParse || (this._monthsParse = []),
                i = 0; 12 > i; i++)
                if (this._monthsParse[i] || (r = t.utc([2e3, i]),
                    u = "^" + this.months(r, "") + "|^" + this.monthsShort(r, ""),
                    this._monthsParse[i] = new RegExp(u.replace(".", ""), "i")),
                    this._monthsParse[i].test(n))
                    return i
        },
        _weekdays: "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"),
        weekdays: function (n) {
            return this._weekdays[n.day()]
        },
        _weekdaysShort: "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_"),
        weekdaysShort: function (n) {
            return this._weekdaysShort[n.day()]
        },
        _weekdaysMin: "Su_Mo_Tu_We_Th_Fr_Sa".split("_"),
        weekdaysMin: function (n) {
            return this._weekdaysMin[n.day()]
        },
        weekdaysParse: function (n) {
            var i, r, u;
            for (this._weekdaysParse || (this._weekdaysParse = []),
                i = 0; 7 > i; i++)
                if (this._weekdaysParse[i] || (r = t([2e3, 1]).day(i),
                    u = "^" + this.weekdays(r, "") + "|^" + this.weekdaysShort(r, "") + "|^" + this.weekdaysMin(r, ""),
                    this._weekdaysParse[i] = new RegExp(u.replace(".", ""), "i")),
                    this._weekdaysParse[i].test(n))
                    return i
        },
        _longDateFormat: {
            LT: "h:mm A",
            L: "MM/DD/YYYY",
            LL: "MMMM D YYYY",
            LLL: "MMMM D YYYY LT",
            LLLL: "dddd, MMMM D YYYY LT"
        },
        longDateFormat: function (n) {
            var t = this._longDateFormat[n];
            return !t && this._longDateFormat[n.toUpperCase()] && (t = this._longDateFormat[n.toUpperCase()].replace(/MMMM|MM|DD|dddd/g, function (n) {
                return n.slice(1)
            }),
                this._longDateFormat[n] = t),
                t
        },
        isPM: function (n) {
            return "p" === (n + "").toLowerCase().charAt(0)
        },
        _meridiemParse: /[ap]\.?m?\.?/i,
        meridiem: function (n, t, i) {
            return n > 11 ? i ? "pm" : "PM" : i ? "am" : "AM"
        },
        _calendar: {
            sameDay: "[Today at] LT",
            nextDay: "[Tomorrow at] LT",
            nextWeek: "dddd [at] LT",
            lastDay: "[Yesterday at] LT",
            lastWeek: "[Last] dddd [at] LT",
            sameElse: "L"
        },
        calendar: function (n, t) {
            var i = this._calendar[n];
            return "function" == typeof i ? i.apply(t) : i
        },
        _relativeTime: {
            future: "in %s",
            past: "%s ago",
            s: "a few seconds",
            m: "a minute",
            mm: "%d minutes",
            h: "an hour",
            hh: "%d hours",
            d: "a day",
            dd: "%d days",
            M: "a month",
            MM: "%d months",
            y: "a year",
            yy: "%d years"
        },
        relativeTime: function (n, t, i, r) {
            var u = this._relativeTime[i];
            return "function" == typeof u ? u(n, t, i, r) : u.replace(/%d/i, n)
        },
        pastFuture: function (n, t) {
            var i = this._relativeTime[n > 0 ? "future" : "past"];
            return "function" == typeof i ? i(t) : i.replace(/%s/i, t)
        },
        ordinal: function (n) {
            return this._ordinal.replace("%d", n)
        },
        _ordinal: "%d",
        preparse: function (n) {
            return n
        },
        postformat: function (n) {
            return n
        },
        week: function (n) {
            return a(n, this._week.dow, this._week.doy).week
        },
        _week: {
            dow: 0,
            doy: 6
        }
    }),
    t = function (n, t, i) {
        return st({
            _i: n,
            _f: t,
            _l: i,
            _isUTC: !1
        })
    }
    ,
    t.utc = function (n, t, i) {
        return st({
            _useUTC: !0,
            _isUTC: !0,
            _l: i,
            _i: n,
            _f: t
        }).utc()
    }
    ,
    t.unix = function (n) {
        return t(1e3 * n)
    }
    ,
    t.duration = function (n, i) {
        var u, e, o = t.isDuration(n), s = "number" == typeof n, f = o ? n._input : s ? {} : n, r = li.exec(n);
        return s ? i ? f[i] = n : f.milliseconds = n : r && (u = "-" === r[1] ? -1 : 1,
            f = {
                y: 0,
                d: ~~r[2] * u,
                h: ~~r[3] * u,
                m: ~~r[4] * u,
                s: ~~r[5] * u,
                ms: ~~r[6] * u
            }),
            e = new v(f),
            o && n.hasOwnProperty("_lang") && (e._lang = n._lang),
            e
    }
    ,
    t.version = "2.2.1",
    t.defaultFormat = "YYYY-MM-DDTHH:mm:ssZ",
    t.updateOffset = function () { }
    ,
    t.lang = function (n, i) {
        return n ? (n = n.toLowerCase(),
            n = n.replace("_", "-"),
            i ? bt(n, i) : null === i ? (kt(n),
                n = "en") : o[n] || u(n),
            t.duration.fn._lang = t.fn._lang = u(n),
            void 0) : t.fn._lang._abbr
    }
    ,
    t.langData = function (n) {
        return n && n._lang && n._lang._abbr && (n = n._lang._abbr),
            u(n)
    }
    ,
    t.isMoment = function (n) {
        return n instanceof l
    }
    ,
    t.isDuration = function (n) {
        return n instanceof v
    }
    ,
    s(t.fn = l.prototype, {
        clone: function () {
            return t(this)
        },
        valueOf: function () {
            return +this._d + 6e4 * (this._offset || 0)
        },
        unix: function () {
            return Math.floor(+this / 1e3)
        },
        toString: function () {
            return this.format("ddd MMM DD YYYY HH:mm:ss [GMT]ZZ")
        },
        toDate: function () {
            return this._offset ? new Date(+this) : this._d
        },
        toISOString: function () {
            return ft(t(this).utc(), "YYYY-MM-DD[T]HH:mm:ss.SSS[Z]")
        },
        toArray: function () {
            var n = this;
            return [n.year(), n.month(), n.date(), n.hours(), n.minutes(), n.seconds(), n.milliseconds()]
        },
        isValid: function () {
            return null == this._isValid && (this._isValid = this._a ? !ut(this._a, (this._isUTC ? t.utc(this._a) : t(this._a)).toArray()) : !isNaN(this._d.getTime())),
                !!this._isValid
        },
        invalidAt: function () {
            for (var i = this._a, r = (this._isUTC ? t.utc(this._a) : t(this._a)).toArray(), n = 6; n >= 0 && i[n] === r[n]; --n)
                ;
            return n
        },
        utc: function () {
            return this.zone(0)
        },
        local: function () {
            return this.zone(0),
                this._isUTC = !1,
                this
        },
        format: function (n) {
            var i = ft(this, n || t.defaultFormat);
            return this.lang().postformat(i)
        },
        add: function (n, i) {
            var r;
            return r = "string" == typeof n ? t.duration(+i, n) : t.duration(n, i),
                y(this, r, 1),
                this
        },
        subtract: function (n, i) {
            var r;
            return r = "string" == typeof n ? t.duration(+i, n) : t.duration(n, i),
                y(this, r, -1),
                this
        },
        diff: function (n, i, r) {
            var u, o, f = this._isUTC ? t(n).zone(this._offset || 0) : t(n).local(), s = 6e4 * (this.zone() - f.zone());
            return i = h(i),
                "year" === i || "month" === i ? (u = 432e5 * (this.daysInMonth() + f.daysInMonth()),
                    o = 12 * (this.year() - f.year()) + (this.month() - f.month()),
                    o += (this - t(this).startOf("month") - (f - t(f).startOf("month"))) / u,
                    o -= 6e4 * (this.zone() - t(this).startOf("month").zone() - (f.zone() - t(f).startOf("month").zone())) / u,
                    "year" === i && (o /= 12)) : (u = this - f,
                        o = "second" === i ? u / 1e3 : "minute" === i ? u / 6e4 : "hour" === i ? u / 36e5 : "day" === i ? (u - s) / 864e5 : "week" === i ? (u - s) / 6048e5 : u),
                r ? o : e(o)
        },
        from: function (n, i) {
            return t.duration(this.diff(n)).lang(this.lang()._abbr).humanize(!i)
        },
        fromNow: function (n) {
            return this.from(t(), n)
        },
        calendar: function () {
            var n = this.diff(t().zone(this.zone()).startOf("day"), "days", !0)
                , i = -6 > n ? "sameElse" : -1 > n ? "lastWeek" : 0 > n ? "lastDay" : 1 > n ? "sameDay" : 2 > n ? "nextDay" : 7 > n ? "nextWeek" : "sameElse";
            return this.format(this.lang().calendar(i, this))
        },
        isLeapYear: function () {
            var n = this.year();
            return 0 == n % 4 && 0 != n % 100 || 0 == n % 400
        },
        isDST: function () {
            return this.zone() < this.clone().month(0).zone() || this.zone() < this.clone().month(5).zone()
        },
        day: function (n) {
            var t = this._isUTC ? this._d.getUTCDay() : this._d.getDay();
            return null != n ? "string" == typeof n && (n = this.lang().weekdaysParse(n),
                "number" != typeof n) ? this : this.add({
                    d: n - t
                }) : t
        },
        month: function (n) {
            var i, r = this._isUTC ? "UTC" : "";
            return null != n ? "string" == typeof n && (n = this.lang().monthsParse(n),
                "number" != typeof n) ? this : (i = this.date(),
                    this.date(1),
                    this._d["set" + r + "Month"](n),
                    this.date(Math.min(i, this.daysInMonth())),
                    t.updateOffset(this),
                    this) : this._d["get" + r + "Month"]()
        },
        startOf: function (n) {
            switch (n = h(n)) {
                case "year":
                    this.month(0);
                case "month":
                    this.date(1);
                case "week":
                case "isoweek":
                case "day":
                    this.hours(0);
                case "hour":
                    this.minutes(0);
                case "minute":
                    this.seconds(0);
                case "second":
                    this.milliseconds(0)
            }
            return "week" === n ? this.weekday(0) : "isoweek" === n && this.isoWeekday(1),
                this
        },
        endOf: function (n) {
            return n = h(n),
                this.startOf(n).add("isoweek" === n ? "week" : n, 1).subtract("ms", 1)
        },
        isAfter: function (n, i) {
            return i = "undefined" != typeof i ? i : "millisecond",
                +this.clone().startOf(i) > +t(n).startOf(i)
        },
        isBefore: function (n, i) {
            return i = "undefined" != typeof i ? i : "millisecond",
                +this.clone().startOf(i) < +t(n).startOf(i)
        },
        isSame: function (n, i) {
            return i = "undefined" != typeof i ? i : "millisecond",
                +this.clone().startOf(i) == +t(n).startOf(i)
        },
        min: function (n) {
            return n = t.apply(null, arguments),
                this > n ? this : n
        },
        max: function (n) {
            return n = t.apply(null, arguments),
                n > this ? this : n
        },
        zone: function (n) {
            var i = this._offset || 0;
            return null == n ? this._isUTC ? i : this._d.getTimezoneOffset() : ("string" == typeof n && (n = ot(n)),
                Math.abs(n) < 16 && (n = 60 * n),
                this._offset = n,
                this._isUTC = !0,
                i !== n && y(this, t.duration(i - n, "m"), 1, !0),
                this)
        },
        zoneAbbr: function () {
            return this._isUTC ? "UTC" : ""
        },
        zoneName: function () {
            return this._isUTC ? "Coordinated Universal Time" : ""
        },
        hasAlignedHourOffset: function (n) {
            return n = n ? t(n).zone() : 0,
                0 == (this.zone() - n) % 60
        },
        daysInMonth: function () {
            return t.utc([this.year(), this.month() + 1, 0]).date()
        },
        dayOfYear: function (n) {
            var i = c((t(this).startOf("day") - t(this).startOf("year")) / 864e5) + 1;
            return null == n ? i : this.add("d", n - i)
        },
        weekYear: function (n) {
            var t = a(this, this.lang()._week.dow, this.lang()._week.doy).year;
            return null == n ? t : this.add("y", n - t)
        },
        isoWeekYear: function (n) {
            var t = a(this, 1, 4).year;
            return null == n ? t : this.add("y", n - t)
        },
        week: function (n) {
            var t = this.lang().week(this);
            return null == n ? t : this.add("d", 7 * (n - t))
        },
        isoWeek: function (n) {
            var t = a(this, 1, 4).week;
            return null == n ? t : this.add("d", 7 * (n - t))
        },
        weekday: function (n) {
            var t = (this._d.getDay() + 7 - this.lang()._week.dow) % 7;
            return null == n ? t : this.add("d", n - t)
        },
        isoWeekday: function (n) {
            return null == n ? this.day() || 7 : this.day(this.day() % 7 ? n : n - 7)
        },
        get: function (n) {
            return n = h(n),
                this[n.toLowerCase()]()
        },
        set: function (n, t) {
            n = h(n);
            this[n.toLowerCase()](t)
        },
        lang: function (t) {
            return t === n ? this._lang : (this._lang = u(t),
                this)
        }
    }),
    i = 0; i < d.length; i++)
    ht(d[i].toLowerCase().replace(/s$/, ""), d[i]);
ht("year", "FullYear");
t.fn.days = t.fn.day;
t.fn.months = t.fn.month;
t.fn.weeks = t.fn.week;
t.fn.isoWeeks = t.fn.isoWeek;
t.fn.toJSON = t.fn.toISOString;
s(t.duration.fn = v.prototype, {
    _bubble: function () {
        var t, i, r, o, s = this._milliseconds, u = this._days, f = this._months, n = this._data;
        n.milliseconds = s % 1e3;
        t = e(s / 1e3);
        n.seconds = t % 60;
        i = e(t / 60);
        n.minutes = i % 60;
        r = e(i / 60);
        n.hours = r % 24;
        u += e(r / 24);
        n.days = u % 30;
        f += e(u / 30);
        n.months = f % 12;
        o = e(f / 12);
        n.years = o
    },
    weeks: function () {
        return e(this.days() / 7)
    },
    valueOf: function () {
        return this._milliseconds + 864e5 * this._days + 2592e6 * (this._months % 12) + 31536e6 * ~~(this._months / 12)
    },
    humanize: function (n) {
        var i = +this
            , t = si(i, !n, this.lang());
        return n && (t = this.lang().pastFuture(i, t)),
            this.lang().postformat(t)
    },
    add: function (n, i) {
        var r = t.duration(n, i);
        return this._milliseconds += r._milliseconds,
            this._days += r._days,
            this._months += r._months,
            this._bubble(),
            this
    },
    subtract: function (n, i) {
        var r = t.duration(n, i);
        return this._milliseconds -= r._milliseconds,
            this._days -= r._days,
            this._months -= r._months,
            this._bubble(),
            this
    },
    get: function (n) {
        return n = h(n),
            this[n.toLowerCase() + "s"]()
    },
    as: function (n) {
        return n = h(n),
            this["as" + n.charAt(0).toUpperCase() + n.slice(1) + "s"]()
    },
    lang: t.fn.lang
});
for (i in g)
    g.hasOwnProperty(i) && (ct(i, g[i]),
        hi(i.toLowerCase()));
ct("Weeks", 6048e5);
t.duration.fn.asMonths = function () {
    return (+this - 31536e6 * this.years()) / 2592e6 + 12 * this.years()
}
    ;
t.lang("en", {
    ordinal: function (n) {
        var t = n % 10
            , i = 1 == ~~(n % 100 / 10) ? "th" : 1 === t ? "st" : 2 === t ? "nd" : 3 === t ? "rd" : "th";
        return n + i
    }
}),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ar-ma", {
            months: "يناير_فبراير_مارس_أبريل_ماي_يونيو_يوليوز_غشت_شتنبر_أكتوبر_نونبر_دجنبر".split("_"),
            monthsShort: "يناير_فبراير_مارس_أبريل_ماي_يونيو_يوليوز_غشت_شتنبر_أكتوبر_نونبر_دجنبر".split("_"),
            weekdays: "الأحد_الإتنين_الثلاثاء_الأربعاء_الخميس_الجمعة_السبت".split("_"),
            weekdaysShort: "احد_اتنين_ثلاثاء_اربعاء_خميس_جمعة_سبت".split("_"),
            weekdaysMin: "ح_ن_ث_ر_خ_ج_س".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[اليوم على الساعة] LT",
                nextDay: "[غدا على الساعة] LT",
                nextWeek: "dddd [على الساعة] LT",
                lastDay: "[أمس على الساعة] LT",
                lastWeek: "dddd [على الساعة] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "في %s",
                past: "منذ %s",
                s: "ثوان",
                m: "دقيقة",
                mm: "%d دقائق",
                h: "ساعة",
                hh: "%d ساعات",
                d: "يوم",
                dd: "%d أيام",
                M: "شهر",
                MM: "%d أشهر",
                y: "سنة",
                yy: "%d سنوات"
            },
            week: {
                dow: 6,
                doy: 12
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ar", {
            months: "يناير/ كانون الثاني_فبراير/ شباط_مارس/ آذار_أبريل/ نيسان_مايو/ أيار_يونيو/ حزيران_يوليو/ تموز_أغسطس/ آب_سبتمبر/ أيلول_أكتوبر/ تشرين الأول_نوفمبر/ تشرين الثاني_ديسمبر/ كانون الأول".split("_"),
            monthsShort: "يناير/ كانون الثاني_فبراير/ شباط_مارس/ آذار_أبريل/ نيسان_مايو/ أيار_يونيو/ حزيران_يوليو/ تموز_أغسطس/ آب_سبتمبر/ أيلول_أكتوبر/ تشرين الأول_نوفمبر/ تشرين الثاني_ديسمبر/ كانون الأول".split("_"),
            weekdays: "الأحد_الإثنين_الثلاثاء_الأربعاء_الخميس_الجمعة_السبت".split("_"),
            weekdaysShort: "الأحد_الإثنين_الثلاثاء_الأربعاء_الخميس_الجمعة_السبت".split("_"),
            weekdaysMin: "ح_ن_ث_ر_خ_ج_س".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[اليوم على الساعة] LT",
                nextDay: "[غدا على الساعة] LT",
                nextWeek: "dddd [على الساعة] LT",
                lastDay: "[أمس على الساعة] LT",
                lastWeek: "dddd [على الساعة] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "في %s",
                past: "منذ %s",
                s: "ثوان",
                m: "دقيقة",
                mm: "%d دقائق",
                h: "ساعة",
                hh: "%d ساعات",
                d: "يوم",
                dd: "%d أيام",
                M: "شهر",
                MM: "%d أشهر",
                y: "سنة",
                yy: "%d سنوات"
            },
            week: {
                dow: 6,
                doy: 12
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("bg", {
            months: "януари_февруари_март_април_май_юни_юли_август_септември_октомври_ноември_декември".split("_"),
            monthsShort: "янр_фев_мар_апр_май_юни_юли_авг_сеп_окт_ное_дек".split("_"),
            weekdays: "неделя_понеделник_вторник_сряда_четвъртък_петък_събота".split("_"),
            weekdaysShort: "нед_пон_вто_сря_чет_пет_съб".split("_"),
            weekdaysMin: "нд_пн_вт_ср_чт_пт_сб".split("_"),
            longDateFormat: {
                LT: "h:mm",
                L: "D.MM.YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Днес в] LT",
                nextDay: "[Утре в] LT",
                nextWeek: "dddd [в] LT",
                lastDay: "[Вчера в] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                        case 3:
                        case 6:
                            return "[В изминалата] dddd [в] LT";
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            return "[В изминалия] dddd [в] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "след %s",
                past: "преди %s",
                s: "няколко секунди",
                m: "минута",
                mm: "%d минути",
                h: "час",
                hh: "%d часа",
                d: "ден",
                dd: "%d дни",
                M: "месец",
                MM: "%d месеца",
                y: "година",
                yy: "%d години"
            },
            ordinal: function (n) {
                var t = n % 10
                    , i = n % 100;
                return 0 === n ? n + "-ев" : 0 === i ? n + "-ен" : i > 10 && 20 > i ? n + "-ти" : 1 === t ? n + "-ви" : 2 === t ? n + "-ри" : 7 === t || 8 === t ? n + "-ми" : n + "-ти"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (t) {
        function i(n, t, i) {
            return n + " " + f({
                mm: "munutenn",
                MM: "miz",
                dd: "devezh"
            }[i], n)
        }
        function u(n) {
            switch (r(n)) {
                case 1:
                case 3:
                case 4:
                case 5:
                case 9:
                    return n + " bloaz";
                default:
                    return n + " vloaz"
            }
        }
        function r(n) {
            return n > 9 ? r(n % 10) : n
        }
        function f(n, t) {
            return 2 === t ? e(n) : n
        }
        function e(t) {
            var i = {
                m: "v",
                b: "v",
                d: "z"
            };
            return i[t.charAt(0)] === n ? t : i[t.charAt(0)] + t.substring(1)
        }
        t.lang("br", {
            months: "Genver_C'hwevrer_Meurzh_Ebrel_Mae_Mezheven_Gouere_Eost_Gwengolo_Here_Du_Kerzu".split("_"),
            monthsShort: "Gen_C'hwe_Meu_Ebr_Mae_Eve_Gou_Eos_Gwe_Her_Du_Ker".split("_"),
            weekdays: "Sul_Lun_Meurzh_Merc'her_Yaou_Gwener_Sadorn".split("_"),
            weekdaysShort: "Sul_Lun_Meu_Mer_Yao_Gwe_Sad".split("_"),
            weekdaysMin: "Su_Lu_Me_Mer_Ya_Gw_Sa".split("_"),
            longDateFormat: {
                LT: "h[e]mm A",
                L: "DD/MM/YYYY",
                LL: "D [a viz] MMMM YYYY",
                LLL: "D [a viz] MMMM YYYY LT",
                LLLL: "dddd, D [a viz] MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Hiziv da] LT",
                nextDay: "[Warc'hoazh da] LT",
                nextWeek: "dddd [da] LT",
                lastDay: "[Dec'h da] LT",
                lastWeek: "dddd [paset da] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "a-benn %s",
                past: "%s 'zo",
                s: "un nebeud segondennoù",
                m: "ur vunutenn",
                mm: i,
                h: "un eur",
                hh: "%d eur",
                d: "un devezh",
                dd: i,
                M: "ur miz",
                MM: i,
                y: "ur bloaz",
                yy: u
            },
            ordinal: function (n) {
                var t = 1 === n ? "añ" : "vet";
                return n + t
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ca", {
            months: "Gener_Febrer_Març_Abril_Maig_Juny_Juliol_Agost_Setembre_Octubre_Novembre_Desembre".split("_"),
            monthsShort: "Gen._Febr._Mar._Abr._Mai._Jun._Jul._Ag._Set._Oct._Nov._Des.".split("_"),
            weekdays: "Diumenge_Dilluns_Dimarts_Dimecres_Dijous_Divendres_Dissabte".split("_"),
            weekdaysShort: "Dg._Dl._Dt._Dc._Dj._Dv._Ds.".split("_"),
            weekdaysMin: "Dg_Dl_Dt_Dc_Dj_Dv_Ds".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: function () {
                    return "[avui a " + (1 !== this.hours() ? "les" : "la") + "] LT"
                },
                nextDay: function () {
                    return "[demà a " + (1 !== this.hours() ? "les" : "la") + "] LT"
                },
                nextWeek: function () {
                    return "dddd [a " + (1 !== this.hours() ? "les" : "la") + "] LT"
                },
                lastDay: function () {
                    return "[ahir a " + (1 !== this.hours() ? "les" : "la") + "] LT"
                },
                lastWeek: function () {
                    return "[el] dddd [passat a " + (1 !== this.hours() ? "les" : "la") + "] LT"
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "en %s",
                past: "fa %s",
                s: "uns segons",
                m: "un minut",
                mm: "%d minuts",
                h: "una hora",
                hh: "%d hores",
                d: "un dia",
                dd: "%d dies",
                M: "un mes",
                MM: "%d mesos",
                y: "un any",
                yy: "%d anys"
            },
            ordinal: "%dº",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n) {
            return n > 1 && 5 > n && 1 != ~~(n / 10)
        }
        function t(n, t, r, u) {
            var f = n + " ";
            switch (r) {
                case "s":
                    return t || u ? "pár vteřin" : "pár vteřinami";
                case "m":
                    return t ? "minuta" : u ? "minutu" : "minutou";
                case "mm":
                    return t || u ? f + (i(n) ? "minuty" : "minut") : f + "minutami";
                case "h":
                    return t ? "hodina" : u ? "hodinu" : "hodinou";
                case "hh":
                    return t || u ? f + (i(n) ? "hodiny" : "hodin") : f + "hodinami";
                case "d":
                    return t || u ? "den" : "dnem";
                case "dd":
                    return t || u ? f + (i(n) ? "dny" : "dní") : f + "dny";
                case "M":
                    return t || u ? "měsíc" : "měsícem";
                case "MM":
                    return t || u ? f + (i(n) ? "měsíce" : "měsíců") : f + "měsíci";
                case "y":
                    return t || u ? "rok" : "rokem";
                case "yy":
                    return t || u ? f + (i(n) ? "roky" : "let") : f + "lety"
            }
        }
        var r = "leden_únor_březen_duben_květen_červen_červenec_srpen_září_říjen_listopad_prosinec".split("_")
            , u = "led_úno_bře_dub_kvě_čvn_čvc_srp_zář_říj_lis_pro".split("_");
        n.lang("cs", {
            months: r,
            monthsShort: u,
            monthsParse: function (n, t) {
                for (var r = [], i = 0; 12 > i; i++)
                    r[i] = new RegExp("^" + n[i] + "$|^" + t[i] + "$", "i");
                return r
            }(r, u),
            weekdays: "neděle_pondělí_úterý_středa_čtvrtek_pátek_sobota".split("_"),
            weekdaysShort: "ne_po_út_st_čt_pá_so".split("_"),
            weekdaysMin: "ne_po_út_st_čt_pá_so".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD.MM.YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[dnes v] LT",
                nextDay: "[zítra v] LT",
                nextWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[v neděli v] LT";
                        case 1:
                        case 2:
                            return "[v] dddd [v] LT";
                        case 3:
                            return "[ve středu v] LT";
                        case 4:
                            return "[ve čtvrtek v] LT";
                        case 5:
                            return "[v pátek v] LT";
                        case 6:
                            return "[v sobotu v] LT"
                    }
                },
                lastDay: "[včera v] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[minulou neděli v] LT";
                        case 1:
                        case 2:
                            return "[minulé] dddd [v] LT";
                        case 3:
                            return "[minulou středu v] LT";
                        case 4:
                        case 5:
                            return "[minulý] dddd [v] LT";
                        case 6:
                            return "[minulou sobotu v] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "za %s",
                past: "před %s",
                s: t,
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("cv", {
            months: "кăрлач_нарăс_пуш_ака_май_çĕртме_утă_çурла_авăн_юпа_чӳк_раштав".split("_"),
            monthsShort: "кăр_нар_пуш_ака_май_çĕр_утă_çур_ав_юпа_чӳк_раш".split("_"),
            weekdays: "вырсарникун_тунтикун_ытларикун_юнкун_кĕçнерникун_эрнекун_шăматкун".split("_"),
            weekdaysShort: "выр_тун_ытл_юн_кĕç_эрн_шăм".split("_"),
            weekdaysMin: "вр_тн_ыт_юн_кç_эр_шм".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD-MM-YYYY",
                LL: "YYYY [çулхи] MMMM [уйăхĕн] D[-мĕшĕ]",
                LLL: "YYYY [çулхи] MMMM [уйăхĕн] D[-мĕшĕ], LT",
                LLLL: "dddd, YYYY [çулхи] MMMM [уйăхĕн] D[-мĕшĕ], LT"
            },
            calendar: {
                sameDay: "[Паян] LT [сехетре]",
                nextDay: "[Ыран] LT [сехетре]",
                lastDay: "[Ĕнер] LT [сехетре]",
                nextWeek: "[Çитес] dddd LT [сехетре]",
                lastWeek: "[Иртнĕ] dddd LT [сехетре]",
                sameElse: "L"
            },
            relativeTime: {
                future: function (n) {
                    var t = /сехет$/i.exec(n) ? "рен" : /çул$/i.exec(n) ? "тан" : "ран";
                    return n + t
                },
                past: "%s каялла",
                s: "пĕр-ик çеккунт",
                m: "пĕр минут",
                mm: "%d минут",
                h: "пĕр сехет",
                hh: "%d сехет",
                d: "пĕр кун",
                dd: "%d кун",
                M: "пĕр уйăх",
                MM: "%d уйăх",
                y: "пĕр çул",
                yy: "%d çул"
            },
            ordinal: "%d-мĕш",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("da", {
            months: "januar_februar_marts_april_maj_juni_juli_august_september_oktober_november_december".split("_"),
            monthsShort: "jan_feb_mar_apr_maj_jun_jul_aug_sep_okt_nov_dec".split("_"),
            weekdays: "søndag_mandag_tirsdag_onsdag_torsdag_fredag_lørdag".split("_"),
            weekdaysShort: "søn_man_tir_ons_tor_fre_lør".split("_"),
            weekdaysMin: "sø_ma_ti_on_to_fr_lø".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D. MMMM, YYYY LT"
            },
            calendar: {
                sameDay: "[I dag kl.] LT",
                nextDay: "[I morgen kl.] LT",
                nextWeek: "dddd [kl.] LT",
                lastDay: "[I går kl.] LT",
                lastWeek: "[sidste] dddd [kl] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "om %s",
                past: "%s siden",
                s: "få sekunder",
                m: "et minut",
                mm: "%d minutter",
                h: "en time",
                hh: "%d timer",
                d: "en dag",
                dd: "%d dage",
                M: "en måned",
                MM: "%d måneder",
                y: "et år",
                yy: "%d år"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i) {
            var r = {
                m: ["eine Minute", "einer Minute"],
                h: ["eine Stunde", "einer Stunde"],
                d: ["ein Tag", "einem Tag"],
                dd: [n + " Tage", n + " Tagen"],
                M: ["ein Monat", "einem Monat"],
                MM: [n + " Monate", n + " Monaten"],
                y: ["ein Jahr", "einem Jahr"],
                yy: [n + " Jahre", n + " Jahren"]
            };
            return t ? r[i][0] : r[i][1]
        }
        n.lang("de", {
            months: "Januar_Februar_März_April_Mai_Juni_Juli_August_September_Oktober_November_Dezember".split("_"),
            monthsShort: "Jan._Febr._Mrz._Apr._Mai_Jun._Jul._Aug._Sept._Okt._Nov._Dez.".split("_"),
            weekdays: "Sonntag_Montag_Dienstag_Mittwoch_Donnerstag_Freitag_Samstag".split("_"),
            weekdaysShort: "So._Mo._Di._Mi._Do._Fr._Sa.".split("_"),
            weekdaysMin: "So_Mo_Di_Mi_Do_Fr_Sa".split("_"),
            longDateFormat: {
                LT: "H:mm [Uhr]",
                L: "DD.MM.YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd, D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Heute um] LT",
                sameElse: "L",
                nextDay: "[Morgen um] LT",
                nextWeek: "dddd [um] LT",
                lastDay: "[Gestern um] LT",
                lastWeek: "[letzten] dddd [um] LT"
            },
            relativeTime: {
                future: "in %s",
                past: "vor %s",
                s: "ein paar Sekunden",
                m: t,
                mm: "%d Minuten",
                h: t,
                hh: "%d Stunden",
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("el", {
            monthsNominativeEl: "Ιανουάριος_Φεβρουάριος_Μάρτιος_Απρίλιος_Μάιος_Ιούνιος_Ιούλιος_Αύγουστος_Σεπτέμβριος_Οκτώβριος_Νοέμβριος_Δεκέμβριος".split("_"),
            monthsGenitiveEl: "Ιανουαρίου_Φεβρουαρίου_Μαρτίου_Απριλίου_Μαΐου_Ιουνίου_Ιουλίου_Αυγούστου_Σεπτεμβρίου_Οκτωβρίου_Νοεμβρίου_Δεκεμβρίου".split("_"),
            months: function (n, t) {
                return /D/.test(t.substring(0, t.indexOf("MMMM"))) ? this._monthsGenitiveEl[n.month()] : this._monthsNominativeEl[n.month()]
            },
            monthsShort: "Ιαν_Φεβ_Μαρ_Απρ_Μαϊ_Ιουν_Ιουλ_Αυγ_Σεπ_Οκτ_Νοε_Δεκ".split("_"),
            weekdays: "Κυριακή_Δευτέρα_Τρίτη_Τετάρτη_Πέμπτη_Παρασκευή_Σάββατο".split("_"),
            weekdaysShort: "Κυρ_Δευ_Τρι_Τετ_Πεμ_Παρ_Σαβ".split("_"),
            weekdaysMin: "Κυ_Δε_Τρ_Τε_Πε_Πα_Σα".split("_"),
            meridiem: function (n, t, i) {
                return n > 11 ? i ? "μμ" : "ΜΜ" : i ? "πμ" : "ΠΜ"
            },
            longDateFormat: {
                LT: "h:mm A",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendarEl: {
                sameDay: "[Σήμερα {}] LT",
                nextDay: "[Αύριο {}] LT",
                nextWeek: "dddd [{}] LT",
                lastDay: "[Χθες {}] LT",
                lastWeek: "[την προηγούμενη] dddd [{}] LT",
                sameElse: "L"
            },
            calendar: function (n, t) {
                var i = this._calendarEl[n]
                    , r = t && t.hours();
                return i.replace("{}", 1 == r % 12 ? "στη" : "στις")
            },
            relativeTime: {
                future: "σε %s",
                past: "%s πριν",
                s: "δευτερόλεπτα",
                m: "ένα λεπτό",
                mm: "%d λεπτά",
                h: "μία ώρα",
                hh: "%d ώρες",
                d: "μία μέρα",
                dd: "%d μέρες",
                M: "ένας μήνας",
                MM: "%d μήνες",
                y: "ένας χρόνος",
                yy: "%d χρόνια"
            },
            ordinal: function (n) {
                return n + "η"
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("en-ca", {
            months: "January_February_March_April_May_June_July_August_September_October_November_December".split("_"),
            monthsShort: "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_"),
            weekdays: "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"),
            weekdaysShort: "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_"),
            weekdaysMin: "Su_Mo_Tu_We_Th_Fr_Sa".split("_"),
            longDateFormat: {
                LT: "h:mm A",
                L: "YYYY-MM-DD",
                LL: "D MMMM, YYYY",
                LLL: "D MMMM, YYYY LT",
                LLLL: "dddd, D MMMM, YYYY LT"
            },
            calendar: {
                sameDay: "[Today at] LT",
                nextDay: "[Tomorrow at] LT",
                nextWeek: "dddd [at] LT",
                lastDay: "[Yesterday at] LT",
                lastWeek: "[Last] dddd [at] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "in %s",
                past: "%s ago",
                s: "a few seconds",
                m: "a minute",
                mm: "%d minutes",
                h: "an hour",
                hh: "%d hours",
                d: "a day",
                dd: "%d days",
                M: "a month",
                MM: "%d months",
                y: "a year",
                yy: "%d years"
            },
            ordinal: function (n) {
                var t = n % 10
                    , i = 1 == ~~(n % 100 / 10) ? "th" : 1 === t ? "st" : 2 === t ? "nd" : 3 === t ? "rd" : "th";
                return n + i
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("en-gb", {
            months: "January_February_March_April_May_June_July_August_September_October_November_December".split("_"),
            monthsShort: "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_"),
            weekdays: "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"),
            weekdaysShort: "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_"),
            weekdaysMin: "Su_Mo_Tu_We_Th_Fr_Sa".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Today at] LT",
                nextDay: "[Tomorrow at] LT",
                nextWeek: "dddd [at] LT",
                lastDay: "[Yesterday at] LT",
                lastWeek: "[Last] dddd [at] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "in %s",
                past: "%s ago",
                s: "a few seconds",
                m: "a minute",
                mm: "%d minutes",
                h: "an hour",
                hh: "%d hours",
                d: "a day",
                dd: "%d days",
                M: "a month",
                MM: "%d months",
                y: "a year",
                yy: "%d years"
            },
            ordinal: function (n) {
                var t = n % 10
                    , i = 1 == ~~(n % 100 / 10) ? "th" : 1 === t ? "st" : 2 === t ? "nd" : 3 === t ? "rd" : "th";
                return n + i
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("eo", {
            months: "januaro_februaro_marto_aprilo_majo_junio_julio_aŭgusto_septembro_oktobro_novembro_decembro".split("_"),
            monthsShort: "jan_feb_mar_apr_maj_jun_jul_aŭg_sep_okt_nov_dec".split("_"),
            weekdays: "Dimanĉo_Lundo_Mardo_Merkredo_Ĵaŭdo_Vendredo_Sabato".split("_"),
            weekdaysShort: "Dim_Lun_Mard_Merk_Ĵaŭ_Ven_Sab".split("_"),
            weekdaysMin: "Di_Lu_Ma_Me_Ĵa_Ve_Sa".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "YYYY-MM-DD",
                LL: "D[-an de] MMMM, YYYY",
                LLL: "D[-an de] MMMM, YYYY LT",
                LLLL: "dddd, [la] D[-an de] MMMM, YYYY LT"
            },
            meridiem: function (n, t, i) {
                return n > 11 ? i ? "p.t.m." : "P.T.M." : i ? "a.t.m." : "A.T.M."
            },
            calendar: {
                sameDay: "[Hodiaŭ je] LT",
                nextDay: "[Morgaŭ je] LT",
                nextWeek: "dddd [je] LT",
                lastDay: "[Hieraŭ je] LT",
                lastWeek: "[pasinta] dddd [je] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "je %s",
                past: "antaŭ %s",
                s: "sekundoj",
                m: "minuto",
                mm: "%d minutoj",
                h: "horo",
                hh: "%d horoj",
                d: "tago",
                dd: "%d tagoj",
                M: "monato",
                MM: "%d monatoj",
                y: "jaro",
                yy: "%d jaroj"
            },
            ordinal: "%da",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("es", {
            months: "enero_febrero_marzo_abril_mayo_junio_julio_agosto_septiembre_octubre_noviembre_diciembre".split("_"),
            monthsShort: "ene._feb._mar._abr._may._jun._jul._ago._sep._oct._nov._dic.".split("_"),
            weekdays: "domingo_lunes_martes_miércoles_jueves_viernes_sábado".split("_"),
            weekdaysShort: "dom._lun._mar._mié._jue._vie._sáb.".split("_"),
            weekdaysMin: "Do_Lu_Ma_Mi_Ju_Vi_Sá".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD/MM/YYYY",
                LL: "D [de] MMMM [de] YYYY",
                LLL: "D [de] MMMM [de] YYYY LT",
                LLLL: "dddd, D [de] MMMM [de] YYYY LT"
            },
            calendar: {
                sameDay: function () {
                    return "[hoy a la" + (1 !== this.hours() ? "s" : "") + "] LT"
                },
                nextDay: function () {
                    return "[mañana a la" + (1 !== this.hours() ? "s" : "") + "] LT"
                },
                nextWeek: function () {
                    return "dddd [a la" + (1 !== this.hours() ? "s" : "") + "] LT"
                },
                lastDay: function () {
                    return "[ayer a la" + (1 !== this.hours() ? "s" : "") + "] LT"
                },
                lastWeek: function () {
                    return "[el] dddd [pasado a la" + (1 !== this.hours() ? "s" : "") + "] LT"
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "en %s",
                past: "hace %s",
                s: "unos segundos",
                m: "un minuto",
                mm: "%d minutos",
                h: "una hora",
                hh: "%d horas",
                d: "un día",
                dd: "%d días",
                M: "un mes",
                MM: "%d meses",
                y: "un año",
                yy: "%d años"
            },
            ordinal: "%dº",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i, r) {
            return r || t ? "paari sekundi" : "paar sekundit"
        }
        n.lang("et", {
            months: "jaanuar_veebruar_märts_aprill_mai_juuni_juuli_august_september_oktoober_november_detsember".split("_"),
            monthsShort: "jaan_veebr_märts_apr_mai_juuni_juuli_aug_sept_okt_nov_dets".split("_"),
            weekdays: "pühapäev_esmaspäev_teisipäev_kolmapäev_neljapäev_reede_laupäev".split("_"),
            weekdaysShort: "P_E_T_K_N_R_L".split("_"),
            weekdaysMin: "P_E_T_K_N_R_L".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD.MM.YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd, D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Täna,] LT",
                nextDay: "[Homme,] LT",
                nextWeek: "[Järgmine] dddd LT",
                lastDay: "[Eile,] LT",
                lastWeek: "[Eelmine] dddd LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s pärast",
                past: "%s tagasi",
                s: t,
                m: "minut",
                mm: "%d minutit",
                h: "tund",
                hh: "%d tundi",
                d: "päev",
                dd: "%d päeva",
                M: "kuu",
                MM: "%d kuud",
                y: "aasta",
                yy: "%d aastat"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("eu", {
            months: "urtarrila_otsaila_martxoa_apirila_maiatza_ekaina_uztaila_abuztua_iraila_urria_azaroa_abendua".split("_"),
            monthsShort: "urt._ots._mar._api._mai._eka._uzt._abu._ira._urr._aza._abe.".split("_"),
            weekdays: "igandea_astelehena_asteartea_asteazkena_osteguna_ostirala_larunbata".split("_"),
            weekdaysShort: "ig._al._ar._az._og._ol._lr.".split("_"),
            weekdaysMin: "ig_al_ar_az_og_ol_lr".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "YYYY-MM-DD",
                LL: "YYYY[ko] MMMM[ren] D[a]",
                LLL: "YYYY[ko] MMMM[ren] D[a] LT",
                LLLL: "dddd, YYYY[ko] MMMM[ren] D[a] LT",
                l: "YYYY-M-D",
                ll: "YYYY[ko] MMM D[a]",
                lll: "YYYY[ko] MMM D[a] LT",
                llll: "ddd, YYYY[ko] MMM D[a] LT"
            },
            calendar: {
                sameDay: "[gaur] LT[etan]",
                nextDay: "[bihar] LT[etan]",
                nextWeek: "dddd LT[etan]",
                lastDay: "[atzo] LT[etan]",
                lastWeek: "[aurreko] dddd LT[etan]",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s barru",
                past: "duela %s",
                s: "segundo batzuk",
                m: "minutu bat",
                mm: "%d minutu",
                h: "ordu bat",
                hh: "%d ordu",
                d: "egun bat",
                dd: "%d egun",
                M: "hilabete bat",
                MM: "%d hilabete",
                y: "urte bat",
                yy: "%d urte"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = {
            1: "۱",
            2: "۲",
            3: "۳",
            4: "۴",
            5: "۵",
            6: "۶",
            7: "۷",
            8: "۸",
            9: "۹",
            0: "۰"
        }
            , i = {
                "۱": "1",
                "۲": "2",
                "۳": "3",
                "۴": "4",
                "۵": "5",
                "۶": "6",
                "۷": "7",
                "۸": "8",
                "۹": "9",
                "۰": "0"
            };
        n.lang("fa", {
            months: "ژانویه_فوریه_مارس_آوریل_مه_ژوئن_ژوئیه_اوت_سپتامبر_اکتبر_نوامبر_دسامبر".split("_"),
            monthsShort: "ژانویه_فوریه_مارس_آوریل_مه_ژوئن_ژوئیه_اوت_سپتامبر_اکتبر_نوامبر_دسامبر".split("_"),
            weekdays: "یک‌شنبه_دوشنبه_سه‌شنبه_چهارشنبه_پنج‌شنبه_جمعه_شنبه".split("_"),
            weekdaysShort: "یک‌شنبه_دوشنبه_سه‌شنبه_چهارشنبه_پنج‌شنبه_جمعه_شنبه".split("_"),
            weekdaysMin: "ی_د_س_چ_پ_ج_ش".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            meridiem: function (n) {
                return 12 > n ? "قبل از ظهر" : "بعد از ظهر"
            },
            calendar: {
                sameDay: "[امروز ساعت] LT",
                nextDay: "[فردا ساعت] LT",
                nextWeek: "dddd [ساعت] LT",
                lastDay: "[دیروز ساعت] LT",
                lastWeek: "dddd [پیش] [ساعت] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "در %s",
                past: "%s پیش",
                s: "چندین ثانیه",
                m: "یک دقیقه",
                mm: "%d دقیقه",
                h: "یک ساعت",
                hh: "%d ساعت",
                d: "یک روز",
                dd: "%d روز",
                M: "یک ماه",
                MM: "%d ماه",
                y: "یک سال",
                yy: "%d سال"
            },
            preparse: function (n) {
                return n.replace(/[۰-۹]/g, function (n) {
                    return i[n]
                }).replace(/،/g, ",")
            },
            postformat: function (n) {
                return n.replace(/\d/g, function (n) {
                    return t[n]
                }).replace(/,/g, "،")
            },
            ordinal: "%dم",
            week: {
                dow: 6,
                doy: 12
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i, u) {
            var f = "";
            switch (i) {
                case "s":
                    return u ? "muutaman sekunnin" : "muutama sekunti";
                case "m":
                    return u ? "minuutin" : "minuutti";
                case "mm":
                    f = u ? "minuutin" : "minuuttia";
                    break;
                case "h":
                    return u ? "tunnin" : "tunti";
                case "hh":
                    f = u ? "tunnin" : "tuntia";
                    break;
                case "d":
                    return u ? "päivän" : "päivä";
                case "dd":
                    f = u ? "päivän" : "päivää";
                    break;
                case "M":
                    return u ? "kuukauden" : "kuukausi";
                case "MM":
                    f = u ? "kuukauden" : "kuukautta";
                    break;
                case "y":
                    return u ? "vuoden" : "vuosi";
                case "yy":
                    f = u ? "vuoden" : "vuotta"
            }
            return r(n, u) + " " + f
        }
        function r(n, t) {
            return 10 > n ? t ? u[n] : i[n] : n
        }
        var i = "nolla yksi kaksi kolme neljä viisi kuusi seitsemän kahdeksan yhdeksän".split(" ")
            , u = ["nolla", "yhden", "kahden", "kolmen", "neljän", "viiden", "kuuden", i[7], i[8], i[9]];
        n.lang("fi", {
            months: "tammikuu_helmikuu_maaliskuu_huhtikuu_toukokuu_kesäkuu_heinäkuu_elokuu_syyskuu_lokakuu_marraskuu_joulukuu".split("_"),
            monthsShort: "tammi_helmi_maalis_huhti_touko_kesä_heinä_elo_syys_loka_marras_joulu".split("_"),
            weekdays: "sunnuntai_maanantai_tiistai_keskiviikko_torstai_perjantai_lauantai".split("_"),
            weekdaysShort: "su_ma_ti_ke_to_pe_la".split("_"),
            weekdaysMin: "su_ma_ti_ke_to_pe_la".split("_"),
            longDateFormat: {
                LT: "HH.mm",
                L: "DD.MM.YYYY",
                LL: "Do MMMM[ta] YYYY",
                LLL: "Do MMMM[ta] YYYY, [klo] LT",
                LLLL: "dddd, Do MMMM[ta] YYYY, [klo] LT",
                l: "D.M.YYYY",
                ll: "Do MMM YYYY",
                lll: "Do MMM YYYY, [klo] LT",
                llll: "ddd, Do MMM YYYY, [klo] LT"
            },
            calendar: {
                sameDay: "[tänään] [klo] LT",
                nextDay: "[huomenna] [klo] LT",
                nextWeek: "dddd [klo] LT",
                lastDay: "[eilen] [klo] LT",
                lastWeek: "[viime] dddd[na] [klo] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s päästä",
                past: "%s sitten",
                s: t,
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("fr-ca", {
            months: "janvier_février_mars_avril_mai_juin_juillet_août_septembre_octobre_novembre_décembre".split("_"),
            monthsShort: "janv._févr._mars_avr._mai_juin_juil._août_sept._oct._nov._déc.".split("_"),
            weekdays: "dimanche_lundi_mardi_mercredi_jeudi_vendredi_samedi".split("_"),
            weekdaysShort: "dim._lun._mar._mer._jeu._ven._sam.".split("_"),
            weekdaysMin: "Di_Lu_Ma_Me_Je_Ve_Sa".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "YYYY-MM-DD",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Aujourd'hui à] LT",
                nextDay: "[Demain à] LT",
                nextWeek: "dddd [à] LT",
                lastDay: "[Hier à] LT",
                lastWeek: "dddd [dernier à] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "dans %s",
                past: "il y a %s",
                s: "quelques secondes",
                m: "une minute",
                mm: "%d minutes",
                h: "une heure",
                hh: "%d heures",
                d: "un jour",
                dd: "%d jours",
                M: "un mois",
                MM: "%d mois",
                y: "un an",
                yy: "%d ans"
            },
            ordinal: function (n) {
                return n + (1 === n ? "er" : "")
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("fr", {
            months: "janvier_février_mars_avril_mai_juin_juillet_août_septembre_octobre_novembre_décembre".split("_"),
            monthsShort: "janv._févr._mars_avr._mai_juin_juil._août_sept._oct._nov._déc.".split("_"),
            weekdays: "dimanche_lundi_mardi_mercredi_jeudi_vendredi_samedi".split("_"),
            weekdaysShort: "dim._lun._mar._mer._jeu._ven._sam.".split("_"),
            weekdaysMin: "Di_Lu_Ma_Me_Je_Ve_Sa".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Aujourd'hui à] LT",
                nextDay: "[Demain à] LT",
                nextWeek: "dddd [à] LT",
                lastDay: "[Hier à] LT",
                lastWeek: "dddd [dernier à] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "dans %s",
                past: "il y a %s",
                s: "quelques secondes",
                m: "une minute",
                mm: "%d minutes",
                h: "une heure",
                hh: "%d heures",
                d: "un jour",
                dd: "%d jours",
                M: "un mois",
                MM: "%d mois",
                y: "un an",
                yy: "%d ans"
            },
            ordinal: function (n) {
                return n + (1 === n ? "er" : "")
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("gl", {
            months: "Xaneiro_Febreiro_Marzo_Abril_Maio_Xuño_Xullo_Agosto_Setembro_Outubro_Novembro_Decembro".split("_"),
            monthsShort: "Xan._Feb._Mar._Abr._Mai._Xuñ._Xul._Ago._Set._Out._Nov._Dec.".split("_"),
            weekdays: "Domingo_Luns_Martes_Mércores_Xoves_Venres_Sábado".split("_"),
            weekdaysShort: "Dom._Lun._Mar._Mér._Xov._Ven._Sáb.".split("_"),
            weekdaysMin: "Do_Lu_Ma_Mé_Xo_Ve_Sá".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: function () {
                    return "[hoxe " + (1 !== this.hours() ? "ás" : "á") + "] LT"
                },
                nextDay: function () {
                    return "[mañá " + (1 !== this.hours() ? "ás" : "á") + "] LT"
                },
                nextWeek: function () {
                    return "dddd [" + (1 !== this.hours() ? "ás" : "a") + "] LT"
                },
                lastDay: function () {
                    return "[onte " + (1 !== this.hours() ? "á" : "a") + "] LT"
                },
                lastWeek: function () {
                    return "[o] dddd [pasado " + (1 !== this.hours() ? "ás" : "a") + "] LT"
                },
                sameElse: "L"
            },
            relativeTime: {
                future: function (n) {
                    return "uns segundos" === n ? "nuns segundos" : "en " + n
                },
                past: "hai %s",
                s: "uns segundos",
                m: "un minuto",
                mm: "%d minutos",
                h: "unha hora",
                hh: "%d horas",
                d: "un día",
                dd: "%d días",
                M: "un mes",
                MM: "%d meses",
                y: "un ano",
                yy: "%d anos"
            },
            ordinal: "%dº",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("he", {
            months: "ינואר_פברואר_מרץ_אפריל_מאי_יוני_יולי_אוגוסט_ספטמבר_אוקטובר_נובמבר_דצמבר".split("_"),
            monthsShort: "ינו׳_פבר׳_מרץ_אפר׳_מאי_יוני_יולי_אוג׳_ספט׳_אוק׳_נוב׳_דצמ׳".split("_"),
            weekdays: "ראשון_שני_שלישי_רביעי_חמישי_שישי_שבת".split("_"),
            weekdaysShort: "א׳_ב׳_ג׳_ד׳_ה׳_ו׳_ש׳".split("_"),
            weekdaysMin: "א_ב_ג_ד_ה_ו_ש".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D [ב]MMMM YYYY",
                LLL: "D [ב]MMMM YYYY LT",
                LLLL: "dddd, D [ב]MMMM YYYY LT",
                l: "D/M/YYYY",
                ll: "D MMM YYYY",
                lll: "D MMM YYYY LT",
                llll: "ddd, D MMM YYYY LT"
            },
            calendar: {
                sameDay: "[היום ב־]LT",
                nextDay: "[מחר ב־]LT",
                nextWeek: "dddd [בשעה] LT",
                lastDay: "[אתמול ב־]LT",
                lastWeek: "[ביום] dddd [האחרון בשעה] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "בעוד %s",
                past: "לפני %s",
                s: "מספר שניות",
                m: "דקה",
                mm: "%d דקות",
                h: "שעה",
                hh: function (n) {
                    return 2 === n ? "שעתיים" : n + " שעות"
                },
                d: "יום",
                dd: function (n) {
                    return 2 === n ? "יומיים" : n + " ימים"
                },
                M: "חודש",
                MM: function (n) {
                    return 2 === n ? "חודשיים" : n + " חודשים"
                },
                y: "שנה",
                yy: function (n) {
                    return 2 === n ? "שנתיים" : n + " שנים"
                }
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = {
            1: "१",
            2: "२",
            3: "३",
            4: "४",
            5: "५",
            6: "६",
            7: "७",
            8: "८",
            9: "९",
            0: "०"
        }
            , i = {
                "१": "1",
                "२": "2",
                "३": "3",
                "४": "4",
                "५": "5",
                "६": "6",
                "७": "7",
                "८": "8",
                "९": "9",
                "०": "0"
            };
        n.lang("hi", {
            months: "जनवरी_फ़रवरी_मार्च_अप्रैल_मई_जून_जुलाई_अगस्त_सितम्बर_अक्टूबर_नवम्बर_दिसम्बर".split("_"),
            monthsShort: "जन._फ़र._मार्च_अप्रै._मई_जून_जुल._अग._सित._अक्टू._नव._दिस.".split("_"),
            weekdays: "रविवार_सोमवार_मंगलवार_बुधवार_गुरूवार_शुक्रवार_शनिवार".split("_"),
            weekdaysShort: "रवि_सोम_मंगल_बुध_गुरू_शुक्र_शनि".split("_"),
            weekdaysMin: "र_सो_मं_बु_गु_शु_श".split("_"),
            longDateFormat: {
                LT: "A h:mm बजे",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY, LT",
                LLLL: "dddd, D MMMM YYYY, LT"
            },
            calendar: {
                sameDay: "[आज] LT",
                nextDay: "[कल] LT",
                nextWeek: "dddd, LT",
                lastDay: "[कल] LT",
                lastWeek: "[पिछले] dddd, LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s में",
                past: "%s पहले",
                s: "कुछ ही क्षण",
                m: "एक मिनट",
                mm: "%d मिनट",
                h: "एक घंटा",
                hh: "%d घंटे",
                d: "एक दिन",
                dd: "%d दिन",
                M: "एक महीने",
                MM: "%d महीने",
                y: "एक वर्ष",
                yy: "%d वर्ष"
            },
            preparse: function (n) {
                return n.replace(/[१२३४५६७८९०]/g, function (n) {
                    return i[n]
                })
            },
            postformat: function (n) {
                return n.replace(/\d/g, function (n) {
                    return t[n]
                })
            },
            meridiem: function (n) {
                return 4 > n ? "रात" : 10 > n ? "सुबह" : 17 > n ? "दोपहर" : 20 > n ? "शाम" : "रात"
            },
            week: {
                dow: 0,
                doy: 6
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i) {
            var r = n + " ";
            switch (i) {
                case "m":
                    return t ? "jedna minuta" : "jedne minute";
                case "mm":
                    return r + (1 === n ? "minuta" : 2 === n || 3 === n || 4 === n ? "minute" : "minuta");
                case "h":
                    return t ? "jedan sat" : "jednog sata";
                case "hh":
                    return r + (1 === n ? "sat" : 2 === n || 3 === n || 4 === n ? "sata" : "sati");
                case "dd":
                    return r + (1 === n ? "dan" : "dana");
                case "MM":
                    return r + (1 === n ? "mjesec" : 2 === n || 3 === n || 4 === n ? "mjeseca" : "mjeseci");
                case "yy":
                    return r + (1 === n ? "godina" : 2 === n || 3 === n || 4 === n ? "godine" : "godina")
            }
        }
        n.lang("hr", {
            months: "sječanj_veljača_ožujak_travanj_svibanj_lipanj_srpanj_kolovoz_rujan_listopad_studeni_prosinac".split("_"),
            monthsShort: "sje._vel._ožu._tra._svi._lip._srp._kol._ruj._lis._stu._pro.".split("_"),
            weekdays: "nedjelja_ponedjeljak_utorak_srijeda_četvrtak_petak_subota".split("_"),
            weekdaysShort: "ned._pon._uto._sri._čet._pet._sub.".split("_"),
            weekdaysMin: "ne_po_ut_sr_če_pe_su".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD. MM. YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd, D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[danas u] LT",
                nextDay: "[sutra u] LT",
                nextWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[u] [nedjelju] [u] LT";
                        case 3:
                            return "[u] [srijedu] [u] LT";
                        case 6:
                            return "[u] [subotu] [u] LT";
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            return "[u] dddd [u] LT"
                    }
                },
                lastDay: "[jučer u] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                        case 3:
                            return "[prošlu] dddd [u] LT";
                        case 6:
                            return "[prošle] [subote] [u] LT";
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            return "[prošli] dddd [u] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "za %s",
                past: "prije %s",
                s: "par sekundi",
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: "dan",
                dd: t,
                M: "mjesec",
                MM: t,
                y: "godinu",
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i, r) {
            var u = n;
            switch (i) {
                case "s":
                    return r || t ? "néhány másodperc" : "néhány másodperce";
                case "m":
                    return "egy" + (r || t ? " perc" : " perce");
                case "mm":
                    return u + (r || t ? " perc" : " perce");
                case "h":
                    return "egy" + (r || t ? " óra" : " órája");
                case "hh":
                    return u + (r || t ? " óra" : " órája");
                case "d":
                    return "egy" + (r || t ? " nap" : " napja");
                case "dd":
                    return u + (r || t ? " nap" : " napja");
                case "M":
                    return "egy" + (r || t ? " hónap" : " hónapja");
                case "MM":
                    return u + (r || t ? " hónap" : " hónapja");
                case "y":
                    return "egy" + (r || t ? " év" : " éve");
                case "yy":
                    return u + (r || t ? " év" : " éve")
            }
            return ""
        }
        function i(n) {
            return (n ? "" : "[múlt] ") + "[" + r[this.day()] + "] LT[-kor]"
        }
        var r = "vasárnap hétfőn kedden szerdán csütörtökön pénteken szombaton".split(" ");
        n.lang("hu", {
            months: "január_február_március_április_május_június_július_augusztus_szeptember_október_november_december".split("_"),
            monthsShort: "jan_feb_márc_ápr_máj_jún_júl_aug_szept_okt_nov_dec".split("_"),
            weekdays: "vasárnap_hétfő_kedd_szerda_csütörtök_péntek_szombat".split("_"),
            weekdaysShort: "v_h_k_sze_cs_p_szo".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "YYYY.MM.DD.",
                LL: "YYYY. MMMM D.",
                LLL: "YYYY. MMMM D., LT",
                LLLL: "YYYY. MMMM D., dddd LT"
            },
            calendar: {
                sameDay: "[ma] LT[-kor]",
                nextDay: "[holnap] LT[-kor]",
                nextWeek: function () {
                    return i.call(this, !0)
                },
                lastDay: "[tegnap] LT[-kor]",
                lastWeek: function () {
                    return i.call(this, !1)
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "%s múlva",
                past: "%s",
                s: t,
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("id", {
            months: "Januari_Februari_Maret_April_Mei_Juni_Juli_Agustus_September_Oktober_November_Desember".split("_"),
            monthsShort: "Jan_Feb_Mar_Apr_Mei_Jun_Jul_Ags_Sep_Okt_Nov_Des".split("_"),
            weekdays: "Minggu_Senin_Selasa_Rabu_Kamis_Jumat_Sabtu".split("_"),
            weekdaysShort: "Min_Sen_Sel_Rab_Kam_Jum_Sab".split("_"),
            weekdaysMin: "Mg_Sn_Sl_Rb_Km_Jm_Sb".split("_"),
            longDateFormat: {
                LT: "HH.mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY [pukul] LT",
                LLLL: "dddd, D MMMM YYYY [pukul] LT"
            },
            meridiem: function (n) {
                return 11 > n ? "pagi" : 15 > n ? "siang" : 19 > n ? "sore" : "malam"
            },
            calendar: {
                sameDay: "[Hari ini pukul] LT",
                nextDay: "[Besok pukul] LT",
                nextWeek: "dddd [pukul] LT",
                lastDay: "[Kemarin pukul] LT",
                lastWeek: "dddd [lalu pukul] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "dalam %s",
                past: "%s yang lalu",
                s: "beberapa detik",
                m: "semenit",
                mm: "%d menit",
                h: "sejam",
                hh: "%d jam",
                d: "sehari",
                dd: "%d hari",
                M: "sebulan",
                MM: "%d bulan",
                y: "setahun",
                yy: "%d tahun"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n) {
            return 11 == n % 100 ? !0 : 1 == n % 10 ? !1 : !0
        }
        function t(n, t, r, u) {
            var f = n + " ";
            switch (r) {
                case "s":
                    return t || u ? "nokkrar sekúndur" : "nokkrum sekúndum";
                case "m":
                    return t ? "mínúta" : "mínútu";
                case "mm":
                    return i(n) ? f + (t || u ? "mínútur" : "mínútum") : t ? f + "mínúta" : f + "mínútu";
                case "hh":
                    return i(n) ? f + (t || u ? "klukkustundir" : "klukkustundum") : f + "klukkustund";
                case "d":
                    return t ? "dagur" : u ? "dag" : "degi";
                case "dd":
                    return i(n) ? t ? f + "dagar" : f + (u ? "daga" : "dögum") : t ? f + "dagur" : f + (u ? "dag" : "degi");
                case "M":
                    return t ? "mánuður" : u ? "mánuð" : "mánuði";
                case "MM":
                    return i(n) ? t ? f + "mánuðir" : f + (u ? "mánuði" : "mánuðum") : t ? f + "mánuður" : f + (u ? "mánuð" : "mánuði");
                case "y":
                    return t || u ? "ár" : "ári";
                case "yy":
                    return i(n) ? f + (t || u ? "ár" : "árum") : f + (t || u ? "ár" : "ári")
            }
        }
        n.lang("is", {
            months: "janúar_febrúar_mars_apríl_maí_júní_júlí_ágúst_september_október_nóvember_desember".split("_"),
            monthsShort: "jan_feb_mar_apr_maí_jún_júl_ágú_sep_okt_nóv_des".split("_"),
            weekdays: "sunnudagur_mánudagur_þriðjudagur_miðvikudagur_fimmtudagur_föstudagur_laugardagur".split("_"),
            weekdaysShort: "sun_mán_þri_mið_fim_fös_lau".split("_"),
            weekdaysMin: "Su_Má_Þr_Mi_Fi_Fö_La".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD/MM/YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY [kl.] LT",
                LLLL: "dddd, D. MMMM YYYY [kl.] LT"
            },
            calendar: {
                sameDay: "[í dag kl.] LT",
                nextDay: "[á morgun kl.] LT",
                nextWeek: "dddd [kl.] LT",
                lastDay: "[í gær kl.] LT",
                lastWeek: "[síðasta] dddd [kl.] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "eftir %s",
                past: "fyrir %s síðan",
                s: t,
                m: t,
                mm: t,
                h: "klukkustund",
                hh: t,
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("it", {
            months: "Gennaio_Febbraio_Marzo_Aprile_Maggio_Giugno_Luglio_Agosto_Settembre_Ottobre_Novembre_Dicembre".split("_"),
            monthsShort: "Gen_Feb_Mar_Apr_Mag_Giu_Lug_Ago_Set_Ott_Nov_Dic".split("_"),
            weekdays: "Domenica_Lunedì_Martedì_Mercoledì_Giovedì_Venerdì_Sabato".split("_"),
            weekdaysShort: "Dom_Lun_Mar_Mer_Gio_Ven_Sab".split("_"),
            weekdaysMin: "D_L_Ma_Me_G_V_S".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Oggi alle] LT",
                nextDay: "[Domani alle] LT",
                nextWeek: "dddd [alle] LT",
                lastDay: "[Ieri alle] LT",
                lastWeek: "[lo scorso] dddd [alle] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: function (n) {
                    return (/^[0-9].+$/.test(n) ? "tra" : "in") + " " + n
                },
                past: "%s fa",
                s: "secondi",
                m: "un minuto",
                mm: "%d minuti",
                h: "un'ora",
                hh: "%d ore",
                d: "un giorno",
                dd: "%d giorni",
                M: "un mese",
                MM: "%d mesi",
                y: "un anno",
                yy: "%d anni"
            },
            ordinal: "%dº",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ja", {
            months: "1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月".split("_"),
            monthsShort: "1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月".split("_"),
            weekdays: "日曜日_月曜日_火曜日_水曜日_木曜日_金曜日_土曜日".split("_"),
            weekdaysShort: "日_月_火_水_木_金_土".split("_"),
            weekdaysMin: "日_月_火_水_木_金_土".split("_"),
            longDateFormat: {
                LT: "Ah時m分",
                L: "YYYY/MM/DD",
                LL: "YYYY年M月D日",
                LLL: "YYYY年M月D日LT",
                LLLL: "YYYY年M月D日LT dddd"
            },
            meridiem: function (n) {
                return 12 > n ? "午前" : "午後"
            },
            calendar: {
                sameDay: "[今日] LT",
                nextDay: "[明日] LT",
                nextWeek: "[来週]dddd LT",
                lastDay: "[昨日] LT",
                lastWeek: "[前週]dddd LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s後",
                past: "%s前",
                s: "数秒",
                m: "1分",
                mm: "%d分",
                h: "1時間",
                hh: "%d時間",
                d: "1日",
                dd: "%d日",
                M: "1ヶ月",
                MM: "%dヶ月",
                y: "1年",
                yy: "%d年"
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t) {
            var i = {
                nominative: "იანვარი_თებერვალი_მარტი_აპრილი_მაისი_ივნისი_ივლისი_აგვისტო_სექტემბერი_ოქტომბერი_ნოემბერი_დეკემბერი".split("_"),
                accusative: "იანვარს_თებერვალს_მარტს_აპრილის_მაისს_ივნისს_ივლისს_აგვისტს_სექტემბერს_ოქტომბერს_ნოემბერს_დეკემბერს".split("_")
            }
                , r = /D[oD] *MMMM?/.test(t) ? "accusative" : "nominative";
            return i[r][n.month()]
        }
        function i(n, t) {
            var i = {
                nominative: "კვირა_ორშაბათი_სამშაბათი_ოთხშაბათი_ხუთშაბათი_პარასკევი_შაბათი".split("_"),
                accusative: "კვირას_ორშაბათს_სამშაბათს_ოთხშაბათს_ხუთშაბათს_პარასკევს_შაბათს".split("_")
            }
                , r = /(წინა|შემდეგ)/.test(t) ? "accusative" : "nominative";
            return i[r][n.day()]
        }
        n.lang("ka", {
            months: t,
            monthsShort: "იან_თებ_მარ_აპრ_მაი_ივნ_ივლ_აგვ_სექ_ოქტ_ნოე_დეკ".split("_"),
            weekdays: i,
            weekdaysShort: "კვი_ორშ_სამ_ოთხ_ხუთ_პარ_შაბ".split("_"),
            weekdaysMin: "კვ_ორ_სა_ოთ_ხუ_პა_შა".split("_"),
            longDateFormat: {
                LT: "h:mm A",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[დღეს] LT[-ზე]",
                nextDay: "[ხვალ] LT[-ზე]",
                lastDay: "[გუშინ] LT[-ზე]",
                nextWeek: "[შემდეგ] dddd LT[-ზე]",
                lastWeek: "[წინა] dddd LT-ზე",
                sameElse: "L"
            },
            relativeTime: {
                future: function (n) {
                    return /(წამი|წუთი|საათი|წელი)/.test(n) ? n.replace(/ი$/, "ში") : n + "ში"
                },
                past: function (n) {
                    return /(წამი|წუთი|საათი|დღე|თვე)/.test(n) ? n.replace(/(ი|ე)$/, "ის წინ") : /წელი/.test(n) ? n.replace(/წელი$/, "წლის წინ") : void 0
                },
                s: "რამდენიმე წამი",
                m: "წუთი",
                mm: "%d წუთი",
                h: "საათი",
                hh: "%d საათი",
                d: "დღე",
                dd: "%d დღე",
                M: "თვე",
                MM: "%d თვე",
                y: "წელი",
                yy: "%d წელი"
            },
            ordinal: function (n) {
                return 0 === n ? n : 1 === n ? n + "-ლი" : 20 > n || 100 >= n && 0 == n % 20 || 0 == n % 100 ? "მე-" + n : n + "-ე"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ko", {
            months: "1월_2월_3월_4월_5월_6월_7월_8월_9월_10월_11월_12월".split("_"),
            monthsShort: "1월_2월_3월_4월_5월_6월_7월_8월_9월_10월_11월_12월".split("_"),
            weekdays: "일요일_월요일_화요일_수요일_목요일_금요일_토요일".split("_"),
            weekdaysShort: "일_월_화_수_목_금_토".split("_"),
            weekdaysMin: "일_월_화_수_목_금_토".split("_"),
            longDateFormat: {
                LT: "A h시 mm분",
                L: "YYYY.MM.DD",
                LL: "YYYY년 MMMM D일",
                LLL: "YYYY년 MMMM D일 LT",
                LLLL: "YYYY년 MMMM D일 dddd LT"
            },
            meridiem: function (n) {
                return 12 > n ? "오전" : "오후"
            },
            calendar: {
                sameDay: "오늘 LT",
                nextDay: "내일 LT",
                nextWeek: "dddd LT",
                lastDay: "어제 LT",
                lastWeek: "지난주 dddd LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s 후",
                past: "%s 전",
                s: "몇초",
                ss: "%d초",
                m: "일분",
                mm: "%d분",
                h: "한시간",
                hh: "%d시간",
                d: "하루",
                dd: "%d일",
                M: "한달",
                MM: "%d달",
                y: "일년",
                yy: "%d년"
            },
            ordinal: "%d일"
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n, t, i) {
            var r = n.split("_");
            return i ? 1 == t % 10 && 11 !== t ? r[2] : r[3] : 1 == t % 10 && 11 !== t ? r[0] : r[1]
        }
        function t(n, t, u) {
            return n + " " + i(r[u], n, t)
        }
        var r = {
            mm: "minūti_minūtes_minūte_minūtes",
            hh: "stundu_stundas_stunda_stundas",
            dd: "dienu_dienas_diena_dienas",
            MM: "mēnesi_mēnešus_mēnesis_mēneši",
            yy: "gadu_gadus_gads_gadi"
        };
        n.lang("lv", {
            months: "janvāris_februāris_marts_aprīlis_maijs_jūnijs_jūlijs_augusts_septembris_oktobris_novembris_decembris".split("_"),
            monthsShort: "jan_feb_mar_apr_mai_jūn_jūl_aug_sep_okt_nov_dec".split("_"),
            weekdays: "svētdiena_pirmdiena_otrdiena_trešdiena_ceturtdiena_piektdiena_sestdiena".split("_"),
            weekdaysShort: "Sv_P_O_T_C_Pk_S".split("_"),
            weekdaysMin: "Sv_P_O_T_C_Pk_S".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "YYYY. [gada] D. MMMM",
                LLL: "YYYY. [gada] D. MMMM, LT",
                LLLL: "YYYY. [gada] D. MMMM, dddd, LT"
            },
            calendar: {
                sameDay: "[Šodien pulksten] LT",
                nextDay: "[Rīt pulksten] LT",
                nextWeek: "dddd [pulksten] LT",
                lastDay: "[Vakar pulksten] LT",
                lastWeek: "[Pagājušā] dddd [pulksten] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s vēlāk",
                past: "%s agrāk",
                s: "dažas sekundes",
                m: "minūti",
                mm: t,
                h: "stundu",
                hh: t,
                d: "dienu",
                dd: t,
                M: "mēnesi",
                MM: t,
                y: "gadu",
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ml", {
            months: "ജനുവരി_ഫെബ്രുവരി_മാർച്ച്_ഏപ്രിൽ_മേയ്_ജൂൺ_ജൂലൈ_ഓഗസ്റ്റ്_സെപ്റ്റംബർ_ഒക്ടോബർ_നവംബർ_ഡിസംബർ".split("_"),
            monthsShort: "ജനു._ഫെബ്രു._മാർ._ഏപ്രി._മേയ്_ജൂൺ_ജൂലൈ._ഓഗ._സെപ്റ്റ._ഒക്ടോ._നവം._ഡിസം.".split("_"),
            weekdays: "ഞായറാഴ്ച_തിങ്കളാഴ്ച_ചൊവ്വാഴ്ച_ബുധനാഴ്ച_വ്യാഴാഴ്ച_വെള്ളിയാഴ്ച_ശനിയാഴ്ച".split("_"),
            weekdaysShort: "ഞായർ_തിങ്കൾ_ചൊവ്വ_ബുധൻ_വ്യാഴം_വെള്ളി_ശനി".split("_"),
            weekdaysMin: "ഞാ_തി_ചൊ_ബു_വ്യാ_വെ_ശ".split("_"),
            longDateFormat: {
                LT: "A h:mm -നു",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY, LT",
                LLLL: "dddd, D MMMM YYYY, LT"
            },
            calendar: {
                sameDay: "[ഇന്ന്] LT",
                nextDay: "[നാളെ] LT",
                nextWeek: "dddd, LT",
                lastDay: "[ഇന്നലെ] LT",
                lastWeek: "[കഴിഞ്ഞ] dddd, LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s കഴിഞ്ഞ്",
                past: "%s മുൻപ്",
                s: "അൽപ നിമിഷങ്ങൾ",
                m: "ഒരു മിനിറ്റ്",
                mm: "%d മിനിറ്റ്",
                h: "ഒരു മണിക്കൂർ",
                hh: "%d മണിക്കൂർ",
                d: "ഒരു ദിവസം",
                dd: "%d ദിവസം",
                M: "ഒരു മാസം",
                MM: "%d മാസം",
                y: "ഒരു വർഷം",
                yy: "%d വർഷം"
            },
            meridiem: function (n) {
                return 4 > n ? "രാത്രി" : 12 > n ? "രാവിലെ" : 17 > n ? "ഉച്ച കഴിഞ്ഞ്" : 20 > n ? "വൈകുന്നേരം" : "രാത്രി"
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = {
            1: "१",
            2: "२",
            3: "३",
            4: "४",
            5: "५",
            6: "६",
            7: "७",
            8: "८",
            9: "९",
            0: "०"
        }
            , i = {
                "१": "1",
                "२": "2",
                "३": "3",
                "४": "4",
                "५": "5",
                "६": "6",
                "७": "7",
                "८": "8",
                "९": "9",
                "०": "0"
            };
        n.lang("mr", {
            months: "जानेवारी_फेब्रुवारी_मार्च_एप्रिल_मे_जून_जुलै_ऑगस्ट_सप्टेंबर_ऑक्टोबर_नोव्हेंबर_डिसेंबर".split("_"),
            monthsShort: "जाने._फेब्रु._मार्च._एप्रि._मे._जून._जुलै._ऑग._सप्टें._ऑक्टो._नोव्हें._डिसें.".split("_"),
            weekdays: "रविवार_सोमवार_मंगळवार_बुधवार_गुरूवार_शुक्रवार_शनिवार".split("_"),
            weekdaysShort: "रवि_सोम_मंगळ_बुध_गुरू_शुक्र_शनि".split("_"),
            weekdaysMin: "र_सो_मं_बु_गु_शु_श".split("_"),
            longDateFormat: {
                LT: "A h:mm वाजता",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY, LT",
                LLLL: "dddd, D MMMM YYYY, LT"
            },
            calendar: {
                sameDay: "[आज] LT",
                nextDay: "[उद्या] LT",
                nextWeek: "dddd, LT",
                lastDay: "[काल] LT",
                lastWeek: "[मागील] dddd, LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s नंतर",
                past: "%s पूर्वी",
                s: "सेकंद",
                m: "एक मिनिट",
                mm: "%d मिनिटे",
                h: "एक तास",
                hh: "%d तास",
                d: "एक दिवस",
                dd: "%d दिवस",
                M: "एक महिना",
                MM: "%d महिने",
                y: "एक वर्ष",
                yy: "%d वर्षे"
            },
            preparse: function (n) {
                return n.replace(/[१२३४५६७८९०]/g, function (n) {
                    return i[n]
                })
            },
            postformat: function (n) {
                return n.replace(/\d/g, function (n) {
                    return t[n]
                })
            },
            meridiem: function (n) {
                return 4 > n ? "रात्री" : 10 > n ? "सकाळी" : 17 > n ? "दुपारी" : 20 > n ? "सायंकाळी" : "रात्री"
            },
            week: {
                dow: 0,
                doy: 6
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ms-my", {
            months: "Januari_Februari_Mac_April_Mei_Jun_Julai_Ogos_September_Oktober_November_Disember".split("_"),
            monthsShort: "Jan_Feb_Mac_Apr_Mei_Jun_Jul_Ogs_Sep_Okt_Nov_Dis".split("_"),
            weekdays: "Ahad_Isnin_Selasa_Rabu_Khamis_Jumaat_Sabtu".split("_"),
            weekdaysShort: "Ahd_Isn_Sel_Rab_Kha_Jum_Sab".split("_"),
            weekdaysMin: "Ah_Is_Sl_Rb_Km_Jm_Sb".split("_"),
            longDateFormat: {
                LT: "HH.mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY [pukul] LT",
                LLLL: "dddd, D MMMM YYYY [pukul] LT"
            },
            meridiem: function (n) {
                return 11 > n ? "pagi" : 15 > n ? "tengahari" : 19 > n ? "petang" : "malam"
            },
            calendar: {
                sameDay: "[Hari ini pukul] LT",
                nextDay: "[Esok pukul] LT",
                nextWeek: "dddd [pukul] LT",
                lastDay: "[Kelmarin pukul] LT",
                lastWeek: "dddd [lepas pukul] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "dalam %s",
                past: "%s yang lepas",
                s: "beberapa saat",
                m: "seminit",
                mm: "%d minit",
                h: "sejam",
                hh: "%d jam",
                d: "sehari",
                dd: "%d hari",
                M: "sebulan",
                MM: "%d bulan",
                y: "setahun",
                yy: "%d tahun"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("nb", {
            months: "januar_februar_mars_april_mai_juni_juli_august_september_oktober_november_desember".split("_"),
            monthsShort: "jan._feb._mars_april_mai_juni_juli_aug._sep._okt._nov._des.".split("_"),
            weekdays: "søndag_mandag_tirsdag_onsdag_torsdag_fredag_lørdag".split("_"),
            weekdaysShort: "sø._ma._ti._on._to._fr._lø.".split("_"),
            weekdaysMin: "sø_ma_ti_on_to_fr_lø".split("_"),
            longDateFormat: {
                LT: "H.mm",
                L: "DD.MM.YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY [kl.] LT",
                LLLL: "dddd D. MMMM YYYY [kl.] LT"
            },
            calendar: {
                sameDay: "[i dag kl.] LT",
                nextDay: "[i morgen kl.] LT",
                nextWeek: "dddd [kl.] LT",
                lastDay: "[i går kl.] LT",
                lastWeek: "[forrige] dddd [kl.] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "om %s",
                past: "for %s siden",
                s: "noen sekunder",
                m: "ett minutt",
                mm: "%d minutter",
                h: "en time",
                hh: "%d timer",
                d: "en dag",
                dd: "%d dager",
                M: "en måned",
                MM: "%d måneder",
                y: "ett år",
                yy: "%d år"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = {
            1: "१",
            2: "२",
            3: "३",
            4: "४",
            5: "५",
            6: "६",
            7: "७",
            8: "८",
            9: "९",
            0: "०"
        }
            , i = {
                "१": "1",
                "२": "2",
                "३": "3",
                "४": "4",
                "५": "5",
                "६": "6",
                "७": "7",
                "८": "8",
                "९": "9",
                "०": "0"
            };
        n.lang("ne", {
            months: "जनवरी_फेब्रुवरी_मार्च_अप्रिल_मई_जुन_जुलाई_अगष्ट_सेप्टेम्बर_अक्टोबर_नोभेम्बर_डिसेम्बर".split("_"),
            monthsShort: "जन._फेब्रु._मार्च_अप्रि._मई_जुन_जुलाई._अग._सेप्ट._अक्टो._नोभे._डिसे.".split("_"),
            weekdays: "आइतबार_सोमबार_मङ्गलबार_बुधबार_बिहिबार_शुक्रबार_शनिबार".split("_"),
            weekdaysShort: "आइत._सोम._मङ्गल._बुध._बिहि._शुक्र._शनि.".split("_"),
            weekdaysMin: "आइ._सो._मङ्_बु._बि._शु._श.".split("_"),
            longDateFormat: {
                LT: "Aको h:mm बजे",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY, LT",
                LLLL: "dddd, D MMMM YYYY, LT"
            },
            preparse: function (n) {
                return n.replace(/[१२३४५६७८९०]/g, function (n) {
                    return i[n]
                })
            },
            postformat: function (n) {
                return n.replace(/\d/g, function (n) {
                    return t[n]
                })
            },
            meridiem: function (n) {
                return 3 > n ? "राती" : 10 > n ? "बिहान" : 15 > n ? "दिउँसो" : 18 > n ? "बेलुका" : 20 > n ? "साँझ" : "राती"
            },
            calendar: {
                sameDay: "[आज] LT",
                nextDay: "[भोली] LT",
                nextWeek: "[आउँदो] dddd[,] LT",
                lastDay: "[हिजो] LT",
                lastWeek: "[गएको] dddd[,] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%sमा",
                past: "%s अगाडी",
                s: "केही समय",
                m: "एक मिनेट",
                mm: "%d मिनेट",
                h: "एक घण्टा",
                hh: "%d घण्टा",
                d: "एक दिन",
                dd: "%d दिन",
                M: "एक महिना",
                MM: "%d महिना",
                y: "एक बर्ष",
                yy: "%d बर्ष"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = "jan._feb._mrt._apr._mei_jun._jul._aug._sep._okt._nov._dec.".split("_")
            , i = "jan_feb_mrt_apr_mei_jun_jul_aug_sep_okt_nov_dec".split("_");
        n.lang("nl", {
            months: "januari_februari_maart_april_mei_juni_juli_augustus_september_oktober_november_december".split("_"),
            monthsShort: function (n, r) {
                return /-MMM-/.test(r) ? i[n.month()] : t[n.month()]
            },
            weekdays: "zondag_maandag_dinsdag_woensdag_donderdag_vrijdag_zaterdag".split("_"),
            weekdaysShort: "zo._ma._di._wo._do._vr._za.".split("_"),
            weekdaysMin: "Zo_Ma_Di_Wo_Do_Vr_Za".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD-MM-YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Vandaag om] LT",
                nextDay: "[Morgen om] LT",
                nextWeek: "dddd [om] LT",
                lastDay: "[Gisteren om] LT",
                lastWeek: "[afgelopen] dddd [om] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "over %s",
                past: "%s geleden",
                s: "een paar seconden",
                m: "één minuut",
                mm: "%d minuten",
                h: "één uur",
                hh: "%d uur",
                d: "één dag",
                dd: "%d dagen",
                M: "één maand",
                MM: "%d maanden",
                y: "één jaar",
                yy: "%d jaar"
            },
            ordinal: function (n) {
                return n + (1 === n || 8 === n || n >= 20 ? "ste" : "de")
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("nn", {
            months: "januar_februar_mars_april_mai_juni_juli_august_september_oktober_november_desember".split("_"),
            monthsShort: "jan_feb_mar_apr_mai_jun_jul_aug_sep_okt_nov_des".split("_"),
            weekdays: "sundag_måndag_tysdag_onsdag_torsdag_fredag_laurdag".split("_"),
            weekdaysShort: "sun_mån_tys_ons_tor_fre_lau".split("_"),
            weekdaysMin: "su_må_ty_on_to_fr_lø".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[I dag klokka] LT",
                nextDay: "[I morgon klokka] LT",
                nextWeek: "dddd [klokka] LT",
                lastDay: "[I går klokka] LT",
                lastWeek: "[Føregående] dddd [klokka] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "om %s",
                past: "for %s siden",
                s: "noen sekund",
                m: "ett minutt",
                mm: "%d minutt",
                h: "en time",
                hh: "%d timar",
                d: "en dag",
                dd: "%d dagar",
                M: "en månad",
                MM: "%d månader",
                y: "ett år",
                yy: "%d år"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n) {
            return 5 > n % 10 && n % 10 > 1 && 1 != ~~(n / 10)
        }
        function t(n, t, r) {
            var u = n + " ";
            switch (r) {
                case "m":
                    return t ? "minuta" : "minutę";
                case "mm":
                    return u + (i(n) ? "minuty" : "minut");
                case "h":
                    return t ? "godzina" : "godzinę";
                case "hh":
                    return u + (i(n) ? "godziny" : "godzin");
                case "MM":
                    return u + (i(n) ? "miesiące" : "miesięcy");
                case "yy":
                    return u + (i(n) ? "lata" : "lat")
            }
        }
        var r = "styczeń_luty_marzec_kwiecień_maj_czerwiec_lipiec_sierpień_wrzesień_październik_listopad_grudzień".split("_")
            , u = "stycznia_lutego_marca_kwietnia_maja_czerwca_lipca_sierpnia_września_października_listopada_grudnia".split("_");
        n.lang("pl", {
            months: function (n, t) {
                return /D MMMM/.test(t) ? u[n.month()] : r[n.month()]
            },
            monthsShort: "sty_lut_mar_kwi_maj_cze_lip_sie_wrz_paź_lis_gru".split("_"),
            weekdays: "niedziela_poniedziałek_wtorek_środa_czwartek_piątek_sobota".split("_"),
            weekdaysShort: "nie_pon_wt_śr_czw_pt_sb".split("_"),
            weekdaysMin: "N_Pn_Wt_Śr_Cz_Pt_So".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Dziś o] LT",
                nextDay: "[Jutro o] LT",
                nextWeek: "[W] dddd [o] LT",
                lastDay: "[Wczoraj o] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[W zeszłą niedzielę o] LT";
                        case 3:
                            return "[W zeszłą środę o] LT";
                        case 6:
                            return "[W zeszłą sobotę o] LT";
                        default:
                            return "[W zeszły] dddd [o] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "za %s",
                past: "%s temu",
                s: "kilka sekund",
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: "1 dzień",
                dd: "%d dni",
                M: "miesiąc",
                MM: t,
                y: "rok",
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("pt-br", {
            months: "Janeiro_Fevereiro_Março_Abril_Maio_Junho_Julho_Agosto_Setembro_Outubro_Novembro_Dezembro".split("_"),
            monthsShort: "Jan_Fev_Mar_Abr_Mai_Jun_Jul_Ago_Set_Out_Nov_Dez".split("_"),
            weekdays: "Domingo_Segunda-feira_Terça-feira_Quarta-feira_Quinta-feira_Sexta-feira_Sábado".split("_"),
            weekdaysShort: "Dom_Seg_Ter_Qua_Qui_Sex_Sáb".split("_"),
            weekdaysMin: "Dom_2ª_3ª_4ª_5ª_6ª_Sáb".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D [de] MMMM [de] YYYY",
                LLL: "D [de] MMMM [de] YYYY LT",
                LLLL: "dddd, D [de] MMMM [de] YYYY LT"
            },
            calendar: {
                sameDay: "[Hoje às] LT",
                nextDay: "[Amanhã às] LT",
                nextWeek: "dddd [às] LT",
                lastDay: "[Ontem às] LT",
                lastWeek: function () {
                    return 0 === this.day() || 6 === this.day() ? "[Último] dddd [às] LT" : "[Última] dddd [às] LT"
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "em %s",
                past: "%s atrás",
                s: "segundos",
                m: "um minuto",
                mm: "%d minutos",
                h: "uma hora",
                hh: "%d horas",
                d: "um dia",
                dd: "%d dias",
                M: "um mês",
                MM: "%d meses",
                y: "um ano",
                yy: "%d anos"
            },
            ordinal: "%dº"
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("pt", {
            months: "Janeiro_Fevereiro_Março_Abril_Maio_Junho_Julho_Agosto_Setembro_Outubro_Novembro_Dezembro".split("_"),
            monthsShort: "Jan_Fev_Mar_Abr_Mai_Jun_Jul_Ago_Set_Out_Nov_Dez".split("_"),
            weekdays: "Domingo_Segunda-feira_Terça-feira_Quarta-feira_Quinta-feira_Sexta-feira_Sábado".split("_"),
            weekdaysShort: "Dom_Seg_Ter_Qua_Qui_Sex_Sáb".split("_"),
            weekdaysMin: "Dom_2ª_3ª_4ª_5ª_6ª_Sáb".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D [de] MMMM [de] YYYY",
                LLL: "D [de] MMMM [de] YYYY LT",
                LLLL: "dddd, D [de] MMMM [de] YYYY LT"
            },
            calendar: {
                sameDay: "[Hoje às] LT",
                nextDay: "[Amanhã às] LT",
                nextWeek: "dddd [às] LT",
                lastDay: "[Ontem às] LT",
                lastWeek: function () {
                    return 0 === this.day() || 6 === this.day() ? "[Último] dddd [às] LT" : "[Última] dddd [às] LT"
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "em %s",
                past: "%s atrás",
                s: "segundos",
                m: "um minuto",
                mm: "%d minutos",
                h: "uma hora",
                hh: "%d horas",
                d: "um dia",
                dd: "%d dias",
                M: "um mês",
                MM: "%d meses",
                y: "um ano",
                yy: "%d anos"
            },
            ordinal: "%dº",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("ro", {
            months: "Ianuarie_Februarie_Martie_Aprilie_Mai_Iunie_Iulie_August_Septembrie_Octombrie_Noiembrie_Decembrie".split("_"),
            monthsShort: "Ian_Feb_Mar_Apr_Mai_Iun_Iul_Aug_Sep_Oct_Noi_Dec".split("_"),
            weekdays: "Duminică_Luni_Marţi_Miercuri_Joi_Vineri_Sâmbătă".split("_"),
            weekdaysShort: "Dum_Lun_Mar_Mie_Joi_Vin_Sâm".split("_"),
            weekdaysMin: "Du_Lu_Ma_Mi_Jo_Vi_Sâ".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY H:mm",
                LLLL: "dddd, D MMMM YYYY H:mm"
            },
            calendar: {
                sameDay: "[azi la] LT",
                nextDay: "[mâine la] LT",
                nextWeek: "dddd [la] LT",
                lastDay: "[ieri la] LT",
                lastWeek: "[fosta] dddd [la] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "peste %s",
                past: "%s în urmă",
                s: "câteva secunde",
                m: "un minut",
                mm: "%d minute",
                h: "o oră",
                hh: "%d ore",
                d: "o zi",
                dd: "%d zile",
                M: "o lună",
                MM: "%d luni",
                y: "un an",
                yy: "%d ani"
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n, t) {
            var i = n.split("_");
            return 1 == t % 10 && 11 != t % 100 ? i[0] : t % 10 >= 2 && 4 >= t % 10 && (10 > t % 100 || t % 100 >= 20) ? i[1] : i[2]
        }
        function t(n, t, r) {
            return "m" === r ? t ? "минута" : "минуту" : n + " " + i({
                mm: "минута_минуты_минут",
                hh: "час_часа_часов",
                dd: "день_дня_дней",
                MM: "месяц_месяца_месяцев",
                yy: "год_года_лет"
            }[r], +n)
        }
        function r(n, t) {
            var i = {
                nominative: "январь_февраль_март_апрель_май_июнь_июль_август_сентябрь_октябрь_ноябрь_декабрь".split("_"),
                accusative: "января_февраля_марта_апреля_мая_июня_июля_августа_сентября_октября_ноября_декабря".split("_")
            }
                , r = /D[oD]? *MMMM?/.test(t) ? "accusative" : "nominative";
            return i[r][n.month()]
        }
        function u(n, t) {
            var i = {
                nominative: "янв_фев_мар_апр_май_июнь_июль_авг_сен_окт_ноя_дек".split("_"),
                accusative: "янв_фев_мар_апр_мая_июня_июля_авг_сен_окт_ноя_дек".split("_")
            }
                , r = /D[oD]? *MMMM?/.test(t) ? "accusative" : "nominative";
            return i[r][n.month()]
        }
        function f(n, t) {
            var i = {
                nominative: "воскресенье_понедельник_вторник_среда_четверг_пятница_суббота".split("_"),
                accusative: "воскресенье_понедельник_вторник_среду_четверг_пятницу_субботу".split("_")
            }
                , r = /\[ ?[Вв] ?(?:прошлую|следующую)? ?\] ?dddd/.test(t) ? "accusative" : "nominative";
            return i[r][n.day()]
        }
        n.lang("ru", {
            months: r,
            monthsShort: u,
            weekdays: f,
            weekdaysShort: "вск_пнд_втр_срд_чтв_птн_сбт".split("_"),
            weekdaysMin: "вс_пн_вт_ср_чт_пт_сб".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "D MMMM YYYY г.",
                LLL: "D MMMM YYYY г., LT",
                LLLL: "dddd, D MMMM YYYY г., LT"
            },
            calendar: {
                sameDay: "[Сегодня в] LT",
                nextDay: "[Завтра в] LT",
                lastDay: "[Вчера в] LT",
                nextWeek: function () {
                    return 2 === this.day() ? "[Во] dddd [в] LT" : "[В] dddd [в] LT"
                },
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[В прошлое] dddd [в] LT";
                        case 1:
                        case 2:
                        case 4:
                            return "[В прошлый] dddd [в] LT";
                        case 3:
                        case 5:
                        case 6:
                            return "[В прошлую] dddd [в] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "через %s",
                past: "%s назад",
                s: "несколько секунд",
                m: t,
                mm: t,
                h: "час",
                hh: t,
                d: "день",
                dd: t,
                M: "месяц",
                MM: t,
                y: "год",
                yy: t
            },
            ordinal: function (n, t) {
                switch (t) {
                    case "M":
                    case "d":
                    case "DDD":
                        return n + "-й";
                    case "D":
                        return n + "-го";
                    case "w":
                    case "W":
                        return n + "-я";
                    default:
                        return n
                }
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function i(n) {
            return n > 1 && 5 > n
        }
        function t(n, t, r, u) {
            var f = n + " ";
            switch (r) {
                case "s":
                    return t || u ? "pár sekúnd" : "pár sekundami";
                case "m":
                    return t ? "minúta" : u ? "minútu" : "minútou";
                case "mm":
                    return t || u ? f + (i(n) ? "minúty" : "minút") : f + "minútami";
                case "h":
                    return t ? "hodina" : u ? "hodinu" : "hodinou";
                case "hh":
                    return t || u ? f + (i(n) ? "hodiny" : "hodín") : f + "hodinami";
                case "d":
                    return t || u ? "deň" : "dňom";
                case "dd":
                    return t || u ? f + (i(n) ? "dni" : "dní") : f + "dňami";
                case "M":
                    return t || u ? "mesiac" : "mesiacom";
                case "MM":
                    return t || u ? f + (i(n) ? "mesiace" : "mesiacov") : f + "mesiacmi";
                case "y":
                    return t || u ? "rok" : "rokom";
                case "yy":
                    return t || u ? f + (i(n) ? "roky" : "rokov") : f + "rokmi"
            }
        }
        var r = "január_február_marec_apríl_máj_jún_júl_august_september_október_november_december".split("_")
            , u = "jan_feb_mar_apr_máj_jún_júl_aug_sep_okt_nov_dec".split("_");
        n.lang("sk", {
            months: r,
            monthsShort: u,
            monthsParse: function (n, t) {
                for (var r = [], i = 0; 12 > i; i++)
                    r[i] = new RegExp("^" + n[i] + "$|^" + t[i] + "$", "i");
                return r
            }(r, u),
            weekdays: "nedeľa_pondelok_utorok_streda_štvrtok_piatok_sobota".split("_"),
            weekdaysShort: "ne_po_ut_st_št_pi_so".split("_"),
            weekdaysMin: "ne_po_ut_st_št_pi_so".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD.MM.YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[dnes o] LT",
                nextDay: "[zajtra o] LT",
                nextWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[v nedeľu o] LT";
                        case 1:
                        case 2:
                            return "[v] dddd [o] LT";
                        case 3:
                            return "[v stredu o] LT";
                        case 4:
                            return "[vo štvrtok o] LT";
                        case 5:
                            return "[v piatok o] LT";
                        case 6:
                            return "[v sobotu o] LT"
                    }
                },
                lastDay: "[včera o] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[minulú nedeľu o] LT";
                        case 1:
                        case 2:
                            return "[minulý] dddd [o] LT";
                        case 3:
                            return "[minulú stredu o] LT";
                        case 4:
                        case 5:
                            return "[minulý] dddd [o] LT";
                        case 6:
                            return "[minulú sobotu o] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "za %s",
                past: "pred %s",
                s: t,
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: t,
                dd: t,
                M: t,
                MM: t,
                y: t,
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function t(n, t, i) {
            var r = n + " ";
            switch (i) {
                case "m":
                    return t ? "ena minuta" : "eno minuto";
                case "mm":
                    return r + (1 === n ? "minuta" : 2 === n ? "minuti" : 3 === n || 4 === n ? "minute" : "minut");
                case "h":
                    return t ? "ena ura" : "eno uro";
                case "hh":
                    return r + (1 === n ? "ura" : 2 === n ? "uri" : 3 === n || 4 === n ? "ure" : "ur");
                case "dd":
                    return r + (1 === n ? "dan" : "dni");
                case "MM":
                    return r + (1 === n ? "mesec" : 2 === n ? "meseca" : 3 === n || 4 === n ? "mesece" : "mesecev");
                case "yy":
                    return r + (1 === n ? "leto" : 2 === n ? "leti" : 3 === n || 4 === n ? "leta" : "let")
            }
        }
        n.lang("sl", {
            months: "januar_februar_marec_april_maj_junij_julij_avgust_september_oktober_november_december".split("_"),
            monthsShort: "jan._feb._mar._apr._maj._jun._jul._avg._sep._okt._nov._dec.".split("_"),
            weekdays: "nedelja_ponedeljek_torek_sreda_četrtek_petek_sobota".split("_"),
            weekdaysShort: "ned._pon._tor._sre._čet._pet._sob.".split("_"),
            weekdaysMin: "ne_po_to_sr_če_pe_so".split("_"),
            longDateFormat: {
                LT: "H:mm",
                L: "DD. MM. YYYY",
                LL: "D. MMMM YYYY",
                LLL: "D. MMMM YYYY LT",
                LLLL: "dddd, D. MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[danes ob] LT",
                nextDay: "[jutri ob] LT",
                nextWeek: function () {
                    switch (this.day()) {
                        case 0:
                            return "[v] [nedeljo] [ob] LT";
                        case 3:
                            return "[v] [sredo] [ob] LT";
                        case 6:
                            return "[v] [soboto] [ob] LT";
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            return "[v] dddd [ob] LT"
                    }
                },
                lastDay: "[včeraj ob] LT",
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                        case 3:
                        case 6:
                            return "[prejšnja] dddd [ob] LT";
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            return "[prejšnji] dddd [ob] LT"
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "čez %s",
                past: "%s nazaj",
                s: "nekaj sekund",
                m: t,
                mm: t,
                h: t,
                hh: t,
                d: "en dan",
                dd: t,
                M: "en mesec",
                MM: t,
                y: "eno leto",
                yy: t
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("sq", {
            months: "Janar_Shkurt_Mars_Prill_Maj_Qershor_Korrik_Gusht_Shtator_Tetor_Nëntor_Dhjetor".split("_"),
            monthsShort: "Jan_Shk_Mar_Pri_Maj_Qer_Kor_Gus_Sht_Tet_Nën_Dhj".split("_"),
            weekdays: "E Diel_E Hënë_E Marte_E Mërkure_E Enjte_E Premte_E Shtunë".split("_"),
            weekdaysShort: "Die_Hën_Mar_Mër_Enj_Pre_Sht".split("_"),
            weekdaysMin: "D_H_Ma_Më_E_P_Sh".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Sot në] LT",
                nextDay: "[Neser në] LT",
                nextWeek: "dddd [në] LT",
                lastDay: "[Dje në] LT",
                lastWeek: "dddd [e kaluar në] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "në %s",
                past: "%s me parë",
                s: "disa seconda",
                m: "një minut",
                mm: "%d minutea",
                h: "një orë",
                hh: "%d orë",
                d: "një ditë",
                dd: "%d ditë",
                M: "një muaj",
                MM: "%d muaj",
                y: "një vit",
                yy: "%d vite"
            },
            ordinal: "%d.",
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("sv", {
            months: "januari_februari_mars_april_maj_juni_juli_augusti_september_oktober_november_december".split("_"),
            monthsShort: "jan_feb_mar_apr_maj_jun_jul_aug_sep_okt_nov_dec".split("_"),
            weekdays: "söndag_måndag_tisdag_onsdag_torsdag_fredag_lördag".split("_"),
            weekdaysShort: "sön_mån_tis_ons_tor_fre_lör".split("_"),
            weekdaysMin: "sö_må_ti_on_to_fr_lö".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "YYYY-MM-DD",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[Idag] LT",
                nextDay: "[Imorgon] LT",
                lastDay: "[Igår] LT",
                nextWeek: "dddd LT",
                lastWeek: "[Förra] dddd[en] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "om %s",
                past: "för %s sedan",
                s: "några sekunder",
                m: "en minut",
                mm: "%d minuter",
                h: "en timme",
                hh: "%d timmar",
                d: "en dag",
                dd: "%d dagar",
                M: "en månad",
                MM: "%d månader",
                y: "ett år",
                yy: "%d år"
            },
            ordinal: function (n) {
                var t = n % 10
                    , i = 1 == ~~(n % 100 / 10) ? "e" : 1 === t ? "a" : 2 === t ? "a" : 3 === t ? "e" : "e";
                return n + i
            },
            week: {
                dow: 1,
                doy: 4
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("th", {
            months: "มกราคม_กุมภาพันธ์_มีนาคม_เมษายน_พฤษภาคม_มิถุนายน_กรกฎาคม_สิงหาคม_กันยายน_ตุลาคม_พฤศจิกายน_ธันวาคม".split("_"),
            monthsShort: "มกรา_กุมภา_มีนา_เมษา_พฤษภา_มิถุนา_กรกฎา_สิงหา_กันยา_ตุลา_พฤศจิกา_ธันวา".split("_"),
            weekdays: "อาทิตย์_จันทร์_อังคาร_พุธ_พฤหัสบดี_ศุกร์_เสาร์".split("_"),
            weekdaysShort: "อาทิตย์_จันทร์_อังคาร_พุธ_พฤหัส_ศุกร์_เสาร์".split("_"),
            weekdaysMin: "อา._จ._อ._พ._พฤ._ศ._ส.".split("_"),
            longDateFormat: {
                LT: "H นาฬิกา m นาที",
                L: "YYYY/MM/DD",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY เวลา LT",
                LLLL: "วันddddที่ D MMMM YYYY เวลา LT"
            },
            meridiem: function (n) {
                return 12 > n ? "ก่อนเที่ยง" : "หลังเที่ยง"
            },
            calendar: {
                sameDay: "[วันนี้ เวลา] LT",
                nextDay: "[พรุ่งนี้ เวลา] LT",
                nextWeek: "dddd[หน้า เวลา] LT",
                lastDay: "[เมื่อวานนี้ เวลา] LT",
                lastWeek: "[วัน]dddd[ที่แล้ว เวลา] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "อีก %s",
                past: "%sที่แล้ว",
                s: "ไม่กี่วินาที",
                m: "1 นาที",
                mm: "%d นาที",
                h: "1 ชั่วโมง",
                hh: "%d ชั่วโมง",
                d: "1 วัน",
                dd: "%d วัน",
                M: "1 เดือน",
                MM: "%d เดือน",
                y: "1 ปี",
                yy: "%d ปี"
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        var t = {
            1: "'inci",
            5: "'inci",
            8: "'inci",
            70: "'inci",
            80: "'inci",
            2: "'nci",
            7: "'nci",
            20: "'nci",
            50: "'nci",
            3: "'üncü",
            4: "'üncü",
            100: "'üncü",
            6: "'ncı",
            9: "'uncu",
            10: "'uncu",
            30: "'uncu",
            60: "'ıncı",
            90: "'ıncı"
        };
        n.lang("tr", {
            months: "Ocak_Şubat_Mart_Nisan_Mayıs_Haziran_Temmuz_Ağustos_Eylül_Ekim_Kasım_Aralık".split("_"),
            monthsShort: "Oca_Şub_Mar_Nis_May_Haz_Tem_Ağu_Eyl_Eki_Kas_Ara".split("_"),
            weekdays: "Pazar_Pazartesi_Salı_Çarşamba_Perşembe_Cuma_Cumartesi".split("_"),
            weekdaysShort: "Paz_Pts_Sal_Çar_Per_Cum_Cts".split("_"),
            weekdaysMin: "Pz_Pt_Sa_Ça_Pe_Cu_Ct".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd, D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[bugün saat] LT",
                nextDay: "[yarın saat] LT",
                nextWeek: "[haftaya] dddd [saat] LT",
                lastDay: "[dün] LT",
                lastWeek: "[geçen hafta] dddd [saat] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "%s sonra",
                past: "%s önce",
                s: "birkaç saniye",
                m: "bir dakika",
                mm: "%d dakika",
                h: "bir saat",
                hh: "%d saat",
                d: "bir gün",
                dd: "%d gün",
                M: "bir ay",
                MM: "%d ay",
                y: "bir yıl",
                yy: "%d yıl"
            },
            ordinal: function (n) {
                if (0 === n)
                    return n + "'ıncı";
                var i = n % 10
                    , r = n % 100 - i
                    , u = n >= 100 ? 100 : null;
                return n + (t[i] || t[r] || t[u])
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("tzm-la", {
            months: "innayr_brˤayrˤ_marˤsˤ_ibrir_mayyw_ywnyw_ywlywz_ɣwšt_šwtanbir_ktˤwbrˤ_nwwanbir_dwjnbir".split("_"),
            monthsShort: "innayr_brˤayrˤ_marˤsˤ_ibrir_mayyw_ywnyw_ywlywz_ɣwšt_šwtanbir_ktˤwbrˤ_nwwanbir_dwjnbir".split("_"),
            weekdays: "asamas_aynas_asinas_akras_akwas_asimwas_asiḍyas".split("_"),
            weekdaysShort: "asamas_aynas_asinas_akras_akwas_asimwas_asiḍyas".split("_"),
            weekdaysMin: "asamas_aynas_asinas_akras_akwas_asimwas_asiḍyas".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[asdkh g] LT",
                nextDay: "[aska g] LT",
                nextWeek: "dddd [g] LT",
                lastDay: "[assant g] LT",
                lastWeek: "dddd [g] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "dadkh s yan %s",
                past: "yan %s",
                s: "imik",
                m: "minuḍ",
                mm: "%d minuḍ",
                h: "saɛa",
                hh: "%d tassaɛin",
                d: "ass",
                dd: "%d ossan",
                M: "ayowr",
                MM: "%d iyyirn",
                y: "asgas",
                yy: "%d isgasn"
            },
            week: {
                dow: 6,
                doy: 12
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("tzm", {
            months: "ⵉⵏⵏⴰⵢⵔ_ⴱⵕⴰⵢⵕ_ⵎⴰⵕⵚ_ⵉⴱⵔⵉⵔ_ⵎⴰⵢⵢⵓ_ⵢⵓⵏⵢⵓ_ⵢⵓⵍⵢⵓⵣ_ⵖⵓⵛⵜ_ⵛⵓⵜⴰⵏⴱⵉⵔ_ⴽⵟⵓⴱⵕ_ⵏⵓⵡⴰⵏⴱⵉⵔ_ⴷⵓⵊⵏⴱⵉⵔ".split("_"),
            monthsShort: "ⵉⵏⵏⴰⵢⵔ_ⴱⵕⴰⵢⵕ_ⵎⴰⵕⵚ_ⵉⴱⵔⵉⵔ_ⵎⴰⵢⵢⵓ_ⵢⵓⵏⵢⵓ_ⵢⵓⵍⵢⵓⵣ_ⵖⵓⵛⵜ_ⵛⵓⵜⴰⵏⴱⵉⵔ_ⴽⵟⵓⴱⵕ_ⵏⵓⵡⴰⵏⴱⵉⵔ_ⴷⵓⵊⵏⴱⵉⵔ".split("_"),
            weekdays: "ⴰⵙⴰⵎⴰⵙ_ⴰⵢⵏⴰⵙ_ⴰⵙⵉⵏⴰⵙ_ⴰⴽⵔⴰⵙ_ⴰⴽⵡⴰⵙ_ⴰⵙⵉⵎⵡⴰⵙ_ⴰⵙⵉⴹⵢⴰⵙ".split("_"),
            weekdaysShort: "ⴰⵙⴰⵎⴰⵙ_ⴰⵢⵏⴰⵙ_ⴰⵙⵉⵏⴰⵙ_ⴰⴽⵔⴰⵙ_ⴰⴽⵡⴰⵙ_ⴰⵙⵉⵎⵡⴰⵙ_ⴰⵙⵉⴹⵢⴰⵙ".split("_"),
            weekdaysMin: "ⴰⵙⴰⵎⴰⵙ_ⴰⵢⵏⴰⵙ_ⴰⵙⵉⵏⴰⵙ_ⴰⴽⵔⴰⵙ_ⴰⴽⵡⴰⵙ_ⴰⵙⵉⵎⵡⴰⵙ_ⴰⵙⵉⴹⵢⴰⵙ".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD/MM/YYYY",
                LL: "D MMMM YYYY",
                LLL: "D MMMM YYYY LT",
                LLLL: "dddd D MMMM YYYY LT"
            },
            calendar: {
                sameDay: "[ⴰⵙⴷⵅ ⴴ] LT",
                nextDay: "[ⴰⵙⴽⴰ ⴴ] LT",
                nextWeek: "dddd [ⴴ] LT",
                lastDay: "[ⴰⵚⴰⵏⵜ ⴴ] LT",
                lastWeek: "dddd [ⴴ] LT",
                sameElse: "L"
            },
            relativeTime: {
                future: "ⴷⴰⴷⵅ ⵙ ⵢⴰⵏ %s",
                past: "ⵢⴰⵏ %s",
                s: "ⵉⵎⵉⴽ",
                m: "ⵎⵉⵏⵓⴺ",
                mm: "%d ⵎⵉⵏⵓⴺ",
                h: "ⵙⴰⵄⴰ",
                hh: "%d ⵜⴰⵙⵙⴰⵄⵉⵏ",
                d: "ⴰⵙⵙ",
                dd: "%d oⵙⵙⴰⵏ",
                M: "ⴰⵢoⵓⵔ",
                MM: "%d ⵉⵢⵢⵉⵔⵏ",
                y: "ⴰⵙⴳⴰⵙ",
                yy: "%d ⵉⵙⴳⴰⵙⵏ"
            },
            week: {
                dow: 6,
                doy: 12
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        function r(n, t) {
            var i = n.split("_");
            return 1 == t % 10 && 11 != t % 100 ? i[0] : t % 10 >= 2 && 4 >= t % 10 && (10 > t % 100 || t % 100 >= 20) ? i[1] : i[2]
        }
        function t(n, t, i) {
            return "m" === i ? t ? "хвилина" : "хвилину" : "h" === i ? t ? "година" : "годину" : n + " " + r({
                mm: "хвилина_хвилини_хвилин",
                hh: "година_години_годин",
                dd: "день_дні_днів",
                MM: "місяць_місяці_місяців",
                yy: "рік_роки_років"
            }[i], +n)
        }
        function u(n, t) {
            var i = {
                nominative: "січень_лютий_березень_квітень_травень_червень_липень_серпень_вересень_жовтень_листопад_грудень".split("_"),
                accusative: "січня_лютого_березня_квітня_травня_червня_липня_серпня_вересня_жовтня_листопада_грудня".split("_")
            }
                , r = /D[oD]? *MMMM?/.test(t) ? "accusative" : "nominative";
            return i[r][n.month()]
        }
        function f(n, t) {
            var i = {
                nominative: "неділя_понеділок_вівторок_середа_четвер_п’ятниця_субота".split("_"),
                accusative: "неділю_понеділок_вівторок_середу_четвер_п’ятницю_суботу".split("_"),
                genitive: "неділі_понеділка_вівторка_середи_четверга_п’ятниці_суботи".split("_")
            }
                , r = /(\[[ВвУу]\]) ?dddd/.test(t) ? "accusative" : /\[?(?:минулої|наступної)? ?\] ?dddd/.test(t) ? "genitive" : "nominative";
            return i[r][n.day()]
        }
        function i(n) {
            return function () {
                return n + "о" + (11 === this.hours() ? "б" : "") + "] LT"
            }
        }
        n.lang("uk", {
            months: u,
            monthsShort: "січ_лют_бер_квіт_трав_черв_лип_серп_вер_жовт_лист_груд".split("_"),
            weekdays: f,
            weekdaysShort: "нед_пон_вів_сер_чет_п’ят_суб".split("_"),
            weekdaysMin: "нд_пн_вт_ср_чт_пт_сб".split("_"),
            longDateFormat: {
                LT: "HH:mm",
                L: "DD.MM.YYYY",
                LL: "D MMMM YYYY р.",
                LLL: "D MMMM YYYY р., LT",
                LLLL: "dddd, D MMMM YYYY р., LT"
            },
            calendar: {
                sameDay: i("[Сьогодні "),
                nextDay: i("[Завтра "),
                lastDay: i("[Вчора "),
                nextWeek: i("[У] dddd ["),
                lastWeek: function () {
                    switch (this.day()) {
                        case 0:
                        case 3:
                        case 5:
                        case 6:
                            return i("[Минулої] dddd [").call(this);
                        case 1:
                        case 2:
                        case 4:
                            return i("[Минулого] dddd [").call(this)
                    }
                },
                sameElse: "L"
            },
            relativeTime: {
                future: "за %s",
                past: "%s тому",
                s: "декілька секунд",
                m: t,
                mm: t,
                h: "годину",
                hh: t,
                d: "день",
                dd: t,
                M: "місяць",
                MM: t,
                y: "рік",
                yy: t
            },
            ordinal: function (n, t) {
                switch (t) {
                    case "M":
                    case "d":
                    case "DDD":
                    case "w":
                    case "W":
                        return n + "-й";
                    case "D":
                        return n + "-го";
                    default:
                        return n
                }
            },
            week: {
                dow: 1,
                doy: 7
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("zh-cn", {
            months: "一月_二月_三月_四月_五月_六月_七月_八月_九月_十月_十一月_十二月".split("_"),
            monthsShort: "1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月".split("_"),
            weekdays: "星期日_星期一_星期二_星期三_星期四_星期五_星期六".split("_"),
            weekdaysShort: "周日_周一_周二_周三_周四_周五_周六".split("_"),
            weekdaysMin: "日_一_二_三_四_五_六".split("_"),
            longDateFormat: {
                LT: "Ah点mm",
                L: "YYYY年MMMD日",
                LL: "YYYY年MMMD日",
                LLL: "YYYY年MMMD日LT",
                LLLL: "YYYY年MMMD日ddddLT",
                l: "YYYY年MMMD日",
                ll: "YYYY年MMMD日",
                lll: "YYYY年MMMD日LT",
                llll: "YYYY年MMMD日ddddLT"
            },
            meridiem: function (n, t) {
                return 9 > n ? "早上" : 11 > n && 30 > t ? "上午" : 13 > n && 30 > t ? "中午" : 18 > n ? "下午" : "晚上"
            },
            calendar: {
                sameDay: "[今天]LT",
                nextDay: "[明天]LT",
                nextWeek: "[下]ddddLT",
                lastDay: "[昨天]LT",
                lastWeek: "[上]ddddLT",
                sameElse: "L"
            },
            ordinal: function (n, t) {
                switch (t) {
                    case "d":
                    case "D":
                    case "DDD":
                        return n + "日";
                    case "M":
                        return n + "月";
                    case "w":
                    case "W":
                        return n + "周";
                    default:
                        return n
                }
            },
            relativeTime: {
                future: "%s内",
                past: "%s前",
                s: "几秒",
                m: "1分钟",
                mm: "%d分钟",
                h: "1小时",
                hh: "%d小时",
                d: "1天",
                dd: "%d天",
                M: "1个月",
                MM: "%d个月",
                y: "1年",
                yy: "%d年"
            }
        })
    }),
    function (n) {
        n(t)
    }(function (n) {
        n.lang("zh-tw", {
            months: "一月_二月_三月_四月_五月_六月_七月_八月_九月_十月_十一月_十二月".split("_"),
            monthsShort: "1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月".split("_"),
            weekdays: "星期日_星期一_星期二_星期三_星期四_星期五_星期六".split("_"),
            weekdaysShort: "週日_週一_週二_週三_週四_週五_週六".split("_"),
            weekdaysMin: "日_一_二_三_四_五_六".split("_"),
            longDateFormat: {
                LT: "Ah點mm",
                L: "YYYY年MMMD日",
                LL: "YYYY年MMMD日",
                LLL: "YYYY年MMMD日LT",
                LLLL: "YYYY年MMMD日ddddLT",
                l: "YYYY年MMMD日",
                ll: "YYYY年MMMD日",
                lll: "YYYY年MMMD日LT",
                llll: "YYYY年MMMD日ddddLT"
            },
            meridiem: function (n, t) {
                return 9 > n ? "早上" : 11 > n && 30 > t ? "上午" : 13 > n && 30 > t ? "中午" : 18 > n ? "下午" : "晚上"
            },
            calendar: {
                sameDay: "[今天]LT",
                nextDay: "[明天]LT",
                nextWeek: "[下]ddddLT",
                lastDay: "[昨天]LT",
                lastWeek: "[上]ddddLT",
                sameElse: "L"
            },
            ordinal: function (n, t) {
                switch (t) {
                    case "d":
                    case "D":
                    case "DDD":
                        return n + "日";
                    case "M":
                        return n + "月";
                    case "w":
                    case "W":
                        return n + "週";
                    default:
                        return n
                }
            },
            relativeTime: {
                future: "%s內",
                past: "%s前",
                s: "幾秒",
                m: "一分鐘",
                mm: "%d分鐘",
                h: "一小時",
                hh: "%d小時",
                d: "一天",
                dd: "%d天",
                M: "一個月",
                MM: "%d個月",
                y: "一年",
                yy: "%d年"
            }
        })
    });
t.lang("en");
lt && (module.exports = t);
"undefined" == typeof ender && (this.moment = t);
"function" == typeof define && define.amd && define("moment", [], function () {
    return t
})
}
.call(this),
    "undefined" == typeof jQuery)
throw new Error("Bootstrap's JavaScript requires jQuery");
+function (n) {
    var t = n.fn.jquery.split(" ")[0].split(".");
    if (t[0] < 2 && t[1] < 9 || 1 == t[0] && 9 == t[1] && t[2] < 1)
        throw new Error("Bootstrap's JavaScript requires jQuery version 1.9.1 or higher");
}(jQuery);
+function (n) {
    function t() {
        var i = document.createElement("bootstrap"), t = {
            WebkitTransition: "webkitTransitionEnd",
            MozTransition: "transitionend",
            OTransition: "oTransitionEnd otransitionend",
            transition: "transitionend"
        }, n;
        for (n in t)
            if (void 0 !== i.style[n])
                return {
                    end: t[n]
                };
        return !1
    }
    n.fn.emulateTransitionEnd = function (t) {
        var i = !1, u = this, r;
        n(this).one("bsTransitionEnd", function () {
            i = !0
        });
        return r = function () {
            i || n(u).trigger(n.support.transition.end)
        }
            ,
            setTimeout(r, t),
            this
    }
        ;
    n(function () {
        n.support.transition = t();
        n.support.transition && (n.event.special.bsTransitionEnd = {
            bindType: n.support.transition.end,
            delegateType: n.support.transition.end,
            handle: function (t) {
                if (n(t.target).is(this))
                    return t.handleObj.handler.apply(this, arguments)
            }
        })
    })
}(jQuery);
+function (n) {
    function u(i) {
        return this.each(function () {
            var r = n(this)
                , u = r.data("bs.alert");
            u || r.data("bs.alert", u = new t(this));
            "string" == typeof i && u[i].call(r)
        })
    }
    var i = '[data-dismiss="alert"]', t = function (t) {
        n(t).on("click", i, this.close)
    }, r;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 150;
    t.prototype.close = function (i) {
        function e() {
            r.detach().trigger("closed.bs.alert").remove()
        }
        var f = n(this), u = f.attr("data-target"), r;
        u || (u = f.attr("href"),
            u = u && u.replace(/.*(?=#[^\s]*$)/, ""));
        r = n(u);
        i && i.preventDefault();
        r.length || (r = f.closest(".alert"));
        r.trigger(i = n.Event("close.bs.alert"));
        i.isDefaultPrevented() || (r.removeClass("in"),
            n.support.transition && r.hasClass("fade") ? r.one("bsTransitionEnd", e).emulateTransitionEnd(t.TRANSITION_DURATION) : e())
    }
        ;
    r = n.fn.alert;
    n.fn.alert = u;
    n.fn.alert.Constructor = t;
    n.fn.alert.noConflict = function () {
        return n.fn.alert = r,
            this
    }
        ;
    n(document).on("click.bs.alert.data-api", i, t.prototype.close)
}(jQuery);
+function (n) {
    function i(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.button")
                , f = "object" == typeof i && i;
            r || u.data("bs.button", r = new t(this, f));
            "toggle" == i ? r.toggle() : i && r.setState(i)
        })
    }
    var t = function (i, r) {
        this.$element = n(i);
        this.options = n.extend({}, t.DEFAULTS, r);
        this.isLoading = !1
    }, r;
    t.VERSION = "3.3.4";
    t.DEFAULTS = {
        loadingText: "loading..."
    };
    t.prototype.setState = function (t) {
        var r = "disabled"
            , i = this.$element
            , f = i.is("input") ? "val" : "html"
            , u = i.data();
        t += "Text";
        null == u.resetText && i.data("resetText", i[f]());
        setTimeout(n.proxy(function () {
            i[f](null == u[t] ? this.options[t] : u[t]);
            "loadingText" == t ? (this.isLoading = !0,
                i.addClass(r).attr(r, r)) : this.isLoading && (this.isLoading = !1,
                    i.removeClass(r).removeAttr(r))
        }, this), 0)
    }
        ;
    t.prototype.toggle = function () {
        var t = !0, i = this.$element.closest('[data-toggle="buttons"]'), n;
        i.length ? (n = this.$element.find("input"),
            "radio" == n.prop("type") && (n.prop("checked") && this.$element.hasClass("active") ? t = !1 : i.find(".active").removeClass("active")),
            t && n.prop("checked", !this.$element.hasClass("active")).trigger("change")) : this.$element.attr("aria-pressed", !this.$element.hasClass("active"));
        t && this.$element.toggleClass("active")
    }
        ;
    r = n.fn.button;
    n.fn.button = i;
    n.fn.button.Constructor = t;
    n.fn.button.noConflict = function () {
        return n.fn.button = r,
            this
    }
        ;
    n(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function (t) {
        var r = n(t.target);
        r.hasClass("btn") || (r = r.closest(".btn"));
        i.call(r, "toggle");
        t.preventDefault()
    }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function (t) {
        n(t.target).closest(".btn").toggleClass("focus", /^focus(in)?$/.test(t.type))
    })
}(jQuery);
+function (n) {
    function i(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.carousel")
                , f = n.extend({}, t.DEFAULTS, u.data(), "object" == typeof i && i)
                , e = "string" == typeof i ? i : f.slide;
            r || u.data("bs.carousel", r = new t(this, f));
            "number" == typeof i ? r.to(i) : e ? r[e]() : f.interval && r.pause().cycle()
        })
    }
    var t = function (t, i) {
        this.$element = n(t);
        this.$indicators = this.$element.find(".carousel-indicators");
        this.options = i;
        this.paused = null;
        this.sliding = null;
        this.interval = null;
        this.$active = null;
        this.$items = null;
        this.options.keyboard && this.$element.on("keydown.bs.carousel", n.proxy(this.keydown, this));
        "hover" == this.options.pause && !("ontouchstart" in document.documentElement) && this.$element.on("mouseenter.bs.carousel", n.proxy(this.pause, this)).on("mouseleave.bs.carousel", n.proxy(this.cycle, this))
    }, u, r;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 600;
    t.DEFAULTS = {
        interval: 5e3,
        pause: "hover",
        wrap: !0,
        keyboard: !0
    };
    t.prototype.keydown = function (n) {
        if (!/input|textarea/i.test(n.target.tagName)) {
            switch (n.which) {
                case 37:
                    this.prev();
                    break;
                case 39:
                    this.next();
                    break;
                default:
                    return
            }
            n.preventDefault()
        }
    }
        ;
    t.prototype.cycle = function (t) {
        return t || (this.paused = !1),
            this.interval && clearInterval(this.interval),
            this.options.interval && !this.paused && (this.interval = setInterval(n.proxy(this.next, this), this.options.interval)),
            this
    }
        ;
    t.prototype.getItemIndex = function (n) {
        return this.$items = n.parent().children(".item"),
            this.$items.index(n || this.$active)
    }
        ;
    t.prototype.getItemForDirection = function (n, t) {
        var i = this.getItemIndex(t), f = "prev" == n && 0 === i || "next" == n && i == this.$items.length - 1, r, u;
        return f && !this.options.wrap ? t : (r = "prev" == n ? -1 : 1,
            u = (i + r) % this.$items.length,
            this.$items.eq(u))
    }
        ;
    t.prototype.to = function (n) {
        var i = this
            , t = this.getItemIndex(this.$active = this.$element.find(".item.active"));
        if (!(n > this.$items.length - 1) && !(0 > n))
            return this.sliding ? this.$element.one("slid.bs.carousel", function () {
                i.to(n)
            }) : t == n ? this.pause().cycle() : this.slide(n > t ? "next" : "prev", this.$items.eq(n))
    }
        ;
    t.prototype.pause = function (t) {
        return t || (this.paused = !0),
            this.$element.find(".next, .prev").length && n.support.transition && (this.$element.trigger(n.support.transition.end),
                this.cycle(!0)),
            this.interval = clearInterval(this.interval),
            this
    }
        ;
    t.prototype.next = function () {
        if (!this.sliding)
            return this.slide("next")
    }
        ;
    t.prototype.prev = function () {
        if (!this.sliding)
            return this.slide("prev")
    }
        ;
    t.prototype.slide = function (i, r) {
        var e = this.$element.find(".item.active"), u = r || this.getItemForDirection(i, e), l = this.interval, f = "next" == i ? "left" : "right", a = this, o, s, h, c;
        return u.hasClass("active") ? this.sliding = !1 : (o = u[0],
            s = n.Event("slide.bs.carousel", {
                relatedTarget: o,
                direction: f
            }),
            (this.$element.trigger(s),
                !s.isDefaultPrevented()) ? ((this.sliding = !0,
                    l && this.pause(),
                    this.$indicators.length) && (this.$indicators.find(".active").removeClass("active"),
                        h = n(this.$indicators.children()[this.getItemIndex(u)]),
                        h && h.addClass("active")),
                    c = n.Event("slid.bs.carousel", {
                        relatedTarget: o,
                        direction: f
                    }),
                    n.support.transition && this.$element.hasClass("slide") ? (u.addClass(i),
                        u[0].offsetWidth,
                        e.addClass(f),
                        u.addClass(f),
                        e.one("bsTransitionEnd", function () {
                            u.removeClass([i, f].join(" ")).addClass("active");
                            e.removeClass(["active", f].join(" "));
                            a.sliding = !1;
                            setTimeout(function () {
                                a.$element.trigger(c)
                            }, 0)
                        }).emulateTransitionEnd(t.TRANSITION_DURATION)) : (e.removeClass("active"),
                            u.addClass("active"),
                            this.sliding = !1,
                            this.$element.trigger(c)),
                    l && this.cycle(),
                    this) : void 0)
    }
        ;
    u = n.fn.carousel;
    n.fn.carousel = i;
    n.fn.carousel.Constructor = t;
    n.fn.carousel.noConflict = function () {
        return n.fn.carousel = u,
            this
    }
        ;
    r = function (t) {
        var o, r = n(this), u = n(r.attr("data-target") || (o = r.attr("href")) && o.replace(/.*(?=#[^\s]+$)/, "")), e, f;
        u.hasClass("carousel") && (e = n.extend({}, u.data(), r.data()),
            f = r.attr("data-slide-to"),
            f && (e.interval = !1),
            i.call(u, e),
            f && u.data("bs.carousel").to(f),
            t.preventDefault())
    }
        ;
    n(document).on("click.bs.carousel.data-api", "[data-slide]", r).on("click.bs.carousel.data-api", "[data-slide-to]", r);
    n(window).on("load", function () {
        n('[data-ride="carousel"]').each(function () {
            var t = n(this);
            i.call(t, t.data())
        })
    })
}(jQuery);
+function (n) {
    function r(t) {
        var i, r = t.attr("data-target") || (i = t.attr("href")) && i.replace(/.*(?=#[^\s]+$)/, "");
        return n(r)
    }
    function i(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.collapse")
                , f = n.extend({}, t.DEFAULTS, u.data(), "object" == typeof i && i);
            !r && f.toggle && /show|hide/.test(i) && (f.toggle = !1);
            r || u.data("bs.collapse", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    var t = function (i, r) {
        this.$element = n(i);
        this.options = n.extend({}, t.DEFAULTS, r);
        this.$trigger = n('[data-toggle="collapse"][href="#' + i.id + '"],[data-toggle="collapse"][data-target="#' + i.id + '"]');
        this.transitioning = null;
        this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger);
        this.options.toggle && this.toggle()
    }, u;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 350;
    t.DEFAULTS = {
        toggle: !0
    };
    t.prototype.dimension = function () {
        var n = this.$element.hasClass("width");
        return n ? "width" : "height"
    }
        ;
    t.prototype.show = function () {
        var f, r, e, u, o, s;
        if (!this.transitioning && !this.$element.hasClass("in") && (r = this.$parent && this.$parent.children(".panel").children(".in, .collapsing"),
            !(r && r.length && (f = r.data("bs.collapse"),
                f && f.transitioning)) && (e = n.Event("show.bs.collapse"),
                    this.$element.trigger(e),
                    !e.isDefaultPrevented()))) {
            if (r && r.length && (i.call(r, "hide"),
                f || r.data("bs.collapse", null)),
                u = this.dimension(),
                this.$element.removeClass("collapse").addClass("collapsing")[u](0).attr("aria-expanded", !0),
                this.$trigger.removeClass("collapsed").attr("aria-expanded", !0),
                this.transitioning = 1,
                o = function () {
                    this.$element.removeClass("collapsing").addClass("collapse in")[u]("");
                    this.transitioning = 0;
                    this.$element.trigger("shown.bs.collapse")
                }
                ,
                !n.support.transition)
                return o.call(this);
            s = n.camelCase(["scroll", u].join("-"));
            this.$element.one("bsTransitionEnd", n.proxy(o, this)).emulateTransitionEnd(t.TRANSITION_DURATION)[u](this.$element[0][s])
        }
    }
        ;
    t.prototype.hide = function () {
        var r, i, u;
        if (!this.transitioning && this.$element.hasClass("in") && (r = n.Event("hide.bs.collapse"),
            this.$element.trigger(r),
            !r.isDefaultPrevented()))
            return i = this.dimension(),
                this.$element[i](this.$element[i]())[0].offsetHeight,
                this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1),
                this.$trigger.addClass("collapsed").attr("aria-expanded", !1),
                this.transitioning = 1,
                u = function () {
                    this.transitioning = 0;
                    this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse")
                }
                ,
                n.support.transition ? void this.$element[i](0).one("bsTransitionEnd", n.proxy(u, this)).emulateTransitionEnd(t.TRANSITION_DURATION) : u.call(this)
    }
        ;
    t.prototype.toggle = function () {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    }
        ;
    t.prototype.getParent = function () {
        return n(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each(n.proxy(function (t, i) {
            var u = n(i);
            this.addAriaAndCollapsedClass(r(u), u)
        }, this)).end()
    }
        ;
    t.prototype.addAriaAndCollapsedClass = function (n, t) {
        var i = n.hasClass("in");
        n.attr("aria-expanded", i);
        t.toggleClass("collapsed", !i).attr("aria-expanded", i)
    }
        ;
    u = n.fn.collapse;
    n.fn.collapse = i;
    n.fn.collapse.Constructor = t;
    n.fn.collapse.noConflict = function () {
        return n.fn.collapse = u,
            this
    }
        ;
    n(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function (t) {
        var u = n(this);
        u.attr("data-target") || t.preventDefault();
        var f = r(u)
            , e = f.data("bs.collapse")
            , o = e ? "toggle" : u.data();
        i.call(f, o)
    })
}(jQuery);
+function (n) {
    function r(t) {
        t && 3 === t.which || (n(o).remove(),
            n(i).each(function () {
                var r = n(this)
                    , i = u(r)
                    , f = {
                        relatedTarget: this
                    };
                i.hasClass("open") && (i.trigger(t = n.Event("hide.bs.dropdown", f)),
                    t.isDefaultPrevented() || (r.attr("aria-expanded", "false"),
                        i.removeClass("open").trigger("hidden.bs.dropdown", f)))
            }))
    }
    function u(t) {
        var i = t.attr("data-target"), r;
        return i || (i = t.attr("href"),
            i = i && /#[A-Za-z]/.test(i) && i.replace(/.*(?=#[^\s]*$)/, "")),
            r = i && n(i),
            r && r.length ? r : t.parent()
    }
    function e(i) {
        return this.each(function () {
            var r = n(this)
                , u = r.data("bs.dropdown");
            u || r.data("bs.dropdown", u = new t(this));
            "string" == typeof i && u[i].call(r)
        })
    }
    var o = ".dropdown-backdrop", i = '[data-toggle="dropdown"]', t = function (t) {
        n(t).on("click.bs.dropdown", this.toggle)
    }, f;
    t.VERSION = "3.3.4";
    t.prototype.toggle = function (t) {
        var f = n(this), i, o, e;
        if (!f.is(".disabled, :disabled")) {
            if (i = u(f),
                o = i.hasClass("open"),
                r(),
                !o) {
                if ("ontouchstart" in document.documentElement && !i.closest(".navbar-nav").length && n('<div class="dropdown-backdrop"/>').insertAfter(n(this)).on("click", r),
                    e = {
                        relatedTarget: this
                    },
                    i.trigger(t = n.Event("show.bs.dropdown", e)),
                    t.isDefaultPrevented())
                    return;
                f.trigger("focus").attr("aria-expanded", "true");
                i.toggleClass("open").trigger("shown.bs.dropdown", e)
            }
            return !1
        }
    }
        ;
    t.prototype.keydown = function (t) {
        var e, o, s, h, f, r;
        if (/(38|40|27|32)/.test(t.which) && !/input|textarea/i.test(t.target.tagName) && (e = n(this),
            t.preventDefault(),
            t.stopPropagation(),
            !e.is(".disabled, :disabled"))) {
            if (o = u(e),
                s = o.hasClass("open"),
                !s && 27 != t.which || s && 27 == t.which)
                return 27 == t.which && o.find(i).trigger("focus"),
                    e.trigger("click");
            h = " li:not(.disabled):visible a";
            f = o.find('[role="menu"]' + h + ', [role="listbox"]' + h);
            f.length && (r = f.index(t.target),
                38 == t.which && r > 0 && r--,
                40 == t.which && r < f.length - 1 && r++,
                ~r || (r = 0),
                f.eq(r).trigger("focus"))
        }
    }
        ;
    f = n.fn.dropdown;
    n.fn.dropdown = e;
    n.fn.dropdown.Constructor = t;
    n.fn.dropdown.noConflict = function () {
        return n.fn.dropdown = f,
            this
    }
        ;
    n(document).on("click.bs.dropdown.data-api", r).on("click.bs.dropdown.data-api", ".dropdown form", function (n) {
        n.stopPropagation()
    }).on("click.bs.dropdown.data-api", i, t.prototype.toggle).on("keydown.bs.dropdown.data-api", i, t.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="menu"]', t.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="listbox"]', t.prototype.keydown)
}(jQuery);
+function (n) {
    function i(i, r) {
        return this.each(function () {
            var f = n(this)
                , u = f.data("bs.modal")
                , e = n.extend({}, t.DEFAULTS, f.data(), "object" == typeof i && i);
            u || f.data("bs.modal", u = new t(this, e));
            "string" == typeof i ? u[i](r) : e.show && u.show(r)
        })
    }
    var t = function (t, i) {
        this.options = i;
        this.$body = n(document.body);
        this.$element = n(t);
        this.$dialog = this.$element.find(".modal-dialog");
        this.$backdrop = null;
        this.isShown = null;
        this.originalBodyPad = null;
        this.scrollbarWidth = 0;
        this.ignoreBackdropClick = !1;
        this.options.remote && this.$element.find(".modal-content").load(this.options.remote, n.proxy(function () {
            this.$element.trigger("loaded.bs.modal")
        }, this))
    }, r;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 300;
    t.BACKDROP_TRANSITION_DURATION = 150;
    t.DEFAULTS = {
        backdrop: !0,
        keyboard: !0,
        show: !0
    };
    t.prototype.toggle = function (n) {
        return this.isShown ? this.hide() : this.show(n)
    }
        ;
    t.prototype.show = function (i) {
        var r = this
            , u = n.Event("show.bs.modal", {
                relatedTarget: i
            });
        this.$element.trigger(u);
        this.isShown || u.isDefaultPrevented() || (this.isShown = !0,
            this.checkScrollbar(),
            this.setScrollbar(),
            this.$body.addClass("modal-open"),
            this.escape(),
            this.resize(),
            this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', n.proxy(this.hide, this)),
            this.$dialog.on("mousedown.dismiss.bs.modal", function () {
                r.$element.one("mouseup.dismiss.bs.modal", function (t) {
                    n(t.target).is(r.$element) && (r.ignoreBackdropClick = !0)
                })
            }),
            this.backdrop(function () {
                var f = n.support.transition && r.$element.hasClass("fade"), u;
                r.$element.parent().length || r.$element.appendTo(r.$body);
                r.$element.show().scrollTop(0);
                r.adjustDialog();
                f && r.$element[0].offsetWidth;
                r.$element.addClass("in").attr("aria-hidden", !1);
                r.enforceFocus();
                u = n.Event("shown.bs.modal", {
                    relatedTarget: i
                });
                f ? r.$dialog.one("bsTransitionEnd", function () {
                    r.$element.trigger("focus").trigger(u)
                }).emulateTransitionEnd(t.TRANSITION_DURATION) : r.$element.trigger("focus").trigger(u)
            }))
    }
        ;
    t.prototype.hide = function (i) {
        i && i.preventDefault();
        i = n.Event("hide.bs.modal");
        this.$element.trigger(i);
        this.isShown && !i.isDefaultPrevented() && (this.isShown = !1,
            this.escape(),
            this.resize(),
            n(document).off("focusin.bs.modal"),
            this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal").off("mouseup.dismiss.bs.modal"),
            this.$dialog.off("mousedown.dismiss.bs.modal"),
            n.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", n.proxy(this.hideModal, this)).emulateTransitionEnd(t.TRANSITION_DURATION) : this.hideModal())
    }
        ;
    t.prototype.enforceFocus = function () {
        n(document).off("focusin.bs.modal").on("focusin.bs.modal", n.proxy(function (n) {
            this.$element[0] === n.target || this.$element.has(n.target).length || this.$element.trigger("focus")
        }, this))
    }
        ;
    t.prototype.escape = function () {
        this.isShown && this.options.keyboard ? this.$element.on("keydown.dismiss.bs.modal", n.proxy(function (n) {
            27 == n.which && this.hide()
        }, this)) : this.isShown || this.$element.off("keydown.dismiss.bs.modal")
    }
        ;
    t.prototype.resize = function () {
        this.isShown ? n(window).on("resize.bs.modal", n.proxy(this.handleUpdate, this)) : n(window).off("resize.bs.modal")
    }
        ;
    t.prototype.hideModal = function () {
        var n = this;
        this.$element.hide();
        this.backdrop(function () {
            n.$body.removeClass("modal-open");
            n.resetAdjustments();
            n.resetScrollbar();
            n.$element.trigger("hidden.bs.modal")
        })
    }
        ;
    t.prototype.removeBackdrop = function () {
        this.$backdrop && this.$backdrop.remove();
        this.$backdrop = null
    }
        ;
    t.prototype.backdrop = function (i) {
        var e = this, f = this.$element.hasClass("fade") ? "fade" : "", r, u;
        if (this.isShown && this.options.backdrop) {
            if (r = n.support.transition && f,
                this.$backdrop = n('<div class="modal-backdrop ' + f + '" />').appendTo(this.$body),
                this.$element.on("click.dismiss.bs.modal", n.proxy(function (n) {
                    return this.ignoreBackdropClick ? void (this.ignoreBackdropClick = !1) : void (n.target === n.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus() : this.hide()))
                }, this)),
                r && this.$backdrop[0].offsetWidth,
                this.$backdrop.addClass("in"),
                !i)
                return;
            r ? this.$backdrop.one("bsTransitionEnd", i).emulateTransitionEnd(t.BACKDROP_TRANSITION_DURATION) : i()
        } else
            !this.isShown && this.$backdrop ? (this.$backdrop.removeClass("in"),
                u = function () {
                    e.removeBackdrop();
                    i && i()
                }
                ,
                n.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one("bsTransitionEnd", u).emulateTransitionEnd(t.BACKDROP_TRANSITION_DURATION) : u()) : i && i()
    }
        ;
    t.prototype.handleUpdate = function () {
        this.adjustDialog()
    }
        ;
    t.prototype.adjustDialog = function () {
        var n = this.$element[0].scrollHeight > document.documentElement.clientHeight;
        this.$element.css({
            paddingLeft: !this.bodyIsOverflowing && n ? this.scrollbarWidth : "",
            paddingRight: this.bodyIsOverflowing && !n ? this.scrollbarWidth : ""
        })
    }
        ;
    t.prototype.resetAdjustments = function () {
        this.$element.css({
            paddingLeft: "",
            paddingRight: ""
        })
    }
        ;
    t.prototype.checkScrollbar = function () {
        var n = window.innerWidth, t;
        n || (t = document.documentElement.getBoundingClientRect(),
            n = t.right - Math.abs(t.left));
        this.bodyIsOverflowing = document.body.clientWidth < n;
        this.scrollbarWidth = this.measureScrollbar()
    }
        ;
    t.prototype.setScrollbar = function () {
        var n = parseInt(this.$body.css("padding-right") || 0, 10);
        this.originalBodyPad = document.body.style.paddingRight || "";
        this.bodyIsOverflowing && this.$body.css("padding-right", n + this.scrollbarWidth)
    }
        ;
    t.prototype.resetScrollbar = function () {
        this.$body.css("padding-right", this.originalBodyPad)
    }
        ;
    t.prototype.measureScrollbar = function () {
        var n = document.createElement("div"), t;
        return n.className = "modal-scrollbar-measure",
            this.$body.append(n),
            t = n.offsetWidth - n.clientWidth,
            this.$body[0].removeChild(n),
            t
    }
        ;
    r = n.fn.modal;
    n.fn.modal = i;
    n.fn.modal.Constructor = t;
    n.fn.modal.noConflict = function () {
        return n.fn.modal = r,
            this
    }
        ;
    n(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function (t) {
        var r = n(this)
            , f = r.attr("href")
            , u = n(r.attr("data-target") || f && f.replace(/.*(?=#[^\s]+$)/, ""))
            , e = u.data("bs.modal") ? "toggle" : n.extend({
                remote: !/#/.test(f) && f
            }, u.data(), r.data());
        r.is("a") && t.preventDefault();
        u.one("show.bs.modal", function (n) {
            n.isDefaultPrevented() || u.one("hidden.bs.modal", function () {
                r.is(":visible") && r.trigger("focus")
            })
        });
        i.call(u, e, this)
    })
}(jQuery);
+function (n) {
    function r(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.tooltip")
                , f = "object" == typeof i && i;
            (r || !/destroy|hide/.test(i)) && (r || u.data("bs.tooltip", r = new t(this, f)),
                "string" == typeof i && r[i]())
        })
    }
    var t = function (n, t) {
        this.type = null;
        this.options = null;
        this.enabled = null;
        this.timeout = null;
        this.hoverState = null;
        this.$element = null;
        this.init("tooltip", n, t)
    }, i;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 150;
    t.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1,
        viewport: {
            selector: "body",
            padding: 0
        }
    };
    t.prototype.init = function (t, i, r) {
        var f, e, u, o, s;
        if (this.enabled = !0,
            this.type = t,
            this.$element = n(i),
            this.options = this.getOptions(r),
            this.$viewport = this.options.viewport && n(this.options.viewport.selector || this.options.viewport),
            this.$element[0] instanceof document.constructor && !this.options.selector)
            throw new Error("`selector` option must be specified when initializing " + this.type + " on the window.document object!");
        for (f = this.options.trigger.split(" "),
            e = f.length; e--;)
            if (u = f[e],
                "click" == u)
                this.$element.on("click." + this.type, this.options.selector, n.proxy(this.toggle, this));
            else
                "manual" != u && (o = "hover" == u ? "mouseenter" : "focusin",
                    s = "hover" == u ? "mouseleave" : "focusout",
                    this.$element.on(o + "." + this.type, this.options.selector, n.proxy(this.enter, this)),
                    this.$element.on(s + "." + this.type, this.options.selector, n.proxy(this.leave, this)));
        this.options.selector ? this._options = n.extend({}, this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    }
        ;
    t.prototype.getDefaults = function () {
        return t.DEFAULTS
    }
        ;
    t.prototype.getOptions = function (t) {
        return t = n.extend({}, this.getDefaults(), this.$element.data(), t),
            t.delay && "number" == typeof t.delay && (t.delay = {
                show: t.delay,
                hide: t.delay
            }),
            t
    }
        ;
    t.prototype.getDelegateOptions = function () {
        var t = {}
            , i = this.getDefaults();
        return this._options && n.each(this._options, function (n, r) {
            i[n] != r && (t[n] = r)
        }),
            t
    }
        ;
    t.prototype.enter = function (t) {
        var i = t instanceof this.constructor ? t : n(t.currentTarget).data("bs." + this.type);
        return i && i.$tip && i.$tip.is(":visible") ? void (i.hoverState = "in") : (i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()),
            n(t.currentTarget).data("bs." + this.type, i)),
            clearTimeout(i.timeout),
            i.hoverState = "in",
            i.options.delay && i.options.delay.show ? void (i.timeout = setTimeout(function () {
                "in" == i.hoverState && i.show()
            }, i.options.delay.show)) : i.show())
    }
        ;
    t.prototype.leave = function (t) {
        var i = t instanceof this.constructor ? t : n(t.currentTarget).data("bs." + this.type);
        return i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()),
            n(t.currentTarget).data("bs." + this.type, i)),
            clearTimeout(i.timeout),
            i.hoverState = "out",
            i.options.delay && i.options.delay.hide ? void (i.timeout = setTimeout(function () {
                "out" == i.hoverState && i.hide()
            }, i.options.delay.hide)) : i.hide()
    }
        ;
    t.prototype.show = function () {
        var c = n.Event("show.bs." + this.type), l, p, h;
        if (this.hasContent() && this.enabled) {
            if (this.$element.trigger(c),
                l = n.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]),
                c.isDefaultPrevented() || !l)
                return;
            var u = this
                , r = this.tip()
                , a = this.getUID(this.type);
            this.setContent();
            r.attr("id", a);
            this.$element.attr("aria-describedby", a);
            this.options.animation && r.addClass("fade");
            var i = "function" == typeof this.options.placement ? this.options.placement.call(this, r[0], this.$element[0]) : this.options.placement
                , v = /\s?auto?\s?/i
                , y = v.test(i);
            y && (i = i.replace(v, "") || "top");
            r.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(i).data("bs." + this.type, this);
            this.options.container ? r.appendTo(this.options.container) : r.insertAfter(this.$element);
            var f = this.getPosition()
                , o = r[0].offsetWidth
                , s = r[0].offsetHeight;
            if (y) {
                var w = i
                    , b = this.options.container ? n(this.options.container) : this.$element.parent()
                    , e = this.getPosition(b);
                i = "bottom" == i && f.bottom + s > e.bottom ? "top" : "top" == i && f.top - s < e.top ? "bottom" : "right" == i && f.right + o > e.width ? "left" : "left" == i && f.left - o < e.left ? "right" : i;
                r.removeClass(w).addClass(i)
            }
            p = this.getCalculatedOffset(i, f, o, s);
            this.applyPlacement(p, i);
            h = function () {
                var n = u.hoverState;
                u.$element.trigger("shown.bs." + u.type);
                u.hoverState = null;
                "out" == n && u.leave(u)
            }
                ;
            n.support.transition && this.$tip.hasClass("fade") ? r.one("bsTransitionEnd", h).emulateTransitionEnd(t.TRANSITION_DURATION) : h()
        }
    }
        ;
    t.prototype.applyPlacement = function (t, i) {
        var r = this.tip(), l = r[0].offsetWidth, e = r[0].offsetHeight, o = parseInt(r.css("margin-top"), 10), s = parseInt(r.css("margin-left"), 10), h, f, u;
        isNaN(o) && (o = 0);
        isNaN(s) && (s = 0);
        t.top = t.top + o;
        t.left = t.left + s;
        n.offset.setOffset(r[0], n.extend({
            using: function (n) {
                r.css({
                    top: Math.round(n.top),
                    left: Math.round(n.left)
                })
            }
        }, t), 0);
        r.addClass("in");
        h = r[0].offsetWidth;
        f = r[0].offsetHeight;
        "top" == i && f != e && (t.top = t.top + e - f);
        u = this.getViewportAdjustedDelta(i, t, h, f);
        u.left ? t.left += u.left : t.top += u.top;
        var c = /top|bottom/.test(i)
            , a = c ? 2 * u.left - l + h : 2 * u.top - e + f
            , v = c ? "offsetWidth" : "offsetHeight";
        r.offset(t);
        this.replaceArrow(a, r[0][v], c)
    }
        ;
    t.prototype.replaceArrow = function (n, t, i) {
        this.arrow().css(i ? "left" : "top", 50 * (1 - n / t) + "%").css(i ? "top" : "left", "")
    }
        ;
    t.prototype.setContent = function () {
        var n = this.tip()
            , t = this.getTitle();
        n.find(".tooltip-inner")[this.options.html ? "html" : "text"](t);
        n.removeClass("fade in top bottom left right")
    }
        ;
    t.prototype.hide = function (i) {
        function f() {
            "in" != u.hoverState && r.detach();
            u.$element.removeAttr("aria-describedby").trigger("hidden.bs." + u.type);
            i && i()
        }
        var u = this
            , r = n(this.$tip)
            , e = n.Event("hide.bs." + this.type);
        return this.$element.trigger(e),
            e.isDefaultPrevented() ? void 0 : (r.removeClass("in"),
                n.support.transition && r.hasClass("fade") ? r.one("bsTransitionEnd", f).emulateTransitionEnd(t.TRANSITION_DURATION) : f(),
                this.hoverState = null,
                this)
    }
        ;
    t.prototype.fixTitle = function () {
        var n = this.$element;
        (n.attr("title") || "string" != typeof n.attr("data-original-title")) && n.attr("data-original-title", n.attr("title") || "").attr("title", "")
    }
        ;
    t.prototype.hasContent = function () {
        return this.getTitle()
    }
        ;
    t.prototype.getPosition = function (t) {
        t = t || this.$element;
        var u = t[0]
            , r = "BODY" == u.tagName
            , i = u.getBoundingClientRect();
        null == i.width && (i = n.extend({}, i, {
            width: i.right - i.left,
            height: i.bottom - i.top
        }));
        var f = r ? {
            top: 0,
            left: 0
        } : t.offset()
            , e = {
                scroll: r ? document.documentElement.scrollTop || document.body.scrollTop : t.scrollTop()
            }
            , o = r ? {
                width: n(window).width(),
                height: n(window).height()
            } : null;
        return n.extend({}, i, e, o, f)
    }
        ;
    t.prototype.getCalculatedOffset = function (n, t, i, r) {
        return "bottom" == n ? {
            top: t.top + t.height,
            left: t.left + t.width / 2 - i / 2
        } : "top" == n ? {
            top: t.top - r,
            left: t.left + t.width / 2 - i / 2
        } : "left" == n ? {
            top: t.top + t.height / 2 - r / 2,
            left: t.left - i
        } : {
            top: t.top + t.height / 2 - r / 2,
            left: t.left + t.width
        }
    }
        ;
    t.prototype.getViewportAdjustedDelta = function (n, t, i, r) {
        var f = {
            top: 0,
            left: 0
        }, e, u, o, s, h, c;
        return this.$viewport ? (e = this.options.viewport && this.options.viewport.padding || 0,
            u = this.getPosition(this.$viewport),
            /right|left/.test(n) ? (o = t.top - e - u.scroll,
                s = t.top + e - u.scroll + r,
                o < u.top ? f.top = u.top - o : s > u.top + u.height && (f.top = u.top + u.height - s)) : (h = t.left - e,
                    c = t.left + e + i,
                    h < u.left ? f.left = u.left - h : c > u.width && (f.left = u.left + u.width - c)),
            f) : f
    }
        ;
    t.prototype.getTitle = function () {
        var t = this.$element
            , n = this.options;
        return t.attr("data-original-title") || ("function" == typeof n.title ? n.title.call(t[0]) : n.title)
    }
        ;
    t.prototype.getUID = function (n) {
        do
            n += ~~(1e6 * Math.random());
        while (document.getElementById(n));
        return n
    }
        ;
    t.prototype.tip = function () {
        return this.$tip = this.$tip || n(this.options.template)
    }
        ;
    t.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    }
        ;
    t.prototype.enable = function () {
        this.enabled = !0
    }
        ;
    t.prototype.disable = function () {
        this.enabled = !1
    }
        ;
    t.prototype.toggleEnabled = function () {
        this.enabled = !this.enabled
    }
        ;
    t.prototype.toggle = function (t) {
        var i = this;
        t && (i = n(t.currentTarget).data("bs." + this.type),
            i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()),
                n(t.currentTarget).data("bs." + this.type, i)));
        i.tip().hasClass("in") ? i.leave(i) : i.enter(i)
    }
        ;
    t.prototype.destroy = function () {
        var n = this;
        clearTimeout(this.timeout);
        this.hide(function () {
            n.$element.off("." + n.type).removeData("bs." + n.type)
        })
    }
        ;
    i = n.fn.tooltip;
    n.fn.tooltip = r;
    n.fn.tooltip.Constructor = t;
    n.fn.tooltip.noConflict = function () {
        return n.fn.tooltip = i,
            this
    }
}(jQuery);
+function (n) {
    function r(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.popover")
                , f = "object" == typeof i && i;
            (r || !/destroy|hide/.test(i)) && (r || u.data("bs.popover", r = new t(this, f)),
                "string" == typeof i && r[i]())
        })
    }
    var t = function (n, t) {
        this.init("popover", n, t)
    }, i;
    if (!n.fn.tooltip)
        throw new Error("Popover%20requires%20tooltip.html");
    t.VERSION = "3.3.4";
    t.DEFAULTS = n.extend({}, n.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover" role="tooltip"><div class="arrow"><\/div><h3 class="popover-title"><\/h3><div class="popover-content"><\/div><\/div>'
    });
    t.prototype = n.extend({}, n.fn.tooltip.Constructor.prototype);
    t.prototype.constructor = t;
    t.prototype.getDefaults = function () {
        return t.DEFAULTS
    }
        ;
    t.prototype.setContent = function () {
        var n = this.tip()
            , i = this.getTitle()
            , t = this.getContent();
        n.find(".popover-title")[this.options.html ? "html" : "text"](i);
        n.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof t ? "html" : "append" : "text"](t);
        n.removeClass("fade top bottom left right in");
        n.find(".popover-title").html() || n.find(".popover-title").hide()
    }
        ;
    t.prototype.hasContent = function () {
        return this.getTitle() || this.getContent()
    }
        ;
    t.prototype.getContent = function () {
        var t = this.$element
            , n = this.options;
        return t.attr("data-content") || ("function" == typeof n.content ? n.content.call(t[0]) : n.content)
    }
        ;
    t.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    }
        ;
    i = n.fn.popover;
    n.fn.popover = r;
    n.fn.popover.Constructor = t;
    n.fn.popover.noConflict = function () {
        return n.fn.popover = i,
            this
    }
}(jQuery);
+function (n) {
    function t(i, r) {
        this.$body = n(document.body);
        this.$scrollElement = n(n(i).is(document.body) ? window : i);
        this.options = n.extend({}, t.DEFAULTS, r);
        this.selector = (this.options.target || "") + " .nav li > a";
        this.offsets = [];
        this.targets = [];
        this.activeTarget = null;
        this.scrollHeight = 0;
        this.$scrollElement.on("scroll.bs.scrollspy", n.proxy(this.process, this));
        this.refresh();
        this.process()
    }
    function i(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.scrollspy")
                , f = "object" == typeof i && i;
            r || u.data("bs.scrollspy", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    t.VERSION = "3.3.4";
    t.DEFAULTS = {
        offset: 10
    };
    t.prototype.getScrollHeight = function () {
        return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight)
    }
        ;
    t.prototype.refresh = function () {
        var t = this
            , i = "offset"
            , r = 0;
        this.offsets = [];
        this.targets = [];
        this.scrollHeight = this.getScrollHeight();
        n.isWindow(this.$scrollElement[0]) || (i = "position",
            r = this.$scrollElement.scrollTop());
        this.$body.find(this.selector).map(function () {
            var f = n(this)
                , u = f.data("target") || f.attr("href")
                , t = /^#./.test(u) && n(u);
            return t && t.length && t.is(":visible") && [[t[i]().top + r, u]] || null
        }).sort(function (n, t) {
            return n[0] - t[0]
        }).each(function () {
            t.offsets.push(this[0]);
            t.targets.push(this[1])
        })
    }
        ;
    t.prototype.process = function () {
        var n, i = this.$scrollElement.scrollTop() + this.options.offset, f = this.getScrollHeight(), e = this.options.offset + f - this.$scrollElement.height(), t = this.offsets, r = this.targets, u = this.activeTarget;
        if (this.scrollHeight != f && this.refresh(),
            i >= e)
            return u != (n = r[r.length - 1]) && this.activate(n);
        if (u && i < t[0])
            return this.activeTarget = null,
                this.clear();
        for (n = t.length; n--;)
            u != r[n] && i >= t[n] && (void 0 === t[n + 1] || i < t[n + 1]) && this.activate(r[n])
    }
        ;
    t.prototype.activate = function (t) {
        this.activeTarget = t;
        this.clear();
        var r = this.selector + '[data-target="' + t + '"],' + this.selector + '[href="' + t + '"]'
            , i = n(r).parents("li").addClass("active");
        i.parent(".dropdown-menu").length && (i = i.closest("li.dropdown").addClass("active"));
        i.trigger("activate.bs.scrollspy")
    }
        ;
    t.prototype.clear = function () {
        n(this.selector).parentsUntil(this.options.target, ".active").removeClass("active")
    }
        ;
    var r = n.fn.scrollspy;
    n.fn.scrollspy = i;
    n.fn.scrollspy.Constructor = t;
    n.fn.scrollspy.noConflict = function () {
        return n.fn.scrollspy = r,
            this
    }
        ;
    n(window).on("load.bs.scrollspy.data-api", function () {
        n('[data-spy="scroll"]').each(function () {
            var t = n(this);
            i.call(t, t.data())
        })
    })
}(jQuery);
+function (n) {
    function r(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.tab");
            r || u.data("bs.tab", r = new t(this));
            "string" == typeof i && r[i]()
        })
    }
    var t = function (t) {
        this.element = n(t)
    }, u, i;
    t.VERSION = "3.3.4";
    t.TRANSITION_DURATION = 150;
    t.prototype.show = function () {
        var t = this.element, f = t.closest("ul:not(.dropdown-menu)"), i = t.data("target"), u;
        if (i || (i = t.attr("href"),
            i = i && i.replace(/.*(?=#[^\s]*$)/, "")),
            !t.parent("li").hasClass("active")) {
            var r = f.find(".active:last a")
                , e = n.Event("hide.bs.tab", {
                    relatedTarget: t[0]
                })
                , o = n.Event("show.bs.tab", {
                    relatedTarget: r[0]
                });
            (r.trigger(e),
                t.trigger(o),
                o.isDefaultPrevented() || e.isDefaultPrevented()) || (u = n(i),
                    this.activate(t.closest("li"), f),
                    this.activate(u, u.parent(), function () {
                        r.trigger({
                            type: "hidden.bs.tab",
                            relatedTarget: t[0]
                        });
                        t.trigger({
                            type: "shown.bs.tab",
                            relatedTarget: r[0]
                        })
                    }))
        }
    }
        ;
    t.prototype.activate = function (i, r, u) {
        function e() {
            f.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1);
            i.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0);
            o ? (i[0].offsetWidth,
                i.addClass("in")) : i.removeClass("fade");
            i.parent(".dropdown-menu").length && i.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0);
            u && u()
        }
        var f = r.find("> .active")
            , o = u && n.support.transition && (f.length && f.hasClass("fade") || !!r.find("> .fade").length);
        f.length && o ? f.one("bsTransitionEnd", e).emulateTransitionEnd(t.TRANSITION_DURATION) : e();
        f.removeClass("in")
    }
        ;
    u = n.fn.tab;
    n.fn.tab = r;
    n.fn.tab.Constructor = t;
    n.fn.tab.noConflict = function () {
        return n.fn.tab = u,
            this
    }
        ;
    i = function (t) {
        t.preventDefault();
        r.call(n(this), "show")
    }
        ;
    n(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', i).on("click.bs.tab.data-api", '[data-toggle="pill"]', i)
}(jQuery);
+function (n) {
    function i(i) {
        return this.each(function () {
            var u = n(this)
                , r = u.data("bs.affix")
                , f = "object" == typeof i && i;
            r || u.data("bs.affix", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    var t = function (i, r) {
        this.options = n.extend({}, t.DEFAULTS, r);
        this.$target = n(this.options.target).on("scroll.bs.affix.data-api", n.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", n.proxy(this.checkPositionWithEventLoop, this));
        this.$element = n(i);
        this.affixed = null;
        this.unpin = null;
        this.pinnedOffset = null;
        this.checkPosition()
    }, r;
    t.VERSION = "3.3.4";
    t.RESET = "affix affix-top affix-bottom";
    t.DEFAULTS = {
        offset: 0,
        target: window
    };
    t.prototype.getState = function (n, t, i, r) {
        var u = this.$target.scrollTop()
            , f = this.$element.offset()
            , e = this.$target.height();
        if (null != i && "top" == this.affixed)
            return i > u ? "top" : !1;
        if ("bottom" == this.affixed)
            return null != i ? u + this.unpin <= f.top ? !1 : "bottom" : n - r >= u + e ? !1 : "bottom";
        var o = null == this.affixed
            , s = o ? u : f.top
            , h = o ? e : t;
        return null != i && i >= u ? "top" : null != r && s + h >= n - r ? "bottom" : !1
    }
        ;
    t.prototype.getPinnedOffset = function () {
        if (this.pinnedOffset)
            return this.pinnedOffset;
        this.$element.removeClass(t.RESET).addClass("affix");
        var n = this.$target.scrollTop()
            , i = this.$element.offset();
        return this.pinnedOffset = i.top - n
    }
        ;
    t.prototype.checkPositionWithEventLoop = function () {
        setTimeout(n.proxy(this.checkPosition, this), 1)
    }
        ;
    t.prototype.checkPosition = function () {
        var i, e, o;
        if (this.$element.is(":visible")) {
            var s = this.$element.height()
                , r = this.options.offset
                , f = r.top
                , u = r.bottom
                , h = n(document.body).height();
            if ("object" != typeof r && (u = f = r),
                "function" == typeof f && (f = r.top(this.$element)),
                "function" == typeof u && (u = r.bottom(this.$element)),
                i = this.getState(h, s, f, u),
                this.affixed != i) {
                if (null != this.unpin && this.$element.css("top", ""),
                    e = "affix" + (i ? "-" + i : ""),
                    o = n.Event(e + ".bs.affix"),
                    this.$element.trigger(o),
                    o.isDefaultPrevented())
                    return;
                this.affixed = i;
                this.unpin = "bottom" == i ? this.getPinnedOffset() : null;
                this.$element.removeClass(t.RESET).addClass(e).trigger(e.replace("affix", "affixed") + ".bs.affix")
            }
            "bottom" == i && this.$element.offset({
                top: h - s - u
            })
        }
    }
        ;
    r = n.fn.affix;
    n.fn.affix = i;
    n.fn.affix.Constructor = t;
    n.fn.affix.noConflict = function () {
        return n.fn.affix = r,
            this
    }
        ;
    n(window).on("load", function () {
        n('[data-spy="affix"]').each(function () {
            var r = n(this)
                , t = r.data();
            t.offset = t.offset || {};
            null != t.offsetBottom && (t.offset.bottom = t.offsetBottom);
            null != t.offsetTop && (t.offset.top = t.offsetTop);
            i.call(r, t)
        })
    })
}(jQuery);
window.Modernizr = function (n, t, i) {
    function l(n) {
        c.cssText = n
    }
    function at(n, t) {
        return l(y.join(n + ";") + (t || ""))
    }
    function h(n, t) {
        return typeof n === t
    }
    function a(n, t) {
        return !!~("" + n).indexOf(t)
    }
    function ut(n, t) {
        var u, r;
        for (u in n)
            if (r = n[u],
                !a(r, "-") && c[r] !== i)
                return t == "pfx" ? r : !0;
        return !1
    }
    function vt(n, t, r) {
        var f, u;
        for (f in n)
            if (u = t[n[f]],
                u !== i)
                return r === !1 ? n[f] : h(u, "function") ? u.bind(r || t) : u;
        return !1
    }
    function f(n, t, i) {
        var r = n.charAt(0).toUpperCase() + n.slice(1)
            , u = (n + " " + st.join(r + " ") + r).split(" ");
        return h(t, "string") || h(t, "undefined") ? ut(u, t) : (u = (n + " " + ht.join(r + " ") + r).split(" "),
            vt(u, t, i))
    }
    function yt() {
        u.input = function (i) {
            for (var r = 0, u = i.length; r < u; r++)
                w[i[r]] = i[r] in e;
            return w.list && (w.list = !!t.createElement("datalist") && !!n.HTMLDataListElement),
                w
        }("autocomplete autofocus list placeholder max min multiple pattern required step".split(" "));
        u.inputtypes = function (n) {
            for (var u = 0, r, f, o, h = n.length; u < h; u++)
                e.setAttribute("type", f = n[u]),
                    r = e.type !== "text",
                    r && (e.value = g,
                        e.style.cssText = "position:absolute;visibility:hidden;",
                        /^range$/.test(f) && e.style.WebkitAppearance !== i ? (s.appendChild(e),
                            o = t.defaultView,
                            r = o.getComputedStyle && o.getComputedStyle(e, null).WebkitAppearance !== "textfield" && e.offsetHeight !== 0,
                            s.removeChild(e)) : /^(search|tel)$/.test(f) || (r = /^(url|email)$/.test(f) ? e.checkValidity && e.checkValidity() === !1 : e.value != g)),
                    ct[n[u]] = !!r;
            return ct
        }("search tel url email datetime date month week time datetime-local number range color".split(" "))
    }
    var u = {}, d = !0, s = t.documentElement, o = "modernizr", ft = t.createElement(o), c = ft.style, e = t.createElement("input"), g = ":)", et = {}.toString, y = " -webkit- -moz- -o- -ms- ".split(" "), ot = "Webkit Moz O ms", st = ot.split(" "), ht = ot.toLowerCase().split(" "), p = {
        svg: "http://www.w3.org/2000/svg"
    }, r = {}, ct = {}, w = {}, nt = [], tt = nt.slice, b, v = function (n, i, r, u) {
        var l, a, c, v, f = t.createElement("div"), h = t.body, e = h || t.createElement("body");
        if (parseInt(r, 10))
            while (r--)
                c = t.createElement("div"),
                    c.id = u ? u[r] : o + (r + 1),
                    f.appendChild(c);
        return l = ["&#173;", '<style id="s', o, '">', n, "<\/style>"].join(""),
            f.id = o,
            (h ? f : e).innerHTML += l,
            e.appendChild(f),
            h || (e.style.background = "",
                e.style.overflow = "hidden",
                v = s.style.overflow,
                s.style.overflow = "hidden",
                s.appendChild(e)),
            a = i(f, n),
            h ? f.parentNode.removeChild(f) : (e.parentNode.removeChild(e),
                s.style.overflow = v),
            !!a
    }, lt = function () {
        function n(n, u) {
            u = u || t.createElement(r[n] || "div");
            n = "on" + n;
            var f = n in u;
            return f || (u.setAttribute || (u = t.createElement("div")),
                u.setAttribute && u.removeAttribute && (u.setAttribute(n, ""),
                    f = h(u[n], "function"),
                    h(u[n], "undefined") || (u[n] = i),
                    u.removeAttribute(n))),
                u = null,
                f
        }
        var r = {
            select: "input",
            change: "input",
            submit: "form",
            reset: "form",
            error: "img",
            load: "img",
            abort: "img"
        };
        return n
    }(), it = {}.hasOwnProperty, rt, k;
    rt = !h(it, "undefined") && !h(it.call, "undefined") ? function (n, t) {
        return it.call(n, t)
    }
        : function (n, t) {
            return t in n && h(n.constructor.prototype[t], "undefined")
        }
        ;
    Function.prototype.bind || (Function.prototype.bind = function (n) {
        var t = this, i, r;
        if (typeof t != "function")
            throw new TypeError;
        return i = tt.call(arguments, 1),
            r = function () {
                var f, e, u;
                return this instanceof r ? (f = function () { }
                    ,
                    f.prototype = t.prototype,
                    e = new f,
                    u = t.apply(e, i.concat(tt.call(arguments))),
                    Object(u) === u ? u : e) : t.apply(n, i.concat(tt.call(arguments)))
            }
            ,
            r
    }
    );
    r.flexbox = function () {
        return f("flexWrap")
    }
        ;
    r.flexboxlegacy = function () {
        return f("boxDirection")
    }
        ;
    r.canvas = function () {
        var n = t.createElement("canvas");
        return !!n.getContext && !!n.getContext("2d")
    }
        ;
    r.canvastext = function () {
        return !!u.canvas && !!h(t.createElement("canvas").getContext("2d").fillText, "function")
    }
        ;
    r.webgl = function () {
        return !!n.WebGLRenderingContext
    }
        ;
    r.touch = function () {
        var i;
        return "ontouchstart" in n || n.DocumentTouch && t instanceof DocumentTouch ? i = !0 : v(["@media (", y.join("touch-enabled),("), o, ")", "{#modernizr{top:9px;position:absolute}}"].join(""), function (n) {
            i = n.offsetTop === 9
        }),
            i
    }
        ;
    r.geolocation = function () {
        return "geolocation" in navigator
    }
        ;
    r.postmessage = function () {
        return !!n.postMessage
    }
        ;
    r.websqldatabase = function () {
        return !!n.openDatabase
    }
        ;
    r.indexedDB = function () {
        return !!f("indexedDB", n)
    }
        ;
    r.hashchange = function () {
        return lt("hashchange", n) && (t.documentMode === i || t.documentMode > 7)
    }
        ;
    r.history = function () {
        return !!n.history && !!history.pushState
    }
        ;
    r.draganddrop = function () {
        var n = t.createElement("div");
        return "draggable" in n || "ondragstart" in n && "ondrop" in n
    }
        ;
    r.websockets = function () {
        return "WebSocket" in n || "MozWebSocket" in n
    }
        ;
    r.rgba = function () {
        return l("background-color:rgba(150,255,150,.5)"),
            a(c.backgroundColor, "rgba")
    }
        ;
    r.hsla = function () {
        return l("background-color:hsla(120,40%,100%,.5)"),
            a(c.backgroundColor, "rgba") || a(c.backgroundColor, "hsla")
    }
        ;
    r.multiplebgs = function () {
        return l("background:url(https://),url(https://),red url(https://)"),
            /(url\s*\(.*?){3}/.test(c.background)
    }
        ;
    r.backgroundsize = function () {
        return f("backgroundSize")
    }
        ;
    r.borderimage = function () {
        return f("borderImage")
    }
        ;
    r.borderradius = function () {
        return f("borderRadius")
    }
        ;
    r.boxshadow = function () {
        return f("boxShadow")
    }
        ;
    r.textshadow = function () {
        return t.createElement("div").style.textShadow === ""
    }
        ;
    r.opacity = function () {
        return at("opacity:.55"),
            /^0.55$/.test(c.opacity)
    }
        ;
    r.cssanimations = function () {
        return f("animationName")
    }
        ;
    r.csscolumns = function () {
        return f("columnCount")
    }
        ;
    r.cssgradients = function () {
        var n = "background-image:";
        return l((n + "-webkit- ".split(" ").join("gradient(linear,left top,right bottom,from(#9f9),to(white));" + n) + y.join("linear-gradient(left top,#9f9, white);" + n)).slice(0, -n.length)),
            a(c.backgroundImage, "gradient")
    }
        ;
    r.cssreflections = function () {
        return f("boxReflect")
    }
        ;
    r.csstransforms = function () {
        return !!f("transform")
    }
        ;
    r.csstransforms3d = function () {
        var n = !!f("perspective");
        return n && "webkitPerspective" in s.style && v("@media (transform-3d),(-webkit-transform-3d){#modernizr{left:9px;position:absolute;height:3px;}}", function (t) {
            n = t.offsetLeft === 9 && t.offsetHeight === 3
        }),
            n
    }
        ;
    r.csstransitions = function () {
        return f("transition")
    }
        ;
    r.fontface = function () {
        var n;
        return v('@font-face {font-family:"font";src:url("https:///")}', function (i, r) {
            var f = t.getElementById("smodernizr")
                , u = f.sheet || f.styleSheet
                , e = u ? u.cssRules && u.cssRules[0] ? u.cssRules[0].cssText : u.cssText || "" : "";
            n = /src/i.test(e) && e.indexOf(r.split(" ")[0]) === 0
        }),
            n
    }
        ;
    r.generatedcontent = function () {
        var n;
        return v(["#", o, "{font:0/0 a}#", o, ':after{content:"', g, '";visibility:hidden;font:3px/1 a}'].join(""), function (t) {
            n = t.offsetHeight >= 3
        }),
            n
    }
        ;
    r.video = function () {
        var i = t.createElement("video")
            , n = !1;
        try {
            (n = !!i.canPlayType) && (n = new Boolean(n),
                n.ogg = i.canPlayType('video/ogg; codecs="theora"').replace(/^no$/, ""),
                n.h264 = i.canPlayType('video/mp4; codecs="avc1.42E01E"').replace(/^no$/, ""),
                n.webm = i.canPlayType('video/webm; codecs="vp8, vorbis"').replace(/^no$/, ""))
        } catch (r) { }
        return n
    }
        ;
    r.audio = function () {
        var i = t.createElement("audio")
            , n = !1;
        try {
            (n = !!i.canPlayType) && (n = new Boolean(n),
                n.ogg = i.canPlayType('audio/ogg; codecs="vorbis"').replace(/^no$/, ""),
                n.mp3 = i.canPlayType("audio/mpeg;").replace(/^no$/, ""),
                n.wav = i.canPlayType('audio/wav; codecs="1"').replace(/^no$/, ""),
                n.m4a = (i.canPlayType("audio/x-m4a;") || i.canPlayType("audio/aac;")).replace(/^no$/, ""))
        } catch (r) { }
        return n
    }
        ;
    r.localstorage = function () {
        try {
            return localStorage.setItem(o, o),
                localStorage.removeItem(o),
                !0
        } catch (n) {
            return !1
        }
    }
        ;
    r.sessionstorage = function () {
        try {
            return sessionStorage.setItem(o, o),
                sessionStorage.removeItem(o),
                !0
        } catch (n) {
            return !1
        }
    }
        ;
    r.webworkers = function () {
        return !!n.Worker
    }
        ;
    r.applicationcache = function () {
        return !!n.applicationCache
    }
        ;
    r.svg = function () {
        return !!t.createElementNS && !!t.createElementNS(p.svg, "svg").createSVGRect
    }
        ;
    r.inlinesvg = function () {
        var n = t.createElement("div");
        return n.innerHTML = "<svg/>",
            (n.firstChild && n.firstChild.namespaceURI) == p.svg
    }
        ;
    r.smil = function () {
        return !!t.createElementNS && /SVGAnimate/.test(et.call(t.createElementNS(p.svg, "animate")))
    }
        ;
    r.svgclippaths = function () {
        return !!t.createElementNS && /SVGClipPath/.test(et.call(t.createElementNS(p.svg, "clipPath")))
    }
        ;
    for (k in r)
        rt(r, k) && (b = k.toLowerCase(),
            u[b] = r[k](),
            nt.push((u[b] ? "" : "no-") + b));
    return u.input || yt(),
        u.addTest = function (n, t) {
            if (typeof n == "object")
                for (var r in n)
                    rt(n, r) && u.addTest(r, n[r]);
            else {
                if (n = n.toLowerCase(),
                    u[n] !== i)
                    return u;
                t = typeof t == "function" ? t() : t;
                typeof d != "undefined" && d && (s.className += " " + (t ? "" : "no-") + n);
                u[n] = t
            }
            return u
        }
        ,
        l(""),
        ft = e = null,
        function (n, t) {
            function v(n, t) {
                var i = n.createElement("p")
                    , r = n.getElementsByTagName("head")[0] || n.documentElement;
                return i.innerHTML = "x<style>" + t + "<\/style>",
                    r.insertBefore(i.lastChild, r.firstChild)
            }
            function s() {
                var n = r.elements;
                return typeof n == "string" ? n.split(" ") : n
            }
            function u(n) {
                var t = a[n[l]];
                return t || (t = {},
                    o++,
                    n[l] = o,
                    a[o] = t),
                    t
            }
            function h(n, r, f) {
                if (r || (r = t),
                    i)
                    return r.createElement(n);
                f || (f = u(r));
                var e;
                return e = f.cache[n] ? f.cache[n].cloneNode() : b.test(n) ? (f.cache[n] = f.createElem(n)).cloneNode() : f.createElem(n),
                    e.canHaveChildren && !w.test(n) ? f.frag.appendChild(e) : e
            }
            function y(n, r) {
                if (n || (n = t),
                    i)
                    return n.createDocumentFragment();
                r = r || u(n);
                for (var e = r.frag.cloneNode(), f = 0, o = s(), h = o.length; f < h; f++)
                    e.createElement(o[f]);
                return e
            }
            function p(n, t) {
                t.cache || (t.cache = {},
                    t.createElem = n.createElement,
                    t.createFrag = n.createDocumentFragment,
                    t.frag = t.createFrag());
                n.createElement = function (i) {
                    return r.shivMethods ? h(i, n, t) : t.createElem(i)
                }
                    ;
                n.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + s().join().replace(/\w+/g, function (n) {
                    return t.createElem(n),
                        t.frag.createElement(n),
                        'c("' + n + '")'
                }) + ");return n}")(r, t.frag)
            }
            function c(n) {
                n || (n = t);
                var f = u(n);
                return r.shivCSS && !e && !f.hasCSS && (f.hasCSS = !!v(n, "article,aside,figcaption,figure,footer,header,hgroup,nav,section{display:block}mark{background:#FF0;color:#000}")),
                    i || p(n, f),
                    n
            }
            var f = n.html5 || {}, w = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i, b = /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i, e, l = "_html5shiv", o = 0, a = {}, i, r;
            (function () {
                try {
                    var n = t.createElement("a");
                    n.innerHTML = "<xyz><\/xyz>";
                    e = "hidden" in n;
                    i = n.childNodes.length == 1 || function () {
                        t.createElement("a");
                        var n = t.createDocumentFragment();
                        return typeof n.cloneNode == "undefined" || typeof n.createDocumentFragment == "undefined" || typeof n.createElement == "undefined"
                    }()
                } catch (r) {
                    e = !0;
                    i = !0
                }
            }
            )();
            r = {
                elements: f.elements || "abbr article aside audio bdi canvas data datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video",
                shivCSS: f.shivCSS !== !1,
                supportsUnknownElements: i,
                shivMethods: f.shivMethods !== !1,
                type: "default",
                shivDocument: c,
                createElement: h,
                createDocumentFragment: y
            };
            n.html5 = r;
            c(t)
        }(this, t),
        u._version = "2.6.2",
        u._prefixes = y,
        u._domPrefixes = ht,
        u._cssomPrefixes = st,
        u.hasEvent = lt,
        u.testProp = function (n) {
            return ut([n])
        }
        ,
        u.testAllProps = f,
        u.testStyles = v,
        s.className = s.className.replace(/(^|\s)no-js(\s|$)/, "$1$2") + (d ? " js " + nt.join(" ") : ""),
        u
}(this, this.document),
    function (n, t, i) {
        function h(n) {
            return "[object Function]" == y.call(n)
        }
        function c(n) {
            return "string" == typeof n
        }
        function l() { }
        function w(n) {
            return !n || "loaded" == n || "complete" == n || "uninitialized" == n
        }
        function e() {
            var n = a.shift();
            v = 1;
            n ? n.t ? s(function () {
                ("c" == n.t ? u.injectCss : u.injectJs)(n.s, 0, n.a, n.x, n.e, 1)
            }, 0) : (n(),
                e()) : v = 0
        }
        function ut(n, i, f, h, c, l, y) {
            function k(t) {
                if (!nt && w(p.readyState) && (tt.r = nt = 1,
                    !v && e(),
                    p.onload = p.onreadystatechange = null,
                    t)) {
                    "img" != n && s(function () {
                        g.removeChild(p)
                    }, 50);
                    for (var u in r[i])
                        r[i].hasOwnProperty(u) && r[i][u].onload()
                }
            }
            var y = y || u.errorTimeout
                , p = t.createElement(n)
                , nt = 0
                , b = 0
                , tt = {
                    t: f,
                    s: i,
                    e: c,
                    a: l,
                    x: y
                };
            1 === r[i] && (b = 1,
                r[i] = []);
            "object" == n ? p.data = i : (p.src = i,
                p.type = n);
            p.width = p.height = "0";
            p.onerror = p.onload = p.onreadystatechange = function () {
                k.call(this, b)
            }
                ;
            a.splice(h, 0, tt);
            "img" != n && (b || 2 === r[i] ? (g.insertBefore(p, d ? null : o),
                s(k, y)) : r[i].push(p))
        }
        function ft(n, t, i, r, u) {
            return v = 0,
                t = t || "j",
                c(n) ? ut("c" == t ? et : nt, n, t, this.i++, i, r, u) : (a.splice(this.i++, 0, n),
                    1 == a.length && e()),
                this
        }
        function b() {
            var n = u;
            return n.loader = {
                load: ft,
                i: 0
            },
                n
        }
        var f = t.documentElement, s = n.setTimeout, o = t.getElementsByTagName("script")[0], y = {}.toString, a = [], v = 0, k = "MozAppearance" in f.style, d = k && !!t.createRange().compareNode, g = d ? f : o.parentNode, f = n.opera && "[object Opera]" == y.call(n.opera), f = !!t.attachEvent && !f, nt = k ? "object" : f ? "script" : "img", et = f ? "script" : nt, tt = Array.isArray || function (n) {
            return "[object Array]" == y.call(n)
        }
            , p = [], r = {}, it = {
                timeout: function (n, t) {
                    return t.length && (n.timeout = t[0]),
                        n
                }
            }, rt, u;
        u = function (n) {
            function a(n) {
                for (var n = n.split("!"), f = p.length, t = n.pop(), e = n.length, t = {
                    url: t,
                    origUrl: t,
                    prefixes: n
                }, u, r, i = 0; i < e; i++)
                    r = n[i].split("="),
                        (u = it[r.shift()]) && (t = u(t, r));
                for (i = 0; i < f; i++)
                    t = p[i](t);
                return t
            }
            function f(n, t, u, f, e) {
                var o = a(n)
                    , s = o.autoCallback;
                o.url.split(".").pop().split("?").shift();
                o.bypass || (t && (t = h(t) ? t : t[n] || t[f] || t[n.split("http://coderthemes.com/").pop().split("?")[0]]),
                    o.instead ? o.instead(n, t, u, f, e) : (r[o.url] ? o.noexec = !0 : r[o.url] = 1,
                        u.load(o.url, o.forceCSS || !o.forceJS && "css" == o.url.split(".").pop().split("?").shift() ? "c" : i, o.noexec, o.attrs, o.timeout),
                        (h(t) || h(s)) && u.load(function () {
                            b();
                            t && t(o.origUrl, e, f);
                            s && s(o.origUrl, e, f);
                            r[o.url] = 2
                        })))
            }
            function s(n, t) {
                function a(n, o) {
                    if (n) {
                        if (c(n))
                            o || (i = function () {
                                var n = [].slice.call(arguments);
                                s.apply(this, n);
                                u()
                            }
                            ),
                                f(n, i, t, 0, e);
                        else if (Object(n) === n)
                            for (r in v = function () {
                                var t = 0, i;
                                for (i in n)
                                    n.hasOwnProperty(i) && t++;
                                return t
                            }(),
                                n)
                                n.hasOwnProperty(r) && (!o && !--v && (h(i) ? i = function () {
                                    var n = [].slice.call(arguments);
                                    s.apply(this, n);
                                    u()
                                }
                                    : i[r] = function (n) {
                                        return function () {
                                            var t = [].slice.call(arguments);
                                            n && n.apply(this, t);
                                            u()
                                        }
                                    }(s[r])),
                                    f(n[r], i, t, r, e))
                    } else
                        o || u()
                }
                var e = !!n.test, o = n.load || n.both, i = n.callback || l, s = i, u = n.complete || l, v, r;
                a(e ? n.yep : n.nope, !!o);
                o && a(o)
            }
            var e, t, o = this.yepnope.loader;
            if (c(n))
                f(n, 0, o, 0);
            else if (tt(n))
                for (e = 0; e < n.length; e++)
                    t = n[e],
                        c(t) ? f(t, 0, o, 0) : tt(t) ? u(t) : Object(t) === t && s(t, o);
            else
                Object(n) === n && s(n, o)
        }
            ;
        u.addPrefix = function (n, t) {
            it[n] = t
        }
            ;
        u.addFilter = function (n) {
            p.push(n)
        }
            ;
        u.errorTimeout = 1e4;
        null == t.readyState && t.addEventListener && (t.readyState = "loading",
            t.addEventListener("DOMContentLoaded", rt = function () {
                t.removeEventListener("DOMContentLoaded", rt, 0);
                t.readyState = "complete"
            }
                , 0));
        n.yepnope = b();
        n.yepnope.executeStack = e;
        n.yepnope.injectJs = function (n, i, r, f, h, c) {
            var a = t.createElement("script"), v, y, f = f || u.errorTimeout;
            a.src = n;
            for (y in r)
                a.setAttribute(y, r[y]);
            i = c ? e : i || l;
            a.onreadystatechange = a.onload = function () {
                !v && w(a.readyState) && (v = 1,
                    i(),
                    a.onload = a.onreadystatechange = null)
            }
                ;
            s(function () {
                v || (v = 1,
                    i(1))
            }, f);
            h ? a.onload() : o.parentNode.insertBefore(a, o)
        }
            ;
        n.yepnope.injectCss = function (n, i, r, u, f, h) {
            var u = t.createElement("link"), c, i = h ? e : i || l;
            u.href = n;
            u.rel = "stylesheet";
            u.type = "text/css";
            for (c in r)
                u.setAttribute(c, r[c]);
            f || (o.parentNode.insertBefore(u, o),
                s(i, 0))
        }
    }(this, document);
Modernizr.load = function () {
    yepnope.apply(window, [].slice.call(arguments, 0))
}
    ,
    function () {
        var i, n, t = function (n, t) {
            return function () {
                return n.apply(t, arguments)
            }
        };
        i = function () {
            function n() { }
            return n.prototype.extend = function (n, t) {
                var i, r;
                for (i in n)
                    r = n[i],
                        null != r && (t[i] = r);
                return t
            }
                ,
                n.prototype.isMobile = function (n) {
                    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(n)
                }
                ,
                n
        }();
        n = this.WeakMap || (n = function () {
            function n() {
                this.keys = [];
                this.values = []
            }
            return n.prototype.get = function (n) {
                var t, u, i, f, r;
                for (r = this.keys,
                    t = i = 0,
                    f = r.length; f > i; t = ++i)
                    if (u = r[t],
                        u === n)
                        return this.values[t]
            }
                ,
                n.prototype.set = function (n, t) {
                    var i, f, r, e, u;
                    for (u = this.keys,
                        i = r = 0,
                        e = u.length; e > r; i = ++r)
                        if (f = u[i],
                            f === n)
                            return void (this.values[i] = t);
                    return this.keys.push(n),
                        this.values.push(t)
                }
                ,
                n
        }());
        this.WOW = function () {
            function r(i) {
                null == i && (i = {});
                this.scrollCallback = t(this.scrollCallback, this);
                this.scrollHandler = t(this.scrollHandler, this);
                this.start = t(this.start, this);
                this.scrolled = !0;
                this.config = this.util().extend(i, this.defaults);
                this.animationNameCache = new n
            }
            return r.prototype.defaults = {
                boxClass: "wow",
                animateClass: "animated",
                offset: 0,
                mobile: !0
            },
                r.prototype.init = function () {
                    var n;
                    return this.element = window.document.documentElement,
                        "interactive" === (n = document.readyState) || "complete" === n ? this.start() : document.addEventListener("DOMContentLoaded", this.start)
                }
                ,
                r.prototype.start = function () {
                    var i, n, r, t;
                    if (this.boxes = this.element.getElementsByClassName(this.config.boxClass),
                        this.boxes.length) {
                        if (this.disabled())
                            return this.resetStyle();
                        for (t = this.boxes,
                            n = 0,
                            r = t.length; r > n; n++)
                            i = t[n],
                                this.applyStyle(i, !0);
                        return window.addEventListener("scroll", this.scrollHandler, !1),
                            window.addEventListener("resize", this.scrollHandler, !1),
                            this.interval = setInterval(this.scrollCallback, 50)
                    }
                }
                ,
                r.prototype.stop = function () {
                    return window.removeEventListener("scroll", this.scrollHandler, !1),
                        window.removeEventListener("resize", this.scrollHandler, !1),
                        null != this.interval ? clearInterval(this.interval) : void 0
                }
                ,
                r.prototype.show = function (n) {
                    return this.applyStyle(n),
                        n.className = "" + n.className + " " + this.config.animateClass
                }
                ,
                r.prototype.applyStyle = function (n, t) {
                    var i, r, u;
                    return r = n.getAttribute("data-wow-duration"),
                        i = n.getAttribute("data-wow-delay"),
                        u = n.getAttribute("data-wow-iteration"),
                        this.animate(function (f) {
                            return function () {
                                return f.customStyle(n, t, r, i, u)
                            }
                        }(this))
                }
                ,
                r.prototype.animate = function () {
                    return "requestAnimationFrame" in window ? function (n) {
                        return window.requestAnimationFrame(n)
                    }
                        : function (n) {
                            return n()
                        }
                }(),
                r.prototype.resetStyle = function () {
                    var r, n, u, t, i;
                    for (t = this.boxes,
                        i = [],
                        n = 0,
                        u = t.length; u > n; n++)
                        r = t[n],
                            i.push(r.setAttribute("style", "visibility: visible;"));
                    return i
                }
                ,
                r.prototype.customStyle = function (n, t, i, r, u) {
                    return t && this.cacheAnimationName(n),
                        n.style.visibility = t ? "hidden" : "visible",
                        i && this.vendorSet(n.style, {
                            animationDuration: i
                        }),
                        r && this.vendorSet(n.style, {
                            animationDelay: r
                        }),
                        u && this.vendorSet(n.style, {
                            animationIterationCount: u
                        }),
                        this.vendorSet(n.style, {
                            animationName: t ? "none" : this.cachedAnimationName(n)
                        }),
                        n
                }
                ,
                r.prototype.vendors = ["moz", "webkit"],
                r.prototype.vendorSet = function (n, t) {
                    var i, r, u, f = [];
                    for (i in t)
                        r = t[i],
                            n["" + i] = r,
                            f.push(function () {
                                var t, o, f, e;
                                for (f = this.vendors,
                                    e = [],
                                    t = 0,
                                    o = f.length; o > t; t++)
                                    u = f[t],
                                        e.push(n["" + u + i.charAt(0).toUpperCase() + i.substr(1)] = r);
                                return e
                            }
                                .call(this));
                    return f
                }
                ,
                r.prototype.vendorCSS = function (n, t) {
                    var i, u, e, r, o, f;
                    for (u = window.getComputedStyle(n),
                        i = u.getPropertyCSSValue(t),
                        f = this.vendors,
                        r = 0,
                        o = f.length; o > r; r++)
                        e = f[r],
                            i = i || u.getPropertyCSSValue("-" + e + "-" + t);
                    return i
                }
                ,
                r.prototype.animationName = function (n) {
                    var t;
                    try {
                        t = this.vendorCSS(n, "animation-name").cssText
                    } catch (i) {
                        t = window.getComputedStyle(n).getPropertyValue("animation-name")
                    }
                    return "none" === t ? "" : t
                }
                ,
                r.prototype.cacheAnimationName = function (n) {
                    return this.animationNameCache.set(n, this.animationName(n))
                }
                ,
                r.prototype.cachedAnimationName = function (n) {
                    return this.animationNameCache.get(n)
                }
                ,
                r.prototype.scrollHandler = function () {
                    return this.scrolled = !0
                }
                ,
                r.prototype.scrollCallback = function () {
                    var n;
                    if (this.scrolled && (this.scrolled = !1,
                        this.boxes = function () {
                            var t, u, i, r;
                            for (i = this.boxes,
                                r = [],
                                t = 0,
                                u = i.length; u > t; t++)
                                n = i[t],
                                    n && (this.isVisible(n) ? this.show(n) : r.push(n));
                            return r
                        }
                            .call(this),
                        !this.boxes.length))
                        return this.stop()
                }
                ,
                r.prototype.offsetTop = function (n) {
                    for (var t; void 0 === n.offsetTop;)
                        n = n.parentNode;
                    for (t = n.offsetTop; n = n.offsetParent;)
                        t += n.offsetTop;
                    return t
                }
                ,
                r.prototype.isVisible = function (n) {
                    var r, u, t, f, i;
                    return u = n.getAttribute("data-wow-offset") || this.config.offset,
                        i = window.pageYOffset,
                        f = i + this.element.clientHeight - u,
                        t = this.offsetTop(n),
                        r = t + n.clientHeight,
                        f >= t && r >= i
                }
                ,
                r.prototype.util = function () {
                    return this._util || (this._util = new i)
                }
                ,
                r.prototype.disabled = function () {
                    return !this.config.mobile && this.util().isMobile(navigator.userAgent)
                }
                ,
                r
        }()
    }
        .call(this),
    function (n) {
        function i(n) {
            return typeof n == "object" ? n : {
                top: n,
                left: n
            }
        }
        var t = n.scrollTo = function (t, i, r) {
            n(window).scrollTo(t, i, r)
        }
            ;
        t.defaults = {
            axis: "xy",
            duration: parseFloat(n.fn.jquery) >= 1.3 ? 0 : 1,
            limit: !0
        };
        t.window = function () {
            return n(window)._scrollable()
        }
            ;
        n.fn._scrollable = function () {
            return this.map(function () {
                var t = this, r = !t.nodeName || n.inArray(t.nodeName.toLowerCase(), ["iframe", "#document", "html", "body"]) != -1, i;
                return r ? (i = (t.contentWindow || t).document || t.ownerDocument || t,
                    /webkit/i.test(navigator.userAgent) || i.compatMode == "BackCompat" ? i.body : i.documentElement) : t
            })
        }
            ;
        n.fn.scrollTo = function (r, u, f) {
            return typeof u == "object" && (f = u,
                u = 0),
                typeof f == "function" && (f = {
                    onAfter: f
                }),
                r == "max" && (r = 9e9),
                f = n.extend({}, t.defaults, f),
                u = u || f.duration,
                f.queue = f.queue && f.axis.length > 1,
                f.queue && (u /= 2),
                f.offset = i(f.offset),
                f.over = i(f.over),
                this._scrollable().each(function () {
                    function l(n) {
                        h.animate(o, u, f.easing, n && function () {
                            n.call(this, e, f)
                        }
                        )
                    }
                    if (r != null) {
                        var s = this, h = n(s), e = r, c, o = {}, a = h.is("html,body");
                        switch (typeof e) {
                            case "number":
                            case "string":
                                if (/^([+-]=?)?\d+(\.\d+)?(px|%)?$/.test(e)) {
                                    e = i(e);
                                    break
                                }
                                if (e = n(e, this),
                                    !e.length)
                                    return;
                            case "object":
                                (e.is || e.style) && (c = (e = n(e)).offset())
                        }
                        n.each(f.axis.split(""), function (n, i) {
                            var y = i == "x" ? "Left" : "Top", u = y.toLowerCase(), r = "scroll" + y, p = s[r], w = t.max(s, i), v;
                            c ? (o[r] = c[u] + (a ? 0 : p - h.offset()[u]),
                                f.margin && (o[r] -= parseInt(e.css("margin" + y)) || 0,
                                    o[r] -= parseInt(e.css("border" + y + "Width")) || 0),
                                o[r] += f.offset[u] || 0,
                                f.over[u] && (o[r] += e[i == "x" ? "width" : "height"]() * f.over[u])) : (v = e[u],
                                    o[r] = v.slice && v.slice(-1) == "%" ? parseFloat(v) / 100 * w : v);
                            f.limit && /^\d+$/.test(o[r]) && (o[r] = o[r] <= 0 ? 0 : Math.min(o[r], w));
                            !n && f.queue && (p != o[r] && l(f.onAfterFirst),
                                delete o[r])
                        });
                        l(f.onAfter)
                    }
                }).end()
        }
            ;
        t.max = function (t, i) {
            var r = i == "x" ? "Width" : "Height"
                , u = "scroll" + r;
            if (!n(t).is("html,body"))
                return t[u] - n(t)[r.toLowerCase()]();
            var f = "client" + r
                , e = t.ownerDocument.documentElement
                , o = t.ownerDocument.body;
            return Math.max(e[u], o[u]) - Math.min(e[f], o[f])
        }
    }(jQuery),
    function (n) {
        var e = !1, c = !1, b = 5e3, k = 2e3, r = 0, d = function () {
            var n = document.getElementsByTagName("script")
                , n = n[n.length - 1].src.split("?")[0];
            return 0 < n.split("http://coderthemes.com/").length ? n.split("http://coderthemes.com/").slice(0, -1).join("http://coderthemes.com/") + "/" : ""
        }(), v = ["ms", "moz", "webkit", "o"], t = window.requestAnimationFrame || !1, i = window.cancelAnimationFrame || !1, y, o, u, f;
        if (!t)
            for (y in v)
                o = v[y],
                    t || (t = window[o + "RequestAnimationFrame"]),
                    i || (i = window[o + "CancelAnimationFrame"] || window[o + "CancelRequestAnimationFrame"]);
        var s = window.MutationObserver || window.WebKitMutationObserver || !1
            , p = {
                zindex: "auto",
                cursoropacitymin: 0,
                cursoropacitymax: 1,
                cursorcolor: "#424242",
                cursorwidth: "5px",
                cursorborder: "1px solid #fff",
                cursorborderradius: "5px",
                scrollspeed: 60,
                mousescrollstep: 24,
                touchbehavior: !1,
                hwacceleration: !0,
                usetransition: !0,
                boxzoom: !1,
                dblclickzoom: !0,
                gesturezoom: !0,
                grabcursorenabled: !0,
                autohidemode: !0,
                background: "",
                iframeautoresize: !0,
                cursorminheight: 32,
                preservenativescrolling: !0,
                railoffset: !1,
                bouncescroll: !0,
                spacebarenabled: !0,
                railpadding: {
                    top: 0,
                    right: 0,
                    left: 0,
                    bottom: 0
                },
                disableoutline: !0,
                horizrailenabled: !0,
                railalign: "right",
                railvalign: "bottom",
                enabletranslate3d: !0,
                enablemousewheel: !0,
                enablekeyboard: !0,
                smoothscroll: !0,
                sensitiverail: !0,
                enablemouselockapi: !0,
                cursorfixedheight: !1,
                directionlockdeadzone: 6,
                hidecursordelay: 400,
                nativeparentscrolling: !0,
                enablescrollonselection: !0,
                overflowx: !0,
                overflowy: !0,
                cursordragspeed: .3,
                rtlmode: !1,
                cursordragontouch: !1,
                oneaxismousemode: "auto"
            }
            , l = !1
            , g = function () {
                var r, n, i, t;
                if (l)
                    return l;
                for (r = document.createElement("DIV"),
                    n = {
                        haspointerlock: "pointerLockElement" in document || "mozPointerLockElement" in document || "webkitPointerLockElement" in document
                    },
                    n.isopera = ("opera" in window),
                    n.isopera12 = n.isopera && ("getUserMedia" in navigator),
                    n.isoperamini = "[object OperaMini]" === Object.prototype.toString.call(window.operamini),
                    n.isie = ("all" in document) && ("attachEvent" in r) && !n.isopera,
                    n.isieold = n.isie && !("msInterpolationMode" in r.style),
                    n.isie7 = n.isie && !n.isieold && (!("documentMode" in document) || 7 == document.documentMode),
                    n.isie8 = n.isie && ("documentMode" in document) && 8 == document.documentMode,
                    n.isie9 = n.isie && ("performance" in window) && 9 <= document.documentMode,
                    n.isie10 = n.isie && ("performance" in window) && 10 <= document.documentMode,
                    n.isie9mobile = /iemobile.9/i.test(navigator.userAgent),
                    n.isie9mobile && (n.isie9 = !1),
                    n.isie7mobile = !n.isie9mobile && n.isie7 && /iemobile/i.test(navigator.userAgent),
                    n.ismozilla = ("MozAppearance" in r.style),
                    n.iswebkit = ("WebkitAppearance" in r.style),
                    n.ischrome = ("chrome" in window),
                    n.ischrome22 = n.ischrome && n.haspointerlock,
                    n.ischrome26 = n.ischrome && ("transition" in r.style),
                    n.cantouch = ("ontouchstart" in document.documentElement) || ("ontouchstart" in window),
                    n.hasmstouch = window.navigator.msPointerEnabled || !1,
                    n.ismac = /^mac$/i.test(navigator.platform),
                    n.isios = n.cantouch && /iphone|ipad|ipod/i.test(navigator.platform),
                    n.isios4 = n.isios && !("seal" in Object),
                    n.isandroid = /android/i.test(navigator.userAgent),
                    n.trstyle = !1,
                    n.hastransform = !1,
                    n.hastranslate3d = !1,
                    n.transitionstyle = !1,
                    n.hastransition = !1,
                    n.transitionend = !1,
                    i = ["transform", "msTransform", "webkitTransform", "MozTransform", "OTransform"],
                    t = 0; t < i.length; t++)
                    if ("undefined" != typeof r.style[i[t]]) {
                        n.trstyle = i[t];
                        break
                    }
                n.hastransform = !1 != n.trstyle;
                n.hastransform && (r.style[n.trstyle] = "translate3d(1px,2px,3px)",
                    n.hastranslate3d = /translate3d/.test(r.style[n.trstyle]));
                n.transitionstyle = !1;
                n.prefixstyle = "";
                n.transitionend = !1;
                for (var i = "transition webkitTransition MozTransition OTransition OTransition msTransition KhtmlTransition".split(" "), u = " -webkit- -moz- -o- -o -ms- -khtml-".split(" "), f = "transitionend webkitTransitionEnd transitionend otransitionend oTransitionEnd msTransitionEnd KhtmlTransitionEnd".split(" "), t = 0; t < i.length; t++)
                    if (i[t] in r.style) {
                        n.transitionstyle = i[t];
                        n.prefixstyle = u[t];
                        n.transitionend = f[t];
                        break
                    }
                n.ischrome26 && (n.prefixstyle = u[1]);
                n.hastransition = n.transitionstyle;
                n: {
                    for (i = ["-moz-grab", "-webkit-grab", "grab"],
                        (n.ischrome && !n.ischrome22 || n.isie) && (i = []),
                        t = 0; t < i.length; t++)
                        if (u = i[t],
                            r.style.cursor = u,
                            r.style.cursor == u) {
                            i = u;
                            break n
                        }
                    i = "url(http://www.google.com/intl/en_ALL/mapfiles/openhand.cur),n-resize"
                }
                return n.cursorgrabvalue = i,
                    n.hasmousecapture = "setCapture" in r,
                    n.hasMutationObserver = !1 !== s,
                    l = n
            }
            , nt = function (u, f) {
                function it() {
                    var n = o.win, t;
                    if ("zIndex" in n)
                        return n.zIndex();
                    for (; 0 < n.length && 9 != n[0].nodeType;) {
                        if (t = n.css("zIndex"),
                            !isNaN(t) && 0 != t)
                            return parseInt(t);
                        n = n.parent()
                    }
                    return !1
                }
                function l(n, t, i) {
                    return t = n.css(t),
                        n = parseFloat(t),
                        isNaN(n) ? (n = tt[t] || 0,
                            i = 3 == n ? i ? o.win.outerHeight() - o.win.innerHeight() : o.win.outerWidth() - o.win.innerWidth() : 1,
                            o.isie8 && n && (n += 1),
                            i ? n : 0) : n
                }
                function y(n, t, i, r) {
                    o._bind(n, t, function (r) {
                        r = r ? r : window.event;
                        var u = {
                            original: r,
                            target: r.target || r.srcElement,
                            type: "wheel",
                            deltaMode: "MozMousePixelScroll" == r.type ? 0 : 1,
                            deltaX: 0,
                            deltaZ: 0,
                            preventDefault: function () {
                                return r.preventDefault ? r.preventDefault() : r.returnValue = !1,
                                    !1
                            },
                            stopImmediatePropagation: function () {
                                r.stopImmediatePropagation ? r.stopImmediatePropagation() : r.cancelBubble = !0
                            }
                        };
                        return "mousewheel" == t ? (u.deltaY = -.025 * r.wheelDelta,
                            r.wheelDeltaX && (u.deltaX = -.025 * r.wheelDeltaX)) : u.deltaY = r.detail,
                            i.call(n, u)
                    }, r)
                }
                function nt(n, t, i) {
                    var u, r;
                    if (0 == n.deltaMode ? (u = -Math.floor(n.deltaX * (o.opt.mousescrollstep / 54)),
                        r = -Math.floor(n.deltaY * (o.opt.mousescrollstep / 54))) : 1 == n.deltaMode && (u = -Math.floor(n.deltaX * o.opt.mousescrollstep),
                            r = -Math.floor(n.deltaY * o.opt.mousescrollstep)),
                        t && o.opt.oneaxismousemode && 0 == u && r && (u = r,
                            r = 0),
                        u && (o.scrollmom && o.scrollmom.stop(),
                            o.lastdeltax += u,
                            o.debounced("mousewheelx", function () {
                                var n = o.lastdeltax;
                                o.lastdeltax = 0;
                                o.rail.drag || o.doScrollLeftBy(n)
                            }, 120)),
                        r) {
                        if (o.opt.nativeparentscrolling && i && !o.ispage && !o.zoomactive)
                            if (0 > r) {
                                if (o.getScrollTop() >= o.page.maxh)
                                    return !0
                            } else if (0 >= o.getScrollTop())
                                return !0;
                        o.scrollmom && o.scrollmom.stop();
                        o.lastdeltay += r;
                        o.debounced("mousewheely", function () {
                            var n = o.lastdeltay;
                            o.lastdeltay = 0;
                            o.rail.drag || o.doScrollBy(n)
                        }, 120)
                    }
                    return n.stopImmediatePropagation(),
                        n.preventDefault()
                }
                var o = this, a, h, v, tt;
                if (this.version = "3.5.0",
                    this.name = "nicescroll",
                    this.me = f,
                    this.opt = {
                        doc: n("body"),
                        win: !1
                    },
                    n.extend(this.opt, p),
                    this.opt.snapbackspeed = 80,
                    u)
                    for (a in o.opt)
                        "undefined" != typeof u[a] && (o.opt[a] = u[a]);
                this.iddoc = (this.doc = o.opt.doc) && this.doc[0] ? this.doc[0].id || "" : "";
                this.ispage = /BODY|HTML/.test(o.opt.win ? o.opt.win[0].nodeName : this.doc[0].nodeName);
                this.haswrapper = !1 !== o.opt.win;
                this.win = o.opt.win || (this.ispage ? n(window) : this.doc);
                this.docscroll = this.ispage && !this.haswrapper ? n(window) : this.win;
                this.body = n("body");
                this.iframe = this.isfixed = this.viewport = !1;
                this.isiframe = "IFRAME" == this.doc[0].nodeName && "IFRAME" == this.win[0].nodeName;
                this.istextarea = "TEXTAREA" == this.win[0].nodeName;
                this.forcescreen = !1;
                this.canshowonmouseevent = "scroll" != o.opt.autohidemode;
                this.page = this.view = this.onzoomout = this.onzoomin = this.onscrollcancel = this.onscrollend = this.onscrollstart = this.onclick = this.ongesturezoom = this.onkeypress = this.onmousewheel = this.onmousemove = this.onmouseup = this.onmousedown = !1;
                this.scroll = {
                    x: 0,
                    y: 0
                };
                this.scrollratio = {
                    x: 0,
                    y: 0
                };
                this.cursorheight = 20;
                this.scrollvaluemax = 0;
                this.observerremover = this.observer = this.scrollmom = this.scrollrunning = this.checkrtlmode = !1;
                do
                    this.id = "ascrail" + k++;
                while (document.getElementById(this.id));
                this.hasmousefocus = this.hasfocus = this.zoomactive = this.zoom = this.selectiondrag = this.cursorfreezed = this.cursor = this.rail = !1;
                this.visibility = !0;
                this.hidden = this.locked = !1;
                this.cursoractive = !0;
                this.overflowx = o.opt.overflowx;
                this.overflowy = o.opt.overflowy;
                this.nativescrollingarea = !1;
                this.checkarea = 0;
                this.events = [];
                this.saved = {};
                this.delaylist = {};
                this.synclist = {};
                this.lastdeltay = this.lastdeltax = 0;
                this.detected = g();
                h = n.extend({}, this.detected);
                this.ishwscroll = (this.canhwscroll = h.hastransform && o.opt.hwacceleration) && o.haswrapper;
                this.istouchcapable = !1;
                h.cantouch && h.ischrome && !h.isios && !h.isandroid && (this.istouchcapable = !0,
                    h.cantouch = !1);
                h.cantouch && h.ismozilla && !h.isios && !h.isandroid && (this.istouchcapable = !0,
                    h.cantouch = !1);
                o.opt.enablemouselockapi || (h.hasmousecapture = !1,
                    h.haspointerlock = !1);
                this.delayed = function (n, t, i, r) {
                    var u = o.delaylist[n]
                        , f = (new Date).getTime();
                    if (!r && u && u.tt)
                        return !1;
                    u && u.tt && clearTimeout(u.tt);
                    u && u.last + i > f && !u.tt ? o.delaylist[n] = {
                        last: f + i,
                        tt: setTimeout(function () {
                            o.delaylist[n].tt = 0;
                            t.call()
                        }, i)
                    } : u && u.tt || (o.delaylist[n] = {
                        last: f,
                        tt: 0
                    },
                        setTimeout(function () {
                            t.call()
                        }, 0))
                }
                    ;
                this.debounced = function (n, t, i) {
                    var r = o.delaylist[n];
                    (new Date).getTime();
                    o.delaylist[n] = t;
                    r || setTimeout(function () {
                        var t = o.delaylist[n];
                        o.delaylist[n] = !1;
                        t.call()
                    }, i)
                }
                    ;
                this.synched = function (n, i) {
                    return o.synclist[n] = i,
                        function () {
                            o.onsync || (t(function () {
                                o.onsync = !1;
                                for (n in o.synclist) {
                                    var t = o.synclist[n];
                                    t && t.call(o);
                                    o.synclist[n] = !1
                                }
                            }),
                                o.onsync = !0)
                        }(),
                        n
                }
                    ;
                this.unsynched = function (n) {
                    o.synclist[n] && (o.synclist[n] = !1)
                }
                    ;
                this.css = function (n, t) {
                    for (var i in t)
                        o.saved.css.push([n, i, n.css(i)]),
                            n.css(i, t[i])
                }
                    ;
                this.scrollTop = function (n) {
                    return "undefined" == typeof n ? o.getScrollTop() : o.setScrollTop(n)
                }
                    ;
                this.scrollLeft = function (n) {
                    return "undefined" == typeof n ? o.getScrollLeft() : o.setScrollLeft(n)
                }
                    ;
                BezierClass = function (n, t, i, r, u, f, e) {
                    this.st = n;
                    this.ed = t;
                    this.spd = i;
                    this.p1 = r || 0;
                    this.p2 = u || 1;
                    this.p3 = f || 0;
                    this.p4 = e || 1;
                    this.ts = (new Date).getTime();
                    this.df = this.ed - this.st
                }
                    ;
                BezierClass.prototype = {
                    B2: function (n) {
                        return 3 * n * n * (1 - n)
                    },
                    B3: function (n) {
                        return 3 * n * (1 - n) * (1 - n)
                    },
                    B4: function (n) {
                        return (1 - n) * (1 - n) * (1 - n)
                    },
                    getNow: function () {
                        var n = 1 - ((new Date).getTime() - this.ts) / this.spd
                            , t = this.B2(n) + this.B3(n) + this.B4(n);
                        return 0 > n ? this.ed : this.st + Math.round(this.df * t)
                    },
                    update: function (n, t) {
                        return this.st = this.getNow(),
                            this.ed = n,
                            this.spd = t,
                            this.ts = (new Date).getTime(),
                            this.df = this.ed - this.st,
                            this
                    }
                };
                this.ishwscroll ? (this.doc.translate = {
                    x: 0,
                    y: 0,
                    tx: "0px",
                    ty: "0px"
                },
                    h.hastranslate3d && h.isios && this.doc.css("-webkit-backface-visibility", "hidden"),
                    v = function () {
                        var n = o.doc.css(h.trstyle);
                        return n && "matrix" == n.substr(0, 6) ? n.replace(/^.*\((.*)\)$/g, "$1").replace(/px/g, "").split(/, +/) : !1
                    }
                    ,
                    this.getScrollTop = function (n) {
                        if (!n) {
                            if (n = v())
                                return 16 == n.length ? -n[13] : -n[5];
                            if (o.timerscroll && o.timerscroll.bz)
                                return o.timerscroll.bz.getNow()
                        }
                        return o.doc.translate.y
                    }
                    ,
                    this.getScrollLeft = function (n) {
                        if (!n) {
                            if (n = v())
                                return 16 == n.length ? -n[12] : -n[4];
                            if (o.timerscroll && o.timerscroll.bh)
                                return o.timerscroll.bh.getNow()
                        }
                        return o.doc.translate.x
                    }
                    ,
                    this.notifyScrollEvent = document.createEvent ? function (n) {
                        var t = document.createEvent("UIEvents");
                        t.initUIEvent("scroll", !1, !0, window, 1);
                        n.dispatchEvent(t)
                    }
                        : document.fireEvent ? function (n) {
                            var t = document.createEventObject();
                            n.fireEvent("onscroll");
                            t.cancelBubble = !0
                        }
                            : function () { }
                    ,
                    h.hastranslate3d && o.opt.enabletranslate3d ? (this.setScrollTop = function (n, t) {
                        o.doc.translate.y = n;
                        o.doc.translate.ty = -1 * n + "px";
                        o.doc.css(h.trstyle, "translate3d(" + o.doc.translate.tx + "," + o.doc.translate.ty + ",0px)");
                        t || o.notifyScrollEvent(o.win[0])
                    }
                        ,
                        this.setScrollLeft = function (n, t) {
                            o.doc.translate.x = n;
                            o.doc.translate.tx = -1 * n + "px";
                            o.doc.css(h.trstyle, "translate3d(" + o.doc.translate.tx + "," + o.doc.translate.ty + ",0px)");
                            t || o.notifyScrollEvent(o.win[0])
                        }
                    ) : (this.setScrollTop = function (n, t) {
                        o.doc.translate.y = n;
                        o.doc.translate.ty = -1 * n + "px";
                        o.doc.css(h.trstyle, "translate(" + o.doc.translate.tx + "," + o.doc.translate.ty + ")");
                        t || o.notifyScrollEvent(o.win[0])
                    }
                        ,
                        this.setScrollLeft = function (n, t) {
                            o.doc.translate.x = n;
                            o.doc.translate.tx = -1 * n + "px";
                            o.doc.css(h.trstyle, "translate(" + o.doc.translate.tx + "," + o.doc.translate.ty + ")");
                            t || o.notifyScrollEvent(o.win[0])
                        }
                    )) : (this.getScrollTop = function () {
                        return o.docscroll.scrollTop()
                    }
                        ,
                        this.setScrollTop = function (n) {
                            return o.docscroll.scrollTop(n)
                        }
                        ,
                        this.getScrollLeft = function () {
                            return o.docscroll.scrollLeft()
                        }
                        ,
                        this.setScrollLeft = function (n) {
                            return o.docscroll.scrollLeft(n)
                        }
                );
                this.getTarget = function (n) {
                    return n ? n.target ? n.target : n.srcElement ? n.srcElement : !1 : !1
                }
                    ;
                this.hasParent = function (n, t) {
                    if (!n)
                        return !1;
                    for (var i = n.target || n.srcElement || n || !1; i && i.id != t;)
                        i = i.parentNode || !1;
                    return !1 !== i
                }
                    ;
                tt = {
                    thin: 1,
                    medium: 3,
                    thick: 5
                };
                this.getOffset = function () {
                    if (o.isfixed)
                        return {
                            top: parseFloat(o.win.css("top")),
                            left: parseFloat(o.win.css("left"))
                        };
                    if (!o.viewport)
                        return o.win.offset();
                    var n = o.win.offset()
                        , t = o.viewport.offset();
                    return {
                        top: n.top - t.top + o.viewport.scrollTop(),
                        left: n.left - t.left + o.viewport.scrollLeft()
                    }
                }
                    ;
                this.updateScrollBar = function (n) {
                    var t, r;
                    if (o.ishwscroll)
                        o.rail.css({
                            height: o.win.innerHeight()
                        }),
                            o.railh && o.railh.css({
                                width: o.win.innerWidth()
                            });
                    else {
                        var u = o.getOffset()
                            , i = u.top
                            , t = u.left
                            , i = i + l(o.win, "border-top-width", !0);
                        o.win.outerWidth();
                        o.win.innerWidth();
                        t = t + (o.rail.align ? o.win.outerWidth() - l(o.win, "border-right-width") - o.rail.width : l(o.win, "border-left-width"));
                        r = o.opt.railoffset;
                        r && (r.top && (i += r.top),
                            o.rail.align && r.left && (t += r.left));
                        o.locked || o.rail.css({
                            top: i,
                            left: t,
                            height: n ? n.h : o.win.innerHeight()
                        });
                        o.zoom && o.zoom.css({
                            top: i + 1,
                            left: 1 == o.rail.align ? t - 20 : t + o.rail.width + 4
                        });
                        o.railh && !o.locked && (i = u.top,
                            t = u.left,
                            n = o.railh.align ? i + l(o.win, "border-top-width", !0) + o.win.innerHeight() - o.railh.height : i + l(o.win, "border-top-width", !0),
                            t += l(o.win, "border-left-width"),
                            o.railh.css({
                                top: n,
                                left: t,
                                width: o.railh.width
                            }))
                    }
                }
                    ;
                this.doRailClick = function (n, t, i) {
                    var r;
                    o.locked || (o.cancelEvent(n),
                        t ? (t = i ? o.doScrollLeft : o.doScrollTop,
                            r = i ? (n.pageX - o.railh.offset().left - o.cursorwidth / 2) * o.scrollratio.x : (n.pageY - o.rail.offset().top - o.cursorheight / 2) * o.scrollratio.y,
                            t(r)) : (t = i ? o.doScrollLeftBy : o.doScrollBy,
                                r = i ? o.scroll.x : o.scroll.y,
                                n = i ? n.pageX - o.railh.offset().left : n.pageY - o.rail.offset().top,
                                i = i ? o.view.w : o.view.h,
                                r >= n ? t(i) : t(-i)))
                }
                    ;
                o.hasanimationframe = t;
                o.hascancelanimationframe = i;
                o.hasanimationframe ? o.hascancelanimationframe || (i = function () {
                    o.cancelAnimationFrame = !0
                }
                ) : (t = function (n) {
                    return setTimeout(n, 15 - Math.floor(+new Date / 1e3) % 16)
                }
                    ,
                    i = clearInterval);
                this.init = function () {
                    var f, u, p, l, a, k, i, g, v, nt, t, y;
                    if (o.saved.css = [],
                        h.isie7mobile || h.isoperamini)
                        return !0;
                    if (h.hasmstouch && o.css(o.ispage ? n("html") : o.win, {
                        "-ms-touch-action": "none"
                    }),
                        o.zindex = "auto",
                        o.zindex = !o.ispage && "auto" == o.opt.zindex ? it() || "auto" : o.opt.zindex,
                        !o.ispage && "auto" != o.zindex && o.zindex > r && (r = o.zindex),
                        o.isie && 0 == o.zindex && "auto" == o.opt.zindex && (o.zindex = "auto"),
                        !o.ispage || !h.cantouch && !h.isieold && !h.isie9mobile) {
                        f = o.docscroll;
                        o.ispage && (f = o.haswrapper ? o.win : o.doc);
                        h.isie9mobile || o.css(f, {
                            "overflow-y": "hidden"
                        });
                        o.ispage && h.isie7 && ("BODY" == o.doc[0].nodeName ? o.css(n("html"), {
                            "overflow-y": "hidden"
                        }) : "HTML" == o.doc[0].nodeName && o.css(n("body"), {
                            "overflow-y": "hidden"
                        }));
                        !h.isios || o.ispage || o.haswrapper || o.css(n("body"), {
                            "-webkit-overflow-scrolling": "touch"
                        });
                        u = n(document.createElement("div"));
                        u.css({
                            position: "relative",
                            top: 0,
                            float: "right",
                            width: o.opt.cursorwidth,
                            height: "0px",
                            "background-color": o.opt.cursorcolor,
                            border: o.opt.cursorborder,
                            "background-clip": "padding-box",
                            "-webkit-border-radius": o.opt.cursorborderradius,
                            "-moz-border-radius": o.opt.cursorborderradius,
                            "border-radius": o.opt.cursorborderradius
                        });
                        u.hborder = parseFloat(u.outerHeight() - u.innerHeight());
                        o.cursor = u;
                        t = n(document.createElement("div"));
                        t.attr("id", o.id);
                        t.addClass("nicescroll-rails");
                        a = ["left", "right"];
                        for (k in a)
                            l = a[k],
                                (p = o.opt.railpadding[l]) ? t.css("padding-" + l, p + "px") : o.opt.railpadding[l] = 0;
                        t.append(u);
                        t.width = Math.max(parseFloat(o.opt.cursorwidth), u.outerWidth()) + o.opt.railpadding.left + o.opt.railpadding.right;
                        t.css({
                            width: t.width + "px",
                            zIndex: o.zindex,
                            background: o.opt.background,
                            cursor: "default"
                        });
                        t.visibility = !0;
                        t.scrollable = !0;
                        t.align = "left" == o.opt.railalign ? 0 : 1;
                        o.rail = t;
                        u = o.rail.drag = !1;
                        !o.opt.boxzoom || o.ispage || h.isieold || (u = document.createElement("div"),
                            o.bind(u, "click", o.doZoom),
                            o.zoom = n(u),
                            o.zoom.css({
                                cursor: "pointer",
                                "z-index": o.zindex,
                                backgroundImage: "url(" + d + "zoomico.png)",
                                height: 18,
                                width: 18,
                                backgroundPosition: "0px 0px"
                            }),
                            o.opt.dblclickzoom && o.bind(o.win, "dblclick", o.doZoom),
                            h.cantouch && o.opt.gesturezoom && (o.ongesturezoom = function (n) {
                                return 1.5 < n.scale && o.doZoomIn(n),
                                    .8 > n.scale && o.doZoomOut(n),
                                    o.cancelEvent(n)
                            }
                                ,
                                o.bind(o.win, "gestureend", o.ongesturezoom)));
                        o.railh = !1;
                        o.opt.horizrailenabled && (o.css(f, {
                            "overflow-x": "hidden"
                        }),
                            u = n(document.createElement("div")),
                            u.css({
                                position: "relative",
                                top: 0,
                                height: o.opt.cursorwidth,
                                width: "0px",
                                "background-color": o.opt.cursorcolor,
                                border: o.opt.cursorborder,
                                "background-clip": "padding-box",
                                "-webkit-border-radius": o.opt.cursorborderradius,
                                "-moz-border-radius": o.opt.cursorborderradius,
                                "border-radius": o.opt.cursorborderradius
                            }),
                            u.wborder = parseFloat(u.outerWidth() - u.innerWidth()),
                            o.cursorh = u,
                            i = n(document.createElement("div")),
                            i.attr("id", o.id + "-hr"),
                            i.addClass("nicescroll-rails"),
                            i.height = Math.max(parseFloat(o.opt.cursorwidth), u.outerHeight()),
                            i.css({
                                height: i.height + "px",
                                zIndex: o.zindex,
                                background: o.opt.background
                            }),
                            i.append(u),
                            i.visibility = !0,
                            i.scrollable = !0,
                            i.align = "top" == o.opt.railvalign ? 0 : 1,
                            o.railh = i,
                            o.railh.drag = !1);
                        o.ispage ? (t.css({
                            position: "fixed",
                            top: "0px",
                            height: "100%"
                        }),
                            t.align ? t.css({
                                right: "0px"
                            }) : t.css({
                                left: "0px"
                            }),
                            o.body.append(t),
                            o.railh && (i.css({
                                position: "fixed",
                                left: "0px",
                                width: "100%"
                            }),
                                i.align ? i.css({
                                    bottom: "0px"
                                }) : i.css({
                                    top: "0px"
                                }),
                                o.body.append(i))) : (o.ishwscroll ? ("static" == o.win.css("position") && o.css(o.win, {
                                    position: "relative"
                                }),
                                    f = "HTML" == o.win[0].nodeName ? o.body : o.win,
                                    o.zoom && (o.zoom.css({
                                        position: "absolute",
                                        top: 1,
                                        right: 0,
                                        "margin-right": t.width + 4
                                    }),
                                        f.append(o.zoom)),
                                    t.css({
                                        position: "absolute",
                                        top: 0
                                    }),
                                    t.align ? t.css({
                                        right: 0
                                    }) : t.css({
                                        left: 0
                                    }),
                                    f.append(t),
                                    i && (i.css({
                                        position: "absolute",
                                        left: 0,
                                        bottom: 0
                                    }),
                                        i.align ? i.css({
                                            bottom: 0
                                        }) : i.css({
                                            top: 0
                                        }),
                                        f.append(i))) : (o.isfixed = "fixed" == o.win.css("position"),
                                            f = o.isfixed ? "fixed" : "absolute",
                                            o.isfixed || (o.viewport = o.getViewport(o.win[0])),
                                            o.viewport && (o.body = o.viewport,
                                                !1 == /fixed|relative|absolute/.test(o.viewport.css("position")) && o.css(o.viewport, {
                                                    position: "relative"
                                                })),
                                            t.css({
                                                position: f
                                            }),
                                            o.zoom && o.zoom.css({
                                                position: f
                                            }),
                                            o.updateScrollBar(),
                                            o.body.append(t),
                                            o.zoom && o.body.append(o.zoom),
                                            o.railh && (i.css({
                                                position: f
                                            }),
                                                o.body.append(i))),
                                    h.isios && o.css(o.win, {
                                        "-webkit-tap-highlight-color": "rgba(0,0,0,0)",
                                        "-webkit-touch-callout": "none"
                                    }),
                                    h.isie && o.opt.disableoutline && o.win.attr("hideFocus", "true"),
                                    h.iswebkit && o.opt.disableoutline && o.win.css({
                                        outline: "none"
                                    }));
                        !1 === o.opt.autohidemode ? (o.autohidedom = !1,
                            o.rail.css({
                                opacity: o.opt.cursoropacitymax
                            }),
                            o.railh && o.railh.css({
                                opacity: o.opt.cursoropacitymax
                            })) : !0 === o.opt.autohidemode || "leave" === o.opt.autohidemode ? (o.autohidedom = n().add(o.rail),
                                h.isie8 && (o.autohidedom = o.autohidedom.add(o.cursor)),
                                o.railh && (o.autohidedom = o.autohidedom.add(o.railh)),
                                o.railh && h.isie8 && (o.autohidedom = o.autohidedom.add(o.cursorh))) : "scroll" == o.opt.autohidemode ? (o.autohidedom = n().add(o.rail),
                                    o.railh && (o.autohidedom = o.autohidedom.add(o.railh))) : "cursor" == o.opt.autohidemode ? (o.autohidedom = n().add(o.cursor),
                                        o.railh && (o.autohidedom = o.autohidedom.add(o.cursorh))) : "hidden" == o.opt.autohidemode && (o.autohidedom = !1,
                                            o.hide(),
                                            o.locked = !1);
                        h.isie9mobile ? (o.scrollmom = new w(o),
                            o.onmangotouch = function (n) {
                                var t, i, r;
                                if (n = o.getScrollTop(),
                                    t = o.getScrollLeft(),
                                    n == o.scrollmom.lastscrolly && t == o.scrollmom.lastscrollx)
                                    return !0;
                                if (i = n - o.mangotouch.sy,
                                    r = t - o.mangotouch.sx,
                                    0 != Math.round(Math.sqrt(Math.pow(r, 2) + Math.pow(i, 2)))) {
                                    var f = 0 > i ? -1 : 1
                                        , e = 0 > r ? -1 : 1
                                        , u = +new Date;
                                    o.mangotouch.lazy && clearTimeout(o.mangotouch.lazy);
                                    80 < u - o.mangotouch.tm || o.mangotouch.dry != f || o.mangotouch.drx != e ? (o.scrollmom.stop(),
                                        o.scrollmom.reset(t, n),
                                        o.mangotouch.sy = n,
                                        o.mangotouch.ly = n,
                                        o.mangotouch.sx = t,
                                        o.mangotouch.lx = t,
                                        o.mangotouch.dry = f,
                                        o.mangotouch.drx = e,
                                        o.mangotouch.tm = u) : (o.scrollmom.stop(),
                                            o.scrollmom.update(o.mangotouch.sx - r, o.mangotouch.sy - i),
                                            o.mangotouch.tm = u,
                                            i = Math.max(Math.abs(o.mangotouch.ly - n), Math.abs(o.mangotouch.lx - t)),
                                            o.mangotouch.ly = n,
                                            o.mangotouch.lx = t,
                                            2 < i && (o.mangotouch.lazy = setTimeout(function () {
                                                o.mangotouch.lazy = !1;
                                                o.mangotouch.dry = 0;
                                                o.mangotouch.drx = 0;
                                                o.mangotouch.tm = 0;
                                                o.scrollmom.doMomentum(30)
                                            }, 100)))
                                }
                            }
                            ,
                            t = o.getScrollTop(),
                            i = o.getScrollLeft(),
                            o.mangotouch = {
                                sy: t,
                                ly: t,
                                dry: 0,
                                sx: i,
                                lx: i,
                                drx: 0,
                                lazy: !1,
                                tm: 0
                            },
                            o.bind(o.docscroll, "scroll", o.onmangotouch)) : ((h.cantouch || o.istouchcapable || o.opt.touchbehavior || h.hasmstouch) && (o.scrollmom = new w(o),
                                o.ontouchstart = function (t) {
                                    var i, r;
                                    if (t.pointerType && 2 != t.pointerType)
                                        return !1;
                                    if (!o.locked) {
                                        if (h.hasmstouch)
                                            for (i = t.target ? t.target : !1; i;) {
                                                if (r = n(i).getNiceScroll(),
                                                    0 < r.length && r[0].me == o.me)
                                                    break;
                                                if (0 < r.length)
                                                    return !1;
                                                if ("DIV" == i.nodeName && i.id == o.id)
                                                    break;
                                                i = i.parentNode ? i.parentNode : !1
                                            }
                                        if (o.cancelScroll(),
                                            (i = o.getTarget(t)) && /INPUT/i.test(i.nodeName) && /range/i.test(i.type))
                                            return o.stopPropagation(t);
                                        if (!("clientX" in t) && "changedTouches" in t && (t.clientX = t.changedTouches[0].clientX,
                                            t.clientY = t.changedTouches[0].clientY),
                                            o.forcescreen && (r = t,
                                                t = {
                                                    original: t.original ? t.original : t
                                                },
                                                t.clientX = r.screenX,
                                                t.clientY = r.screenY),
                                            o.rail.drag = {
                                                x: t.clientX,
                                                y: t.clientY,
                                                sx: o.scroll.x,
                                                sy: o.scroll.y,
                                                st: o.getScrollTop(),
                                                sl: o.getScrollLeft(),
                                                pt: 2,
                                                dl: !1
                                            },
                                            o.ispage || !o.opt.directionlockdeadzone)
                                            o.rail.drag.dl = "f";
                                        else {
                                            var r = n(window).width()
                                                , u = n(window).height()
                                                , f = Math.max(document.body.scrollWidth, document.documentElement.scrollWidth)
                                                , e = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                                                , u = Math.max(0, e - u)
                                                , r = Math.max(0, f - r);
                                            o.rail.drag.ck = !o.rail.scrollable && o.railh.scrollable ? 0 < u ? "v" : !1 : o.rail.scrollable && !o.railh.scrollable ? 0 < r ? "h" : !1 : !1;
                                            o.rail.drag.ck || (o.rail.drag.dl = "f")
                                        }
                                        if (o.opt.touchbehavior && o.isiframe && h.isie && (r = o.win.position(),
                                            o.rail.drag.x += r.left,
                                            o.rail.drag.y += r.top),
                                            o.hasmoving = !1,
                                            o.lastmouseup = !1,
                                            o.scrollmom.reset(t.clientX, t.clientY),
                                            !h.cantouch && !this.istouchcapable && !h.hasmstouch) {
                                            if (!i || !/INPUT|SELECT|TEXTAREA/i.test(i.nodeName))
                                                return !o.ispage && h.hasmousecapture && i.setCapture(),
                                                    o.opt.touchbehavior ? o.cancelEvent(t) : o.stopPropagation(t);
                                            /SUBMIT|CANCEL|BUTTON/i.test(n(i).attr("type")) && (pc = {
                                                tg: i,
                                                click: !1
                                            },
                                                o.preventclick = pc)
                                        }
                                    }
                                }
                                ,
                                o.ontouchend = function (n) {
                                    return n.pointerType && 2 != n.pointerType ? !1 : o.rail.drag && 2 == o.rail.drag.pt && (o.scrollmom.doMomentum(),
                                        o.rail.drag = !1,
                                        o.hasmoving && (o.hasmoving = !1,
                                            o.lastmouseup = !0,
                                            o.hideCursor(),
                                            h.hasmousecapture && document.releaseCapture(),
                                            !h.cantouch)) ? o.cancelEvent(n) : void 0
                                }
                                ,
                                g = o.opt.touchbehavior && o.isiframe && !h.hasmousecapture,
                                o.ontouchmove = function (t, i) {
                                    var e, f, u;
                                    if (t.pointerType && 2 != t.pointerType)
                                        return !1;
                                    if (o.rail.drag && 2 == o.rail.drag.pt) {
                                        if (h.cantouch && "undefined" == typeof t.original)
                                            return !0;
                                        o.hasmoving = !0;
                                        o.preventclick && !o.preventclick.click && (o.preventclick.click = o.preventclick.tg.onclick || !1,
                                            o.preventclick.tg.onclick = o.onpreventclick);
                                        t = n.extend({
                                            original: t
                                        }, t);
                                        "changedTouches" in t && (t.clientX = t.changedTouches[0].clientX,
                                            t.clientY = t.changedTouches[0].clientY);
                                        o.forcescreen && (f = t,
                                            t = {
                                                original: t.original ? t.original : t
                                            },
                                            t.clientX = f.screenX,
                                            t.clientY = f.screenY);
                                        f = ofy = 0;
                                        g && !i && (e = o.win.position(),
                                            f = -e.left,
                                            ofy = -e.top);
                                        var c = t.clientY + ofy
                                            , e = c - o.rail.drag.y
                                            , l = t.clientX + f
                                            , s = l - o.rail.drag.x
                                            , r = o.rail.drag.st - e;
                                        if (o.ishwscroll && o.opt.bouncescroll ? 0 > r ? r = Math.round(r / 2) : r > o.page.maxh && (r = o.page.maxh + Math.round((r - o.page.maxh) / 2)) : (0 > r && (c = r = 0),
                                            r > o.page.maxh && (r = o.page.maxh,
                                                c = 0)),
                                            o.railh && o.railh.scrollable && (u = o.rail.drag.sl - s,
                                                o.ishwscroll && o.opt.bouncescroll ? 0 > u ? u = Math.round(u / 2) : u > o.page.maxw && (u = o.page.maxw + Math.round((u - o.page.maxw) / 2)) : (0 > u && (l = u = 0),
                                                    u > o.page.maxw && (u = o.page.maxw,
                                                        l = 0))),
                                            f = !1,
                                            o.rail.drag.dl)
                                            f = !0,
                                                "v" == o.rail.drag.dl ? u = o.rail.drag.sl : "h" == o.rail.drag.dl && (r = o.rail.drag.st);
                                        else {
                                            var e = Math.abs(e)
                                                , s = Math.abs(s)
                                                , a = o.opt.directionlockdeadzone;
                                            if ("v" == o.rail.drag.ck) {
                                                if (e > a && s <= .3 * e)
                                                    return o.rail.drag = !1,
                                                        !0;
                                                s > a && (o.rail.drag.dl = "f",
                                                    n("body").scrollTop(n("body").scrollTop()))
                                            } else if ("h" == o.rail.drag.ck) {
                                                if (s > a && e <= .3 * s)
                                                    return o.rail.drag = !1,
                                                        !0;
                                                e > a && (o.rail.drag.dl = "f",
                                                    n("body").scrollLeft(n("body").scrollLeft()))
                                            }
                                        }
                                        if (o.synched("touchmove", function () {
                                            o.rail.drag && 2 == o.rail.drag.pt && (o.prepareTransition && o.prepareTransition(0),
                                                o.rail.scrollable && o.setScrollTop(r),
                                                o.scrollmom.update(l, c),
                                                o.railh && o.railh.scrollable ? (o.setScrollLeft(u),
                                                    o.showCursor(r, u)) : o.showCursor(r),
                                                h.isie10 && document.selection.clear())
                                        }),
                                            h.ischrome && o.istouchcapable && (f = !1),
                                            f)
                                            return o.cancelEvent(t)
                                    }
                                }
                            ),
                                o.onmousedown = function (n, t) {
                                    if (!(o.rail.drag && 1 != o.rail.drag.pt)) {
                                        if (o.locked)
                                            return o.cancelEvent(n);
                                        o.cancelScroll();
                                        o.rail.drag = {
                                            x: n.clientX,
                                            y: n.clientY,
                                            sx: o.scroll.x,
                                            sy: o.scroll.y,
                                            pt: 1,
                                            hr: !!t
                                        };
                                        var i = o.getTarget(n);
                                        return !o.ispage && h.hasmousecapture && i.setCapture(),
                                            o.isiframe && !h.hasmousecapture && (o.saved.csspointerevents = o.doc.css("pointer-events"),
                                                o.css(o.doc, {
                                                    "pointer-events": "none"
                                                })),
                                            o.cancelEvent(n)
                                    }
                                }
                                ,
                                o.onmouseup = function (n) {
                                    if (o.rail.drag && (h.hasmousecapture && document.releaseCapture(),
                                        o.isiframe && !h.hasmousecapture && o.doc.css("pointer-events", o.saved.csspointerevents),
                                        1 == o.rail.drag.pt))
                                        return o.rail.drag = !1,
                                            o.cancelEvent(n)
                                }
                                ,
                                o.onmousemove = function (n) {
                                    if (o.rail.drag && 1 == o.rail.drag.pt) {
                                        if (h.ischrome && 0 == n.which)
                                            return o.onmouseup(n);
                                        if (o.cursorfreezed = !0,
                                            o.rail.drag.hr) {
                                            o.scroll.x = o.rail.drag.sx + (n.clientX - o.rail.drag.x);
                                            0 > o.scroll.x && (o.scroll.x = 0);
                                            var t = o.scrollvaluemaxw;
                                            o.scroll.x > t && (o.scroll.x = t)
                                        } else
                                            o.scroll.y = o.rail.drag.sy + (n.clientY - o.rail.drag.y),
                                                0 > o.scroll.y && (o.scroll.y = 0),
                                                t = o.scrollvaluemax,
                                                o.scroll.y > t && (o.scroll.y = t);
                                        return o.synched("mousemove", function () {
                                            o.rail.drag && 1 == o.rail.drag.pt && (o.showCursor(),
                                                o.rail.drag.hr ? o.doScrollLeft(Math.round(o.scroll.x * o.scrollratio.x), o.opt.cursordragspeed) : o.doScrollTop(Math.round(o.scroll.y * o.scrollratio.y), o.opt.cursordragspeed))
                                        }),
                                            o.cancelEvent(n)
                                    }
                                }
                                ,
                                h.cantouch || o.opt.touchbehavior ? (o.onpreventclick = function (n) {
                                    if (o.preventclick)
                                        return o.preventclick.tg.onclick = o.preventclick.click,
                                            o.preventclick = !1,
                                            o.cancelEvent(n)
                                }
                                    ,
                                    o.bind(o.win, "mousedown", o.ontouchstart),
                                    o.onclick = h.isios ? !1 : function (n) {
                                        return o.lastmouseup ? (o.lastmouseup = !1,
                                            o.cancelEvent(n)) : !0
                                    }
                                    ,
                                    o.opt.grabcursorenabled && h.cursorgrabvalue && (o.css(o.ispage ? o.doc : o.win, {
                                        cursor: h.cursorgrabvalue
                                    }),
                                        o.css(o.rail, {
                                            cursor: h.cursorgrabvalue
                                        }))) : (v = function (n) {
                                            if (o.selectiondrag) {
                                                if (n) {
                                                    var t = o.win.outerHeight();
                                                    n = n.pageY - o.selectiondrag.top;
                                                    0 < n && n < t && (n = 0);
                                                    n >= t && (n -= t);
                                                    o.selectiondrag.df = n
                                                }
                                                0 != o.selectiondrag.df && (o.doScrollBy(2 * -Math.floor(o.selectiondrag.df / 6)),
                                                    o.debounced("doselectionscroll", function () {
                                                        v()
                                                    }, 50))
                                            }
                                        }
                                            ,
                                            o.hasTextSelected = "getSelection" in document ? function () {
                                                return 0 < document.getSelection().rangeCount
                                            }
                                                : "selection" in document ? function () {
                                                    return "None" != document.selection.type
                                                }
                                                    : function () {
                                                        return !1
                                                    }
                                            ,
                                            o.onselectionstart = function () {
                                                o.ispage || (o.selectiondrag = o.win.offset())
                                            }
                                            ,
                                            o.onselectionend = function () {
                                                o.selectiondrag = !1
                                            }
                                            ,
                                            o.onselectiondrag = function (n) {
                                                o.selectiondrag && o.hasTextSelected() && o.debounced("selectionscroll", function () {
                                                    v(n)
                                                }, 250)
                                            }
                                ),
                                h.hasmstouch && (o.css(o.rail, {
                                    "-ms-touch-action": "none"
                                }),
                                    o.css(o.cursor, {
                                        "-ms-touch-action": "none"
                                    }),
                                    o.bind(o.win, "MSPointerDown", o.ontouchstart),
                                    o.bind(document, "MSPointerUp", o.ontouchend),
                                    o.bind(document, "MSPointerMove", o.ontouchmove),
                                    o.bind(o.cursor, "MSGestureHold", function (n) {
                                        n.preventDefault()
                                    }),
                                    o.bind(o.cursor, "contextmenu", function (n) {
                                        n.preventDefault()
                                    })),
                                this.istouchcapable && (o.bind(o.win, "touchstart", o.ontouchstart),
                                    o.bind(document, "touchend", o.ontouchend),
                                    o.bind(document, "touchcancel", o.ontouchend),
                                    o.bind(document, "touchmove", o.ontouchmove)),
                                o.bind(o.cursor, "mousedown", o.onmousedown),
                                o.bind(o.cursor, "mouseup", o.onmouseup),
                                o.railh && (o.bind(o.cursorh, "mousedown", function (n) {
                                    o.onmousedown(n, !0)
                                }),
                                    o.bind(o.cursorh, "mouseup", function (n) {
                                        if (!o.rail.drag || 2 != o.rail.drag.pt)
                                            return o.rail.drag = !1,
                                                o.hasmoving = !1,
                                                o.hideCursor(),
                                                h.hasmousecapture && document.releaseCapture(),
                                                o.cancelEvent(n)
                                    })),
                                !o.opt.cursordragontouch && (h.cantouch || o.opt.touchbehavior) || (o.rail.css({
                                    cursor: "default"
                                }),
                                    o.railh && o.railh.css({
                                        cursor: "default"
                                    }),
                                    o.jqbind(o.rail, "mouseenter", function () {
                                        o.canshowonmouseevent && o.showCursor();
                                        o.rail.active = !0
                                    }),
                                    o.jqbind(o.rail, "mouseleave", function () {
                                        o.rail.active = !1;
                                        o.rail.drag || o.hideCursor()
                                    }),
                                    o.opt.sensitiverail && (o.bind(o.rail, "click", function (n) {
                                        o.doRailClick(n, !1, !1)
                                    }),
                                        o.bind(o.rail, "dblclick", function (n) {
                                            o.doRailClick(n, !0, !1)
                                        }),
                                        o.bind(o.cursor, "click", function (n) {
                                            o.cancelEvent(n)
                                        }),
                                        o.bind(o.cursor, "dblclick", function (n) {
                                            o.cancelEvent(n)
                                        })),
                                    o.railh && (o.jqbind(o.railh, "mouseenter", function () {
                                        o.canshowonmouseevent && o.showCursor();
                                        o.rail.active = !0
                                    }),
                                        o.jqbind(o.railh, "mouseleave", function () {
                                            o.rail.active = !1;
                                            o.rail.drag || o.hideCursor()
                                        }),
                                        o.opt.sensitiverail && (o.bind(o.railh, "click", function (n) {
                                            o.doRailClick(n, !1, !0)
                                        }),
                                            o.bind(o.railh, "dblclick", function (n) {
                                                o.doRailClick(n, !0, !0)
                                            }),
                                            o.bind(o.cursorh, "click", function (n) {
                                                o.cancelEvent(n)
                                            }),
                                            o.bind(o.cursorh, "dblclick", function (n) {
                                                o.cancelEvent(n)
                                            })))),
                                !h.cantouch && !o.opt.touchbehavior ? (o.bind(h.hasmousecapture ? o.win : document, "mouseup", o.onmouseup),
                                    o.bind(document, "mousemove", o.onmousemove),
                                    o.onclick && o.bind(document, "click", o.onclick),
                                    !o.ispage && o.opt.enablescrollonselection && (o.bind(o.win[0], "mousedown", o.onselectionstart),
                                        o.bind(document, "mouseup", o.onselectionend),
                                        o.bind(o.cursor, "mouseup", o.onselectionend),
                                        o.cursorh && o.bind(o.cursorh, "mouseup", o.onselectionend),
                                        o.bind(document, "mousemove", o.onselectiondrag)),
                                    o.zoom && (o.jqbind(o.zoom, "mouseenter", function () {
                                        o.canshowonmouseevent && o.showCursor();
                                        o.rail.active = !0
                                    }),
                                        o.jqbind(o.zoom, "mouseleave", function () {
                                            o.rail.active = !1;
                                            o.rail.drag || o.hideCursor()
                                        }))) : (o.bind(h.hasmousecapture ? o.win : document, "mouseup", o.ontouchend),
                                            o.bind(document, "mousemove", o.ontouchmove),
                                            o.onclick && o.bind(document, "click", o.onclick),
                                            o.opt.cursordragontouch && (o.bind(o.cursor, "mousedown", o.onmousedown),
                                                o.bind(o.cursor, "mousemove", o.onmousemove),
                                                o.cursorh && o.bind(o.cursorh, "mousedown", function (n) {
                                                    o.onmousedown(n, !0)
                                                }),
                                                o.cursorh && o.bind(o.cursorh, "mousemove", o.onmousemove))),
                                o.opt.enablemousewheel && (o.isiframe || o.bind(h.isie && o.ispage ? document : o.win, "mousewheel", o.onmousewheel),
                                    o.bind(o.rail, "mousewheel", o.onmousewheel),
                                    o.railh && o.bind(o.railh, "mousewheel", o.onmousewheelhr)),
                                o.ispage || h.cantouch || /HTML|BODY/.test(o.win[0].nodeName) || (o.win.attr("tabindex") || o.win.attr({
                                    tabindex: b++
                                }),
                                    o.jqbind(o.win, "focus", function (n) {
                                        e = o.getTarget(n).id || !0;
                                        o.hasfocus = !0;
                                        o.canshowonmouseevent && o.noticeCursor()
                                    }),
                                    o.jqbind(o.win, "blur", function () {
                                        e = !1;
                                        o.hasfocus = !1
                                    }),
                                    o.jqbind(o.win, "mouseenter", function (n) {
                                        c = o.getTarget(n).id || !0;
                                        o.hasmousefocus = !0;
                                        o.canshowonmouseevent && o.noticeCursor()
                                    }),
                                    o.jqbind(o.win, "mouseleave", function () {
                                        c = !1;
                                        o.hasmousefocus = !1;
                                        o.rail.drag || o.hideCursor()
                                    })));
                        o.onkeypress = function (n) {
                            var i;
                            if (o.locked && 0 == o.page.maxh || (n = n ? n : window.e,
                                i = o.getTarget(n),
                                i && /INPUT|TEXTAREA|SELECT|OPTION/.test(i.nodeName) && (!i.getAttribute("type") && !i.type || !/submit|button|cancel/i.tp)))
                                return !0;
                            if (o.hasfocus || o.hasmousefocus && !e || o.ispage && !e && !c) {
                                if (i = n.keyCode,
                                    o.locked && 27 != i)
                                    return o.cancelEvent(n);
                                var r = n.ctrlKey || !1
                                    , u = n.shiftKey || !1
                                    , t = !1;
                                switch (i) {
                                    case 38:
                                    case 63233:
                                        o.doScrollBy(72);
                                        t = !0;
                                        break;
                                    case 40:
                                    case 63235:
                                        o.doScrollBy(-72);
                                        t = !0;
                                        break;
                                    case 37:
                                    case 63232:
                                        o.railh && (r ? o.doScrollLeft(0) : o.doScrollLeftBy(72),
                                            t = !0);
                                        break;
                                    case 39:
                                    case 63234:
                                        o.railh && (r ? o.doScrollLeft(o.page.maxw) : o.doScrollLeftBy(-72),
                                            t = !0);
                                        break;
                                    case 33:
                                    case 63276:
                                        o.doScrollBy(o.view.h);
                                        t = !0;
                                        break;
                                    case 34:
                                    case 63277:
                                        o.doScrollBy(-o.view.h);
                                        t = !0;
                                        break;
                                    case 36:
                                    case 63273:
                                        o.railh && r ? o.doScrollPos(0, 0) : o.doScrollTo(0);
                                        t = !0;
                                        break;
                                    case 35:
                                    case 63275:
                                        o.railh && r ? o.doScrollPos(o.page.maxw, o.page.maxh) : o.doScrollTo(o.page.maxh);
                                        t = !0;
                                        break;
                                    case 32:
                                        o.opt.spacebarenabled && (u ? o.doScrollBy(o.view.h) : o.doScrollBy(-o.view.h),
                                            t = !0);
                                        break;
                                    case 27:
                                        o.zoomactive && (o.doZoom(),
                                            t = !0)
                                }
                                if (t)
                                    return o.cancelEvent(n)
                            }
                        }
                            ;
                        o.opt.enablekeyboard && o.bind(document, h.isopera && !h.isopera12 ? "keypress" : "keydown", o.onkeypress);
                        o.bind(window, "resize", o.lazyResize);
                        o.bind(window, "orientationchange", o.lazyResize);
                        o.bind(window, "load", o.lazyResize);
                        !h.ischrome || o.ispage || o.haswrapper || (nt = o.win.attr("style"),
                            t = parseFloat(o.win.css("width")) + 1,
                            o.win.css("width", t),
                            o.synched("chromefix", function () {
                                o.win.attr("style", nt)
                            }));
                        o.onAttributeChange = function () {
                            o.lazyResize(250)
                        }
                            ;
                        o.ispage || o.haswrapper || (!1 !== s ? (o.observer = new s(function (n) {
                            n.forEach(o.onAttributeChange)
                        }
                        ),
                            o.observer.observe(o.win[0], {
                                childList: !0,
                                characterData: !1,
                                attributes: !0,
                                subtree: !1
                            }),
                            o.observerremover = new s(function (n) {
                                n.forEach(function (n) {
                                    if (0 < n.removedNodes.length)
                                        for (var t in n.removedNodes)
                                            if (n.removedNodes[t] == o.win[0])
                                                return o.remove()
                                })
                            }
                            ),
                            o.observerremover.observe(o.win[0].parentNode, {
                                childList: !0,
                                characterData: !1,
                                attributes: !1,
                                subtree: !1
                            })) : (o.bind(o.win, h.isie && !h.isie9 ? "propertychange" : "DOMAttrModified", o.onAttributeChange),
                                h.isie9 && o.win[0].attachEvent("onpropertychange", o.onAttributeChange),
                                o.bind(o.win, "DOMNodeRemoved", function (n) {
                                    n.target == o.win[0] && o.remove()
                                })));
                        !o.ispage && o.opt.boxzoom && o.bind(window, "resize", o.resizeZoom);
                        o.istextarea && o.bind(o.win, "mouseup", o.lazyResize);
                        o.checkrtlmode = !0;
                        o.lazyResize(30)
                    }
                    "IFRAME" == this.doc[0].nodeName && (y = function (t) {
                        o.iframexd = !1;
                        try {
                            var i = "contentDocument" in this ? this.contentDocument : this.contentWindow.document
                        } catch (r) {
                            o.iframexd = !0;
                            i = !1
                        }
                        if (o.iframexd)
                            return "console" in window && console.log("NiceScroll error: policy restriced iframe"),
                                !0;
                        o.forcescreen = !0;
                        o.isiframe && (o.iframe = {
                            doc: n(i),
                            html: o.doc.contents().find("html")[0],
                            body: o.doc.contents().find("body")[0]
                        },
                            o.getContentSize = function () {
                                return {
                                    w: Math.max(o.iframe.html.scrollWidth, o.iframe.body.scrollWidth),
                                    h: Math.max(o.iframe.html.scrollHeight, o.iframe.body.scrollHeight)
                                }
                            }
                            ,
                            o.docscroll = n(o.iframe.body));
                        h.isios || !o.opt.iframeautoresize || o.isiframe || (o.win.scrollTop(0),
                            o.doc.height(""),
                            t = Math.max(i.getElementsByTagName("html")[0].scrollHeight, i.body.scrollHeight),
                            o.doc.height(t));
                        o.lazyResize(30);
                        h.isie7 && o.css(n(o.iframe.html), {
                            "overflow-y": "hidden"
                        });
                        o.css(n(o.iframe.body), {
                            "overflow-y": "hidden"
                        });
                        h.isios && o.haswrapper && o.css(n(i.body), {
                            "-webkit-transform": "translate3d(0,0,0)"
                        });
                        "contentWindow" in this ? o.bind(this.contentWindow, "scroll", o.onscroll) : o.bind(i, "scroll", o.onscroll);
                        o.opt.enablemousewheel && o.bind(i, "mousewheel", o.onmousewheel);
                        o.opt.enablekeyboard && o.bind(i, h.isopera ? "keypress" : "keydown", o.onkeypress);
                        (h.cantouch || o.opt.touchbehavior) && (o.bind(i, "mousedown", o.ontouchstart),
                            o.bind(i, "mousemove", function (n) {
                                o.ontouchmove(n, !0)
                            }),
                            o.opt.grabcursorenabled && h.cursorgrabvalue && o.css(n(i.body), {
                                cursor: h.cursorgrabvalue
                            }));
                        o.bind(i, "mouseup", o.ontouchend);
                        o.zoom && (o.opt.dblclickzoom && o.bind(i, "dblclick", o.doZoom),
                            o.ongesturezoom && o.bind(i, "gestureend", o.ongesturezoom))
                    }
                        ,
                        this.doc[0].readyState && "complete" == this.doc[0].readyState && setTimeout(function () {
                            y.call(o.doc[0], !1)
                        }, 500),
                        o.bind(this.doc, "load", y))
                }
                    ;
                this.showCursor = function (n, t) {
                    o.cursortimeout && (clearTimeout(o.cursortimeout),
                        o.cursortimeout = 0);
                    o.rail && (o.autohidedom && (o.autohidedom.stop().css({
                        opacity: o.opt.cursoropacitymax
                    }),
                        o.cursoractive = !0),
                        o.rail.drag && 1 == o.rail.drag.pt || ("undefined" != typeof n && !1 !== n && (o.scroll.y = Math.round(1 * n / o.scrollratio.y)),
                            "undefined" != typeof t && (o.scroll.x = Math.round(1 * t / o.scrollratio.x))),
                        o.cursor.css({
                            height: o.cursorheight,
                            top: o.scroll.y
                        }),
                        o.cursorh && (!o.rail.align && o.rail.visibility ? o.cursorh.css({
                            width: o.cursorwidth,
                            left: o.scroll.x + o.rail.width
                        }) : o.cursorh.css({
                            width: o.cursorwidth,
                            left: o.scroll.x
                        }),
                            o.cursoractive = !0),
                        o.zoom && o.zoom.stop().css({
                            opacity: o.opt.cursoropacitymax
                        }))
                }
                    ;
                this.hideCursor = function (n) {
                    o.cursortimeout || !o.rail || !o.autohidedom || o.hasmousefocus && "leave" == o.opt.autohidemode || (o.cursortimeout = setTimeout(function () {
                        o.rail.active && o.showonmouseevent || (o.autohidedom.stop().animate({
                            opacity: o.opt.cursoropacitymin
                        }),
                            o.zoom && o.zoom.stop().animate({
                                opacity: o.opt.cursoropacitymin
                            }),
                            o.cursoractive = !1);
                        o.cursortimeout = 0
                    }, n || o.opt.hidecursordelay))
                }
                    ;
                this.noticeCursor = function (n, t, i) {
                    o.showCursor(t, i);
                    o.rail.active || o.hideCursor(n)
                }
                    ;
                this.getContentSize = o.ispage ? function () {
                    return {
                        w: Math.max(document.body.scrollWidth, document.documentElement.scrollWidth),
                        h: Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                    }
                }
                    : o.haswrapper ? function () {
                        return {
                            w: o.doc.outerWidth() + parseInt(o.win.css("paddingLeft")) + parseInt(o.win.css("paddingRight")),
                            h: o.doc.outerHeight() + parseInt(o.win.css("paddingTop")) + parseInt(o.win.css("paddingBottom"))
                        }
                    }
                        : function () {
                            return {
                                w: o.docscroll[0].scrollWidth,
                                h: o.docscroll[0].scrollHeight
                            }
                        }
                    ;
                this.onResize = function (n, t) {
                    if (!o.win)
                        return !1;
                    if (!o.haswrapper && !o.ispage) {
                        if ("none" == o.win.css("display"))
                            return o.visibility && o.hideRail().hideRailHr(),
                                !1;
                        o.hidden || o.visibility || o.showRail().showRailHr()
                    }
                    var i = o.page.maxh
                        , r = o.page.maxw
                        , u = o.view.w;
                    if (o.view = {
                        w: o.ispage ? o.win.width() : parseInt(o.win[0].clientWidth),
                        h: o.ispage ? o.win.height() : parseInt(o.win[0].clientHeight)
                    },
                        o.page = t ? t : o.getContentSize(),
                        o.page.maxh = Math.max(0, o.page.h - o.view.h),
                        o.page.maxw = Math.max(0, o.page.w - o.view.w),
                        o.page.maxh == i && o.page.maxw == r && o.view.w == u) {
                        if (o.ispage || (i = o.win.offset(),
                            o.lastposition && (r = o.lastposition,
                                r.top == i.top && r.left == i.left)))
                            return o;
                        o.lastposition = i
                    }
                    return (0 == o.page.maxh ? (o.hideRail(),
                        o.scrollvaluemax = 0,
                        o.scroll.y = 0,
                        o.scrollratio.y = 0,
                        o.cursorheight = 0,
                        o.setScrollTop(0),
                        o.rail.scrollable = !1) : o.rail.scrollable = !0,
                        0 == o.page.maxw ? (o.hideRailHr(),
                            o.scrollvaluemaxw = 0,
                            o.scroll.x = 0,
                            o.scrollratio.x = 0,
                            o.cursorwidth = 0,
                            o.setScrollLeft(0),
                            o.railh.scrollable = !1) : o.railh.scrollable = !0,
                        o.locked = 0 == o.page.maxh && 0 == o.page.maxw,
                        o.locked) ? (o.ispage || o.updateScrollBar(o.view),
                            !1) : (!o.hidden && !o.visibility ? o.showRail().showRailHr() : !o.hidden && !o.railh.visibility && o.showRailHr(),
                                o.istextarea && o.win.css("resize") && "none" != o.win.css("resize") && (o.view.h -= 20),
                                o.cursorheight = Math.min(o.view.h, Math.round(o.view.h * (o.view.h / o.page.h))),
                                o.cursorheight = o.opt.cursorfixedheight ? o.opt.cursorfixedheight : Math.max(o.opt.cursorminheight, o.cursorheight),
                                o.cursorwidth = Math.min(o.view.w, Math.round(o.view.w * (o.view.w / o.page.w))),
                                o.cursorwidth = o.opt.cursorfixedheight ? o.opt.cursorfixedheight : Math.max(o.opt.cursorminheight, o.cursorwidth),
                                o.scrollvaluemax = o.view.h - o.cursorheight - o.cursor.hborder,
                                o.railh && (o.railh.width = 0 < o.page.maxh ? o.view.w - o.rail.width : o.view.w,
                                    o.scrollvaluemaxw = o.railh.width - o.cursorwidth - o.cursorh.wborder),
                                o.checkrtlmode && o.railh && (o.checkrtlmode = !1,
                                    o.opt.rtlmode && 0 == o.scroll.x && o.setScrollLeft(o.page.maxw)),
                                o.ispage || o.updateScrollBar(o.view),
                                o.scrollratio = {
                                    x: o.page.maxw / o.scrollvaluemaxw,
                                    y: o.page.maxh / o.scrollvaluemax
                                },
                                o.getScrollTop() > o.page.maxh ? o.doScrollTop(o.page.maxh) : (o.scroll.y = Math.round(o.getScrollTop() * (1 / o.scrollratio.y)),
                                    o.scroll.x = Math.round(o.getScrollLeft() * (1 / o.scrollratio.x)),
                                    o.cursoractive && o.noticeCursor()),
                                o.scroll.y && 0 == o.getScrollTop() && o.doScrollTo(Math.floor(o.scroll.y * o.scrollratio.y)),
                                o)
                }
                    ;
                this.resize = o.onResize;
                this.lazyResize = function (n) {
                    return n = isNaN(n) ? 30 : n,
                        o.delayed("resize", o.resize, n),
                        o
                }
                    ;
                this._bind = function (n, t, i, r) {
                    o.events.push({
                        e: n,
                        n: t,
                        f: i,
                        b: r,
                        q: !1
                    });
                    n.addEventListener ? n.addEventListener(t, i, r || !1) : n.attachEvent ? n.attachEvent("on" + t, i) : n["on" + t] = i
                }
                    ;
                this.jqbind = function (t, i, r) {
                    o.events.push({
                        e: t,
                        n: i,
                        f: r,
                        q: !0
                    });
                    n(t).bind(i, r)
                }
                    ;
                this.bind = function (n, t, i, r) {
                    var u = "jquery" in n ? n[0] : n;
                    "mousewheel" == t ? "onwheel" in o.win ? o._bind(u, "wheel", i, r || !1) : (n = "undefined" != typeof document.onmousewheel ? "mousewheel" : "DOMMouseScroll",
                        y(u, n, i, r || !1),
                        "DOMMouseScroll" == n && y(u, "MozMousePixelScroll", i, r || !1)) : u.addEventListener ? (h.cantouch && /mouseup|mousedown|mousemove/.test(t) && o._bind(u, "mousedown" == t ? "touchstart" : "mouseup" == t ? "touchend" : "touchmove", function (n) {
                            if (n.touches) {
                                if (2 > n.touches.length) {
                                    var t = n.touches.length ? n.touches[0] : n;
                                    t.original = n;
                                    i.call(this, t)
                                }
                            } else
                                n.changedTouches && (t = n.changedTouches[0],
                                    t.original = n,
                                    i.call(this, t))
                        }, r || !1),
                            o._bind(u, t, i, r || !1),
                            h.cantouch && "mouseup" == t && o._bind(u, "touchcancel", i, r || !1)) : o._bind(u, t, function (n) {
                                return (n = n || window.event || !1) && n.srcElement && (n.target = n.srcElement),
                                    "pageY" in n || (n.pageX = n.clientX + document.documentElement.scrollLeft,
                                        n.pageY = n.clientY + document.documentElement.scrollTop),
                                    !1 === i.call(u, n) || !1 === r ? o.cancelEvent(n) : !0
                            })
                }
                    ;
                this._unbind = function (n, t, i, r) {
                    n.removeEventListener ? n.removeEventListener(t, i, r) : n.detachEvent ? n.detachEvent("on" + t, i) : n["on" + t] = !1
                }
                    ;
                this.unbindAll = function () {
                    for (var n, t = 0; t < o.events.length; t++)
                        n = o.events[t],
                            n.q ? n.e.unbind(n.n, n.f) : o._unbind(n.e, n.n, n.f, n.b)
                }
                    ;
                this.cancelEvent = function (n) {
                    return (n = n.original ? n.original : n ? n : window.event || !1,
                        !n) ? !1 : (n.preventDefault && n.preventDefault(),
                            n.stopPropagation && n.stopPropagation(),
                            n.preventManipulation && n.preventManipulation(),
                            n.cancelBubble = !0,
                            n.cancel = !0,
                            n.returnValue = !1)
                }
                    ;
                this.stopPropagation = function (n) {
                    return (n = n.original ? n.original : n ? n : window.event || !1,
                        !n) ? !1 : n.stopPropagation ? n.stopPropagation() : (n.cancelBubble && (n.cancelBubble = !0),
                            !1)
                }
                    ;
                this.showRail = function () {
                    return 0 != o.page.maxh && (o.ispage || "none" != o.win.css("display")) && (o.visibility = !0,
                        o.rail.visibility = !0,
                        o.rail.css("display", "block")),
                        o
                }
                    ;
                this.showRailHr = function () {
                    return o.railh ? (0 != o.page.maxw && (o.ispage || "none" != o.win.css("display")) && (o.railh.visibility = !0,
                        o.railh.css("display", "block")),
                        o) : o
                }
                    ;
                this.hideRail = function () {
                    return o.visibility = !1,
                        o.rail.visibility = !1,
                        o.rail.css("display", "none"),
                        o
                }
                    ;
                this.hideRailHr = function () {
                    return o.railh ? (o.railh.visibility = !1,
                        o.railh.css("display", "none"),
                        o) : o
                }
                    ;
                this.show = function () {
                    return o.hidden = !1,
                        o.locked = !1,
                        o.showRail().showRailHr()
                }
                    ;
                this.hide = function () {
                    return o.hidden = !0,
                        o.locked = !0,
                        o.hideRail().hideRailHr()
                }
                    ;
                this.toggle = function () {
                    return o.hidden ? o.show() : o.hide()
                }
                    ;
                this.remove = function () {
                    var r, i, t, u;
                    for (o.stop(),
                        o.cursortimeout && clearTimeout(o.cursortimeout),
                        o.doZoomOut(),
                        o.unbindAll(),
                        h.isie9 && o.win[0].detachEvent("onpropertychange", o.onAttributeChange),
                        !1 !== o.observer && o.observer.disconnect(),
                        !1 !== o.observerremover && o.observerremover.disconnect(),
                        o.events = null,
                        o.cursor && o.cursor.remove(),
                        o.cursorh && o.cursorh.remove(),
                        o.rail && o.rail.remove(),
                        o.railh && o.railh.remove(),
                        o.zoom && o.zoom.remove(),
                        r = 0; r < o.saved.css.length; r++)
                        i = o.saved.css[r],
                            i[0].css(i[1], "undefined" == typeof i[2] ? "" : i[2]);
                    o.saved = !1;
                    o.me.data("__nicescroll", "");
                    t = n.nicescroll;
                    t.each(function (n) {
                        if (this && this.id === o.id) {
                            delete t[n];
                            for (var i = ++n; i < t.length; i++,
                                n++)
                                t[n] = t[i];
                            t.length--;
                            t.length && delete t[t.length]
                        }
                    });
                    for (u in o)
                        o[u] = null,
                            delete o[u];
                    o = null
                }
                    ;
                this.scrollstart = function (n) {
                    return this.onscrollstart = n,
                        o
                }
                    ;
                this.scrollend = function (n) {
                    return this.onscrollend = n,
                        o
                }
                    ;
                this.scrollcancel = function (n) {
                    return this.onscrollcancel = n,
                        o
                }
                    ;
                this.zoomin = function (n) {
                    return this.onzoomin = n,
                        o
                }
                    ;
                this.zoomout = function (n) {
                    return this.onzoomout = n,
                        o
                }
                    ;
                this.isScrollable = function (t) {
                    if (t = t.target ? t.target : t,
                        "OPTION" == t.nodeName)
                        return !0;
                    for (; t && 1 == t.nodeType && !/BODY|HTML/.test(t.nodeName);) {
                        var i = n(t)
                            , i = i.css("overflowY") || i.css("overflowX") || i.css("overflow") || "";
                        if (/scroll|auto/.test(i))
                            return t.clientHeight != t.scrollHeight;
                        t = t.parentNode ? t.parentNode : !1
                    }
                    return !1
                }
                    ;
                this.getViewport = function (t) {
                    var i, r;
                    for (t = t && t.parentNode ? t.parentNode : !1; t && 1 == t.nodeType && !/BODY|HTML/.test(t.nodeName);) {
                        if ((i = n(t),
                            /fixed|absolute/.test(i.css("position"))) || (r = i.css("overflowY") || i.css("overflowX") || i.css("overflow") || "",
                                /scroll|auto/.test(r) && t.clientHeight != t.scrollHeight || 0 < i.getNiceScroll().length))
                            return i;
                        t = t.parentNode ? t.parentNode : !1
                    }
                    return !1
                }
                    ;
                this.onmousewheel = function (n) {
                    if (o.locked)
                        return o.debounced("checkunlock", o.resize, 250),
                            !0;
                    if (o.rail.drag)
                        return o.cancelEvent(n);
                    if ("auto" == o.opt.oneaxismousemode && 0 != n.deltaX && (o.opt.oneaxismousemode = !1),
                        o.opt.oneaxismousemode && 0 == n.deltaX && !o.rail.scrollable)
                        return o.railh && o.railh.scrollable ? o.onmousewheelhr(n) : !0;
                    var t = +new Date
                        , i = !1;
                    return (o.opt.preservenativescrolling && o.checkarea + 600 < t && (o.nativescrollingarea = o.isScrollable(n),
                        i = !0),
                        o.checkarea = t,
                        o.nativescrollingarea) ? !0 : ((n = nt(n, !1, i)) && (o.checkarea = 0),
                            n)
                }
                    ;
                this.onmousewheelhr = function (n) {
                    if (o.locked || !o.railh.scrollable)
                        return !0;
                    if (o.rail.drag)
                        return o.cancelEvent(n);
                    var t = +new Date
                        , i = !1;
                    return o.opt.preservenativescrolling && o.checkarea + 600 < t && (o.nativescrollingarea = o.isScrollable(n),
                        i = !0),
                        o.checkarea = t,
                        o.nativescrollingarea ? !0 : o.locked ? o.cancelEvent(n) : nt(n, !0, i)
                }
                    ;
                this.stop = function () {
                    return o.cancelScroll(),
                        o.scrollmon && o.scrollmon.stop(),
                        o.cursorfreezed = !1,
                        o.scroll.y = Math.round(o.getScrollTop() * (1 / o.scrollratio.y)),
                        o.noticeCursor(),
                        o
                }
                    ;
                this.getTransitionSpeed = function (n) {
                    var t = Math.round(10 * o.opt.scrollspeed);
                    return n = Math.min(t, Math.round(n / 20 * o.opt.scrollspeed)),
                        20 < n ? n : 0
                }
                    ;
                o.opt.smoothscroll ? o.ishwscroll && h.hastransition && o.opt.usetransition ? (this.prepareTransition = function (n, t) {
                    var i = t ? 20 < n ? n : 0 : o.getTransitionSpeed(n)
                        , r = i ? h.prefixstyle + "transform " + i + "ms ease-out" : "";
                    return o.lasttransitionstyle && o.lasttransitionstyle == r || (o.lasttransitionstyle = r,
                        o.doc.css(h.transitionstyle, r)),
                        i
                }
                    ,
                    this.doScrollLeft = function (n, t) {
                        var i = o.scrollrunning ? o.newscrolly : o.getScrollTop();
                        o.doScrollPos(n, i, t)
                    }
                    ,
                    this.doScrollTop = function (n, t) {
                        var i = o.scrollrunning ? o.newscrollx : o.getScrollLeft();
                        o.doScrollPos(i, n, t)
                    }
                    ,
                    this.doScrollPos = function (n, t, i) {
                        var r = o.getScrollTop()
                            , u = o.getScrollLeft();
                        if (((0 > (o.newscrolly - r) * (t - r) || 0 > (o.newscrollx - u) * (n - u)) && o.cancelScroll(),
                            !1 == o.opt.bouncescroll && (0 > t ? t = 0 : t > o.page.maxh && (t = o.page.maxh),
                                0 > n ? n = 0 : n > o.page.maxw && (n = o.page.maxw)),
                            o.scrollrunning && n == o.newscrollx && t == o.newscrolly) || (o.newscrolly = t,
                                o.newscrollx = n,
                                o.newscrollspeed = i || !1,
                                o.timer))
                            return !1;
                        o.timer = setTimeout(function () {
                            var r = o.getScrollTop(), u = o.getScrollLeft(), i, f;
                            i = n - u;
                            f = t - r;
                            i = Math.round(Math.sqrt(Math.pow(i, 2) + Math.pow(f, 2)));
                            i = o.newscrollspeed && 1 < o.newscrollspeed ? o.newscrollspeed : o.getTransitionSpeed(i);
                            o.newscrollspeed && 1 >= o.newscrollspeed && (i *= o.newscrollspeed);
                            o.prepareTransition(i, !0);
                            o.timerscroll && o.timerscroll.tm && clearInterval(o.timerscroll.tm);
                            0 < i && (!o.scrollrunning && o.onscrollstart && o.onscrollstart.call(o, {
                                type: "scrollstart",
                                current: {
                                    x: u,
                                    y: r
                                },
                                request: {
                                    x: n,
                                    y: t
                                },
                                end: {
                                    x: o.newscrollx,
                                    y: o.newscrolly
                                },
                                speed: i
                            }),
                                h.transitionend ? o.scrollendtrapped || (o.scrollendtrapped = !0,
                                    o.bind(o.doc, h.transitionend, o.onScrollEnd, !1)) : (o.scrollendtrapped && clearTimeout(o.scrollendtrapped),
                                        o.scrollendtrapped = setTimeout(o.onScrollEnd, i)),
                                o.timerscroll = {
                                    bz: new BezierClass(r, o.newscrolly, i, 0, 0, .58, 1),
                                    bh: new BezierClass(u, o.newscrollx, i, 0, 0, .58, 1)
                                },
                                o.cursorfreezed || (o.timerscroll.tm = setInterval(function () {
                                    o.showCursor(o.getScrollTop(), o.getScrollLeft())
                                }, 60)));
                            o.synched("doScroll-set", function () {
                                o.timer = 0;
                                o.scrollendtrapped && (o.scrollrunning = !0);
                                o.setScrollTop(o.newscrolly);
                                o.setScrollLeft(o.newscrollx);
                                o.scrollendtrapped || o.onScrollEnd()
                            })
                        }, 50)
                    }
                    ,
                    this.cancelScroll = function () {
                        if (!o.scrollendtrapped)
                            return !0;
                        var n = o.getScrollTop()
                            , t = o.getScrollLeft();
                        return o.scrollrunning = !1,
                            h.transitionend || clearTimeout(h.transitionend),
                            o.scrollendtrapped = !1,
                            o._unbind(o.doc, h.transitionend, o.onScrollEnd),
                            o.prepareTransition(0),
                            o.setScrollTop(n),
                            o.railh && o.setScrollLeft(t),
                            o.timerscroll && o.timerscroll.tm && clearInterval(o.timerscroll.tm),
                            o.timerscroll = !1,
                            o.cursorfreezed = !1,
                            o.showCursor(n, t),
                            o
                    }
                    ,
                    this.onScrollEnd = function () {
                        o.scrollendtrapped && o._unbind(o.doc, h.transitionend, o.onScrollEnd);
                        o.scrollendtrapped = !1;
                        o.prepareTransition(0);
                        o.timerscroll && o.timerscroll.tm && clearInterval(o.timerscroll.tm);
                        o.timerscroll = !1;
                        var n = o.getScrollTop()
                            , t = o.getScrollLeft();
                        if (o.setScrollTop(n),
                            o.railh && o.setScrollLeft(t),
                            o.noticeCursor(!1, n, t),
                            o.cursorfreezed = !1,
                            0 > n ? n = 0 : n > o.page.maxh && (n = o.page.maxh),
                            0 > t ? t = 0 : t > o.page.maxw && (t = o.page.maxw),
                            n != o.newscrolly || t != o.newscrollx)
                            return o.doScrollPos(t, n, o.opt.snapbackspeed);
                        o.onscrollend && o.scrollrunning && o.onscrollend.call(o, {
                            type: "scrollend",
                            current: {
                                x: t,
                                y: n
                            },
                            end: {
                                x: o.newscrollx,
                                y: o.newscrolly
                            }
                        });
                        o.scrollrunning = !1
                    }
                ) : (this.doScrollLeft = function (n, t) {
                    var i = o.scrollrunning ? o.newscrolly : o.getScrollTop();
                    o.doScrollPos(n, i, t)
                }
                    ,
                    this.doScrollTop = function (n, t) {
                        var i = o.scrollrunning ? o.newscrollx : o.getScrollLeft();
                        o.doScrollPos(i, n, t)
                    }
                    ,
                    this.doScrollPos = function (n, r, u) {
                        function l() {
                            var r, n, u, i;
                            if (o.cancelAnimationFrame)
                                return !0;
                            if (o.scrollrunning = !0,
                                a = 1 - a)
                                return o.timer = t(l) || 1;
                            r = 0;
                            n = sy = o.getScrollTop();
                            o.dst.ay ? (n = o.bzscroll ? o.dst.py + o.bzscroll.getNow() * o.dst.ay : o.newscrolly,
                                u = n - sy,
                                (0 > u && n < o.newscrolly || 0 < u && n > o.newscrolly) && (n = o.newscrolly),
                                o.setScrollTop(n),
                                n == o.newscrolly && (r = 1)) : r = 1;
                            i = sx = o.getScrollLeft();
                            o.dst.ax ? (i = o.bzscroll ? o.dst.px + o.bzscroll.getNow() * o.dst.ax : o.newscrollx,
                                u = i - sx,
                                (0 > u && i < o.newscrollx || 0 < u && i > o.newscrollx) && (i = o.newscrollx),
                                o.setScrollLeft(i),
                                i == o.newscrollx && (r += 1)) : r += 1;
                            2 == r ? (o.timer = 0,
                                o.cursorfreezed = !1,
                                o.bzscroll = !1,
                                o.scrollrunning = !1,
                                0 > n ? n = 0 : n > o.page.maxh && (n = o.page.maxh),
                                0 > i ? i = 0 : i > o.page.maxw && (i = o.page.maxw),
                                i != o.newscrollx || n != o.newscrolly ? o.doScrollPos(i, n) : o.onscrollend && o.onscrollend.call(o, {
                                    type: "scrollend",
                                    current: {
                                        x: sx,
                                        y: sy
                                    },
                                    end: {
                                        x: o.newscrollx,
                                        y: o.newscrolly
                                    }
                                })) : o.timer = t(l) || 1
                        }
                        var e, s, f, c, h, a;
                        if (r = "undefined" == typeof r || !1 === r ? o.getScrollTop(!0) : r,
                            o.timer && o.newscrolly == r && o.newscrollx == n)
                            return !0;
                        o.timer && i(o.timer);
                        o.timer = 0;
                        e = o.getScrollTop();
                        s = o.getScrollLeft();
                        (0 > (o.newscrolly - e) * (r - e) || 0 > (o.newscrollx - s) * (n - s)) && o.cancelScroll();
                        o.newscrolly = r;
                        o.newscrollx = n;
                        o.bouncescroll && o.rail.visibility || (0 > o.newscrolly ? o.newscrolly = 0 : o.newscrolly > o.page.maxh && (o.newscrolly = o.page.maxh));
                        o.bouncescroll && o.railh.visibility || (0 > o.newscrollx ? o.newscrollx = 0 : o.newscrollx > o.page.maxw && (o.newscrollx = o.page.maxw));
                        o.dst = {};
                        o.dst.x = n - s;
                        o.dst.y = r - e;
                        o.dst.px = s;
                        o.dst.py = e;
                        f = Math.round(Math.sqrt(Math.pow(o.dst.x, 2) + Math.pow(o.dst.y, 2)));
                        o.dst.ax = o.dst.x / f;
                        o.dst.ay = o.dst.y / f;
                        c = 0;
                        h = f;
                        0 == o.dst.x ? (c = e,
                            h = r,
                            o.dst.ay = 1,
                            o.dst.py = 0) : 0 == o.dst.y && (c = s,
                                h = n,
                                o.dst.ax = 1,
                                o.dst.px = 0);
                        f = o.getTransitionSpeed(f);
                        u && 1 >= u && (f *= u);
                        o.bzscroll = 0 < f ? o.bzscroll ? o.bzscroll.update(h, f) : new BezierClass(c, h, f, 0, 1, 0, 1) : !1;
                        o.timer || ((e == o.page.maxh && r >= o.page.maxh || s == o.page.maxw && n >= o.page.maxw) && o.checkContentSize(),
                            a = 1,
                            o.cancelAnimationFrame = !1,
                            o.timer = 1,
                            o.onscrollstart && !o.scrollrunning && o.onscrollstart.call(o, {
                                type: "scrollstart",
                                current: {
                                    x: s,
                                    y: e
                                },
                                request: {
                                    x: n,
                                    y: r
                                },
                                end: {
                                    x: o.newscrollx,
                                    y: o.newscrolly
                                },
                                speed: f
                            }),
                            l(),
                            (e == o.page.maxh && r >= e || s == o.page.maxw && n >= s) && o.checkContentSize(),
                            o.noticeCursor())
                    }
                    ,
                    this.cancelScroll = function () {
                        return o.timer && i(o.timer),
                            o.timer = 0,
                            o.bzscroll = !1,
                            o.scrollrunning = !1,
                            o
                    }
                ) : (this.doScrollLeft = function (n, t) {
                    var i = o.getScrollTop();
                    o.doScrollPos(n, i, t)
                }
                    ,
                    this.doScrollTop = function (n, t) {
                        var i = o.getScrollLeft();
                        o.doScrollPos(i, n, t)
                    }
                    ,
                    this.doScrollPos = function (n, t) {
                        var r = n > o.page.maxw ? o.page.maxw : n, i;
                        0 > r && (r = 0);
                        i = t > o.page.maxh ? o.page.maxh : t;
                        0 > i && (i = 0);
                        o.synched("scroll", function () {
                            o.setScrollTop(i);
                            o.setScrollLeft(r)
                        })
                    }
                    ,
                    this.cancelScroll = function () { }
                );
                this.doScrollBy = function (n, t) {
                    var i = 0, i = t ? Math.floor((o.scroll.y - n) * o.scrollratio.y) : (o.timer ? o.newscrolly : o.getScrollTop(!0)) - n, r;
                    if (o.bouncescroll && (r = Math.round(o.view.h / 2),
                        i < -r ? i = -r : i > o.page.maxh + r && (i = o.page.maxh + r)),
                        o.cursorfreezed = !1,
                        py = o.getScrollTop(!0),
                        0 > i && 0 >= py)
                        return o.noticeCursor();
                    if (i > o.page.maxh && py >= o.page.maxh)
                        return o.checkContentSize(),
                            o.noticeCursor();
                    o.doScrollTop(i)
                }
                    ;
                this.doScrollLeftBy = function (n, t) {
                    var i = 0, i = t ? Math.floor((o.scroll.x - n) * o.scrollratio.x) : (o.timer ? o.newscrollx : o.getScrollLeft(!0)) - n, r;
                    if (o.bouncescroll && (r = Math.round(o.view.w / 2),
                        i < -r ? i = -r : i > o.page.maxw + r && (i = o.page.maxw + r)),
                        o.cursorfreezed = !1,
                        px = o.getScrollLeft(!0),
                        0 > i && 0 >= px || i > o.page.maxw && px >= o.page.maxw)
                        return o.noticeCursor();
                    o.doScrollLeft(i)
                }
                    ;
                this.doScrollTo = function (n, t) {
                    t && Math.round(n * o.scrollratio.y);
                    o.cursorfreezed = !1;
                    o.doScrollTop(n)
                }
                    ;
                this.checkContentSize = function () {
                    var n = o.getContentSize();
                    (n.h != o.page.h || n.w != o.page.w) && o.resize(!1, n)
                }
                    ;
                o.onscroll = function () {
                    o.rail.drag || o.cursorfreezed || o.synched("scroll", function () {
                        o.scroll.y = Math.round(o.getScrollTop() * (1 / o.scrollratio.y));
                        o.railh && (o.scroll.x = Math.round(o.getScrollLeft() * (1 / o.scrollratio.x)));
                        o.noticeCursor()
                    })
                }
                    ;
                o.bind(o.docscroll, "scroll", o.onscroll);
                this.doZoomIn = function (t) {
                    var i, f, e, u;
                    if (!o.zoomactive) {
                        o.zoomactive = !0;
                        o.zoomrestore = {
                            style: {}
                        };
                        i = "position top left zIndex backgroundColor marginTop marginBottom marginLeft marginRight".split(" ");
                        f = o.win[0].style;
                        for (e in i)
                            u = i[e],
                                o.zoomrestore.style[u] = "undefined" != typeof f[u] ? f[u] : "";
                        return o.zoomrestore.style.width = o.win.css("width"),
                            o.zoomrestore.style.height = o.win.css("height"),
                            o.zoomrestore.padding = {
                                w: o.win.outerWidth() - o.win.width(),
                                h: o.win.outerHeight() - o.win.height()
                            },
                            h.isios4 && (o.zoomrestore.scrollTop = n(window).scrollTop(),
                                n(window).scrollTop(0)),
                            o.win.css({
                                position: h.isios4 ? "absolute" : "fixed",
                                top: 0,
                                left: 0,
                                "z-index": r + 100,
                                margin: "0px"
                            }),
                            i = o.win.css("backgroundColor"),
                            ("" == i || /transparent|rgba\(0, 0, 0, 0\)|rgba\(0,0,0,0\)/.test(i)) && o.win.css("backgroundColor", "#fff"),
                            o.rail.css({
                                "z-index": r + 101
                            }),
                            o.zoom.css({
                                "z-index": r + 102
                            }),
                            o.zoom.css("backgroundPosition", "0px -18px"),
                            o.resizeZoom(),
                            o.onzoomin && o.onzoomin.call(o),
                            o.cancelEvent(t)
                    }
                }
                    ;
                this.doZoomOut = function (t) {
                    if (o.zoomactive)
                        return o.zoomactive = !1,
                            o.win.css("margin", ""),
                            o.win.css(o.zoomrestore.style),
                            h.isios4 && n(window).scrollTop(o.zoomrestore.scrollTop),
                            o.rail.css({
                                "z-index": o.zindex
                            }),
                            o.zoom.css({
                                "z-index": o.zindex
                            }),
                            o.zoomrestore = !1,
                            o.zoom.css("backgroundPosition", "0px 0px"),
                            o.onResize(),
                            o.onzoomout && o.onzoomout.call(o),
                            o.cancelEvent(t)
                }
                    ;
                this.doZoom = function (n) {
                    return o.zoomactive ? o.doZoomOut(n) : o.doZoomIn(n)
                }
                    ;
                this.resizeZoom = function () {
                    if (o.zoomactive) {
                        var t = o.getScrollTop();
                        o.win.css({
                            width: n(window).width() - o.zoomrestore.padding.w + "px",
                            height: n(window).height() - o.zoomrestore.padding.h + "px"
                        });
                        o.onResize();
                        o.setScrollTop(Math.min(o.page.maxh, t))
                    }
                }
                    ;
                this.init();
                n.nicescroll.push(this)
            }
            , w = function (n) {
                var t = this;
                this.nc = n;
                this.steptime = this.lasttime = this.speedy = this.speedx = this.lasty = this.lastx = 0;
                this.snapy = this.snapx = !1;
                this.demuly = this.demulx = 0;
                this.lastscrolly = this.lastscrollx = -1;
                this.timer = this.chky = this.chkx = 0;
                this.time = function () {
                    return +new Date
                }
                    ;
                this.reset = function (n, i) {
                    t.stop();
                    var r = t.time();
                    t.steptime = 0;
                    t.lasttime = r;
                    t.speedx = 0;
                    t.speedy = 0;
                    t.lastx = n;
                    t.lasty = i;
                    t.lastscrollx = -1;
                    t.lastscrolly = -1
                }
                    ;
                this.update = function (n, i) {
                    var r = t.time();
                    t.steptime = r - t.lasttime;
                    t.lasttime = r;
                    var r = i - t.lasty
                        , e = n - t.lastx
                        , u = t.nc.getScrollTop()
                        , f = t.nc.getScrollLeft()
                        , u = u + r
                        , f = f + e;
                    t.snapx = 0 > f || f > t.nc.page.maxw;
                    t.snapy = 0 > u || u > t.nc.page.maxh;
                    t.speedx = e;
                    t.speedy = r;
                    t.lastx = n;
                    t.lasty = i
                }
                    ;
                this.stop = function () {
                    t.nc.unsynched("domomentum2d");
                    t.timer && clearTimeout(t.timer);
                    t.timer = 0;
                    t.lastscrollx = -1;
                    t.lastscrolly = -1
                }
                    ;
                this.doSnapy = function (n, i) {
                    var r = !1;
                    0 > i ? (i = 0,
                        r = !0) : i > t.nc.page.maxh && (i = t.nc.page.maxh,
                            r = !0);
                    0 > n ? (n = 0,
                        r = !0) : n > t.nc.page.maxw && (n = t.nc.page.maxw,
                            r = !0);
                    r && t.nc.doScrollPos(n, i, t.nc.opt.snapbackspeed)
                }
                    ;
                this.doMomentum = function (n) {
                    var e = t.time(), u = n ? e + n : t.lasttime, f;
                    n = t.nc.getScrollLeft();
                    var h = t.nc.getScrollTop()
                        , o = t.nc.page.maxh
                        , s = t.nc.page.maxw;
                    if (t.speedx = 0 < s ? Math.min(60, t.speedx) : 0,
                        t.speedy = 0 < o ? Math.min(60, t.speedy) : 0,
                        u = u && 60 >= e - u,
                        (0 > h || h > o || 0 > n || n > s) && (u = !1),
                        n = t.speedx && u ? t.speedx : !1,
                        t.speedy && u && t.speedy || n) {
                        f = Math.max(16, t.steptime);
                        50 < f && (n = f / 50,
                            t.speedx *= n,
                            t.speedy *= n,
                            f = 50);
                        t.demulxy = 0;
                        t.lastscrollx = t.nc.getScrollLeft();
                        t.chkx = t.lastscrollx;
                        t.lastscrolly = t.nc.getScrollTop();
                        t.chky = t.lastscrolly;
                        var i = t.lastscrollx
                            , r = t.lastscrolly
                            , c = function () {
                                var n = 600 < t.time() - e ? .04 : .02;
                                t.speedx && (i = Math.floor(t.lastscrollx - t.speedx * (1 - t.demulxy)),
                                    t.lastscrollx = i,
                                    0 > i || i > s) && (n = .1);
                                t.speedy && (r = Math.floor(t.lastscrolly - t.speedy * (1 - t.demulxy)),
                                    t.lastscrolly = r,
                                    0 > r || r > o) && (n = .1);
                                t.demulxy = Math.min(1, t.demulxy + n);
                                t.nc.synched("domomentum2d", function () {
                                    t.speedx && (t.nc.getScrollLeft() != t.chkx && t.stop(),
                                        t.chkx = i,
                                        t.nc.setScrollLeft(i));
                                    t.speedy && (t.nc.getScrollTop() != t.chky && t.stop(),
                                        t.chky = r,
                                        t.nc.setScrollTop(r));
                                    t.timer || (t.nc.hideCursor(),
                                        t.doSnapy(i, r))
                                });
                                1 > t.demulxy ? t.timer = setTimeout(c, f) : (t.stop(),
                                    t.nc.hideCursor(),
                                    t.doSnapy(i, r))
                            };
                        c()
                    } else
                        t.doSnapy(t.nc.getScrollLeft(), t.nc.getScrollTop())
                }
            }
            , h = n.fn.scrollTop;
        n.cssHooks.pageYOffset = {
            get: function (t, i) {
                return (i = n.data(t, "__nicescroll") || !1) && i.ishwscroll ? i.getScrollTop() : h.call(t)
            },
            set: function (t, i) {
                var r = n.data(t, "__nicescroll") || !1;
                return r && r.ishwscroll ? r.setScrollTop(parseInt(i)) : h.call(t, i),
                    this
            }
        };
        n.fn.scrollTop = function (t) {
            if ("undefined" == typeof t) {
                var i = this[0] ? n.data(this[0], "__nicescroll") || !1 : !1;
                return i && i.ishwscroll ? i.getScrollTop() : h.call(this)
            }
            return this.each(function () {
                var i = n.data(this, "__nicescroll") || !1;
                i && i.ishwscroll ? i.setScrollTop(parseInt(t)) : h.call(n(this), t)
            })
        }
            ;
        u = n.fn.scrollLeft;
        n.cssHooks.pageXOffset = {
            get: function (t, i) {
                return (i = n.data(t, "__nicescroll") || !1) && i.ishwscroll ? i.getScrollLeft() : u.call(t)
            },
            set: function (t, i) {
                var r = n.data(t, "__nicescroll") || !1;
                return r && r.ishwscroll ? r.setScrollLeft(parseInt(i)) : u.call(t, i),
                    this
            }
        };
        n.fn.scrollLeft = function (t) {
            if ("undefined" == typeof t) {
                var i = this[0] ? n.data(this[0], "__nicescroll") || !1 : !1;
                return i && i.ishwscroll ? i.getScrollLeft() : u.call(this)
            }
            return this.each(function () {
                var i = n.data(this, "__nicescroll") || !1;
                i && i.ishwscroll ? i.setScrollLeft(parseInt(t)) : u.call(n(this), t)
            })
        }
            ;
        f = function (t) {
            var i = this, r;
            if (this.length = 0,
                this.name = "nicescrollarray",
                this.each = function (n) {
                    for (var t = 0, r = 0; t < i.length; t++)
                        n.call(i[t], r++);
                    return i
                }
                ,
                this.push = function (n) {
                    i[i.length] = n;
                    i.length++
                }
                ,
                this.eq = function (n) {
                    return i[n]
                }
                ,
                t)
                for (a = 0; a < t.length; a++)
                    r = n.data(t[a], "__nicescroll") || !1,
                        r && (this[this.length] = r,
                            this.length++);
            return this
        }
            ,
            function (n, t, i) {
                for (var r = 0; r < t.length; r++)
                    i(n, t[r])
            }(f.prototype, "show hide toggle onResize resize remove stop doScrollPos".split(" "), function (n, t) {
                n[t] = function () {
                    var n = arguments;
                    return this.each(function () {
                        this[t].apply(this, n)
                    })
                }
            });
        n.fn.getNiceScroll = function (t) {
            return "undefined" == typeof t ? new f(this) : this[t] && n.data(this[t], "__nicescroll") || !1
        }
            ;
        n.extend(n.expr[":"], {
            nicescroll: function (t) {
                return n.data(t, "__nicescroll") ? !0 : !1
            }
        });
        n.fn.niceScroll = function (t, i) {
            var r, u;
            return "undefined" != typeof i || "object" != typeof t || "jquery" in t || (i = t,
                t = !1),
                r = new f,
                "undefined" == typeof i && (i = {}),
                t && (i.doc = n(t),
                    i.win = n(this)),
                u = !("doc" in i),
                u || "win" in i || (i.win = n(this)),
                this.each(function () {
                    var t = n(this).data("__nicescroll") || !1;
                    t || (i.doc = u ? n(this) : i.doc,
                        t = new nt(i, n(this)),
                        n(this).data("__nicescroll", t));
                    r.push(t)
                }),
                1 == r.length ? r[0] : r
        }
            ;
        window.NiceScroll = {
            getjQuery: function () {
                return n
            }
        };
        n.nicescroll || (n.nicescroll = new f,
            n.nicescroll.options = p)
    }(jQuery)
