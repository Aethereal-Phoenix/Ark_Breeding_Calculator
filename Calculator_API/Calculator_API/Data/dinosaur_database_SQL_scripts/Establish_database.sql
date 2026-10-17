-- CREATE DATABASE

-- Create dinosaur db
DROP TABLE If EXISTS breeding_line;
DROP TABLE IF EXISTS dinosaur;
DROP TABLE IF EXISTS stats;
DROP TABLE IF EXISTS mutated_stats;

PRAGMA foreign_keys = ON;

CREATE TABLE breeding_line(
    breeding_line_id VARCHAR(36) PRIMARY KEY,
    species INT NOT NULL,
    breeding_line_name VARCHAR(50)
);

CREATE TABLE dinosaur(
    dinosaur_id VARCHAR(36) PRIMARY KEY,
    breeding_line_id VARCHAR(36) NOT NULL,
    dinosaur_name VARCHAR(50) NOT NULL,
    species INT NOT NULL,
    gender INT NOT NULL,
    generation INT NOT NULL,
    mutated BIT,
    aquatic_dino BIT,
    father_id VARCHAR(36) DEFAULT NULL,
    mother_id VARCHAR(36) DEFAULT NULL,

    CONSTRAINT fk_breeding_line_id_ref_breeding_line_breeding_line_id FOREIGN KEY (breeding_line_id) REFERENCES breeding_line(breeding_line_id),
    CONSTRAINT fk_father_id_ref_dinosaur_dinosaur_id FOREIGN KEY (father_id) REFERENCES dinosaur(dinosaur_id),
    CONSTRAINT fk_mother_id_ref_dinosaur_dinosaur_id FOREIGN KEY (mother_id) REFERENCES dinosaur(dinosaur_id)
);

CREATE TABLE stats (
    dinosaur_id VARCHAR(36) PRIMARY KEY,
    health DECIMAL(9,1) NOT NULL,
    stamina DECIMAL(9,1) NOT NULL,
    oxygen DECIMAL(9,1),
    food DECIMAL(9,1) NOT NULL,
    water DECIMAL(9,1),
    weight DECIMAL(9,1) NOT NULL,
    melee DECIMAL(9,1) NOT NULL,
    movement_speed DECIMAL(9,1) NOT NULL,

    CONSTRAINT fk_stats_dinosaur_id_ref_dinosaur_dinosaur_id FOREIGN KEY (dinosaur_id) REFERENCES dinosaur(dinosaur_id)
);

CREATE TABLE mutated_stats (
    dinosaur_id VARCHAR(36) PRIMARY KEY,
    health INT NOT NULL CHECK(health BETWEEN 0 AND 10),
    stamina INT NOT NULL CHECK(stamina BETWEEN 0 AND 10),
    oxygen INT CHECK(oxygen BETWEEN 0 AND 10),
    food INT NOT NULL CHECK(food BETWEEN 0 AND 10),
    water INT CHECK(water BETWEEN 0 AND 10),
    weight INT NOT NULL CHECK(weight BETWEEN 0 AND 10),
    melee INT NOT NULL CHECK(melee BETWEEN 0 AND 10),
    movement_speed INT NOT NULL CHECK(movement_speed BETWEEN 0 AND 10),

    CONSTRAINT fk_mutated_stats_dinosaur_id_ref_dinosaur_dinosaur_id FOREIGN KEY (dinosaur_id) REFERENCES dinosaur(dinosaur_id)
);

INSERT INTO breeding_line
VALUES
('ab7640c0-ce7d-4f91-a8f2-a9e64ea85d80', 99, 'Shadowmane Tester'),
('3f27c753-9f3f-4314-9e67-0381cb12472e', 94, 'Rex Tester'),
('9f24247b-0744-40d4-91ac-73cf5068bb35', 91, 'Quetzal Tester');

INSERT INTO dinosaur
VALUES
('7f3b89f0-cade-4ef3-96ca-5e7f69a88064', 'ab7640c0-ce7d-4f91-a8f2-a9e64ea85d80', 'Male Shadowmane Tester', 99, 1, 1, true, true, null, null),
('a1436de7-32ce-4a4f-862e-df812e89988b', 'ab7640c0-ce7d-4f91-a8f2-a9e64ea85d80', 'Female Shadowmane Tester', 99, 2, 1, true, true, null, null),
('5beefd6f-e1b5-4792-ac01-c7a61c6f6b13', '3f27c753-9f3f-4314-9e67-0381cb12472e', 'Male Rex Tester', 94, 1, 1, false, true, null, null),
('3f0c90db-96ab-4be4-bf20-e46284817df9', '3f27c753-9f3f-4314-9e67-0381cb12472e', 'Female Rex Tester', 94, 2, 1, false, true, null, null),
('97afe4bb-a80e-43c5-ad8e-33bc1ca3886f', '9f24247b-0744-40d4-91ac-73cf5068bb35', 'Male Quetzal Tester', 91, 1, 1, false, true, null, null),
('2b9522ef-792d-496d-8730-0228e7b713d8', '9f24247b-0744-40d4-91ac-73cf5068bb35', 'Female Quetzal Tester', 91, 2, 1, false, true, null, null);

INSERT INTO stats
VALUES
('7f3b89f0-cade-4ef3-96ca-5e7f69a88064', 1000.00, 2000.00, null, 3000.00, null, 4000.00, 5000.00, 100),
('a1436de7-32ce-4a4f-862e-df812e89988b', 1500.00, 2500.00, null, 3500.00, null, 4500.00, 5500.00, 150),
('5beefd6f-e1b5-4792-ac01-c7a61c6f6b13', 1000.00, 2000.00, 3000.00, 4000.00, 5000.00, 6000.00, 7000.00, 8000.00), 
('3f0c90db-96ab-4be4-bf20-e46284817df9', 1500.00, 2500.00, 3500.00, 4500.00, 5500.00, 6500.00, 7500.00, 8500.00), 
('97afe4bb-a80e-43c5-ad8e-33bc1ca3886f', 1000.00, 2000.00, 3000.00, 4000.00, 5000.00, 6000.00, 7000.00, 8000.00), 
('2b9522ef-792d-496d-8730-0228e7b713d8', 1500.00, 2500.00, 3500.00, 4500.00, 5500.00, 6500.00, 7500.00, 8500.00);  

INSERT INTO mutated_stats
VALUES
('7f3b89f0-cade-4ef3-96ca-5e7f69a88064', 1, 1, null, 1, null, 1, 1, 1),
('a1436de7-32ce-4a4f-862e-df812e89988b', 1, 2, null, 1, null, 1, 1, 1), 
('5beefd6f-e1b5-4792-ac01-c7a61c6f6b13', 1, 1, 2, 1, 1, 1, 1, 1),
('3f0c90db-96ab-4be4-bf20-e46284817df9', 1, 1, 1, 2, 1, 1, 1, 1),
('97afe4bb-a80e-43c5-ad8e-33bc1ca3886f', 1, 1, 1, 1, 2, 1, 1, 1),
('2b9522ef-792d-496d-8730-0228e7b713d8', 1, 1, 1, 1, 1, 2, 1, 1);