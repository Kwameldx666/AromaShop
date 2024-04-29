using Aroma.Domain.Entities.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.Support
{
    public class USupportForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("SupportUser")]
        public int SupportUserId { get; set; }
        [JsonIgnore]
        public virtual UDbTable SupportUser { get; set; } // Свойство для связи с пользователем

        public string name { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
