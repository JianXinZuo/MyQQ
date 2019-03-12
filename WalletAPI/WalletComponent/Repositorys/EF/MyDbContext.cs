using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WalletComponent.Common.EFCoreExtend;
using WalletComponent.Domains;

namespace WalletComponent.Repositorys.EF
{
    public class MyDbContext : DbContext
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        //public DbSet<Users> Users { get; set; }
        //public DbSet<UserGroup> UserGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Users>().ToTable("Users")
            //    .HasMany(u => u.GroupList)
            //    .WithOne(g => g.User)
            //    .HasForeignKey(g => g.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ChatMessages>().ToTable("ChatMessages").HasKey(c => c.Id);      //添加ChatMessages表结构

            modelBuilder.ApplyConfiguration(new UsersMapping());
            modelBuilder.ApplyConfiguration(new GroupsMapping());
            modelBuilder.ApplyConfiguration(new FriendNotificationMapping());
        }
    }

    public class UsersMapping : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users").HasKey(u => u.Id);

            builder.HasMany(u => u.GroupList)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(u => u.UserGroupRelationshipList)
                .WithOne(r => r.Users)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class GroupsMapping : IEntityTypeConfiguration<Groups>
    {
        public void Configure(EntityTypeBuilder<Groups> builder)
        {
            builder.ToTable("Groups").HasKey(g => g.Id);

            //分组与关联表一对多
            builder.HasMany(g => g.UserGroupRelationship)
                .WithOne(r => r.Group)
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class UserGroupRelationshipMapping : IEntityTypeConfiguration<UserGroupRelationship>
    {
        public void Configure(EntityTypeBuilder<UserGroupRelationship> builder)
        {
            //设置与
            builder.ToTable("UserGroupRelationships").HasKey(r => r.Id);

            //用户与关联表 一对多
            builder.HasOne(r => r.Users)
                .WithMany(u => u.UserGroupRelationshipList)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class FriendNotificationMapping : IEntityTypeConfiguration<FriendNotification>
    {
        public void Configure(EntityTypeBuilder<FriendNotification> builder)
        {
            builder.ToTable("FriendNotification").HasKey(f => f.Id);

            builder.HasOne(f => f.ToUser)
                .WithMany(u => u.FriendNotificationListByTo)
                .HasForeignKey(f => f.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(f => f.FromUser)
                .WithMany(u => u.FriendNotificationListByFrom)
                .HasForeignKey(f => f.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

}
