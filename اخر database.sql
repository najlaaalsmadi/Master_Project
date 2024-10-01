ALTER TABLE [dbo].[Learning_Equipment]
ADD category_id INT,
ADD CONSTRAINT fk_category
FOREIGN KEY (category_id) REFERENCES Categories(category_id);


ALTER TABLE Learning_Equipment
ADD category_id INT;

ALTER TABLE Learning_Equipment
ADD CONSTRAINT fk_category
FOREIGN KEY (category_id) REFERENCES Categories(category_id);

USE [project_Master_core];
CREATE TABLE EmailMessage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    DateSent DATETIME NOT NULL DEFAULT GETDATE()
);


