
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE SetearMalaPaga
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE AspNetUsers
	SET MalaPaga = 'True'
	FROM Facturas
	INNER JOIN AspNetUsers
	ON AspNetUsers.Id = Facturas.UsuarioId
	WHERE Pagada = 'False' AND FechaLimiteDePago < GETDATE()

END
GO
