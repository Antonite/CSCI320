using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.HtmlControls;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    //todo case sensitivity
    private MySqlConnection connection;
    private static String summonerId;
    private const String API_KEY = "e171bba5-29fa-41e8-a1af-2c82b601b947";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (((String) HttpContext.Current.Session["summonerName"]) != "")
        {
            updateDb( (String) HttpContext.Current.Session["summonerName"] );
            summoner_box.Text = (String) HttpContext.Current.Session["summonerName"];
            HttpContext.Current.Session["summonerName"] = "";
            submit(findSummoner, null);

            summoner_id.Text = "";
            status.Text = "";
            status.Style.Add("display", "none");
            summoner_id.Style.Add("display", "none");
            runeTable.Style.Add("display", "none");
            matchup_panel.Style.Add("display", "none");

            return;
            //this.findSummoner.Click(this.findSummoner, EventArgs.Empty);
        }

        if (sum_rank.InnerText.Equals("Rank"))
        {
            champ_button.Attributes.Add("disabled", "disabled");
        }


        summoner_id.Text = "";
        status.Text = "";
        status.Style.Add("display", "none");
        summoner_id.Style.Add("display", "none");
        runeTable.Style.Add("display", "none");

        matchup_panel.Style.Add("display", "none");

        //itemImg1.Style.Add("display", "none");
        matchupStats.Text = "";
        matchupPercent.Text = "";
        //matchupTopRunes.Text = "";
        //matchupTopMasteries.Text = "";
        ChampAsName.Text = "";
        ChampVsName.Text = "";
        winPercentAsLeft.Text = "";
        winPercentVsLeft.Text = "";
        winPercentAsRight.Text = "";
        winPercentVsRight.Text = "";
        matchupTopItemsPercent.Text = "";
        matchupTopRunesPercent.Text = "";
        matchupTopMasteriesPercent.Text = "";
        sumWinRate.Text = "";
        // connection = connectToServer();
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

    
    protected string parseApiRank(string tier, string division) {
        string rank_string = "";
        switch (tier) { 
            case "BRONZE": rank_string += "B";
                break;
            case "SILVER": rank_string += "S";
                break;
            case "GOLD": rank_string += "G";
                break;
            case "PLATINUM": rank_string += "P";
                break;
            case "DIAMOND": rank_string += "D";
                break;
        }
        switch( division ){
            case "I": rank_string += "1";
                break;
            case "II": rank_string += "2";
                break;
            case "III": rank_string += "3";
                break;
            case "IV": rank_string += "4";
                break;
            case "V": rank_string += "5";
                break;
        }
        return rank_string;
    }

    protected int getMasteryIndex(int mastery_id) { 
        int index = -1;
        switch (mastery_id) { 
            case 4111: index = 0;
                break;
            case 4112: index = 1;
                break;
            case 4113: index = 2;
                break;
            case 4114: index = 3;
                break;
            case 4121: index = 4;
                break;
            case 4122: index = 5;
                break;
            case 4123: index = 6;
                break;
            case 4124: index = 7;
                break;
            case 4131: index = 8;
                break;
            case 4132: index = 9;
                break;
            case 4133: index = 10;
                break;
            case 4134: index = 11;
                break;
            case 4141: index = 12;
                break;
            case 4142: index = 13;
                break;
            case 4143: index = 14;
                break;
            case 4144: index = 15;
                break;
            case 4151: index = 16;
                break;
            case 4152: index = 17;
                break;
            case 4154: index = 18;
                break;
            case 4162: index = 19;
                break;
            case 4211: index = 0;
                break;
            case 4212: index = 1;
                break;
            case 4213: index = 2;
                break;
            case 4214: index = 3;
                break;
            case 4221: index = 4;
                break;
            case 4222: index = 5;
                break;
            case 4224: index = 6;
                break;
            case 4231: index = 7;
                break;
            case 4232: index = 8;
                break;
            case 4233: index = 9;
                break;
            case 4234: index = 10;
                break;
            case 4241: index = 11;
                break;
            case 4242: index = 12;
                break;
            case 4243: index = 13;
                break;
            case 4244: index = 14;
                break;
            case 4251: index = 15;
                break;
            case 4252: index = 16;
                break;
            case 4253: index = 17;
                break;
            case 4262: index = 18;
                break;
            case 4311: index = 0;
                break;
            case 4312: index = 1;
                break;
            case 4313: index = 2;
                break;
            case 4314: index = 3;
                break;
            case 4322: index = 4;
                break;
            case 4323: index = 5;
                break;
            case 4324: index = 6;
                break;
            case 4331: index = 7;
                break;
            case 4332: index = 8;
                break;
            case 4333: index = 9;
                break;
            case 4334: index = 10;
                break;
            case 4341: index = 11;
                break;
            case 4342: index = 12;
                break;
            case 4343: index = 13;
                break;
            case 4344: index = 14;
                break;
            case 4352: index = 15;
                break;
            case 4353: index = 16;
                break;
            case 4362: index = 17;
                break;
        }
        return index;
    }



    protected void submit(object sender, EventArgs e)
    {
        connection = connectToServer();
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
            champ_button.Attributes.Add("disabled", "disabled");

            connection.Close();
            return;
        }

        //close connection
        connection.Close();

        champ_button.Attributes.Remove("disabled");
    }

    protected void getMatchups (object sender, EventArgs e)
    {
        connection = connectToServer();
        string championAsName;
        string championVsName;
        string offenceMastery;
        string defenceMastery;
        string utilityMastery;

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

                matchupPercent.Text = totalWinPercent + "%";
                matchupTopItemsPercent.Text = itemsWinPercent + "%";
                matchupTopRunesPercent.Text = runesWinPercent + "%";
                matchupTopMasteriesPercent.Text = masteriesWinPercent + "%";

                championAsName = reader.GetString(0);
                championVsName = reader.GetString(1);

                //fill in data for labels
                ChampAsName.Text = championAsName;
                ChampVsName.Text = championVsName;


                //fill champion Images
                championAsPath.Value = "ChampionImages\\" + championAsName + "_Square_0.png";
                championVsPath.Value = "ChampionImages\\" + championVsName + "_Square_0.png";

                //fill item images
                item1Path.Value = "ItemImages\\" + reader.GetString(4) + ".png";
                item2Path.Value = "ItemImages\\" + reader.GetString(5) + ".png";
                item3Path.Value = "ItemImages\\" + reader.GetString(6) + ".png";
                item4Path.Value = "ItemImages\\" + reader.GetString(7) + ".png";
                item5Path.Value = "ItemImages\\" + reader.GetString(8) + ".png";
                item6Path.Value = "ItemImages\\" + reader.GetString(9) + ".png";
                item7Path.Value = "ItemImages\\" + reader.GetString(10) + ".png";
                
                //get masteries
                offenceMastery = reader.GetString(13);
                defenceMastery = reader.GetString(14);
                utilityMastery = reader.GetString(15);
               

                string[] runeList = new string[30];
                for (int i = 18; i < 48; i++)
                {
                    string aRune = reader.GetString(i);
                    if (aRune == null) runeList[i - 18] = "0";
                    else { runeList[i - 18] = aRune; }
                    //matchupTopRunesPercent.Text += runeList[i - 18] + ",";
                }
 

                connection.Close();

                //fill chapion stats
                fillChampStats(championAsName, championVsName);

                //fill masteries
                fillMasteries(offenceMastery, defenceMastery, utilityMastery);

                //fill the runes pages
                processRunes(runeList);

                rune_panel.Attributes.Add("class", "fullwidth panel panel-lol noshadow");
                mastery_panel.Attributes.Add("class", "fullwidth panel panel-lol noshadow");

                matchup_panel.Style.Add("display", "block");
            }
            else
            {
                matchupStats.Text = "No data found";
                connection.Close();
            }

        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
    }


    protected MySqlConnection connectToServer()
    {
        
        //connect to server
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["lolmatchupsDB"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        return connection;
    }


    protected void fillMasteries(String offence, String defence, String utility)
    {
        MySqlDataReader reader;
        //search for mastery details
        MySqlCommand cmd = new MySqlCommand();

        //mastery disctionaries
        Dictionary<int, string[]> offenceDict = new Dictionary<int, string[]>();
        Dictionary<int, string[]> defenceDict = new Dictionary<int, string[]>();
        Dictionary<int, string[]> utilityDict = new Dictionary<int, string[]>();
        int totalOffence = 0; ;
        int totalDefence = 0; ;
        int totalUtility = 0; ;

        cmd = new MySqlCommand();
        cmd.CommandText = "select * from lolmatchups.mastery ORDER BY tree, `index`";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;

        //parse offence masteries
        String parseOffence = offence.Substring(1, offence.Length - 2);
        String parseDefence = defence.Substring(1, defence.Length - 2);
        String parseUtility = utility.Substring(1, utility.Length - 2);
        string[] splitOffence = parseOffence.Split(new char[] { ',' });
        for (int i = 0; i < splitOffence.Length; i++)
        {
            splitOffence[i] = splitOffence[i].TrimStart();
            totalOffence += int.Parse(splitOffence[i]);
        }
        string[] splitDefence = parseDefence.Split(new char[] { ',' });
        for (int i = 0; i < splitDefence.Length; i++)
        {
            splitDefence[i] = splitDefence[i].TrimStart();
            totalDefence += int.Parse(splitDefence[i]);
        }
        string[] splitUtility = parseUtility.Split(new char[] { ',' });
        for (int i = 0; i < splitUtility.Length; i++)
        {
            splitUtility[i] = splitUtility[i].TrimStart();
            totalUtility += int.Parse(splitUtility[i]);
        }


        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //get defence
                for (int i = 0; i < 19;i++ )
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    defenceDict.Add(i, masteryRanks);
                }
                //get offence
                for (int i = 0; i < 20; i++)
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    offenceDict.Add(i, masteryRanks);
                }
                //get utility
                for (int i = 0; i < 18; i++)
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    utilityDict.Add(i, masteryRanks);
                }


            }
            else
            {
                matchupStats.Text = "No data found";
                connection.Close();
            }
        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
        connection.Close();
        //fill in labels
        //defence
        defence1.Text = splitDefence[0] + "/" + defenceDict[0][5];
        defenceDesc1.Text = defenceDict[0][int.Parse(splitDefence[0])];

        defence2.Text = splitDefence[1] + "/" + defenceDict[1][5];
        defenceDesc2.Text = defenceDict[1][int.Parse(splitDefence[1])];

        defence3.Text = splitDefence[2] + "/" + defenceDict[2][5];
        defenceDesc3.Text = defenceDict[2][int.Parse(splitDefence[2])];

        defence4.Text = splitDefence[3] + "/" + defenceDict[3][5];
        defenceDesc4.Text = defenceDict[3][int.Parse(splitDefence[3])];

        defence5.Text = splitDefence[4] + "/" + defenceDict[4][5];
        defenceDesc5.Text = defenceDict[4][int.Parse(splitDefence[4])];

        defence6.Text = splitDefence[5] + "/" + defenceDict[5][5];
        defenceDesc6.Text = defenceDict[5][int.Parse(splitDefence[5])];

        defence7.Text = splitDefence[6] + "/" + defenceDict[6][5];
        defenceDesc7.Text = defenceDict[6][int.Parse(splitDefence[6])];

        defence8.Text = splitDefence[7] + "/" + defenceDict[7][5];
        defenceDesc8.Text = defenceDict[7][int.Parse(splitDefence[7])];

        defence9.Text = splitDefence[8] + "/" + defenceDict[8][5];
        defenceDesc9.Text = defenceDict[8][int.Parse(splitDefence[8])];

        defence10.Text = splitDefence[9] + "/" + defenceDict[9][5];
        defenceDesc10.Text = defenceDict[9][int.Parse(splitDefence[9])];

        defence11.Text = splitDefence[10] + "/" + defenceDict[10][5];
        defenceDesc11.Text = defenceDict[10][int.Parse(splitDefence[10])];

        defence12.Text = splitDefence[11] + "/" + defenceDict[11][5];
        defenceDesc12.Text = defenceDict[11][int.Parse(splitDefence[11])];

        defence13.Text = splitDefence[12] + "/" + defenceDict[12][5];
        defenceDesc13.Text = defenceDict[12][int.Parse(splitDefence[12])];

        defence14.Text = splitDefence[13] + "/" + defenceDict[13][5];
        defenceDesc14.Text = defenceDict[13][int.Parse(splitDefence[13])];

        defence15.Text = splitDefence[14] + "/" + defenceDict[14][5];
        defenceDesc15.Text = defenceDict[14][int.Parse(splitDefence[14])];

        defence16.Text = splitDefence[15] + "/" + defenceDict[15][5];
        defenceDesc16.Text = defenceDict[15][int.Parse(splitDefence[15])];

        defence17.Text = splitDefence[16] + "/" + defenceDict[16][5];
        defenceDesc17.Text = defenceDict[16][int.Parse(splitDefence[16])];

        defence18.Text = splitDefence[17] + "/" + defenceDict[17][5];
        defenceDesc18.Text = defenceDict[17][int.Parse(splitDefence[17])];

        defence19.Text = splitDefence[18] + "/" + defenceDict[18][5];
        defenceDesc19.Text = defenceDict[18][int.Parse(splitDefence[18])];

        //offence
        offence1.Text = splitOffence[0] + "/" + offenceDict[0][5];
        offenceDesc1.Text = offenceDict[0][int.Parse(splitOffence[0])];

        offence2.Text = splitOffence[1] + "/" + offenceDict[1][5];
        offenceDesc2.Text = offenceDict[1][int.Parse(splitOffence[1])];

        offence3.Text = splitOffence[2] + "/" + offenceDict[2][5];
        offenceDesc3.Text = offenceDict[2][int.Parse(splitOffence[2])];

        offence4.Text = splitOffence[3] + "/" + offenceDict[3][5];
        offenceDesc4.Text = offenceDict[3][int.Parse(splitOffence[3])];

        offence5.Text = splitOffence[4] + "/" + offenceDict[4][5];
        offenceDesc5.Text = offenceDict[4][int.Parse(splitOffence[4])];

        offence6.Text = splitOffence[5] + "/" + offenceDict[5][5];
        offenceDesc6.Text = offenceDict[5][int.Parse(splitOffence[5])];

        offence7.Text = splitOffence[6] + "/" + offenceDict[6][5];
        offenceDesc7.Text = offenceDict[6][int.Parse(splitOffence[6])];

        offence8.Text = splitOffence[7] + "/" + offenceDict[7][5];
        offenceDesc8.Text = offenceDict[7][int.Parse(splitOffence[7])];

        offence9.Text = splitOffence[8] + "/" + offenceDict[8][5];
        offenceDesc9.Text = offenceDict[8][int.Parse(splitOffence[8])];

        offence10.Text = splitOffence[9] + "/" + offenceDict[9][5];
        offenceDesc10.Text = offenceDict[9][int.Parse(splitOffence[9])];

        offence11.Text = splitOffence[10] + "/" + offenceDict[10][5];
        offenceDesc11.Text = offenceDict[10][int.Parse(splitOffence[10])];

        offence12.Text = splitOffence[11] + "/" + offenceDict[11][5];
        offenceDesc12.Text = offenceDict[11][int.Parse(splitOffence[11])];

        offence13.Text = splitOffence[12] + "/" + offenceDict[12][5];
        offenceDesc13.Text = offenceDict[12][int.Parse(splitOffence[12])];

        offence14.Text = splitOffence[13] + "/" + offenceDict[13][5];
        offenceDesc14.Text = offenceDict[13][int.Parse(splitOffence[13])];

        offence15.Text = splitOffence[14] + "/" + offenceDict[14][5];
        offenceDesc15.Text = offenceDict[14][int.Parse(splitOffence[14])];

        offence16.Text = splitOffence[15] + "/" + offenceDict[15][5];
        offenceDesc16.Text = offenceDict[15][int.Parse(splitOffence[15])];

        offence17.Text = splitOffence[16] + "/" + offenceDict[16][5];
        offenceDesc17.Text = offenceDict[16][int.Parse(splitOffence[16])];

        offence18.Text = splitOffence[17] + "/" + offenceDict[17][5];
        offenceDesc18.Text = offenceDict[17][int.Parse(splitOffence[17])];

        offence19.Text = splitOffence[18] + "/" + offenceDict[18][5];
        offenceDesc19.Text = offenceDict[18][int.Parse(splitOffence[18])];

        offence20.Text = splitOffence[19] + "/" + offenceDict[19][5];
        offenceDesc20.Text = offenceDict[19][int.Parse(splitOffence[19])];

        //utility
        utility1.Text = splitUtility[0] + "/" + utilityDict[0][5];
        utilityDesc1.Text = utilityDict[0][int.Parse(splitUtility[0])];

        utility2.Text = splitUtility[1] + "/" + utilityDict[1][5];
        utilityDesc2.Text = utilityDict[1][int.Parse(splitUtility[1])];

        utility3.Text = splitUtility[2] + "/" + utilityDict[2][5];
        utilityDesc3.Text = utilityDict[2][int.Parse(splitUtility[2])];

        utility4.Text = splitUtility[3] + "/" + utilityDict[3][5];
        utilityDesc4.Text = utilityDict[3][int.Parse(splitUtility[3])];

        utility5.Text = splitUtility[4] + "/" + utilityDict[4][5];
        utilityDesc5.Text = utilityDict[4][int.Parse(splitUtility[4])];

        utility6.Text = splitUtility[5] + "/" + utilityDict[5][5];
        utilityDesc6.Text = utilityDict[5][int.Parse(splitUtility[5])];

        utility7.Text = splitUtility[6] + "/" + utilityDict[6][5];
        utilityDesc7.Text = utilityDict[6][int.Parse(splitUtility[6])];

        utility8.Text = splitUtility[7] + "/" + utilityDict[7][5];
        utilityDesc8.Text = utilityDict[7][int.Parse(splitUtility[7])];

        utility9.Text = splitUtility[8] + "/" + utilityDict[8][5];
        utilityDesc9.Text = utilityDict[8][int.Parse(splitUtility[8])];

        utility10.Text = splitUtility[9] + "/" + utilityDict[9][5];
        utilityDesc10.Text = utilityDict[9][int.Parse(splitUtility[9])];

        utility11.Text = splitUtility[10] + "/" + utilityDict[10][5];
        utilityDesc11.Text = utilityDict[10][int.Parse(splitUtility[10])];

        utility12.Text = splitUtility[11] + "/" + utilityDict[11][5];
        utilityDesc12.Text = utilityDict[11][int.Parse(splitUtility[11])];

        utility13.Text = splitUtility[12] + "/" + utilityDict[12][5];
        utilityDesc13.Text = utilityDict[12][int.Parse(splitUtility[12])];

        utility14.Text = splitUtility[13] + "/" + utilityDict[13][5];
        utilityDesc14.Text = utilityDict[13][int.Parse(splitUtility[13])];

        utility15.Text = splitUtility[14] + "/" + utilityDict[14][5];
        utilityDesc15.Text = utilityDict[14][int.Parse(splitUtility[14])];

        utility16.Text = splitUtility[15] + "/" + utilityDict[15][5];
        utilityDesc16.Text = utilityDict[15][int.Parse(splitUtility[15])];

        utility17.Text = splitUtility[16] + "/" + utilityDict[16][5];
        utilityDesc17.Text = utilityDict[16][int.Parse(splitUtility[16])];

        utility18.Text = splitUtility[17] + "/" + utilityDict[17][5];
        utilityDesc18.Text = utilityDict[17][int.Parse(splitUtility[17])];



        sampleMasteriesAggregate.Text = totalOffence + "-" + totalDefence + "-" + totalUtility;
    }


    protected void fillChampStats(String nameAs, String nameVs)
    {
        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();

        Dictionary<string, string[]> allChampions = new Dictionary<string, string[]>();

        //fill percent total % champions
        cmd = new MySqlCommand();
        cmd.CommandText = "select champs.name, " +
                            "stats.played_as_won, " +
                            "stats.played_as_total, " +
                            "stats.played_against_won, " +
                            "stats.played_against_total, " +
                            "stats.champion_id " +
            "from lolmatchups.player_champion_stat stats " +
            "join lolmatchups.champion champs on stats.champion_id = champs.champion_id " +
            "where stats.summoner_id = @id";
        //cmd.Parameters.Add("@as", MySqlDbType.VarChar, 32);
        //cmd.Parameters["@as"].Value = nameAs;
        //cmd.Parameters.Add("@vs", MySqlDbType.VarChar, 32);
        //cmd.Parameters["@vs"].Value = nameVs;
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

            string winPercentAschamp;
            string winPercentVschamp;
            string totalSummonerPercent;
            float totalPlayed = 0;
            float totalWon = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    totalWon += reader.GetFloat(1);
                    totalPlayed += reader.GetFloat(2);

                    winPercentAschamp = Math.Round(reader.GetFloat(1) / reader.GetFloat(2) * 100) + "%";
                    winPercentVschamp = Math.Round(reader.GetFloat(3) / reader.GetFloat(4) * 100) + "%";
                    if (winPercentAschamp.Equals("NaN%")) winPercentAschamp = "None played";
                    if (winPercentVschamp.Equals("NaN%")) winPercentVschamp = "None played";


                    if (reader.GetString(0).Equals(nameAs))
                    {
                        winPercentAsLeft.Text = winPercentAschamp;
                        winPercentVsLeft.Text = winPercentVschamp;
                    }
                    else if (reader.GetString(0).Equals(nameVs))
                    {
                        winPercentAsRight.Text = winPercentAschamp;
                        winPercentVsRight.Text = winPercentVschamp;
                    }


                    if (!winPercentAschamp.Equals("None played"))
                    {
                        string[] champStats = new string[3];
                        champStats[0] = reader.GetString(5);
                        champStats[1] = winPercentAschamp;
                        champStats[2] = winPercentVschamp;
                        allChampions.Add(reader.GetString(0), champStats);
                    }

                }


                createChampions(allChampions);

            }
            else
            {
                matchupStats.Text = "No data found";
            }

            totalSummonerPercent = Math.Round(totalWon / totalPlayed * 100) + "%";
            if (totalSummonerPercent.Equals("NaN%")) totalSummonerPercent = "None played";
            sumWinRate.Text = totalSummonerPercent + " Winrate";

            connection.Close();
        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }

    }

    protected void createChampions(Dictionary<string, string[]> allChampions)
    {
        String champTile;
        String champRow;
        String tag = "";
        int count = 0;

        champRow = "";


        foreach (string aName in allChampions.Keys)
        {
            count++;
            if (count == 1)
            {
                champRow = "<div class=\"row\">";
                tag = "champion_tile_first";
            }
            else if (count == 6)
            {
                tag = "champion_tile_last";
            }

            champTile = "<div id=\"example_champ\" runat=\"server\" class=\"col-md-2 panel champion_tile " + tag + " \">" +
                            "<table class=\"fullwidth center\">" +
                                "<tr>" +
                                    "<td>" +
                                        "<img src=\"ChampionImages\\" + aName + "_Square_0.png\" class=\"champion_portrait center\"></img>" +
                                        "<div class=\"champion_name\">" + aName + "</div>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        "<div class=\"champion_as_won won\">" + allChampions[aName][1] + "</div>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        "<div class=\"champion_vs_won won\">" + allChampions[aName][2] + "</div>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</div>";

            champRow += champTile;

            if (count == 6)
            {
                champRow += "</div>";
                champion_panel.Controls.Add(new LiteralControl(champRow));
                count = 0;
            }

            tag = "";

        }

        if (count != 0)
        {
            champRow += "</div>";
            champion_panel.Controls.Add(new LiteralControl(champRow));
        }

    }



    protected void processRunes(String[] runesIds)
    {
        Dictionary<string, double> runeDict = new Dictionary<string, double>();

        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = "select * from lolmatchups.rune where rune.rune_id in (@r1,@r2,@r3,@r3,@r5,@r6,@r7,@r8,@r9,@r10,@r11,@r12,@r13,@r14,@r15,@r16,@r17,@r18,@r19,@r20,@r21,@r22,@r23,@r24,@r25,@r26,@r27,@r28,@r29,@r30)";
        for (int i = 0; i < 30; i++)
        {
            cmd.Parameters.Add("@r" + (i+1), MySqlDbType.VarChar, 8);
            cmd.Parameters["@r" + (i + 1)].Value = runesIds[i];
        }

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

                while (reader.Read())
                {
                    string runeid = reader.GetString(0);
                    string runeEffect = reader.GetString(4);

                    string[] split = runeEffect.Split(new char[] {' '}, 2);
                    
                    string effectType = split[1].TrimStart();
                    string effectAmount = split[0].Substring(1, split[0].Length-1);

                    if (effectAmount.EndsWith("%")) effectAmount = effectAmount.Substring(0, split[0].Length - 2);

                    //remove additional parentheses
                    string[] refineSplit = effectType.Split(new char[] { '(' }, 2);
                    effectType = refineSplit[0].TrimEnd();

                    //capitalize first letter
                    effectType = char.ToUpper(effectType[0]) + effectType.Substring(1);

                    int runeCount = 0;

                    foreach (string aRune in runesIds)
                    {
                        if (aRune.Equals(runeid)) runeCount++;
                    }
                    if (runeDict.ContainsKey(effectType))
                    {
                        runeDict[effectType] += runeCount * Double.Parse(effectAmount);
                    }
                    else
                    {
                        runeDict.Add(effectType, runeCount * Double.Parse(effectAmount));
                    }


                    
                }

                HtmlTableRow runeRow = new HtmlTableRow();
                HtmlTableCell runeCell = new HtmlTableCell();
                

                foreach (String runeKey in runeDict.Keys)
                {
                    runeRow = new HtmlTableRow();

                    //rune name
                    runeCell = new HtmlTableCell();
                    runeCell.InnerText = runeKey;
                    runeRow.Cells.Add(runeCell);


                    //total effect
                    runeCell = new HtmlTableCell();
                    runeCell.InnerText = runeDict[runeKey] + "";
                    runeRow.Cells.Add(runeCell);

                    runeTable.Rows.Add(runeRow);
                  
                }


                runeTable.Style.Remove("display");
            }
            else
            {
                matchupStats.Text = "No data found";
            }

        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
        connection.Close();
    }

    private void updateDb(String summoner_name)
    {
        MySqlDataReader reader;
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT player.summoner_id, player.name, player.last_update_match_id FROM lolmatchups.player WHERE lolmatchups.player.name = @name LIMIT 1";
        cmd.Parameters.Add("@name", MySqlDbType.VarChar);
        cmd.Parameters["@name"].Value = summoner_name;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
          
        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        
        try { 
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                reader.Read();
                int summoner_id = reader.GetInt32( 0 );
                int last_match_id = reader.GetInt32( 2 );
                int new_last_match_id;
                connection.Close();

                String url = "https://na.api.pvp.net/api/lol/na/v2.2/matchhistory/" + summoner_id.ToString() + "?rankedQueues=RANKED_SOLO_5x5,RANKED_TEAM_5x5&api_key=" + API_KEY;
                MatchHistoryResponse json_response = new MatchHistoryResponse();
                
                HttpWebResponse response = executeApiRequest( url );
                try { 
                    Match[] mh_matches = json_response.matches;
                    for (int i = 0; i < mh_matches.Length; i++) { 
                        Match mh_match = mh_matches[i];
                        if (mh_match.matchId == last_match_id) { 
                            break;
                        } else if (i == 0) { 
                            new_last_match_id = mh_match.matchId;
                        }
                        Console.WriteLine( last_match_id.ToString() );
                    }
                } catch (Exception e) { Debug.WriteLine( e.Message ); }
            } else {
                connection.Close();
                //Get summoner ID and build a simple match history
                String summoner_url = "https://na.api.pvp.net/api/lol/na/v1.4/summoner/by-name/" + summoner_name + "?api_key=" + API_KEY;
                
                HttpWebResponse response = executeApiRequest( summoner_url );
                
                Dictionary<string, SummonerInfo> json_response = new JavaScriptSerializer().Deserialize<Dictionary<string, SummonerInfo>>( new StreamReader( response.GetResponseStream() ).ReadToEnd() );

                string stored_summoner_name = summoner_name.ToLower().Trim();
                SummonerInfo s_info = json_response[stored_summoner_name];

                String mh_url1 = "https://na.api.pvp.net/api/lol/na/v2.2/matchhistory/" + s_info.id + "?rankedQueues=RANKED_SOLO_5x5,RANKED_TEAM_5x5&api_key=" + API_KEY;
                String mh_url2 = "https://na.api.pvp.net/api/lol/na/v2.2/matchhistory/" + s_info.id + "?rankedQueues=RANKED_SOLO_5x5,RANKED_TEAM_5x5&beginIndex=15&api_key=" + API_KEY;
                HttpWebResponse mh_response1 = executeApiRequest( mh_url1 );
                HttpWebResponse mh_response2 = executeApiRequest( mh_url2 );

                List<PlayerReturnData> mh1 = parseMhResponse( mh_response1, s_info.id, 0 );
                List<PlayerReturnData> mh2 = parseMhResponse( mh_response2, s_info.id, 0 );

                int latest_match_id = mh1[0].match_id;

                String league_url = "https://na.api.pvp.net/api/lol/na/v2.5/league/by-summoner/" + s_info.id + "/entry?api_key=" + API_KEY;
                
                HttpWebResponse league_response = executeApiRequest( league_url );
                Dictionary<string, League[]> league_json_response = new JavaScriptSerializer().Deserialize<Dictionary<string, League[]>>( new StreamReader( league_response.GetResponseStream() ).ReadToEnd() );
                
                League[] summoner_leagues = league_json_response[s_info.id.ToString()];
                string tier = "";
                string division = "";
                foreach (League league in summoner_leagues) {
                    if (league.queue == "RANKED_SOLO_5x5") { 
                        tier = league.tier;
                        division = league.entries[0].division;
                        break;
                    }
                }
                
                string rank_string = parseApiRank( tier, division );
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "INSERT INTO player VALUES (@sid, @name, @rank, @level, @last_update_match_id);";
                cmd.Parameters.Add("@sid",MySqlDbType.UInt32);
                cmd.Parameters["@sid"].Value = s_info.id;
                cmd.Parameters.Add("@name",MySqlDbType.VarChar);
                cmd.Parameters["@name"].Value = summoner_name;
                cmd.Parameters.Add("@rank",MySqlDbType.VarChar);
                cmd.Parameters["@rank"].Value = rank_string;
                cmd.Parameters.Add("@level",MySqlDbType.UInt32);
                cmd.Parameters["@level"].Value = s_info.summonerLevel;
                cmd.Parameters.Add("@last_update_match_id",MySqlDbType.UInt32);
                cmd.Parameters["@last_update_match_id"].Value = latest_match_id;
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();
                connection.Close();
                updateRows( mh1, s_info.id );
                //return;
                updateRows( mh2, s_info.id );
            }
        }catch( Exception e ){ Debug.WriteLine( e.ToString() ); }
        
    }

    protected HttpWebResponse executeApiRequest( string url ) {
        System.Threading.Thread.Sleep(1001);
        try
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(String.Format(
                "Server error (HTTP {0}: {1}).",
                response.StatusCode,
                response.StatusDescription));
            return response;
        }
        catch (Exception e)
        {
            Debug.WriteLine(url);
            Debug.WriteLine(e.Message);
            return null;
        }
    }

    protected List<PlayerReturnData> parseMhResponse(HttpWebResponse mh_response, int summoner_id, int last_match_id) { 
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MatchHistoryResponse));
        object objResponse = ser.ReadObject(mh_response.GetResponseStream());
        MatchHistoryResponse mh_json = objResponse as MatchHistoryResponse;
        Match[] matches = mh_json.matches;
        List<PlayerReturnData> toReturn = new List<PlayerReturnData>();
        foreach( Match match in matches ){
            int current_match_id = match.matchId;
            if( last_match_id != current_match_id ){
                string match_url = "https://na.api.pvp.net/api/lol/na/v2.2/match/" + current_match_id.ToString() + "?api_key=" + API_KEY;
                HttpWebResponse match_response = executeApiRequest( match_url );
                PlayerReturnData matchup_data = parseMatchResponse( match_response, summoner_id );
                toReturn.Add( matchup_data );
            }
        }
        return toReturn;
    }

    protected PlayerReturnData parseMatchResponse(HttpWebResponse match_response, int summoner_id) { 
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Match));
        object objResponse = ser.ReadObject(match_response.GetResponseStream());
        Match match_json = objResponse as Match;
        
        Participant[] participants = match_json.participants;
        ParticipantIdentity[] participant_identities = match_json.participantIdentities;
        int match_id = match_json.matchId;
        string target_lane = "";
        string target_role = "";
        int target_team = 0;
        int index = -1;
        for( int i = 0; i < participants.Length; i++) { 
            Participant participant = participants[i];
            ParticipantIdentity p_id = participant_identities[i];
            string lane = participant.timeline.lane;
            string role = participant.timeline.role;
            int p_summoner_id = p_id.player.summonerId;
            int team_id = participant.teamId;
            if (p_summoner_id == summoner_id) { 
                target_lane = lane;
                target_role = role;
                target_team = team_id;
                index = i;
                break;
            }
        }
        int op_index = -1;
        for (int i = 0; i < participants.Length; i++) { 
            if( index == i ){
                continue;
            }
            Participant participant = participants[i];
            ParticipantIdentity p_id = participant_identities[i];
            string lane = participant.timeline.lane;
            string role = participant.timeline.role;
            int team_id = participant.teamId;
            if (lane == target_lane) {
                if (team_id != target_team) {
                    if (role == target_role) { 
                        op_index = i;
                        break;
                    }
                }
            }
        }
        if (op_index == -1) { 
            //TODO not found condition
            return null;
        } else { 
            Participant player_participant = participants[index];
            Participant opp_participant = participants[op_index];
            ParticipantIdentity p_id = participant_identities[index];
            ParticipantIdentity opp_id = participant_identities[op_index];
            
            Stats stats = player_participant.stats;
            Rune[] runes = player_participant.runes;
            Mastery[] masteries = player_participant.masteries;
            bool won = stats.winner;
            //parse player runes
            List<int> rune_list = new List<int>();
            foreach (Rune rune in runes) { 
                for( int i = 0; i < rune.rank; i++ ){
                    rune_list.Add( rune.runeId );
                }
            }
            rune_list.Sort();

            //parse player masteries
            int[] offense = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            int[] defense = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            int[] utility = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            foreach( Mastery mastery in masteries ){
                int m_index = getMasteryIndex( mastery.masteryId );
                if( mastery.masteryId > 4300 ){
                    utility[m_index] = mastery.rank;
                }else if( mastery.masteryId > 4200 ){
                    defense[m_index] = mastery.rank;
                }else{
                    offense[m_index] = mastery.rank;
                }
            }
            string off_str = "[";
            string def_str = "[";
            string util_str = "[";
            foreach( int mastery_rank in offense ){
                off_str += mastery_rank.ToString() + ",";
            }
            off_str = off_str.Remove( off_str.Length - 1 );
            off_str += "]";
            foreach( int mastery_rank in defense ){
                def_str += mastery_rank.ToString() + ",";
            }
            def_str = def_str.Remove( def_str.Length - 1 );
            def_str += "]";
            foreach( int mastery_rank in utility ){
                util_str += mastery_rank.ToString() + ",";
            }
            util_str = util_str.Remove( util_str.Length - 1);
            util_str += "]";

            //parse player items
            List<int> items = new List<int>();
            items.Add( stats.item0 );
            items.Add( stats.item1 );
            items.Add( stats.item2 );
            items.Add( stats.item3 );
            items.Add( stats.item4 );
            items.Add( stats.item5 );
            items.Add( stats.item6 );
            items.Sort();
            items.Reverse();

            //parse player ss
            List<int> ss = new List<int>();
            ss.Add( player_participant.spell1Id );
            ss.Add( player_participant.spell2Id );

            //parse player stats
            int kills = stats.kills;
            int deaths = stats.deaths;
            int assists = stats.assists;
            int cs = stats.minionsKilled;
            int p_champ_id = player_participant.championId;
            int o_champ_id = opp_participant.championId;

            PlayerReturnData toReturn = new PlayerReturnData();
            toReturn.assists = assists;
            toReturn.kills = kills;
            toReturn.deaths = deaths;
            toReturn.cs = cs;
            toReturn.pChampId = p_champ_id;
            toReturn.oChampId = o_champ_id;
            toReturn.ss = ss;
            toReturn.items = items;
            toReturn.offense = off_str;
            toReturn.defense = def_str;
            toReturn.utility = util_str;
            toReturn.runes = rune_list;
            toReturn.won = won;
            toReturn.match_id = match_id;
            return toReturn;
        }
    }

    protected void updateRows(List<PlayerReturnData> mh, int summoner_id ) { 
        foreach (PlayerReturnData match in mh) {

            updateChampStats( match, summoner_id );

            MySqlCommand cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_matchup WHERE lolmatchups.player_matchup.summoner_id = @sid AND lolmatchups.player_matchup.player_champion_id = @pid AND lolmatchups.player_matchup.opponent_champion_id = @oid LIMIT 1";
            cmd.Parameters.Add("@sid", MySqlDbType.Int32);
            cmd.Parameters["@sid"].Value = summoner_id;
            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = match.pChampId;
            cmd.Parameters.Add("@oid", MySqlDbType.Int32);
            cmd.Parameters["@oid"].Value = match.oChampId;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;

            //open connection
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

            MySqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.HasRows) {
                //matchup exists, altering
                reader.Read();
                int matchup_id = reader.GetInt32( 0 );
                int won = reader.GetInt32( 4 );
                int played = reader.GetInt32( 5 );
                int kills = reader.GetInt32( 6 );
                int deaths = reader.GetInt32( 7 );
                int assists = reader.GetInt32( 8 );
                int cs = reader.GetInt32( 9 );
                connection.Close();
                reader.Close();

                if (match.won) won++;
                played++;
                kills += match.kills;
                deaths += match.deaths;
                assists += match.assists;
                cs += match.cs;
                
                //update row
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE lolmatchups.player_matchup SET won=@won,played=@played,kills=@k,deaths=@d,assists=@a,cumulative_creep_score=@cs WHERE lolmatchups.player_matchup.summoner_id = @sid AND lolmatchups.player_matchup.player_champion_id = @pid AND lolmatchups.player_matchup.opponent_champion_id = @oid LIMIT 1";
                cmd.Parameters.Add("@won", MySqlDbType.Int32);
                cmd.Parameters["@won"].Value = won;
                cmd.Parameters.Add("@played", MySqlDbType.Int32);
                cmd.Parameters["@played"].Value = played;
                cmd.Parameters.Add("@k", MySqlDbType.Int32);
                cmd.Parameters["@k"].Value = kills;
                cmd.Parameters.Add("@d", MySqlDbType.Int32);
                cmd.Parameters["@d"].Value = deaths;
                cmd.Parameters.Add("@a", MySqlDbType.Int32);
                cmd.Parameters["@a"].Value = assists;
                cmd.Parameters.Add("@cs", MySqlDbType.Int32);
                cmd.Parameters["@cs"].Value = cs;
                cmd.Parameters.Add("@sid", MySqlDbType.Int32);
                cmd.Parameters["@sid"].Value = summoner_id;
                cmd.Parameters.Add("@pid", MySqlDbType.Int32);
                cmd.Parameters["@pid"].Value = match.pChampId;
                cmd.Parameters.Add("@oid", MySqlDbType.Int32);
                cmd.Parameters["@oid"].Value = match.oChampId;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                updateSS( match, summoner_id, matchup_id );
                updateMasteries( match, summoner_id, matchup_id );
                updateRunes( match, summoner_id, matchup_id );
                updateItems( match, summoner_id, matchup_id );
            } else { 
                //no matchup exists
                if( match.won ) insertMatchup( summoner_id, match.pChampId, match.oChampId, 1, 1, match.kills, match.deaths, match.assists, match.cs );
                else insertMatchup( summoner_id, match.pChampId, match.oChampId, 0, 1, match.kills, match.deaths, match.assists, match.cs );
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "SELECT matchup_id FROM lolmatchups.player_matchup WHERE lolmatchups.player_matchup.summoner_id = @sid AND lolmatchups.player_matchup.player_champion_id = @pid AND lolmatchups.player_matchup.opponent_champion_id = @oid LIMIT 1";
                cmd.Parameters.Add("@sid", MySqlDbType.Int32);
                cmd.Parameters["@sid"].Value = summoner_id;
                cmd.Parameters.Add("@pid", MySqlDbType.Int32);
                cmd.Parameters["@pid"].Value = match.pChampId;
                cmd.Parameters.Add("@oid", MySqlDbType.Int32);
                cmd.Parameters["@oid"].Value = match.oChampId;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                try { connection.Open(); }
                catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

                MySqlDataReader matchup_reader = cmd.ExecuteReader();
                matchup_reader.Read();
                int matchup_id = matchup_reader.GetInt32( 0 );
                connection.Close();
                
                if( match.won ){
                    insertMastery( matchup_id, match.offense, match.defense, match.utility, 1, 1, true );
                    insertRunes( matchup_id, match.runes, 1, 1, true );
                    insertItems( matchup_id, match.items, 1, 1, true );
                    insertSS( matchup_id, match.ss, 1, 1, true );
                }
                else{
                    insertMastery( matchup_id, match.offense, match.defense, match.utility, 0, 1, true );
                    insertRunes( matchup_id, match.runes, 0, 1, true );
                    insertItems( matchup_id, match.items, 0, 1, true );
                    insertSS( matchup_id, match.ss, 1, 1, true );
                }
            }
        }
    }

    protected void updateRunes(PlayerReturnData match, int summoner_id, int matchup_id) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM player_rune_set WHERE player_rune_set.matchup_id=@mid ";
        for (int i = 1; i < match.runes.Count+1; i++) { 
            cmd.CommandText += "AND rune_id" + i.ToString() + "=@r" + i.ToString() + " ";
            cmd.Parameters.Add("@r"+i.ToString(), MySqlDbType.Int32);
            cmd.Parameters["@r"+i.ToString()].Value = match.runes[i-1];
        }
        for (int i = match.runes.Count; i < 30; i++) { 
            cmd.CommandText += "AND rune_id" + i.ToString() + " IS NULL ";
        }
        cmd.CommandText += "LIMIT 1;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows) {
            //mastery exists, altering
            reader.Read();
            int p_runes_id = reader.GetInt32( 0 );
            int won = reader.GetInt32( 31 );
            int played = reader.GetInt32( 32 );
            connection.Close();
            reader.Close();

            if( match.won ) won++;
            played++;

            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE player_rune_set SET won=@won,used=@played WHERE player_rune_set.player_rune_set_id=@pruneid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played;
            cmd.Parameters.Add("@pruneid", MySqlDbType.Int32);
            cmd.Parameters["@pruneid"].Value = p_runes_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_rune_set WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pruneid = reader.GetInt32( 1 ); 
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_rune_set WHERE player_rune_set_id=@pruneid;";
            cmd.Parameters.Add("@pruneid", MySqlDbType.Int32);
            cmd.Parameters["@pruneid"].Value = best_pruneid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 31 );
            int best_played = reader.GetInt32( 32 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_rune_set SET player_rune_set_id=@pruneid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pruneid",MySqlDbType.Int32);
                cmd.Parameters["@pruneid"].Value = p_runes_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        } else { 
            //create new ss, check if its better than the best
            int won = 0;
            int played = 1;
            if( match.won ) won = 1;
            insertRunes( matchup_id, match.runes, won, played, false );

            //get this player_rune_set_id and check if its better than the current best
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_rune_set WHERE player_rune_set.matchup_id=@mid ";
            for (int i = 1; i < match.runes.Count+1; i++) { 
                cmd.CommandText += "AND rune_id" + i.ToString() + "=@r" + i.ToString() + " ";
                cmd.Parameters.Add("@r"+i.ToString(), MySqlDbType.Int32);
                cmd.Parameters["@r"+i.ToString()].Value = match.runes[i-1];
            }
            for (int i = match.runes.Count; i < 30; i++) { 
                cmd.CommandText += "AND rune_id" + i.ToString() + " IS NULL ";
            }
            cmd.CommandText += "LIMIT 1;";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int p_runes_id = reader.GetInt32( 0 );
            reader.Close();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_rune_set WHERE matchup_id=@mid LIMIT 1;";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pruneid = reader.GetInt32( 1 );
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_rune_set WHERE player_rune_set_id=@pruneid;";
            cmd.Parameters.Add("@pruneid", MySqlDbType.Int32);
            cmd.Parameters["@pruneid"].Value = best_pruneid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 31 );
            int best_played = reader.GetInt32( 32 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_rune_set SET player_rune_set_id=@pruneid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pruneid",MySqlDbType.Int32);
                cmd.Parameters["@pruneid"].Value = p_runes_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    protected void updateItems(PlayerReturnData match, int summoner_id, int matchup_id) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM player_items WHERE player_items.matchup_id=@mid ";
        if (match.items[0] != 0) { 
            cmd.CommandText += "AND item_id1=@id1 ";
            cmd.Parameters.Add("@id1", MySqlDbType.Int32);
            cmd.Parameters["@id1"].Value = match.items[0];
        } else { 
            cmd.CommandText += "AND item_id1 IS NULL ";
        }
        if (match.items[1] != 0) { 
            cmd.CommandText += "AND item_id2=@id2 ";
            cmd.Parameters.Add("@id2", MySqlDbType.Int32);
            cmd.Parameters["@id2"].Value = match.items[1];
        } else { 
            cmd.CommandText += "AND item_id2 IS NULL ";
        }
        if (match.items[2] != 0) { 
            cmd.CommandText += "AND item_id3=@id3 ";
            cmd.Parameters.Add("@id3", MySqlDbType.Int32);
            cmd.Parameters["@id3"].Value = match.items[2];
        } else { 
            cmd.CommandText += "AND item_id3 IS NULL ";
        }
        if (match.items[3] != 0) { 
            cmd.CommandText += "AND item_id4=@id4 ";
            cmd.Parameters.Add("@id4", MySqlDbType.Int32);
            cmd.Parameters["@id4"].Value = match.items[3];
        } else { 
            cmd.CommandText += "AND item_id4 IS NULL ";
        }
        if (match.items[4] != 0) { 
            cmd.CommandText += "AND item_id5=@id5 ";
            cmd.Parameters.Add("@id5", MySqlDbType.Int32);
            cmd.Parameters["@id5"].Value = match.items[4];
        } else { 
            cmd.CommandText += "AND item_id5 IS NULL ";
        }
        if (match.items[5] != 0) { 
            cmd.CommandText += "AND item_id6=@id6 ";
            cmd.Parameters.Add("@id6", MySqlDbType.Int32);
            cmd.Parameters["@id6"].Value = match.items[5];
        } else { 
            cmd.CommandText += "AND item_id6 IS NULL ";
        }
        if (match.items[6] != 0) { 
            cmd.CommandText += "AND item_id7=@id7 ";
            cmd.Parameters.Add("@id7", MySqlDbType.Int32);
            cmd.Parameters["@id7"].Value = match.items[6];
        } else { 
            cmd.CommandText += "AND item_id7 IS NULL ";
        }

        cmd.CommandText += "LIMIT 1;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows) {
            //mastery exists, altering
            reader.Read();
            int p_items_id = reader.GetInt32( 0 );
            int won = reader.GetInt32( 9 );
            int played = reader.GetInt32( 10 );
            connection.Close();
            reader.Close();

            if( match.won ) won++;
            played++;

            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE player_items SET won=@won,used=@played WHERE player_items.player_items_id=@pitemid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played;
            cmd.Parameters.Add("@pitemid", MySqlDbType.Int32);
            cmd.Parameters["@pitemid"].Value = p_items_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_items WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pitemid = reader.GetInt32( 1 ); 
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_items WHERE player_items_id=@pitemid;";
            cmd.Parameters.Add("@pitemid", MySqlDbType.Int32);
            cmd.Parameters["@pitemid"].Value = best_pitemid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 9 );
            int best_played = reader.GetInt32( 10 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_items SET player_items_id=@pitemid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pitemid",MySqlDbType.Int32);
                cmd.Parameters["@pitemid"].Value = p_items_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        } else { 
            //create new ss, check if its better than the best
            int won = 0;
            int played = 1;
            if( match.won ) won = 1;
            insertItems( matchup_id, match.items, won, played, false );

            //get this player_rune_set_id and check if its better than the current best
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_items WHERE player_items.matchup_id=@mid ";
            if (match.items[0] != 0) { 
                cmd.CommandText += "AND item_id1=@id1 ";
                cmd.Parameters.Add("@id1", MySqlDbType.Int32);
                cmd.Parameters["@id1"].Value = match.items[0];
            } else { 
                cmd.CommandText += "AND item_id1 IS NULL ";
            }
            if (match.items[1] != 0) { 
                cmd.CommandText += "AND item_id2=@id2 ";
                cmd.Parameters.Add("@id2", MySqlDbType.Int32);
                cmd.Parameters["@id2"].Value = match.items[1];
            } else { 
                cmd.CommandText += "AND item_id2 IS NULL ";
            }
            if (match.items[2] != 0) { 
                cmd.CommandText += "AND item_id3=@id3 ";
                cmd.Parameters.Add("@id3", MySqlDbType.Int32);
                cmd.Parameters["@id3"].Value = match.items[2];
            } else { 
                cmd.CommandText += "AND item_id3 IS NULL ";
            }
            if (match.items[3] != 0) { 
                cmd.CommandText += "AND item_id4=@id4 ";
                cmd.Parameters.Add("@id4", MySqlDbType.Int32);
                cmd.Parameters["@id4"].Value = match.items[3];
            } else { 
                cmd.CommandText += "AND item_id4 IS NULL ";
            }
            if (match.items[4] != 0) { 
                cmd.CommandText += "AND item_id5=@id5 ";
                cmd.Parameters.Add("@id5", MySqlDbType.Int32);
                cmd.Parameters["@id5"].Value = match.items[4];
            } else { 
                cmd.CommandText += "AND item_id5 IS NULL ";
            }
            if (match.items[5] != 0) { 
                cmd.CommandText += "AND item_id6=@id6 ";
                cmd.Parameters.Add("@id6", MySqlDbType.Int32);
                cmd.Parameters["@id6"].Value = match.items[5];
            } else { 
                cmd.CommandText += "AND item_id6 IS NULL ";
            }
            if (match.items[6] != 0) { 
                cmd.CommandText += "AND item_id7=@id7 ";
                cmd.Parameters.Add("@id7", MySqlDbType.Int32);
                cmd.Parameters["@id7"].Value = match.items[6];
            } else { 
                cmd.CommandText += "AND item_id7 IS NULL ";
            }
            cmd.CommandText += "LIMIT 1;";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int p_items_id = reader.GetInt32( 0 );
            reader.Close();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_items WHERE matchup_id=@mid LIMIT 1;";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pitemid = reader.GetInt32( 1 );
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_items WHERE player_items_id=@pitemid;";
            cmd.Parameters.Add("@pitemid", MySqlDbType.Int32);
            cmd.Parameters["@pitemid"].Value = best_pitemid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 9 );
            int best_played = reader.GetInt32( 10 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_items SET player_items_id=@pitemid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pitemid",MySqlDbType.Int32);
                cmd.Parameters["@pitemid"].Value = p_items_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    protected void updateMasteries(PlayerReturnData match, int summoner_id, int matchup_id) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM player_mastery WHERE player_mastery.matchup_id=@mid AND player_mastery.offense_values=@off AND player_mastery.defense_values=@def AND player_mastery.utility_values=@util LIMIT 1";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.Parameters.Add("@off", MySqlDbType.VarChar);
        cmd.Parameters["@off"].Value = match.offense;
        cmd.Parameters.Add("@def", MySqlDbType.VarChar);
        cmd.Parameters["@def"].Value = match.defense;
        cmd.Parameters.Add("@util", MySqlDbType.VarChar);
        cmd.Parameters["@util"].Value = match.utility;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows) {
            //mastery exists, altering
            reader.Read();
            int p_mastery_id = reader.GetInt32( 0 );
            int won = reader.GetInt32( 5 );
            int played = reader.GetInt32( 6 );
            connection.Close();
            reader.Close();

            if( match.won ) won++;
            played++;

            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE player_mastery SET won=@won,used=@played WHERE player_mastery.player_mastery_id=@pmastid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played;
            cmd.Parameters.Add("@pmastid", MySqlDbType.Int32);
            cmd.Parameters["@pmastid"].Value = p_mastery_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_masteries WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pmastid = reader.GetInt32( 1 ); 
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_mastery WHERE player_mastery_id=@pmastid;";
            cmd.Parameters.Add("@pmastid", MySqlDbType.Int32);
            cmd.Parameters["@pmastid"].Value = best_pmastid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 5 );
            int best_played = reader.GetInt32( 6 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_masteries SET player_mastery_id=@pmastid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pmastid",MySqlDbType.Int32);
                cmd.Parameters["@pmastid"].Value = p_mastery_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        } else { 
            //create new ss, check if its better than the best
            int won = 0;
            int played = 1;
            if( match.won ) won = 1;
            insertMastery( matchup_id, match.offense, match.defense, match.utility, won, played, false );

            //get this player_summoner_spell_id and check if its better than the current best
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM player_mastery WHERE player_mastery.matchup_id=@mid AND player_mastery.offense_values=@off AND player_mastery.defense_values=@def AND player_mastery.utility_values=@util LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@off", MySqlDbType.VarChar);
            cmd.Parameters["@off"].Value = match.offense;
            cmd.Parameters.Add("@def", MySqlDbType.VarChar);
            cmd.Parameters["@def"].Value = match.defense;
            cmd.Parameters.Add("@util", MySqlDbType.VarChar);
            cmd.Parameters["@util"].Value = match.utility;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int p_mastery_id = reader.GetInt32( 0 );
            reader.Close();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_masteries WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pmastid = reader.GetInt32( 1 );
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_mastery WHERE player_mastery_id=@pmastid;";
            cmd.Parameters.Add("@pmastid", MySqlDbType.Int32);
            cmd.Parameters["@pmastid"].Value = best_pmastid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 5 );
            int best_played = reader.GetInt32( 6 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_masteries SET player_mastery_id=@pmastid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pmastid",MySqlDbType.Int32);
                cmd.Parameters["@pmastid"].Value = p_mastery_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    protected void updateSS(PlayerReturnData match, int summoner_id, int matchup_id) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM lolmatchups.player_summoner_spells WHERE player_summoner_spells.matchup_id=@mid AND player_summoner_spells.ss_id1=@ssid1 AND player_summoner_spells.ss_id2=@ssid2 LIMIT 1";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.Parameters.Add("@ssid1", MySqlDbType.Int32);
        cmd.Parameters["@ssid1"].Value = match.ss[0];
        cmd.Parameters.Add("@ssid2", MySqlDbType.Int32);
        cmd.Parameters["@ssid2"].Value = match.ss[1];
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
            
        if (reader.HasRows) {
            //ss exists, altering
            reader.Read();
            int p_ss_id = reader.GetInt32( 0 );
            int won = reader.GetInt32( 4 );
            int played = reader.GetInt32( 5 );
            connection.Close();
            reader.Close();

            if( match.won ) won++;
            played++;

            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE lolmatchups.player_summoner_spells SET won=@won,used=@played WHERE lolmatchups.player_summoner_spells.player_summoner_spells_id=@pssid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played;
            cmd.Parameters.Add("@pssid", MySqlDbType.Int32);
            cmd.Parameters["@pssid"].Value = p_ss_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_ss WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pssid = reader.GetInt32( 1 );
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_summoner_spells WHERE player_summoner_spells_id=@pssid;";
            cmd.Parameters.Add("@pssid", MySqlDbType.Int32);
            cmd.Parameters["@pssid"].Value = best_pssid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 4 );
            int best_played = reader.GetInt32( 5 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_ss SET player_summoner_spell_id=@pssid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pssid",MySqlDbType.Int32);
                cmd.Parameters["@pssid"].Value = p_ss_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        } else { 
            //create new ss, check if its better than the best
            int won = 0;
            int played = 1;
            if( match.won ) won = 1;
            insertSS( matchup_id, match.ss, won, played, false );

            //get this player_summoner_spell_id and check if its better than the current best
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_summoner_spells WHERE player_summoner_spells.matchup_id=@mid AND player_summoner_spells.ss_id1=@ssid1 AND player_summoner_spells.ss_id2=@ssid2 LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@ssid1", MySqlDbType.Int32);
            cmd.Parameters["@ssid1"].Value = match.ss[0];
            cmd.Parameters.Add("@ssid2", MySqlDbType.Int32);
            cmd.Parameters["@ssid2"].Value = match.ss[1];
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int p_ss_id = reader.GetInt32( 0 );
            reader.Close();
            connection.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.best_matchup_ss WHERE matchup_id=@mid LIMIT 1";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_pssid = reader.GetInt32( 1 );
            connection.Close();
            reader.Close();

            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "SELECT * FROM lolmatchups.player_summoner_spells WHERE player_summoner_spells_id=@pssid;";
            cmd.Parameters.Add("@pssid", MySqlDbType.Int32);
            cmd.Parameters["@pssid"].Value = best_pssid;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try { connection.Open(); }
            catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
            reader = cmd.ExecuteReader();
            reader.Read();
            int best_won = reader.GetInt32( 4 );
            int best_played = reader.GetInt32( 5 );
            connection.Close();
            reader.Close();

            if (won / played > best_won / best_played) { 
                cmd = new MySqlCommand();
                connection = connectToServer();
                cmd.CommandText = "UPDATE best_matchup_ss SET player_summoner_spell_id=@pssid WHERE matchup_id=@mid;";
                cmd.Parameters.Add("@pssid",MySqlDbType.Int32);
                cmd.Parameters["@pssid"].Value = p_ss_id;
                cmd.Parameters.Add("@mid",MySqlDbType.Int32);
                cmd.Parameters["@mid"].Value = matchup_id;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    protected void updateChampStats(PlayerReturnData match, int summoner_id) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM lolmatchups.player_champion_stat WHERE lolmatchups.player_champion_stat.summoner_id = @sid AND lolmatchups.player_champion_stat.champion_id=@pid LIMIT 1;";
        cmd.Parameters.Add("@sid", MySqlDbType.Int32);
        cmd.Parameters["@sid"].Value = summoner_id;
        cmd.Parameters.Add("@pid", MySqlDbType.Int32);
        cmd.Parameters["@pid"].Value = match.pChampId;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
            
        if (reader.HasRows) {
            //champ_stat exists, altering
            reader.Read();
            int played_as_won = reader.GetInt32( 2 );
            int played_as_total = reader.GetInt32( 3 );
            connection.Close();
            reader.Close();

            if( match.won ) played_as_won++;
            played_as_total++;
            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE lolmatchups.player_champion_stat SET played_as_won=@won,played_as_total=@played WHERE lolmatchups.player_champion_stat.summoner_id = @sid AND lolmatchups.player_champion_stat.champion_id = @pid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = played_as_won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played_as_total;
            cmd.Parameters.Add("@sid", MySqlDbType.Int32);
            cmd.Parameters["@sid"].Value = summoner_id;
            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = match.pChampId;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        } else {
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO player_champion_stat(summoner_id,champion_id,played_as_won,played_as_total,played_against_won,played_against_total) VALUES (@sid,@cid,@aswon,1,0,0);";
            cmd.Parameters.Add("@sid", MySqlDbType.Int32);
            cmd.Parameters["@sid"].Value = summoner_id;
            cmd.Parameters.Add("@cid", MySqlDbType.Int32);
            cmd.Parameters["@cid"].Value = match.pChampId;
            cmd.Parameters.Add("@aswon", MySqlDbType.Int32);
            if( match.won ) cmd.Parameters["@aswon"].Value = 1;
            else cmd.Parameters["@aswon"].Value = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        //enemy champion
        cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT * FROM lolmatchups.player_champion_stat WHERE lolmatchups.player_champion_stat.summoner_id = @sid AND lolmatchups.player_champion_stat.champion_id=@oid LIMIT 1;";
        cmd.Parameters.Add("@sid", MySqlDbType.Int32);
        cmd.Parameters["@sid"].Value = summoner_id;
        cmd.Parameters.Add("@oid", MySqlDbType.Int32);
        cmd.Parameters["@oid"].Value = match.oChampId;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        reader = cmd.ExecuteReader();

        if (reader.HasRows) { 
            //champ_stat exists, altering
            reader.Read();
            int played_against_won = reader.GetInt32( 4 );
            int played_against_total = reader.GetInt32( 5 );
            connection.Close();
            reader.Close();
             
            if( match.won ) played_against_won++;
            played_against_total++;
            //update row
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "UPDATE lolmatchups.player_champion_stat SET played_against_won=@won,played_against_total=@played WHERE lolmatchups.player_champion_stat.summoner_id = @sid AND lolmatchups.player_champion_stat.champion_id = @oid";
            cmd.Parameters.Add("@won", MySqlDbType.Int32);
            cmd.Parameters["@won"].Value = played_against_won;
            cmd.Parameters.Add("@played", MySqlDbType.Int32);
            cmd.Parameters["@played"].Value = played_against_total;
            cmd.Parameters.Add("@sid", MySqlDbType.Int32);
            cmd.Parameters["@sid"].Value = summoner_id;
            cmd.Parameters.Add("@oid", MySqlDbType.Int32);
            cmd.Parameters["@oid"].Value = match.oChampId;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        } else { 
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO player_champion_stat(summoner_id,champion_id,played_as_won,played_as_total,played_against_won,played_against_total) VALUES (@sid,@cid,0,0,@agwon,1);";
            cmd.Parameters.Add("@sid", MySqlDbType.Int32);
            cmd.Parameters["@sid"].Value = summoner_id;
            cmd.Parameters.Add("@cid", MySqlDbType.Int32);
            cmd.Parameters["@cid"].Value = match.oChampId;
            cmd.Parameters.Add("@agwon", MySqlDbType.Int32);
            if( match.won ) cmd.Parameters["@agwon"].Value = 1;
            else cmd.Parameters["@agwon"].Value = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    protected void insertSS( int matchup_id, List<int> ss, int won, int played, bool fresh){
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "INSERT INTO lolmatchups.player_summoner_spells(matchup_id, ss_id1, ss_id2, won, used) VALUES (@mid,@id0,@id1,@won,@played);";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.Parameters.Add("@id0",MySqlDbType.Int32);
        cmd.Parameters["@id0"].Value = ss[0];
        cmd.Parameters.Add("@id1",MySqlDbType.Int32);
        cmd.Parameters["@id1"].Value = ss[1];
        cmd.Parameters.Add("@won",MySqlDbType.Int32);
        cmd.Parameters["@won"].Value = won;
        cmd.Parameters.Add("@played",MySqlDbType.Int32);
        cmd.Parameters["@played"].Value = played;
        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();

        cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT player_summoner_spells_id FROM player_summoner_spells WHERE matchup_id = @mid;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int p_ss_id = reader.GetInt32( 0 );
        connection.Close();

        if (fresh) {
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO best_matchup_ss(matchup_id,player_summoner_spells_id) VALUES (@mid,@ssid);";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@ssid", MySqlDbType.Int32);
            cmd.Parameters["@ssid"].Value = p_ss_id;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    protected void insertItems(int matchup_id, List<int> items, int won, int played, bool fresh) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "INSERT INTO lolmatchups.player_items(matchup_id, item_id1, item_id2, item_id3, item_id4, item_id5, item_id6, item_id7, won, used) VALUES (@mid,@id0,@id1,@id2,@id3,@id4,@id5,@id6,@won,@played);";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.Parameters.Add("@id0",MySqlDbType.Int32);
        if( items[0] != 0 ) cmd.Parameters["@id0"].Value = items[0];
        else cmd.Parameters["@id0"].Value = null;
        cmd.Parameters.Add("@id1",MySqlDbType.Int32);
        if( items[1] != 0 ) cmd.Parameters["@id1"].Value = items[1];
        else cmd.Parameters["@id1"].Value = null;
        cmd.Parameters.Add("@id2",MySqlDbType.Int32);
        if( items[2] != 0 ) cmd.Parameters["@id2"].Value = items[2];
        else cmd.Parameters["@id2"].Value = null;
        cmd.Parameters.Add("@id3",MySqlDbType.Int32);
        if( items[3] != 0 ) cmd.Parameters["@id3"].Value = items[3];
        else cmd.Parameters["@id3"].Value = null;
        cmd.Parameters.Add("@id4",MySqlDbType.Int32);
        if( items[4] != 0 ) cmd.Parameters["@id4"].Value = items[4];
        else cmd.Parameters["@id4"].Value = null;
        cmd.Parameters.Add("@id5",MySqlDbType.Int32);
        if( items[5] != 0 ) cmd.Parameters["@id5"].Value = items[5];
        else cmd.Parameters["@id5"].Value = null;
        cmd.Parameters.Add("@id6",MySqlDbType.Int32);
        if( items[6] != 0 ) cmd.Parameters["@id6"].Value = items[6];
        else cmd.Parameters["@id6"].Value = null;
        cmd.Parameters.Add("@won",MySqlDbType.Int32);
        cmd.Parameters["@won"].Value = won;
        cmd.Parameters.Add("@played",MySqlDbType.Int32);
        cmd.Parameters["@played"].Value = played;
        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();

        cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT player_items_id FROM player_items WHERE matchup_id = @mid;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int p_items_id = reader.GetInt32( 0 );
        connection.Close();

        if (fresh) { 
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO best_matchup_items(matchup_id,player_items_id) VALUES (@mid,@itemsid);";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@itemsid", MySqlDbType.Int32);
            cmd.Parameters["@itemsid"].Value = p_items_id;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    protected void insertMatchup(int summoner_id, int pChampId, int oChampId, int won, int played, int kills, int deaths, int assists, int cs) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "INSERT INTO lolmatchups.player_matchup(summoner_id, player_champion_id, opponent_champion_id, won, played, kills, deaths, assists, cumulative_creep_score) VALUES (@sid,@pid,@oid,@won,@played,@kills,@deaths,@assists,@cs);";
        cmd.Parameters.Add("@sid", MySqlDbType.Int32);
        cmd.Parameters["@sid"].Value = summoner_id;
        cmd.Parameters.Add("@pid",MySqlDbType.Int32);
        cmd.Parameters["@pid"].Value = pChampId;
        cmd.Parameters.Add("@oid",MySqlDbType.Int32);
        cmd.Parameters["@oid"].Value = oChampId;
        cmd.Parameters.Add("@won",MySqlDbType.Int32);
        cmd.Parameters["@won"].Value = won;
        cmd.Parameters.Add("@played",MySqlDbType.Int32);
        cmd.Parameters["@played"].Value = played;
        cmd.Parameters.Add("@kills",MySqlDbType.Int32);
        cmd.Parameters["@kills"].Value = kills;
        cmd.Parameters.Add("@deaths",MySqlDbType.Int32);
        cmd.Parameters["@deaths"].Value = deaths;
        cmd.Parameters.Add("@assists",MySqlDbType.Int32);
        cmd.Parameters["@assists"].Value = assists;
        cmd.Parameters.Add("@cs",MySqlDbType.Int32);
        cmd.Parameters["@cs"].Value = cs;
        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();
    }

    protected void insertMastery( int matchup_id, string offense, string defense, string utility, int won, int played, bool fresh ){
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "INSERT INTO lolmatchups.player_mastery(matchup_id, offense_values, defense_values, utility_values, won, used) VALUES (@mid,@off,@def,@util,@won,@played);";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.Parameters.Add("@off",MySqlDbType.VarChar);
        cmd.Parameters["@off"].Value = offense;
        cmd.Parameters.Add("@def",MySqlDbType.VarChar);
        cmd.Parameters["@def"].Value = defense;
        cmd.Parameters.Add("@util",MySqlDbType.VarChar);
        cmd.Parameters["@util"].Value = utility;
        cmd.Parameters.Add("@won",MySqlDbType.Int32);
        cmd.Parameters["@won"].Value = won;
        cmd.Parameters.Add("@played",MySqlDbType.Int32);
        cmd.Parameters["@played"].Value = played;
        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();

        cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT player_mastery_id FROM player_mastery WHERE matchup_id = @mid;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int p_mastery_id = reader.GetInt32( 0 );
        connection.Close();
        reader.Close();

        if (fresh) { 
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO best_matchup_masteries(matchup_id,player_mastery_id) VALUES (@mid,@mastid);";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@mastid", MySqlDbType.Int32);
            cmd.Parameters["@mastid"].Value = p_mastery_id;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    protected void insertRunes(int matchup_id, List<int> runes, int won, int played, bool fresh) { 
        MySqlCommand cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "INSERT INTO lolmatchups.player_rune_set(matchup_id,rune_id1,rune_id2,rune_id3,rune_id4,rune_id5,rune_id6,rune_id7,rune_id8,rune_id9,rune_id10,rune_id11,rune_id12,rune_id13,rune_id14,rune_id15,rune_id16,rune_id17,rune_id18,rune_id19,rune_id20,rune_id21,rune_id22,rune_id23,rune_id24,rune_id25,rune_id26,rune_id27,rune_id28,rune_id29,rune_id30,won,used) VALUES (@mid,@r1,@r2,@r3,@r4,@r5,@r6,@r7,@r8,@r9,@r10,@r11,@r12,@r13,@r14,@r15,@r16,@r17,@r18,@r19,@r20,@r21,@r22,@r23,@r24,@r25,@r26,@r27,@r28,@r29,@r30,@won,@played);";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        for (int i = 1; i < runes.Count+1; i++) { 
            cmd.Parameters.Add("@r"+i.ToString(), MySqlDbType.Int32);
            cmd.Parameters["@r"+i.ToString()].Value = runes[i-1];
        }
        for (int i = runes.Count; i < 30; i++) { 
            cmd.Parameters.Add("@r"+i.ToString(), MySqlDbType.Int32);
            cmd.Parameters["@r"+i.ToString()].Value = null;
        }
        cmd.Parameters.Add("@won",MySqlDbType.Int32);
        cmd.Parameters["@won"].Value = won;
        cmd.Parameters.Add("@played",MySqlDbType.Int32);
        cmd.Parameters["@played"].Value = played;
        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();

        cmd = new MySqlCommand();
        connection = connectToServer();
        cmd.CommandText = "SELECT player_rune_set_id FROM player_rune_set WHERE matchup_id = @mid;";
        cmd.Parameters.Add("@mid", MySqlDbType.Int32);
        cmd.Parameters["@mid"].Value = matchup_id;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }
        MySqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int p_runes_id = reader.GetInt32( 0 );
        connection.Close();

        if (fresh) { 
            cmd = new MySqlCommand();
            connection = connectToServer();
            cmd.CommandText = "INSERT INTO best_matchup_rune_set(matchup_id,player_rune_set_id) VALUES (@mid,@rsid);";
            cmd.Parameters.Add("@mid", MySqlDbType.Int32);
            cmd.Parameters["@mid"].Value = matchup_id;
            cmd.Parameters.Add("@rsid", MySqlDbType.Int32);
            cmd.Parameters["@rsid"].Value = p_runes_id;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    //START DataContract for matchhistory query
    [DataContract]
    public class MatchHistoryResponse {
        [DataMember]
        public Match[] matches{ get; set; }
    }

    [DataContract]
    public class Match { 
        [DataMember]
        public string matchVersion { get; set; }
        [DataMember]
        public string region { get; set; }
        [DataMember]
        public int mapId { get; set; }
        [DataMember]
        public string season { get; set; }
        [DataMember]
        public string queueType { get; set; }
        [DataMember]
        public int matchDuration { get; set; }
        [DataMember]
        public long matchCreation { get; set; }
        [DataMember]
        public string matchType { get; set; }
        [DataMember]
        public int matchId { get; set; }
        [DataMember]
        public Participant[] participants { get; set; }
        [DataMember]
        public string matchMode { get; set; }
        [DataMember]
        public string platformId { get; set; }
        [DataMember]
        public ParticipantIdentity[] participantIdentities { get; set; }
        [DataMember]
        public int participantId { get; set; }
    }

    [DataContract]
    public class Participant { 
        [DataMember]
        public Mastery[] masteries { get; set; }
        [DataMember]
        public Stats stats { get; set; }
        [DataMember]
        public Rune[] runes { get; set; }
        [DataMember]
        public Timeline timeline { get; set; }
        [DataMember]
        public int spell2Id { get; set; }
        [DataMember]
        public int participantId { get; set; }
        [DataMember]
        public int championId { get; set; }
        [DataMember]
        public int teamId { get; set; }
        [DataMember]
        public string highestAchievedSeasonTier { get; set; }
        [DataMember]
        public int spell1Id { get; set; }
    }

    [DataContract]
    public class Mastery { 
        [DataMember]
        public int rank { get; set; }
        [DataMember]
        public int masteryId { get; set; }
    }

    [DataContract]
    public class Stats { 
        [DataMember]
        public int unrealKills { get; set; }
        [DataMember]
        public int item2 { get; set; }
        [DataMember]
        public int item1 { get; set; }
        [DataMember]
        public int totalDamageTaken { get; set; }
        [DataMember]
        public int item0 { get; set; }
        [DataMember]
        public int pentaKills { get; set; }
        [DataMember]
        public int sightWardsBoughtInGame { get; set; }
        [DataMember]
        public Boolean winner { get; set; }
        [DataMember]
        public int magicDamageDealt { get; set; }
        [DataMember]
        public int wardsKilled { get; set; }
        [DataMember]
        public int largestCriticalStrike { get; set; }
        [DataMember]
        public int trueDamageDealt { get; set; }
        [DataMember]
        public int doubleKills { get; set; }
        [DataMember]
        public int physicalDamageDealt { get; set; }
        [DataMember]
        public int tripleKills { get; set; }
        [DataMember]
        public int deaths { get; set; }
        [DataMember]
        public Boolean firstBloodAssist { get; set; }
        [DataMember]
        public int magicDamageDealtToChampions { get; set; }
        [DataMember]
        public int assists { get; set; }
        [DataMember]
        public int visionWardsBoughtInGame { get; set; }
        [DataMember]
        public int totalTimeCrowdControlDealt { get; set; }
        [DataMember]
        public int champLevel { get; set; }
        [DataMember]
        public int physicalDamageTaken { get; set; }
        [DataMember]
        public int totalDamageDealt { get; set; }
        [DataMember]
        public int largestKillingSpree { get; set; }
        [DataMember]
        public int inhibitorKills { get; set; }
        [DataMember]
        public int minionsKilled { get; set; }
        [DataMember]
        public int towerKills { get; set; }
        [DataMember]
        public int physicalDamageDealtToChampions { get; set; }
        [DataMember]
        public int quadraKills { get; set; }
        [DataMember]
        public int goldSpent { get; set; }
        [DataMember]
        public int totalDamageDealtToChampions { get; set; }
        [DataMember]
        public int goldEarned { get; set; }
        [DataMember]
        public int neutralMinionsKilledTeamJungle { get; set; }
        [DataMember]
        public Boolean firstBloodKill { get; set; }
        [DataMember]
        public Boolean firstTowerKill { get; set; }
        [DataMember]
        public int wardsPlaced { get; set; }
        [DataMember]
        public int trueDamageDealtToChampions { get; set; }
        [DataMember]
        public int killingSprees { get; set; }
        [DataMember]
        public Boolean firstInhibitorKill { get; set; }
        [DataMember]
        public int totalScoreRank { get; set; }
        [DataMember]
        public int totalUnitsHealed { get; set; }
        [DataMember]
        public int kills { get; set; }
        [DataMember]
        public Boolean firstInhibitorAssist { get; set; }
        [DataMember]
        public int totalPlayerScore { get; set; }
        [DataMember]
        public int neutralMinionsKilledEnemyJungle { get; set; }
        [DataMember]
        public int magicDamageTaken { get; set; }
        [DataMember]
        public int largestMultiKill { get; set; }
        [DataMember]
        public int totalHeal { get; set; }
        [DataMember]
        public int item4 { get; set; }
        [DataMember]
        public int item3 { get; set; }
        [DataMember]
        public int objectivePlayerScore { get; set; }
        [DataMember]
        public int item6 { get; set; }
        [DataMember]
        public Boolean firstTowerAssist { get; set; }
        [DataMember]
        public int item5 { get; set; }
        [DataMember]
        public int trueDamageTaken { get; set; }
        [DataMember]
        public int neutralMinionsKilled { get; set; }
        [DataMember]
        public int combatPlayerScore { get; set; }
    }

    [DataContract]
    public class Rune { 
        [DataMember]
        public int rank { get; set; }
        [DataMember]
        public int runeId { get; set; }
    }

    [DataContract]
    public class Timeline { 
        [DataMember]
        public string lane { get; set; }
        [DataMember]
        public string role { get; set; }
    }

    [DataContract]
    public class ParticipantIdentity { 
        [DataMember]
        public Player player { get; set; }
        [DataMember]
        public int participantId { get; set; }
    }

    [DataContract]
    public class Player { 
        [DataMember]
        public int profileIcon { get; set; }
        [DataMember]
        public string matchHistoryUri { get; set; }
        [DataMember]
        public string summonerName { get; set; }
        [DataMember]
        public int summonerId { get; set; }
    }
    //END DataContract for matchhistory query

    //START Classes for summoner ID query
    [DataContract]
    public class SummonerInfo { 
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int profileIconId { get; set; }
        [DataMember]
        public long revisionDate { get; set; }
        [DataMember]
        public int summonerLevel { get; set; }
    }
    //END DataContract for summoner ID query

    //START DataContracts for League query
    [DataContract]
    public class League { 
        [DataMember]
        public string queue { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Entry[] entries { get; set; }
        [DataMember]
        public string tier {  get; set; }
    }

    [DataContract]
    public class Entry { 
        [DataMember]
        public int leaguePoints { get; set; }
        [DataMember]
        public Boolean isFreshBlood { get; set; }
        [DataMember]
        public Boolean isHotStreak { get; set; }
        [DataMember]
        public string division { get; set; }
        [DataMember]
        public Boolean isInactive { get; set; }
        [DataMember]
        public Boolean isVeteran{ get; set; }
        [DataMember]
        public int losses { get; set; }
        [DataMember]
        public string playerOrTeamName { get; set; }
        [DataMember]
        public string playerOrTeamId { get; set; }
        [DataMember]
        public int wins { get; set; }
    }

    protected class PlayerReturnData { 
        public List<int> ss;
        public List<int> runes;
        public List<int> items;
        public string offense;
        public string defense;
        public string utility;
        public int kills;
        public int deaths;
        public int assists;
        public int cs;
        public int pChampId;
        public int oChampId;
        public bool won;
        public int match_id;
    }
}