-- Create the database

USE master
CREATE DATABASE SharpPaf
GO

USE SharpPaf
GO

-- Create the tables

CREATE TABLE BuildingNames
(
	Id INTEGER NOT NULL,
	Name VARCHAR(50) NOT NULL,

	CONSTRAINT pk_BuildingNamesId PRIMARY KEY(Id)
)

CREATE TABLE Localities
(
	Id INTEGER NOT NULL,
	DependentLocality VARCHAR(35) NULL,
	DoubleDependentLocality VARCHAR(35) NULL,
	PostTown VARCHAR(30) NOT NULL,

	CONSTRAINT pk_LocalitiesId PRIMARY KEY(Id)
)

CREATE TABLE Organisations
(
	Id INTEGER NOT NULL,
	Name VARCHAR(60) NOT NULL,
	Department VARCHAR(60) NULL,
	PostcodeType TINYINT NOT NULL,

	CONSTRAINT pk_OrganisationsId PRIMARY KEY(Id)
)

CREATE TABLE SubBuildingNames
(
	Id INTEGER NOT NULL,
	Name VARCHAR(30) NOT NULL,

	CONSTRAINT pk_SubBuildingNamesId PRIMARY KEY(Id)
)

CREATE TABLE ThoroughfareDescriptors
(
	Id INTEGER NOT NULL,
	Descriptor VARCHAR(20) NOT NULL,

	CONSTRAINT pk_ThoroughfareDescriptorsId PRIMARY KEY(Id)
)

CREATE TABLE Thoroughfares
(
	Id INTEGER NOT NULL,
	Name VARCHAR(60) NOT NULL,

	CONSTRAINT pk_ThoroughfaresId PRIMARY KEY(Id)
)
GO

-- Create this table after the others as it has foreign key constraints on them
CREATE TABLE Addresses
(
	Id INTEGER NOT NULL,
	LocalityId INTEGER NOT NULL,
	BuildingNameId INTEGER NULL,
	DependentThoroughfareId INTEGER NULL,
	DependentThoroughfareDescriptorId INTEGER NULL,
	OrganisationId INTEGER NULL,
	SubBuildingNameId INTEGER NULL,
	ThoroughfareId INTEGER NULL,
	ThoroughfareDescriptorId INTEGER NULL,
	BuildingNumber SMALLINT NULL,
	POBoxNumber CHAR(6) NULL,
	Postcode CHAR(8) NOT NULL,
	IsBuildingNumberConcatenated BIT NOT NULL,
	IsSmallUserOrganisation BIT NOT NULL,
	DeliveryPointSuffix CHAR(2) NOT NULL,
	NumberOfHouseholds SMALLINT NOT NULL,
	PostcodeType TINYINT NOT NULL,

	CONSTRAINT pk_AddressesId PRIMARY KEY(Id),

	CONSTRAINT fk_AddressesLocalityId FOREIGN KEY (LocalityId)
	REFERENCES Localities(Id),

	CONSTRAINT fk_AddressesBuildingNameId FOREIGN KEY (BuildingNameId)
	REFERENCES BuildingNames(Id),

	CONSTRAINT fk_AddressesDependentThoroughfareDescriptorId FOREIGN KEY (DependentThoroughfareDescriptorId)
	REFERENCES ThoroughfareDescriptors(Id),

	CONSTRAINT fk_AddressesDependentThoroughfareId FOREIGN KEY (DependentThoroughfareId)
	REFERENCES Thoroughfares(Id),

	CONSTRAINT fk_AddressesOrganisationId FOREIGN KEY (OrganisationId)
	REFERENCES Organisations(Id),

	CONSTRAINT fk_AddressesSubBuildingNameId FOREIGN KEY (SubBuildingNameId)
	REFERENCES SubBuildingNames(Id),

	CONSTRAINT fk_AddressesThoroughfareDescriptorId FOREIGN KEY (ThoroughfareDescriptorId)
	REFERENCES ThoroughfareDescriptors(Id),

	CONSTRAINT fk_AddressesThoroughfareId FOREIGN KEY (ThoroughfareId)
	REFERENCES Thoroughfares(Id)
)
GO


-- Create helper views to link the data together

CREATE VIEW FullAddresses AS
SELECT
	Addresses.Id AS AddressId,
	Organisations.Name AS OrganisationName,
	Department, 
	SubBuildingNames.Name AS SubBuildingName,
	BuildingNames.Name AS BuildingName,
	BuildingNumber, 
	DependentThoroughfares.Name AS DependentThoroughfareName,
	DependentThoroughfareDescriptors.Descriptor AS DependentThoroughfareDescriptor,
	Thoroughfares.Name AS ThoroughfareName,
	ThoroughfareDescriptors.Descriptor AS ThoroughfareDescriptor,
	POBoxNumber,
	DoubleDependentLocality,
	DependentLocality,
	PostTown,
	Postcode,
	IsBuildingNumberConcatenated
FROM
	Addresses
	INNER JOIN Localities ON LocalityId = Localities.Id
	LEFT OUTER JOIN Organisations ON OrganisationId = Organisations.Id
	LEFT OUTER JOIN BuildingNames ON BuildingNameId = BuildingNames.Id
	LEFT OUTER JOIN SubBuildingNames ON SubBuildingNameId = SubBuildingNames.Id
	LEFT OUTER JOIN Thoroughfares AS DependentThoroughfares ON DependentThoroughfareId = DependentThoroughfares.Id
	LEFT OUTER JOIN ThoroughfareDescriptors AS DependentThoroughfareDescriptors ON DependentThoroughfareDescriptorId = DependentThoroughfareDescriptors.Id
	LEFT OUTER JOIN Thoroughfares ON ThoroughfareId = Thoroughfares.Id
	LEFT OUTER JOIN ThoroughfareDescriptors ON ThoroughfareDescriptorId = ThoroughfareDescriptors.Id
GO
