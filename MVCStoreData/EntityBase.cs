using System.ComponentModel.DataAnnotations;

namespace MVCStoreData
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        [Display(Name = "Kayıt T.")]
        public DateTime DateCreated { get; set; }

        //Soft Delete
        [Display(Name = "Durum")]
        public bool Enabled { get; set; }
    }
}
