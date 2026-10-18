Create PROCEDURE [dbo].[SP_UpdatePeopleByID]
    @ID int,
    @Name nvarchar(MAX),
    @PhoneNumber nvarchar(MAX),
    @Address nvarchar(MAX),
    @Gender nvarchar(MAX),
    @Age int,
    @Message NVARCHAR(250) OUTPUT,
    @ErrorType INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [People] WHERE [ID] = @ID)
        BEGIN
            SET @Message = 'People not found.';
            SET @ErrorType = 2;
            RETURN;
        END

        UPDATE [People]
        SET
            [Name] = COALESCE(@Name,Name),
            [PhoneNumber] = COALESCE(@PhoneNumber,PhoneNumber),
            [Address] = COALESCE(@Address,Address),
            [Gender] = COALESCE(@Gender,Gender),
            [Age] = COALESCE(@Age,Age)
        WHERE [ID] = @ID;

        SET @Message = 'People updated successfully.';
        SET @ErrorType = 0;

        SELECT [People].[ID], [People].[Name], [People].[PhoneNumber], [People].[Address], [People].[Gender], [People].[Age], [People].[Email]
        FROM [People]
        WHERE [ID] = @ID;

    END TRY
    BEGIN CATCH
        SET @Message = ERROR_MESSAGE() + ' | Line: ' + CAST(ERROR_LINE() AS NVARCHAR);
        SET @ErrorType = 5;
    END CATCH
END
