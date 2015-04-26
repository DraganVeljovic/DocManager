CREATE TABLE Document
(
	[id] INT NOT NULL PRIMARY KEY, 
    [title] NVARCHAR(50) NOT NULL, 
    [keywords] NVARCHAR(50) NOT NULL, 
    [type] NCHAR(10) NOT NULL, 
    [owner_id] INT NOT NULL, 
    [created] DATETIME NOT NULL, 
    [version] INT NOT NULL, 
    [isReading] INT NOT NULL, 
    [isWriting] BIT NOT NULL, 
    [isActive] BIT NOT NULL, 
    [filePath] NVARCHAR(100) NOT NULL 
)
