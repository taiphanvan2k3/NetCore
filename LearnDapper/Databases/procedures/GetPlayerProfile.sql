CREATE OR ALTER PROCEDURE GetPlayerProfile
	@PlayerId INT
AS
BEGIN
	SELECT p.Id, p.Name, t.CountryName, c.Name as ConfederationName from Players p 
	left join Teams t on p.TeamId = t.Id
	left JOIN Confederations c on t.ConfederationId = c.Id 
	WHERE p.Id = @PlayerId
END

EXEC GetPlayerProfile @PlayerId = 2;
