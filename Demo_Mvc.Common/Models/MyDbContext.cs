/*
 * @author: S 2024/9/29 19:18:50
 */

using Demo_Mvc.Common.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// MySql 上下文对象
    /// </summary>
    public partial class MyDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyDbContext()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }


        /// <summary>
        /// 管理员表
        /// </summary>
        public virtual DbSet<Admins> Admins { get; set; } = null!;

        /// <summary>
        /// 模型创建
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasComment("后台人员表");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasComment("ID");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("创建时间");

                entity.Property(e => e.IsDeleted).HasComment("是否删除");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .HasComment("昵称");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasComment("密码");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasComment("账号");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
