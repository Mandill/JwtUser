using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos.Models;

[PrimaryKey("Userid", "Tokenid")]
[Table("tbl_refreshtoken")]
[Index("TblUserUserId", Name = "IX_tbl_refreshtoken_TblUserUserId")]
public partial class TblRefreshtoken
{
    [Key]
    [Column("userid")]
    [StringLength(50)]
    [Unicode(false)]
    public string Userid { get; set; } = null!;

    [Key]
    [Column("tokenid")]
    [StringLength(50)]
    [Unicode(false)]
    public string Tokenid { get; set; } = null!;

    [Column("refreshtoken")]
    [Unicode(false)]
    public string? Refreshtoken { get; set; }

    [Column("expiretime", TypeName = "datetime")]
    public DateTime? Expiretime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TblUserUserId { get; set; }

    [ForeignKey("TblUserUserId")]
    [InverseProperty("TblRefreshtokens")]
    public virtual TblUser? TblUserUser { get; set; }
}
