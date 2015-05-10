using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    //todo case sensitivity
    private MySqlConnection connection;
    private static String summonerId;

    protected void Page_Load(object sender, EventArgs e)
    {
        summoner_id.Text = "";
        status.Text = "";
        status.Style.Add("display", "none");
        summoner_id.Style.Add("display", "none");
        champ_as.Style.Add("display", "none");
        champ_vs.Style.Add("display", "none");
        champ_button.Style.Add("display", "none");
        //itemImg1.Style.Add("display", "none");
        matchupStats.Text = "";
        matchupPercent.Text = "";
        matchupTopRunes.Text = "";
        matchupTopMasteries.Text = "";
        ChampAsName.Text = "";
        ChampVsName.Text = "";
        matchupTopItemsPercent.Text = "";
        matchupTopRunesPercent.Text = "";
        matchupTopMasteriesPercent.Text = "";
        connection = connectToServer();
    }

    protected String parseRank (String rank)
    {
        String result = "";

        char div = rank[0];
        char num = rank[1];

        switch (div)
        {
            case 'B': result += "Bronze ";
                break;
            case 'S': result += "Silver ";
                break;
            case 'G': result += "Gold ";
                break;
            case 'P': result += "Platinum ";
                break;
            case 'D': result += "Diamond ";
                break;
            case 'M': result += "Master ";
                break;
            case 'C': result += "Challenger ";
                break;
            default: result = "Badly formatted rank";
                break;
        }

        switch (num)
        {
            case '1': result += "I";
                break;
            case '2': result += "II";
                break;
            case '3': result += "III";
                break;
            case '4': result += "IV";
                break;
            case '5': result += "V";
                break;
            default: result = "Badly formatted rank";
                break;
        }

        return result;
    }

    protected void submit(object sender, EventArgs e)
    {   
        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = "SELECT player_champion_stat.champion_id," +
            "player_champion_stat.played_as_won," +
            "player_champion_stat.played_as_total," +
            "player_champion_stat.played_against_won," +
            "player_champion_stat.played_against_total," +
            "player_champion_stat.summoner_id," +
            "player.rank," +
            "player.level," +
            "player.last_update_match_id " +
            "FROM lolmatchups.player_champion_stat," +
                "lolmatchups.player where lolmatchups.player_champion_stat.summoner_id =" +
                " lolmatchups.player.summoner_id and lolmatchups.player.name = @name";
        cmd.Parameters.Add("@name", MySqlDbType.VarChar, 11);
        cmd.Parameters["@name"].Value = summoner_box.Text;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;

        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            reader.Read();

            sum_name.InnerText = summoner_box.Text;
            sum_rank.InnerText = parseRank (reader.GetString(6));

            summonerId = reader.GetString(5);
            summoner_id.Text = "";
            summoner_id.Style.Add("display", "none");
        }
        catch (Exception notFound)
        {
            summoner_id.Text = "Summoner " + summoner_box.Text + " not found";
            summoner_id.Style.Add("display", "inline");
            champ_as.Style.Add("display", "none");
            champ_vs.Style.Add("display", "none");
            champ_button.Style.Add("display", "none");

            connection.Close();
            return;
        }

        //close connection
        connection.Close();

        //reset input box
        //summoner_box.Text = "";

        champ_as.Style.Add("display", "inline");
        champ_vs.Style.Add("display", "inline");
        champ_button.Style.Add("display", "inline");
    }

    protected void getMatchups (object sender, EventArgs e)
    {
        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText =
            "SELECT player.name," +
            "opponent.name," +
            "player_matchup.won," +
            "player_matchup.played," +
            "items.item_id1," +
            "items.item_id2," +
            "items.item_id3," +
            "items.item_id4," +
            "items.item_id5," +
            "items.item_id6," +
            "items.item_id7," + //10
            "items.won," +
            "items.used," +
            "masteries.offense_values," +
            "masteries.defense_values," +
            "masteries.utility_values," +
            "masteries.won," +
            "masteries.used," + //17
            "runes.rune_id1," +
            "runes.rune_id2," +
            "runes.rune_id3," +
            "runes.rune_id4," +
            "runes.rune_id5," +
            "runes.rune_id6," +
            "runes.rune_id7," +
            "runes.rune_id8," +
            "runes.rune_id9," +
            "runes.rune_id10," +
            "runes.rune_id11," +
            "runes.rune_id12," +
            "runes.rune_id13," +
            "runes.rune_id14," +
            "runes.rune_id15," +
            "runes.rune_id16," +
            "runes.rune_id17," +
            "runes.rune_id18," +
            "runes.rune_id19," +
            "runes.rune_id20," +
            "runes.rune_id21," +
            "runes.rune_id22," +
            "runes.rune_id23," +
            "runes.rune_id24," +
            "runes.rune_id25," +
            "runes.rune_id26," +
            "runes.rune_id27," +
            "runes.rune_id28," +
            "runes.rune_id29," +
            "runes.rune_id30," + //47
            "runes.won," +
            "runes.used" +
            " FROM lolmatchups.player_matchup " +
                "join lolmatchups.champion player on player_matchup.player_champion_id = player.champion_id " +
                "join lolmatchups.champion opponent on player_matchup.opponent_champion_id = opponent.champion_id " +
                "join lolmatchups.player_items items on player_matchup.matchup_id = items.matchup_id " +
                "join lolmatchups.best_matchup_items bestItems on player_matchup.matchup_id = bestItems.matchup_id " +
                "join lolmatchups.player_mastery masteries on player_matchup.matchup_id = masteries.matchup_id " +
                "join lolmatchups.best_matchup_masteries bestMasteries on player_matchup.matchup_id = bestMasteries.matchup_id " +
                "join lolmatchups.player_rune_set runes on player_matchup.matchup_id = runes.matchup_id " +
                "join lolmatchups.best_matchup_rune_set bestRunes on player_matchup.matchup_id = bestRunes.matchup_id " +
                " where player.name = @as" +
                " and opponent.name = @vs" +
                " and lolmatchups.player_matchup.summoner_id = @id";
        cmd.Parameters.Add("@as", MySqlDbType.VarChar, 32);
        cmd.Parameters["@as"].Value = champ_as.Text;
        cmd.Parameters.Add("@vs", MySqlDbType.VarChar, 32);
        cmd.Parameters["@vs"].Value = champ_vs.Text;
        cmd.Parameters.Add("@id", MySqlDbType.VarChar, 8);
        cmd.Parameters["@id"].Value = summonerId;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;


        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {

                reader.Read();
                //calculate win % and diplay
                float totalWinPercent = reader.GetFloat(2) / reader.GetFloat(3) * 100;
                float itemsWinPercent = reader.GetFloat(11) / reader.GetFloat(12) * 100;
                float runesWinPercent = reader.GetFloat(48) / reader.GetFloat(49) * 100;
                float masteriesWinPercent = reader.GetFloat(16) / reader.GetFloat(17) * 100;

                matchupPercent.Text = "Matchup Winrate: " + totalWinPercent + "%";
                matchupTopItemsPercent.Text = itemsWinPercent + "%";
                matchupTopRunesPercent.Text = runesWinPercent + "%";
                matchupTopMasteriesPercent.Text = masteriesWinPercent + "%";

                //fill in data for labels
                ChampAsName.Text = reader.GetString(0);
                ChampVsName.Text = reader.GetString(1);

                //fill item images
                item1Path.Value = "ItemImages\\" + reader.GetString(4) + ".png";
                item2Path.Value = "ItemImages\\" + reader.GetString(5) + ".png";
                item3Path.Value = "ItemImages\\" + reader.GetString(6) + ".png";
                item4Path.Value = "ItemImages\\" + reader.GetString(7) + ".png";
                item5Path.Value = "ItemImages\\" + reader.GetString(8) + ".png";
                item6Path.Value = "ItemImages\\" + reader.GetString(9) + ".png";
                item7Path.Value = "ItemImages\\" + reader.GetString(10) + ".png";
                
                //matchupTopMasteries.Text = reader.GetString(13) + "...";
               // matchupTopRunes.Text = reader.GetString(18) + " ... " + reader.GetString(47);



            }
            else
            {
                matchupStats.Text = "No data found";
            }

        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
        }
    }

    protected MySqlConnection connectToServer()
    {
        
        //connect to server
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["lolmatchupsDB"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        return connection;
    }


}