function GerPremierLeagueTable(league) {
    jQuery.support.cors = true;
    $.ajax({
        url: 'http://soccer.sportsopendata.net/v1/leagues/' + league+'/seasons/17-18/standings',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data);
        },
        error: function () {
            errorMessage();
        }
    });
}

function errorMessage() {
    $("#divResult").empty();
    var strResult = "<h3>Data not available</h3>";
    $("#divResult").html(strResult);
}

function WriteResponse(data) {
    $("#divResult").empty();
    var strResult = "<table class='table table-hover'><th>Position</th><th>Team</th><th>Wins</th><th>Draws</th><th>Losts</th><th>Matches Played</th><th>Goal Difference</th><th>Points</th>";
    $.each(data.data.standings, function (index, val) {
        strResult += "<tr><td>" + val.position + "</td><td> " + val.team + "</td><td>" + val.overall.wins + "</td><td>" + val.overall.draws + "</td><td>" + val.overall.losts + "</td><td>" + val.overall.matches_played + "</td><td>" + val.overall.goal_difference + "</td><td>" + val.overall.points + "</td></tr>";
    });
    strResult += "</table>";
    $("#divResult").html(strResult);
}