CREATE PROCEDURE [dbo].[SP_SearchPeople]
    @Name nvarchar(MAX)=NULL
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
where
(
(@Name is null or Name COLLATE SQL_Latin1_General_CP1_CI_AS LIKE @Name + '%')
)
;
END
