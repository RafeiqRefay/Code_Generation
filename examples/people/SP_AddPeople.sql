Create PROCEDURE [dbo].[SP_AddPeople]
    @Name nvarchar(MAX),
    @PhoneNumber nvarchar(MAX),
    @Address nvarchar(MAX),
    @Gender nvarchar(MAX),
    @Age int,
    @Email nvarchar(MAX)
    ,@NewID INT OUTPUT
    ,@Message NVARCHAR(250) OUTPUT
    ,@ErrorType INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        INSERT INTO People (Name, PhoneNumber, Address, Gender, Age, Email)
        VALUES (@Name, @PhoneNumber, @Address, @Gender, @Age, @Email);

        SET @NewID = SCOPE_IDENTITY();
        SET @Message = 'People added successfully.';
        SET @ErrorType = 0;  -- Success

    END TRY
    BEGIN CATCH
        SET @NewID = -1;
        SET @Message = ERROR_MESSAGE();
        SET @ErrorType = 5;  -- DatabaseError
    END CATCH
END
