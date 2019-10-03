var ASCENDING = 0
var DESCENDING = 1

class TreeGenerator {
    constructor(rootID) {      
        
        console.log("1")
        var treeId = getTreeId()
        var urlTreesAsJson = getUrlTreesAsJson()
       
        $.get(urlTreesAsJson, {}, function (data, response) {
            $.each(data, function (index, value) {

                console.log(value)
                if (value.parent == null) {
                    console.log("item with parent nu;;")
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
