namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comboes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TransactionTypeId = c.Int(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        FeeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.ChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Fees", t => t.FeeId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.ChannelId)
                .Index(t => t.FeeId);
            
            CreateTable(
                "dbo.Fees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FlatAmount = c.String(),
                        PercentOfTransaction = c.String(),
                        Maximum = c.String(),
                        Minimum = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SinkNodeId = c.Int(nullable: false),
                        CardPAN = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SinkNodes", t => t.SinkNodeId, cascadeDelete: true)
                .Index(t => t.SinkNodeId);
            
            CreateTable(
                "dbo.SinkNodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        HostName = c.String(),
                        IPAddress = c.String(),
                        Port = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        NodeType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RouteId = c.Int(nullable: false),
                        ComboId = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comboes", t => t.ComboId, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.ComboId);
            
            CreateTable(
                "dbo.SourceNodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SchemeId = c.Int(nullable: false),
                        Name = c.String(),
                        HostName = c.String(),
                        IPAddress = c.String(),
                        Port = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        NodeType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schemes", t => t.SchemeId, cascadeDelete: true)
                .Index(t => t.SchemeId);

            //    CreateTable(
            //        "dbo.Transactions",
            //        c => new
            //            {
            //                Id = c.Int(nullable: false, identity: true),
            //                CardPAN = c.String(),
            //                Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //                TransactionFee = c.Decimal(nullable: false, precision: 18, scale: 2),
            //                ProcessingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
            //                Date = c.DateTime(nullable: false),
            //                MTI = c.String(),
            //                STAN = c.String(),
            //                ChannelCode = c.String(),
            //                OriginalDataElement = c.String(),
            //                TransactionTypeCode = c.String(),
            //                Account1 = c.String(),
            //                Account2 = c.String(),
            //                FeeCode = c.String(),
            //                ResponseCode = c.String(),
            //                ResponseDescription = c.String(),
            //                IsReversePending = c.Boolean(nullable: false),
            //                IsReversed = c.Boolean(nullable: false),
            //                SourceNodeId = c.Int(nullable: false),
            //                DateCreated = c.DateTime(nullable: false),
            //                DateModified = c.DateTime(nullable: false),
            //            })
            //        .PrimaryKey(t => t.Id)
            //        .ForeignKey("dbo.SourceNodes", t => t.SourceNodeId, cascadeDelete: true)
            //        .Index(t => t.SourceNodeId);

        }

        public override void Down()
        {
            //DropForeignKey("dbo.Transactions", "SourceNodeId", "dbo.SourceNodes");
            DropForeignKey("dbo.SourceNodes", "SchemeId", "dbo.Schemes");
            DropForeignKey("dbo.Schemes", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.Schemes", "ComboId", "dbo.Comboes");
            DropForeignKey("dbo.Routes", "SinkNodeId", "dbo.SinkNodes");
            DropForeignKey("dbo.Comboes", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.Comboes", "FeeId", "dbo.Fees");
            DropForeignKey("dbo.Comboes", "ChannelId", "dbo.Channels");
            //DropIndex("dbo.Transactions", new[] { "SourceNodeId" });
            DropIndex("dbo.SourceNodes", new[] { "SchemeId" });
            DropIndex("dbo.Schemes", new[] { "ComboId" });
            DropIndex("dbo.Schemes", new[] { "RouteId" });
            DropIndex("dbo.Routes", new[] { "SinkNodeId" });
            DropIndex("dbo.Comboes", new[] { "FeeId" });
            DropIndex("dbo.Comboes", new[] { "ChannelId" });
            DropIndex("dbo.Comboes", new[] { "TransactionTypeId" });
            //DropTable("dbo.Transactions");
            DropTable("dbo.SourceNodes");
            DropTable("dbo.Schemes");
            DropTable("dbo.SinkNodes");
            DropTable("dbo.Routes");
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.Fees");
            DropTable("dbo.Comboes");
            DropTable("dbo.Channels");
        }
    }
}
