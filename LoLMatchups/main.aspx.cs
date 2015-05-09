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
    private System.ComponentModel.IContainer components;
    private MySqlConnection connection;
    private String summonerId;
    protected void Page_Load(object sender, EventArgs e)
    {
        summoner_id.Text = "";
        status.Text = "";
        status.Style.Add("display", "none");
        summoner_id.Style.Add("display", "none");
        champ_as.Style.Add("display", "none");
        champ_vs.Style.Add("display", "none");
        champ_button.Style.Add("display", "none");
        win_as.Text = "";
        win_vs.Text = "";
        connection = connectToServer();
    }

    protected String parseRank (String rank)
    {
        String result = "";

        char div = rank[0];
        char num = rank[1];

        switch (div)
        {
            case 'b': result += "Bronze ";
                break;
            case 's': result += "Silver ";
                break;
            case 'g': result += "Gold ";
                break;
            case 'p': result += "Platinum ";
                break;
            case 'd': result += "Diamond ";
                break;
            case 'm': result += "Master ";
                break;
            case 'c': result += "Challenger ";
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
            "SELECT player_matchup.player_champion_id," +
            "player_matchup.player_champion_id," +
            "player_matchup.won," +
            "player_matchup.played " +
            "FROM lolmatchups.player_matchup" +
                " where lolmatchups.player_matchup.player_champion_id = @as " +
                "and lolmatchups.player_matchup.opponent_champion_id = @vs" +
                " and lolmatchups.player_matchup.summoner_id = @id";
        cmd.Parameters.Add("@as", MySqlDbType.VarChar, 8);
        cmd.Parameters["@as"].Value = champ_as.Text;
        cmd.Parameters.Add("@vs", MySqlDbType.VarChar, 8);
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
            reader.Read();

            win_as.Text = reader.GetString(0);
        } catch (Exception notFound)
        {
            win_as.Text = notFound.ToString();
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