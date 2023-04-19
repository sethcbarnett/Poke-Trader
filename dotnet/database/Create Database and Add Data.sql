USE master
GO

--drop database if it exists
IF DB_ID('collections_and_trades') IS NOT NULL
BEGIN
	ALTER DATABASE collections_and_trades SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE collections_and_trades;
END

CREATE DATABASE collections_and_trades
GO

USE collections_and_trades
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL,
	email varchar(50) NOT NULL,
	street_address varchar (100) NOT NULL,
	city varchar (50) NOT NULL,
	state_abbreviation varchar (3) NOT NULL,
	zip_code int NOT NULL,
	is_premium bit NOT NULL DEFAULT 0,
	CONSTRAINT PK_users PRIMARY KEY (user_id)
)

CREATE TABLE collection (
	collection_id int IDENTITY(1, 1) NOT NULL,
	user_id int NOT NULL,
	is_public bit NOT NULL DEFAULT 0,
	CONSTRAINT PK_collection PRIMARY KEY (collection_id),
	CONSTRAINT FK_users_collection FOREIGN KEY (user_id) references users (user_id),
)

CREATE TABLE card (
	id varchar(20) NOT NULL UNIQUE,
	name varchar(50) NOT NULL,
	img varchar(100) NOT NULL,
	price varchar(20) NOT NULL,
	low_price varchar(20) NOT NULL,
	high_price varchar(20) NOT NULL,
	rarity varchar(50) NOT NULL,
	tcg_url varchar(100) NOT NULL,
	CONSTRAINT PK_card PRIMARY KEY (id)
)

CREATE TABLE collection_card (
	collection_id int NOT NULL,
	id varchar(20) NOT NULL,
	grade varchar(50),
	quantity int NOT NULL,
	amount_to_trade int NOT NULL,
	CONSTRAINT PK_collection_card PRIMARY KEY (collection_id, id),
	CONSTRAINT FK_collection_card_collection FOREIGN KEY (collection_id) references collection (collection_id),
	CONSTRAINT FK_collection_card_card FOREIGN KEY (id) references card (id),
)

CREATE TABLE trade (
	trade_id int IDENTITY(1, 1) NOT NULL,
	user_id_from int NOT NULL,
	user_id_to int NOT NULL,
	status varchar(10) DEFAULT 'pending',
	CONSTRAINT PK_trade PRIMARY KEY (trade_id),
	CONSTRAINT FK_trade_user_from FOREIGN KEY (user_id_from) references users (user_id),
	CONSTRAINT FK_trade_user_to FOREIGN KEY (user_id_to) references users (user_id),
)

CREATE TABLE trade_card_collection (
	trade_id int NOT NULL,
	id varchar(20) NOT NULL,
	collection_id int NOT NULL,
	CONSTRAINT PK_trade_card_collection PRIMARY KEY (trade_id, id, collection_id),
	CONSTRAINT FK_trade_card_collection_trade FOREIGN KEY (trade_id) references trade (trade_id),
	CONSTRAINT FK_trade_card_collection_card FOREIGN KEY (id) references card (id),
	CONSTRAINT FK_trade_card_collection_collection FOREIGN KEY (collection_id) references collection (collection_id)
)

--populate default data
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code, is_premium) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345, 1);

INSERT INTO collection (user_id) VALUES ((SELECT user_id FROM users WHERE username = 'user'));
INSERT INTO collection (user_id, is_public) VALUES ((SELECT user_id FROM users WHERE username = 'admin'), 1);

INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Alakazam-EX', 'xy10-117', 'https://images.pokemontcg.io/xy10/117.png', '7.43', '4.33', '25', 'Rare Ultra', 'https://prices.pokemontcg.io/tcgplayer/xy10-117');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Detective Pikachu','det1-10','https://images.pokemontcg.io/det1/10.png','2.37', '1.78', '9.99', 'Rare', 'https://prices.pokemontcg.io/tcgplayer/det1-10');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Caterpie','xy2-1','https://images.pokemontcg.io/xy2/1.png', '0.17', '0.06', '1.61', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy2-1');

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'user'
		)
	), 
	(
		SELECT id FROM card WHERE name = 'Alakazam-EX'
	), 1, 1
);

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'user'
		)
	), 
	(
		SELECT id FROM card WHERE name = 'Detective Pikachu'
	), 1, 1
);

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'admin'
		)
	), 
	(
		SELECT id FROM card WHERE name = 'Detective Pikachu'
	), 1, 1
);

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'admin'
		)
	), 
	(
		SELECT id FROM card WHERE name = 'Caterpie'
	), 1, 1
);

INSERT INTO trade (user_id_from, user_id_to) VALUES (1, 2);

INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 1, 'xy10-117');
INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 1, 'det1-10');
INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 2, 'xy2-1');


--Proxy Data for demonstration

--Pikachu Loverrr3000's collection of 100 Pikachu's
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('PikachuLoverrr3000','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);
INSERT INTO collection (user_id, is_public) VALUES (3, 0);

INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'basep-1', 'https://images.pokemontcg.io/basep/1.png', '8.12', '1.95', 'Price Not Found', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-1');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'mcd19-6', 'https://images.pokemontcg.io/mcd19/6.png', '17.77', '9.72', '20', '', 'https://prices.pokemontcg.io/tcgplayer/mcd19-6');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'basep-4', 'https://images.pokemontcg.io/basep/4.png', '8.83', '4.25', '40', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-4');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ru1-7', 'https://images.pokemontcg.io/ru1/7.png', '140.99', '125', '349.95', '', 'https://prices.pokemontcg.io/tcgplayer/ru1-7');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Detective Pikachu', 'det1-10', 'https://images.pokemontcg.io/det1/10.png', '2.43', '1.78', '9.99', 'Rare', 'https://prices.pokemontcg.io/tcgplayer/det1-10');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pop6-9', 'https://images.pokemontcg.io/pop6/9.png', '21.23', '18.46', '26.8', 'Uncommon', 'https://prices.pokemontcg.io/tcgplayer/pop6-9');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM04', 'https://images.pokemontcg.io/smp/SM04.png', '6.83', '5', '30', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM04');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'hsp-HGSS03', 'https://images.pokemontcg.io/hsp/HGSS03.png', '2.94', '1.7', '29.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/hsp-HGSS03');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pop4-13', 'https://images.pokemontcg.io/pop4/13.png', '11.82', '6.47', '20', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop4-13');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pop9-15', 'https://images.pokemontcg.io/pop9/15.png', '4.94', '2.85', '49.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop9-15');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'dpp-DP16', 'https://images.pokemontcg.io/dpp/DP16.png', '36.5', '24.04', '83.48', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/dpp-DP16');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'mcd16-6', 'https://images.pokemontcg.io/mcd16/6.png', '13.89', '2', 'Price Not Found', '', 'https://prices.pokemontcg.io/cardmarket/mcd16-6');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pop5-12', 'https://images.pokemontcg.io/pop5/12.png', '17.68', '11.04', '120', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop5-12');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu δ', 'pop5-13', 'https://images.pokemontcg.io/pop5/13.png', '13.49', '7.84', '15.68', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop5-13');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm115-19', 'https://images.pokemontcg.io/sm115/19.png', '0.6', '0.18', '4', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm115-19');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'np-12', 'https://images.pokemontcg.io/np/12.png', '18.59', '11.85', '50', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/np-12');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pop2-16', 'https://images.pokemontcg.io/pop2/16.png', '7.07', '5', '19', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop2-16');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xy6-20', 'https://images.pokemontcg.io/xy6/20.png', '0.4', '0.03', '19.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy6-20');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('_____''s Pikachu', 'basep-24', 'https://images.pokemontcg.io/basep/24.png', '101.57', '64.66', '200', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-24');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Flying Pikachu', 'basep-25', 'https://images.pokemontcg.io/basep/25.png', '24.8', '15', '39.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-25');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'swshp-SWSH020', 'https://images.pokemontcg.io/swshp/SWSH020.png', '39.42', '26.68', '59.69', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/swshp-SWSH020');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'basep-26', 'https://images.pokemontcg.io/basep/26.png', '23.87', '19.45', '79.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-26');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'basep-27', 'https://images.pokemontcg.io/basep/27.png', '5.43', '3.82', '22', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-27');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Surfing Pikachu', 'basep-28', 'https://images.pokemontcg.io/basep/28.png', '11.83', '7.99', '25', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/basep-28');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu & Zekrom-GX', 'sm9-33', 'https://images.pokemontcg.io/sm9/33.png', '4.89', '3.99', '12', 'Rare Holo GX', 'https://prices.pokemontcg.io/tcgplayer/sm9-33');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'bw4-39', 'https://images.pokemontcg.io/bw4/39.png', '1.1', '0.45', '4', 'Common', 'https://prices.pokemontcg.io/tcgplayer/bw4-39');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm35-28', 'https://images.pokemontcg.io/sm35/28.png', '0.36', '0.1', '3.87', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm35-28');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xy3-27', 'https://images.pokemontcg.io/xy3/27.png', '0.97', '0.24', '2.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy3-27');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'g1-26', 'https://images.pokemontcg.io/g1/26.png', '0.62', '0.2', '15', 'Common', 'https://prices.pokemontcg.io/tcgplayer/g1-26');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm4-30', 'https://images.pokemontcg.io/sm4/30.png', '0.32', '0.01', '1.32', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm4-30');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex4-43', 'https://images.pokemontcg.io/ex4/43.png', '5.06', '3.85', '14.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex4-43');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xy12-35', 'https://images.pokemontcg.io/xy12/35.png', '0.2', '0.01', '5.39', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy12-35');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xy1-42', 'https://images.pokemontcg.io/xy1/42.png', '0.45', '0.22', '2.17', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy1-42');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu δ', 'np-35', 'https://images.pokemontcg.io/np/35.png', '31.9', '74.95', '99.95', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/np-35');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm3-40', 'https://images.pokemontcg.io/sm3/40.png', '0.25', '0.09', '19.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm3-40');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu V', 'swsh4-43', 'https://images.pokemontcg.io/swsh4/43.png', '0.81', '0.49', '1000', 'Rare Holo V', 'https://prices.pokemontcg.io/tcgplayer/swsh4-43');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'swshp-SWSH039', 'https://images.pokemontcg.io/swshp/SWSH039.png', '2.51', '0.4', '19.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/swshp-SWSH039');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'bw7-50', 'https://images.pokemontcg.io/bw7/50.png', '1.67', '0.5', '19.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/bw7-50');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu VMAX', 'swsh4-44', 'https://images.pokemontcg.io/swsh4/44.png', '2.73', '1.99', '1000', 'Rare Holo VMAX', 'https://prices.pokemontcg.io/tcgplayer/swsh4-44');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xy8-48', 'https://images.pokemontcg.io/xy8/48.png', '0.42', '0.19', '4.77', 'Common', 'https://prices.pokemontcg.io/tcgplayer/xy8-48');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'base1-58', 'https://images.pokemontcg.io/base1/58.png', '3.85', '0.5', '30', 'Common', 'https://prices.pokemontcg.io/tcgplayer/base1-58');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm11-55', 'https://images.pokemontcg.io/sm11/55.png', '0.27', '0.08', '4.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm11-55');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex9-60', 'https://images.pokemontcg.io/ex9/60.png', '4.56', '2.2', '20.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex9-60');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm11-56', 'https://images.pokemontcg.io/sm11/56.png', '0.35', '0.22', '4.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm11-56');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'hgss3-61', 'https://images.pokemontcg.io/hgss3/61.png', '2.5', '1.2', '6.95', 'Common', 'https://prices.pokemontcg.io/tcgplayer/hgss3-61');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm12-66', 'https://images.pokemontcg.io/sm12/66.png', '0.3', '0.15', '5', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm12-66');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'sm10-54', 'https://images.pokemontcg.io/sm10/54.png', '0.3', '0.14', '4.3', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm10-54');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex16-57', 'https://images.pokemontcg.io/ex16/57.png', '2.31', '1.45', '6.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex16-57');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'swsh1-65', 'https://images.pokemontcg.io/swsh1/65.png', '0.14', '0.03', '1.22', 'Common', 'https://prices.pokemontcg.io/tcgplayer/swsh1-65');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'bwp-BW54', 'https://images.pokemontcg.io/bwp/BW54.png', '59.99', '43.99', '70.19', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/bwp-BW54');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu V', 'swshp-SWSH063', 'https://images.pokemontcg.io/swshp/SWSH063.png', '0.91', '0.5', '19.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/swshp-SWSH063');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'base2-60', 'https://images.pokemontcg.io/base2/60.png', '8.53', '3.75', '35.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/base2-60');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pl4-71', 'https://images.pokemontcg.io/pl4/71.png', '1.98', '1', '7.97', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pl4-71');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'hgss1-78', 'https://images.pokemontcg.io/hgss1/78.png', '2.77', '0.8', '5.95', 'Common', 'https://prices.pokemontcg.io/tcgplayer/hgss1-78');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'dp5-70', 'https://images.pokemontcg.io/dp5/70.png', '1.08', '0.25', '4.47', 'Common', 'https://prices.pokemontcg.io/tcgplayer/dp5-70');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'neo1-70', 'https://images.pokemontcg.io/neo1/70.png', '7.34', '2.97', '23.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/neo1-70');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'dp7-70', 'https://images.pokemontcg.io/dp7/70.png', '2.31', '1.49', '4.4', 'Common', 'https://prices.pokemontcg.io/tcgplayer/dp7-70');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex6-74', 'https://images.pokemontcg.io/ex6/74.png', '3.78', '2.23', '28.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex6-74');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex13-78', 'https://images.pokemontcg.io/ex13/78.png', '3.1', '0.94', '9.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex13-78');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Lt. Surge''s Pikachu', 'gym1-81', 'https://images.pokemontcg.io/gym1/81.png', '8.49', '4.99', '27.87', 'Common', 'https://prices.pokemontcg.io/tcgplayer/gym1-81');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Special Delivery Pikachu', 'swshp-SWSH074', 'https://images.pokemontcg.io/swshp/SWSH074.png', '147.22', '130', '280', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/swshp-SWSH074');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu δ', 'ex13-79', 'https://images.pokemontcg.io/ex13/79.png', '3.58', '1.45', '14.37', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex13-79');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ex2-72', 'https://images.pokemontcg.io/ex2/72.png', '2.79', '1.75', '5.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ex2-72');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Lt. Surge''s Pikachu', 'gym2-84', 'https://images.pokemontcg.io/gym2/84.png', '9.4', '5.99', '30', 'Common', 'https://prices.pokemontcg.io/tcgplayer/gym2-84');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM76', 'https://images.pokemontcg.io/smp/SM76.png', '1.29', '0.96', '30', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM76');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM81', 'https://images.pokemontcg.io/smp/SM81.png', '3.87', '2.54', '31', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM81');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'base4-87', 'https://images.pokemontcg.io/base4/87.png', '1.29', '0.42', '6.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/base4-87');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu δ', 'ex12-93', 'https://images.pokemontcg.io/ex12/93.png', '107.54', '31.93', '118.98', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/ex12-93');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'base6-86', 'https://images.pokemontcg.io/base6/86.png', '2.82', '0.98', '5', 'Common', 'https://prices.pokemontcg.io/tcgplayer/base6-86');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM86', 'https://images.pokemontcg.io/smp/SM86.png', '4.58', '3.4', '30', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM86');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu-EX', 'xyp-XY84', 'https://images.pokemontcg.io/xyp/XY84.png', '24.51', '14.98', '59.95', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/xyp-XY84');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xyp-XY89', 'https://images.pokemontcg.io/xyp/XY89.png', '5.53', '3.75', '19.95', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/xyp-XY89');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM98', 'https://images.pokemontcg.io/smp/SM98.png', '3.76', '2.5', '15', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM98');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'dp2-94', 'https://images.pokemontcg.io/dp2/94.png', '2.23', '0.75', '6.66', 'Common', 'https://prices.pokemontcg.io/tcgplayer/dp2-94');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'xyp-XY95', 'https://images.pokemontcg.io/xyp/XY95.png', '20.58', '7.77', '30', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/xyp-XY95');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu ?', 'ex13-104', 'https://images.pokemontcg.io/ex13/104.png', '1000', '1750', '2499.99', 'Rare Holo Star', 'https://prices.pokemontcg.io/tcgplayer/ex13-104');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM108', 'https://images.pokemontcg.io/smp/SM108.png', '10.97', '6.16', '22.79', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM108');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pl2-112', 'https://images.pokemontcg.io/pl2/112.png', '87.24', '50.49', '119.99', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/pl2-112');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Flying Pikachu', 'pl2-113', 'https://images.pokemontcg.io/pl2/113.png', '73.14', '48.99', '90', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/pl2-113');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM109', 'https://images.pokemontcg.io/smp/SM109.png', '24.44', '18.99', '27', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM109');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Surfing Pikachu', 'pl2-114', 'https://images.pokemontcg.io/pl2/114.png', '69.63', '57.34', '77.53', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/pl2-114');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM110', 'https://images.pokemontcg.io/smp/SM110.png', '10.62', '9', '11.59', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM110');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM111', 'https://images.pokemontcg.io/smp/SM111.png', '15.54', '10.46', '27', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM111');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM112', 'https://images.pokemontcg.io/smp/SM112.png', '12.92', '6', '39.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM112');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Flying Pikachu', 'xy12-110', 'https://images.pokemontcg.io/xy12/110.png', '1.34', '0.89', '20', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/xy12-110');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'bw1-115', 'https://images.pokemontcg.io/bw1/115.png', '35.47', '26.41', '99.95', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/bw1-115');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM113', 'https://images.pokemontcg.io/smp/SM113.png', '12.9', '9', '22.79', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM113');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Surfing Pikachu', 'xy12-111', 'https://images.pokemontcg.io/xy12/111.png', '1.56', '0.9', '20', 'Rare Secret', 'https://prices.pokemontcg.io/tcgplayer/xy12-111');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'bw11-RC7', 'https://images.pokemontcg.io/bw11/RC7.png', '16.63', '4.89', '99.99', 'Uncommon', 'https://prices.pokemontcg.io/tcgplayer/bw11-RC7');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ash''s Pikachu', 'smp-SM114', 'https://images.pokemontcg.io/smp/SM114.png', '11.72', '9.02', '28.97', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM114');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'g1-RC29', 'https://images.pokemontcg.io/g1/RC29.png', '26.05', '13.1', '99.95', 'Rare Ultra', 'https://prices.pokemontcg.io/tcgplayer/g1-RC29');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ecard3-84', 'https://images.pokemontcg.io/ecard3/84.png', '35.2', '18.71', '18.71', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ecard3-84');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'pl3-120', 'https://images.pokemontcg.io/pl3/120.png', '2.17', '1.19', '35.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pl3-120');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu-EX', 'xyp-XY124', 'https://images.pokemontcg.io/xyp/XY124.png', '72.46', '34.99', '199.85', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/xyp-XY124');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'ecard1-124', 'https://images.pokemontcg.io/ecard1/124.png', '3.77', '1.99', '35', 'Common', 'https://prices.pokemontcg.io/tcgplayer/ecard1-124');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu & Zekrom-GX', 'sm9-162', 'https://images.pokemontcg.io/sm9/162.png', '26.28', '18.57', '121.49', 'Rare Ultra', 'https://prices.pokemontcg.io/tcgplayer/sm9-162');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM157', 'https://images.pokemontcg.io/smp/SM157.png', '4.03', '3.2', '15', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM157');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu & Zekrom-GX', 'smp-SM168', 'https://images.pokemontcg.io/smp/SM168.png', '23.1', '14.23', '29.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM168');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pikachu', 'smp-SM162', 'https://images.pokemontcg.io/smp/SM162.png', '7.23', '5.88', '19.47', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM162');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Detective Pikachu', 'smp-SM170', 'https://images.pokemontcg.io/smp/SM170.png', '19.57', '9.57', '19.99', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM170');

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-1', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'mcd19-6', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-4', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ru1-7', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'det1-10', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop6-9', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM04', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'hsp-HGSS03', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop4-13', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop9-15', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'dpp-DP16', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'mcd16-6', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop5-12', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop5-13', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm115-19', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'np-12', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pop2-16', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy6-20', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-24', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-25', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swshp-SWSH020', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-26', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-27', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'basep-28', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm9-33', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'bw4-39', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm35-28', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy3-27', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'g1-26', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm4-30', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex4-43', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy12-35', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy1-42', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'np-35', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm3-40', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swsh4-43', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swshp-SWSH039', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'bw7-50', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swsh4-44', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy8-48', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'base1-58', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm11-55', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex9-60', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm11-56', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'hgss3-61', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm12-66', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm10-54', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex16-57', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swsh1-65', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'bwp-BW54', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swshp-SWSH063', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'base2-60', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pl4-71', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'hgss1-78', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'dp5-70', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'neo1-70', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'dp7-70', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex6-74', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex13-78', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'gym1-81', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'swshp-SWSH074', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex13-79', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex2-72', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'gym2-84', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM76', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM81', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'base4-87', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex12-93', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'base6-86', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM86', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xyp-XY84', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xyp-XY89', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM98', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'dp2-94', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xyp-XY95', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ex13-104', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM108', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pl2-112', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pl2-113', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM109', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pl2-114', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM110', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM111', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM112', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy12-110', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'bw1-115', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM113', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xy12-111', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'bw11-RC7', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM114', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'g1-RC29', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ecard3-84', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'pl3-120', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'xyp-XY124', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'ecard1-124', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'sm9-162', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM157', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM162', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM168', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (3, 'smp-SM170', 1, 1);

--Ash's collection

INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('aketchum','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);
INSERT INTO collection (user_id, is_public) VALUES (4, 1);

INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Pidgeot', 'si1-2', 'https://images.pokemontcg.io/si1/2.png', '12.29', '6.5', '39.99', '', 'https://prices.pokemontcg.io/tcgplayer/si1-2');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Bulbasaur', 'det1-1', 'https://images.pokemontcg.io/det1/1.png', '0.35', '0.1', '19.99', 'Common', 'https://prices.pokemontcg.io/tcgplayer/det1-1');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Charizard', 'det1-5', 'https://images.pokemontcg.io/det1/5.png', '8.39', '6', '19.99', 'Rare Ultra', 'https://prices.pokemontcg.io/tcgplayer/det1-5');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Snorlax-GX', 'smp-SM05', 'https://images.pokemontcg.io/smp/SM05.png', '2.84', '2.34', '9.95', 'Promo', 'https://prices.pokemontcg.io/tcgplayer/smp-SM05');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Dragonite δ', 'ex11-3', 'https://images.pokemontcg.io/ex11/3.png', '70.28', '35', '85.52', 'Rare Holo', 'https://prices.pokemontcg.io/tcgplayer/ex11-3');

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'smp-SM108', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'si1-2', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'det1-1', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'det1-5', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'smp-SM05', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (4, 'ex11-3', 1, 1);


--Team Rocket's Collection
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('teamrocket','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);
INSERT INTO collection (user_id, is_public) VALUES (5, 1);

INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Meowth δ', 'pop5-11', 'https://images.pokemontcg.io/pop5/11.png', '3.49', '1.8', '4.97', 'Common', 'https://prices.pokemontcg.io/tcgplayer/pop5-11');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Wobbuffet', 'hgss1-13', 'https://images.pokemontcg.io/hgss1/13.png', '6.97', '3.77', '9.99', 'Rare Holo', 'https://prices.pokemontcg.io/tcgplayer/hgss1-13');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Seviper', 'ex2-11', 'https://images.pokemontcg.io/ex2/11.png', '20.34', '14.04', '27.99', 'Rare Holo', 'https://prices.pokemontcg.io/tcgplayer/ex2-11');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Yanmega', 'sm11-3', 'https://images.pokemontcg.io/sm11/3.png', '0.3', '0.21', '4.99', 'Uncommon', 'https://prices.pokemontcg.io/tcgplayer/sm11-3');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Woobat', 'mcd12-7', 'https://images.pokemontcg.io/mcd12/7.png', '3.6', '1.66', '28.99', '', 'https://prices.pokemontcg.io/tcgplayer/mcd12-7');
INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES ('Ekans', 'sm115-25', 'https://images.pokemontcg.io/sm115/25.png', '0.06', '0.02', '1.2', 'Common', 'https://prices.pokemontcg.io/tcgplayer/sm115-25');

INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'pop5-11', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'hgss1-13', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'ex2-11', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'sm11-3', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'mcd12-7', 1, 1);
INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES (5, 'sm115-25', 1, 1);

GO