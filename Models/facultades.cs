using System;
using System.ComponentModel.DataAnnotations;

namespace _2018AN601.Models
{
    public class facultades{
    
        [Key]
        public int facultad_id {get; set;}
        public string nombre_facultad {get; set;}        
    }
}