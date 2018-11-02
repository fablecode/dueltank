﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

MERGE dbo.AspNetRoles AS Target
USING
(
	SELECT 
		* 
	FROM
	(
		VALUES
			(1, NULL, 'Admin', 'Admin'),
			(2, NULL, 'User', 'User')
		
	)As s(Id, ConcurrencyStamp, Name, NormalizedName)
) As Source
ON 
	Target.Name = Source.Name
WHEN NOT MATCHED THEN
	INSERT (Id, ConcurrencyStamp, Name, NormalizedName)
	VALUES (Id, Source.ConcurrencyStamp, Source.Name, Source.NormalizedName);

MERGE dbo.DeckType AS Target
USING
(
	SELECT 
		* 
	FROM
	(
		VALUES
			('Main', GetDate(), GetDate()),
			('Extra', GetDate(), GetDate()),
			('Side', GetDate(), GetDate())
		
	)As s(Name, Created, Updated)
) As Source
ON 
	Target.Name = Source.Name
WHEN NOT MATCHED THEN
	INSERT (Name, Created, Updated)
	VALUES (Source.Name, Source.Created, Source.Updated);
