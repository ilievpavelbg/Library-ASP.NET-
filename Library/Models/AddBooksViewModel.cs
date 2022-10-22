﻿using Library.Data;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddBooksViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.00", "10.00")]

        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<Category> Category { get; set; } = new List<Category>();
    }
}