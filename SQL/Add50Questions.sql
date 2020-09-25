USE [AskMeDude]
GO


declare @id int
set @id=1
while(@id<=50)
begin
INSERT INTO [dbo].[Question]
           ([Text]
           ,[A]
           ,[B]
           ,[C]
           ,[D]
           ,[E]
           ,[RightAnswer]
           ,[UserId]
           ,[CategoryId]
           ,[IsActive])
     VALUES
           (cast (@id as nvarchar(5),'A','B','C','D','E','A',1,1,1)
set @id=@id+1
end


