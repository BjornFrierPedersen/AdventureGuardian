﻿// <auto-generated />
using System;
using AdventureGuardian.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdventureGuardian.Infrastructure.Migrations
{
    [DbContext(typeof(AdventureGuardianDbContext))]
    [Migration("20231205152328_InititalCreate")]
    partial class InititalCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AdventureGuardian.Models.Models.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BackgroundStory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BonusHitPoints")
                        .HasColumnType("integer");

                    b.Property<int?>("CampaignId")
                        .HasColumnType("integer");

                    b.Property<int?>("EncounterId")
                        .HasColumnType("integer");

                    b.Property<int>("HitPoints")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.HasIndex("EncounterId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Encounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CampaignId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int[]>("Creatures")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Encounters");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Stats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer");

                    b.Property<int>("Charisma")
                        .HasColumnType("integer");

                    b.Property<int>("Constitution")
                        .HasColumnType("integer");

                    b.Property<int>("Dexterity")
                        .HasColumnType("integer");

                    b.Property<int>("Intelligence")
                        .HasColumnType("integer");

                    b.Property<int>("Strength")
                        .HasColumnType("integer");

                    b.Property<int>("Wisdom")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Worlds.World", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CampaignId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ExplicitContent")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId")
                        .IsUnique();

                    b.ToTable("Worlds");

                    b.HasDiscriminator<string>("Discriminator").HasValue("World");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Worlds.FantasyWorld", b =>
                {
                    b.HasBaseType("AdventureGuardian.Models.Models.Worlds.World");

                    b.HasDiscriminator().HasValue("FantasyWorld");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Worlds.RealismWorld", b =>
                {
                    b.HasBaseType("AdventureGuardian.Models.Models.Worlds.World");

                    b.HasDiscriminator().HasValue("RealismWorld");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Worlds.SciFiWorld", b =>
                {
                    b.HasBaseType("AdventureGuardian.Models.Models.Worlds.World");

                    b.HasDiscriminator().HasValue("SciFiWorld");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Character", b =>
                {
                    b.HasOne("AdventureGuardian.Models.Models.Campaign", null)
                        .WithMany("Characters")
                        .HasForeignKey("CampaignId");

                    b.HasOne("AdventureGuardian.Models.Models.Encounter", null)
                        .WithMany("Characters")
                        .HasForeignKey("EncounterId");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Encounter", b =>
                {
                    b.HasOne("AdventureGuardian.Models.Models.Campaign", "Campaign")
                        .WithMany("Encounters")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Stats", b =>
                {
                    b.HasOne("AdventureGuardian.Models.Models.Character", null)
                        .WithOne("BonusStats")
                        .HasForeignKey("AdventureGuardian.Models.Models.Stats", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Worlds.World", b =>
                {
                    b.HasOne("AdventureGuardian.Models.Models.Campaign", "Campaign")
                        .WithOne("World")
                        .HasForeignKey("AdventureGuardian.Models.Models.Worlds.World", "CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Campaign", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("Encounters");

                    b.Navigation("World")
                        .IsRequired();
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Character", b =>
                {
                    b.Navigation("BonusStats")
                        .IsRequired();
                });

            modelBuilder.Entity("AdventureGuardian.Models.Models.Encounter", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
