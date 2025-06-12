using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolioWebApp.Models;

public partial class Board
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    public string? Writer { get; set; }

    [Required]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Contents { get; set; } = null!;

    public DateTime? PostDate { get; set; }

    public int? ReadCount { get; set; }
}
