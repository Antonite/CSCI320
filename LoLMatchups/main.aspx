<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
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
                                <div id="sum_avatar" runat="server" class="avatar">Avatar</div>
                                <div id="sum_name" runat="server" class="summoner-name">Summoner Name</div>
                                <div id="sum_rank" runat="server" class="summoner-rank">Rank</div>
                                <div id="sum_winrate" runat="server" class="summoner-winrate">W / L</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="valign-bottom">
                                <div id="summoner_navbar" class="navbar navbar-default summoner-navbar-lol">
                                    <ul class="nav navbar-nav">
                                        <li><a id="summary_tab" href="#" onclick="hideOtherPanels('#summoner_panel')">Summary</a></li>
                                        <li><a id="champion_tab" href="#" onclick="hideOtherPanels('#champion_panel')">Champions</a></li>
                                        <li><a id="history_tab" href="#" onclick="hideOtherPanels('#history_panel')">Match History</a></li>
                                        <li><a id="rune_tab" href="#" onclick="hideOtherPanels('#rune_panel')">Runes</a></li>
                                        <li><a id="mastery_tab" href="#" onclick="hideOtherPanels('#mastery_panel')">Masteries</a></li>
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


                    <div id="matchup_panel" runat="server" class="fullwidth panel panel_lol test">
                        <div ID="matchupStatsPanel">
                            <asp:Label ID="matchupStats" runat="server" Text="matchupStats"></asp:Label>
                            <table class="fullwidth center">
                                <tr>
                                    <td>
                                        <img src="http://placekitten.com/g/150/150" class="champion_portrait center" />
                                        <div class="champion_name"><asp:Label ID="ChampAsName" runat="server" Text="ChampAsName"></asp:Label></div>
                                    </td>
                                    <td>
                                        <div class="champion_name">VS</div>
                                    </td>
                                    <td>
                                        <img src="http://placekitten.com/g/150/150" class="champion_portrait center" />
                                        <div class="champion_name"><asp:Label ID="ChampVsName" runat="server" Text="ChampVSName"></asp:Label></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td><td><asp:Label ID="matchupPercent" runat="server" Text="matchupPercent"></asp:Label></td><td></td>
                                </tr>
                                <tr class="blank_row">
                                </tr>
                                <tr>
                                    <td>Items:</td>
                                        <td><asp:Label ID="matchupTopItems" runat="server" Text="matchupTopItems"></asp:Label>
                                            <div class="container hide" runat="server">
                                                    <div class="col-xs-1 thumb" id="itemImg1">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" id="itemImage1">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-1 thumb">
                                                        <a class="thumbnail" href="#">
                                                            <img class="img-responsive" src="http://placehold.it/50x50" alt="">
                                                        </a>
                                                    </div>
                                            </div>
                                      </td>
                                    <td><asp:Label ID="matchupTopItemsPercent" runat="server" Text="matchupTopItemsPercent"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Runes:</td>
                                    <td><asp:Label ID="matchupTopRunes" runat="server" Text="matchupTopRunes"></asp:Label></td>
                                    <td><asp:Label ID="matchupTopRunesPercent" runat="server" Text="matchupTopRunesPercent"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Masteries:</td>
                                    <td><asp:Label ID="matchupTopMasteries" runat="server" Text="matchupTopMasteries"></asp:Label></td>
                                    <td><asp:Label ID="matchupTopMasteriesPercent" runat="server" Text="matchupTopMasteriesPercent"></asp:Label></td>
                                </tr>
                            </table>
                            
                            

                        </div>
                    </div>
                </div>

               <div id="champion_panel" runat="server" class="fullwidth panel panel_lol">
                    <div class="row">
                    <%--You can fit 6 champion_tile divs on one line. However, the overflow should occur automatically so you can 
                        just drop all of the divs into this row div. For the 1st and 6th tiles in every row, you need to subclass
                        it with champion_tile_first or champion_tile_last so that the margins don't get messed up.
                        
                        Note that since champion_panel has a set height, weird stuff will probably happen if you have more than 2 rows of tiles.--%>

                    <%--<div id="example_champ" runat="server" class="col-md-2 panel champion_tile champion_tile_first">
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
                                    <a href="#" class=last_match>Last match</a>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    </div>
                </div>


                <div id="history_panel" runat="server" class="fullwidth panel panel_lol">Detailed Match History</div>
                <div id="rune_panel" runat="server" class="fullwidth panel panel_lol">Detailed Rune Info</div>
                <div id="mastery_panel" runat="server" class="fullwidth panel panel_lol">Detailed Mastery Info</div>
                
                </br>
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
