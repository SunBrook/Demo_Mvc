/*
 * @author: S 2024/9/29 19:14:34
 */

namespace Demo_Mvc.Common.Models.DbModels
{
    /// <summary>
    /// 管理员表
    /// </summary>
    public partial class Admins
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; } = null!;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
