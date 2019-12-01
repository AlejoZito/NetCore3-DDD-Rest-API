﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCore3_api.Infrastructure;

namespace NetCore3_api.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20191201175408_InvoiceFK")]
    partial class InvoiceFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.Charge", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long?>("InvoiceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.ChargeCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChargeCategories");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "MarketPlace"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Servicios"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Externo"
                        });
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.EventType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("EventTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CategoryId = 1L,
                            Name = "Clasificado"
                        },
                        new
                        {
                            Id = 2L,
                            CategoryId = 1L,
                            Name = "Venta"
                        },
                        new
                        {
                            Id = 3L,
                            CategoryId = 1L,
                            Name = "Envío"
                        },
                        new
                        {
                            Id = 4L,
                            CategoryId = 2L,
                            Name = "Crédito"
                        },
                        new
                        {
                            Id = 5L,
                            CategoryId = 2L,
                            Name = "Fidelidad"
                        },
                        new
                        {
                            Id = 6L,
                            CategoryId = 2L,
                            Name = "Publicidad"
                        },
                        new
                        {
                            Id = 7L,
                            CategoryId = 3L,
                            Name = "MercadoPago"
                        },
                        new
                        {
                            Id = 8L,
                            CategoryId = 3L,
                            Name = "MercadoShop"
                        });
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Payment.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Payment.PaymentCharge", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("ChargeId")
                        .HasColumnType("bigint");

                    b.Property<long>("PaymentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChargeId");

                    b.HasIndex("PaymentId");

                    b.ToTable("PaymentCharge");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.User.Invoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.User.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Username = "Hermeto Pascoal"
                        },
                        new
                        {
                            Id = 2L,
                            Username = "Leon Montana"
                        });
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.Charge", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.Event.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.User.Invoice", null)
                        .WithMany("Charges")
                        .HasForeignKey("InvoiceId");

                    b.OwnsOne("NetCore3_api.Domain.Models.ValueObjects.AmountCurrency", "Amount", b1 =>
                        {
                            b1.Property<long>("ChargeId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int?>("Currency")
                                .HasColumnType("int");

                            b1.HasKey("ChargeId");

                            b1.ToTable("Charges");

                            b1.WithOwner()
                                .HasForeignKey("ChargeId");
                        });
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.Event", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.Event.EventType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Event.EventType", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.Event.ChargeCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Payment.Payment", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.Payment.PaymentCharge", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.Event.Charge", "Charge")
                        .WithMany("Payments")
                        .HasForeignKey("ChargeId");

                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.Payment.Payment", "Payment")
                        .WithMany("Charges")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetCore3_api.Domain.Models.Aggregates.User.Invoice", b =>
                {
                    b.HasOne("NetCore3_api.Domain.Models.Aggregates.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
