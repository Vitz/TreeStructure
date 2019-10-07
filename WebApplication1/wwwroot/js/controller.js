var ASCENDING = 0
var DESCENDING = 1

class TreeGenerator {
    constructor(rootID) {      
        
        var treeId = getTreeId()
        var urlTreesAsJson = getUrlTreesAsJson()
       
        $.get(urlTreesAsJson, {}, function (data, response) {
            $.each(data, function (index, value) {

                if (value.parent == null) {
                    var row = prepareRow(value)
                    document.getElementById(getTreeRootId()).appendChild(row)
                }
                else {
                    var parent = document.getElementById((value.parent).toString() + "_node");
                    var row = prepareRow(value)
                    parent.appendChild(row)
                }

                var ulNode = document.createElement("UL");
                ulNode.id = (value.id).toString() + "_node"
                row.appendChild(ulNode)
            })
            fillOptionsWithParents(getTreeId());
            deletIconsWhenNoChildren();
            setActionsToIcons();
            setRedirectionId();
        }).fail(function () {
            var fail = document.createElement("div");
            $(fail).addClass("alert alert-warning");
            $(fail).append("There is no items in this view.");
            $("#alerts").append($(fail))
             
        });
    };
        
}

function getTreeId() {
    var id = document.getElementById("tree_id").innerText;
    return id;
};

function getTreeRootId() {
    return "tree"
};

function getUrlTreesAsJson() {
    return '/api/treeapi/' + this.getTreeId();
};

$(document).ready(function () {
    th = new TreeGenerator()
});
