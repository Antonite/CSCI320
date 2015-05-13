var iPath1 = document.getElementById("item1Path").value;
var iPath2 = document.getElementById("item2Path").value;
var iPath3 = document.getElementById("item3Path").value;
var iPath4 = document.getElementById("item4Path").value;
var iPath5 = document.getElementById("item5Path").value;
var iPath6 = document.getElementById("item6Path").value;
var iPath7 = document.getElementById("item7Path").value;
var cPathAs = document.getElementById("championAsPath").value;
var cPathVs = document.getElementById("championVsPath").value;

$("#itemImage1").attr("src", iPath1);
$("#itemImage2").attr("src", iPath2);
$("#itemImage3").attr("src", iPath3);
$("#itemImage4").attr("src", iPath4);
$("#itemImage5").attr("src", iPath5);
$("#itemImage6").attr("src", iPath6);
$("#itemImage7").attr("src", iPath7);
$("#champAsImage").attr("src", cPathAs);
$("#champVsImage").attr("src", cPathVs);

function hideOtherPanels(panel_id)
{
    $(".panel_lol").each(function () {
        $(this).addClass("hide");
    });
    $("#matchup_panel").removeClass("hide");
    $(panel_id).removeClass("hide");
}

$(document).ready(function () {
    // Hide all panels but the summary panel.
    hideOtherPanels("#summary_panel");

    // Add a listener to every mastery tile that sets the mastery info to its description.
    $(".mastery-tile").each(function () {
        var buffText = $(this).children(".mastery-desc").first().text();

        if ($(this).children(".mastery-level").text().startsWith("0"))
            $(this).css("opacity", ".2");

        $(this).on({
            mouseenter: function () {
                $("#mastery_buff").text(buffText);
            },
            mouseleave: function () {
                $("#mastery_buff").text("Hover over a mastery icon to see its effect.");
            },
        })
    });

    // Fix the winrate text color to vary with percent and to format out NaN%
    $(".winrate").each(function () {
        var contents = $(this).text();

        if (contents == "None played")
        {
            $(this).css("color", "#aaa");
        }
        else 
        {
            var percent = contents.substring(0, contents.length - 1);

            $(this).css("color", jQuery.Color("#444").transition(jQuery.Color("#3949ab"), percent / 100));
        }
    });
});