CREATE OR ALTER PROCEDURE GetPlayerDetailWithMultipleLevel
	@PlayerId INT
AS
BEGIN
	SELECT p.Id, p.Name, t.Id as TeamId, t.CountryName as Name, c.Id as ConfederationId, c.Name from Players p 
	left join Teams t on p.TeamId = t.Id
	left JOIN Confederations c on t.ConfederationId = c.Id 
	WHERE p.Id = @PlayerId
END

EXEC GetPlayerDetailWithMultipleLevel @PlayerId = 2;
