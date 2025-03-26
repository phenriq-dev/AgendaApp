Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class InitialCreate
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Appointments",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .UserId = c.Int(nullable := False),
                        .Title = c.String(),
                        .Description = c.String(),
                        .DateTime = c.DateTime(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Users", Function(t) t.UserId, cascadeDelete := True) _
                .Index(Function(t) t.UserId)
            
            CreateTable(
                "dbo.Users",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Name = c.String(),
                        .Email = c.String(),
                        .PasswordHash = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Appointments", "UserId", "dbo.Users")
            DropIndex("dbo.Appointments", New String() { "UserId" })
            DropTable("dbo.Users")
            DropTable("dbo.Appointments")
        End Sub
    End Class
End Namespace
