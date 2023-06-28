Create table UserPassportDetails
(
UserID Int Not null Primary Key,
Type varchar(10) NOT NULL,
FirstName nvarchar NOT NULL,
MiddleName nvarchar,
LastName nvarchar NOT NULL,
PassportNumber nvarchar NOT NULL UNIQUE,
Nationality varchar(10) NOT NULL,
DateOfBirth DateTime NOT NULL,
Gender varchar(10) NOT NULL,
DateOfExpiry DateTime NOT NULL,
CreationDate Date NOT NULL,
CreationTime Time NOT NULL,
ModifiedDate Date NOT NULL,
ModifiedTime Time NOT NULL
)

Create Table AccessCodeDeatils
(
ID Int primary key NOT NULL,
UserID int NOT NULL,
AccessCode nvarchar NOT NULL,
DateOfIssue DateTime NOT NULL,
IPAddress nvarchar,
FirstUse varchar(1) NOT NULL,
CreationDate Date NOT NULL,
CreationTime Time NOT NULL,
ModifiedDate Date NOT NULL,
ModifiedTime Time NOT NULL
FOREIGN KEY (UserID) REFERENCES UserPassportDetails(UserID)
)