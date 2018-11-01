/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = 'ArchetypeCatalog')
BEGIN
	PRINT 'Catalog exists'
END
ELSE
BEGIN
	PRINT 'Catalog does not exists';
	PRINT 'Creating catalog archetype......'
	CREATE FULLTEXT CATALOG [ArchetypeCatalog]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];
END

IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = 'CardCatalog')
BEGIN
	PRINT 'Catalog exists'
END
ELSE
BEGIN
	PRINT 'Catalog does not exists';
	PRINT 'Creating catalog card......'
	CREATE FULLTEXT CATALOG [CardCatalog]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];
END

IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = 'DeckCatalog')
BEGIN
	PRINT 'Catalog exists'
END
ELSE
BEGIN
	PRINT 'Catalog does not exists';
	PRINT 'Creating catalog card......'
	CREATE FULLTEXT CATALOG [DeckCatalog]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];
END
