

ALTER DATABASE [CorinaTest] SET TRUSTWORTHY ON

CREATE ASSEMBLY Reprojection
FROM 'C:\Reprojection\Reprojection.dll'
WITH PERMISSION_SET = UNSAFE
GO


-----
DECLARE @Hash BINARY(64),
        @ClrName NVARCHAR(4000),
        @AssemblySize INT,
        @MvID UNIQUEIDENTIFIER;

SELECT  @Hash = HASHBYTES(N'SHA2_512', af.[content]),
        @ClrName = CONVERT(NVARCHAR(4000), ASSEMBLYPROPERTY(af.[name],
                N'CLRName')),
        @AssemblySize = DATALENGTH(af.[content]),
        @MvID = CONVERT(UNIQUEIDENTIFIER, ASSEMBLYPROPERTY(af.[name], N'MvID'))
FROM    sys.assembly_files af
WHERE   af.[name] = N'C:\Reprojection\Reprojection.dll'
AND     af.[file_id] = 1;

--SELECT  @ClrName, @AssemblySize, @MvID, @Hash;

EXEC sys.sp_add_trusted_assembly @Hash, @ClrName;

GO

-----
DECLARE @Hash BINARY(64),
        @ClrName NVARCHAR(4000),
        @AssemblySize INT,
        @MvID UNIQUEIDENTIFIER;

SELECT  @Hash = HASHBYTES(N'SHA2_512', af.[content]),
        @ClrName = CONVERT(NVARCHAR(4000), ASSEMBLYPROPERTY(af.[name],
                N'CLRName')),
        @AssemblySize = DATALENGTH(af.[content]),
        @MvID = CONVERT(UNIQUEIDENTIFIER, ASSEMBLYPROPERTY(af.[name], N'MvID'))
FROM    sys.assembly_files af
WHERE   af.[name] = N'C:\Reprojection\projnet.dll'
AND     af.[file_id] = 1;

--SELECT  @ClrName, @AssemblySize, @MvID, @Hash;

EXEC sys.sp_add_trusted_assembly @Hash, @ClrName;

GO

------
DECLARE @Hash BINARY(64),
        @ClrName NVARCHAR(4000),
        @AssemblySize INT,
        @MvID UNIQUEIDENTIFIER;

SELECT  @Hash = HASHBYTES(N'SHA2_512', af.[content]),
        @ClrName = CONVERT(NVARCHAR(4000), ASSEMBLYPROPERTY(af.[name],
                N'CLRName')),
        @AssemblySize = DATALENGTH(af.[content]),
        @MvID = CONVERT(UNIQUEIDENTIFIER, ASSEMBLYPROPERTY(af.[name], N'MvID'))
FROM    sys.assembly_files af
WHERE   af.[name] = N'C:\Reprojection\geoapi.dll'
AND     af.[file_id] = 1;

--SELECT  @ClrName, @AssemblySize, @MvID, @Hash;

--EXEC sys.sp_drop_trusted_assembly  @Hash
EXEC sys.sp_add_trusted_assembly @Hash, @ClrName;
GO

-----
DECLARE @Hash BINARY(64),
        @ClrName NVARCHAR(4000),
        @AssemblySize INT,
        @MvID UNIQUEIDENTIFIER;

SELECT  @Hash = HASHBYTES(N'SHA2_512', af.[content]),
        @ClrName = CONVERT(NVARCHAR(4000), ASSEMBLYPROPERTY(af.[name],
                N'CLRName')),
        @AssemblySize = DATALENGTH(af.[content]),
        @MvID = CONVERT(UNIQUEIDENTIFIER, ASSEMBLYPROPERTY(af.[name], N'MvID'))
FROM    sys.assembly_files af
WHERE   af.[name] = N'C:\Reprojection\geoapi.coordinatesystems.dll'
AND     af.[file_id] = 1;

--SELECT  @ClrName, @AssemblySize, @MvID, @Hash;

EXEC sys.sp_add_trusted_assembly @Hash, @ClrName;
GO

---


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

---
ALTER DATABASE [CorinaTest] SET TRUSTWORTHY off

--- TESTARE

DECLARE @Norwich geometry = geometry::STPointFromText('POINT(626000 354000)', 27700);
SELECT dbo.GeometryToGeography(@Norwich, 4326).ToString();
--53.035498 1.369338 (WGS84)

DECLARE @Norwich geometry = geometry::STPointFromText('POINT(626000 354000)', 32631);
SELECT dbo.GeometryToGeometry(@Norwich, 4326).ToString();
--390659,5877462 (UTM 31U)

DECLARE @Norwich geography = geography::STPointFromText('POINT(1.369338 53.035498)', 4326);
SELECT dbo.GeographyToGeometry(@Norwich, 32631).ToString();