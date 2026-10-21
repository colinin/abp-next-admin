var abp = abp || {};
(function ($) {

    if (!abp.ajax || !abp.ajax.defaultOpts) {
        throw "abp/jquery library requires the jquery library included to the page!";
    }

    var headers = abp.ajax.defaultOpts.headers || {};
    headers['_abpdontwrapresult'] = 'true';

})(jQuery);