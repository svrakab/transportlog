/*!
 * Globalize v1.4.0
 *
 * http://github.com/jquery/globalize
 *
 * Copyright jQuery Foundation and other contributors
 * Released under the MIT license
 * http://jquery.org/license
 *
 * Date: 2018-07-17T20:38Z
 */
(function (root, factory) {
    // UMD returnExports
    if (typeof define === "function" && define.amd) {
        // AMD
        define([
            "cldr",
            "../globalize",
            "./number",
            "cldr/event",
            "cldr/supplemental"
        ], factory);
    }
    else if (typeof exports === "object") {
        // Node, CommonJS
        module.exports = factory(require("cldrjs"), require("../globalize"));
    }
    else {
        // Global
        factory(root.Cldr, root.Globalize);
    }
}(this, function (Cldr, Globalize) {
    var alwaysArray = Globalize._alwaysArray, formatMessage = Globalize._formatMessage, numberNumberingSystem = Globalize._numberNumberingSystem, numberPattern = Globalize._numberPattern, runtimeBind = Globalize._runtimeBind, stringPad = Globalize._stringPad, validateCldr = Globalize._validateCldr, validateDefaultLocale = Globalize._validateDefaultLocale, validateParameterPresence = Globalize._validateParameterPresence, validateParameterType = Globalize._validateParameterType, validateParameterTypeNumber = Globalize._validateParameterTypeNumber, validateParameterTypePlainObject = Globalize._validateParameterTypePlainObject;
    var validateParameterTypeCurrency = function (value, name) {
        validateParameterType(value, name, value === undefined || typeof value === "string" && (/^[A-Za-z]{3}$/).test(value), "3-letter currency code string as defined by ISO 4217");
    };
    /**
     * supplementalOverride( currency, pattern, cldr )
     *
     * Return pattern with fraction digits overriden by supplemental currency data.
     */
    var currencySupplementalOverride = function (currency, pattern, cldr) {
        var digits, fraction = "", fractionData = cldr.supplemental(["currencyData/fractions", currency]) ||
            cldr.supplemental("currencyData/fractions/DEFAULT");
        digits = +fractionData._digits;
        if (digits) {
            fraction = "." + stringPad("0", digits).slice(0, -1) + fractionData._rounding;
        }
        return pattern.replace(/\.(#+|0*[0-9]|0+[0-9]?)/g, fraction);
    };
    var objectFilter = function (object, testRe) {
        var key, copy = {};
        for (key in object) {
            if (testRe.test(key)) {
                copy[key] = object[key];
            }
        }
        return copy;
    };
    var currencyUnitPatterns = function (cldr) {
        return objectFilter(cldr.main([
            "numbers",
            "currencyFormats-numberSystem-" + numberNumberingSystem(cldr)
        ]), /^unitPattern/);
    };
    /**
     * codeProperties( currency, cldr )
     *
     * Return number pattern with the appropriate currency code in as literal.
     */
    var currencyCodeProperties = function (currency, cldr) {
        var pattern = numberPattern("decimal", cldr);
        // The number of decimal places and the rounding for each currency is not locale-specific. Those
        // values overridden by Supplemental Currency Data.
        pattern = currencySupplementalOverride(currency, pattern, cldr);
        return {
            currency: currency,
            pattern: pattern,
            unitPatterns: currencyUnitPatterns(cldr)
        };
    };
    /**
     * nameFormat( formattedNumber, pluralForm, properties )
     *
     * Return the appropriate name form currency format.
     */
    var currencyNameFormat = function (formattedNumber, pluralForm, properties) {
        var displayName, unitPattern, displayNames = properties.displayNames || {}, unitPatterns = properties.unitPatterns;
        displayName = displayNames["displayName-count-" + pluralForm] ||
            displayNames["displayName-count-other"] ||
            displayNames.displayName ||
            properties.currency;
        unitPattern = unitPatterns["unitPattern-count-" + pluralForm] ||
            unitPatterns["unitPattern-count-other"];
        return formatMessage(unitPattern, [formattedNumber, displayName]);
    };
    var currencyFormatterFn = function (numberFormatter, pluralGenerator, properties) {
        var fn;
        // Return formatter when style is "code" or "name".
        if (pluralGenerator && properties) {
            fn = function currencyFormatter(value) {
                validateParameterPresence(value, "value");
                validateParameterTypeNumber(value, "value");
                return currencyNameFormat(numberFormatter(value), pluralGenerator(value), properties);
            };
            // Return formatter when style is "symbol" or "accounting".
        }
        else {
            fn = function currencyFormatter(value) {
                return numberFormatter(value);
            };
        }
        return fn;
    };
    /**
     * nameProperties( currency, cldr )
     *
     * Return number pattern with the appropriate currency code in as literal.
     */
    var currencyNameProperties = function (currency, cldr) {
        var properties = currencyCodeProperties(currency, cldr);
        properties.displayNames = objectFilter(cldr.main([
            "numbers/currencies",
            currency
        ]), /^displayName/);
        return properties;
    };
    /**
     * Unicode regular expression for: everything except math symbols, currency signs, dingbats, and
     * box-drawing characters.
     *
     * Generated by:
     *
     * regenerate()
     *   .addRange( 0x0, 0x10FFFF )
     *   .remove( require( "unicode-7.0.0/categories/S/symbols" ) ).toString();
     *
     * https://github.com/mathiasbynens/regenerate
     * https://github.com/mathiasbynens/unicode-7.0.0
     */
    var regexpNotS = /[\0-#%-\*,-;\?-\]_a-\{\}\x7F-\xA1\xA7\xAA\xAB\xAD\xB2\xB3\xB5-\xB7\xB9-\xD6\xD8-\xF6\xF8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0300-\u0374\u0376-\u0383\u0386-\u03F5\u03F7-\u0481\u0483-\u058C\u0590-\u0605\u0609\u060A\u060C\u060D\u0610-\u06DD\u06DF-\u06E8\u06EA-\u06FC\u06FF-\u07F5\u07F7-\u09F1\u09F4-\u09F9\u09FC-\u0AF0\u0AF2-\u0B6F\u0B71-\u0BF2\u0BFB-\u0C7E\u0C80-\u0D78\u0D7A-\u0E3E\u0E40-\u0F00\u0F04-\u0F12\u0F14\u0F18\u0F19\u0F20-\u0F33\u0F35\u0F37\u0F39-\u0FBD\u0FC6\u0FCD\u0FD0-\u0FD4\u0FD9-\u109D\u10A0-\u138F\u139A-\u17DA\u17DC-\u193F\u1941-\u19DD\u1A00-\u1B60\u1B6B-\u1B73\u1B7D-\u1FBC\u1FBE\u1FC2-\u1FCC\u1FD0-\u1FDC\u1FE0-\u1FEC\u1FF0-\u1FFC\u1FFF-\u2043\u2045-\u2051\u2053-\u2079\u207D-\u2089\u208D-\u209F\u20BE-\u20FF\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2150-\u218F\u2308-\u230B\u2329\u232A\u23FB-\u23FF\u2427-\u243F\u244B-\u249B\u24EA-\u24FF\u2768-\u2793\u27C5\u27C6\u27E6-\u27EF\u2983-\u2998\u29D8-\u29DB\u29FC\u29FD\u2B74\u2B75\u2B96\u2B97\u2BBA-\u2BBC\u2BC9\u2BD2-\u2CE4\u2CEB-\u2E7F\u2E9A\u2EF4-\u2EFF\u2FD6-\u2FEF\u2FFC-\u3003\u3005-\u3011\u3014-\u301F\u3021-\u3035\u3038-\u303D\u3040-\u309A\u309D-\u318F\u3192-\u3195\u31A0-\u31BF\u31E4-\u31FF\u321F-\u3229\u3248-\u324F\u3251-\u325F\u3280-\u3289\u32B1-\u32BF\u32FF\u3400-\u4DBF\u4E00-\uA48F\uA4C7-\uA6FF\uA717-\uA71F\uA722-\uA788\uA78B-\uA827\uA82C-\uA835\uA83A-\uAA76\uAA7A-\uAB5A\uAB5C-\uD7FF\uDC00-\uFB28\uFB2A-\uFBB1\uFBC2-\uFDFB\uFDFE-\uFE61\uFE63\uFE67\uFE68\uFE6A-\uFF03\uFF05-\uFF0A\uFF0C-\uFF1B\uFF1F-\uFF3D\uFF3F\uFF41-\uFF5B\uFF5D\uFF5F-\uFFDF\uFFE7\uFFEF-\uFFFB\uFFFE\uFFFF]|\uD800[\uDC00-\uDD36\uDD40-\uDD78\uDD8A\uDD8B\uDD8D-\uDD8F\uDD9C-\uDD9F\uDDA1-\uDDCF\uDDFD-\uDFFF]|[\uD801\uD803-\uD819\uD81B-\uD82E\uD830-\uD833\uD836-\uD83A\uD83F-\uDBFF][\uDC00-\uDFFF]|\uD802[\uDC00-\uDC76\uDC79-\uDEC7\uDEC9-\uDFFF]|\uD81A[\uDC00-\uDF3B\uDF40-\uDF44\uDF46-\uDFFF]|\uD82F[\uDC00-\uDC9B\uDC9D-\uDFFF]|\uD834[\uDCF6-\uDCFF\uDD27\uDD28\uDD65-\uDD69\uDD6D-\uDD82\uDD85-\uDD8B\uDDAA-\uDDAD\uDDDE-\uDDFF\uDE42-\uDE44\uDE46-\uDEFF\uDF57-\uDFFF]|\uD835[\uDC00-\uDEC0\uDEC2-\uDEDA\uDEDC-\uDEFA\uDEFC-\uDF14\uDF16-\uDF34\uDF36-\uDF4E\uDF50-\uDF6E\uDF70-\uDF88\uDF8A-\uDFA8\uDFAA-\uDFC2\uDFC4-\uDFFF]|\uD83B[\uDC00-\uDEEF\uDEF2-\uDFFF]|\uD83C[\uDC2C-\uDC2F\uDC94-\uDC9F\uDCAF\uDCB0\uDCC0\uDCD0\uDCF6-\uDD0F\uDD2F\uDD6C-\uDD6F\uDD9B-\uDDE5\uDE03-\uDE0F\uDE3B-\uDE3F\uDE49-\uDE4F\uDE52-\uDEFF\uDF2D-\uDF2F\uDF7E\uDF7F\uDFCF-\uDFD3\uDFF8-\uDFFF]|\uD83D[\uDCFF\uDD4B-\uDD4F\uDD7A\uDDA4\uDE43\uDE44\uDED0-\uDEDF\uDEED-\uDEEF\uDEF4-\uDEFF\uDF74-\uDF7F\uDFD5-\uDFFF]|\uD83E[\uDC0C-\uDC0F\uDC48-\uDC4F\uDC5A-\uDC5F\uDC88-\uDC8F\uDCAE-\uDFFF]|[\uD800-\uDBFF]/;
    /**
     * symbolProperties( currency, cldr )
     *
     * Return pattern replacing `¤` with the appropriate currency symbol literal.
     */
    var currencySymbolProperties = function (currency, cldr, options) {
        var currencySpacing, pattern, symbol, symbolEntries = ["symbol"], regexp = {
            "[:digit:]": /\d/,
            "[:^S:]": regexpNotS
        };
        // If options.symbolForm === "narrow" was passed, prepend it.
        if (options.symbolForm === "narrow") {
            symbolEntries.unshift("symbol-alt-narrow");
        }
        symbolEntries.some(function (symbolEntry) {
            return symbol = cldr.main([
                "numbers/currencies",
                currency,
                symbolEntry
            ]);
        });
        currencySpacing = ["beforeCurrency", "afterCurrency"].map(function (position) {
            return cldr.main([
                "numbers",
                "currencyFormats-numberSystem-" + numberNumberingSystem(cldr),
                "currencySpacing",
                position
            ]);
        });
        pattern = cldr.main([
            "numbers",
            "currencyFormats-numberSystem-" + numberNumberingSystem(cldr),
            options.style === "accounting" ? "accounting" : "standard"
        ]);
        pattern =
            // The number of decimal places and the rounding for each currency is not locale-specific.
            // Those values are overridden by Supplemental Currency Data.
            currencySupplementalOverride(currency, pattern, cldr)
                // Replace "¤" (\u00A4) with the appropriate symbol literal.
                .split(";").map(function (pattern) {
                return pattern.split("\u00A4").map(function (part, i) {
                    var currencyMatch = regexp[currencySpacing[i].currencyMatch], surroundingMatch = regexp[currencySpacing[i].surroundingMatch], insertBetween = "";
                    // For currencyMatch and surroundingMatch definitions, read [1].
                    // When i === 0, beforeCurrency is being handled. Otherwise, afterCurrency.
                    // 1: http://www.unicode.org/reports/tr35/tr35-numbers.html#Currencies
                    currencyMatch = currencyMatch.test(symbol.charAt(i ? symbol.length - 1 : 0));
                    surroundingMatch = surroundingMatch.test(part.charAt(i ? 0 : part.length - 1).replace(/[#@,.]/g, "0"));
                    if (currencyMatch && part && surroundingMatch) {
                        insertBetween = currencySpacing[i].insertBetween;
                    }
                    return (i ? insertBetween : "") + part + (i ? "" : insertBetween);
                }).join("'" + symbol + "'");
            }).join(";");
        return {
            pattern: pattern
        };
    };
    /**
     * objectOmit( object, keys )
     *
     * Return a copy of the object, filtered to omit the blacklisted key or array of keys.
     */
    var objectOmit = function (object, keys) {
        var key, copy = {};
        keys = alwaysArray(keys);
        for (key in object) {
            if (keys.indexOf(key) === -1) {
                copy[key] = object[key];
            }
        }
        return copy;
    };
    function validateRequiredCldr(path, value) {
        validateCldr(path, value, {
            skip: [
                /numbers\/currencies\/[^/]+\/symbol-alt-/,
                /supplemental\/currencyData\/fractions\/[A-Za-z]{3}$/
            ]
        });
    }
    /**
     * .currencyFormatter( currency [, options] )
     *
     * @currency [String] 3-letter currency code as defined by ISO 4217.
     *
     * @options [Object]:
     * - style: [String] "symbol" (default), "accounting", "code" or "name".
     * - see also number/format options.
     *
     * Return a function that formats a currency according to the given options and default/instance
     * locale.
     */
    Globalize.currencyFormatter =
        Globalize.prototype.currencyFormatter = function (currency, options) {
            var args, cldr, numberFormatter, pluralGenerator, properties, returnFn, style;
            validateParameterPresence(currency, "currency");
            validateParameterTypeCurrency(currency, "currency");
            validateParameterTypePlainObject(options, "options");
            cldr = this.cldr;
            options = options || {};
            args = [currency, options];
            style = options.style || "symbol";
            validateDefaultLocale(cldr);
            // Get properties given style ("symbol" default, "code" or "name").
            cldr.on("get", validateRequiredCldr);
            properties = ({
                accounting: currencySymbolProperties,
                code: currencyCodeProperties,
                name: currencyNameProperties,
                symbol: currencySymbolProperties
            }[style])(currency, cldr, options);
            cldr.off("get", validateRequiredCldr);
            // options = options minus style, plus raw pattern.
            options = objectOmit(options, "style");
            options.raw = properties.pattern;
            // Return formatter when style is "symbol" or "accounting".
            if (style === "symbol" || style === "accounting") {
                numberFormatter = this.numberFormatter(options);
                returnFn = currencyFormatterFn(numberFormatter);
                runtimeBind(args, cldr, returnFn, [numberFormatter]);
                // Return formatter when style is "code" or "name".
            }
            else {
                numberFormatter = this.numberFormatter(options);
                pluralGenerator = this.pluralGenerator();
                returnFn = currencyFormatterFn(numberFormatter, pluralGenerator, properties);
                runtimeBind(args, cldr, returnFn, [numberFormatter, pluralGenerator, properties]);
            }
            return returnFn;
        };
    /**
     * .currencyParser( currency [, options] )
     *
     * @currency [String] 3-letter currency code as defined by ISO 4217.
     *
     * @options [Object] see currencyFormatter.
     *
     * Return the currency parser according to the given options and the default/instance locale.
     */
    Globalize.currencyParser =
        Globalize.prototype.currencyParser = function ( /* currency, options */) {
            // TODO implement parser.
        };
    /**
     * .formatCurrency( value, currency [, options] )
     *
     * @value [Number] number to be formatted.
     *
     * @currency [String] 3-letter currency code as defined by ISO 4217.
     *
     * @options [Object] see currencyFormatter.
     *
     * Format a currency according to the given options and the default/instance locale.
     */
    Globalize.formatCurrency =
        Globalize.prototype.formatCurrency = function (value, currency, options) {
            validateParameterPresence(value, "value");
            validateParameterTypeNumber(value, "value");
            return this.currencyFormatter(currency, options)(value);
        };
    /**
     * .parseCurrency( value, currency [, options] )
     *
     * @value [String]
     *
     * @currency [String] 3-letter currency code as defined by ISO 4217.
     *
     * @options [Object]: See currencyFormatter.
     *
     * Return the parsed currency or NaN when value is invalid.
     */
    Globalize.parseCurrency =
        Globalize.prototype.parseCurrency = function ( /* value, currency, options */) {
        };
    return Globalize;
}));
