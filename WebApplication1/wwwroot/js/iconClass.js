
class Icon {
    constructor(rowData, divClassName, divTitle, iconIdSufix, additionalClasses, defaultTag = "div") {
        var divIcon = document.createElement(defaultTag);
        divIcon.className = divClassName;
        divIcon.title = divTitle;
        var icon = document.createElement("li");
        icon.id = rowData.id.toString() + iconIdSufix
        icon.classList = icon.classList + " fa " +  additionalClasses
        divIcon.appendChild(icon)
        return divIcon
    }
}