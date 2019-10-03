


function setActionRollIcons() {
    $(document).ready(function () {
        $("div[title='roll_icon']").click(function (e) {
            var branch = $(this).parent().parent().parent().children()[1];
            branch.toggleAttribute("hidden");
            $(this).children().first().toggleClass("fa-plus")
            $(this).children().first().toggleClass("fa-minus")
        });
    });
}

function setActionSortDesIcons() {
    $(document).ready(function () {
        $("div[title='arrow_down_icon']").click(function (e) {
            var branch = $(this).parent().parent().parent().children()[1];
            sortBranch(branch, DESCENDING)
        });
    });
}

function setActionSortAscIcons() {
    $(document).ready(function () {
        $("div[title='arrow_up_icon']").click(function (e) {
            var branch = $(this).parent().parent().parent().children()[1];
            sortBranch(branch, ASCENDING)
        });
    });
}

function setActionMoveIcons() {
    $(document).ready(function () {
        $("div[title='edit_icon']").click(function (e) {
            var id = this.parentElement.parentElement.id;
            document.getElementById("selected_id").value = id
            $.getJSON("/api/treeitemapi/item/" + id, {}, function (data, response) {
                var id = data.id
                var value = data.value
                var parent = data.parent
                document.getElementById("selected_id").value = id
                document.getElementById("selected_value").value = value
                $("option[value=" + parent + "]").attr("selected", "selected")
                $("form[id=edit_form]").attr("action", "/treeitems/move/" + id.toString())
            })

        });
    });
}

function setActionAddIcons() {
    $(document).ready(function () {
        $("div[title='add_icon']").click(function (_, item) {
            var id = this.parentElement.parentElement.id;
            $("option[value=" + id + "]").attr("selected", "selected")
        })
    });
}

function setActionDeleteIcons() {
    $(document).ready(function () {
        $("form[title='times_icon']").each(function (e) {
            var id = this.parentElement.parentElement.id;
            $(this).attr("action", "/TreeItems/DeleteWithChildren/" + id)
            $(this).attr("method", "post")
        });
        $("form[title='times_icon']").click(function () {
            this.submit();
        });
    });
}


function sortSingleUl(branch, sortType) {
    $(branch).each(function (_, ul) {
        var $toSort = $(ul).children();
        var values = $toSort.get().map(function (li) {
            return li.getElementsByClassName("tree_value")[0];
        });
        values.sort(function (a, b) { return a.value > b.value });
        if (sortType == ASCENDING) values.reverse()
        values.forEach(function (val, index) {
            $($toSort[index]).parent().append(val.parentElement.parentElement.parentElement)

        });
    });
}

function sortBranch(branch, sortType) {
    sortSingleUl($(branch), sortType)
    var divs = $(branch).children()
    $(divs.children()).each(function (_, child) {
        $(child).each(function (_, item) {
            if (item.nodeName == "UL")
                sortBranch(item);
        });
    });
}
