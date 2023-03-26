using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601.Controllers
{
    [ApiController] 
    public class equiposController : ControllerBase
    {
        private readonly prestamosContext _contexto;

        public equiposController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get(){
            var equiposList =   from e in _contexto.equipos
                                join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                                join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo
                                select new {
                                    equipoId = e.id_equipos,
                                    nombreEquipo = e.nombre,
                                    e.descripcion,
                                    e.tipo_equipo_id,
                                    tipo_equipo_des = te.descripcion,
                                    e.marca_id,
                                    ma.nombre_marca,
                                    e.modelo,
                                    e.anio_compra,
                                    e.estado_equipo_id,
                                    estado_equipo_des = ee.descripcion
                                };

            if(equiposList.Count()>0){
                return Ok(equiposList);
            }                      
            return NotFound();        
        } 

        [HttpGet]
        [Route("api/equipos/{id}")]
        public IActionResult getbyid(int id)
        {
            var unEquipo = (from e in _contexto.equipos
                                join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                                join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo

                                where e.id_equipos == id  //filtro por id
                                select new {
                                    equipoId = e.id_equipos,
                                    nombreEquipo = e.nombre,
                                    e.descripcion,
                                    e.tipo_equipo_id,
                                    tipo_equipo_des = te.descripcion,
                                    e.marca_id,
                                    ma.nombre_marca,
                                    e.modelo,
                                    e.anio_compra,
                                    e.estado_equipo_id,
                                    estado_equipo_des = ee.descripcion
                                }
                                ).FirstOrDefault();
            if(unEquipo != null){
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/equipos/tipo/{idTipoEquipo}")]
        public IActionResult getByTipoEquipo(int idTipoEquipo)
        {
            var equiposList =   from e in _contexto.equipos
                                join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                                join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo
                                where te.id_tipo_equipo  == idTipoEquipo
                                select new {
                                    equipoId = e.id_equipos,
                                    nombreEquipo = e.nombre,
                                    e.descripcion,
                                    e.tipo_equipo_id,
                                    tipo_equipo_des = te.descripcion,
                                    e.marca_id,
                                    ma.nombre_marca,
                                    e.modelo,
                                    e.anio_compra,
                                    e.estado_equipo_id,
                                    estado_equipo_des = ee.descripcion
                                };
            if(equiposList.Count()>0){
                return Ok(equiposList);
            }                      
            return NotFound();  
        }

        [HttpGet]
        [Route("api/equipos/buscarnombre/{buscarNombre}")]
        public IActionResult getbyname(string buscarNombre)
        {
            IEnumerable<equipos> equipobyName = from e in _contexto.equipos
                                                where e.nombre.Contains(buscarNombre)
                                                select e;
            if( equipobyName.Count()>0){
                return Ok(equipobyName);
            }                           
            return NotFound();         
        }

        [HttpPost]
        [Route("api/equipos/insertar")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo) 
        {
            try{
                IEnumerable<equipos> equipoExiste = from e in  _contexto.equipos
                                                    join t in _contexto.tipo_equipo on e.tipo_equipo_id equals t.id_tipo_equipo
                                                    where e.nombre == equipoNuevo.nombre
                                                    && equipoNuevo.tipo_equipo_id == t.id_tipo_equipo
                                                    select e;
                if(equipoExiste.Count()==0){
                    _contexto.equipos.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }                                                    
                return Ok(equipoExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoAModificar)
        {
            equipos equipoExiste = (from e in _contexto.equipos
                                    where  e.id_equipos  == equipoAModificar.id_equipos
                                    select e).FirstOrDefault();
            if(equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.nombre = equipoAModificar.nombre;
            equipoExiste.descripcion = equipoAModificar.descripcion;
            equipoExiste.modelo = equipoAModificar.modelo;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(equipoExiste);
        }
    }
}