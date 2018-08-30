/** 
 * combobox和combotree模糊查询 
 */
(function () {
    //combobox可编辑，自定义模糊查询  
    $.fn.combobox.defaults.editable = true;
    $.fn.combobox.defaults.filter = function (q, row) {
        var opts = $(this).combobox('options');
        return row[opts.textField].indexOf(q) >= 0;
    };
    //combotree可编辑，自定义模糊查询（筛选的数据是暂时是已逗号隔开---"kkk,hhhh"，后面扩展正则表达式符号）
    $.fn.combotree.defaults.editable = true;
    $.extend($.fn.combotree.defaults.keyHandler, {
        up: function () {
            console.log('up');
        },
        down: function () {
            console.log('down');
        },
        enter: function () {
            console.log('enter');
        },
        query: function (q) {
            var t = $(this).combotree('tree');
            var nodes = t.tree('getChildren');
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                var searchText = q.substring(q.lastIndexOf(',') + 1).trim().toLowerCase();
                var nodeText = node.text.trim().toLowerCase();
                //方案一
                //if (nodeText.indexOf(searchText) >= 0) {
                //    $(node.target).show();
                //    t.tree('expandTo', node.target);
                //} else {
                //    $(node.target).hide();
                //}
                //方案二
                if (searchText != "" && nodeText.indexOf(searchText) >= 0) {
                    t.tree('expandTo', node.target);
                    t.tree('select', node.target);
                }
            }
            var opts = $(this).combotree('options');
            if (!opts.hasSetEvents) {
                opts.hasSetEvents = true;
                var onShowPanel = opts.onShowPanel;
                opts.onShowPanel = function () {
                    var nodes = t.tree('getChildren');
                    for (var i = 0; i < nodes.length; i++) {
                        $(nodes[i].target).show();
                    }
                    onShowPanel.call(this);
                };
                $(this).combo('options').onShowPanel = opts.onShowPanel;
            }
        }
    });
})(jQuery);