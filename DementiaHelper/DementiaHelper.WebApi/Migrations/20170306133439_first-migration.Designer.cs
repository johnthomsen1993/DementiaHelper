﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DementiaHelper.WebApi.Data;

namespace DementiaHelper.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170306133439_first-migration")]
    partial class firstmigration
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

                    b.Property<string>("Email");

                    b.Property<string>("Hash");

                    b.Property<string>("Salt");

                    b.HasKey("ApplicationUserId");

                    b.ToTable("ApplicationUsers");
                });
        }
    }
}
