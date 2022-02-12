
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE CreacionFacturas 
	@FechaInicio DATETIME,
	@FechaFin DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MontoPorPeticion DECIMAL(4,4) = 1.0 / 2

	INSERT INTO Facturas (UsuarioId, Monto, FechaEmision, Pagada, FechaLimiteDePago)
	SELECT LlavesApi.UsuarioId,
	COUNT(*) * @MontoPorPeticion AS Monto,
	GETDATE() AS FechaEmision,
	0 AS Pagada,
	DATEADD(d, 60, GETDATE()) AS FechaLimiteDePago
	FROM Peticiones
	INNER JOIN LlavesApi
	ON LlavesApi.Id = Peticiones.LlaveId
	WHERE LlavesApi.TipoLlave != 1
	AND Peticiones.FechaPeticion >= @FechaInicio
	AND Peticiones.FechaPeticion < @FechaFin
	GROUP BY LlavesApi.UsuarioId

	INSERT INTO FacturasEmitidas (Mes, Anio)
	SELECT
		CASE MONTH(GETDATE())
			WHEN 1 THEN 12
			ELSE MONTH(GETDATE()) - 1
			END AS Mes,
		CASE MONTH(GETDATE())
			WHEN 1 THEN YEAR(GETDATE()) - 1
			ELSE YEAR(GETDATE())
			END AS Anio

END
GO
