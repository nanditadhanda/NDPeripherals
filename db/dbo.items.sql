CREATE TABLE [dbo].[items] (
    [itemID]            INT       NOT NULL IDENTITY ,
    [itemName]          NVARCHAR (50)  NOT NULL,
    [itemCat]           NVARCHAR (50)  NULL,
    [itemOrdered]       INT            NULL,
    [itemStock]         INT            NULL,
    [itemSold]          INT            DEFAULT ((0)) NULL,
    [itemSupplierPrice] DECIMAL (6, 2) NULL,
    [itemSalePrice]     DECIMAL (6, 2) NULL,
    [itemDesc]          NTEXT          NULL,
    PRIMARY KEY CLUSTERED ([itemID] ASC),
    CONSTRAINT [chkCategory] CHECK ([itemCat]='input' OR [itemCat]='output' OR [itemCat]='storage')
);

