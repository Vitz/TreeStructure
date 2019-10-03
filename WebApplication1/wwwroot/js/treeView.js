
// fill select with options (all items available in three)
function fillOptionsWithParents(tree_id) {
    var select_edit = $('#select_parent_edit')
    var select_add = $('#select_parent_add')
    var url = '/api/treeitemapi/' + tree_id;
    $.get(url, {}, function (data, response) {
        $(data).each
            (function (_, item) {
                select_edit.append(new Option(item.value, item.id));
                select_add.append(new Option(item.value, item.id));
            });
    });
}

// make leaf insted of branch
function deletIconsWhenNoChildren() {
    $("ul").each(function (index) {
        if (this.firstChild == null) {
            var rollIcon = this.parentElement.getElementsByClassName("fa")[0]
            rollIcon.hidden = true;
            var sortIcon = this.parentElement.getElementsByClassName("fa")[1]
            sortIcon.hidden = true;
            var sortIcon = this.parentElement.getElementsByClassName("fa")[2]
            sortIcon.hidden = true;
            $(this).parent().addClass("leaf")
        }
    });
}



