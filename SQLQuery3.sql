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
Begin
SET NOCOUNT ON;  
IF Not Exists (select 1 from dbo.UserpassportDetails where PassportNumber=@PassportNumber)
BEGIN
Insert into dbo.UserPassportDetails values(@Type,@FirstName,@MiddleName,@LastName,@PassportNumber,@Nationality,@DateOfBirth,@Gender,@DateofExpiry,
GETDATE(),GETDATE())
END
End

