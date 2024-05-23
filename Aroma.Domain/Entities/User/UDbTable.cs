using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.Rating;
using Aroma.Domain.Entities.Support;
using Newtonsoft.Json;

namespace Aroma.Domain.Entities.User
{
    public class UDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username cannot be longer than 30 characters.")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password cannot be shorter than 8 characters.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(30)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastLogin { get; set; }

        [StringLength(30)]
        public string LastIP { get; set; }

        public UserRole Level { get; set; }
        public decimal Balance { get; set; }

        public string Code { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<USupportForm> SupportMessages { get; set; }

    

        [JsonIgnore]
        public ICollection<RatingUdbTable> Ratings { get; set; } // Добавлено свойство для связи с оценками

        public bool EmailAccess { get; set; }
        public ICollection<USupportForm> SupportMesages { get; set; }
    }
}
