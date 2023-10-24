using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Database.Entity.Sqlite;
[Table("LAYOUT")]
public class Config
{
    [Key]
    [Column("UUID")]
    public Guid Uuid { get; set; }

    [Column("NODENAME")]
    [StringLength(10)]
    [Required]
    public string? NodeName { get; set; }
}