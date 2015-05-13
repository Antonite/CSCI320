CREATE DATABASE `lolmatchups` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE lolmatchups;

CREATE TABLE `champion` (
  `champion_id` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `title` varchar(45) NOT NULL,
  `image_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`champion_id`),
  UNIQUE KEY `champion_id_UNIQUE` (`champion_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `item` (
  `item_id` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(900) NOT NULL,
  `child_id1` int(11) DEFAULT NULL,
  `child_id2` int(11) DEFAULT NULL,
  `child_id3` int(11) DEFAULT NULL,
  `child_id4` int(11) DEFAULT NULL,
  `image_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`item_id`),
  UNIQUE KEY `item_id_UNIQUE` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `mastery` (
  `mastery_id` int(10) unsigned NOT NULL,
  `name` varchar(20) NOT NULL,
  `description_rank1` varchar(200) NOT NULL,
  `description_rank2` varchar(100) DEFAULT NULL,
  `description_rank3` varchar(100) DEFAULT NULL,
  `description_rank4` varchar(100) DEFAULT NULL,
  `tree` varchar(10) NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `image_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`mastery_id`),
  UNIQUE KEY `mastery_id_UNIQUE` (`mastery_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `player` (
  `summoner_id` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `rank` varchar(45) DEFAULT NULL,
  `level` int(10) unsigned NOT NULL,
  `last_update_match_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`summoner_id`),
  UNIQUE KEY `summoner_id_UNIQUE` (`summoner_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `player_champion_stat` (
  `summoner_id` int(10) unsigned NOT NULL,
  `champion_id` int(10) unsigned NOT NULL,
  `played_as_won` int(10) unsigned NOT NULL DEFAULT '0',
  `played_as_total` int(10) unsigned NOT NULL DEFAULT '0',
  `played_against_won` int(10) unsigned NOT NULL DEFAULT '0',
  `played_against_total` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`summoner_id`,`champion_id`),
  KEY `fk_player_champion_id_idx` (`champion_id`),
  CONSTRAINT `fk_enemy_champ` FOREIGN KEY (`champion_id`) REFERENCES `champion` (`champion_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_player_champ` FOREIGN KEY (`champion_id`) REFERENCES `champion` (`champion_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_summoner_champ` FOREIGN KEY (`summoner_id`) REFERENCES `player` (`summoner_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `rune` (
  `rune_id` int(10) unsigned NOT NULL,
  `name` varchar(60) NOT NULL,
  `type` varchar(15) NOT NULL,
  `tier` int(10) unsigned NOT NULL,
  `effect` varchar(100) NOT NULL,
  PRIMARY KEY (`rune_id`),
  UNIQUE KEY `rune_id_UNIQUE` (`rune_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `summoner_spell` (
  `summoner_spell_id` int(10) unsigned NOT NULL,
  `name` varchar(15) NOT NULL,
  `description` varchar(300) NOT NULL,
  `summoner_level` int(10) unsigned NOT NULL,
  PRIMARY KEY (`summoner_spell_id`),
  UNIQUE KEY `summoner_spell_id_UNIQUE` (`summoner_spell_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `player_matchup` (
  `matchup_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `summoner_id` int(10) unsigned NOT NULL,
  `player_champion_id` int(10) unsigned NOT NULL,
  `opponent_champion_id` int(10) unsigned NOT NULL,
  `won` int(10) unsigned NOT NULL DEFAULT '0',
  `played` int(10) unsigned NOT NULL DEFAULT '0',
  `kills` int(10) unsigned NOT NULL DEFAULT '0',
  `deaths` int(10) unsigned NOT NULL DEFAULT '0',
  `assists` int(10) unsigned NOT NULL DEFAULT '0',
  `cumulative_creep_score` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`matchup_id`),
  UNIQUE KEY `matchup_id_UNIQUE` (`matchup_id`),
  KEY `fk_mathcup_summoner_idx` (`summoner_id`),
  KEY `fk_matchup_player_champ_idx` (`player_champion_id`),
  KEY `fk_matchup_enemy_champ_idx` (`opponent_champion_id`),
  CONSTRAINT `fk_matchup_enemy_champ` FOREIGN KEY (`opponent_champion_id`) REFERENCES `champion` (`champion_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_matchup_player_champ` FOREIGN KEY (`player_champion_id`) REFERENCES `champion` (`champion_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_mathcup_summoner` FOREIGN KEY (`summoner_id`) REFERENCES `player` (`summoner_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9127 DEFAULT CHARSET=utf8;

CREATE TABLE `player_items` (
  `player_items_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `matchup_id` int(10) unsigned NOT NULL,
  `item_id1` int(10) unsigned DEFAULT NULL,
  `item_id2` int(10) unsigned DEFAULT NULL,
  `item_id3` int(10) unsigned DEFAULT NULL,
  `item_id4` int(10) unsigned DEFAULT NULL,
  `item_id5` int(10) unsigned DEFAULT NULL,
  `item_id6` int(10) unsigned DEFAULT NULL,
  `item_id7` int(10) unsigned DEFAULT NULL,
  `won` int(10) unsigned NOT NULL DEFAULT '0',
  `used` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`player_items_id`),
  KEY `fk_item_matchup_idx` (`matchup_id`),
  KEY `fk_item_table_idx` (`item_id1`),
  KEY `fk_item2_items_idx` (`item_id2`),
  KEY `fk_item3_items_idx` (`item_id3`),
  KEY `fk_item4_items_idx` (`item_id4`),
  KEY `fk_item5_items_idx` (`item_id5`),
  KEY `fk_item6_items_idx` (`item_id6`),
  KEY `fk_item7_items_idx` (`item_id7`),
  CONSTRAINT `fk_item1_items` FOREIGN KEY (`item_id1`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item2_items` FOREIGN KEY (`item_id2`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item3_items` FOREIGN KEY (`item_id3`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item4_items` FOREIGN KEY (`item_id4`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item5_items` FOREIGN KEY (`item_id5`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item6_items` FOREIGN KEY (`item_id6`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item7_items` FOREIGN KEY (`item_id7`) REFERENCES `item` (`item_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_item_matchup` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9128 DEFAULT CHARSET=utf8;



CREATE TABLE `player_mastery` (
  `player_mastery_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `matchup_id` int(10) unsigned NOT NULL,
  `offense_values` varchar(100) NOT NULL,
  `defense_values` varchar(100) NOT NULL,
  `utility_values` varchar(100) NOT NULL,
  `won` int(10) unsigned NOT NULL DEFAULT '0',
  `used` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`player_mastery_id`),
  KEY `fk_matchup_mastery_idx` (`matchup_id`),
  CONSTRAINT `fk_mastery_matchup` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9129 DEFAULT CHARSET=utf8;



CREATE TABLE `player_rune_set` (
  `player_rune_set_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `matchup_id` int(10) unsigned NOT NULL,
  `rune_id1` int(10) unsigned DEFAULT NULL,
  `rune_id2` int(10) unsigned DEFAULT NULL,
  `rune_id3` int(10) unsigned DEFAULT NULL,
  `rune_id4` int(10) unsigned DEFAULT NULL,
  `rune_id5` int(10) unsigned DEFAULT NULL,
  `rune_id6` int(10) unsigned DEFAULT NULL,
  `rune_id7` int(10) unsigned DEFAULT NULL,
  `rune_id8` int(10) unsigned DEFAULT NULL,
  `rune_id9` int(10) unsigned DEFAULT NULL,
  `rune_id10` int(10) unsigned DEFAULT NULL,
  `rune_id11` int(10) unsigned DEFAULT NULL,
  `rune_id12` int(10) unsigned DEFAULT NULL,
  `rune_id13` int(10) unsigned DEFAULT NULL,
  `rune_id14` int(10) unsigned DEFAULT NULL,
  `rune_id15` int(10) unsigned DEFAULT NULL,
  `rune_id16` int(10) unsigned DEFAULT NULL,
  `rune_id17` int(10) unsigned DEFAULT NULL,
  `rune_id18` int(10) unsigned DEFAULT NULL,
  `rune_id19` int(10) unsigned DEFAULT NULL,
  `rune_id20` int(10) unsigned DEFAULT NULL,
  `rune_id21` int(10) unsigned DEFAULT NULL,
  `rune_id22` int(10) unsigned DEFAULT NULL,
  `rune_id23` int(10) unsigned DEFAULT NULL,
  `rune_id24` int(10) unsigned DEFAULT NULL,
  `rune_id25` int(10) unsigned DEFAULT NULL,
  `rune_id26` int(10) unsigned DEFAULT NULL,
  `rune_id27` int(10) unsigned DEFAULT NULL,
  `rune_id28` int(10) unsigned DEFAULT NULL,
  `rune_id29` int(10) unsigned DEFAULT NULL,
  `rune_id30` int(10) unsigned DEFAULT NULL,
  `won` int(10) unsigned NOT NULL DEFAULT '0',
  `used` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`player_rune_set_id`),
  KEY `fk_rune_matchup_idx` (`matchup_id`),
  KEY `fk_rune1_runes_idx` (`rune_id1`),
  KEY `fk_rune2_runes_idx` (`rune_id2`),
  KEY `fk_rune3_runes_idx` (`rune_id3`),
  KEY `fk_rune4_runes_idx` (`rune_id4`),
  KEY `fk_rune5_runes_idx` (`rune_id5`),
  KEY `fk_runes6_runes_idx` (`rune_id6`),
  KEY `fk_rune7_runes_idx` (`rune_id7`),
  KEY `fk_rune8_runes_idx` (`rune_id8`),
  KEY `fk_rune9_runes_idx` (`rune_id9`),
  KEY `fk_rune10_runes_idx` (`rune_id10`),
  KEY `fk_rune11_runes_idx` (`rune_id11`),
  KEY `fk_rune12_runes_idx` (`rune_id12`),
  KEY `fk_rune13_runes_idx` (`rune_id13`),
  KEY `fk_rune14_runes_idx` (`rune_id14`),
  KEY `fk_rune15_runes_idx` (`rune_id15`),
  KEY `fk_rune16_runes_idx` (`rune_id16`),
  KEY `fk_rune17_runes_idx` (`rune_id17`),
  KEY `fk_rune18_runes_idx` (`rune_id18`),
  KEY `fk_rune19_runes_idx` (`rune_id19`),
  KEY `fk_rune20_idx` (`rune_id20`),
  KEY `fk_rune21_runes_idx` (`rune_id21`),
  KEY `fk_rune22_runes_idx` (`rune_id22`),
  KEY `fk_rune23_runes_idx` (`rune_id23`),
  KEY `fk_rune24_runes_idx` (`rune_id24`),
  KEY `fk_rune25_runes_idx` (`rune_id25`),
  KEY `fk_rune26_runes_idx` (`rune_id26`),
  KEY `fk_rune27_runes_idx` (`rune_id27`),
  KEY `fk_rune28_runes_idx` (`rune_id28`),
  KEY `fk_rune29_runes_idx` (`rune_id29`),
  KEY `fk_rune30_runes_idx` (`rune_id30`),
  CONSTRAINT `fk_rune10_runes` FOREIGN KEY (`rune_id10`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune11_runes` FOREIGN KEY (`rune_id11`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune12_runes` FOREIGN KEY (`rune_id12`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune13_runes` FOREIGN KEY (`rune_id13`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune14_runes` FOREIGN KEY (`rune_id14`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune15_runes` FOREIGN KEY (`rune_id15`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune16_runes` FOREIGN KEY (`rune_id16`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune17_runes` FOREIGN KEY (`rune_id17`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune18_runes` FOREIGN KEY (`rune_id18`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune19_runes` FOREIGN KEY (`rune_id19`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune1_runes` FOREIGN KEY (`rune_id1`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune20_runes` FOREIGN KEY (`rune_id20`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune21_runes` FOREIGN KEY (`rune_id21`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune22_runes` FOREIGN KEY (`rune_id22`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune23_runes` FOREIGN KEY (`rune_id23`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune24_runes` FOREIGN KEY (`rune_id24`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune25_runes` FOREIGN KEY (`rune_id25`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune26_runes` FOREIGN KEY (`rune_id26`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune27_runes` FOREIGN KEY (`rune_id27`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune28_runes` FOREIGN KEY (`rune_id28`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune29_runes` FOREIGN KEY (`rune_id29`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune2_runes` FOREIGN KEY (`rune_id2`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune30_runes` FOREIGN KEY (`rune_id30`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune3_runes` FOREIGN KEY (`rune_id3`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune4_runes` FOREIGN KEY (`rune_id4`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune5_runes` FOREIGN KEY (`rune_id5`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune7_runes` FOREIGN KEY (`rune_id7`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune8_runes` FOREIGN KEY (`rune_id8`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune9_runes` FOREIGN KEY (`rune_id9`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_runes6_runes` FOREIGN KEY (`rune_id6`) REFERENCES `rune` (`rune_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_rune_matchup` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9128 DEFAULT CHARSET=utf8;



CREATE TABLE `player_summoner_spells` (
  `player_summoner_spells_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `matchup_id` int(10) unsigned NOT NULL,
  `ss_id1` int(10) unsigned NOT NULL,
  `ss_id2` int(10) unsigned NOT NULL,
  `won` int(10) unsigned NOT NULL DEFAULT '0',
  `used` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`player_summoner_spells_id`),
  KEY `fk_ss_matchup_idx` (`matchup_id`),
  KEY `fk_ss1_ss_idx` (`ss_id1`),
  KEY `fk_ss2_ss_idx` (`ss_id2`),
  CONSTRAINT `fk_ss1_ss` FOREIGN KEY (`ss_id1`) REFERENCES `summoner_spell` (`summoner_spell_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ss2_ss` FOREIGN KEY (`ss_id2`) REFERENCES `summoner_spell` (`summoner_spell_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ss_matchup` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9128 DEFAULT CHARSET=utf8;


CREATE TABLE `best_matchup_items` (
  `matchup_id` int(10) unsigned NOT NULL,
  `player_items_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`matchup_id`),
  UNIQUE KEY `matchup_id_UNIQUE` (`matchup_id`),
  UNIQUE KEY `player_items_id_UNIQUE` (`player_items_id`),
  CONSTRAINT `fk_player_items_best_items` FOREIGN KEY (`player_items_id`) REFERENCES `player_items` (`player_items_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_matchup_best_items` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `best_matchup_masteries` (
  `matchup_id` int(10) unsigned NOT NULL,
  `player_mastery_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`matchup_id`),
  UNIQUE KEY `player_mastery_id_UNIQUE` (`player_mastery_id`),
  UNIQUE KEY `matchup_id_UNIQUE` (`matchup_id`),
  CONSTRAINT `fk_matchup_best_masteries` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_player_masteries_best_matchup` FOREIGN KEY (`player_mastery_id`) REFERENCES `player_mastery` (`player_mastery_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `best_matchup_rune_set` (
  `matchup_id` int(10) unsigned NOT NULL,
  `player_rune_set_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`matchup_id`),
  UNIQUE KEY `player_rune_set_id_UNIQUE` (`player_rune_set_id`),
  UNIQUE KEY `matchup_id_UNIQUE` (`matchup_id`),
  CONSTRAINT `fk_player_runes_best_matchup` FOREIGN KEY (`player_rune_set_id`) REFERENCES `player_rune_set` (`player_rune_set_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_matchup_best_runes` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `best_matchup_ss` (
  `matchup_id` int(10) unsigned NOT NULL,
  `player_summoner_spells_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`matchup_id`),
  UNIQUE KEY `player_summoner_spells_id_UNIQUE` (`player_summoner_spells_id`),
  UNIQUE KEY `matchup_id_UNIQUE` (`matchup_id`),
  CONSTRAINT `fk_matchup_best_ss` FOREIGN KEY (`matchup_id`) REFERENCES `player_matchup` (`matchup_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_player_ss_best_matchup` FOREIGN KEY (`player_summoner_spells_id`) REFERENCES `player_summoner_spells` (`player_summoner_spells_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `lolmatchups`.`player` (`summoner_id`,`name`,`rank`,`level`,`last_update_match_id`) VALUES ( 22101250,"Kowz Rule","P4",30,1);

INSERT INTO `lolmatchups`.`player_champion_stat` VALUES (22101250, 266, 1, 1, 0, 0 );