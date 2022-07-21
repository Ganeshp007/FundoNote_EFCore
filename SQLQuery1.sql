create database FundoNote_EFCore
use FundoNote_EFCore
select * from Users
select * from Notes
select * from Labels

sp_help Labels

truncate table Users
truncate table Notes
truncate table Labels
drop table Users
drop table Notes
drop table Labels
drop table [dbo].[__EFMigrationsHistory]


select LabelName from Labels where USER_ID=1 and Noted=1