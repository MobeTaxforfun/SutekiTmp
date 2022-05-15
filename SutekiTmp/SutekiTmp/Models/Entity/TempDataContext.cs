using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SutekiTmp.Models.Entity
{
    public partial class TempDataContext : DbContext
    {
        public TempDataContext()
        {
        }

        public TempDataContext(DbContextOptions<TempDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=TempData;User Id=sa;password=651qa90o$");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuId).ValueGeneratedNever();

                entity.Property(e => e.MenuName).HasMaxLength(50);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission");

                entity.Property(e => e.PermissionId).ValueGeneratedNever();

                entity.Property(e => e.PermissionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionName).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.HasMany(d => d.Menus)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "RoleMenu",
                        l => l.HasOne<Menu>().WithMany().HasForeignKey("MenuId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RoleMenu_Menu"),
                        r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RoleMenu_Role"),
                        j =>
                        {
                            j.HasKey("RoleId", "MenuId");

                            j.ToTable("RoleMenu");
                        });
            });

            modelBuilder.Entity<RoleMenuPermission>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.MenuId, e.PermissionId });

                entity.ToTable("RoleMenuPermission");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenuPermissions)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenuPermission_Menu");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RoleMenuPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenuPermission_Permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenuPermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenuPermission_Role");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PassWord)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRole_Role"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserRole_User"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("UserRole");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
