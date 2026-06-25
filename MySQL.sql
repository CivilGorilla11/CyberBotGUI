-- Create the database
CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

-- Table for storing tasks (matches CyberTask.cs)
CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderTime DATETIME NULL,
    CreatedAt DATETIME NOT NULL,
    IsComplete BOOLEAN DEFAULT FALSE
);

-- Table for storing activity logs (matches LogEntry.cs + DatabaseService.cs)
CREATE TABLE IF NOT EXISTS ActivityLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(255) NOT NULL,
    Details TEXT,              -- ✅ matches DatabaseService.GetLog()
    Category VARCHAR(50),
    Timestamp DATETIME NOT NULL
);

CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

CREATE TABLE IF NOT EXISTS ActivityLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(255) NOT NULL,
    Details TEXT,              -- ✅ matches DatabaseService.GetLog()
    Category VARCHAR(50),
    Timestamp DATETIME NOT NULL
);
DROP TABLE IF EXISTS ActivityLog;

CREATE TABLE IF NOT EXISTS ActivityLog(
	    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(255) NOT NULL,
    Details TEXT,
    Category VARCHAR(50),
    Timestamp DATETIME NOT NULL
);

DROP TABLE IF EXISTS ActivityLog;

DROP TABLE IF EXISTS Tasks;

-- Create the database
CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

-- Table for storing tasks (matches CyberTask.cs)
CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderTime DATETIME NULL,
    CreatedAt DATETIME NOT NULL,
    IsComplete BOOLEAN DEFAULT FALSE
);

-- Table for storing activity logs (matches LogEntry.cs)
CREATE TABLE IF NOT EXISTS ActivityLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(255) NOT NULL,
    Details TEXT,
    Category VARCHAR(50),
    Timestamp DATETIME NOT NULL,
    INDEX idx_timestamp (Timestamp)
);







