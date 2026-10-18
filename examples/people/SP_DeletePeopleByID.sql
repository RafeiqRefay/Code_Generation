CREATE PROCEDURE [dbo].[SP_DeletePeopleByID]
    @ID int,
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

        DELETE FROM [People] WHERE [ID] = @ID;

        SET @Message = 'People deleted successfully.';
        SET @ErrorType = 0;

    END TRY
    BEGIN CATCH
        -- FK Constraint
        IF ERROR_NUMBER() = 547
        BEGIN
            SET @Message = 'Cannot delete People. It is linked to other records.';
            SET @ErrorType = 4;
        END
        ELSE
        BEGIN
            SET @Message = ERROR_MESSAGE();
            SET @ErrorType = 5;
        END
    END CATCH
END
