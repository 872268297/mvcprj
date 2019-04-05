
function getStyle(obj, styleName) {
    if (obj.currentStyle) { return obj.currentStyle[styleName]; } else {
        return getComputedStyle(obj, null)[styleName];
    }
}

function randomNum(minNum, maxNum) {
    switch (arguments.length) {
        case 1:
            return parseInt(Math.random() * minNum + 1, 10);
            break;
        case 2:
            return parseInt(Math.random() * (maxNum - minNum + 1) + minNum, 10);
            break;
        default:
            return 0;
            break;
    }
}

function getNextExp(val) {
    return parseInt(val) * 100;
}

function UpadteUserAsset() {
    //更新用户信息
    $.post('/api/User/GetUserAsset', {}, function (res) {
        if (res.success) {
            asset = res.data;
            SetUserAsset(asset);
        }
    });
}

function compleTask(id) {
    $.post('/api/User/CompleteTask', { id: id }, function (res) {
        if (res.success) {
            GetDailyTask();
        } else {
            layer.msg(res.message);
        }
    });
}

function GetDailyTask() {
    //获取每日任务
    $.post('/api/User/GetDailyTask', {}, function (res) {
        var data = res.data;
        var str = '';
        $.each(data, function (i, v) {
            if (v.currentCount >= v.totalCount) {
                if (v.isComplete) {
                    str += '<div class="div_task_list_item"><span class="div_task_list_item_progress_received">';
                    str += '已完成</span>';
                } else {
                    str += '<div class="div_task_list_item"><span class="div_task_list_item_progress_complete" onclick="compleTask(' + v.id + ')">';
                    str += '领取</span>';
                }
            } else {
                str += '<div class="div_task_list_item"><span class="div_task_list_item_progress">';
                str += v.currentCount + '/' + v.totalCount + '</span>';
            }
            str += '<span class="div_task_list_item_discribe">';
            str += v.discribe + '</span><span class="div_task_list_item_exp">(+' + v.exp + 'EXP)</span> </div>';
        });
        $('.div_task_list').empty().append($(str));
    });
}

function SetUserAsset(asset) {
    $('#a_user,.a_user').text(decode(user));
    var headIcon = asset.headIcon;
    $('.head_right_top').each(function (i, o) {
        $(this).attr('src', headIcon);
    });
    var sex = asset.sex;
    if (sex == "男") {
        $('._sex').removeClass("personal_female").addClass("personal_male");
    } else {
        $('._sex').removeClass("personal_male").addClass("personal_female");
    }
    if (asset.sign) {
        $('._sign').empty().text(asset.sign);
    } else {
        $('._sign').empty().append($('<a class="_a_edit_sign" href="/Personal/EditInfo" >点击编辑个性签名</a>'));
    }
    var exp = parseInt(asset.exp);
    var level = asset.level;
    var nextExp = getNextExp(level);
    $('.div_level_bar_containter span').text(exp + '/' + nextExp);
    $('.div_level_bar_cur_level').text('LV' + level);
    $('.div_level_bar_nex_level').text('LV' + (parseInt(level) + 1));
    $('.div_level_bar_scroll').css('width', (exp / nextExp) * 100 + '%');
    $('.span_gold').html('金币&nbsp;&nbsp;' + parseInt(asset.gold));
    $('.span_siliver').html('银币&nbsp;&nbsp;' + parseInt(asset.silver));

}

function ShowAfterLogin() {
    $('#div_login_panel').hide();
    SetUserAsset(asset);
    $('#div_login_ed_panel').show();
    $("#a_logout").show().off('click').on('click', function () {
        $.post('../api/User/LogOut', {}, function (res) {
            if (res.success) {
                $('#div_login_panel').show();
                $('#div_login_ed_panel').hide();
                user = '';
                asset = null;
            } else {
                layer.msg(res.message);
            }
        });
    });
}

function ShowLoginDlg() {
    $('#txtLoginUser').val('');
    $('#txtLoginPass').val('');
    $('#validCode').val('');
    $('#divReg').hide();
    $('#divLogin').show();
    $('#imgCode').click();
    layer.open({
        type: 1,
        title: '登录',
        content: $('#divLoginAndReg'),
        area: ['420px', '260px']

    });
}

function ShowRegDlg() {
    $('#txtRegUser').val('');
    $('#txtRegPass').val('');
    $('#txtRegPass2').val('');
    $('#divReg').show();
    $('#divLogin').hide();
    layer.open({
        type: 1,
        title: '注册账号',
        content: $('#divLoginAndReg'),
        area: ['420px', '255px']

    });
}