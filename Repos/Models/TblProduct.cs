using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos.Models;

[Table("tbl_products")]
public partial class TblProduct
{
    [Key]
    [Column("productid")]
    [StringLength(50)]
    [Unicode(false)]
    public string Productid { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("price")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Price { get; set; }
}
