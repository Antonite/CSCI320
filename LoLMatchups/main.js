var iPath1 = document.getElementById("item1Path").value;
var iPath2 = document.getElementById("item2Path").value;
var iPath3 = document.getElementById("item3Path").value;
var iPath4 = document.getElementById("item4Path").value;
var iPath5 = document.getElementById("item5Path").value;
var iPath6 = document.getElementById("item6Path").value;
var iPath7 = document.getElementById("item7Path").value;

$("#itemImage1").attr("src", iPath1);
$("#itemImage2").attr("src", iPath2);
$("#itemImage3").attr("src", iPath3);
$("#itemImage4").attr("src", iPath4);
$("#itemImage5").attr("src", iPath5);
$("#itemImage6").attr("src", iPath6);
$("#itemImage7").attr("src", iPath7);

function hideOtherPanels(panel_id)
{
    $(".panel_lol").addClass("hide");
    $("#matchup_panel").removeClass("hide");
    $(panel_id).removeClass("hide");
}

$(document).ready(function () {
    $(".mastery-tile").each(function () {
        var buffText = $(this).children(".mastery-desc").first().text();

        $(this).on({
            mouseenter: function () {
                $("#mastery_buff").text(buffText);
            },
            mouseleave: function () {
                $("#mastery_buff").text("Hover over a mastery icon to see its effect.");
            },
        })
    });
});