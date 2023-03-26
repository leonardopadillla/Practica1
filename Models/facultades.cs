using System;
using System.ComponentModel.DataAnnotations;

namespace Practica1.Models
{
    public class facultades{
    
        [Key]
        public int facultad_id {get; set;}
        public string nombre_facultad {get; set;}        
    }
}