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
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code, is_premium) VALUES ('admin2','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345, 1);
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code, is_premium) VALUES ('mary','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345, 1);

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
	), 2, 1
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
	), 1, 0
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
	), 3, 1
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
	), 7, 3
);

INSERT INTO trade (user_id_from, user_id_to) VALUES (1, 2);

INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 1, 'xy10-117');
INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 1, 'det1-10');
INSERT INTO trade_card_collection (trade_id, collection_id, id) VALUES (1, 2, 'xy2-1');

GO