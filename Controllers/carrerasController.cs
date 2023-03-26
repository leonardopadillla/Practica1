using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public carrerasController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/carreras")]
        public IActionResult Get(){
            var carrerasList =  from ca in _contexto.carreras
                                join f in _contexto.facultades on ca.facultad_id equals f.facultad_id
                                select new {
                                carreraId = ca.carrera_id,
                                ca.nombre_carrera,                                
                                facultadId = f.facultad_id
                                };
                if(carrerasList.Count()>0){
                return Ok(carrerasList);
            }                      
            return NotFound();             
        } 

        [HttpGet]
        [Route("api/carreras/{id}")]
        public IActionResult getbyid(int id)
        {
            var unaCarrera = (  from ca in _contexto.carreras
                                join f in _contexto.facultades on ca.facultad_id equals f.facultad_id
                                where ca.carrera_id == id  //filtro por id
                                select new {
                                carreraId = ca.carrera_id,
                                ca.nombre_carrera,                                
                                facultadId = f.facultad_id,
                                f.nombre_facultad
                                }
                                ).FirstOrDefault();
            if(unaCarrera != null){
                return Ok(unaCarrera);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/carreras/facultad/{idFacultad}")]
        public IActionResult getByFacultad(int idFacultad)
        {
            var carrerasList =  from ca in _contexto.carreras
                                join f in _contexto.facultades on ca.facultad_id equals f.facultad_id
                                where f.facultad_id == idFacultad
                                select new {
                                carreraId = ca.carrera_id,
                                ca.nombre_carrera,                                
                                facultadId = f.facultad_id,
                                f.nombre_facultad
                                };
                if(carrerasList.Count()>0){
                return Ok(carrerasList);
            }                      
            return NotFound(); 
        }

        [HttpPost]
        [Route("api/carreras/insertar")]
        public IActionResult guardarUsuario([FromBody] carreras carreraNuevo) 
        {
            try{
                IEnumerable<carreras> carreraExiste = from ca in  _contexto.carreras
                                                    join f in _contexto.facultades on ca.facultad_id equals f.facultad_id
                                                    where ca.nombre_carrera == carreraNuevo.nombre_carrera                                                    
                                                    select ca;
                if(carreraExiste.Count()==0){
                    _contexto.carreras.Add(carreraNuevo);
                    _contexto.SaveChanges();
                    return Ok(carreraNuevo);
                }                                                    
                return Ok(carreraExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/carreras")]
        public IActionResult usuarioEquipo([FromBody] carreras carreraAModificar)
        {
            carreras carreraExiste = (from ca in _contexto.carreras
                                    where  ca.carrera_id  == carreraAModificar.carrera_id
                                    select ca).FirstOrDefault();
            if(carreraExiste is null)
            {
                return NotFound();
            }

            carreraExiste.nombre_carrera = carreraAModificar.nombre_carrera;
            

            _contexto.Entry(carreraExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(carreraExiste);
        }
    }
}