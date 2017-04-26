using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DementiaHelper.WebApi.Data;

namespace DementiaHelper.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170426102922_AddedChatTodatabase")]
    partial class AddedChatTodatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DementiaHelper.WebApi.model.AccountInformation", b =>
                {
                    b.Property<int>("AccountInformationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("AccountInformationId");

                    b.ToTable("AccountInformations");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.AccountPicture", b =>
                {
                    b.Property<int>("AccountPictureId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountInformationForeignKey");

                    b.Property<byte[]>("Image");

                    b.HasKey("AccountPictureId");

                    b.HasIndex("AccountInformationForeignKey")
                        .IsUnique();

                    b.ToTable("AccountPictures");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ApplicationUser", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatGroupId");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Hash");

                    b.Property<string>("Lastname");

                    b.Property<int>("RoleId");

                    b.Property<string>("Salt");

                    b.HasKey("ApplicationUserId");

                    b.HasIndex("ChatGroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Caregiver", b =>
                {
                    b.Property<int>("CaregiverId");

                    b.HasKey("CaregiverId");

                    b.ToTable("Caregiver");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.CaregiverConnection", b =>
                {
                    b.Property<int>("CaregiverConnectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaregiverId");

                    b.Property<int>("CitizenId");

                    b.HasKey("CaregiverConnectionId");

                    b.HasIndex("CaregiverId");

                    b.HasIndex("CitizenId");

                    b.ToTable("CaregiverConnection");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatGroup", b =>
                {
                    b.Property<int>("ChatGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupName");

                    b.HasKey("ChatGroupId");

                    b.ToTable("ChatGroups");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatMessage", b =>
                {
                    b.Property<int>("ChatMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChatGroupId");

                    b.Property<int>("GroupId");

                    b.Property<string>("Message");

                    b.Property<string>("Sender");

                    b.HasKey("ChatMessageId");

                    b.HasIndex("ChatGroupId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Citizen", b =>
                {
                    b.Property<int>("CitizenId");

                    b.HasKey("CitizenId");

                    b.ToTable("Citizen");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductName");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Relative", b =>
                {
                    b.Property<int>("RelativeId");

                    b.HasKey("RelativeId");

                    b.ToTable("Relative");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.RelativeConnection", b =>
                {
                    b.Property<int>("RelativeConnectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CitizenId");

                    b.Property<int>("RelativeId");

                    b.HasKey("RelativeConnectionId");

                    b.HasIndex("CitizenId");

                    b.HasIndex("RelativeId");

                    b.ToTable("RelativeConnectiob");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleName");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingList", b =>
                {
                    b.Property<int>("ShoppingListId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaregiverConnectionId");

                    b.Property<int>("RelativeConnectionId");

                    b.HasKey("ShoppingListId");

                    b.HasIndex("CaregiverConnectionId");

                    b.HasIndex("RelativeConnectionId");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListDetail", b =>
                {
                    b.Property<int>("ShoppingListDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Bought");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<int>("ShoppingListId");

                    b.HasKey("ShoppingListDetailId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ShoppingListDetails");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.AccountPicture", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.AccountInformation", "AccountInformation")
                        .WithOne("Picture")
                        .HasForeignKey("DementiaHelper.WebApi.model.AccountPicture", "AccountInformationForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ApplicationUser", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ChatGroup", "ChatGroup")
                        .WithMany()
                        .HasForeignKey("ChatGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Caregiver", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("CaregiverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.CaregiverConnection", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Caregiver", "Caregiver")
                        .WithMany()
                        .HasForeignKey("CaregiverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatMessage", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ChatGroup", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatGroupId");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Citizen", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Relative", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("RelativeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.RelativeConnection", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.Relative", "Relative")
                        .WithMany()
                        .HasForeignKey("RelativeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingList", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.CaregiverConnection", "CaregiverConnection")
                        .WithMany()
                        .HasForeignKey("CaregiverConnectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.RelativeConnection", "RelativeConnection")
                        .WithMany()
                        .HasForeignKey("RelativeConnectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListDetail", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.ShoppingList", "ShoppingList")
                        .WithMany()
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
