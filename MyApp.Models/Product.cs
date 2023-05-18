using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class Product
    {
        public int Id { get; set; } //entity framework core automatically identifies this as a primary key('cause it has a name Id or <entity>Id
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }//entity framework core automatically identifies this as a Foreign key
        [ValidateNever]
        public Category Category { get; set; }
        
    }
}
