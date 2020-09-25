using System;
using System.ComponentModel.DataAnnotations;

namespace DBAccessLibrary
{
    public class Company
    {

        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }
        public Company(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public int Id { get; set; }
        
        [StringLength(500)]
        [Required]
        public string Address  { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }

}