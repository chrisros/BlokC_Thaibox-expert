function menuOpen() {
    
    document.getElementById('dropMenu').style.display = 'block';
}

document.getElementById('dropMenu').onmouseout = function (e) {
    e = e || window.event;
    var target = e.srcElement || e.target;
    if (target.id == "dropMenu") {
        document.getElementById('dropMenu').style.display = 'none';
    }
};