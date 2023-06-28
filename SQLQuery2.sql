create procedure sp_AddUserPassportDetails
@Type varchar(10),
@FirstName nvarchar ,
@MiddleName nvarchar,
@LastName nvarchar ,
@PassportNumber nvarchar  ,
@Nationality varchar(10) ,
@DateOfBirth DateTime ,
@Gender varchar(10) ,
@DateOfExpiry DateTime ,
@CreationDate Date ,
@ModifiedDate Date
As
Go
IF Exists (select 1 from dbo.UserpassportDetails where PassportNumber=@PassportNumber)
BEGIN
END
ELSE
BEGIN
Insert into UserPassportDetails values(@Type,@FirstName,@MiddleName,@LastName,@PassportNumber,@Nationality,@DatefBirth,@Gender,@DateofExpiry,
)
