using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public facultadesController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/facultades")]
        public IActionResult Get(){
            var marcasList = _contexto.marcas;
                return Ok(marcasList);         
        } 

        [HttpGet]
        [Route("api/facultades/{id}")]
        public IActionResult getbyid(int id)
        {
            facultades unEquipo = (from e in _contexto.facultades
                                where e.facultad_id == id  //filtro por id
                                select e).FirstOrDefault();
            if(unEquipo != null){
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/facultades/nombre_facultad/{nombre_facultad}")]
        public IActionResult getbyname(string nombre_facultad)
        {
            IEnumerable<facultades> facultdadbyName = from e in _contexto.facultades
                                                where e.nombre_facultad.Contains(nombre_facultad)
                                                select e;
            if( facultdadbyName.Count()>0){
                return Ok(facultdadbyName);
            }                           
            return NotFound();         
        }

        [HttpPost]
        [Route("api/facultades/insertar")]
        public IActionResult guardarFacultad([FromBody] facultades facultadNueva, string usuario) 
        {
            try{
                IEnumerable<facultades> facultdadExiste = from e in  _contexto.facultades
                                                    where e.nombre_facultad == facultadNueva.nombre_facultad
                                                    select e;
                if(facultdadExiste.Count()==0){
                    _contexto.facultades.Add(facultadNueva);
                    _contexto.SaveChanges();
                    return Ok(facultadNueva);
                }                                                    
                return Ok(facultdadExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/facultades")]
        public IActionResult updateEquipo([FromBody] facultades facultadAModificar)
        {
            facultades facultdadExiste = (from e in _contexto.facultades
                                    where  e.facultad_id  == facultadAModificar.facultad_id
                                    select e).FirstOrDefault();
            if(facultdadExiste is null)
            {
                return NotFound();
            }

            facultdadExiste.nombre_facultad = facultadAModificar.nombre_facultad;

            _contexto.Entry(facultdadExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(facultdadExiste);
        }
    }
}