USE YourDatabaseName
GO

/* Create table for admin user. */
CREATE TABLE a_admin (
Firstname Varchar(30) Not Null,
Lastname Varchar(30),
Email Varchar(100) Primary Key,
PasswordHash Varchar(200),
Salt Varchar(200)
);

/* Create "enum" table to store news entry statuses. */
CREATE TABLE a_status (
Status Varchar(10) Primary Key
);

/* Create table for news entries. */
CREATE TABLE a_news (
EntryID Int identity(1,1) Primary Key,
Title Varchar(100) Not Null,
Created DateTime,
Entry Varchar(4000) Not Null,
Status Varchar(10) Not Null,
	Constraint fk_status Foreign Key (Status) References a_status(Status)
	On Delete Cascade,
UserEmail Varchar(100) Not Null,
	Constraint fk_admin Foreign Key (UserEmail) References a_admin(Email)
    On Delete Cascade
);

/* Insert status values for news. */
INSERT INTO a_status VALUES('Published');
INSERT INTO a_status VALUES('Draft');