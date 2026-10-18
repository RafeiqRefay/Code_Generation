CREATE PROCEDURE [dbo].[SP_GetPeopleByID]
    @ID int,
    @Message VARCHAR(150) OUTPUT,
    @ErrorType INT OUTPUT

AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS (
            SELECT 1
            FROM [People]
            WHERE [People].[ID] = @ID
        )
        BEGIN
            SET @Message = 'People not found.';
            SET @ErrorType = 2;
            RETURN;
        END

        SELECT
            [People].[ID],
            [People].[Name],
            [People].[PhoneNumber],
            [People].[Address],
            [People].[Gender],
            [People].[Age],
            [People].[Email]
        FROM [People]
        WHERE [People].[ID] = @ID

        SET @Message = 'People loaded successfully.';
        SET @ErrorType = 0;

    END TRY
    BEGIN CATCH
        SET @Message =
            ERROR_MESSAGE() + ' | Line: ' + CAST(ERROR_LINE() AS VARCHAR);
        SET @ErrorType = 5;
    END CATCH
END
