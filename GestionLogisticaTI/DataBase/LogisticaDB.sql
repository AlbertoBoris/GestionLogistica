USE [master]
GO
/****** Object:  Database [GestionLogisticaTI]    Script Date: 5/03/2026 23:38:42 ******/
CREATE DATABASE [GestionLogistica]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GestionLogistica', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\GestionLogistica.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GestionLogistica_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\GestionLogistica_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

USE [GestionLogistica]
GO
/****** Object:  UserDefinedTableType [dbo].[Tipo_AjusteDetalle]    Script Date: 5/03/2026 23:38:43 ******/
CREATE TYPE [dbo].[Tipo_AjusteDetalle] AS TABLE(
	[idProducto] [int] NULL,
	[cantidad] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Tipo_AuditoriaDetalle]    Script Date: 5/03/2026 23:38:43 ******/
CREATE TYPE [dbo].[Tipo_AuditoriaDetalle] AS TABLE(
	[idProducto] [int] NULL,
	[stockFisico] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Tipo_MovimientoDetalle]    Script Date: 5/03/2026 23:38:43 ******/
CREATE TYPE [dbo].[Tipo_MovimientoDetalle] AS TABLE(
	[idProducto] [int] NULL,
	[cantidad] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Tipo_PedidoDetalle]    Script Date: 5/03/2026 23:38:43 ******/
CREATE TYPE [dbo].[Tipo_PedidoDetalle] AS TABLE(
	[idProducto] [int] NULL,
	[cantidad] [int] NULL
)
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[idCliente] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](150) NOT NULL,
	[rucDni] [varchar](20) NOT NULL,
	[telefono] [varchar](20) NULL,
	[direccion] [varchar](200) NULL,
	[estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[rucDni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movimiento]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimiento](
	[idMovimiento] [int] IDENTITY(1,1) NOT NULL,
	[tipoMovimiento] [varchar](20) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[numeroDocumento] [varchar](50) NULL,
	[idUsuario] [int] NOT NULL,
	[motivo] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[idMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovimientoDetalle]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimientoDetalle](
	[idMovimientoDetalle] [int] IDENTITY(1,1) NOT NULL,
	[idMovimiento] [int] NOT NULL,
	[idProducto] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idMovimientoDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedido]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedido](
	[idPedido] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[estado] [varchar](20) NOT NULL,
	[idCliente] [int] NOT NULL,
	[fechaDespacho] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[idPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PedidoDetalle]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PedidoDetalle](
	[idPedidoDetalle] [int] IDENTITY(1,1) NOT NULL,
	[idPedido] [int] NOT NULL,
	[idProducto] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idPedidoDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[idProducto] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](150) NOT NULL,
	[descripcion] [varchar](255) NULL,
	[stockActual] [int] NOT NULL,
	[stockMinimo] [int] NOT NULL,
	[ubicacion] [varchar](100) NULL,
	[estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[nombreRol] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[correo] [varchar](100) NOT NULL,
	[contrasena] [varchar](256) NULL,
	[estado] [varchar](20) NOT NULL,
	[idRol] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Movimiento] ADD  DEFAULT (getdate()) FOR [fecha]
GO
ALTER TABLE [dbo].[Pedido] ADD  DEFAULT (getdate()) FOR [fecha]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [stockActual]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [stockMinimo]
GO
ALTER TABLE [dbo].[Movimiento]  WITH CHECK ADD  CONSTRAINT [FK_Movimiento_Usuario] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[Movimiento] CHECK CONSTRAINT [FK_Movimiento_Usuario]
GO
ALTER TABLE [dbo].[MovimientoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoDetalle_Movimiento] FOREIGN KEY([idMovimiento])
REFERENCES [dbo].[Movimiento] ([idMovimiento])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovimientoDetalle] CHECK CONSTRAINT [FK_MovimientoDetalle_Movimiento]
GO
ALTER TABLE [dbo].[MovimientoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoDetalle_Producto] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[MovimientoDetalle] CHECK CONSTRAINT [FK_MovimientoDetalle_Producto]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY([idCliente])
REFERENCES [dbo].[Cliente] ([idCliente])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_Cliente]
GO
ALTER TABLE [dbo].[PedidoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_PedidoDetalle_Pedido] FOREIGN KEY([idPedido])
REFERENCES [dbo].[Pedido] ([idPedido])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PedidoDetalle] CHECK CONSTRAINT [FK_PedidoDetalle_Pedido]
GO
ALTER TABLE [dbo].[PedidoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_PedidoDetalle_Producto] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[PedidoDetalle] CHECK CONSTRAINT [FK_PedidoDetalle_Producto]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([idRol])
REFERENCES [dbo].[Rol] ([idRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
ALTER TABLE [dbo].[Movimiento]  WITH CHECK ADD  CONSTRAINT [CK_Movimiento_Tipo] CHECK  (([tipoMovimiento]='Ajuste' OR [tipoMovimiento]='Salida' OR [tipoMovimiento]='Ingreso'))
GO
ALTER TABLE [dbo].[Movimiento] CHECK CONSTRAINT [CK_Movimiento_Tipo]
GO
ALTER TABLE [dbo].[PedidoDetalle]  WITH CHECK ADD  CONSTRAINT [CK_PedidoDetalle_Cantidad] CHECK  (([cantidad]>(0)))
GO
ALTER TABLE [dbo].[PedidoDetalle] CHECK CONSTRAINT [CK_PedidoDetalle_Cantidad]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_StockActual] CHECK  (([stockActual]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_StockActual]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_StockMinimo] CHECK  (([stockMinimo]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_StockMinimo]
GO
/****** Object:  StoredProcedure [dbo].[sp_Auditoria_AplicarAjustes]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Auditoria_AplicarAjustes]
    @idUsuario INT,
    @Detalles Tipo_AuditoriaDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM @Detalles)
        BEGIN
            RAISERROR('Debe registrar al menos un producto.',16,1);
            ROLLBACK;
            RETURN;
        END

        INSERT INTO Movimiento (
            tipoMovimiento,
            fecha,
            numeroDocumento,
            motivo,
            idUsuario
        )
        VALUES (
            'Ajuste',
            GETDATE(),
            'AUD-' + CAST(YEAR(GETDATE()) AS VARCHAR),
            'Ajuste generado por auditoría de inventario',
            @idUsuario
        );

        DECLARE @idMovimiento INT = SCOPE_IDENTITY();

        INSERT INTO MovimientoDetalle (
            idMovimiento,
            idProducto,
            cantidad
        )
        SELECT 
            @idMovimiento,
            P.idProducto,
            (D.stockFisico - P.stockActual)
        FROM @Detalles D
        INNER JOIN Producto P
            ON D.idProducto = P.idProducto
        WHERE D.stockFisico <> P.stockActual;

        UPDATE P
        SET P.stockActual = D.stockFisico
        FROM Producto P
        INNER JOIN @Detalles D
            ON P.idProducto = D.idProducto;

        COMMIT TRANSACTION;

        SELECT 'Auditoría aplicada correctamente' AS Mensaje;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Auditoria_ListarProductos]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Auditoria_ListarProductos]
    @ubicacion VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre,
        stockActual,
        stockMinimo,
        ubicacion
    FROM Producto
    WHERE 
        estado = 'Activo'
        AND (@ubicacion IS NULL 
             OR ubicacion LIKE '%' + @ubicacion + '%')
    ORDER BY nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Actualizar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Editar Cliente
CREATE   PROCEDURE [dbo].[sp_Cliente_Actualizar]
    @idCliente INT,
    @nombre VARCHAR(100),
    @rucDni VARCHAR(20),
    @telefono VARCHAR(20),
    @direccion VARCHAR(200)
AS
BEGIN
    UPDATE dbo.Cliente
    SET nombre = @nombre,
        rucDni = @rucDni,
        telefono = @telefono,
        direccion = @direccion
    WHERE idCliente = @idCliente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_CambiarEstado]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cliente_CambiarEstado]
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Cliente
    SET estado = CASE 
                    WHEN estado = 'Activo' THEN 'Inactivo'
                    ELSE 'Activo'
                 END
    WHERE idCliente = @idCliente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Insertar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cliente_Insertar]
    @nombre VARCHAR(100),
    @rucDni VARCHAR(20),
    @telefono VARCHAR(20),
    @direccion VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Cliente
    (
        nombre,
        rucDni,
        telefono,
        direccion,
        estado
    )
    VALUES
    (
        @nombre,
        @rucDni,
        @telefono,
        @direccion,
        'Activo'
    );
    SELECT SCOPE_IDENTITY() AS NuevoId;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Listar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cliente_Listar]
    @nombre VARCHAR(100) = NULL,
    @estado VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idCliente,
        nombre,
        rucDni,
        telefono,
        direccion,
        estado
    FROM dbo.Cliente
    WHERE 
        (@nombre IS NULL 
         OR nombre LIKE '%' + @nombre + '%')
    AND
        (@estado IS NULL 
         OR estado = @estado)
    ORDER BY nombre ASC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_ObtenerPorId]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cliente_ObtenerPorId]
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idCliente,
        nombre,
        rucDni,
        telefono,
        direccion,
        estado
    FROM Cliente
    WHERE idCliente = @idCliente;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Despacho_ActualizarGuia]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Editar Guía
CREATE   PROCEDURE [dbo].[sp_Despacho_ActualizarGuia]
    @idPedido INT,
    @nuevoNumeroGuia VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE M
    SET M.numeroDocumento = @nuevoNumeroGuia
    FROM Movimiento M
    INNER JOIN Pedido P
        ON M.motivo = 'Despacho de pedido ID ' + CAST(P.idPedido AS VARCHAR)
       AND M.tipoMovimiento = 'Salida'
    WHERE P.idPedido = @idPedido;

    SELECT 'Guía actualizada correctamente' AS Mensaje;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Despacho_ListarHistorial]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Listar Historial
CREATE OR ALTER PROCEDURE sp_Despacho_ListarHistorial
    @clienteId INT = NULL,
    @fechaDesde DATE = NULL,
    @fechaHasta DATE = NULL,
    @numeroGuia VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        C.nombre AS Cliente,
        P.fecha,
        P.fechaDespacho,
        M.numeroDocumento AS NumeroGuia
    FROM Pedido P

    INNER JOIN Cliente C 
        ON P.idCliente = C.idCliente

    OUTER APPLY (
        SELECT TOP 1 numeroDocumento
        FROM Movimiento
        WHERE tipoMovimiento = 'Salida'
        AND numeroDocumento IS NOT NULL
        ORDER BY fecha DESC
    ) M

    WHERE 
        P.estado = 'Despachado'
        AND (@clienteId IS NULL OR P.idCliente = @clienteId)
        AND (@fechaDesde IS NULL OR P.fechaDespacho >= @fechaDesde)
        AND (@fechaHasta IS NULL OR P.fechaDespacho < DATEADD(DAY,1,@fechaHasta))
        AND (@numeroGuia IS NULL OR M.numeroDocumento LIKE '%' + @numeroGuia + '%')

    ORDER BY P.fechaDespacho DESC;

END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Despacho_ObtenerDetalle]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Obtener Detalle
CREATE   PROCEDURE [dbo].[sp_Despacho_ObtenerDetalle]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        P.fechaDespacho,
        C.nombre AS Cliente,
        M.numeroDocumento AS NumeroGuia
    FROM Pedido P
    INNER JOIN Cliente C ON P.idCliente = C.idCliente
    INNER JOIN Movimiento M 
        ON M.motivo = 'Despacho de pedido ID ' + CAST(P.idPedido AS VARCHAR)
       AND M.tipoMovimiento = 'Salida'
    WHERE P.idPedido = @idPedido;

    SELECT 
        PR.nombre AS Producto,
        PD.cantidad
    FROM PedidoDetalle PD
    INNER JOIN Producto PR ON PD.idProducto = PR.idProducto
    WHERE PD.idPedido = @idPedido;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Movimiento_ConsultarHistorial]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Movimiento_ConsultarHistorial]
    @tipoMovimiento VARCHAR(20) = NULL,
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL,
    @producto VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.idMovimiento,
        M.fecha,
        M.tipoMovimiento,
        M.numeroDocumento,
        M.motivo,
        P.nombre AS Producto,
        MD.cantidad,
        U.nombre AS Usuario
    FROM Movimiento M
    INNER JOIN MovimientoDetalle MD 
        ON M.idMovimiento = MD.idMovimiento
    INNER JOIN Producto P 
        ON MD.idProducto = P.idProducto
    INNER JOIN Usuario U 
        ON M.idUsuario = U.idUsuario
    WHERE
        (@tipoMovimiento IS NULL OR M.tipoMovimiento = @tipoMovimiento)
        AND (@fechaInicio IS NULL OR M.fecha >= @fechaInicio)
        AND (@fechaFin IS NULL OR M.fecha < DATEADD(DAY,1,@fechaFin))
        AND (@producto IS NULL OR P.nombre LIKE '%' + @producto + '%')
    ORDER BY M.fecha DESC
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Movimiento_InsertarAjuste]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Movimiento_InsertarAjuste]
    @numeroDocumento VARCHAR(50),
    @motivo VARCHAR(200),
    @idUsuario INT,
    @detalles Tipo_AjusteDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        -- Validar que haya al menos un producto
        IF NOT EXISTS (SELECT 1 FROM @detalles)
        BEGIN
            RAISERROR('Debe registrar al menos un producto para el ajuste.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Crear cabecera movimiento
        INSERT INTO Movimiento
        (
            tipoMovimiento,
            fecha,
            numeroDocumento,
            motivo,
            idUsuario
        )
        VALUES
        (
            'Ajuste',
            GETDATE(),
            @numeroDocumento,
            @motivo,
            @idUsuario
        );

        DECLARE @idMovimiento INT = SCOPE_IDENTITY();

        -- Insertar detalle
        INSERT INTO MovimientoDetalle
        (
            idMovimiento,
            idProducto,
            cantidad
        )
        SELECT
            @idMovimiento,
            idProducto,
            cantidad
        FROM @detalles;

        -- Actualizar stock
        UPDATE P
        SET P.stockActual = P.stockActual + D.cantidad
        FROM Producto P
        INNER JOIN @detalles D
            ON P.idProducto = D.idProducto;

        COMMIT TRANSACTION;

        SELECT 
            'Ajuste de inventario registrado correctamente.' AS Mensaje,
            @idMovimiento AS idMovimiento;

    END TRY

    BEGIN CATCH

        ROLLBACK TRANSACTION;

        SELECT ERROR_MESSAGE() AS ErrorMensaje;

    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Movimiento_InsertarIngreso]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Movimiento_InsertarIngreso]
    @numeroDocumento VARCHAR(50),
    @motivo VARCHAR(200) = NULL,
    @idUsuario INT,
    @detalles Tipo_MovimientoDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION

        INSERT INTO Movimiento
        (
            tipoMovimiento,
            fecha,
            numeroDocumento,
            motivo,
            idUsuario
        )
        VALUES
        (
            'Ingreso',
            GETDATE(),
            @numeroDocumento,
            @motivo,
            @idUsuario
        )

        DECLARE @idMovimiento INT = SCOPE_IDENTITY()

        INSERT INTO MovimientoDetalle
        (
            idMovimiento,
            idProducto,
            cantidad
        )
        SELECT
            @idMovimiento,
            idProducto,
            cantidad
        FROM @detalles

        UPDATE P
        SET P.stockActual = P.stockActual + D.cantidad
        FROM Producto P
        INNER JOIN @detalles D
            ON P.idProducto = D.idProducto

        COMMIT TRANSACTION

        SELECT 'Ingreso registrado correctamente'

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT ERROR_MESSAGE()
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_Actualizar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_Actualizar]
    @idPedido INT,
    @idCliente INT,
    @Detalles Tipo_PedidoDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar estado
        IF NOT EXISTS (
            SELECT 1 FROM Pedido
            WHERE idPedido = @idPedido
              AND estado = 'Pendiente'
        )
        BEGIN
            RAISERROR('Solo se puede modificar pedidos en estado Pendiente.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Actualizar cabecera
        UPDATE Pedido
        SET idCliente = @idCliente
        WHERE idPedido = @idPedido;

        -- Eliminar detalle anterior
        DELETE FROM PedidoDetalle
        WHERE idPedido = @idPedido;

        -- Insertar nuevo detalle
        INSERT INTO PedidoDetalle (idPedido, idProducto, cantidad)
        SELECT @idPedido, idProducto, cantidad
        FROM @Detalles;

        COMMIT TRANSACTION;

        SELECT 'Pedido actualizado correctamente' AS Mensaje;

    END TRY
    BEGIN CATCH
        ROLLBACK;
        SELECT ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_AutorizarSalida]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_AutorizarSalida]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar stock suficiente
    IF EXISTS
    (
        SELECT 1
        FROM PedidoDetalle PD
        INNER JOIN Producto P
            ON PD.idProducto = P.idProducto
        WHERE PD.idPedido = @idPedido
        AND PD.cantidad > P.stockActual
    )
    BEGIN
        RAISERROR('No hay stock suficiente para autorizar el pedido.',16,1);
        RETURN;
    END

    UPDATE Pedido
    SET estado = 'Autorizado'
    WHERE idPedido = @idPedido;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_Cancelar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Cancelar Pedido
CREATE   PROCEDURE [dbo].[sp_Pedido_Cancelar]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Pedido
        WHERE idPedido = @idPedido
          AND estado = 'Pendiente'
    )
    BEGIN
        RAISERROR('Solo se puede cancelar pedidos en estado Pendiente.',16,1);
        RETURN;
    END

    UPDATE Pedido
    SET estado = 'Cancelado'
    WHERE idPedido = @idPedido;

    SELECT 'Pedido cancelado correctamente' AS Mensaje;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_ConfirmarDespacho]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_ConfirmarDespacho]
    @idPedido INT,
    @idUsuario INT,
    @numeroDocumento VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar pedido autorizado
        IF NOT EXISTS (
            SELECT 1 
            FROM Pedido
            WHERE idPedido = @idPedido
              AND estado = 'Autorizado'
        )
        BEGIN
            RAISERROR('El pedido no existe o no está autorizado.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Validar stock suficiente
        IF EXISTS (
            SELECT 1
            FROM PedidoDetalle PD
            INNER JOIN Producto P
                ON PD.idProducto = P.idProducto
            WHERE PD.idPedido = @idPedido
              AND PD.cantidad > P.stockActual
        )
        BEGIN
            RAISERROR('Stock insuficiente para confirmar despacho.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Insertar cabecera movimiento tipo Salida
        INSERT INTO Movimiento (
            tipoMovimiento,
            fecha,
            numeroDocumento,
            motivo,
            idUsuario
        )
        VALUES (
            'Salida',
            GETDATE(),
            @numeroDocumento,
            'Despacho de pedido ID ' + CAST(@idPedido AS VARCHAR),
            @idUsuario
        );

        DECLARE @idMovimiento INT = SCOPE_IDENTITY();

        -- Insertar detalles desde pedido
        INSERT INTO MovimientoDetalle (
            idMovimiento,
            idProducto,
            cantidad
        )
        SELECT
            @idMovimiento,
            idProducto,
            cantidad
        FROM PedidoDetalle
        WHERE idPedido = @idPedido;

        -- Descontar stock
        UPDATE P
        SET P.stockActual = P.stockActual - PD.cantidad
        FROM Producto P
        INNER JOIN PedidoDetalle PD
            ON P.idProducto = PD.idProducto
        WHERE PD.idPedido = @idPedido;

        -- Cambiar estado del pedido
        UPDATE Pedido
        SET estado = 'Despachado',
        fechaDespacho = GETDATE()
        WHERE idPedido = @idPedido;

        COMMIT TRANSACTION;

        SELECT 
            'Despacho confirmado correctamente' AS Mensaje,
            @idMovimiento AS idMovimientoGenerado;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_DetalleAutorizacion]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Pedido_DetalleAutorizacion]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        C.nombre AS Cliente
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    WHERE P.idPedido = @idPedido

    SELECT 
        PD.idProducto,
        PR.nombre AS Producto,
        PD.cantidad,
        PR.stockActual
    FROM PedidoDetalle PD
    INNER JOIN Producto PR
        ON PD.idProducto = PR.idProducto
    WHERE PD.idPedido = @idPedido
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_Insertar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Insertar Pedido
CREATE   PROCEDURE [dbo].[sp_Pedido_Insertar]
    @idCliente INT,
    @Detalles Tipo_PedidoDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar cliente activo
        IF NOT EXISTS (
            SELECT 1 
            FROM Cliente 
            WHERE idCliente = @idCliente 
              AND estado = 'Activo'
        )
        BEGIN
            RAISERROR('Cliente no válido o inactivo.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Validar que haya productos
        IF NOT EXISTS (SELECT 1 FROM @Detalles)
        BEGIN
            RAISERROR('Debe agregar al menos un producto al pedido.',16,1);
            ROLLBACK;
            RETURN;
        END

        -- Insertar cabecera
        INSERT INTO Pedido (
            fecha,
            estado,
            idCliente
        )
        VALUES (
            GETDATE(),
            'Pendiente',
            @idCliente
        );

        DECLARE @idPedido INT = SCOPE_IDENTITY();

        -- Insertar detalles
        INSERT INTO PedidoDetalle (
            idPedido,
            idProducto,
            cantidad
        )
        SELECT
            @idPedido,
            idProducto,
            cantidad
        FROM @Detalles;

        COMMIT TRANSACTION;

        SELECT 
            'Pedido registrado correctamente' AS Mensaje,
            @idPedido AS idPedido;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_Listar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_Listar]
    @estado VARCHAR(20) = NULL,
    @idCliente INT = NULL,
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        P.estado,
        C.nombre AS Cliente,
        COUNT(PD.idProducto) AS CantidadItems
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    LEFT JOIN PedidoDetalle PD
        ON P.idPedido = PD.idPedido
    WHERE
        (@estado IS NULL OR P.estado = @estado)
        AND (@idCliente IS NULL OR P.idCliente = @idCliente)
        AND (@fechaInicio IS NULL OR P.fecha >= @fechaInicio)
        AND (@fechaFin IS NULL OR P.fecha <= @fechaFin)
    GROUP BY 
        P.idPedido, P.fecha, P.estado, C.nombre
    ORDER BY P.fecha DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_ListarAutorizados]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_ListarAutorizados]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        C.nombre AS Cliente,
        COUNT(PD.idProducto) AS CantidadItems
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    INNER JOIN PedidoDetalle PD
        ON P.idPedido = PD.idPedido
    WHERE P.estado = 'Autorizado'
    GROUP BY 
        P.idPedido,
        P.fecha,
        C.nombre
    ORDER BY P.fecha;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_ListarPendientes]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_ListarPendientes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        C.nombre AS Cliente,
        COUNT(PD.idProducto) AS cantidadItems
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    INNER JOIN PedidoDetalle PD
        ON P.idPedido = PD.idPedido
    WHERE P.estado = 'Pendiente'
    GROUP BY 
        P.idPedido,
        P.fecha,
        C.nombre
    ORDER BY P.fecha DESC
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_ObtenerDetalle]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_ObtenerDetalle]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cabecera
    SELECT 
        P.idPedido,
        P.fecha,
        P.fechaDespacho,
        P.estado,
        C.nombre AS Cliente,
        C.idCliente
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    WHERE P.idPedido = @idPedido;

    -- Detalle
    SELECT 
        PD.idProducto,
        PR.nombre AS Producto,
        PD.cantidad
    FROM PedidoDetalle PD
    INNER JOIN Producto PR
        ON PD.idProducto = PR.idProducto
    WHERE PD.idPedido = @idPedido;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Pedido_ObtenerDetalleDespacho]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Pedido_ObtenerDetalleDespacho]
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha,
        P.fechaDespacho,
        C.nombre AS Cliente,
        PD.idProducto,
        PR.nombre AS Producto,
        PD.cantidad
    FROM Pedido P
    INNER JOIN Cliente C
        ON P.idCliente = C.idCliente
    INNER JOIN PedidoDetalle PD
        ON P.idPedido = PD.idPedido
    INNER JOIN Producto PR
        ON PD.idProducto = PR.idProducto
    WHERE P.idPedido = @idPedido;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Actualizar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_Actualizar]
    @idProducto INT,
    @nombre VARCHAR(200),
    @descripcion VARCHAR(500),
    @stockMinimo INT,
    @ubicacion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Producto
    SET
        nombre = @nombre,
        descripcion = @descripcion,
        stockMinimo = @stockMinimo,
        ubicacion = @ubicacion
    WHERE idProducto = @idProducto
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Buscar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_Buscar]
    @nombre VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre
    FROM Producto
    WHERE 
        estado = 'Activo'
        AND nombre LIKE '%' + @nombre + '%'
    ORDER BY nombre
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_CambiarEstado]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_CambiarEstado]
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Producto
    SET estado =
        CASE
            WHEN estado = 'Activo' THEN 'Inactivo'
            ELSE 'Activo'
        END
    WHERE idProducto = @idProducto
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_ConsultarStock]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_ConsultarStock]
    @nombreProducto VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idProducto,
        P.nombre,
        P.stockActual,
        P.stockMinimo,
        P.ubicacion,
        CASE 
            WHEN P.stockActual <= P.stockMinimo THEN 'Crítico'
            ELSE 'Normal'
        END AS estadoStock
    FROM dbo.Producto P
    WHERE 
        P.estado = 'Activo'
        AND (
            @nombreProducto IS NULL 
            OR P.nombre LIKE '%' + @nombreProducto + '%'
        )
    ORDER BY P.nombre ASC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Insertar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_Insertar]
    @nombre VARCHAR(200),
    @descripcion VARCHAR(500),
    @stockMinimo INT,
    @ubicacion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Producto
    (
        nombre,
        descripcion,
        stockActual,
        stockMinimo,
        ubicacion,
        estado
    )
    VALUES
    (
        @nombre,
        @descripcion,
        0,
        @stockMinimo,
        @ubicacion,
        'Activo'
    )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_Listar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_Listar]
    @nombre VARCHAR(200) = NULL,
    @estado VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre,
        descripcion,
        stockActual,
        stockMinimo,
        ubicacion,
        estado
    FROM Producto
    WHERE
        (@nombre IS NULL OR nombre LIKE '%' + @nombre + '%')
        AND (@estado IS NULL OR estado = @estado)
    ORDER BY nombre
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_ListarActivos]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Producto_ListarActivos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idProducto,
        nombre,
        stockActual,
        ubicacion
    FROM Producto
    WHERE estado = 'Activo'
    ORDER BY nombre
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_ObtenerPorId]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_ObtenerPorId]
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre,
        descripcion,
        stockActual,
        stockMinimo,
        ubicacion,
        estado
    FROM Producto
    WHERE idProducto = @idProducto
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Producto_ObtenerStock]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Producto_ObtenerStock]
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        stockActual
    FROM Producto
    WHERE idProducto = @idProducto
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_Ajustes]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_Ajustes]
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.idMovimiento,
        M.fecha,
        M.numeroDocumento,
        M.motivo,
        P.nombre AS Producto,
        MD.cantidad,
        U.nombre AS Usuario
    FROM Movimiento M
    INNER JOIN MovimientoDetalle MD 
        ON M.idMovimiento = MD.idMovimiento
    INNER JOIN Producto P 
        ON MD.idProducto = P.idProducto
    INNER JOIN Usuario U 
        ON M.idUsuario = U.idUsuario
    WHERE M.tipoMovimiento = 'Ajuste'
      AND (@fechaInicio IS NULL OR M.fecha >= @fechaInicio)
      AND (@fechaFin IS NULL OR M.fecha < DATEADD(DAY,1,@fechaFin))
    ORDER BY M.fecha DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_DespachosPorFecha]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE sp_Reporte_DespachosPorFecha
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPedido,
        P.fecha AS FechaPedido,
        P.fechaDespacho,
        C.nombre AS Cliente,
        M.numeroDocumento AS NumeroGuia,
        U.nombre AS UsuarioDespacho
    FROM Pedido P
    INNER JOIN Cliente C 
        ON P.idCliente = C.idCliente
    LEFT JOIN Movimiento M
        ON M.tipoMovimiento = 'Salida'
    LEFT JOIN Usuario U
        ON M.idUsuario = U.idUsuario
    WHERE 
        P.estado = 'Despachado'
        AND (@fechaInicio IS NULL OR P.fechaDespacho >= @fechaInicio)
        AND (@fechaFin IS NULL OR P.fechaDespacho < DATEADD(DAY,1,@fechaFin))
    ORDER BY P.fechaDespacho DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_InventarioActual]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_InventarioActual]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre,
        stockActual,
        stockMinimo,
        ubicacion,
        estado
    FROM Producto
    ORDER BY nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_LogisticoGeneral]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_LogisticoGeneral]
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.fecha,
        M.tipoMovimiento,
        M.numeroDocumento,
        M.motivo,
        P.nombre AS Producto,
        MD.cantidad,
        U.nombre AS Usuario,
        CASE 
            WHEN M.tipoMovimiento = 'Salida' THEN 'Despacho'
            WHEN M.tipoMovimiento = 'Ingreso' THEN 'Ingreso'
            WHEN M.tipoMovimiento = 'Ajuste' THEN 'Ajuste'
        END AS Categoria
    FROM Movimiento M
    INNER JOIN MovimientoDetalle MD 
        ON M.idMovimiento = MD.idMovimiento
    INNER JOIN Producto P 
        ON MD.idProducto = P.idProducto
    INNER JOIN Usuario U 
        ON M.idUsuario = U.idUsuario
    WHERE 
        (@fechaInicio IS NULL OR M.fecha >= @fechaInicio)
        AND (@fechaFin IS NULL OR M.fecha < DATEADD(DAY,1,@fechaFin))
    ORDER BY M.fecha DESC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_MovimientosPorFecha]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_MovimientosPorFecha]
    @fechaInicio DATETIME,
    @fechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.idMovimiento,
        M.fecha,
        M.tipoMovimiento,
        M.numeroDocumento,
        M.motivo,
        P.nombre AS Producto,
        MD.cantidad,
        U.nombre AS Usuario
    FROM Movimiento M
    INNER JOIN MovimientoDetalle MD 
        ON M.idMovimiento = MD.idMovimiento
    INNER JOIN Producto P 
        ON MD.idProducto = P.idProducto
    INNER JOIN Usuario U 
        ON M.idUsuario = U.idUsuario
    WHERE M.fecha >= @fechaInicio
  AND M.fecha < DATEADD(DAY,1,@fechaFin)
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_ProductosBajoStock]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_ProductosBajoStock]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProducto,
        nombre,
        stockActual,
        stockMinimo,
        ubicacion
    FROM Producto
    WHERE stockActual <= stockMinimo
    ORDER BY stockActual ASC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Reporte_ResumenInventario]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Reporte_ResumenInventario]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(*) AS TotalProductos,
        SUM(stockActual) AS TotalUnidades,
        SUM(CASE WHEN stockActual <= stockMinimo THEN 1 ELSE 0 END) AS ProductosCriticos
    FROM Producto
    WHERE estado = 'Activo';
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Actualizar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Actualizar Usurio
CREATE   PROCEDURE [dbo].[sp_Usuario_Actualizar]
    @idUsuario INT,
    @nombre VARCHAR(100),
    @correo VARCHAR(100),
    @idRol INT
AS
BEGIN
    UPDATE Usuario
    SET nombre = @nombre,
        correo = @correo,
        idRol = @idRol
    WHERE idUsuario = @idUsuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ActualizarPassword]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuario_ActualizarPassword]
    @idUsuario INT,
    @passwordHash VARCHAR(255)
AS
BEGIN
    UPDATE Usuario
    SET contrasena = @passwordHash
    WHERE idUsuario = @idUsuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_CambiarEstado]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuario_CambiarEstado]
    @idUsuario INT
AS
BEGIN
    UPDATE Usuario
    SET estado = CASE 
                    WHEN estado = 'Activo' THEN 'Inactivo'
                    ELSE 'Activo'
                 END
    WHERE idUsuario = @idUsuario;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Insertar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuario_Insertar]
    @nombre VARCHAR(100),
    @correo VARCHAR(100),
    @passwordHash VARCHAR(255),
    @idRol INT
AS
BEGIN
    INSERT INTO Usuario (nombre, correo, contrasena, estado, idRol)
    VALUES (@nombre, @correo, @passwordHash, 'Activo', @idRol);
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Listar]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuario_Listar]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.idUsuario,
        U.nombre,
        U.correo,
        U.estado,
        R.idRol,
        R.nombreRol
    FROM Usuario U
    INNER JOIN Rol R ON U.idRol = R.idRol
    ORDER BY U.nombre;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Login]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuario_Login]
    @correo VARCHAR(100),
    @contrasenaHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.idUsuario,
        U.nombre,
        U.correo,
        R.nombreRol AS Rol
    FROM Usuario U
    INNER JOIN Rol R
        ON U.idRol = R.idRol
    WHERE 
        U.correo = @correo
        AND U.contrasena = @contrasenaHash
        AND U.estado = 'Activo';
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ObtenerPorId]    Script Date: 5/03/2026 23:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Usuario_ObtenerPorId]
    @idUsuario INT
AS
BEGIN
    SELECT 
        idUsuario,
        nombre,
        correo,
        estado,
        idRol
    FROM Usuario
    WHERE idUsuario = @idUsuario;
END;

GO
USE [master]
GO
ALTER DATABASE [GestionLogisticaTI] SET  READ_WRITE 
GO
