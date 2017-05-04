using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DementiaHelper.WebApi.Data;

namespace DementiaHelper.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170504134815_ChangedPhoneToStringAndLimitSize")]
    partial class ChangedPhoneToStringAndLimitSize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DementiaHelper.WebApi.model.ApplicationUser", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChatGroupId");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Hash")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<int>("RoleId");

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.HasKey("ApplicationUserId");

                    b.HasIndex("ChatGroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CitizenId");

                    b.Property<string>("Color");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("AppointmentId");

                    b.HasIndex("CitizenId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Caregiver", b =>
                {
                    b.Property<int>("CaregiverId");

                    b.Property<int?>("CaregiverCenterId");

                    b.HasKey("CaregiverId");

                    b.HasIndex("CaregiverCenterId");

                    b.ToTable("Caregivers");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.CaregiverCenter", b =>
                {
                    b.Property<int>("CaregiverCenterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CaregiverConnectionId")
                        .IsRequired();

                    b.Property<string>("CitizenConnectionId")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<int>("Phone");

                    b.HasKey("CaregiverCenterId");

                    b.ToTable("CaregiverCenters");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatGroup", b =>
                {
                    b.Property<int>("ChatGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupName")
                        .IsRequired();

                    b.HasKey("ChatGroupId");

                    b.ToTable("ChatGroups");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatMessage", b =>
                {
                    b.Property<int>("ChatMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatGroupId");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<int>("SenderId");

                    b.HasKey("ChatMessageId");

                    b.HasIndex("ChatGroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Citizen", b =>
                {
                    b.Property<int>("CitizenId");

                    b.Property<int?>("CaregiverCenterId");

                    b.Property<string>("ConnectionId")
                        .IsRequired();

                    b.HasKey("CitizenId");

                    b.HasIndex("CaregiverCenterId");

                    b.ToTable("Citizens");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CitizenId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Subject");

                    b.HasKey("NoteId");

                    b.HasIndex("CitizenId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Relative", b =>
                {
                    b.Property<int>("RelativeId");

                    b.Property<int?>("CitizenId");

                    b.HasKey("RelativeId");

                    b.HasIndex("CitizenId");

                    b.ToTable("Relatives");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleName")
                        .IsRequired();

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListItem", b =>
                {
                    b.Property<int>("ShoppingListItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Bought");

                    b.Property<int>("CitizenId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("ShoppingListItemId");

                    b.HasIndex("CitizenId");

                    b.HasIndex("ProductId");

                    b.ToTable("ShoppingListItems");
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ApplicationUser", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ChatGroup", "ChatGroup")
                        .WithMany()
                        .HasForeignKey("ChatGroupId");

                    b.HasOne("DementiaHelper.WebApi.model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Appointment", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Caregiver", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.CaregiverCenter", "CaregiverCenter")
                        .WithMany()
                        .HasForeignKey("CaregiverCenterId");

                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("CaregiverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ChatMessage", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.ChatGroup", "ChatGroup")
                        .WithMany()
                        .HasForeignKey("ChatGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Citizen", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.CaregiverCenter", "CaregiverCenter")
                        .WithMany()
                        .HasForeignKey("CaregiverCenterId");

                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Note", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.Relative", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId");

                    b.HasOne("DementiaHelper.WebApi.model.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("RelativeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DementiaHelper.WebApi.model.ShoppingListItem", b =>
                {
                    b.HasOne("DementiaHelper.WebApi.model.Citizen", "Citizen")
                        .WithMany()
                        .HasForeignKey("CitizenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DementiaHelper.WebApi.model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
