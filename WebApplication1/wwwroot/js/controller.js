var ASCENDING = 0
var DESCENDING = 1

class TreeGenerator {
    constructor(rootID) {
        this.rootDiv = ""
        if (document.getElementById(rootID)) {
            this.rootDiv = rootDiv;
        } else {
            this.rootDiv = document.createElement("div")
            this.rootDiv.id = rootID
            document.getElementById("tree").appendChild(this.rootDiv)
        }

        var treeId = this.getTreeId()
        var urlTreesAsJson = this.geturlTreesAsJson()
        // for redirections after edit/add      
        $("input[name=rootId]").each(function (_, item) {
            this.value = treeId;
        })

        var rootDiv = this.rootDiv;
        $.get(urlTreesAsJson, {}, function (data, response) {
            $.each(data, function (index, value) {

                if (value.parent == null) {
                    var row = prepareRow(value)
                    rootDiv.appendChild(row)
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
            fillOptionsWithParents(treeId);
            deletIconsWhenNoChildren();
            setActionsToIcons();
        });
    };


    get getRootDiv()
    {
        return this.rootDiv;
    }

    getTreeId() {
        return document.getElementById("tree_id").innerText;
    };
    geturlTreesAsJson() {
        return '/api/treeitemapi/' + this.getTreeId();
    };
        
}
$(document).ready(function () {
    th = new TreeGenerator("tree1")
});
