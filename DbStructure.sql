USE [db_a9107a_pharma263]
GO

CREATE TABLE [Pharma263].[AccountsPayable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AmountOwed] [decimal](14, 4) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[AmountPaid] [decimal](14, 4) NOT NULL,
	[BalanceOwed] [decimal](14, 4) NOT NULL,
	[AccountsPayableStatusId] [int] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_AccountsPayable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AccountsPayableStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_AccountsPayableStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AccountsReceivable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AmountDue] [decimal](14, 4) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[AmountPaid] [decimal](14, 4) NOT NULL,
	[BalanceDue] [decimal](14, 4) NOT NULL,
	[AccountsReceivableStatusId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_AccountsReceivable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AccountsReceivableStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_AccountsReceivableStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [varchar](255) NOT NULL,
	[ClaimType] [varchar](255) NULL,
	[ClaimValue] [varchar](255) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetRoles](
	[Id] [varchar](255) NOT NULL,
	[Name] [varchar](256) NULL,
	[NormalizedName] [varchar](256) NULL,
	[ConcurrencyStamp] [varchar](255) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](255) NOT NULL,
	[ClaimType] [varchar](255) NULL,
	[ClaimValue] [varchar](255) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetUserLogins](
	[LoginProvider] [varchar](255) NOT NULL,
	[ProviderKey] [varchar](255) NOT NULL,
	[ProviderDisplayName] [varchar](255) NULL,
	[UserId] [varchar](255) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetUserRoles](
	[UserId] [varchar](255) NOT NULL,
	[RoleId] [varchar](255) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetUsers](
	[Id] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[LastName] [varchar](255) NULL,
	[UserName] [varchar](256) NULL,
	[NormalizedUserName] [varchar](256) NULL,
	[Email] [varchar](256) NULL,
	[NormalizedEmail] [varchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [varchar](255) NULL,
	[SecurityStamp] [varchar](255) NULL,
	[ConcurrencyStamp] [varchar](255) NULL,
	[PhoneNumber] [varchar](255) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AspNetUserTokens](
	[UserId] [varchar](255) NOT NULL,
	[LoginProvider] [varchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Value] [varchar](255) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[AuditEntry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityName] [varchar](255) NULL,
	[ActionType] [varchar](255) NULL,
	[Username] [varchar](255) NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL,
	[EntityId] [varchar](255) NULL,
	[Changes] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditEntry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Phone] [varchar](20) NOT NULL,
	[PhysicalAddress] [varchar](200) NOT NULL,
	[DeliveryAddress] [varchar](200) NULL,
	[MCAZLicence] [varchar](50) NULL,
	[HPALicense] [varchar](50) NULL,
	[VATNumber] [varchar](50) NULL,
	[CustomerTypeId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[CustomerType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Medicine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[GenericName] [varchar](50) NULL,
	[Brand] [varchar](50) NULL,
	[Manufacturer] [varchar](50) NULL,
	[DosageForm] [varchar](50) NULL,
	[PackSize] [varchar](50) NULL,
	[QuantityPerUnit] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Medicine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[PaymentMade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AmountPaid] [decimal](14, 4) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[AccountPayableId] [int] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[SaleId] [int] NULL,
 CONSTRAINT [PK_PaymentMade] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[PaymentMethod](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[PaymentReceived](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AmountReceived] [decimal](14, 4) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[AccountsReceivableId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[PurchaseId] [int] NULL,
 CONSTRAINT [PK_PaymentReceived] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Purchase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseDate] [datetime2](7) NOT NULL,
	[Notes] [varchar](200) NULL,
	[Total] [decimal](14, 2) NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
	[GrandTotal] [decimal](14, 2) NOT NULL,
	[PaymentDueDate] [datetime2](7) NULL,
	[PaymentMethodId] [int] NOT NULL,
	[PurchaseStatusId] [int] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[PurchaseItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](14, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [decimal](14, 2) NOT NULL,
	[BatchNo] [varchar](50) NOT NULL,
	[MedicineId] [int] NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
 CONSTRAINT [PK_PurchaseItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[PurchaseStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_PurchaseStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Quarantine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalQuantity] [int] NOT NULL,
	[BatchNo] [varchar](255) NULL,
	[ExpiryDate] [datetimeoffset](7) NOT NULL,
	[MedicineId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Quarantine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Quotation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuotationDate] [datetime2](7) NOT NULL,
	[Notes] [varchar](200) NULL,
	[Total] [decimal](14, 2) NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
	[GrandTotal] [decimal](14, 2) NOT NULL,
	[QuoteExpiryDate] [datetimeoffset](7) NULL,
	[QuoteStatusId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Quotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[QuotationItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](14, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [decimal](14, 2) NOT NULL,
	[StockId] [int] NOT NULL,
	[QuotationId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
 CONSTRAINT [PK_QuotationItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[QuoteStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NULL,
	[Description] [varchar](255) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [varbinary](max) NULL,
 CONSTRAINT [PK_QuoteStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[ReturnDestination](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReturnDestination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[ReturnReason](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReturnReason] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Returns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[DateReturned] [datetime2](7) NOT NULL,
	[Notes] [varchar](255) NULL,
	[ReturnDestinationId] [int] NOT NULL,
	[ReturnReasonId] [int] NOT NULL,
	[ReturnStatusId] [int] NOT NULL,
	[StockId] [int] NOT NULL,
	[SaleId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Returns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[ReturnStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReturnStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Sales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalesDate] [datetime2](7) NOT NULL,
	[Notes] [varchar](200) NULL,
	[Total] [decimal](14, 2) NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
	[GrandTotal] [decimal](14, 2) NOT NULL,
	[PaymentDueDate] [datetime2](7) NULL,
	[PaymentMethodId] [int] NOT NULL,
	[SaleStatusId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[SalesItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](14, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [decimal](14, 2) NOT NULL,
	[StockId] [int] NOT NULL,
	[SaleId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[Discount] [decimal](14, 2) NOT NULL,
 CONSTRAINT [PK_SalesItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[SaleStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_SaleStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalQuantity] [int] NOT NULL,
	[NotifyForQuantityBelow] [int] NOT NULL,
	[BatchNo] [varchar](50) NOT NULL,
	[ExpiryDate] [datetimeoffset](7) NOT NULL,
	[BuyingPrice] [decimal](14, 2) NOT NULL,
	[SellingPrice] [decimal](14, 2) NOT NULL,
	[MedicineId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[StoreSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Logo] [varchar](255) NULL,
	[StoreName] [varchar](150) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Phone] [varchar](25) NOT NULL,
	[Currency] [varchar](20) NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[MCAZLicence] [varchar](100) NULL,
	[VATNumber] [varchar](100) NULL,
	[BankingDetails] [varchar](150) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[ReturnsPolicy] [varchar](500) NULL,
 CONSTRAINT [PK_StoreSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Pharma263].[Supplier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Phone] [varchar](20) NOT NULL,
	[Address] [varchar](200) NOT NULL,
	[Notes] [varchar](200) NULL,
	[MCAZLicence] [varchar](50) NULL,
	[BusinessPartnerNumber] [varchar](50) NULL,
	[VATNumber] [varchar](50) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedDate] [datetimeoffset](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
