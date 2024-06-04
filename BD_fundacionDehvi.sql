use master;

----- DANGER ZONE -----
DROP DATABASE BD_fundacionDehvi;

---- CREACION DE BASE DE DATOS ----
Create database BD_fundacionDehvi;
use BD_fundacionDehvi;

------------------------------------- CREACION DE TABLAS -------------------------------------
---- TABLA ROLE ----

CREATE TABLE Rol(
    idRol tinyint NOT NULL ,
    nombre varchar(20) NOT NULL,
    PRIMARY KEY (idRol)
)

CREATE TABLE Departamento(
    idDepartamento bigint NOT NULL IDENTITY(1,1),
    nombre varchar(50) NOT NULL,
    estado bit NOT NULL,
    PRIMARY KEY (idDepartamento)
)
---- TABLA USUARIO ----
CREATE TABLE Usuario(
    idUsuario bigint NOT NULL IDENTITY(1,1),

    cedulaFisica varchar(12) NOT NULL UNIQUE,
    nombre varchar(50) NOT NULL,
    apellido1 varchar(50) NOT NULL,
    apellido2 varchar(50) NOT NULL,

    correo varchar(255) NOT NULL,
    contrasena varchar(64) NOT NULL, 
    
    idRol tinyint NOT NULL,
    idDepartamento bigint NOT NULL,
    estado bit NOT NULL,

    fechaCreacion datetime NOT NULL,
    rutaImg varchar(255) NULL,

    PRIMARY KEY (idUsuario),
    FOREIGN KEY (idRol) REFERENCES Rol(idRol),
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(idDepartamento)
)

CREATE TABLE Procedimiento(
    idProcedimiento bigint NOT NULL IDENTITY(1,1),
    idDepartamento bigint NOT NULL,
    codProcedimiento varchar(50) NOT NULL,

    nombre varchar(50) NOT NULL,
    descripcion varchar(255) NOT NULL,
    objetivo varchar(255) NOT NULL,
    estado bit NOT NULL,

    idUsuarioCreador bigint NOT NULL,
    fechaCreacion datetime NOT NULL,
    PRIMARY KEY (idProcedimiento),
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(idDepartamento),
    FOREIGN KEY (idUsuarioCreador) REFERENCES Usuario(idUsuario)
)

CREATE TABLE Manual (
    idManual bigint NOT NULL IDENTITY(1,1),
    idProcedimiento bigint NOT NULL,
    nombre varchar(50) NOT NULL,
    codReferencia varchar(50) NOT NULL,
    estado bit NOT NULL,
    idUsuarioCreador bigint NOT NULL,
    fechaCreacion datetime NOT NULL,
    PRIMARY KEY (idManual),
    FOREIGN KEY (idProcedimiento) REFERENCES Procedimiento(idProcedimiento),
    FOREIGN KEY (idUsuarioCreador) REFERENCES Usuario(idUsuario)
)

CREATE TABLE DocManual(
    idDocManual bigint NOT NULL IDENTITY(1,1),
    idManual bigint NOT NULL,
    versionDoc bigint NOT NULL,
    ruta varchar(255) NOT NULL,
    fechaCreacion datetime NOT NULL,
    estado bit NOT NULL,
    idUsuarioCreador bigint NOT NULL,
    PRIMARY KEY (idDocManual),
    FOREIGN KEY (idManual) REFERENCES Manual(idManual),
    FOREIGN KEY (idUsuarioCreador) REFERENCES Usuario(idUsuario)
)

--------------- INSERTS ROLES---------------

INSERT INTO Rol VALUES (1, 'Administrador')
INSERT INTO Rol VALUES (2, 'Jeferatura')
INSERT INTO Rol VALUES (3, 'Empleado')
INSERT INTO Rol VALUES (4, 'Externo')

--------------- INSERTS DEPARTAMENTOS---------------
INSERT INTO Departamento (nombre, estado) VALUES ('Gestion Humana', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Finanzas', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Gestion de Calidad', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Alianzas', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Atencion Integral', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Centros Infantiles', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Operaciones', 1)
INSERT INTO Departamento (nombre, estado) VALUES ('Publicidad', 1)

--------------- INSERTS USUARIOS---------------
INSERT INTO Usuario (nombre, apellido1, apellido2, cedulaFisica, correo, contrasena, idRol, idDepartamento, estado, fechaCreacion) 
VALUES ('Derek Sebastian', 'Leiva', 'Villalobos', '305400042', 'dleiva00042@ufide.ac.cr', 'c9a9899fdd99ae5fb9a3f2e15bbef7df69bcb0b603811ad246316086f89f366f', 1, 1, 1, GETDATE())

INSERT INTO Usuario (nombre, apellido1, apellido2, cedulaFisica, correo, contrasena, idRol, idDepartamento, estado, fechaCreacion) 
VALUES ('Marianne', 'von Herold', 'Hering', '118870134', 'hvon70134@ufide.ac.cr', 'd1901ca699a6eec6b2086c101f18702aca87a4475efbc81c0662f436bcf13e12', 1, 1, 1, GETDATE())

