CREATE DATABASE SportsTournamentDB;

GO
 
USE SportsTournamentDB;

GO
 
CREATE TABLE MatchResults

(

    MatchID INT IDENTITY(1,1) PRIMARY KEY,

    PlayerName VARCHAR(100) NOT NULL,

    Sport VARCHAR(50) NOT NULL,

    MatchCategory VARCHAR(30) NOT NULL,

    PointsAwarded INT NOT NULL

);

GO

Select *From MatchResults; 

 