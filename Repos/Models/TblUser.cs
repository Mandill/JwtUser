using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos.Models;

[Table("tbl_users")]
public partial class TblUser
{
    [Key]
    [Column("user_id")]
    [StringLength(50)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    [Column("username")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("password")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("role")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Role { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }
}
