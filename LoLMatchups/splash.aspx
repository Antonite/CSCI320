<%@ Page Language="C#" AutoEventWireup="true" CodeFile="splash.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/color/jquery.color-2.1.2.min.js" type="text/javascript"></script>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700' rel='stylesheet' type="text/css"/>
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>LoLMatchups</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="upper upper-splash">
        <div id="navbar" class="navbar navbar-lol navbar-splash" style="z-index: 1">
            <a class="navbar-brand" href="#">LoLMatchups</a>
            <div class="navbar-form input-group">
                    <asp:TextBox ID="summoner_box" runat="server" placeholder="Enter a summoner's name" class="form-control text-nav-lol" />
                    <asp:Button ID="findSummoner" runat="server" OnClick="submit" Text="&#xf002;" class="btn btn-nav-lol fa-btn input-group-addon" />
                </div>
        </div>

        <%-- Literal magic being undergone here --%>

        <div class="video-container">
            <video class="bgvid" autoplay="autoplay" muted="muted" preload="auto" loop>
                <source src="http://a.pomf.se/wcfxcn.webm" type="video/webm">
            </video>
        </div>

        <div id="container">
            <div class="row center">
                <div class="col-md-1"></div>
               
                <div class="col-md-10 panel splash-panel">
                    <div class="video-container blur">
                        <video class="bgvid" autoplay="false" muted="muted" preload="auto" loop>
                            <source src="http://a.pomf.se/wcfxcn.webm" type="video/webm">
                        </video>
                    </div>

                    <h1 class="fullwidth splash-text">Why LoLMatchups?</h1>
                    <div class="row">
                        <div class="col-md-6">
                            <i class="fa fa-user fa-splash"></i>
                        </div>
                        <div class="col-md-6">
                            <i class="fa fa-pie-chart fa-splash"></i>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 splash-subhead">
                            Personalised.
                        </div>
                        <div class="col-md-6 splash-subhead">
                            Comprehensive.
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="splash-body">
                                Retrieve summoner-specific performance results for every champion matchup.
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="splash-body">
                                View detailed breakdowns of optimal items, runes, and masteries per matchup.
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <i class="fa fa-eye fa-splash"></i>
                        </div>
                        <div class="col-md-6">
                            <i class="fa fa-server fa-splash"></i>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 splash-subhead">
                            Insightful.
                        </div>
                        <div class="col-md-6 splash-subhead">
                            Riot API.
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="splash-body">
                                Focus on big-picture data that other sites don't present.
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="splash-body">
                                Automatically retrieve new match result data directly from the source.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
