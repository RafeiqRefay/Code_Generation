CREATE PROCEDURE [dbo].[SP_GetAllPeople]
AS
BEGIN

        SELECT
            [People].[ID],
            [People].[Name],
            [People].[PhoneNumber],
            [People].[Address],
            [People].[Gender],
            [People].[Age],
            [People].[Email]
        FROM [People]
;
END
