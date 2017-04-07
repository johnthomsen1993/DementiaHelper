using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DementiaHelper.WebApi.Data;

namespace DementiaHelper.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170330113642_AddShoppingList")]
    partial class AddShoppingList
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

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Hash");

                    b.Property<string>("Lastname");

                    b.Property<int>("RoleId");

                    b.Property<string>("Salt");

                    b.HasKey("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Caregiver", b =>
                {
                    b.Property<int>("CaregiverId");

                    b.Property<int>("ApplicationUserForeignKey");

                    b.HasKey("CaregiverId");

                    b.ToTable("Caregiver");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.CaregiverConnection", b =>
                {
                    b.Property<int>("CaregiverConnectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Caregiver");

                    b.Property<int?>("Citizen");

                    b.HasKey("CaregiverConnectionId");

                    b.HasIndex("Caregiver");

                    b.HasIndex("Citizen");

                    b.ToTable("CaregiverConnection");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Citizen", b =>
                {
                    b.Property<int>("CitizenId");

                    b.Property<int>("ApplicationUserForeignKey");

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

                    b.Property<int>("ApplicationUserForeignKey");

                    b.HasKey("RelativeId");

                    b.ToTable("Relative");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.RelativeConnection", b =>
                {
                    b.Property<int>("RelativeConnectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Citizen");

                    b.Property<int?>("Relative");

                    b.HasKey("RelativeConnectionId");

                    b.HasIndex("Citizen");

                    b.HasIndex("Relative");

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

                    b.Property<int?>("CaregiverConnection");

                    b.Property<int?>("RelativeConnection");

                    b.HasKey("ShoppingListId");

                    b.HasIndex("CaregiverConnection");

                    b.HasIndex("RelativeConnection");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListDetail", b =>
                {
                    b.Property<int>("ShoppingListDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Bought");

                    b.Property<int?>("Product");

                    b.Property<int>("Quantity");

                    b.Property<int?>("ShoppingList");

                    b.HasKey("ShoppingListDetailId");

                    b.HasIndex("Product");

                    b.HasIndex("ShoppingList");

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
                    b.HasOne("DementiaHelper.WebApi.model.Role", "Role")
                        .WithMany("ApplicationUsers")
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
                    b.HasOne("DementiaHelper.WebApi.model.Caregiver", "CaregiverForeignKey")
                        .WithMany()
                        .HasForeignKey("Caregiver");

                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "CitizenForeignKey")
                        .WithMany()
                        .HasForeignKey("Citizen");
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
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "CitizenForeignKey")
                        .WithMany()
                        .HasForeignKey("Citizen");

                    b.HasOne("DementiaHelper.WebApi.model.Relative", "RelativeForeignKey")
                        .WithMany()
                        .HasForeignKey("Relative");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingList", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.CaregiverConnection", "CaregiverConnectionForeignKey")
                        .WithMany()
                        .HasForeignKey("CaregiverConnection");

                    b.HasOne("DementiaHelper.WebApi.model.RelativeConnection", "RelativeConnectionForeignKey")
                        .WithMany()
                        .HasForeignKey("RelativeConnection");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListDetail", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Product", "ProductForeignKey")
                        .WithMany()
                        .HasForeignKey("Product");

                    b.HasOne("DementiaHelper.WebApi.model.ShoppingList", "ShoppingListForeignKey")
                        .WithMany()
                        .HasForeignKey("ShoppingList");
                });
        }
    }
}
