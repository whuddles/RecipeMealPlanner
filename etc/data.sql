-- Switch to the system (aka master) database
USE master;
GO

-- Delete the MealPlanner Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='MealPlanner')
DROP DATABASE MealPlanner;
GO

-- Create a new MealPlanner Database
CREATE DATABASE MealPlanner;
GO

-- Switch to the MealPlanner Database
USE MealPlanner
GO

BEGIN TRANSACTION;

--CREATE TABLE users
--(
--	id			int			identity(1,1),
--	username	varchar(50)	not null,
--	password	varchar(50)	not null,
--	salt		varchar(50)	not null,
--	role		varchar(50)	default('user'),

--	constraint pk_users primary key (id)
--);

CREATE TABLE ingredient
(
	ingredient_id	int				identity(1,1)		PRIMARY KEY,
	name			varchar(50)		not null
);

CREATE TABLE recipe
(
	recipe_id		int				identity(1,1)		PRIMARY KEY,
	name			varchar(100)	not null,
	description		varchar(300)	not null,
	instructions	varchar(max)	not null,
	prep_time		int				not null DEFAULT(0),
	cook_time		int				not null DEFAULT(0)
);

CREATE TABLE recipe_ingredient
(
	recipe_id		int		FOREIGN KEY REFERENCES recipe(recipe_id),
	ingredient_id	int		FOREIGN KEY REFERENCES ingredient(ingredient_id)

	CONSTRAINT pk_recipe_ingredient PRIMARY KEY CLUSTERED (recipe_id, ingredient_id)
);

CREATE TABLE quantity
(
	quantity_id		int			IDENTITY			PRIMARY KEY,
	number			varchar(3)	NOT NULL DEFAULT(0),
	fraction		varchar(3)	NOT NULL DEFAULT(0),
	unit			varchar(20) NOT NULL			
);

CREATE TABLE quantity_ingredient
(
	quantity_id			int			FOREIGN KEY REFERENCES quantity(quantity_id),
	ingredient_id	int			FOREIGN KEY REFERENCES ingredient(ingredient_id)

	CONSTRAINT pk_unit_ingredient PRIMARY KEY CLUSTERED (quantity_id, ingredient_id)
);

SET IDENTITY_INSERT quantity ON;

INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (1, '2', '', 'slices');
INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (2, '2', '', 'Tbsp.');
INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (3, '1', '', 'Tbsp.');
INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (4, '3', '', '');
INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (5, '1', '', '');
INSERT INTO quantity (quantity_id, number, fraction, unit) VALUES (6, '12', '', 'ounce');

SET IDENTITY_INSERT quantity OFF;

SET IDENTITY_INSERT ingredient ON;

INSERT INTO ingredient (ingredient_id, name) VALUES (1, 'multigrain bread');
INSERT INTO ingredient (ingredient_id, name) VALUES (2, 'peanut butter (creamy)');
INSERT INTO ingredient (ingredient_id, name) VALUES (3, 'grape jelly');
INSERT INTO ingredient (ingredient_id, name) VALUES (4, 'egg, large');
INSERT INTO ingredient (ingredient_id, name) VALUES (5, 'milk, 2%');
INSERT INTO ingredient (ingredient_id, name) VALUES (6, 'green onion');
INSERT INTO ingredient (ingredient_id, name) VALUES (7, 'cheddar cheese');
INSERT INTO ingredient (ingredient_id, name) VALUES (8, 'butter');

SET IDENTITY_INSERT ingredient OFF;

SET IDENTITY_INSERT recipe ON;

INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (1, 'Peanut Butter and Jelly Sandwich', 'The classic sandwich for kids of all ages', '1) Spread peanut butter on first slice of bread. 2) Spread jelly on second slice of bread. 3) Put both pieces of bread together with peanut butter and jelly sides facing inward. 4) Cut into rectangles or triangles. 5) Relive the simple joy of childhood!', 2, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (2, 'Cheesy Scrambled Eggs', 'A breakfast favorite', '1) Heat 10" skillet on medium-low heat. 2) Crack eggs into bowl, discard shells, and add milk. 3) Whisk eggs and milk vigorously until pale and slightly frothy. 4) Grate or slice cheese as desired. 5) Chop onion finely, reserving some green for garnish. 6) Melt butter in skillet. 7). Sautee onion in butter for 30 seconds to 1 minute. 8) Pour eggs into skillet and stir continuously with wooden spoon or chopsticks for about 90 seconds, or until mostly solid, but still slightly runny. 9) Add cheese to top of eggs and cover for 20-30 seconds to melt. 10) Scoop eggs onto plate, garnish with reserved green onion rounds, and serve.', 5, 3)

SET IDENTITY_INSERT recipe OFF;

INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (1, 1);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (1, 2);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (1, 3);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (2, 4);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (2, 5);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (2, 6);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (2, 7);
INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (2, 8);

INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (1, 1);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (2, 2);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (2, 3);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (4, 4);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (2, 5);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (5, 6);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (6, 7);
INSERT INTO quantity_ingredient (quantity_id, ingredient_id) VALUES (3, 8);

COMMIT TRANSACTION;
