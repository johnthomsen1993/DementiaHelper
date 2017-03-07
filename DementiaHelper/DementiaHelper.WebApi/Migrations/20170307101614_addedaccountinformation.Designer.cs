using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DementiaHelper.WebApi.Data;

namespace DementiaHelper.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170307101614_addedaccountinformation")]
    partial class addedaccountinformation
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

                    b.Property<int>("BorgerId");

                    b.Property<string>("Email");

                    b.Property<string>("Hash");

                    b.Property<string>("Salt");

                    b.HasKey("ApplicationUserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.AccountPicture", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.AccountInformation", "AccountInformation")
                        .WithOne("Picture")
                        .HasForeignKey("DementiaHelper.WebApi.model.AccountPicture", "AccountInformationForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
