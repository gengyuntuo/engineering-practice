using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Model
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    public class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        // [Index(IsUnique = true)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "TEXT")]
        public string Type { get; set; } = "string";

        [Column(TypeName = "BOOLEAN")]
        public bool IsSystem { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 用户表
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        // [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性 - 好友关系
        // public virtual ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();

        // 导航属性 - 发送的消息
        // public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();

        // 导航属性 - 接收的消息
        // public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }

    /// <summary>
    /// 好友关系表
    /// </summary>
    [Table("Friendships")]
    public class Friendship
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int FriendId { get; set; }

        [Column(TypeName = "BOOLEAN")]
        public bool IsConfirmed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }

        // 导航属性 - 用户
        // [ForeignKey("UserId")]
        // public virtual User User { get; set; }

        // 导航属性 - 好友
        // [ForeignKey("FriendId")]
        // public virtual User Friend { get; set; }
    }

    /// <summary>
    /// 消息表
    /// </summary>
    [Table("Messages")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Content { get; set; }

        [Column(TypeName = "BOOLEAN")]
        public bool IsSystem { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        // 导航属性 - 发送者
        // [ForeignKey("SenderId")]
        // public virtual User Sender { get; set; }

        // 导航属性 - 接收者
        // [ForeignKey("ReceiverId")]
        // public virtual User Receiver { get; set; }
    }

    /// <summary>
    /// 消息统计项
    /// </summary>
    public class MessageStatisticItem
    {
        public int SenderId { get; set; }
        public int TotalMessage { get; set; }
        /// <summary>
        /// 已读消息计数
        /// </summary>
        public int ReadMessage { get; set; }
        /// <summary>
        /// 未读消息计数
        /// </summary>
        public int UnReadMessage { get; set; }
    }
}
