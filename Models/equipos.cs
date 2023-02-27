using System.ComponentModel.DataAnnotations;
namespace Practica1.Models
{
    public class equipos
    {
        [Key]
        public int id_equipos { get; set; }
        public int nombre{ get; set; }
        public int descripcion { get; set; }
        public int tipo_equipo_id { get; set; }
        public int marca_id { get; set; }
        public int modelo { get; set; }
        public int anio_compra { get; set; }
        public int costo{ get; set; }
        public int vida_util { get; set; }
        public int estado_equipo_id { get; set; }
        public int estado { get; set; }
    }
}
