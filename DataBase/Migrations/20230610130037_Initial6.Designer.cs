﻿// <auto-generated />
using System;
using LinkStorage.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinkStorage.Migrations
{
    [DbContext(typeof(DbLinkStorageContext))]
    [Migration("20230610130037_Initial6")]
    partial class Initial6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LinkStorage.Models.SmartContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LinkToContract")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SmartContracts");
                });

            modelBuilder.Entity("LinkStorage.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LinkStorage.Models.SmartContract", b =>
                {
                    b.HasOne("LinkStorage.Models.User", null)
                        .WithMany("SmartContracts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LinkStorage.Models.User", b =>
                {
                    b.Navigation("SmartContracts");
                });
#pragma warning restore 612, 618
        }
    }
}
