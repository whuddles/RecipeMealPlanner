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

CREATE TABLE users
(
	id				int			identity(1,1),
	username		varchar(50)	not null,
	password		varchar(50)	not null,
	salt			varchar(50)	not null,
	role			varchar(50)	default('user'),

	constraint pk_users primary key (id)
);

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
	prep_time		int				not null			DEFAULT(0),
	cook_time		int				not null			DEFAULT(0)
);

CREATE TABLE number
(
	number_id		int				IDENTITY			PRIMARY KEY,
	number			varchar(3)		NOT NULL			DEFAULT('0')
);

CREATE TABLE fraction
(
	fraction_id		int				IDENTITY			PRIMARY KEY,
	fraction		varchar(3)		NOT NULL			DEFAULT('-')
);

CREATE TABLE unit
(
	unit_id			INT				IDENTITY			PRIMARY KEY,
	unit			VARCHAR(20)		NOT NULL
);

CREATE TABLE recipe_ingredient_unit_number_fraction
(
	recipe_id		int				FOREIGN KEY REFERENCES recipe(recipe_id),
	ingredient_id	int				FOREIGN KEY REFERENCES ingredient(ingredient_id),
	unit_id			int				FOREIGN KEY REFERENCES unit(unit_id),
	number_id		int				FOREIGN KEY REFERENCES number(number_id),
	fraction_id		int				FOREIGN KEY REFERENCES fraction(fraction_id)

	CONSTRAINT pk_recipe_ingredient PRIMARY KEY CLUSTERED (recipe_id, ingredient_id, unit_id, number_id, fraction_id)
);

CREATE TABLE users_recipe
(
	id				int				FOREIGN KEY REFERENCES users(id),
	recipe_id		int				FOREIGN KEY REFERENCES recipe(recipe_id)

	CONSTRAINT pk_users_recipe PRIMARY KEY CLUSTERED (id, recipe_id)
);

CREATE TABLE meal
(
	meal_id			int				IDENTITY			PRIMARY KEY,
	meal_name		varchar(100)	NOT NULL
);

CREATE TABLE day
(
	day_id			int				IDENTITY			PRIMARY KEY,
	breakfast		int				NOT NULL			FOREIGN KEY REFERENCES meal(meal_id),
	lunch			int				NOT NULL			FOREIGN KEY REFERENCES meal(meal_id),
	dinner			int				NOT NULL			FOREIGN KEY REFERENCES meal(meal_id)
	
);

CREATE TABLE mealPlan
(
	mealPlan_id		int				IDENTITY			PRIMARY KEY,
	mealPlan_name	varchar(100)	NOT NULL,
	day1			int				NOT NULL			FOREIGN KEY REFERENCES day(day_id),
	day2			int				NULL				FOREIGN KEY REFERENCES day(day_id),
	day3			int				NULL				FOREIGN KEY REFERENCES day(day_id),
	day4			int				NULL				FOREIGN KEY REFERENCES day(day_id),
	day5			int				NULL				FOREIGN KEY REFERENCES day(day_id),
	day6			int				NULL				FOREIGN KEY REFERENCES day(day_id),
	day7			int				NULL				FOREIGN KEY REFERENCES day(day_id)
);





CREATE TABLE users_mealPlan
(
	users_id		int				FOREIGN KEY REFERENCES users(id),
	mealPlan_id		int				FOREIGN KEY REFERENCES mealPlan(mealPlan_id)

	CONSTRAINT pk_users_mealPlan PRIMARY KEY CLUSTERED (users_id, mealPlan_id)
);

CREATE TABLE mealPlan_meal
(
	mealPlan_id		int				FOREIGN KEY REFERENCES mealPlan(mealPlan_id),
	meal_id			int				FOREIGN KEY REFERENCES meal(meal_id)

	CONSTRAINT pk_mealPlan_meal PRIMARY KEY CLUSTERED (mealPlan_id, meal_id)
);

CREATE TABLE meal_recipe
(
	meal_id			int				FOREIGN KEY REFERENCES meal(meal_id),
	recipe_id		int				FOREIGN KEY REFERENCES recipe(recipe_id)

	CONSTRAINT pk_meal_recipe PRIMARY KEY CLUSTERED (meal_id, recipe_id)
);

SET IDENTITY_INSERT unit ON;

INSERT INTO unit (unit_id, unit) VALUES (1, 'item(s)');
INSERT INTO unit (unit_id, unit) VALUES (2, 'slice(s)');
INSERT INTO unit (unit_id, unit) VALUES (3, 'piece(s)');
INSERT INTO unit (unit_id, unit) VALUES (4, 'fl. oz.');
INSERT INTO unit (unit_id, unit) VALUES (5, 'cup');
INSERT INTO unit (unit_id, unit) VALUES (6, 'pint');
INSERT INTO unit (unit_id, unit) VALUES (7, 'quart');
INSERT INTO unit (unit_id, unit) VALUES (8, 'gallon');
INSERT INTO unit (unit_id, unit) VALUES (9, 'tsp.');
INSERT INTO unit (unit_id, unit) VALUES (10, 'Tbsp.');
INSERT INTO unit (unit_id, unit) VALUES (11, 'oz.');
INSERT INTO unit (unit_id, unit) VALUES (12, 'lb.');
INSERT INTO unit (unit_id, unit) VALUES (13, 'pinch');
INSERT INTO unit (unit_id, unit) VALUES (14, 'dash');
INSERT INTO unit (unit_id, unit) VALUES (15, 'jigger');

SET IDENTITY_INSERT unit OFF;

SET IDENTITY_INSERT number ON;

INSERT INTO number (number_id, number) VALUES (0, '0');
INSERT INTO number (number_id, number) VALUES (1, '1');
INSERT INTO number (number_id, number) VALUES (2, '2');
INSERT INTO number (number_id, number) VALUES (3, '3');
INSERT INTO number (number_id, number) VALUES (4, '4');
INSERT INTO number (number_id, number) VALUES (5, '5');
INSERT INTO number (number_id, number) VALUES (6, '6');
INSERT INTO number (number_id, number) VALUES (7, '7');
INSERT INTO number (number_id, number) VALUES (8, '8');
INSERT INTO number (number_id, number) VALUES (9, '9');
INSERT INTO number (number_id, number) VALUES (10, '10');
INSERT INTO number (number_id, number) VALUES (11, '11');
INSERT INTO number (number_id, number) VALUES (12, '12');
INSERT INTO number (number_id, number) VALUES (13, '13');
INSERT INTO number (number_id, number) VALUES (14, '14');
INSERT INTO number (number_id, number) VALUES (15, '15');
INSERT INTO number (number_id, number) VALUES (16, '16');
INSERT INTO number (number_id, number) VALUES (17, '17');
INSERT INTO number (number_id, number) VALUES (18, '18');
INSERT INTO number (number_id, number) VALUES (19, '19');
INSERT INTO number (number_id, number) VALUES (20, '20');

SET IDENTITY_INSERT number OFF;

SET IDENTITY_INSERT fraction ON

INSERT INTO fraction (fraction_id, fraction) VALUES (0, '-');
INSERT INTO fraction (fraction_id, fraction) VALUES (1, '1/8');
INSERT INTO fraction (fraction_id, fraction) VALUES (2, '1/4');
INSERT INTO fraction (fraction_id, fraction) VALUES (3, '1/3');
INSERT INTO fraction (fraction_id, fraction) VALUES (4, '3/8');
INSERT INTO fraction (fraction_id, fraction) VALUES (5, '1/2');
INSERT INTO fraction (fraction_id, fraction) VALUES (6, '5/8');
INSERT INTO fraction (fraction_id, fraction) VALUES (7, '2/3');
INSERT INTO fraction (fraction_id, fraction) VALUES (8, '3/4');
INSERT INTO fraction (fraction_id, fraction) VALUES (9, '7/8');

SET IDENTITY_INSERT fraction OFF

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

INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (1, 'Peanut Butter and Jelly Sandwich', 'The classic sandwich for kids of all ages', '1) Spread peanut butter on first slice of bread. / 2) Spread jelly on second slice of bread. / 3) Put both pieces of bread together with peanut butter and jelly sides facing inward. / 4) Cut into rectangles or triangles. / 5) Relive the simple joy of childhood!', 2, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (2, 'Cheesy Scrambled Eggs', 'A breakfast favorite', '1) Heat 10" skillet on medium-low heat. / 2) Crack eggs into bowl, discard shells, and add milk. / 3) Whisk eggs and milk vigorously until pale and slightly frothy. / 4) Grate or slice cheese as desired. / 5) Chop onion finely, reserving some green for garnish. / 6) Melt butter in skillet. / 7) Sautee onion in butter for 30 seconds to 1 minute. / 8) Pour eggs into skillet and stir continuously with wooden spoon or chopsticks for about 90 seconds, or until mostly solid, but still slightly runny. / 9) Add cheese to top of eggs and cover for 20-30 seconds to melt. / 10) Scoop eggs onto plate, garnish with reserved green onion rounds, and serve.', 5, 3)

SET IDENTITY_INSERT recipe OFF;

SET IDENTITY_INSERT meal ON;

INSERT INTO meal (meal_id, meal_name) VALUES (1, 'Example Meal');

SET IDENTITY_INSERT meal OFF;

INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 1, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 2, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 3, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 4, 1, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 5, 10, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 6, 1, 0, 3);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 7, 11, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 8, 10, 2, 0);

COMMIT TRANSACTION;
