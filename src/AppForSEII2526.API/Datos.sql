-- DESACTIVAR RESTRICCIONES DE CLAVES FORÁNEAS
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- BORRAR DATOS EN ORDEN CORRECTO (primero tablas hijas, luego padres)
DELETE FROM dbo.OfertaItem;
DELETE FROM dbo.Oferta;
DELETE FROM dbo.ReparacionItem;
DELETE FROM dbo.Reparacion;
DELETE FROM dbo.AlquilarItem;
DELETE FROM dbo.Alquiler;
DELETE FROM dbo.CompraItem;
DELETE FROM dbo.Compra;
DELETE FROM dbo.Herramienta;
DELETE FROM dbo.Fabricante;

-- REACTIVAR RESTRICCIONES
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"

-- FABRICANTE
SET IDENTITY_INSERT dbo.Fabricante ON;

INSERT INTO dbo.Fabricante (Id, Nombre) VALUES
(1, 'Makita'),
(2, 'Bosch'),
(3, 'DeWalt'),
(4, 'Stanley'),
(5, 'Hitachi'),
(6, 'Hilti'),
(7, 'Einhell'),
(8, 'Black & Decker'),
(9, 'Milwaukee'),
(10, 'Metabo'),
(11, 'Ryobi'),
(12, 'Craftsman'),
(13, 'Kobalt'),
(14, 'Husky'),
(15, 'Parkside'),
(16, 'Irwin'),
(17, 'Festool'),
(18, 'Rubi'),
(19, 'Bellota'),
(20, 'Total Tools');

SET IDENTITY_INSERT dbo.Fabricante OFF;

-- HERRAMIENTA
SET IDENTITY_INSERT dbo.Herramienta ON;

INSERT INTO dbo.Herramienta (Id, Nombre, Material, Precio, TiempoReparacion, fabricanteId) VALUES
(1, 'Taladro Percutor', 'Acero', 120.50, 5, 1),
(2, 'Sierra Circular', 'Aluminio', 95.00, 4, 2),
(3, 'Martillo Neumático', 'Acero', 150.00, 7, 3),
(4, 'Lijadora Orbital', 'Plástico', 75.90, 3, 4),
(5, 'Cortadora de Azulejos', 'Cerámica', 200.00, 6, 5),
(6, 'Pistola de Calor', 'Metal', 65.00, 3, 6),
(7, 'Multiherramienta', 'Acero', 89.00, 4, 7),
(8, 'Sierra de Calar', 'Aluminio', 110.50, 5, 8),
(9, 'Amoladora Angular', 'Acero', 130.00, 5, 9),
(10, 'Compresor Portátil', 'Hierro', 175.50, 6, 10),
(11, 'Atornillador Eléctrico', 'Plástico', 60.00, 2, 11),
(12, 'Taladro Inalámbrico', 'Aluminio', 115.75, 4, 12),
(13, 'Clavadora Neumática', 'Acero', 140.00, 5, 13),
(14, 'Sierra de Mesa', 'Acero', 250.00, 8, 14),
(15, 'Lijadora de Banda', 'Plástico', 85.00, 4, 15),
(16, 'Cepillo Eléctrico', 'Aluminio', 90.50, 3, 16),
(17, 'Fresadora', 'Hierro', 160.00, 6, 17),
(18, 'Nivel Láser', 'Plástico', 70.00, 2, 18),
(19, 'Cortadora de Metal', 'Acero', 210.00, 7, 19),
(20, 'Taladro de Columna', 'Hierro', 300.00, 10, 20);

SET IDENTITY_INSERT dbo.Herramienta OFF;

-- COMPRA
SET IDENTITY_INSERT dbo.Compra ON;

INSERT INTO dbo.Compra (Id, ClienteId, DireccionEnvio, FechaCompra, PrecioTotal, MetodoPago) VALUES
(1, 'U001', 'Calle Mayor 10', '2025-01-10', 350.50, 0),
(2, 'U002', 'Av. Andalucía 45', '2025-02-15', 240.00, 1),
(3, 'U003', 'Calle Sol 23', '2025-03-20', 150.00, 2),
(4, 'U004', 'Calle Luna 14', '2025-04-12', 500.00, 0),
(5, 'U005', 'Paseo del Prado 5', '2025-04-18', 230.75, 1),
(6, 'U006', 'Calle Real 99', '2025-05-10', 410.00, 2),
(7, 'U007', 'Calle Jardín 6', '2025-05-15', 170.20, 1),
(8, 'U008', 'Calle Olivo 13', '2025-06-02', 255.00, 0),
(9, 'U009', 'Av. Castilla 11', '2025-06-07', 280.60, 2),
(10, 'U010', 'Calle Mar 3', '2025-06-10', 199.90, 1),
(11, 'U011', 'Av. Norte 8', '2025-06-18', 275.00, 0),
(12, 'U012', 'Calle Este 2', '2025-07-01', 190.00, 2),
(13, 'U013', 'Calle Oeste 7', '2025-07-10', 155.90, 1),
(14, 'U014', 'Calle Sur 12', '2025-07-12', 399.50, 0),
(15, 'U015', 'Av. del Río 22', '2025-07-20', 300.10, 1),
(16, 'U016', 'Calle Nieve 4', '2025-08-01', 450.00, 2),
(17, 'U017', 'Av. Paz 9', '2025-08-05', 180.70, 0),
(18, 'U018', 'Calle Luz 16', '2025-08-15', 210.00, 2),
(19, 'U019', 'Calle Roble 1', '2025-09-01', 370.80, 1),
(20, 'U020', 'Av. Olmo 25', '2025-09-10', 290.40, 0);

SET IDENTITY_INSERT dbo.Compra OFF;

-- COMPRA ITEM
INSERT INTO dbo.CompraItem (herramientaId, compraId, cantidad, descripcion, precio) VALUES
(1, 1, 2, 'Taladro percutor Makita', 240.00),
(2, 1, 1, 'Sierra Circular Bosch', 110.50),
(3, 2, 1, 'Martillo Neumático DeWalt', 150.00),
(4, 3, 3, 'Lijadora Orbital Stanley', 225.00),
(5, 4, 2, 'Cortadora de Azulejos Hitachi', 400.00),
(6, 5, 1, 'Pistola de Calor Hilti', 65.00),
(7, 6, 1, 'Multiherramienta Einhell', 89.00),
(8, 7, 2, 'Sierra de Calar Black & Decker', 220.00),
(9, 8, 1, 'Amoladora Angular Milwaukee', 130.00),
(10, 9, 1, 'Compresor Portátil Metabo', 175.50),
(11, 10, 2, 'Atornillador Eléctrico Ryobi', 120.00),
(12, 11, 1, 'Taladro Inalámbrico Craftsman', 115.75),
(13, 12, 1, 'Clavadora Neumática Kobalt', 140.00),
(14, 13, 2, 'Sierra de Mesa Husky', 500.00),
(15, 14, 1, 'Lijadora de Banda Parkside', 85.00),
(16, 15, 1, 'Cepillo Eléctrico Irwin', 90.50),
(17, 16, 1, 'Fresadora Festool', 160.00),
(18, 17, 2, 'Nivel Láser Rubi', 140.00),
(19, 18, 1, 'Cortadora de Metal Bellota', 210.00),
(20, 19, 1, 'Taladro de Columna Total Tools', 300.00);

-- ALQUILER
SET IDENTITY_INSERT dbo.Alquiler ON;

INSERT INTO dbo.Alquiler (Id, DireccionEnvio, FechaAlquiler, FechaFin, PrecioTotal, MetodoPago, NombreCliente, ApellidoCliente, ClienteId) VALUES
(1, 'Calle Prado 12', '2025-01-05', '2025-01-10', 150.00, 0, 'Juan', 'Pérez', 'U001'),
(2, 'Av. Sol 34', '2025-01-08', '2025-01-15', 210.00, 1, 'María', 'González', 'U002'),
(3, 'Calle Río 22', '2025-02-01', '2025-02-07', 180.00, 2, 'Carlos', 'Díaz', 'U003'),
(4, 'Calle Norte 45', '2025-02-05', '2025-02-10', 200.00, 0, 'Laura', 'Martínez', 'U004'),
(5, 'Calle Este 67', '2025-02-10', '2025-02-20', 300.00, 1, 'Andrés', 'Rodríguez', 'U005'),
(6, 'Calle Oeste 33', '2025-03-01', '2025-03-06', 170.00, 2, 'Ana', 'Torres', 'U006'),
(7, 'Calle Jardín 12', '2025-03-10', '2025-03-15', 90.00, 1, 'Pedro', 'Álvarez', 'U007'),
(8, 'Calle Luna 44', '2025-03-15', '2025-03-25', 250.00, 0, 'Sofía', 'Cruz', 'U008'),
(9, 'Av. Mar 11', '2025-04-01', '2025-04-06', 220.00, 2, 'José', 'Mejía', 'U009'),
(10, 'Av. Olmo 77', '2025-04-10', '2025-04-15', 190.00, 1, 'Marta', 'López', 'U010'),
(11, 'Calle Roble 22', '2025-04-12', '2025-04-18', 130.00, 0, 'Lucas', 'Ramírez', 'U011'),
(12, 'Av. Paz 15', '2025-05-01', '2025-05-10', 275.00, 2, 'Rocío', 'García', 'U012'),
(13, 'Calle Sur 5', '2025-05-15', '2025-05-22', 230.00, 1, 'Daniel', 'Hernández', 'U013'),
(14, 'Calle Nieve 31', '2025-06-01', '2025-06-10', 160.00, 0, 'Susana', 'Ortega', 'U014'),
(15, 'Calle Sol 22', '2025-06-05', '2025-06-15', 210.00, 2, 'Jorge', 'Molina', 'U015'),
(16, 'Av. Real 18', '2025-06-20', '2025-06-30', 195.00, 1, 'Carmen', 'Trujillo', 'U016'),
(17, 'Calle Mayor 9', '2025-07-01', '2025-07-08', 180.00, 0, 'Manuel', 'Romero', 'U017'),
(18, 'Calle Azul 55', '2025-07-10', '2025-07-20', 245.00, 1, 'Inés', 'Ruiz', 'U018'),
(19, 'Av. Verde 4', '2025-07-15', '2025-07-25', 270.00, 2, 'Antonio', 'Fernández', 'U019'),
(20, 'Calle Amarilla 10', '2025-08-01', '2025-08-10', 260.00, 0, 'Sara', 'López', 'U020');

SET IDENTITY_INSERT dbo.Alquiler OFF;

-- ALQUILAR ITEM
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, cantidad, precio) VALUES
(1, 1, 1, 80.00),
(2, 2, 2, 150.00),
(3, 3, 1, 90.00),
(4, 4, 1, 100.00),
(5, 5, 2, 160.00),
(6, 6, 1, 70.00),
(7, 7, 1, 45.00),
(8, 8, 1, 130.00),
(9, 9, 1, 110.00),
(10, 10, 1, 90.00),
(11, 11, 2, 100.00),
(12, 12, 1, 125.00),
(13, 13, 2, 140.00),
(14, 14, 1, 80.00),
(15, 15, 1, 105.00),
(16, 16, 1, 95.00),
(17, 17, 2, 120.00),
(18, 18, 1, 110.00),
(19, 19, 1, 130.00),
(20, 20, 1, 115.00);

-- REPARACION
SET IDENTITY_INSERT dbo.Reparacion ON;

INSERT INTO dbo.Reparacion (Id, FechaEntrega, FechaRecogida, PrecioTotal, MetodoPago, ClienteId) VALUES
(1, '2025-01-02', '2025-01-07', 90.00, 0, 'U001'),
(2, '2025-01-10', '2025-01-15', 150.00, 1, 'U002'),
(3, '2025-02-01', '2025-02-05', 200.00, 2, 'U003'),
(4, '2025-02-10', '2025-02-20', 250.00, 0, 'U004'),
(5, '2025-02-25', '2025-03-01', 180.00, 1, 'U005'),
(6, '2025-03-05', '2025-03-10', 130.00, 2, 'U006'),
(7, '2025-03-15', '2025-03-22', 220.00, 0, 'U007'),
(8, '2025-04-01', '2025-04-07', 195.00, 2, 'U008'),
(9, '2025-04-10', '2025-04-15', 205.00, 1, 'U009'),
(10, '2025-04-20', '2025-04-25', 115.00, 0, 'U010'),
(11, '2025-05-01', '2025-05-06', 140.00, 1, 'U011'),
(12, '2025-05-10', '2025-05-15', 160.00, 2, 'U012'),
(13, '2025-05-20', '2025-05-25', 180.00, 0, 'U013'),
(14, '2025-06-01', '2025-06-06', 250.00, 1, 'U014'),
(15, '2025-06-10', '2025-06-15', 200.00, 2, 'U015'),
(16, '2025-06-20', '2025-06-25', 175.00, 0, 'U016'),
(17, '2025-07-01', '2025-07-06', 145.00, 1, 'U017'),
(18, '2025-07-10', '2025-07-15', 190.00, 2, 'U018'),
(19, '2025-07-20', '2025-07-25', 210.00, 0, 'U019'),
(20, '2025-07-30', '2025-08-04', 230.00, 1, 'U020');

SET IDENTITY_INSERT dbo.Reparacion OFF;

-- REPARACION ITEM
INSERT INTO dbo.ReparacionItem (herramientaId, reparacionId, Cantidad, Descripcion, Precio) VALUES
(1, 1, 1, 'Cambio de broca', 50.00),
(2, 2, 1, 'Sustitución de hoja de sierra', 75.00),
(3, 3, 1, 'Revisión de presión de aire', 120.00),
(4, 4, 2, 'Cambio de lijas y ajuste', 130.00),
(5, 5, 1, 'Reparación de motor', 150.00),
(6, 6, 1, 'Cambio de resistencia térmica', 80.00),
(7, 7, 1, 'Revisión de batería', 60.00),
(8, 8, 2, 'Cambio de cuchillas', 140.00),
(9, 9, 1, 'Ajuste de disco de corte', 90.00),
(10, 10, 1, 'Revisión de presión', 70.00),
(11, 11, 2, 'Cambio de punta', 95.00),
(12, 12, 1, 'Reparación de batería', 110.00),
(13, 13, 1, 'Cambio de válvula neumática', 130.00),
(14, 14, 1, 'Sustitución de mesa', 200.00),
(15, 15, 2, 'Cambio de correa', 85.00),
(16, 16, 1, 'Ajuste de cuchillas', 90.00),
(17, 17, 1, 'Mantenimiento general', 100.00),
(18, 18, 1, 'Reemplazo de sensor láser', 120.00),
(19, 19, 1, 'Cambio de disco de corte', 140.00),
(20, 20, 1, 'Revisión completa', 160.00);

-- OFERTA
SET IDENTITY_INSERT dbo.Oferta ON;

INSERT INTO dbo.Oferta (Id, porcentaje, fechaInicio, fechaFinal, fechaOferta, metodoPago, dirigidaA) VALUES
(1, 10.0, '2025-01-01', '2025-01-31', '2025-01-15', 0, 0),
(2, 15.0, '2025-02-01', '2025-02-28', '2025-02-15', 1, 1),
(3, 20.0, '2025-03-01', '2025-03-31', '2025-03-20', 2, 0),
(4, 10.0, '2025-04-01', '2025-04-30', '2025-04-10', 0, 1),
(5, 25.0, '2025-05-01', '2025-05-31', '2025-05-15', 1, 0),
(6, 5.0, '2025-06-01', '2025-06-30', '2025-06-20', 2, 1),
(7, 12.0, '2025-07-01', '2025-07-31', '2025-07-10', 0, 0),
(8, 8.0, '2025-08-01', '2025-08-31', '2025-08-15', 1, 1),
(9, 18.0, '2025-09-01', '2025-09-30', '2025-09-20', 2, 0),
(10, 20.0, '2025-10-01', '2025-10-31', '2025-10-15', 0, 1),
(11, 10.0, '2025-11-01', '2025-11-30', '2025-11-15', 1, 0),
(12, 15.0, '2025-12-01', '2025-12-31', '2025-12-20', 2, 1),
(13, 10.0, '2025-01-01', '2025-01-15', '2025-01-05', 0, 0),
(14, 25.0, '2025-02-10', '2025-02-20', '2025-02-12', 1, 1),
(15, 10.0, '2025-03-05', '2025-03-25', '2025-03-10', 2, 0),
(16, 12.0, '2025-04-05', '2025-04-25', '2025-04-15', 0, 1),
(17, 15.0, '2025-05-10', '2025-05-30', '2025-05-20', 1, 0),
(18, 20.0, '2025-06-10', '2025-06-25', '2025-06-15', 2, 1),
(19, 10.0, '2025-07-01', '2025-07-15', '2025-07-05', 0, 0),
(20, 15.0, '2025-08-01', '2025-08-20', '2025-08-10', 1, 1);

SET IDENTITY_INSERT dbo.Oferta OFF;

-- OFERTA ITEM (CORREGIDO - incluyendo porcentaje)
INSERT INTO dbo.OfertaItem (herramientaId, ofertaId, porcentaje, precioFinal) VALUES
(1, 1, 10.0, 108.45),
(2, 2, 15.0, 80.75),
(3, 3, 20.0, 120.00),
(4, 4, 10.0, 68.31),
(5, 5, 25.0, 150.00),
(6, 6, 5.0, 61.75),
(7, 7, 12.0, 78.32),
(8, 8, 8.0, 101.66),
(9, 9, 18.0, 106.60),
(10, 10, 20.0, 140.40),
(11, 11, 10.0, 54.00),
(12, 12, 15.0, 98.39),
(13, 13, 10.0, 126.00),
(14, 14, 25.0, 187.50),
(15, 15, 10.0, 76.50),
(16, 16, 12.0, 79.64),
(17, 17, 15.0, 136.00),
(18, 18, 20.0, 56.00),
(19, 19, 10.0, 189.00),
(20, 20, 15.0, 255.00);

PRINT 'Todos los datos han sido insertados correctamente';