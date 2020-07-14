namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Transactions",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardPAN = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionFee = c.String(),
                        ProcessingFee = c.String(),
                        Date = c.DateTime(nullable: false),
                        MTI = c.String(),
                        STAN = c.String(),
                        ChannelCode = c.String(),
                        OriginalDataElement = c.String(),
                        TransactionTypeCode = c.String(),
                        Account1 = c.String(),
                        Account2 = c.String(),
                        FeeCode = c.String(),
                        ResponseCode = c.String(),
                        ResponseDescription = c.String(),
                        IsReversePending = c.Boolean(nullable: false),
                        IsReversed = c.Boolean(nullable: false),
                        SourceNodeId = c.Int(nullable: false)
                    })
                    .PrimaryKey(t => t.Id)
                    .ForeignKey("dbo.SourceNodes", t => t.SourceNodeId, cascadeDelete: true)
                    .Index(t => t.SourceNodeId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "SourceNodeId", "dbo.SourceNodes");
            DropIndex("dbo.Transactions", new[] { "SourceNodeId" });
            DropTable("dbo.Transactions");
        }
    }
}
