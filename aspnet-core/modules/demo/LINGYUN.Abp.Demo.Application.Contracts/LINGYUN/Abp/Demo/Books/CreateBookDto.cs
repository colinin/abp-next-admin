﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Demo.Books;

public class CreateBookDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    public BookType Type { get; set; } = BookType.Undefined;

    [Required]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; } = DateTime.Now;

    [Required]
    public float Price { get; set; }

    public Guid AuthorId { get; set; }
}
