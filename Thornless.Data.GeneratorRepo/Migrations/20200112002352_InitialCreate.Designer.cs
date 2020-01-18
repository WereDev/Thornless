﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Thornless.Data.GeneratorRepo;

namespace Thornless.Data.GeneratorRepo.Migrations
{
    [DbContext(typeof(GeneratorContext))]
    [Migration("20200112002352_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Copyright")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlavorHtml")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastUpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CharacterAncestry");
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryNamePartDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharacterAncestryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameMeaningsJson")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamePartsJson")
                        .HasColumnType("TEXT");

                    b.Property<string>("NameSegmentCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("RandomizationWeight")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CharacterAncestryId");

                    b.ToTable("CharacterAncestryNamePart");
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryOptionDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharacterAncestryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamePartSeperatorJson")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeperatorChancePercentage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CharacterAncestryId");

                    b.ToTable("CharacterAncestryOption");
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestrySegmentGroupDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharacterAncestryOptionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameSegmentCodesJson")
                        .HasColumnType("TEXT");

                    b.Property<int>("RandomizationWeight")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CharacterAncestryOptionId");

                    b.ToTable("CharacterAncestrySegmentGroup");
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryNamePartDto", b =>
                {
                    b.HasOne("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryDto", null)
                        .WithMany("NameParts")
                        .HasForeignKey("CharacterAncestryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryOptionDto", b =>
                {
                    b.HasOne("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryDto", null)
                        .WithMany("Options")
                        .HasForeignKey("CharacterAncestryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestrySegmentGroupDto", b =>
                {
                    b.HasOne("Thornless.Data.GeneratorRepo.DataModels.CharacterAncestryOptionDto", null)
                        .WithMany("SegmentGroups")
                        .HasForeignKey("CharacterAncestryOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
