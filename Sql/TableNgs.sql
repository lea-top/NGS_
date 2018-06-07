use NGSData
go

create table NumberingRuns
(
 IdNumberingRuns int IDENTITY(1,1) PRIMARY KEY ,
 IdCantrige  varchar (40),
 DateSaved DATETIME NOT NULL DEFAULT (GETDATE()),
)

create table Runs
(
 Id int IDENTITY(1,1),
 IdCantrige  varchar (40),
 IdRun varchar(10) ,
 IdPlate1  varchar(40) ,
 IdPlate2  varchar(40) ,
 IdPlate3   varchar(40),
 IdPlate4   varchar(40),  
 DateSaved DATETIME NOT NULL DEFAULT (GETDATE()) ,
  PRIMARY KEY(IdCantrige)                
)

 create table PathFiles
(
 Id int IDENTITY(1,1) ,
 NamePath  varchar(250) PRIMARY KEY,            
)
create table Level0
(
 IdRun varchar(250),
 IdLevel0 int IDENTITY(1,1),
 IdPlate varchar (40),
 [NumPlate]  varchar(4),
 [SampleName]  varchar(30),
 [Pos]   varchar(4),
 Id    varchar(250),                 
 [Or260280]   varchar(14),                
 [Comments]  varchar(250),
 [Type1] varchar(6),
 [IdPos] varchar(250),
 [NgUl] varchar(16),
 [Vol]  varchar(6),
 DateSaved DATETIME NOT NULL DEFAULT (GETDATE()),
 PRIMARY KEY([SampleName],IdPlate) 
)

create table Mutations
(
 IdMutation int IDENTITY(1,1),
 NumFile varchar (6),
 Chrom  varchar(40),
 Start1  varchar(50),
 End1   varchar(50),
 Id    varchar(250),                 
 Ref   varchar(30),                
 Alt  varchar(40),
 DyDis varchar(100),
 DyMut varchar(100),
 MutID varchar(100),
 GenotypeChrom  varchar(30),
 GenotypePos  varchar(250),
 GenotypeId  varchar(250),
 GenotypeRef  varchar(40),
 GenotypeAlt   varchar(250) , 
 ColorDyName varchar(50) , 
  DateSaved DATETIME NOT NULL DEFAULT (GETDATE()),
 PRIMARY KEY(Start1,Chrom,End1,DyDis,DyMut,MutID,GenotypeRef,GenotypeAlt,NumFile) 
)

drop table MovedLevel

create table ListPerson
(
 IdRuns  varchar(250) ,
 NumFile varchar (4),
 IdMutation  varchar(250) ,
 NameP  varchar(30) ,
 Genotype   varchar(250),
 AlleleCoverage    varchar(250),                 
 TotalCoverage   varchar(250),                
 GenotypeColor  varchar(30),
 AlleleCoverageColor varchar(30),
 TotalCoverageColor  varchar(30),
 DateSaved DATETIME NOT NULL DEFAULT (GETDATE()),
 primary key(IdMutation,NameP,IdRuns,NumFile) 
 )
 create table ListPersonFinal
(
 IdRuns  varchar(250) ,
 IdMutation  varchar(250) ,
 NameP  varchar(30) ,
 Result   varchar(6),              
 ResultColor  varchar(30),
 DateSaved DATETIME NOT NULL DEFAULT (GETDATE()),
 primary key(IdMutation,NameP,IdRuns) 
 )
go
delete from [dbo].[ListPerson]
delete from [dbo].[Mutations]
delete from [dbo].[PathFiles]