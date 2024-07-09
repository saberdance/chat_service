﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tts_service.Db;

#nullable disable

namespace tts_service.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20240701101107_2024-07-01-Data")]
    partial class _20240701Data
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("tts_service.Models.Sessions.ChatSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatCount")
                        .HasColumnType("int");

                    b.Property<int>("EngineId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ChatSessions");
                });

            modelBuilder.Entity("tts_service.Models.Stastics.TtsCall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CallCount")
                        .HasColumnType("int");

                    b.Property<int>("EngineId")
                        .HasColumnType("int");

                    b.Property<string>("EngineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VoiceId")
                        .HasColumnType("int");

                    b.Property<string>("VoiceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TtsCalls");
                });

            modelBuilder.Entity("tts_service.Models.TtsData.TtsEngine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Avaliable")
                        .HasColumnType("bit");

                    b.Property<int>("CallCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TokenRemain")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TtsEngines");
                });

            modelBuilder.Entity("tts_service.Models.TtsData.TtsVoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Engine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TtsEngineId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TtsEngineId");

                    b.ToTable("TtsVoices");
                });

            modelBuilder.Entity("tts_service.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LastCall")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenExpire")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("tts_service.Models.TtsData.TtsVoice", b =>
                {
                    b.HasOne("tts_service.Models.TtsData.TtsEngine", null)
                        .WithMany("Vocies")
                        .HasForeignKey("TtsEngineId");
                });

            modelBuilder.Entity("tts_service.Models.TtsData.TtsEngine", b =>
                {
                    b.Navigation("Vocies");
                });
#pragma warning restore 612, 618
        }
    }
}
