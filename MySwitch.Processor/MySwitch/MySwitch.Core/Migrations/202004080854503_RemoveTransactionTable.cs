namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTransactionTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "SourceNodeId", "dbo.SourceNodes");
            DropIndex("dbo.Transactions", new[] { "SourceNodeId" });
            DropTable("dbo.Transactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardPAN = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        SourceNodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Transactions", "SourceNodeId");
            AddForeignKey("dbo.Transactions", "SourceNodeId", "dbo.SourceNodes", "Id", cascadeDelete: true);
        }
    }
}
