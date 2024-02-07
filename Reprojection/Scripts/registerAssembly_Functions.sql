ALTER DATABASE [CorinaTest] SET TRUSTWORTHY ON

CREATE ASSEMBLY Reprojection
FROM 'C:\Reprojection\Reprojection.dll'
WITH PERMISSION_SET = UNSAFE;
GO

CREATE FUNCTION dbo.GeometryToGeometry(@geom geometry, @srid int) RETURNS geometry
EXTERNAL NAME Reprojection.[Reprojection.UserDefinedFunctions].GeometryToGeometry
GO

CREATE FUNCTION dbo.GeographyToGeometry(@geog geography, @srid int) RETURNS geometry
EXTERNAL NAME Reprojection.[Reprojection.UserDefinedFunctions].GeographyToGeometry
GO

CREATE FUNCTION dbo.GeometryToGeography(@geom geometry, @srid int) RETURNS geography
EXTERNAL NAME Reprojection.[Reprojection.UserDefinedFunctions].GeometryToGeography
GO

CREATE FUNCTION dbo.GeographyToGeography(@geog geography, @srid int) RETURNS geography
EXTERNAL NAME Reprojection.[Reprojection.UserDefinedFunctions].GeographyToGeography
GO
