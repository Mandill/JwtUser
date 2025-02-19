using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos.Models;

[Keyless]
[Table("tbl_refreshtoken")]
public partial class TblRefreshtoken
{
    [Column("userid")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Userid { get; set; }

    [Column("tokenid")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Tokenid { get; set; }

    [Column("refreshtoken")]
    [Unicode(false)]
    public string? Refreshtoken { get; set; }
}
