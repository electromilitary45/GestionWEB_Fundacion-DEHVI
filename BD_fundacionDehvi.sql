use master;

----- DANGER ZONE -----
DROP DATABASE BD_fundacionDehvi;

---- CREACION DE BASE DE DATOS ----
Create database BD_fundacionDehvi;
use BD_fundacionDehvi;

------------------------------------- CREACION DE TABLAS -------------------------------------


---- TABLA USUARIO ----
CREATE TABLE Usuario(
    idUsuario bigint NOT NULL IDENTITY(1,1),
    cedulaFisica varchar(12) NOT NULL UNIQUE,
    nombre varchar(50) NOT NULL,
    apellido1 varchar(50) NOT NULL,
    apellido2 varchar(50) NOT NULL,
    correo varchar(255) NOT NULL,
    contrasena varchar(64) NOT NULL, 
    rol tinyint NOT NULL,
    estado bit NOT NULL,
    PRIMARY KEY (idUsuario)
)

CREATE TABLE Departamento(
    idDepartamento bigint NOT NULL IDENTITY(1,1),
    nombre varchar(50) NOT NULL,
    estado bit NOT NULL,
    PRIMARY KEY (idDepartamento)
)

CREATE TABLE Usuario_Departamento(
    idUsuario bigint NOT NULL,
    idDepartamento bigint NOT NULL,
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
