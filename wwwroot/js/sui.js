var baseUrl = '';

String.prototype.formatString = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
};

String.prototype.replaceAll = function (src, dec) {
    return this.replace(new RegExp(src, 'gm'), dec);
};

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
};

Date.prototype.formatDate = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

function myPost(url, data, callback) {
    execAjax('POST', true, true, url, data, false, callback);
};

function myGet(url, data, callback) {
    execAjax('GET', true, false, url, data, false, callback);
};

function myList(url, data, callback) {
    execAjax('POST', true, true, url, data, true, callback);
};

function myExec(url, data, callback) {
    execAjax('POST', false, true, url, data, false, callback);
};

function execAjax(type, async, showloading, url, data, islist, callback) {
    var loadIdx = null;
    $.ajax({
        url: baseUrl + url,
        type: type,
        data: data || {},
        cache: false,
        global: false,
        async: async,
        dataType: 'json',
        beforeSend: function () {
            showloading && (loadIdx = layer.load());
        },
        success: function (res) {
            if (res.success) {
                callback && callback(islist ? res : res.data);
            } else {
                if (res.data && res.data.rcode && res.data.rcode == -99) {
                    sui.login(false);
                } else {
                    layer.msg(res.message, { time: 3000 });
                }
            }
        },
        error: function (xhr, status, text) {
            layer.msg(text, { time: 3000 });
        },
        complete: function () {
            loadIdx && layer.close(loadIdx);
        }
    });
};

; (function ($, window, document, undefined) {
    $.fn.loadPage = function (url, data, callback) {
        var _this = this;

        $.ajax({
            type: 'POST',
            url: baseUrl + url,
            data: data || {},
            dataType: 'html',
            cache: false,
            global: false,
            async: true,
            success: function (res) {
                var idx_bg = ~res.indexOf('<body>') ? (res.indexOf('<body>') + 6) : 0,
                    idx_ed = ~res.indexOf('</body>') ? res.lastIndexOf('</body>') : res.length;
                _this.html(res.substring(idx_bg, idx_ed));
                callback && callback();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.msg(errorThrown, { time: 3000 });
            }
        });

        return _this;
    };

    $.fn.loadData = function (url, data, callback) {
        var _this = this;

        myPost(url, data, function (data) {
            switch ($.type(data)) {
                case 'array':
                    $.each(data, function (i, item) {
                        _this.fillData(item);
                    });
                    break;
                case 'object':
                    $.each(data, function (key, val) {
                        if ($.type(val) === 'array') {
                            $.each(val, function (v, vitem) {
                                _this.fillData(vitem);
                            });
                        } else {
                            _this.find(('[name="{0}"]').formatString(key)).setVal(val);
                        }
                    });
                    break;
                case 'string':
                    data && _this.fillData($.parseJSON(data));
                    break;
            }

            callback && callback(data);
        });

        return _this;
    };

    $.fn.postData = function (url, data, callback) {
        var _this = this;

        if (!_this.checkForm()) return;

        data = data || {};
        $.extend(data, _this.getParams());

        myPost(url, data, callback);

        return _this;
    };

    $.fn.getParams = function () {
        var _this = this,
            params = {},
            name = '';

        _this.find('[name]').each(function () {
            name = $(this).attr('name');
            !params[name] && (params[name] = _this.find('[name="' + $(this).attr('name') + '"]').getVal());
        });

        return params;
    };

    $.fn.checkForm = function () {
        layer.close(layer.tips());

        var _this = this;
        res = true,
            val = '',
            dtype = '';
        function showTips(title, obj) {
            res = false;
            layer.tips(title, obj, {
                tips: [2, '#a94442'],
                time: 0,
                tipsMore: true
            });
        }

        _this.find('[name][data-required]').each(function () {
            if ($(this).getVal() != '') return true;
            showTips('不能为空', this);
        });
        if (res) {
            _this.find('[name][data-dtype]').each(function () {
                val = $(this).getVal();
                dtype = $(this).data('dtype');

                if (val == '' || dtype == '') return true;

                switch (dtype) {
                    case 'int':
                        if (!(/^\d+$/.test(val))) showTips('只能是整数', this);
                        break;
                    case 'float':
                        if (!(/^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(val))) showTips('只能是数字', this);
                        break;
                    case 'phone':
                        if (!(/^1(?:3\d|4[4-9]|5[0-35-9]|6[67]|7[013-8]|8\d|9\d)\d{8}$/.test(val))) showTips('手机号码不正确', this);
                        break;
                    case 'tel':
                        if (!(/^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/.test(val))) showTips('电话号码不正确', this);
                        break;
                    case 'idno':
                        if (!(/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/.test(val))) showTips('身份证号不正确', this);
                        break;
                    case 'email':
                        if (!(/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(val))) showTips('邮箱不正确', this);
                        break;
                }
            });
        }
        if (res) {
            _this.find('[name][data-maxlength]').each(function () {
                val = $(this).getVal();
                var maxlength = $(this).data('maxlength');
                if (val == '' || val.length <= parseInt(maxlength)) return true;
                showTips(('最多只能输入{0}个字符').formatString(maxlength), this);
            });
        }
        if (res) {
            _this.find('[name][data-minlength]').each(function () {
                val = $(this).getVal();
                var minlength = $(this).data('minlength');
                if (val == '' || val.length >= parseInt(minlength)) return true;
                showTips(('至少要输入{0}个字符').formatString(minlength), this);
            });
        }

        res && layer.close(layer.tips());
        return res;
    };

    $.fn.fillData = function (data) {
        return this.each(function () {
            var _this = $(this);
            $.each(data, function (key, val) {
                _this.find(('[name="{0}"]').formatString(key)).setVal(val);
            });
        });
    };

    $.fn.clear = function (callback) {
        return this.each(function () {
            $(this).find('[name]').each(function () {
                var _this = $(this);
                if (_this.is(':checkbox') || _this.is(':radio')) {
                    _this.prop('checked', false);
                } else if (_this.is('span') || _this.is('label') || _this.is('p')) {
                    _this.html('');
                } else if (_this.is('div')) {
                    if (_this.hasClass('sui-checkbox') || _this.hasClass('sui-radio') || _this.hasClass('sui-switch')) {
                        _this.removeClass('sui-active');
                    } else if (_this.hasClass('sui-select')) {
                        _this.find('span').html('').end().find('dd').removeClass('sui-active');
                    } else {
                        _this.html('');
                    }
                } else {
                    _this.val('');
                }
            });
            callback && callback();
        });
    };

    $.fn.goTop = function (opts) {
        var _this = this,
            settings = $.extend({
                ele: 'html,body',
                offset: 200,
                speed: 400
            }, opts || {});

        var _ele = $(settings.ele),
            _srcollObj = opts ? _ele : $(window);

        _this.click(function () {
            _ele.animate({ scrollTop: 0 }, settings.speed);
        });

        _srcollObj.scroll(function () {
            if (_ele.scrollTop() > settings.offset) {
                _this.fadeIn(settings.speed);
            } else {
                _this.fadeOut(settings.speed);
            }
        });

        return _this;
    };

    $.fn.getVal = function () {
        var _this = this,
            val = '';

        if (_this.is(':checkbox')) {
            _this.each(function () {
                $(this).is(':checked') && (val += (val != '' ? ',' : '') + $(this).val());
            });
        } else if (_this.is(':radio')) {
            _this.each(function () {
                $(this).is(':checked') && (val = $(this).val());
            });
        } else if (_this.is('span') || _this.is('label') || _this.is('p')) {
            val = _this.html().trim();
        } else if (_this.is('div')) {
            if (_this.hasClass('sui-checkbox')) {
                _this.each(function () {
                    $(this).hasClass('sui-active') && (val += (val != '' ? ',' : '') + $(this).data('value'));
                });
            } else if (_this.hasClass('sui-radio')) {
                _this.each(function () {
                    $(this).hasClass('sui-active') && (val = $(this).data('value'));
                });
            } else if (_this.hasClass('sui-switch')) {
                val = _this.hasClass('sui-active') ? 1 : 0;
            } else if (_this.hasClass('sui-select')) {
                val = _this.find('dd.sui-active').data('value');
            } else if (_this.is('div')) {
                val = _this.html().trim();
            }
        } else {
            val = _this.val().trim();
        }

        return val;
    };

    $.fn.setVal = function (val) {
        val = (val === null || typeof (val) === 'undefined') ? '' : val.toString();

        return this.each(function () {
            var _this = $(this);

            if (_this.is(':checkbox') || _this.is(':radio')) {
                _this.prop(':checked', false).each(function (c, citem) {
                    $.each(val.split(','), function (v, vitem) {
                        if (vitem == '') return true;
                        vitem == $(citem).val() && $(citem).prop(':checked', true);
                    });
                });
            } else if (_this.is('span') || _this.is('label') || _this.is('p')) {
                _this.html(sui.htmlEncode(val).replaceAll('&quot;', '\"').replaceAll('&#39;', '\'').replaceAll('\r\n', '<br/>').replaceAll('\r', '<br/>').replaceAll('\n', '<br/>'));
            } else if (_this.is('div')) {
                if (_this.hasClass('sui-checkbox') || _this.hasClass('sui-radio')) {
                    _this.removeClass('sui-active').each(function (c, citem) {
                        $.each(val.toString().split(','), function (v, vitem) {
                            if (vitem == '') return true;
                            $(citem).data('value') == vitem && $(citem).addClass('sui-active');
                        });
                    });
                } else if (_this.hasClass('sui-switch')) {
                    val == '1' ? _this.addClass('sui-active') : _this.removeClass('sui-active');
                } else if (_this.hasClass('sui-select')) {
                    _this.find('span').text('').end().find('dd').removeClass('sui-active');
                    var _dd = _this.find('dd[data-value="' + val + '"]').addClass('sui-active');
                    _dd && _this.find('span').text(_dd.text());
                } else {
                    _this.html(sui.htmlEncode(val).replaceAll('&quot;', '\"').replaceAll('&#39;', '\'').replaceAll('\r\n', '<br/>').replaceAll('\r', '<br/>').replaceAll('\n', '<br/>'));
                }
            } else {
                _this.val(val);
            }
        });
    };

    $.fn.initFile = function (opts) {
        var $ele = this,
            _settings = $.extend({
                enable_modify: true,
                multi_selection: true,
                filters: {},
                multipart_params: {
                    table_id: '',
                    f_type: 0
                }
            }, opts || {});

        return $ele.each(function () {
            var _this = $(this);
            if (_this.find('ul').length) return true;

            var viewer = new Viewer(_this[0], {
                navbar: 0,
                title: 0,
                filter: function (image) {
                    return image.getAttribute('fimg') == 1;
                }
            });

            var $ul = $('<ul></ul>');
            _this.append($ul);

            if (_settings.enable_modify) {
                $ul.append('<li class="sui-filebtn"><button type="button" class="sui-btn sui-btn-lg">上 传</button></li>');
            }

            myGet('pub/FileHandler.ashx?act=download', {
                'table_id': _settings.multipart_params.table_id,
                'f_type': _settings.multipart_params.f_type
            }, function (res) {
                $ul.append(getFileItem(res, _settings.enable_modify));
                viewer.update();
            });

            _this.on('click', 'button', function () {
                uploader.setOption('multi_selection', _settings.multi_selection);
                uploader.setOption('filters', _settings.filters);
                uploader.setOption('multipart_params', _settings.multipart_params);
                uploader.unbind('suploaded');
                uploader.bind('suploaded', function (event, data) {
                    $ul.append(getFileItem(data, _settings.enable_modify));
                    viewer.update();
                });
                $('#pickfiles').click();
            }).on('click', 'i', function () {
                var $li = $(this).parents('li');
                layer.confirm('您确定要删除吗？', {
                    yes: function (idx, lyo) {
                        myPost('pub/FileHandler.ashx?act=delete', { 'idList': $li.data('value') }, function () {
                            $li.remove();
                            viewer.update();
                        });
                        layer.close(idx);
                    },
                    btn2: function (idx, lyo) {
                        layer.close(idx);
                    }
                });
            });
        });

        function getFileItem(itemArray, enableDelete) {
            var html = [], filetype = '';
            $.each(itemArray, function (i, item) {
                switch (item.f_ext) {
                    case 'jpg':
                    case 'jpeg':
                    case 'png':
                    case 'gif':
                    case 'bmp':
                        filetype = 'img';
                        break;
                    case 'doc':
                    case 'docx':
                    case 'wps':
                        filetype = 'doc';
                        break;
                    case 'xls':
                    case 'xlsx':
                    case 'et':
                        filetype = 'xls';
                        break;
                    case 'ppt':
                    case 'pptx':
                    case 'dps':
                        filetype = 'ppt';
                        break;
                    case 'exe':
                        filetype = 'exe';
                        break;
                    case 'mp3':
                    case 'flac':
                    case 'wav':
                        filetype = 'mp3';
                        break;
                    case 'mp4':
                    case 'wma':
                    case 'avi':
                    case 'rmvb':
                    case 'mkv':
                        filetype = 'mp4';
                        break;
                    case 'pdf':
                    case 'ceb':
                    case 'cebx':
                        filetype = 'pdf';
                        break;
                    case 'rar':
                    case 'zip':
                    case 'cab':
                    case 'iso':
                    case '7z':
                    case 'tar':
                        filetype = 'rar';
                        break;
                    case 'tif':
                    case 'tiff':
                        filetype = 'tif';
                        break;
                    case 'txt':
                        filetype = 'txt';
                        break;
                    default:
                        filetype = 'other';
                        break;
                }
                if (filetype == 'img') {
                    html.push(('<li class="sui-fileitem" data-value="{0}"><img src="{1}" alt="" fimg="1"/>{2}</li>').formatString(item.f_id, item.f_path, enableDelete ? '<i class="sui-icon sui-icon-delete"></i>' : ''));
                } else {
                    html.push(('<li class="sui-fileitem" data-value="{0}"><a target="_blank" href="{1}" title="{2}"><img src="{3}images/filetype/{4}.png" alt=""/><p>{2}</p></a>{5}</li>').formatString(item.f_id, item.f_path, item.f_name, baseUrl, filetype, enableDelete ? '<i class="sui-icon sui-icon-delete"></i>' : ''));
                }
            });
            return html.join('');
        };
    };

    $.fn.setRequired = function () {
        return this.find('[name][data-required]').each(function () {
            var _parent = $(this).parent();
            _parent.is('td') && _parent.prev().prepend('<label class="sui-required">*</label>');
        }).end();
    };

    $.fn.disable = function (edit_fields) {
        return this.find('[name]').each(function () {
            var _this = $(this);
            if (~(',' + edit_fields + ',').indexOf(',' + _this.attr('name') + ',') || _this.attr('type') == 'hidden') return true;

            if (_this.is(':checkbox') || _this.is(':radio')) {
                _this.prop('disabled', true);
            } else if (_this.is('select')) {
                _this.replaceWith($('<span></span>').attr('name', _this.attr('name')).html(_this.find('option:selected').text()));
            } else if (_this.is('input') || _this.is('textarea')) {
                _this.replaceWith($('<span></span>').attr('name', _this.attr('name')).html(sui.htmlEncode(_this.val()).replaceAll('&quot;', '\"').replaceAll('&#39;', '\'').replaceAll('\r\n', '<br/>').replaceAll('\r', '<br/>').replaceAll('\n', '<br/>')));
            } else if (_this.is('div')) {
                if (_this.hasClass('sui-select')) {
                    _this.replaceWith($('<span></span>').attr('name', _this.attr('name')).html(_this.find('dd.sui-active').text()));
                } else if (_this.hasClass('sui-radio') || _this.hasClass('sui-checkbox') || _this.hasClass('sui-switch')) {
                    _this.addClass('sui-disabled');
                }
            }
        }).end().find('label.sui-required').remove().end().setRequired().end();
    };

    $.fn.uploadFile = function (opts, callback) {
        var _settings = $.extend({
            multi_selection: true,
            filters: {},
            multipart_params: {
                table_id: '',
                f_type: 0
            }
        }, opts || {});

        return this.each(function () {
            $(this).on('click', function () {
                uploader.setOption('multi_selection', _settings.multi_selection);
                uploader.setOption('filters', _settings.filters);
                uploader.setOption('multipart_params', _settings.multipart_params);
                uploader.unbind('suploaded');
                uploader.bind('suploaded', function (event, data) {
                    callback && callback(data);
                });
                $('#pickfiles').click();
            });
        });
    };
}(jQuery, window, document));

var sui = (function ($, window, document) {
    $(window).resize(function () {
        setTabStyle();
    });

    function initForm() {
        //click outside select to close them
        $(document).click(function () {
            $('div.sui-select').removeClass('sui-active');
            $('span.sui-tab-bar').removeClass('sui-tab-bar-open').siblings('ul.sui-tab-title').removeClass('sui-tab-more');
        });

        //set input int,float
        $('body').on('keyup change blur', ':text[data-dtype="int"],:text[data-dtype="float"]', function () {
            setInput($(this));
        }).on('paste', ':text[data-dtype="int"],:text[data-dtype="float"]', function () {
            var _this = $(this);
            setTimeout(function () {
                setInput(_this);
            }, 200);
        });

        function setInput(obj) {
            if ($(obj).data('dtype') == 'int') {
                return $(obj).each(function () {
                    var _this = $(this);
                    _this.val(_this.val().trim().replace(/\D/g, ''));   //替换非数字字符
                    _this.val().trim() != '' && _this.val().trim().substring(0, 1) == 0 && _this.val(0);     //第一个数字为0则是0
                });
            } else {
                return $(obj).each(function () {
                    var _this = $(this);
                    _this.val(_this.val().trim().replace(/[^\d\.]/g, ''));  //先把非数字的都替换掉，除了数字和.
                    _this.val(_this.val().trim().replace(/^\./g, '0.'));    //必须保证第一个为数字而不是.
                    _this.val(_this.val().trim().replace(/\.{2,}/g, '.'));  //保证只有出现一个.而没有多个.
                    _this.val(_this.val().trim().replace('.', '$#$').replace(/\./g, '').replace('$#$', '.'));   //保证.只出现一次，而不能出现两次以上
                    _this.val().trim() != '' && _this.val().trim().length > 1 && _this.val().trim().substring(0, 2) == '00' && _this.val(0);     //开头不能连续出现两个0
                    _this.val().trim() != '' && _this.val().trim().length > 1 && _this.val().trim().substring(0, 1) == 0 && _this.val().trim().substring(0, 2) != '0.' && _this.val('0.' + _this.val().trim().substring(1));    //以0开头而没有.则自动添加
                });
            }
        };

        //set input date,datetime
        $('body').on('click focus', ':text[data-dtype="date"],:text[data-dtype="datetime"],:text[data-dtype="datemonth"]', function () {
            var _this = $(this),
                fmtStr = 'yyyy-MM-dd HH:mm';
            var Timetype = _this.data('dtype');
            switch (Timetype) {
                case 'datemonth': fmtStr = 'yyyy-MM'; break;
                case 'date': fmtStr = 'yyyy-MM-dd'; break;
            }
            //return WdatePicker({ dateFmt: _this.data('dtype') == 'date' ? 'yyyy-MM-dd' : 'yyyy-MM-dd HH:mm' });
            return WdatePicker({ dateFmt: fmtStr });
        });

        /* init checkbox
         * $('div.sui-checkbox').on('sclick', function (event, data) {
         *     console.log(data.isChecked+'-'+data.value+'-'+data.text);
         * });
        */
        $('body').on('click', 'div.sui-checkbox', function () {
            var _this = $(this);
            if (_this.hasClass('sui-disabled')) return;

            _this.toggleClass('sui-active').trigger('sclick', { 'isChecked': _this.hasClass('sui-active'), 'value': _this.data('value'), 'text': _this.find('span').text().trim() });
        });

        /* init radio
         * $('div.sui-radio').on('sclick', function (event, data) {
         *     console.log(data.value+'-'+data.text);
         * });
        */
        $('body').on('click', 'div.sui-radio', function () {
            var _this = $(this);
            if (_this.hasClass('sui-disabled') || _this.hasClass('sui-active')) return;

            $(('div.sui-radio[name="{0}"]').formatString(_this.attr('name'))).removeClass('sui-active');
            _this.addClass('sui-active').trigger('sclick', { 'value': _this.data('value'), 'text': _this.find('span').text().trim() });
        });

        /* init switch
         * $('div.sui-switch').on('schange', function (event, data) {
         *     console.log(data.isChecked);
         * });
        */
        $('body').on('click', 'div.sui-switch', function () {
            var _this = $(this);
            if (_this.hasClass('sui-disabled')) return;

            _this.toggleClass('sui-active').trigger('schange', { 'isChecked': _this.hasClass('sui-active') });
        });

        /* init select
         * $('div.sui-select').on('schange', function (event, data) {
         *     console.log(data.value+'-'+data.text);
         * });
        */
        $('body').on('click', 'div.sui-select', function (event) {
            var _this = $(this),
                dl_height = _this.find('dl').outerHeight(),
                offset_bottom = $(window).height() - _this.offset().top - _this.outerHeight() - $(window).scrollTop();

            $('div.sui-select').removeClass('sui-active');
            _this.addClass('sui-active').find('dl').removeClass('sui-open-above sui-open-below').addClass(offset_bottom > dl_height ? 'sui-open-below' : 'sui-open-above');
            event.stopPropagation();
        }).on('click', 'div.sui-select dd', function (event) {
            var _this = $(this);
            if (_this.hasClass('sui-active')) {
                _this.parents('div.sui-select').removeClass('sui-active');
            } else {
                _this.siblings().removeClass('sui-active').end().addClass('sui-active')
                    .parents('div.sui-select').find('span').text(_this.text()).end().removeClass('sui-active').trigger('schange', { 'value': _this.data('value'), 'text': _this.text() });
            }
            event.stopPropagation();
        });

        /* init tab
         * $('div.sui-tab').on('schange', function (event,data) {
         *     console.log(data.idx);
         * });
        */
        $('body').on('click', 'ul.sui-tab-title>li', function (event) {
            var _this = $(this),
                idx = _this.index();

            _this.parents('ul.sui-tab-title').removeClass('sui-tab-more').siblings('span.sui-tab-bar').removeClass('sui-tab-bar-open');

            if (_this.hasClass('sui-active')) return;

            _this.addClass('sui-active').siblings().removeClass('sui-active')
                .parent().siblings('div.sui-tab-content')
                .find('.sui-tab-item:eq(' + idx + ')').addClass('sui-active')
                .siblings().removeClass('sui-active').hide().end().fadeIn(300).end().parent().trigger('schange', { 'idx': idx });
            event.stopPropagation();
        }).on('click', 'span.sui-tab-bar', function (event) {
            $(this).toggleClass('sui-tab-bar-open').siblings('ul.sui-tab-title').toggleClass('sui-tab-more');
            event.stopPropagation();
        });

        $('body').on('click', '[data-action="sui-backlist"]', function () {
            sui.goBack();
        });
    };

    function setTabStyle() {
        $('div.sui-tab').each(function () {
            var _this = $(this),
                titleWidth = _this.find('ul.sui-tab-title').outerWidth(),
                btnWidth = 0,
                liWidth = 0;

            _this.find('ul.sui-tab-title>li').each(function () {
                $(this).is(':visible') && (liWidth += $(this).outerWidth());
            }).end().find('div.sui-tab-btn>button').each(function () {
                $(this).is(':visible') && (btnWidth += $(this).outerWidth());
            });

            if ((liWidth + btnWidth + 30) > titleWidth) {
                _this.addClass('sui-tab-titleline');
                !_this.find('span.sui-tab-bar').length && _this.append('<span class="sui-tab-bar"><i class="sui-icon sui-icon-arrow-right"></i></span>');
            } else {
                _this.removeClass('sui-tab-titleline').find('span.sui-tab-bar').remove();
            }
        });
    };

    function doEdit(url, data, callback) {
        $('#divPageList').slideUp(300, function () {
            var _this = $(this).next();
            _this.loadPage(url, data, function () {
                setTabStyle();
                _this.setRequired();
                callback && callback();
            }).show();
        });
    };

    function goBack(callback) {
        layer.close(layer.tips());//关闭表单页面的验证错误信息
        $('#divPageContent').slideUp(300, function () {
            $(this).prev().slideDown(300, function () {
                callback && callback();
            });
        }).empty();
    };

    function saveBack(callback) {
        layer.msg('操作成功', function () {
            sui.goBack(callback);
        });
    };

    function selectUser($objVal, $objText, title, data, callback) {
        var params = 'dmp=' + new Date().getTime();
        if (data) {
            $.each(data, function (key, val) {
                params += (params != '' ? '&' : '') + key + '=' + val;
            });
        }
        layer.open({
            type: 2,
            title: title,
            area: ['900px', '600px'],
            content: baseUrl + 'pub/select_user.aspx?' + params,
            btn: ['确 定', '关 闭'],
            yes: function (idx) {
                var val = [], text = [], _this = null;
                layer.getChildFrame('#divSelected a', idx).each(function () {
                    _this = $(this);
                    val.push(_this.data('value'));
                    text.push(_this.text());
                });
                $objVal.val(val.join(','));
                $objText.val(text.join(','));

                callback && callback(val, text);
                layer.close(idx);
            },
            btn2: function (idx) {
                layer.close(idx);
            },
            success: function (lyo, idx) {
                var $divSelected = layer.getChildFrame('#divSelected', idx),
                    val = $objVal.val(),
                    text = $objText.val();
                if (val == '') return;

                var arr_val = val.split(','),
                    arr_text = text.split(',');
                for (var i = 0; i < arr_val.length; i++) {
                    $divSelected.append($(('<a href="javascript:void(0);" data-value="{0}">{1}</a>').formatString(arr_val[i], arr_text[i])));
                }
            }
        });
    };

    function htmlEncode(val) {
        return $('<div/>').text(val).html();
    };

    function getCookie(name) {
        var arg = name + '=',
            alen = arg.length,
            clen = document.cookie.length,
            i = 0;
        while (i < clen) {
            var j = i + alen;
            if (document.cookie.substring(i, j) == arg) {
                var endstr = document.cookie.indexOf(';', j);
                if (endstr == -1) {
                    endstr = document.cookie.length;
                }
                return unescape(document.cookie.substring(j, endstr));
            }
            i = document.cookie.indexOf(' ', i) + 1;
            if (i == 0) break;
        }
        return null;
    }

    function login(from_page) {
        layer.open({
            type: 1,
            title: '您的帐号已在其他地方登录，您已经被踢出，请重新登录',
            area: ['460px', '280px'],
            content: '<div style="margin:30px 30px 30px 0;"><div class="sui-input-row"><label>登录帐号：</label><div class="sui-input-body"><input type="text" class="sui-input" id="txt_loginaccount"/></div></div><div class="sui-input-row"><label>登录密码：</label><div class="sui-input-body"><input type="password" class="sui-input" id="txt_loginpwd"/></div></div></div>',
            btn: ['登 录', '取 消'],
            yes: function (idx) {
                var uid = $('#txt_loginaccount').val().trim(),
                    upwd = $('#txt_loginpwd').val().trim();
                if (uid == '') {
                    layer.msg('请输入登录帐号');
                    return;
                }
                if (upwd == '') {
                    layer.msg('请输入登录密码');
                    return;
                }
                myPost('pub/LoginHandler.ashx?act=dologin', { 'loginid': uid, 'loginpwd': upwd }, function (res) {
                    layer.close(idx);
                    from_page && $('[data-action="sui-home"]').click();
                });
            },
            btn2: function (idx) {
                window.location.href = baseUrl + 'login.aspx';
            },
            success: function () {
                $('#txt_loginaccount').val(sui.getCookie('YJZFBZLOGINACCOUNT') || '');
                $('#txt_loginpwd').focus();
            },
            cancel: function () {
                window.location.href = baseUrl + 'login.aspx';
            }
        });
    }

    return {
        initForm: initForm,
        doEdit: doEdit,
        goBack: goBack,
        saveBack: saveBack,
        selectUser: selectUser,
        htmlEncode: htmlEncode,
        getCookie: getCookie,
        login: login,
        setTabStyle: setTabStyle
    };
}(jQuery, window, document));