-- SQLite
-- SQLite
-- CREATE DATABASE

-- Create Dinosaur DB

-- DROP tables to start fresh
DROP TABLE If EXISTS breeding_line;
DROP TABLE IF EXISTS dinosaur;
DROP TABLE IF EXISTS stats;
DROP TABLE IF EXISTS mutated_stats;

-- Enables the use of FKs
PRAGMA foreign_keys = ON;

-- Create the breeding_line table
CREATE TABLE breeding_line (
    breeding_line_id INTEGER PRIMARY KEY AUTOINCREMENT,
    species INT NOT NULL,
    breeding_line_name VARCHAR(100)
);

-- Create the dinosaur table
CREATE TABLE dinosaur (
    dinosaur_id INTEGER PRIMARY KEY AUTOINCREMENT,
    breeding_line_id INTEGER NOT NULL,
    dinosaur_name VARCHAR(50) NOT NULL,
    species INTEGER NOT NULL,
    gender INTEGER NOT NULL,
    generation INTEGER NOT NULL,
    stats_id INTEGER NOT NULL,
    mutated_stats_id INTEGER NOT NULL,
    mutated BIT,
    aquatic_dino BIT,
    father_id INTEGER DEFAULT NULL,
    mother_id INTEGER DEFAULT NULL,

    CONSTRAINT fk_breeding_line_id_ref_breeding_line_breeding_line_id FOREIGN KEY (breeding_line_id) REFERENCES breeding_line(breeding_line_id),
    CONSTRAINT fk_father_id_ref_dinosaur_dinosaur_id FOREIGN KEY (father_id) REFERENCES dinosaur(dinosaur_id),
    CONSTRAINT fk_mother_id_ref_dinosaur_dinosaur_id FOREIGN KEY (mother_id) REFERENCES dinosaur(dinosaur_id)
);

-- create the stats table
CREATE TABLE stats (
    stats_id INTEGER PRIMARY KEY AUTOINCREMENT,
    health DECIMAL(9,1) NOT NULL,
    stamina DECIMAL(9,1) NOT NULL,
    oxygen DECIMAL(9,1),
    food DECIMAL(9,1) NOT NULL,
    water DECIMAL(9,1),
    weight DECIMAL(9,1) NOT NULL,
    melee DECIMAL(9,1) NOT NULL,
    movement_speed DECIMAL(9,1) NOT NULL
);

-- Create the mutated_stats table
CREATE TABLE mutated_stats (
    mutated_stats_id INTEGER PRIMARY KEY AUTOINCREMENT,
    health INTEGER NOT NULL CHECK(health BETWEEN 0 AND 10),
    stamina INTEGER NOT NULL CHECK(stamina BETWEEN 0 AND 10),
    oxygen INTEGER CHECK(oxygen BETWEEN 0 AND 10),
    food INTEGER NOT NULL CHECK(food BETWEEN 0 AND 10),
    water INTEGER CHECK(water BETWEEN 0 AND 10),
    weight INTEGER NOT NULL CHECK(weight BETWEEN 0 AND 10),
    melee INTEGER NOT NULL CHECK(melee BETWEEN 0 AND 10),
    movement_speed INTEGER NOT NULL CHECK(movement_speed BETWEEN 0 AND 10)
);

-- Establish test data

-- breeding_line test data
INSERT INTO breeding_line
VALUES
(10000001, 99, 'Shadowmane Tester'),
(10000002, 94, 'Rex Tester'),
(10000003, 91, 'Quetzal Tester');


-- dinosaur test data
INSERT INTO dinosaur
VALUES
(20000001, 10000001, 'Male Shadowmane Tester', 99, 1, 1, 30000001, 40000001,  true, true, null, null),
(20000002, 10000001, 'Female Shadowmane Tester', 99, 2, 1, 30000002, 40000002, true, true, null, null),
(20000003, 10000002, 'Male Rex Tester', 94, 1, 1, 30000003, 40000003, false, true, null, null),
(20000004, 10000002, 'Female Rex Tester', 94, 2, 1, 30000004, 40000004, false, true, null, null),
(20000005, 10000003, 'Male Quetzal Tester', 91, 1, 1, 30000005, 40000005, false, true, null, null),
(20000006, 10000003, 'Female Quetzal Tester', 91, 2, 1, 30000006, 40000006, false, true, null, null);


-- stats test data
INSERT INTO stats
VALUES
(30000001, 1000.00, 2000.00, null, 3000.00, null, 4000.00, 5000.00, 100),
(30000002, 1500.00, 2500.00, null, 3500.00, null, 4500.00, 5500.00, 150),
(30000003, 1000.00, 2000.00, 3000.00, 4000.00, 5000.00, 6000.00, 7000.00, 8000.00), 
(30000004, 1500.00, 2500.00, 3500.00, 4500.00, 5500.00, 6500.00, 7500.00, 8500.00), 
(30000005, 1000.00, 2000.00, 3000.00, 4000.00, 5000.00, 6000.00, 7000.00, 8000.00), 
(30000006, 1500.00, 2500.00, 3500.00, 4500.00, 5500.00, 6500.00, 7500.00, 8500.00);  

-- mutated_stats test data
INSERT INTO mutated_stats
VALUES
(40000001, 1, 1, null, 1, null, 1, 1, 1),
(40000002, 1, 2, null, 1, null, 1, 1, 1), 
(40000003, 1, 1, 2, 1, 1, 1, 1, 1),
(40000004, 1, 1, 1, 2, 1, 1, 1, 1),
(40000005, 1, 1, 1, 1, 2, 1, 1, 1),
(40000006, 1, 1, 1, 1, 1, 2, 1, 1);