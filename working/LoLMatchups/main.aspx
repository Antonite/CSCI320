<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>LoLMatchups</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="search_box">
        <asp:TextBox id="summoner_box" runat="server" />
        <asp:Button ID="findSummoner" runat="server" OnClick="submit" Text="Find Summoner" />
    </div>
    <div id="summoner_info">
        <asp:Label ID="summoner_id" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Table ID="infoTable" runat="server" Height="100px" Width="1000" EnableTheming="False"></asp:Table>
    </div>
    <div id="server_status">
         <asp:Label ID="status" runat="server" Text="Label"></asp:Label>
    </div>

    <div id="champ_matchup">
        <asp:TextBox id="champ_as" runat="server" />
        <asp:TextBox id="champ_vs" runat="server" />
        <asp:Button ID="champ_button" runat="server" OnClick="getMatchups" Text="Get Matchup" />
    </div>

    <div id="matchup_as">
        <asp:Label ID="win_as" runat="server" Text="win_as"></asp:Label>
    </div>

    <div id="matchup_vs">
        <asp:Label ID="win_vs" runat="server" Text="win_vs"></asp:Label>
    </div>
    </form>
</body>
</html>
