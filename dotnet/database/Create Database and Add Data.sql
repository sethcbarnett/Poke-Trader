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
	CONSTRAINT PK_users PRIMARY KEY (user_id)
)

CREATE TABLE collection (
	collection_id int IDENTITY(1, 1) NOT NULL,
	user_id int NOT NULL,
	CONSTRAINT PK_collection PRIMARY KEY (collection_id),
	CONSTRAINT FK_users_collection FOREIGN KEY (user_id) references users (user_id),
)

CREATE TABLE card (
	api_card_id varchar(20) NOT NULL UNIQUE,
	name varchar(50) NOT NULL,
	CONSTRAINT PK_card PRIMARY KEY (api_card_id)
)

CREATE TABLE collection_card (
	collection_id int NOT NULL,
	api_card_id varchar(20) NOT NULL,
	quantity int NOT NULL,
	CONSTRAINT PK_collection_card PRIMARY KEY (collection_id, api_card_id),
	CONSTRAINT FK_collection_card_collection FOREIGN KEY (collection_id) references collection (collection_id),
	CONSTRAINT FK_collection_card_card FOREIGN KEY (api_card_id) references card (api_card_id),
)

--populate default data
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);
INSERT INTO users (username, password_hash, salt, user_role, email, street_address, city, state_abbreviation, zip_code) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 'abc123@hello.com', '123 muffin lane', 'Cleveland', 'RI', 12345);

INSERT INTO collection (user_id) VALUES ((SELECT user_id FROM users WHERE username = 'user'));
INSERT INTO collection (user_id) VALUES ((SELECT user_id FROM users WHERE username = 'admin'));

INSERT INTO card (name, api_card_id) VALUES ('Alakazam-EX', 'xy10-117');
INSERT INTO card (name, api_card_id) VALUES ('Detective Pikachu','det1');
INSERT INTO card (name, api_card_id) VALUES ('Caterpie','xy2-1');

INSERT INTO collection_card (collection_id, api_card_id, quantity) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'user'
		)
	), 
	(
		SELECT api_card_id FROM card WHERE name = 'Alakazam-EX'
	), 2
);

INSERT INTO collection_card (collection_id, api_card_id, quantity) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'user'
		)
	), 
	(
		SELECT api_card_id FROM card WHERE name = 'Detective Pikachu'
	), 1
);

INSERT INTO collection_card (collection_id, api_card_id, quantity) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'admin'
		)
	), 
	(
		SELECT api_card_id FROM card WHERE name = 'Detective Pikachu'
	), 3
);

INSERT INTO collection_card (collection_id, api_card_id, quantity) VALUES 
(
	(
		SELECT collection_id FROM collection WHERE user_id = 
		(
			SELECT user_id FROM users WHERE username = 'admin'
		)
	), 
	(
		SELECT api_card_id FROM card WHERE name = 'Caterpie'
	), 7
);

GO