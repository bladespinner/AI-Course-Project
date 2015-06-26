function getRecomendations()
{
    var text = $('.text-input').val();
    var url = "/api/wordprediction";
    
    $.get(url,{
        text: encodeURI(text)
    },
    function (data) {
        if (data[0]){
            $("#word1").html(data[0])
            $("#word1").prop("disabled", false);
        }
        else {
            $("#word1").html("</br>")
            $("#word1").prop("disabled", true);
        }
        if (data[1]) {
            $("#word2").html(data[1])
            $("#word2").prop("disabled", false);
        }
        else {
            $("#word2").html("</br>")
            $("#word2").prop("disabled", true);
        }
        if (data[2]) {
            $("#word3").html(data[2])
            $("#word3").prop("disabled", false);
        }
        else {
            $("#word3").html("</br>")
            $("#word3").prop("disabled", true);
        }
    });
}
function guessLetter() {
    var imgData = canvas.toDataURL();

    $.post("api/imagerecognition", {
        text: imgData,
    }, function (e) {
        
        var chA = "a".charCodeAt(0);
        var letters = e.map(function (a,i) {
            return { c: String.fromCharCode(chA + i), p: a }
        });
        var sorted = letters.sort(function (a, b) {
            return b.p - a.p;
        });
        console.log(sorted);
        var val = $('.text-input').val() + sorted[0].c;
        $('.text-input').val(val);
    });
};
var ts;

var canvas, ctx, flag = false,
    prevX = 0,
    currX = 0,
    prevY = 0,
    currY = 0,
    dot_flag = false;

var x = "black",
    y = 18;

function init() {
    canvas = document.getElementById('can');
    ctx = canvas.getContext("2d");
    w = canvas.width;
    h = canvas.height;

    canvas.addEventListener("mousemove", function (e) {
        findxy('move', e);
        if (flag) {
            var d = new Date();
            ts = d.getTime();
        }
    }, false);
    canvas.addEventListener("mousedown", function (e) {
        findxy('down', e)
    }, false);
    canvas.addEventListener("mouseup", function (e) {
        findxy('up', e)
        setTimeout(function () {
            var d = new Date();
            console.log(d.getTime());
            console.log(ts);
            if (d.getTime() < ts + 800) {
                return;
            }
            guessLetter();
            erase();
        }, 1000);
    }, false);
    canvas.addEventListener("mouseout", function (e) {
        findxy('out', e)
    }, false);
}

function color(obj) {
    switch (obj.id) {
        case "green":
            x = "green";
            break;
        case "blue":
            x = "blue";
            break;
        case "red":
            x = "red";
            break;
        case "yellow":
            x = "yellow";
            break;
        case "orange":
            x = "orange";
            break;
        case "black":
            x = "black";
            break;
        case "white":
            x = "white";
            break;
    }
    if (x == "white") y = 14;
    else y = 18;

}

function draw() {
    ctx.beginPath();
    ctx.moveTo(prevX, prevY);
    ctx.lineTo(currX, currY);
    ctx.strokeStyle = x;
    ctx.lineWidth = y;
    ctx.stroke();
    ctx.closePath();
}

function erase() {
    ctx.clearRect(0, 0, w, h);
}

function save() {
    document.getElementById("canvasimg").style.border = "2px solid";
    var dataURL = canvas.toDataURL();
    document.getElementById("canvasimg").src = dataURL;
    document.getElementById("canvasimg").style.display = "inline";
}

function findxy(res, e) {
    if (res == 'down') {
        prevX = currX;
        prevY = currY;
        currX = e.clientX - canvas.offsetLeft;
        currY = e.clientY - canvas.offsetTop;

        flag = true;
        dot_flag = true;
        if (dot_flag) {
            ctx.beginPath();
            ctx.lineCap = 'round';
            ctx.fillStyle = x;
            ctx.fillRect(currX, currY, 2, 2);
            ctx.closePath();
            dot_flag = false;
        }
    }
    if (res == 'up' || res == "out") {
        flag = false;
    }
    if (res == 'move') {
        if (flag) {
            prevX = currX;
            prevY = currY;
            currX = e.clientX - canvas.offsetLeft;
            currY = e.clientY - canvas.offsetTop;
            draw();
        }
    }
}

$(document).ready(function () {
    getRecomendations();
    $('.text-input').keypress(function (e) {
        if(e.key == " ")
        {
            var text = $('.text-input').val();
            getRecomendations();
        }
    })
    var f = function (a) {
        var str = $('.text-input').val()
        str += $(this).html() + " ";
        $('.text-input').val(str);
        getRecomendations();
    }
    $('.send').click(function () {
        var txt = $('.text-input').val();
        $.post("api/wordprediction", {
            text : txt,
        }, function (e) {
        });
        $('.text-container').append("<div class='message'>"
            + "<h6>Me:</h6>"
            + "<p>"
            + txt
            + "</p>"
            + "</div>");
        $('.text-input').val("");
    });
    $("#word1").click(f);
    $("#word2").click(f);
    $("#word3").click(f);

    init();
});