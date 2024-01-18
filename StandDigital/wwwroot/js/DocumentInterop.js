function GetWidth() {
    return window.innerWidth;
}

var full = false;
function fill() {
    full = !full;
    document.getElementById("logocontainer").style.backgroundColor = full ? "#3ebffa" : "transparent";
}