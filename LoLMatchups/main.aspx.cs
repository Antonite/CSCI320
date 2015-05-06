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
        summoner_id.Style.Add("display", "none");
        champ_as.Style.Add("display", "none");
        champ_vs.Style.Add("display", "none");
        champ_button.Style.Add("display", "none");
        matchupStatus.Text = "";
        connection = connectToServer();
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

            //add custom header row
            //(to be removed upon addition of column names in db)
            TableRow arow = new TableRow();
            TableCell acell = new TableCell();
            acell.Text = "Champion Id";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Played as Won";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Played as Total";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Played vs Won";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Played vs Total";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Summoner Id";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Rank";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "Level";
            arow.Cells.Add(acell);
            acell = new TableCell();
            acell.Text = "last_update_match_id";
            arow.Cells.Add(acell);

            infoTable.Rows.Add(arow);
            while (reader.Read())
            {
                
                arow = new TableRow();
                for (int i = 0; i < reader.VisibleFieldCount; i++)
                {
                    acell = new TableCell();
                    acell.Text = reader.GetString(i);
                    arow.Cells.Add(acell);
                }
                infoTable.Rows.Add(arow);
            }

            summonerId = reader.GetString(5);
            summoner_id.Text = "";
            summoner_id.Style.Add("display", "none");
            infoTable.Style.Add("display", "inline");
        }
        catch (Exception notFound)
        {
            summoner_id.Text = "Summoner " + summoner_box.Text + " not found";
            summoner_id.Style.Add("display", "inline");

            infoTable.Style.Add("display", "none");
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
            "player_matchup.played " +
            "FROM lolmatchups.player_matchup " +
            "join lolmatchups.champion player on player_matchup.player_champion_id = player.champion_id " +
            "join lolmatchups.champion opponent on player_matchup.opponent_champion_id = opponent.champion_id" +
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
               float winPercent = reader.GetFloat(2) / reader.GetFloat(3) * 100;
               matchupStatus.Text = reader.GetString(0) + " vs " + reader.GetString(1) + " " + winPercent + "%";

            }
            else
            {
                matchupStatus.Text = "No data found";
            }
        } catch (Exception notFound)
        {
            matchupStatus.Text = notFound.ToString();
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