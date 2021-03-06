﻿CREATE TABLE [dbo].[SupplierDetails] (
    [ID]                                   INT            NOT NULL,
    [PublisherPhone]                       NVARCHAR (100) NULL,
    [PublisherMobilePhone]                 NVARCHAR (100) NULL,
    [PublisherFax]                         NVARCHAR (50)  NULL,
    [PublisherEmail]                       NVARCHAR (100) NULL,
    [PublisherUrl]                         NVARCHAR (511) NULL,
    [PublisherAddress]                     NVARCHAR (256) NULL,
    [PublisherZipCode]                     NVARCHAR (50)  NULL,
    [PublisherCityID]                      INT            NULL,
    [PublisherPrefectureID]                INT            NULL,
    [LegalPersonName]                      NVARCHAR (256) NULL,
    [LegalPersonPhone]                     NVARCHAR (50)  NULL,
    [LegalPersonEmail]                     NVARCHAR (100) NULL,
    [LegalPersonIdentificationType]        INT            NULL,
    [LegalPersonIdentificationNumber]      NVARCHAR (100) NULL,
    [LegalPersonIdentificationIssuer]      NVARCHAR (256) NULL,
    [LegalPersonIdentificationIssueDate]   DATETIME2  NULL,
    [IsSelfRepresented]                    BIT            NULL,
    [SelfPublisherIdentificationType]      INT            NULL,
    [SelfPublisherIdentificationNumber]    NVARCHAR (100) NULL,
    [SelfPublisherIdentificationIssuer]    NVARCHAR (100) NULL,
    [SelfPublisherIdentificationIssueDate] DATETIME2  NULL,
    [ContactIdentificationType]            INT            NULL,
    [ContactIdentificationNumber]          NVARCHAR (100) NULL,
    [ContactIdentificationIssuer]          NVARCHAR (256) NULL,
    [ContactIdentificationIssueDate]       DATETIME2  NULL,
    [AlternateContactName]                 NVARCHAR (256) NULL,
    [AlternateContactPhone]                NVARCHAR (50)  NULL,
    [AlternateContactMobilePhone]          NVARCHAR (50)  NULL,
    [AlternateContactEmail]                NVARCHAR (100) NULL,
    CONSTRAINT [PK_SupplierDetails] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierDetails_Kap_Cities] FOREIGN KEY ([PublisherCityID]) REFERENCES [dbo].[Kap_Cities] ([ID]),
    CONSTRAINT [FK_SupplierDetails_Kap_Prefectures] FOREIGN KEY ([PublisherPrefectureID]) REFERENCES [dbo].[Kap_Prefectures] ([ID])
);

