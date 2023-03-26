using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController] 
    public class equiposController : ControllerBase
    {
        private readonly prestamosContext _contexto;

        public equiposController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estados_equipo")]
        public IActionResult Get(){
            var estado_equipoList = _contexto.estados_equipo;
                return Ok(estado_equipoList);          
        } 

        [HttpGet]
        [Route("api/estados_equipo/{id}")]
        public IActionResult getbyid(int id)
        {
            estados_equipo unEquipo = (from e in _contexto.estados_equipo
                                where e.id_estados_equipo == id  //filtro por id
                                select e).FirstOrDefault();
            if(unEquipo != null){
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estados_equipo/BuscarxEstado/{estado}")]
        public IActionResult getbystatus(string estado)
        {
            IEnumerable<estados_equipo> estadoequipobyStatus = from e in _contexto.estados_equipo
                                                where e.estado.Contains(estado)
                                                select e;
            if( estadoequipobyStatus.Count()>0){
                return Ok(estadoequipobyStatus);
            }                           
            return NotFound();         
        }

        [HttpPost]
        [Route("api/estados_equipo/insertar")]
        public IActionResult guardarestadoEquipo([FromBody] estados_equipo equipoNuevo) 
        {
            try{
                IEnumerable<estados_equipo> equipoExiste = from e in  _contexto.estados_equipo
                                                    where e.descripcion == equipoNuevo.descripcion
                                                    select e;
                if(equipoExiste.Count()==0){
                    _contexto.estados_equipo.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }                                                    
                return Ok(equipoExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/estados_equipo")]
        public IActionResult updateEquipo([FromBody] estados_equipo equipoAModificar)
        {
            estados_equipo equipoExiste = (from e in _contexto.estados_equipo
                                    where  e.id_estados_equipo  == equipoAModificar.id_estados_equipo
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