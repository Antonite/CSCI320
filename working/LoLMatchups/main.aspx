<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <script src="//code.jquery.com/jquery-1.11.2.min.js"></script>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700' rel='stylesheet' type="text/css">
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>LoLMatchups</title>
</head>
<body>
    <div class="upper">
        <div id="navbar" class="navbar navbar-lol">
            <a class="navbar-brand" href="#">LoLMatchups</a>
        </div>

    <form id="form1" runat="server">

        <div id="summoner-panel" class="panel panel-lol center">
            <asp:Table ID="infoTable" runat="server" EnableTheming="False" class="table table-lol"></asp:Table>
        </div>

        <div id="search_box">
            <div class="row">
            <div class="col-xs-3 input-group center">
                <asp:TextBox id="summoner_box" runat="server" placeholder="Enter a summoner's name" class="form-control text-lol"/>
                <div class="input-group-btn">
                    <asp:Button ID="findSummoner" runat="server" OnClick="submit" Text="&#xf002;" class="btn btn-lol fa-btn"/>
                </div>
            </div>
            </div>
            </br>
            <div class="center">  
                <asp:Label ID="summoner_id" runat="server" Text="Label" class="alert alert-warning"></asp:Label>
            </div>
        </div>

        <div id="champ_matchup" class="center">
            <div class="row form-inline">
                <div class="col-md-3"></div>
                <div class="col-md-3 center">
                    <asp:TextBox id="champ_as" runat="server" placeholder="Your champion" class="form-control text-lol"/>
                </div>
                <div class="col-md-3 center">
                    <asp:TextBox id="champ_vs" runat="server" placeholder="Opponent champion" class="form-control text-lol"/>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="champ_button" runat="server" OnClick="getMatchups" Text="Get Matchup" class="btn btn-lol"/>
                </div>
            </div>
        </div>  

    <div id="server_status">
         <asp:Label ID="status" runat="server" Text="Label"></asp:Label>
    </div>

    <div id="matchup_as">
        <asp:Label ID="win_as" runat="server" Text="win_as"></asp:Label>
    </div>

    <div id="matchup_vs">
        <asp:Label ID="win_vs" runat="server" Text="win_vs"></asp:Label>
    </div>
    </form>
    </div>
</body>
</html>