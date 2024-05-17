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
    estado bit NOT NULL,
    fechaCreacion datetime NOT NULL,
    rutaImg varchar(255) NULL,

    PRIMARY KEY (idUsuario),
    FOREIGN KEY (idRol) REFERENCES Rol(idRol)
)

CREATE TABLE Departamento(
    idDepartamento bigint NOT NULL IDENTITY(1,1),
    nombre varchar(50) NOT NULL,
    estado bit NOT NULL,
    PRIMARY KEY (idDepartamento)
)

CREATE TABLE Usuario_Departamento(
    idUsuarioDepartamento bigint NOT NULL IDENTITY(1,1),
    idUsuario bigint NOT NULL,
    idDepartamento bigint NOT NULL,
    PRIMARY KEY (idUsuarioDepartamento),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(idDepartamento)
)

CREATE TABLE Procedimiento(
    idPrcoedimiento bigint NOT NULL IDENTITY(1,1),
    idDepartamento bigint NOT NULL,
    codProcedimiento varchar(50) NOT NULL,

    nombre varchar(50) NOT NULL,
    descripcion varchar(255) NOT NULL,
    objetivo varchar(255) NOT NULL,
    estado bit NOT NULL,

    idUsuarioCreador bigint NOT NULL,
    fechaCreacion datetime NOT NULL,
    PRIMARY KEY (idPrcoedimiento),
    FOREIGN KEY (idDepartamento) REFERENCES Departamento(idDepartamento),
    FOREIGN KEY (idUsuarioCreador) REFERENCES Usuario(idUsuario)
)

CREATE TABLE Manual (
    idManual bigint NOT NULL IDENTITY(1,1),
    idProcedimiento bigint NOT NULL,
    nombre varchar(50) NOT NULL,
    codReferencia varchar(50) NOT NULL,
    estado bit NOT NULL,
    PRIMARY KEY (idManual),
    FOREIGN KEY (idProcedimiento) REFERENCES Procedimiento(idPrcoedimiento)
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

--------------- INSERTS USUARIOS---------------
INSERT INTO Usuario (nombre, apellido1, apellido2, cedulaFisica, correo, contrasena, idRol, estado, fechaCreacion) 
VALUES ('305400042', 'Derek Sebastian', 'Leiva', 'Villalobos', '305400042', 'dleiva00042@ufide.ac.cr', 'contrasegura', 1, 1, GETDATE())

INSERT INTO Usuario (nombre, apellido1, apellido2, cedulaFisica, correo, contrasena, idRol, estado, fechaCreacion) 
VALUES ('118870134', 'Marianne', 'von Herold', 'Hering', '118', 'hvon0134@ufide.ac.cr', 'contrasegura', 1, 1, GETDATE())

