import json, random

def main():
    root_file_name = "matches"
    player_file_name = "sample_player.sql"
    player_champion_file_name = "sample_player_champ.sql"
    player_matchup_file_name = "sample_player_matchup.sql"

    players = {}

    rank_prefixes = [ 'B', 'S', 'G', 'P', 'D' ]
    rank_suffixes = [ '1', '2', '3', '4', '5' ]
    
    for i in range( 1, 11 ):
        print( i )
        with open( "../" + root_file_name + str( i ) + ".json" ) as match_file:
            file_contents = match_file.read()
            match_data = json.loads( file_contents )
            for match in match_data["matches"]:
                for participant in match["participants"]:
                    participant_id = participant["participantId"]
                    champion_id = participant["championId"]
                    match_id = match["matchId"]
                    team_id = participant["teamId"]
                    summoner_id = None
                    summoner_name = None
                    kills = participant["stats"]["kills"]
                    deaths = participant["stats"]["deaths"]
                    assists = participant["stats"]["assists"]
                    creep_score = participant["stats"]["minionsKilled"]
                    
                    if match["participantIdentities"][participant_id - 1]["participantId"] == participant_id:
                        summoner_id = match["participantIdentities"][participant_id - 1]["player"]["summonerId"]
                        summoner_name = match["participantIdentities"][participant_id - 1]["player"]["summonerName"]
                        
                    else:
                        #just in case participants isnt ordered correctly
                        for participant_entry in match["participantIdentities"]:
                            if participant_entry["participantId"] == participant_id:
                                summoner_id = participant_entry["player"]["summonerId"]
                                summoner_name = participant_entry["player"]["summonerName"]
                                kills = participant_entry["stats"]["kills"]
                                deaths = participant_entry["stats"]["deaths"]
                                assists = participant_entry["stats"]["assists"]
                                creep_score = participant_entry["stats"]["minionsKilled"]
                                break

                    matchup = findMatchOpponent( match["participants"], summoner_id, participant_id )
                    if matchup == None:
                        continue
                    
                    matchup["won"] = str( match["teams"][int(str(team_id)[0])-1]["winner"] )

                    played_as_won = 0
                    their_champ_against_won = 0
                    if matchup["won"] == "True":
                        played_as_won = 1
                        their_champ_against_won = 1

                    if summoner_id not in players:
                        players[summoner_id] = { "summoner_name" : summoner_name, "last_match_id" : match_id, "champion_info" : {}, "matchups" : {} }
                        
                        players[summoner_id]["champion_info"][matchup["my_champ"]] = { "played_as_won" : played_as_won, "played_as_total" : 1, "played_against_won" : 0, "played_against_total" : 0 }
                        players[summoner_id]["champion_info"][matchup["their_champ"]] = { "played_as_won" : 0, "played_as_total" : 0, "played_against_won" : their_champ_against_won, "played_against_total" : 1 }
                        
                        players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])] = { "won" : played_as_won, "played" : 1, "kills" : kills, "deaths" : deaths, "assists" : assists, "creep_score" : creep_score }
                    else:
                            
                        if matchup["my_champ"] in players[summoner_id]["champion_info"]:
                            players[summoner_id]["champion_info"][matchup["my_champ"]]["played_as_won"] += played_as_won
                            players[summoner_id]["champion_info"][matchup["my_champ"]]["played_as_total"] += 1
                        else:
                            players[summoner_id]["champion_info"][matchup["my_champ"]] = { "played_as_won" : played_as_won, "played_as_total" : 1, "played_against_won" : 0, "played_against_total" : 0 }

                        if matchup["their_champ"] in players[summoner_id]:
                            players[summoner_id]["champion_info"][matchup["their_champ"]]["played_against_won"] += their_champ_against_won
                            players[summoner_id]["champion_info"][matchup["their_champ"]]["played_against_total"] += 1
                        else:
                            players[summoner_id]["champion_info"][matchup["their_champ"]] = { "played_as_won" : 0, "played_as_total" : 0, "played_against_won" : their_champ_against_won, "played_against_total" : 1 }

                        if str(matchup["my_champ"]) + "-" + str(matchup["their_champ"]) in players[summoner_id]["matchups"]:
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["won"] += played_as_won
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["played"] += 1
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["kills"] += kills
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["deaths"] += deaths
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["assists"] += assists
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["creep_score"] += creep_score
                        else:
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])] = { "won" : played_as_won, "played" : 1, "kills" : kills, "deaths" : deaths, "assists" : assists, "creep_score" : creep_score }
                            

                        if match_id > players[summoner_id]["last_match_id"]:
                            players[summoner_id]["last_match_id"] = match_id
    
    player_file = open( player_file_name, 'w' )
    player_champ_file = open( player_champion_file_name, 'w' )
    player_matchup_file = open( player_matchup_file_name, 'w' )

    player_output = "USE lolmatchups;\nINSERT INTO player VALUES "
    player_champ_output = "USE lolmatchups;\nINSERT INTO player_champion_stat VALUES "
    player_matchup_output = "USE lolmatchups;\nINSERT INTO player_matchup VALUES "

    for ( summoner_id, player_info ) in players.items():
        random_rank = random.choice( rank_prefixes ) + random.choice( rank_suffixes )
        player_output += "(" + str( summoner_id ) + ",\"" + player_info["summoner_name"] + "\",\"" + random_rank + "\"," + "30" + "," + str( player_info["last_match_id"] ) + "),\n"
        for ( champion_id, champion_record ) in player_info["champion_info"].items():
            player_champ_output += "(" + str( summoner_id ) + "," + str( champion_id ) + "," + str( champion_record["played_as_won"] ) + "," + str( champion_record["played_as_total"] ) + "," + str( champion_record["played_against_won"] ) + "," + str( champion_record["played_against_total"] ) + "),\n"
        for ( matchup_pair, matchup_info ) in player_info["matchups"].items():
            player_champ_id = matchup_pair[:matchup_pair.index('-')]
            opp_champ_id = matchup_pair[matchup_pair.index('-')+1:]
            player_matchup_output += "(" + str( summoner_id ) + "," + player_champ_id + "," + opp_champ_id + "," + str( matchup_info["won"] ) + "," + str( matchup_info["played"] ) + "," + str( matchup_info["kills"] ) + "," + str( matchup_info["deaths"] ) + "," + str( matchup_info["assists"] ) + "," + str( matchup_info["creep_score"] ) + "),\n" 

    player_output = player_output[:-2] + ";"
    player_champ_output = player_champ_output[:-2] + ";"
    player_matchup_output = player_matchup_output[:-2] + ";"

    player_file.write( player_output )
    player_champ_file.write( player_champ_output )
    player_matchup_file.write( player_matchup_output )




def findMatchOpponent( participants, summoner_id, participant_id ):
    my_participant = None
    for participant in participants:
        if participant_id == participant["participantId"]:
            my_participant = participant

    #print( "Target: " + my_participant["timeline"]["lane"] + " " + my_participant["timeline"]["role"] + " " + str( my_participant["teamId"] ) )

    their_participant = None
    for participant in participants:
        #print( "Candidate: " + participant["timeline"]["lane"] + " " + participant["timeline"]["role"] + " " + str( participant["teamId"] ) )
        if participant["timeline"]["lane"] == my_participant["timeline"]["lane"] and participant["timeline"]["role"] == my_participant["timeline"]["role"] and my_participant["teamId"] != participant["teamId"]:
            their_participant = participant

    if their_participant == None:
        return None

    return_dict = { "my_champ" : my_participant["championId"], "their_champ" : their_participant["championId"] }
    return return_dict
    
    
main()

