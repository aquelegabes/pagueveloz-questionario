﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PagueVeloz.Data.Context;

namespace PagueVeloz.Data.Migrations
{
    [DbContext(typeof(PagueVelozContext))]
    partial class PagueVelozContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("PagueVeloz.Domain.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeFantasia")
                        .IsRequired()
                        .HasColumnType("TEXT COLLATE NOCASE")
                        .HasMaxLength(100);

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasColumnType("TEXT COLLATE NOCASE")
                        .HasMaxLength(254);

                    b.HasKey("Id");

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Fone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FornecedorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId");

                    b.HasIndex("Numero");

                    b.ToTable("Fones");
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Fornecedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CNPJ")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20)
                        .HasDefaultValue("");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPessoaFisica")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT COLLATE NOCASE")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.HasIndex("EmpresaId")
                        .IsUnique();

                    b.ToTable("Fornecedores");
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FornecedorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Nascimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("RG")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.HasIndex("FornecedorId")
                        .IsUnique();

                    b.HasIndex("RG")
                        .IsUnique();

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Fone", b =>
                {
                    b.HasOne("PagueVeloz.Domain.Models.Fornecedor", "Fornecedor")
                        .WithMany("Fones")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Fornecedor", b =>
                {
                    b.HasOne("PagueVeloz.Domain.Models.Empresa", "Empresa")
                        .WithOne("Fornecedor")
                        .HasForeignKey("PagueVeloz.Domain.Models.Fornecedor", "EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PagueVeloz.Domain.Models.Pessoa", b =>
                {
                    b.HasOne("PagueVeloz.Domain.Models.Fornecedor", "Fornecedor")
                        .WithOne("Pessoa")
                        .HasForeignKey("PagueVeloz.Domain.Models.Pessoa", "FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
