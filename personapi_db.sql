CREATE DATABASE persona_db;
GO

ALTER AUTHORIZATION
ON DATABASE::persona_db
TO sa;
GO

USE persona_db;
GO

CREATE TABLE profesion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nom VARCHAR(90) NOT NULL,
    des VARCHAR(MAX)
);
GO

CREATE TABLE persona (
    cc VARCHAR(15) PRIMARY KEY,
    nombre VARCHAR(45) NOT NULL,
    apellido VARCHAR(45) NOT NULL,
    genero CHAR(1) NOT NULL,
    edad INT,

    CONSTRAINT CK_persona_genero
    CHECK (genero IN ('M', 'F'))
);
GO

CREATE TABLE estudios (
    id_prof INT NOT NULL,
    cc_per VARCHAR(15) NOT NULL,
    fecha DATE,
    univer VARCHAR(50),

    CONSTRAINT PK_estudios
    PRIMARY KEY (id_prof, cc_per),

    CONSTRAINT FK_estudios_profesion
    FOREIGN KEY (id_prof)
    REFERENCES profesion(id),

    CONSTRAINT FK_estudios_persona
    FOREIGN KEY (cc_per)
    REFERENCES persona(cc)
);
GO

CREATE TABLE telefono (
    num VARCHAR(15) PRIMARY KEY,
    oper VARCHAR(45),
    dueno VARCHAR(15) NOT NULL,

    CONSTRAINT FK_telefono_persona
    FOREIGN KEY (dueno)
    REFERENCES persona(cc)
);
GO