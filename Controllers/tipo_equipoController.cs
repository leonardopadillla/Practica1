using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController] 
    public class tipo_equipoController : ControllerBase
    {
        private readonly prestamosContext _contexto;

        public tipo_equipoController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/tipo_equipo")]
        public IActionResult Get(){
            var tipo_equipoList = _contexto.tipo_equipo;
                return Ok(tipo_equipoList);          
        } 

        [HttpGet]
        [Route("api/tipo_equipo/{id}")]
        public IActionResult getbyid(int id)
        {
            tipo_equipo unEquipo = (from e in _contexto.tipo_equipo
                                where e.id_tipo_equipo == id  //filtro por id
                                select e).FirstOrDefault();
            if(unEquipo != null){
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/tipo_equipo/insertar")]
        public IActionResult guardarEquipo([FromBody] tipo_equipo tipoNuevo, string usuario) 
        {
            try{
                IEnumerable<tipo_equipo> equipoExiste = from e in  _contexto.tipo_equipo
                                                    where e.descripcion == tipoNuevo.descripcion
                                                    select e;
                if(equipoExiste.Count()==0){
                    _contexto.tipo_equipo.Add(tipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(tipoNuevo);
                }                                                    
                return Ok(equipoExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/tipo_equipo")]
        public IActionResult updateEquipo([FromBody] tipo_equipo equipoAModificar)
        {
            tipo_equipo equipoExiste = (from e in _contexto.tipo_equipo
                                    where  e.id_tipo_equipo  == equipoAModificar.id_tipo_equipo
                                    select e).FirstOrDefault();
            if(equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.descripcion = equipoAModificar.descripcion;
            equipoExiste.estado = equipoAModificar.estado;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(equipoExiste);
        }
    }
}