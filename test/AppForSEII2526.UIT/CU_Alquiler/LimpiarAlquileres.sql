BEGIN TRANSACTION;

-- Desactivar FKs temporalmente
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

-- Limpiar datos dinámicos (orden correcto)
DELETE FROM dbo.OfertaItem;
DELETE FROM dbo.Oferta;

DELETE FROM dbo.ReparacionItem;
DELETE FROM dbo.Reparacion;

DELETE FROM dbo.AlquilarItem;
DELETE FROM dbo.Alquiler;

DELETE FROM dbo.CompraItem;
DELETE FROM dbo.Compra;

-- Reactivar FKs
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

COMMIT TRANSACTION;

PRINT 'Base de datos restaurada a estado inicial para pruebas';
