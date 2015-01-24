function parseHistory(historyList) {
    var innerText = "Your attempts : ";
    for (var i = 0; i < historyList.length; i++) {
        innerText += historyList[i] + ", ";
    }

    return  innerText.slice(0, innerText.length - 2);
}

function parseData(data) {
    var $status = $("#status>p");
    var $helper = $("#helper");
    var $history = $("#history>p");

    if (Object.keys(data).length == 3) {
        $helper.text(data.Message);
        $status.text(data.Status);
        $history.text(parseHistory(data.History));
    }
    else if (Object.keys(data).length == 2) {
        $helper.text(data.Message);
        $status.text(data.Status);
    }
    else if (Object.keys(data).length == 1) {
        $status.text(data.Status);
    }
}

function Ping() {
    var $status = $("#status>p");

    $.ajax({
        url:"/Home/PingStatus",
        method: "GET",
        success: function (data) {
            if (data.Winner != null) {
                $status.text("Winner is " + data.Winner + ". Thinked number is " + data.ThinkedNumber);
            } else {
                $status.text(data.Status);
            }

            setTimeout("Ping()", 1000);
        },
        error: function () {
            $status.text("Some error");
        }
    });
}

function guessNumber() {
    var $guessValidation = $("#guess>b");
    var $guessNumber = parseInt($("#guess-box").val().trim());
    var $status = $("#status>p");

    if ($guessNumber < 0 || isNaN($guessNumber)) {
        $guessValidation.show();
        return;
    }

    $guessValidation.hide();
    $.ajax({
        url: "/Home/GuessNumber?number=" + $guessNumber + "&userName=" + sessionStorage.getItem("UserName"),
        method: "GET",
        success: function(data) {
            parseData(data);
        },
        error: function () {
            $status.text("Some error");
        }
    });
}

function thinkupNumber() {
    var $thinkValidation = $("#thinkup>b");
    var $status = $("#status>p");
    var $thinkNumber = parseInt($("#thinkup-box").val().trim());

    if ($thinkNumber < 0 || isNaN($thinkNumber)) {
        $thinkValidation.show();
        return;
    }

    $thinkValidation.hide();
    $.ajax({
        url: "/Home/ThinkupNumber?number=" + $thinkNumber + "&userName=" + sessionStorage.getItem("UserName"),
        method: "GET",
        success: function(data) {
            $status.text(data.Status);
        },
        error: function() {
            $status.text("Some error");
        }
    });
}

function login() {
    var $userName = $("#userName").val().trim();
    var $loginValidation = $("#login>b");

    if ($userName.length < 1) {
        $loginValidation.show();
        return;
    }

    $loginValidation.hide();
    sessionStorage.setItem("UserName", $userName);

    document.location.reload();
}

function logout() {
    sessionStorage.removeItem("UserName");
    document.location.reload();
}

$(document).ready(function () {
    var $loginBtn = $("#login-btn");
    var $logoutBtn = $("#logout-btn");

    var $helloUser = $("#helloUser");

    var $loginDiv = $("div#login");
    var $logoutDiv = $("div#logout");

    var $gameField = $(".jumbotron");

    var $thinkupBtn = $("#thinkup-btn");
    var $guessBtn = $("#guess-btn");

    var $thinkValidation = $("#thinkup>b");
    var $guessValidation = $("#guess>b");

    $thinkValidation.hide();
    $guessValidation.hide();

    $loginBtn.click(function () {
        login();
    });
    $logoutBtn.click(function () {
        logout();
    });
    $("#login>b").hide();

    $thinkupBtn.click(function() {
        thinkupNumber();
    });
    $guessBtn.click(function() {
        guessNumber();
    });



    var userName = sessionStorage.getItem("UserName");
    if (userName != null) {
        $loginDiv.hide();
        $logoutDiv.show();

        $gameField.show();

        $helloUser.text("Hi, " + userName + "!");

        Ping();
    } else {
        $loginDiv.show();
        $logoutDiv.hide();

        $gameField.hide();

        $helloUser.text("");
    }
});