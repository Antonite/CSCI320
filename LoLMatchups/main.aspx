<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            if ($("#summoner_box").val() != "" && $("#sum_name").text() != $("#summoner_box").val())
                $("#findSummoner").click();
        </script>
    <script src="http://code.jquery.com/color/jquery.color-2.1.2.min.js" type="text/javascript"></script>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700' rel='stylesheet' type="text/css">
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>LoLMatchups</title>
</head>
<body>
    <script type="text/javascript">
        $(document).ready(function () {
            $.getScript("main.js");
        });
    </script>

    <form id="form1" runat="server">
        <div class="upper">
            <div id="navbar" class="navbar navbar-lol">
                <a class="navbar-brand" href="#">LoLMatchups</a>
                <div class="navbar-form input-group">
                    <asp:TextBox ID="summoner_box" runat="server" placeholder="Enter a summoner's name" class="form-control text-nav-lol" />
                    <asp:Button ID="findSummoner" runat="server" OnClick="submit" Text="&#xf002;" class="btn btn-nav-lol fa-btn input-group-addon" />
                </div>
            </div>

            <div id="container">
                <div id="status-msg" class="center">
                    <asp:Label ID="summoner_id" runat="server" Text="Label" class="alert alert-warning"></asp:Label>
                    <asp:Label ID="status" runat="server" Text="Label" class="alert alert-danger"></asp:Label>
                </div>

                <div id="summoner_panel" runat="server" class="panel center">
                    <table class="fullwidth fullheight">
                        <tr>
                            <td class="valign-top">
                                <%--<div id="sum_avatar" runat="server" class="avatar"></div>--%>
                                <img id="sum_avatar" runat="server" src="ProfileImages/ProfileIcon01.jpg" class="avatar"/>
                                <div id="sum_name" runat="server" class="summoner-name">Summoner Name</div>
                                <div id="sum_rank" runat="server" class="summoner-rank">Rank</div>
                                <div id="sum_winrate" runat="server" class="summoner-winrate">W / L</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="valign-bottom">
                                <div id="summoner_navbar" class="navbar navbar-default summoner-navbar-lol">
                                    <ul class="nav navbar-nav">
                                        <li><a id="summary_tab" runat="server" href="#" onclick="hideOtherPanels('#summary_panel')">Summary</a></li>
                                        <li><a id="champion_tab" runat="server" href="#" onclick="hideOtherPanels('#champion_panel')">Champions</a></li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="champ_matchup" class="fullwidth center">
                    <div class="row form-inline">
                        <div class="col-md-3"></div>
                        <div class="col-md-3 center">
                            <asp:TextBox ID="champ_as" runat="server" placeholder="Your champion" class="form-control text-lol" />
                        </div>
                        <div class="col-md-3 center">
                            <asp:TextBox ID="champ_vs" runat="server" placeholder="Opponent champion" class="form-control text-lol" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="champ_button" runat="server" OnClick="getMatchups" Text="Get Matchup" class="btn btn-default btn-lol" />
                        </div>
                    </div>
                </div>

                <div id="matchup_panel" runat="server" class="fullwidth panel panel_lol">
                    <div id="matchupStatsPanel">
                        <asp:Label ID="matchupStats" runat="server" Text="matchupStats"></asp:Label>

                        <input type="hidden" runat='server' id="item1Path" value="" />
                        <input type="hidden" runat='server' id="item2Path" value="" />
                        <input type="hidden" runat='server' id="item3Path" value="" />
                        <input type="hidden" runat='server' id="item4Path" value="" />
                        <input type="hidden" runat='server' id="item5Path" value="" />
                        <input type="hidden" runat='server' id="item6Path" value="" />
                        <input type="hidden" runat='server' id="item7Path" value="" />
                        <input type="hidden" runat='server' id="championAsPath" value="" />
                        <input type="hidden" runat='server' id="championVsPath" value="" />

                        <div class="row">
                            <div id="champ_as_tile" runat="server" class="col-md-2 panel champion_tile champion_tile_first">
                                <table class="fullwidth center">
                                    <tr>
                                        <td>
                                            <img src="http://placekitten.com/g/150/150" class="champion_portrait center" id="champAsImage" />
                                            <div class="champion_name">
                                                <asp:Label ID="ChampAsName" runat="server" Text="ChampAsName"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="champion_as_won won">
                                                <div class="champion_as_won won">
                                                    <asp:Label ID="winPercentAsLeft" runat="server" class="winrate" Text="winPercentAsLeft"></asp:Label></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="champion_vs_won won">
                                                <div class="champion_vs_won won">
                                                    <asp:Label ID="winPercentVsLeft" runat="server" class="winrate" Text="winPercentVsLeft"></asp:Label></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div id="matchup_info" class="col-md-8 panel">
                                <table class="fullwidth fullheight">
                                    <tr>
                                        <td>
                                <div class="row center">
                                    <div class="col-md-3 matchup-header">Matchup winrate</div>
                                    <div class="col-md-3 matchup-header">Items</div>
                                    <div class="col-md-3 matchup-header">Runes</div>
                                    <div class="col-md-3 matchup-header">Masteries</div>
                                </div>

                                            <div class="row center">
                                                <div class="col-md-3">
                                                    <asp:Label ID="matchupPercent" class="winrate winrate-lg" runat="server" Text="matchupPercent"></asp:Label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="matchupTopItemsPercent" class="winrate winrate-lg" runat="server" Text="matchupTopItemsPercent"></asp:Label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="matchupTopRunesPercent" class="winrate winrate-lg" runat="server" Text="matchupTopRunesPercent"></asp:Label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="matchupTopMasteriesPercent" class="winrate winrate-lg" runat="server" Text="matchupTopMasteriesPercent"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row center">
                                                <div class="col-md-3"></div>
                                                <div class="col-md-3"></div>
                                                <div class="col-md-3"></div>
                                                <div class="col-md-3">
                                                    <div id="topMasteriesDiv" runat="server">
                                                        <div><asp:Label ID="sampleMasteriesAggregate" runat="server" Text="sampleMasteriesAggregate"></asp:Label></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="valign-bottom">
                                        <td>
                                            <div class="row center">
                                                <div class="col-xs-1 col-xs-lol thumb" id="itemImg1">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage1">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage2">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage3">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage4">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage5">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage6">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb">
                                                    <a class="thumbnail" href="#">
                                                        <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage7">
                                                    </a>
                                                </div>
                                                <div class="col-xs-1 col-xs-lol thumb"></div>
                                                <div id="topRunesDiv" class="col-xs-1 col-xs-lol" runat="server">
                                                    <%-- <a href="#">Rune page</a> --%>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div id="champ_vs_tile" runat="server" class="col-md-2 panel champion_tile champion_tile_last">
                                <table class="fullwidth center">
                                    <tr>
                                        <td>
                                            <img src="http://placekitten.com/g/150/150" class="champion_portrait center" id="champVsImage" />
                                            <div class="champion_name">
                                                <asp:Label ID="ChampVsName" runat="server" Text="ChampVsName"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="champion_as_won won">
                                                <asp:Label ID="winPercentAsRight" class="winrate" runat="server" Text="winPercentAsRight"></asp:Label></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="champion_vs_won won">
                                                <asp:Label ID="winPercentVsRight" class="winrate" runat="server" Text="winPercentVsRight"></asp:Label></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                
                <%-- 
                <div class="winrate">0%</div>
                <div class="winrate">10%</div>
                <div class="winrate">20%</div>
                <div class="winrate">30%</div>
                <div class="winrate">50%</div>
                <div class="winrate">60%</div>
                <div class="winrate">70%</div>
                <div class="winrate">80%</div>
                <div class="winrate">90%</div>
                <div class="winrate">100%</div>--%>

                <%-- <div id="summary_panel" runat="server" class="fullwidth panel panel_lol">Player Summary</div> --%>

                <div id="champion_panel" runat="server" class="fullwidth panel panel_lol">
                    <div class="row">
                        <%--You can fit 6 champion_tile divs on one line. However, the overflow should occur automatically so you can 
                        just drop all of the divs into this row div. For the 1st and 6th tiles in every row, you need to subclass
                        it with champion_tile_first or champion_tile_last so that the margins don't get messed up.
                        
                        Note that since champion_panel has a set height, weird stuff will probably happen if you have more than 2 rows of tiles.--%>

                        <div id="example_champ" runat="server" class="col-md-2 panel champion_tile champion_tile_first">
                            <table class="fullwidth center">
                                <tr>
                                    <td>
                                        <img src="http://placekitten.com/g/150/150" class="champion_portrait center"></img>
                                        <div class="champion_name">Champ Name</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="champion_as_won won">W</div>
                                        <div class="champion_as_total lost">T</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="champion_vs_won won">W</div>
                                        <div class="champion_vs_total lost">T</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" class="last_match">Last match</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div id="rune_panel" runat="server" class="fullwidth panel panel_lol">
                    <div class="row">
                        <div class="col-md-12 panel rune-tile">
                            <table id="runeTable" class="table table-hover fullwidth" runat="server">
                                <tr>
                                    <th>Rune effect</th>
                                    <th>Aggregate buff</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div id="mastery_panel" runat="server" class="fullwidth panel panel_lol">
                    <div class="row">
                        <div class="col-md-4 mastery-col">
                            <div class="panel mastery-group mastery-offense">
                            <div class="mastery-header">Offense</div>
                            <div class="row">
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4111.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence1" runat="server" Text="offence1"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc1" runat="server" Text="offenceDesc1"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4112.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence2" runat="server" Text="offence2"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc2" runat="server" Text="offenceDesc2"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4113.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence3" runat="server" Text="offence3"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc3" runat="server" Text="offenceDesc3"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4114.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence4" runat="server" Text="offence4"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc4" runat="server" Text="offenceDesc4"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4121.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence5" runat="server" Text="offence5"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc5" runat="server" Text="offenceDesc5"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4122.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence6" runat="server" Text="offence6"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc6" runat="server" Text="offenceDesc6"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4123.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence7" runat="server" Text="offence7"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc7" runat="server" Text="offenceDesc7"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4124.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence8" runat="server" Text="offence8"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc8" runat="server" Text="offenceDesc8"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4131.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence9" runat="server" Text="offence9"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc9" runat="server" Text="offenceDesc9"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4132.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence10" runat="server" Text="offence10"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc10" runat="server" Text="offenceDesc10"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4133.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence11" runat="server" Text="offence11"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc11" runat="server" Text="offenceDesc11"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4134.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence12" runat="server" Text="offence12"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc12" runat="server" Text="offenceDesc12"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4141.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence13" runat="server" Text="offence13"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc13" runat="server" Text="offenceDesc13"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4142.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence14" runat="server" Text="offence14"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc14" runat="server" Text="offenceDesc14"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4143.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence15" runat="server" Text="offence15"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc15" runat="server" Text="offenceDesc15"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4144.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence16" runat="server" Text="offence16"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc16" runat="server" Text="offenceDesc16"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4151.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence17" runat="server" Text="offence17"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc17" runat="server" Text="offenceDesc17"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4152.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence18" runat="server" Text="offence18"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc18" runat="server" Text="offenceDesc18"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4154.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence19" runat="server" Text="offence19"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc19" runat="server" Text="offenceDesc19"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile">
                                    <img src="MasteryImages\4162.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="offence20" runat="server" Text="offence20"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="offenceDesc20" runat="server" Text="offenceDesc20"></asp:Label></div>
                                </div>
                            </div>
                            </div>
                        </div>
                        <div class="col-md-4 mastery-col">
                            <div class="panel mastery-group mastery-group-defense">
                            <div class="mastery-header">Defense</div>
                            <div class="row">
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4211.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence1" runat="server" Text="defence1"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc1" runat="server" Text="defenceDesc1"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4212.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence2" runat="server" Text="defence2"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc2" runat="server" Text="defenceDesc2"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4213.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence3" runat="server" Text="defence3"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc3" runat="server" Text="defenceDesc3"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4214.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence4" runat="server" Text="defence4"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc4" runat="server" Text="defenceDesc4"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4221.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence5" runat="server" Text="defence5"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc5" runat="server" Text="defenceDesc5"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4222.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence6" runat="server" Text="defence6"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc6" runat="server" Text="defenceDesc6"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4224.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence7" runat="server" Text="defence7"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc7" runat="server" Text="defenceDesc7"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4231.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence8" runat="server" Text="defence8"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc8" runat="server" Text="defenceDesc8"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4232.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence9" runat="server" Text="defence9"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc9" runat="server" Text="defenceDesc9"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4233.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence10" runat="server" Text="defence10"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc10" runat="server" Text="defenceDesc10"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4234.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence11" runat="server" Text="defence11"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc11" runat="server" Text="defenceDesc11"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4241.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence12" runat="server" Text="defence12"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc12" runat="server" Text="defenceDesc12"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4242.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence13" runat="server" Text="defence13"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc13" runat="server" Text="defenceDesc13"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4243.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence14" runat="server" Text="defence14"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc14" runat="server" Text="defenceDesc14"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4244.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence15" runat="server" Text="defence15"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc15" runat="server" Text="defenceDesc15"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4251.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence16" runat="server" Text="defence16"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc16" runat="server" Text="defenceDesc16"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4252.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence17" runat="server" Text="defence17"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc17" runat="server" Text="defenceDesc17"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4253.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence18" runat="server" Text="defence18"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc18" runat="server" Text="defenceDesc18"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4262.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="defence19" runat="server" Text="defence19"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="defenceDesc19" runat="server" Text="defenceDesc19"></asp:Label></div>
                                </div>
                            </div>
                            </div>
                        </div>
                        <div class="col-md-4 mastery-col">
                            <div class="panel mastery-group mastery-group-utility">
                            <div class="mastery-header">Utility</div>
                            <div class="row">
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4311.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility1" runat="server" Text="utility1"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc1" runat="server" Text="utilityDesc1"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4312.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility2" runat="server" Text="utility2"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc2" runat="server" Text="utilityDesc2"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4313.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility3" runat="server" Text="utility3"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc3" runat="server" Text="utilityDesc3"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4314.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility4" runat="server" Text="utility4"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc4" runat="server" Text="utilityDesc4"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4322.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility5" runat="server" Text="utility5"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc5" runat="server" Text="utilityDesc5"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4323.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility6" runat="server" Text="utility6"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc6" runat="server" Text="utilityDesc6"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4324.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility7" runat="server" Text="utility7"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc7" runat="server" Text="utilityDesc7"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4331.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility8" runat="server" Text="utility8"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc8" runat="server" Text="utilityDesc8"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4332.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility9" runat="server" Text="utility9"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc9" runat="server" Text="utilityDesc9"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4333.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility10" runat="server" Text="utility10"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc10" runat="server" Text="utilityDesc10"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4334.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility11" runat="server" Text="utility11"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc11" runat="server" Text="utilityDesc11"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4341.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility12" runat="server" Text="utility12"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc12" runat="server" Text="utilityDesc12"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4342.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility13" runat="server" Text="utility13"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc13" runat="server" Text="utilityDesc13"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4343.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility14" runat="server" Text="utility14"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc14" runat="server" Text="utilityDesc14"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4344.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility15" runat="server" Text="utility15"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc15" runat="server" Text="utilityDesc15"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4352.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility16" runat="server" Text="utility16"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc16" runat="server" Text="utilityDesc16"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4353.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility17" runat="server" Text="utility17"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc17" runat="server" Text="utilityDesc17"></asp:Label></div>
                                </div>
                                <div class="col-md-3 mastery-tile"">
                                    <img src="MasteryImages\4362.png" class="fullwidth fullheight"/>
                                    <div class="mastery-level"><asp:Label ID="utility18" runat="server" Text="utility18"></asp:Label></div>
                                    <div class="mastery-desc"><asp:Label ID="utilityDesc18" runat="server" Text="utilityDesc18"></asp:Label></div>
                                </div>
                            </div>
                            </div>
                        </div>
                        <div class="col-md-12 mastery-col">
                            <div id="mastery_buff" class="panel mastery-buff-panel">
                                Hover over a mastery icon to see its effect.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <%-- Don't delete these yet --%>

    <%--<div id="search_box">
                    <div class="row">
                        <div class="col-xs-3 input-group center">
                            <asp:TextBox ID="summoner_box" runat="server" placeholder="Enter a summoner's name" class="form-control text-lol" />
                            <div class="input-group-btn">
                                <asp:Button ID="findSummoner" runat="server" OnClick="submit" Text="&#xf002;" class="btn btn-lol fa-btn" />
                            </div>
                        </div>
                    </div>
                    </br>
            <div class="center">
                <asp:Label ID="summoner_id" runat="server" Text="Label" class="alert alert-warning"></asp:Label>
            </div>
                </div>--%>
</body>
</html>
