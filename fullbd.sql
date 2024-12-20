USE [DungeonManager]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[idCart] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[idCharacter] [int] NOT NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idCart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Characters]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Characters](
	[idCharacter] [int] IDENTITY(1,1) NOT NULL,
	[CharacterName] [nvarchar](100) NOT NULL,
	[idClass] [int] NULL,
	[idPerks] [int] NULL,
	[idSkills] [int] NULL,
	[idStats] [int] NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[ImageURL] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Characte__696BF5829B0E9F40] PRIMARY KEY CLUSTERED 
(
	[idCharacter] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[idClass] [int] IDENTITY(1,1) NOT NULL,
	[NameClass] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK__Classes__17317A5AB252791A] PRIMARY KEY CLUSTERED 
(
	[idClass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[idOrderItem] [int] IDENTITY(1,1) NOT NULL,
	[idOrder] [int] NOT NULL,
	[idCharacter] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idOrderItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[idOrder] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[OrderDate] [datetime] NULL,
	[idStatus] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[idStatus] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Perks]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perks](
	[idPerk] [int] IDENTITY(1,1) NOT NULL,
	[NamePerk] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](150) NULL,
 CONSTRAINT [PK__Perks__B8780FDF98DD6723] PRIMARY KEY CLUSTERED 
(
	[idPerk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[idRole] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Roles__E5045C54DB472BF5] PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skills]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skills](
	[idSkill] [int] IDENTITY(1,1) NOT NULL,
	[NameSkill] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](150) NULL,
 CONSTRAINT [PK__Skills__C4CE4D6EB865C255] PRIMARY KEY CLUSTERED 
(
	[idSkill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stats]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stats](
	[idStat] [int] IDENTITY(1,1) NOT NULL,
	[Health] [int] NULL,
	[Mana] [int] NULL,
	[Strength] [int] NULL,
	[Agility] [int] NULL,
	[Intelligence] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idStat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19.12.2024 17:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[idUser] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[idRole] [int] NULL,
 CONSTRAINT [PK__Users__3717C982EE17041C] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([idCart], [idUser], [idCharacter], [Quantity]) VALUES (15, 2, 1, 1)
INSERT [dbo].[Cart] ([idCart], [idUser], [idCharacter], [Quantity]) VALUES (16, 2, 2, 1)
INSERT [dbo].[Cart] ([idCart], [idUser], [idCharacter], [Quantity]) VALUES (17, 2, 9, 1)
INSERT [dbo].[Cart] ([idCart], [idUser], [idCharacter], [Quantity]) VALUES (18, 2, 8, 1)
INSERT [dbo].[Cart] ([idCart], [idUser], [idCharacter], [Quantity]) VALUES (19, 2, 7, 1)
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[Characters] ON 

INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (1, N'Dain the Defender', 1, 1, 1, 1, CAST(100.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (2, N'Rogar the Ravager', 2, 2, 2, 2, CAST(120.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (3, N'Shade the Silent', 3, 3, 3, 3, CAST(110.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (4, N'Eldon the Archer', 4, 4, 4, 4, CAST(105.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (5, N'Theron the Pyromancer', 5, 5, 5, 5, CAST(130.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (6, N'Borin the Healer', 6, 6, 6, 6, CAST(115.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (7, N'Garos the Warden', 1, 1, 1, 1, CAST(150.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (8, N'Karn the Berserker', 2, 2, 2, 2, CAST(170.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (9, N'Lyria the Shadow', 3, 3, 3, 3, CAST(160.00 AS Decimal(10, 2)), N'\Images\default.jpg')
INSERT [dbo].[Characters] ([idCharacter], [CharacterName], [idClass], [idPerks], [idSkills], [idStats], [Price], [ImageURL]) VALUES (1011, N'AdminTest1', 1, 1, 1, 1, CAST(333.00 AS Decimal(10, 2)), N'\Images\default.jpg')
SET IDENTITY_INSERT [dbo].[Characters] OFF
GO
SET IDENTITY_INSERT [dbo].[Classes] ON 

INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (1, N'Fighter', N'Сбалансированный воин, идеально подходит для ближнего боя.')
INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (2, N'Barbarian', N'Сильный и разрушительный, но уязвимый в защите.')
INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (3, N'Rogue', N'Мастер скрытности и быстрых атак.')
INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (4, N'Ranger', N'Дальнобойный боец, использующий ловушки и лук.')
INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (5, N'Wizard', N'Маг с мощными заклинаниями разрушения.')
INSERT [dbo].[Classes] ([idClass], [NameClass], [Description]) VALUES (6, N'Cleric', N'Целитель, способный поддерживать союзников.')
SET IDENTITY_INSERT [dbo].[Classes] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON 

INSERT [dbo].[OrderItems] ([idOrderItem], [idOrder], [idCharacter], [Quantity], [Price]) VALUES (1, 1, 1, 2, CAST(100.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderItems] ([idOrderItem], [idOrder], [idCharacter], [Quantity], [Price]) VALUES (1004, 1003, 4, 1, CAST(105.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderItems] ([idOrderItem], [idOrder], [idCharacter], [Quantity], [Price]) VALUES (1006, 1005, 9, 2, CAST(160.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([idOrder], [idUser], [OrderDate], [idStatus]) VALUES (1, 2, CAST(N'2024-12-17T19:11:16.627' AS DateTime), 2)
INSERT [dbo].[Orders] ([idOrder], [idUser], [OrderDate], [idStatus]) VALUES (1003, 1, CAST(N'2024-12-19T10:45:18.800' AS DateTime), 2)
INSERT [dbo].[Orders] ([idOrder], [idUser], [OrderDate], [idStatus]) VALUES (1005, 1, CAST(N'2024-12-19T10:47:28.477' AS DateTime), 2)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderStatus] ON 

INSERT [dbo].[OrderStatus] ([idStatus], [StatusName]) VALUES (1, N'В ожидании')
INSERT [dbo].[OrderStatus] ([idStatus], [StatusName]) VALUES (2, N'Подтверждено')
INSERT [dbo].[OrderStatus] ([idStatus], [StatusName]) VALUES (3, N'Отклонено')
SET IDENTITY_INSERT [dbo].[OrderStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Perks] ON 

INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (1, N'Weapon Mastery', N'Позволяет использовать широкий спектр оружия.')
INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (2, N'Rage', N'Увеличивает урон, снижая защиту.')
INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (3, N'Stealth', N'Позволяет оставаться незаметным.')
INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (4, N'Tracking', N'Умение обнаруживать врагов и ловушки.')
INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (5, N'Arcane Knowledge', N'Усиливает магические способности.')
INSERT [dbo].[Perks] ([idPerk], [NamePerk], [Description]) VALUES (6, N'Divine Aura', N'Улучшает способности лечения.')
SET IDENTITY_INSERT [dbo].[Perks] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([idRole], [RoleName]) VALUES (1, N'Пользователь')
INSERT [dbo].[Roles] ([idRole], [RoleName]) VALUES (2, N'Менеджер')
INSERT [dbo].[Roles] ([idRole], [RoleName]) VALUES (3, N'Администратор')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Skills] ON 

INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (1, N'Second Wind', N'Восстанавливает часть здоровья.')
INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (2, N'Savage Roar', N'Пугает врагов, снижая их точность.')
INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (3, N'Pickpocket', N'Позволяет красть предметы у врагов.')
INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (4, N'Quick Shot', N'Быстрая стрельба из лука.')
INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (5, N'Fireball', N'Наносит урон огненным заклинанием.')
INSERT [dbo].[Skills] ([idSkill], [NameSkill], [Description]) VALUES (6, N'Heal', N'Восстанавливает здоровье союзников.')
SET IDENTITY_INSERT [dbo].[Skills] OFF
GO
SET IDENTITY_INSERT [dbo].[Stats] ON 

INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (1, 100, 20, 15, 10, 5)
INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (2, 120, 10, 20, 8, 3)
INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (3, 80, 15, 10, 18, 5)
INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (4, 90, 15, 12, 15, 8)
INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (5, 70, 50, 5, 12, 20)
INSERT [dbo].[Stats] ([idStat], [Health], [Mana], [Strength], [Agility], [Intelligence]) VALUES (6, 100, 30, 10, 10, 15)
SET IDENTITY_INSERT [dbo].[Stats] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([idUser], [Login], [Password], [Email], [idRole]) VALUES (1, N'Admin', N'admin', N'admin@gmail.com', 3)
INSERT [dbo].[Users] ([idUser], [Login], [Password], [Email], [idRole]) VALUES (2, N'Vladusha', N'vladusha123', N'vladgutsulyak216@gmail.com', 1)
INSERT [dbo].[Users] ([idUser], [Login], [Password], [Email], [idRole]) VALUES (1002, N'Test', N'Test123', N'test@gmail.com', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF__Character__Price__656C112C]  DEFAULT ((0.00)) FOR [Price]
GO
ALTER TABLE [dbo].[Characters] ADD  CONSTRAINT [DF__Character__Image__66603565]  DEFAULT ('') FOR [ImageURL]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((1)) FOR [idStatus]
GO
ALTER TABLE [dbo].[Stats] ADD  DEFAULT ((100)) FOR [Health]
GO
ALTER TABLE [dbo].[Stats] ADD  DEFAULT ((50)) FOR [Mana]
GO
ALTER TABLE [dbo].[Stats] ADD  DEFAULT ((10)) FOR [Strength]
GO
ALTER TABLE [dbo].[Stats] ADD  DEFAULT ((10)) FOR [Agility]
GO
ALTER TABLE [dbo].[Stats] ADD  DEFAULT ((10)) FOR [Intelligence]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Character] FOREIGN KEY([idCharacter])
REFERENCES [dbo].[Characters] ([idCharacter])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Character]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_User] FOREIGN KEY([idUser])
REFERENCES [dbo].[Users] ([idUser])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_User]
GO
ALTER TABLE [dbo].[Characters]  WITH CHECK ADD  CONSTRAINT [FK__Character__idCla__48CFD27E] FOREIGN KEY([idClass])
REFERENCES [dbo].[Classes] ([idClass])
GO
ALTER TABLE [dbo].[Characters] CHECK CONSTRAINT [FK__Character__idCla__48CFD27E]
GO
ALTER TABLE [dbo].[Characters]  WITH CHECK ADD  CONSTRAINT [FK__Character__idPer__49C3F6B7] FOREIGN KEY([idPerks])
REFERENCES [dbo].[Perks] ([idPerk])
GO
ALTER TABLE [dbo].[Characters] CHECK CONSTRAINT [FK__Character__idPer__49C3F6B7]
GO
ALTER TABLE [dbo].[Characters]  WITH CHECK ADD  CONSTRAINT [FK__Character__idSki__4AB81AF0] FOREIGN KEY([idSkills])
REFERENCES [dbo].[Skills] ([idSkill])
GO
ALTER TABLE [dbo].[Characters] CHECK CONSTRAINT [FK__Character__idSki__4AB81AF0]
GO
ALTER TABLE [dbo].[Characters]  WITH CHECK ADD  CONSTRAINT [FK__Character__idSta__4BAC3F29] FOREIGN KEY([idStats])
REFERENCES [dbo].[Stats] ([idStat])
GO
ALTER TABLE [dbo].[Characters] CHECK CONSTRAINT [FK__Character__idSta__4BAC3F29]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_Orderitems_Character] FOREIGN KEY([idCharacter])
REFERENCES [dbo].[Characters] ([idCharacter])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_Orderitems_Character]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Order] FOREIGN KEY([idOrder])
REFERENCES [dbo].[Orders] ([idOrder])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Order]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Status] FOREIGN KEY([idStatus])
REFERENCES [dbo].[OrderStatus] ([idStatus])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Status]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_User] FOREIGN KEY([idUser])
REFERENCES [dbo].[Users] ([idUser])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__idRole__398D8EEE] FOREIGN KEY([idRole])
REFERENCES [dbo].[Roles] ([idRole])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__idRole__398D8EEE]
GO
