CREATE TABLE Employee
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [username] NVARCHAR(50) NOT NULL, 
    [password] NVARCHAR(50) NOT NULL, 
    [firstName] NVARCHAR(50) NOT NULL, 
    [lastName] NVARCHAR(50) NOT NULL, 
    [email] NVARCHAR(50) NOT NULL, 
    [officeNumber] INT NOT NULL, 
    [administrator] BIT NOT NULL
)
