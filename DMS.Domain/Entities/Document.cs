using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string AccessURL { get; set; }
        //public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        //public string Tags { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public bool IsDeleted { get; set; }
        //public DateTime? DeletedDate { get; set; }
    }
}
