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
	day1			int				NULL				FOREIGN KEY REFERENCES day(day_id),
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

CREATE TABLE category
(
	category_id		int				IDENTITY			PRIMARY KEY,
	category_name	varchar(50)		NOT NULL
);

CREATE TABLE recipe_category
(
	recipe_id		int,
	category_id		int

	CONSTRAINT pk_recipe_category PRIMARY KEY CLUSTERED (recipe_id, category_id)
);

SET IDENTITY_INSERT unit ON;

INSERT INTO unit (unit_id, unit) VALUES (1, '-');
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
INSERT INTO unit (unit_id, unit) VALUES (16, 'splash');
INSERT INTO unit (unit_id, unit) VALUES (17, 'part');
INSERT INTO unit (unit_id, unit) VALUES (18, 'extra-large');
INSERT INTO unit (unit_id, unit) VALUES (19, 'large');
INSERT INTO unit (unit_id, unit) VALUES (20, 'medium');
INSERT INTO unit (unit_id, unit) VALUES (21, 'small');
INSERT INTO unit (unit_id, unit) VALUES (22, 'leaf(ves)');
INSERT INTO unit (unit_id, unit) VALUES (23, 'head(s)');
INSERT INTO unit (unit_id, unit) VALUES (24, 'box(s)');
INSERT INTO unit (unit_id, unit) VALUES (25, '6 oz. can(s)');
INSERT INTO unit (unit_id, unit) VALUES (26, '10.5 oz. can(s)');
INSERT INTO unit (unit_id, unit) VALUES (27, '14.5 oz. can(s)');
INSERT INTO unit (unit_id, unit) VALUES (28, '28 oz. can(s)');
INSERT INTO unit (unit_id, unit) VALUES (29, 'loaf(ves)');

SET IDENTITY_INSERT unit OFF;

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
INSERT INTO number (number_id, number) VALUES (21, '21');
INSERT INTO number (number_id, number) VALUES (22, '22');
INSERT INTO number (number_id, number) VALUES (23, '23');
INSERT INTO number (number_id, number) VALUES (24, '24');
INSERT INTO number (number_id, number) VALUES (25, '25');
INSERT INTO number (number_id, number) VALUES (26, '26');
INSERT INTO number (number_id, number) VALUES (27, '27');
INSERT INTO number (number_id, number) VALUES (28, '28');
INSERT INTO number (number_id, number) VALUES (29, '29');
INSERT INTO number (number_id, number) VALUES (30, '30');
INSERT INTO number (number_id, number) VALUES (31, '31');
INSERT INTO number (number_id, number) VALUES (32, '32');
INSERT INTO number (number_id, number) VALUES (33, '33');
INSERT INTO number (number_id, number) VALUES (34, '34');
INSERT INTO number (number_id, number) VALUES (35, '35');
INSERT INTO number (number_id, number) VALUES (36, '36');
INSERT INTO number (number_id, number) VALUES (37, '37');
INSERT INTO number (number_id, number) VALUES (38, '38');
INSERT INTO number (number_id, number) VALUES (39, '39');
INSERT INTO number (number_id, number) VALUES (40, '40');
INSERT INTO number (number_id, number) VALUES (41, '41');
INSERT INTO number (number_id, number) VALUES (42, '42');
INSERT INTO number (number_id, number) VALUES (43, '43');
INSERT INTO number (number_id, number) VALUES (44, '44');
INSERT INTO number (number_id, number) VALUES (45, '45');
INSERT INTO number (number_id, number) VALUES (46, '46');
INSERT INTO number (number_id, number) VALUES (47, '47');
INSERT INTO number (number_id, number) VALUES (48, '48');
INSERT INTO number (number_id, number) VALUES (49, '49');
INSERT INTO number (number_id, number) VALUES (50, '50');
INSERT INTO number (number_id, number) VALUES (51, '51');
INSERT INTO number (number_id, number) VALUES (52, '52');
INSERT INTO number (number_id, number) VALUES (53, '53');
INSERT INTO number (number_id, number) VALUES (54, '54');
INSERT INTO number (number_id, number) VALUES (55, '55');
INSERT INTO number (number_id, number) VALUES (56, '56');
INSERT INTO number (number_id, number) VALUES (57, '57');
INSERT INTO number (number_id, number) VALUES (58, '58');
INSERT INTO number (number_id, number) VALUES (59, '59');
INSERT INTO number (number_id, number) VALUES (60, '60');
INSERT INTO number (number_id, number) VALUES (61, '61');
INSERT INTO number (number_id, number) VALUES (62, '62');
INSERT INTO number (number_id, number) VALUES (63, '63');
INSERT INTO number (number_id, number) VALUES (64, '64');
INSERT INTO number (number_id, number) VALUES (65, '65');
INSERT INTO number (number_id, number) VALUES (66, '66');
INSERT INTO number (number_id, number) VALUES (67, '67');
INSERT INTO number (number_id, number) VALUES (68, '68');
INSERT INTO number (number_id, number) VALUES (69, '69');
INSERT INTO number (number_id, number) VALUES (70, '70');
INSERT INTO number (number_id, number) VALUES (71, '71');
INSERT INTO number (number_id, number) VALUES (72, '72');
INSERT INTO number (number_id, number) VALUES (73, '73');
INSERT INTO number (number_id, number) VALUES (74, '74');
INSERT INTO number (number_id, number) VALUES (75, '75');
INSERT INTO number (number_id, number) VALUES (76, '76');
INSERT INTO number (number_id, number) VALUES (77, '77');
INSERT INTO number (number_id, number) VALUES (78, '78');
INSERT INTO number (number_id, number) VALUES (79, '79');
INSERT INTO number (number_id, number) VALUES (80, '80');
INSERT INTO number (number_id, number) VALUES (81, '81');
INSERT INTO number (number_id, number) VALUES (82, '82');
INSERT INTO number (number_id, number) VALUES (83, '83');
INSERT INTO number (number_id, number) VALUES (84, '84');
INSERT INTO number (number_id, number) VALUES (85, '85');
INSERT INTO number (number_id, number) VALUES (86, '86');
INSERT INTO number (number_id, number) VALUES (87, '87');
INSERT INTO number (number_id, number) VALUES (88, '88');
INSERT INTO number (number_id, number) VALUES (89, '89');
INSERT INTO number (number_id, number) VALUES (90, '90');
INSERT INTO number (number_id, number) VALUES (91, '91');
INSERT INTO number (number_id, number) VALUES (92, '92');
INSERT INTO number (number_id, number) VALUES (93, '93');
INSERT INTO number (number_id, number) VALUES (94, '94');
INSERT INTO number (number_id, number) VALUES (95, '95');
INSERT INTO number (number_id, number) VALUES (96, '96');
INSERT INTO number (number_id, number) VALUES (97, '97');
INSERT INTO number (number_id, number) VALUES (98, '98');
INSERT INTO number (number_id, number) VALUES (99, '99');

SET IDENTITY_INSERT number OFF;

SET IDENTITY_INSERT ingredient ON;

INSERT INTO ingredient (ingredient_id, name) VALUES (1, 'multigrain bread');
INSERT INTO ingredient (ingredient_id, name) VALUES (2, 'peanut butter, crunchy');
INSERT INTO ingredient (ingredient_id, name) VALUES (3, 'grape jelly');
INSERT INTO ingredient (ingredient_id, name) VALUES (4, 'egg, large');
INSERT INTO ingredient (ingredient_id, name) VALUES (5, 'milk, 2%');
INSERT INTO ingredient (ingredient_id, name) VALUES (6, 'green onion');
INSERT INTO ingredient (ingredient_id, name) VALUES (7, 'cheddar cheese, sharp');
INSERT INTO ingredient (ingredient_id, name) VALUES (8, 'butter, salted');
INSERT INTO ingredient (ingredient_id, name) VALUES (9, 'heavy whipping cream');
INSERT INTO ingredient (ingredient_id, name) VALUES (10, 'light brown sugar');
INSERT INTO ingredient (ingredient_id, name) VALUES (11, 'white sugar, granulated');
INSERT INTO ingredient (ingredient_id, name) VALUES (12, 'egg, medium');
INSERT INTO ingredient (ingredient_id, name) VALUES (13, 'dark brown sugar');
INSERT INTO ingredient (ingredient_id, name) VALUES (14, 'salt');
INSERT INTO ingredient (ingredient_id, name) VALUES (15, 'black pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (16, 'chicken breast');
INSERT INTO ingredient (ingredient_id, name) VALUES (17, 'ground beef');
INSERT INTO ingredient (ingredient_id, name) VALUES (18, 'baker''s chocolate');
INSERT INTO ingredient (ingredient_id, name) VALUES (19, 'garlic clove');
INSERT INTO ingredient (ingredient_id, name) VALUES (20, 'garlic powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (21, 'vanilla extract');
INSERT INTO ingredient (ingredient_id, name) VALUES (22, 'vanilla paste');
INSERT INTO ingredient (ingredient_id, name) VALUES (23, 'vanilla bean');
INSERT INTO ingredient (ingredient_id, name) VALUES (24, 'egg, extra large');
INSERT INTO ingredient (ingredient_id, name) VALUES (25, 'egg, small');
INSERT INTO ingredient (ingredient_id, name) VALUES (26, 'bacon, thick cut');
INSERT INTO ingredient (ingredient_id, name) VALUES (27, 'cane syrup, pure');
INSERT INTO ingredient (ingredient_id, name) VALUES (28, '10" pie crust');
INSERT INTO ingredient (ingredient_id, name) VALUES (29, 'pecans, halved');
INSERT INTO ingredient (ingredient_id, name) VALUES (30, 'corn syrup, white');
INSERT INTO ingredient (ingredient_id, name) VALUES (31, 'white bread');
INSERT INTO ingredient (ingredient_id, name) VALUES (32, 'wheat bread');
INSERT INTO ingredient (ingredient_id, name) VALUES (33, 'cocoa powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (34, 'lemon juice');
INSERT INTO ingredient (ingredient_id, name) VALUES (35, 'lime juice');
INSERT INTO ingredient (ingredient_id, name) VALUES (36, '1800 Silver tequila');
INSERT INTO ingredient (ingredient_id, name) VALUES (37, 'sweetened lime juice');
INSERT INTO ingredient (ingredient_id, name) VALUES (38, 'Gran Marnier orange liqueur');
INSERT INTO ingredient (ingredient_id, name) VALUES (39, 'ice, cubed');
INSERT INTO ingredient (ingredient_id, name) VALUES (40, 'ice, crushed');
INSERT INTO ingredient (ingredient_id, name) VALUES (41, 'fresh basil');
INSERT INTO ingredient (ingredient_id, name) VALUES (42, 'tomatoes');
INSERT INTO ingredient (ingredient_id, name) VALUES (43, 'roma tomatoes');
INSERT INTO ingredient (ingredient_id, name) VALUES (44, 'peanut butter, creamy');
INSERT INTO ingredient (ingredient_id, name) VALUES (45, 'ham, sliced');
INSERT INTO ingredient (ingredient_id, name) VALUES (46, 'smoked turkey, sliced');
INSERT INTO ingredient (ingredient_id, name) VALUES (47, 'havarti cheese, sliced');
INSERT INTO ingredient (ingredient_id, name) VALUES (48, 'mustard, brown');
INSERT INTO ingredient (ingredient_id, name) VALUES (49, 'powdered sugar');
INSERT INTO ingredient (ingredient_id, name) VALUES (50, 'cake flour');
INSERT INTO ingredient (ingredient_id, name) VALUES (51, 'White Lily self-rising flour');
INSERT INTO ingredient (ingredient_id, name) VALUES (52, 'green leaf lettuce');
INSERT INTO ingredient (ingredient_id, name) VALUES (53, 'red leaf lettuce');
INSERT INTO ingredient (ingredient_id, name) VALUES (54, 'romaine lettuce');
INSERT INTO ingredient (ingredient_id, name) VALUES (55, 'baby spinach');
INSERT INTO ingredient (ingredient_id, name) VALUES (56, 'baby spring mix');
INSERT INTO ingredient (ingredient_id, name) VALUES (57, 'carrot, whole');
INSERT INTO ingredient (ingredient_id, name) VALUES (58, 'baby carrot');
INSERT INTO ingredient (ingredient_id, name) VALUES (59, 'grated carrots');
INSERT INTO ingredient (ingredient_id, name) VALUES (60, 'onion');
INSERT INTO ingredient (ingredient_id, name) VALUES (61, 'red bell pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (62, 'green bell pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (63, 'banana');
INSERT INTO ingredient (ingredient_id, name) VALUES (64, 'apple, Honeycrisp');
INSERT INTO ingredient (ingredient_id, name) VALUES (65, 'onion powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (66, 'poblano pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (67, 'jalapeño pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (68, 'mayonnaise');
INSERT INTO ingredient (ingredient_id, name) VALUES (69, 'cherry tomatoes');
INSERT INTO ingredient (ingredient_id, name) VALUES (70, 'fresh mozzarella');
INSERT INTO ingredient (ingredient_id, name) VALUES (71, 'mozzarella pearls');
INSERT INTO ingredient (ingredient_id, name) VALUES (72, 'shredded mozzarella');
INSERT INTO ingredient (ingredient_id, name) VALUES (73, 'parmesan, shaved');
INSERT INTO ingredient (ingredient_id, name) VALUES (74, 'parmesan, grated');
INSERT INTO ingredient (ingredient_id, name) VALUES (75, 'marinara');
INSERT INTO ingredient (ingredient_id, name) VALUES (76, 'pizza sauce');
INSERT INTO ingredient (ingredient_id, name) VALUES (77, 'naan bread, mini');
INSERT INTO ingredient (ingredient_id, name) VALUES (78, 'pepperoni');
INSERT INTO ingredient (ingredient_id, name) VALUES (79, 'smoked gouda cheese, sliced');
INSERT INTO ingredient (ingredient_id, name) VALUES (80, 'colby jack cheese');
INSERT INTO ingredient (ingredient_id, name) VALUES (81, 'cream cheese');
INSERT INTO ingredient (ingredient_id, name) VALUES (82, 'pork shoulder');
INSERT INTO ingredient (ingredient_id, name) VALUES (83, 'ribeye steak');
INSERT INTO ingredient (ingredient_id, name) VALUES (84, 'prime rib roast');
INSERT INTO ingredient (ingredient_id, name) VALUES (85, 'chicken wings, whole');
INSERT INTO ingredient (ingredient_id, name) VALUES (86, 'peanut oil');
INSERT INTO ingredient (ingredient_id, name) VALUES (87, 'olive oil, extra virgin (evoo)');
INSERT INTO ingredient (ingredient_id, name) VALUES (88, 'balsamic vinegar');
INSERT INTO ingredient (ingredient_id, name) VALUES (89, 'mustard, dijon');
INSERT INTO ingredient (ingredient_id, name) VALUES (90, 'sour cream');
INSERT INTO ingredient (ingredient_id, name) VALUES (91, 'teriyaki sauce');
INSERT INTO ingredient (ingredient_id, name) VALUES (92, 'soy sauce');
INSERT INTO ingredient (ingredient_id, name) VALUES (93, 'sriracha sauce');
INSERT INTO ingredient (ingredient_id, name) VALUES (94, 'yum yum sauce');
INSERT INTO ingredient (ingredient_id, name) VALUES (95, 'lasagna noodles, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (96, 'ricotta cheese');
INSERT INTO ingredient (ingredient_id, name) VALUES (97, 'mozzarella, sliced');
INSERT INTO ingredient (ingredient_id, name) VALUES (98, 'tomato paste');
INSERT INTO ingredient (ingredient_id, name) VALUES (99, 'fresh oregano');
INSERT INTO ingredient (ingredient_id, name) VALUES (100, 'fresh parsley');
INSERT INTO ingredient (ingredient_id, name) VALUES (101, 'tomatoes, diced');
INSERT INTO ingredient (ingredient_id, name) VALUES (102, 'tomato sauce, canned');
INSERT INTO ingredient (ingredient_id, name) VALUES (103, 'italian sausage');
INSERT INTO ingredient (ingredient_id, name) VALUES (104, 'sweet italian sausage');
INSERT INTO ingredient (ingredient_id, name) VALUES (105, 'hot italian sausage');
INSERT INTO ingredient (ingredient_id, name) VALUES (106, 'white rice, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (107, 'brown rice, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (108, 'basmati rice, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (109, 'jasmine rice, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (110, 'plain yogurt');
INSERT INTO ingredient (ingredient_id, name) VALUES (111, 'chili powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (112, 'ground chipotle pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (113, 'ground ancho pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (114, 'Goya seasoning packet');
INSERT INTO ingredient (ingredient_id, name) VALUES (115, 'adobo');
INSERT INTO ingredient (ingredient_id, name) VALUES (116, 'ground cumin');
INSERT INTO ingredient (ingredient_id, name) VALUES (117, 'fresh thyme');
INSERT INTO ingredient (ingredient_id, name) VALUES (118, 'honey');
INSERT INTO ingredient (ingredient_id, name) VALUES (119, 'maple syrup');
INSERT INTO ingredient (ingredient_id, name) VALUES (120, 'garlic, minced');
INSERT INTO ingredient (ingredient_id, name) VALUES (121, 'strawberry preserves');
INSERT INTO ingredient (ingredient_id, name) VALUES (122, 'cardomom plum jam');
INSERT INTO ingredient (ingredient_id, name) VALUES (123, 'green beans');
INSERT INTO ingredient (ingredient_id, name) VALUES (124, 'pineapple, whole');
INSERT INTO ingredient (ingredient_id, name) VALUES (125, 'pineapple, cored');
INSERT INTO ingredient (ingredient_id, name) VALUES (126, 'strawberries');
INSERT INTO ingredient (ingredient_id, name) VALUES (127, 'blueberries');
INSERT INTO ingredient (ingredient_id, name) VALUES (128, 'canteloupe');
INSERT INTO ingredient (ingredient_id, name) VALUES (129, 'honeydew melon');
INSERT INTO ingredient (ingredient_id, name) VALUES (130, 'corn meal');
INSERT INTO ingredient (ingredient_id, name) VALUES (131, 'baking powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (132, 'baking soda');
INSERT INTO ingredient (ingredient_id, name) VALUES (133, 'hot pork sausage');
INSERT INTO ingredient (ingredient_id, name) VALUES (134, 'mild pork sausage');
INSERT INTO ingredient (ingredient_id, name) VALUES (135, 'breakfast sausage, links');
INSERT INTO ingredient (ingredient_id, name) VALUES (136, 'smoked paprika');
INSERT INTO ingredient (ingredient_id, name) VALUES (137, 'ground cayenne pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (138, 'saigon cinnamon');
INSERT INTO ingredient (ingredient_id, name) VALUES (139, 'Maker''s Mark bourbon');
INSERT INTO ingredient (ingredient_id, name) VALUES (140, 'Diet Dr. Pepper');
INSERT INTO ingredient (ingredient_id, name) VALUES (141, 'graham crackers');
INSERT INTO ingredient (ingredient_id, name) VALUES (142, 'mini marshmallows');
INSERT INTO ingredient (ingredient_id, name) VALUES (143, 'pecans');
INSERT INTO ingredient (ingredient_id, name) VALUES (144, 'canned tuna in water');
INSERT INTO ingredient (ingredient_id, name) VALUES (145, 'celery');
INSERT INTO ingredient (ingredient_id, name) VALUES (146, 'avocado');
INSERT INTO ingredient (ingredient_id, name) VALUES (147, 'vegetable oil');
INSERT INTO ingredient (ingredient_id, name) VALUES (148, 'canola oil');
INSERT INTO ingredient (ingredient_id, name) VALUES (149, 'self-rising flour');
INSERT INTO ingredient (ingredient_id, name) VALUES (150, 'all-purpose flour');
INSERT INTO ingredient (ingredient_id, name) VALUES (151, 'buttermilk');
INSERT INTO ingredient (ingredient_id, name) VALUES (152, 'pecans, chopped');
INSERT INTO ingredient (ingredient_id, name) VALUES (153, 'elbow noodles, dry');
INSERT INTO ingredient (ingredient_id, name) VALUES (154, 'cream of chicken soup, condensed');
INSERT INTO ingredient (ingredient_id, name) VALUES (155, 'mustard powder');
INSERT INTO ingredient (ingredient_id, name) VALUES (156, 'gruyere cheese');
INSERT INTO ingredient (ingredient_id, name) VALUES (157, 'bay leaf');
INSERT INTO ingredient (ingredient_id, name) VALUES (158, 'white sugar, superfine');
INSERT INTO ingredient (ingredient_id, name) VALUES (159, 'flank steak');
INSERT INTO ingredient (ingredient_id, name) VALUES (160, 'cucumber');
INSERT INTO ingredient (ingredient_id, name) VALUES (161, 'baby bella mushroom');
INSERT INTO ingredient (ingredient_id, name) VALUES (162, 'radish');
INSERT INTO ingredient (ingredient_id, name) VALUES (163, 'italian bread');
INSERT INTO ingredient (ingredient_id, name) VALUES (164, 'english muffin');
INSERT INTO ingredient (ingredient_id, name) VALUES (165, 'fruit preserves');
INSERT INTO ingredient (ingredient_id, name) VALUES (166, 'Kerrygold butter');
INSERT INTO ingredient (ingredient_id, name) VALUES (167, 'butter, unsalted');
INSERT INTO ingredient (ingredient_id, name) VALUES (168, 'champagne');
INSERT INTO ingredient (ingredient_id, name) VALUES (169, 'orange juice');
INSERT INTO ingredient (ingredient_id, name) VALUES (170, 'orange');
--INSERT INTO ingredient (ingredient_id, name) VALUES (171, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (172, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (173, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (174, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (175, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (176, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (177, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (178, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (179, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (180, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (181, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (182, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (183, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (184, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (185, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (186, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (187, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (188, 'zzz');
--INSERT INTO ingredient (ingredient_id, name) VALUES (189, 'zzz');

SET IDENTITY_INSERT ingredient OFF;

SET IDENTITY_INSERT recipe ON;

INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (1, 'Peanut Butter and Jelly Sandwich', 'The classic sandwich for kids of all ages', '1) Spread peanut butter on first slice of bread. /2) Spread jelly on second slice of bread. /3) Put both pieces of bread together with peanut butter and jelly sides facing inward. /4) Cut into rectangles or triangles. /5) Relive the simple joy of childhood!', 2, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (2, 'Cheesy Scrambled Eggs', 'A breakfast favorite', '1) Heat 10" skillet on medium-low heat. /2) Crack eggs into bowl, discard shells, and add milk. /3) Whisk eggs and milk vigorously until pale and slightly frothy. /4) Grate or slice cheese as desired. /5) Chop onion finely, reserving some green for garnish. /6) Melt butter in skillet. /7) Sautee onion in butter for 30 seconds to 1 minute. /8) Pour eggs into skillet and stir continuously with wooden spoon or chopsticks for about 90 seconds, or until mostly solid, but still slightly runny. /9) Add cheese to top of eggs and cover for 20-30 seconds to melt. /10) Scoop eggs onto plate, garnish with reserved green onion rounds, and serve.', 5, 3);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (3, 'Easy Caprese Salad', 'Simple italian goodness, arguably healthy', '1) Wash basil and tomatoes. /2) Rough chop basil or cut into ~1/4" wide strips. /3) Cut tomatoes in half, if desired. /4) Cut mozz into bite size pieces. /5) Arrange artfully on plate or in bowl. /6) Drizzle with balsamic reduction, or Simple Balsamic Vinaigrette (see recipes).', 10, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (4, 'Buttermilk Pancakes', 'These are not your mother''s pancakes. They''re my grandmother''s pancakes!', '1) Add buttermilk, oil, vanilla extract, and egg to 1qt measuring cup, then mix thoroughly. /2) Add flour 1/3 cup at a time, gently mixing. DO NOT OVERMIX. There should be lumps remaining when you are finished. /3) Preheat non-stick pan to medium, or griddle to 350F. /4) Flip bubbles appear all over upper surface of batter (2-3 minutes). /5) Cook for another 2-3 minutes. /6) Serve with butter, syrup, fruit, powdered sugar, peanut butter, or whatever you like on pancakes.', 5, 5);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (5, 'Naan Pizza', 'French bread pizza is so passe.', '1) Preheat toaster oven to 400F. /2) Spread your favorite sauce on naan. /3) Sprinkle mozz on naan. /4) Evenly distribute pepperoni slices across naan. /5) Sprinkle grated parmesan on top of pepperoni. /6) Cook for 10 minutes, or until cheese in center begins to brown slightly.', 3, 10);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (6, 'Crock Pot Mac & Cheese', 'So creamy, so cheesy, so easy!', '1) Boil macaroni noodles for 5 minutes, until very al dente. Drain and rinse under cold water. /2) Grate cheddar and gruyere cheeses. /3) Combine all ingredients in crock pot adn stir thoroughly. /4) Cook on high for 2 hours, or on low for 3 hours, stirring once or twice.', 10, 120);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (7, 'Pulled Pork in the Oven', 'Low and slow, no smoker required. *This recipe approved by a genuine Alabama BBQ Fiend!', 'Spice Rub: /1) Combine all spices (use 1 Tbsp. salt and 1/2 cup brown sugar, the rest is for the brine), mix thoroughly. / /Brine: /1) Add remaining salt to room temperature water, stirring until dissolved, then add 3 Tbsp. spice rub, remaining brown sugar, and bay leaves. /2) Rinse the pork shoulder and place in 2 gallon Ziploc bag, or other container of appropriate size. /3) Pour brine solution into container with pork until pork is completely covered. /4) Brine pork for at least 12 hours. I prefer 48 - 96 hours. / /Cooking: /1) Heat oven to 225F. /2) Remove pork shoulder from brine and gently pat dry. /3) Completely coat pork shoulder with spice rub, being sure to get into any crevices or folds. /4) Place pork shoulder in baking pan that allows an inch or two between all sides of pan and shoulder. /5) Place pan in oven, uncovered, and cook until internal temperature reaches 200F. THIS WILL TAKE A LONG TIME, 10-12 HOURS!! For best results, use an oven-safe temperature probe with an external readout and a temperature alarm. Insert the probe into the thickest portion of the meat, not allowing it to touch the bone. /6) Once the internal temperature reaches 200F, turn off the oven and, leaving the oven closed, allow the internal temperature of the meat to drop to 170F. THIS WILL TAKE ABOUT 2 HOURS. /7) Once the meat has cooled, transfer meat to a large bowl or pan and shred with forks or shredding claws. /8) Save juice from cooking pan, pouring off the fat that collects on top. This can be used for a variety of purposes, and is too delicious to let go to waste!', 740, 840);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (8, 'Yvonne''s Lasagna', 'Our Italian friends were green with envy.', '1) Coarsely chop onions. /2) Break up and brown sausage for 5 minutes, pour off fat, then add onions to sausage, cooking and occasionally stirring until onions are soft. /3) While sausage and onions brown, chop basil, oregano, and thyme. /4) Add tomato paste, diced tomatoes, 28 oz of tomato sauce, minced garlic, and 2/3 of chopped herbs to sausage/onion mixture and simmer for 30-45 minutes. /5) As desired consistency is reached, add salt, pepper, and additional garlic and herbs to taste. 6) While sauce simmmers, bring large pot of water to a boil, add noodles, cook until slightly firmer than al dente, then remove from heat source. 7) When sauce is ready, preheat oven to 350F and grease a 9" x 13" glass baking pan. /8) Place a single layer of noodles in the bottom of the pan. /9) Spread a thin layer of ricotta cheese over noodles. /10) Ladle a thin layer of sauce over ricotta cheese. /11) Cover sauce with slices of mozzarella cheese. 12) Repeat steps 8 - 11 until pan is full, making top layer of sauce and cheeses extra thick! /13) Bake uncovered for 30 minutes. Allow to cool for 5-10 minutes before cutting and serving.', 15, 90);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (9, 'Garlic Bread', 'Perfect with pastas', '1) Allow butter to soften (time will depend on room temperature). /2) Thoroughly combine butter and minced garlic. /3) Slice bread to desired thickness (typically 1/2"-1"). /4) Spread 2 tsp. butter/garlic mixture on each slice. /5) Arrange slices on baking sheet and sprinkle with shaved parmesan. /6) Set broiler on high and place baking sheet on top rack of oven for 3 minutes, or until desired crunchiness is reached.', 5, 3);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (10, 'Jalen''s Killer Breakfast Sandwich', 'If you eat too many, they will literally kill you.', '1) Preheat pan to medium low or griddle to 325F. /2) Cut bacon strips into halves and cook until desired firmness is reached. 3) Spread 2 tsp. butter on each slice of bread. /4) Whip egg and milk together until mixture is uniform. /5) Cook egg mixture in same pan/griddle as bacon for about 2 minutes without stirring. Flip sheet of egg if necessary to fully cook, but do not mix in pan. /6) Fold egg into approximate shape of bread, squeeze mustard onto egg, then place bread on top of folded egg, dry side down. /7) Raise temperature to medium-high/375F. /8) Using spatula, carefully lift egg and flip so bread side is down then cook for 2-3 minutes. /9) In the meantime, put bacon slices on top of egg, then put cheese on top of bacon. 10) Spread mayo on dry side of bread, taking care not to scrape butter off of opposite side, then place bread on top of cheese, mayo side down. /11) Use spatula to flip sandwich over in pan/griddle (may need extra spatula or gloved hand to keep it from falling apart) and cook for 2-3 minutes.', 5, 15);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (11, 'Creme Brulee', 'Luxurious culinary decadence arises from simplicity.', '1) Pour heavy cream into 1.5 quart pot, add vanilla paste, and heat on low or medium low until hot, but DO NOT ALLOW CREAM TO SIMMER OR BOIL. /2) While cream is heating separate yolks from eggs (keep or discard egg whites as desired). /3) Combine egg yolks and granulated sugar in bowl, then whisk until pale yellow and fluffy. /4) Preheat oven to 300F. /5) Once cream is hot, pour through a strainer to remove any solids. /6) Pour 1 cup of hot cream VERY SLOWLY into egg/sugar mixture, whisking constantly. This must be done slowly to temper the egss and prevent the hot cream from cooking the eggs. /7) Continue adding cream to egg mixture, whisking as you pour. This can be done more quickly than the initial cup. /8) Spoon foam from top of mixture or pour into a fat separator. /9) Place ramekins in pan (or pans), then fill ramekins with cream/egg mixture, leaving at least 1/8" from top of ramekin to surface. /10) Pour water into pan around ramekins, filling to half the height of the ramekins. /11) Cook creme on middle rack for 30 minutes, until surface is jiggly but just beginning to firm up. /12) Place ramekins on cooling racks until just cool enough to touch. /13) Cover each ramekin tightly with plastic wrap, then place in refrigerator for at least 2 hours, up to 3 days. /14) When ready to serve, pull ramekins from refrigerator and remove plastic wrap, being careful not to let condensate drip onto creme surface. /15) Allow ramekins to sit on counter for 10-20 minutes, then evenly sprinkle 1 Tbsp superfine sugar onto surface of each (more if using wide, shallow ramekins). /16) Use kitchen torch to melt and slightly brown sugar on surface. Keep flame angled across surface, rather than pointing straight down, and use gentle swirling motions to prevent too much heat transfer into creme. /17) Ramekins will be HOT from the flame, so let them cool on counter for 5 minutes before serving.', 130, 40);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (12, 'Spinach and Bacon Quiche', 'Some people think quiches are froo froo. Teach them otherwise with this one!', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (13, 'Jesse''s Every-Day Club Sandwich', '"Yeah, I eat this nearly every day for lunch. What of it?" -Jesse', '1) Spread mayo on one piece of break, mustard on the other. /2) Place two slices of ham on mustard. /3) Place two slices of turkey on ham. /3) Place cheese on top of turkey. /4) Arrange tomato slices on top of cheese. /5) Place lettuce on top of tomato. /6) Place bread with mayo on top of lettuce, mayo side down.', 5, 0)
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (14, 'Peanut Butter and Banana Toast', 'The name says it all... almost.', '1) Toast your favorite bread. /2) Spread peanut butter on toast. /3) Slice banana and layer above peanut butter. /4) For a real treat, drizzle with honey!', 2, 2);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (15, 'Oreo Cheesecake', 'You really need to like Oreo cookies for this one.', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (16, 'Slow-Cooked Beef and Root Veggies with Stout Sauce', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (17, 'Southern Banana Pudding', 'Is there Northern Banana Pudding? If so, this is better.', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (18, 'Garlic Sauteed Green Beans', 'How can this be so good AND so good for you?', '1) Wash, string, and cut beans to your preferred state. /2) In large bowl, toss beans, garlic, salt, and pepper in olive oil. /3) Sautee on medium to medium-high heat until desired softness is reached. /*I typically sautee them for about 10 minutes, leaving them pliable but crisp', 10, 10);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (19, 'Steamed Broccoli', 'Does this really need to be explained? If so, keep reading.', '1) Cut head broccoli into pieces and place in 1.5 quart pot with lid. /2) Add water to pot. /3) Place lid on pot and heat for 5 minutes on high. /4) Pour off excess water and season broccoli to taste.', 2, 5);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (20, 'Garden Salad', 'Fresh and colorful! Add your own faves to this solid base.', '1) Wash all vegetables. /2) Chop vegetables into bite size pieces. /3) Add ingredients to large bowl and toss. /4) Add your favorite dressing to the salad or on the side.', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (21, 'Mega Salad', 'ALL YOUR VEG ARE BELONG TO US', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (22, 'Simple Balsamic Vinaigrette', 'The simplest balsamic vinaigrette worth making', '1) Mince or grate garlic cloves. /2) Combine all ingredients except pepper and mix thoroughly. /3) Add pepper to taste. /4) If too acidic, add a little more maple syrup. /5) Serve immediately, mixing before each use. /6) Will keep for about a week if refrigerated (allow to come to room temp before serving).', 5, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (23, 'Ola Mae''s BEST Pecan Pie', 'Easy to learn, difficult to master.', '', 20, 45);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (24, 'Fresh Fruit Bowl', 'Get Fruit. Cut fruit. Put cut fruit in bowl. Eat.', 'See description.', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (25, 'Jalen''s Teriyaki Beef', 'Pair with stir fry noodles or rice and veggies', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (26, 'Stir Fry Noodles and Veggies', 'Serve underneath a generous portion of Jalen''s Teriyaki Beef', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (27, 'Buttermilk Biscuits', 'These deserve to be smothered in sausage gravy!', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (28, 'Sausage Gravy', 'Goes great with buttermilk biscuits!', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (29, 'Stir Fry Rice and Veggies', 'Perfect vehicle for Jalen''s Teriyaki Beef', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (30, 'Toasted English Muffins', 'Simple and often enjoyed by Americans', '1) Tear or cut English muffin in half. /2) Toast English muffin halves for 1 minute. /3) Spread 1/2 Tbsp. butter on each half of English muffin. /4) Spread 1 Tbsp. of your favorite fruit preserves on each half of English muffin.', 2, 1);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (31, 'Smush''em Eggs (Over Easy) with Toast', 'Soak the yolk up with the toast', '', 2, 2);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (32, 'BST Omelette', 'BST is an acronym', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (33, 'Griddle or Pan Fried Bacon', 'The only ways to cook bacon', '', 0, 10);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (34, 'Cereal and Milk with Banana Slices', 'I once knew a man who couldn''t make a sandwich. This recipe is for you, Mr. Herbert.', '', 2, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (35, 'Kaitlin''s Dangerously Good Brownies', 'Nobody make''s ''em like my girl! Serve hot, with ice cream.', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (36, 'Cornbread', 'There''s no sugar because it isn''t corn cake.', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (37, 'Spicy Ethiopian Tomato Stew', 'Ultra-flavorful, rich Vegan stew!', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (38, 'Snickerdoodle Cookies', 'Everyone''s favorite', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (39, 'Grilled Steak and Veggie Skewers', 'You can''t beat ''em', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (40, 'Grilled Shrimp and Veggie Skewers', 'Put another shrimp on the barbie!', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (41, 'Creamy Pasta Salad', 'Perfect for a summer BBQ', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (42, 'Egg Salad', 'Travis used to make this all the time', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (43, 'Tuna Salad', 'Good alone, on toast, or in a melt', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (44, 'Peanut Butter Marshmallow Graham Crackers', 'Better than s''mores!', '1) Separate graham crackers into squares. /2) Spread 2 tsp. peanut butter on each cracker. /3) Put 9 mini marshmallows on each cracker. /4) Put crackers on top rack of toaster oven and broil on high for 2-3 minutes, until tops of marshmallows are brown.', 5, 2);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (45, 'Perfect Margarita', 'It doesn''t get any better than this.', '1) Pour 1800 Silver, Gran Marnier, and sweetened lime juice into shaker with ice. /2) Add splash of fresh lime juice. /3) Salt rim of glass. /4) Shake, pour, drink. /5) Repeat until you understand Spanish, or until you stop understanding Spanish.', 1, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (46, 'Dr. Maker''s Relaxer', 'Need to relax? Call the Dr.!', '1) Pour Maker''s Mark and Diet Dr. Pepper into shaker with ice. /2) Shake, pour, drink. /3) Repeat until relaxed.', 1, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (47, 'Ultimate Browned Butter Rice Krispie Treats', '', '', 0, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (48, 'Fried Bacon', 'It''s fried bacon', '1) Preheat pan to medium-low or griddle to 325F. /2) Cook bacon until desired crispiness is reached.', 0, 10);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (49, 'Jesse''s Lunch Fruit', 'The fruit Jesse eats every day at lunch', 'Wash and/or peel as appropriate.', 1, 0);
INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (50, 'Mimosa', 'The best way to start drinking all day', '1) Pour ingredients in glass. /2) Garnish with orange slice.', 1, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (51, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (52, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (53, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (54, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (55, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (56, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (57, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (58, '', '', '', 0, 0);
----INSERT INTO recipe (recipe_id, name, description, instructions, prep_time, cook_time) VALUES (59, '', '', '', 0, 0);

SET IDENTITY_INSERT recipe OFF;

SET IDENTITY_INSERT category ON;

INSERT INTO category(category_id, category_name) VALUES (1, '-');
INSERT INTO category(category_id, category_name) VALUES (2, 'breakfast');
INSERT INTO category(category_id, category_name) VALUES (3, 'lunch');
INSERT INTO category(category_id, category_name) VALUES (4, 'dinner');
INSERT INTO category(category_id, category_name) VALUES (5, 'dessert');
INSERT INTO category(category_id, category_name) VALUES (6, 'Italian');
INSERT INTO category(category_id, category_name) VALUES (7, 'East Asian');
INSERT INTO category(category_id, category_name) VALUES (8, 'American');
INSERT INTO category(category_id, category_name) VALUES (9, 'soul food');
INSERT INTO category(category_id, category_name) VALUES (10, 'quick');
INSERT INTO category(category_id, category_name) VALUES (11, 'entree');
INSERT INTO category(category_id, category_name) VALUES (12, 'meat');
INSERT INTO category(category_id, category_name) VALUES (13, 'Vegan');
INSERT INTO category(category_id, category_name) VALUES (14, 'vegetable');
INSERT INTO category(category_id, category_name) VALUES (15, 'side');
INSERT INTO category(category_id, category_name) VALUES (16, 'starch');
INSERT INTO category(category_id, category_name) VALUES (17, 'one-dish meal');
INSERT INTO category(category_id, category_name) VALUES (18, 'kids');
INSERT INTO category(category_id, category_name) VALUES (19, 'salad');
INSERT INTO category(category_id, category_name) VALUES (20, 'snack');
INSERT INTO category(category_id, category_name) VALUES (21, 'sauce');
INSERT INTO category(category_id, category_name) VALUES (22, 'Mediterranean');
INSERT INTO category(category_id, category_name) VALUES (23, 'BBQ');
INSERT INTO category(category_id, category_name) VALUES (24, 'cocktail');
INSERT INTO category(category_id, category_name) VALUES (25, 'Mexican');
INSERT INTO category(category_id, category_name) VALUES (26, 'Greek');
INSERT INTO category(category_id, category_name) VALUES (27, 'Chinese');
INSERT INTO category(category_id, category_name) VALUES (28, 'Indian');
INSERT INTO category(category_id, category_name) VALUES (29, 'Japanese');
INSERT INTO category(category_id, category_name) VALUES (30, 'fusion');
INSERT INTO category(category_id, category_name) VALUES (31, 'decadent');
INSERT INTO category(category_id, category_name) VALUES (32, 'French');
INSERT INTO category(category_id, category_name) VALUES (33, 'slow-cooker');
INSERT INTO category(category_id, category_name) VALUES (34, 'instant pot');
INSERT INTO category(category_id, category_name) VALUES (35, 'sandwich');

SET IDENTITY_INSERT category OFF;

INSERT INTO recipe_category (recipe_id, category_id) VALUES (1, 20);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (1, 13);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (1, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (2, 2)
INSERT INTO recipe_category (recipe_id, category_id) VALUES (2, 11);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (3, 19);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (3, 22);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (3, 6);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (4, 2);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (4, 9);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (5, 4);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (5, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (5, 30);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (6, 31);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (6, 15);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (6, 33);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (7, 23);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (7, 11);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (7, 12);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (8, 6);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (8, 4);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (8, 11);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (10, 3);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (10, 9);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (10, 35);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (11, 5);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (11, 31);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (13, 35);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (13, 3);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (14, 20);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (14, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (14, 5);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (18, 14);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (18, 15);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (18, 4);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (19, 14);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (19, 15);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (19, 4);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (20, 15);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (20, 19);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (20, 13);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (22, 21);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (22, 6);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (24, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (24, 2);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (24, 19);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (30, 2);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (30, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (44, 5);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (44, 20);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (45, 24);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (45, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (45, 17);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (46, 24);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (46, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (46, 17);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (48, 2);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (48, 3);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (48, 4);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (49, 3);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (49, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (50, 24);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (50, 10);
INSERT INTO recipe_category (recipe_id, category_id) VALUES (50, 17);

SET IDENTITY_INSERT meal ON;

INSERT INTO meal (meal_id, meal_name) VALUES (1, 'Simple Breakfast');
INSERT INTO meal (meal_id, meal_name) VALUES (2, 'Simple Lunch');
INSERT INTO meal (meal_id, meal_name) VALUES (3, 'Simple Dinner');
INSERT INTO meal (meal_id, meal_name) VALUES (4, 'Jesse''s Breakfast');
INSERT INTO meal (meal_id, meal_name) VALUES (5, 'Jesse''s Lunch');
INSERT INTO meal (meal_id, meal_name) VALUES (6, 'Jesse''s Dinner');
INSERT INTO meal (meal_id, meal_name) VALUES (7, 'Big Breakfast');
INSERT INTO meal (meal_id, meal_name) VALUES (8, 'Big Lunch');
INSERT INTO meal (meal_id, meal_name) VALUES (9, 'Big Dinner');

SET IDENTITY_INSERT meal OFF;

SET IDENTITY_INSERT users ON;

INSERT INTO users (id, username, password, salt, role) VALUES (1, 'admin@email.com', 'frOinLiQ0GuJuiCIRM0J84AiSKs=', '3o5HLrYGTUc=', 'User');

SET IDENTITY_INSERT users OFF;

INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 1, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 2, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (1, 3, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 4, 1, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 5, 10, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 6, 1, 0, 3);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 7, 11, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (2, 8, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (3, 69, 12, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (3, 70, 12, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (3, 41, 22, 15, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (4, 4, 1, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (4, 151, 5, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (4, 147, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (4, 21, 9, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (4, 149, 5, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (5, 77, 2, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (5, 76, 10, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (5, 78, 2, 8, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (5, 72, 5, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (5, 74, 9, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 153, 5, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 65, 9, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 155, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 136, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 154, 26, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 90, 5, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 68, 5, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 15, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 7, 5, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (6, 156, 5, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 116, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 20, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 65, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 111, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 137, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 14, 5, 0, 5);	
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 15, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 136, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 113, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 155, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 112, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 10, 5, 1, 0);	
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 157, 1, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (7, 82, 12, 7, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 95, 1, 12, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 103, 12, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 60, 20, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 102, 27, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 98, 25, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 101, 27, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 120, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 99, 5, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 41, 5, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 117, 5, 0, 1);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 14, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 15, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 96, 6, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (8, 97, 12, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (9, 163, 29, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (9, 8, 5, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (9, 120, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (9, 73, 11, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 1, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 8, 9, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 4, 1, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 26, 2, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 80, 2, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 68, 9, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (10, 48, 9, 1, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 4, 1, 10, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 9, 7, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 22, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 11, 5, 0, 8);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 158, 10, 6, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 1, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 69, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 48, 9, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 45, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 46, 2, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 47, 2, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 53, 22, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (13, 42, 2, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 1, 2, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 2, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 63, 20, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (11, 118, 9, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (18, 123, 12, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (18, 87, 10, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (18, 120, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (18, 14, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (18, 15, 9, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (19, 42, 23, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (19, 14, 9, 0, 1);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (19, 15, 9, 0, 1);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 53, 23, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 55, 12, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 41, 22, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 43, 4, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 59, 5, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 61, 19, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 160, 20, 1, 0)
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 161, 12, 0, 3)
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 162, 1, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 60, 21, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 87, 5, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 83, 10, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 89, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 119, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 15, 9, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (22, 14, 9, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 63, 20, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 128, 21, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 129, 21, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 125, 1, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 126, 12, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 127, 12, 0, 5);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 64, 20, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 164, 1, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 166, 10, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (24, 165, 10, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (44, 141, 1, 3, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (44, 2, 10, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (44, 142, 1, 54, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 36, 11, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 38, 11, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 37, 11, 2, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 35, 16, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 39, 11, 6, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (45, 14, 9, 0, 2);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (46, 139, 11, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (46, 140, 11, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (46, 39, 11, 6, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (48, 26, 2, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (49, 63, 20, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (49, 64, 19, 1, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (50, 168, 4, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (50, 169, 4, 4, 0);
INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id) VALUES (50, 170, 2, 1, 0);

INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (1, 24);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (1, 30);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (2, 1);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (2, 20);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (3, 5);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (3, 3);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (3, 22);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (4, 10);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (5, 13);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (5, 4);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (6, 8);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (6, 9);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (6, 3);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (6, 22);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (7, 2);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (7, 4);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (7, 48);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (7, 24);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (7, 50);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (8, 13);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (8, 20);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (8, 45);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (9, 6);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (9, 7);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (9, 18);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (9, 19);
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (9, 11);

SET IDENTITY_INSERT day ON;

INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (1, 1, 2, 3);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (2, 4, 5, 6);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (3, 7, 8, 9);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (4, 4, 2, 9);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (5, 1, 5, 6);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (6, 7, 2, 3);
INSERT INTO day (day_id, breakfast, lunch, dinner) VALUES (7, 1, 8, 6);

SET IDENTITY_INSERT day OFF;

SET IDENTITY_INSERT mealPlan ON;

INSERT INTO mealPlan (mealPlan_id, mealPlan_name, day1) VALUES (1, '1-Day Plan', 1);
INSERT INTO mealPlan (mealPlan_id, mealPlan_name, day1, day2) VALUES (2, '2-Day Plan', 2, 1);
INSERT INTO mealPlan (mealPlan_id, mealPlan_name, day1, day2, day3) VALUES (3, '3-Day Plan', 3, 1, 2);
INSERT INTO mealPlan (mealPlan_id, mealPlan_name, day1, day2, day3, day4, day5, day6, day7) VALUES (4, '7-Day Plan', 1, 2, 3, 4, 5, 6, 7);

SET IDENTITY_INSERT mealPlan OFF;

ALTER TABLE recipe_category ADD FOREIGN KEY (recipe_id) REFERENCES recipe(recipe_id)
ALTER TABLE recipe_category ADD FOREIGN KEY (category_id) REFERENCES category(category_id)

COMMIT TRANSACTION;
