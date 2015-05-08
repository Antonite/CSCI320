function hideOtherPanels (panel_id)
{
    $(".panel_lol").addClass("hide");
    $(panel_id).removeClass("hide");
}

/*
$("#champion_tab").on("click", hideOtherPanels("#champion_panel"));
$("#history_tab").on("click", hideOtherPanels("#history_panel"));
$("#rune_tab").on("click", hideOtherPanels("#rune_panel"));
$("#mastery_tab").on("click", hideOtherPanels("#mastery_panel"));
*/