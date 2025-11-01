-- Tạo database (nếu chưa có)
IF DB_ID('GameAngryBird') IS NULL
BEGIN
    CREATE DATABASE [GameAngryBird];
    PRINT 'Database GameAngryBird created.';
END
ELSE
    PRINT 'Database GameAngryBird already exists.';

GO

-- Chuyển sang database
USE [GameAngryBird];
GO

-- Tạo bảng accounts (sử dụng password là TEXT để khớp với LoginProcess hiện tại)
IF OBJECT_ID('dbo.accounts', 'U') IS NOT NULL
    DROP TABLE dbo.accounts;
GO

CREATE TABLE dbo.accounts (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    created_at DATETIME NULL DEFAULT GETDATE()
);
GO

-- Chèn tài khoản admin + 4 tài khoản mẫu
INSERT INTO dbo.accounts (username, password) VALUES
('admin', 'admin123'),
('player1', 'pass1'),
('player2', 'pass2'),
('player3', 'pass3'),
('player4', 'pass4');
GO

-- Kiểm tra dữ liệu vừa chèn
SELECT id, username, password, created_at FROM dbo.accounts;
GO
