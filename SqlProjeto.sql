CREATE DATABASE ProjetoAcessoo;
GO
USE ProjetoAcessoo;
GO

CREATE TABLE Usuario (
Id INT PRIMARY KEY,
Nome VARCHAR(100) NOT NULL
);

CREATE TABLE Ambiente (
Id INT PRIMARY KEY,
Nome VARCHAR(100) NOT NULL
);

CREATE TABLE UsuarioAmbiente (
UsuarioId INT NOT NULL,
AmbienteId INT NOT NULL,
PRIMARY KEY (UsuarioId, AmbienteId),
FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
FOREIGN KEY (AmbienteId) REFERENCES Ambiente(Id)
);

CREATE TABLE LogAcesso (
Id INT IDENTITY(1,1) PRIMARY KEY,
DtAcesso DATETIME NOT NULL,
UsuarioId INT NOT NULL,
AmbienteId INT NOT NULL,
TipoAcesso BIT NOT NULL,
FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
FOREIGN KEY (AmbienteId) REFERENCES Ambiente(Id)
);
ALTER DATABASE ProjetoAcessoo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE ProjetoAcessoo;

USE ProjetoAcessoo;
GO


SELECT name 
FROM sys.tables
WHERE type = 'U';

USE ProjetoAcessoo;
GO
USE ProjetoAcessoo;
GO

SELECT * FROM Usuario;
