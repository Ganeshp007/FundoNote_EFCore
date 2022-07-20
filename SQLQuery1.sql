create database FundoNote_EFCore
use FundoNote_EFCore
select * from Users
select * from Notes

sp_help Labels

truncate table Users
truncate table Notes
drop table Users
drop table Notes
drop table Labels
drop table [dbo].[__EFMigrationsHistory]

USE DB_INFORMATION_SC 
GO  
  
SELECT * FROM information_schema.key_column_usage  
WHERE table_name = 'Labels'  
  
GO