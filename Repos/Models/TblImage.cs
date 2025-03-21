using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos.Models;

[Table("tbl_images")]
public partial class TblImage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("image_data")]
    public byte[] ImageData { get; set; } = null!;

    [Column("image_name")]
    [StringLength(255)]
    public string ImageName { get; set; } = null!;

    [Column("product_id")]
    [StringLength(255)]
    public string ProductId { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
}
