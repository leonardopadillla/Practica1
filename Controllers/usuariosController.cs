using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public usuariosController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/usuarios")]
        public IActionResult Get(){
            var usuariosList =    from u in _contexto.usuarios
                                join ca in _contexto.carreras on u.carrera_id equals ca.carrera_id
                                select new {
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                u.documento,
                                u.tipo,
                                u.carnet,
                                carreraId = ca.carrera_id
                                };
                if(usuariosList.Count()>0){
                return Ok(usuariosList);
            }                      
            return NotFound();        
        } 

        [HttpGet]
        [Route("api/usuarios/{id}")]
        public IActionResult getbyid(int id)
        {
            var unUsuario = (   from u in _contexto.usuarios
                                join ca in _contexto.carreras on u.carrera_id equals ca.carrera_id

                                where u.usuario_id == id  //filtro por id
                                select new {
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                u.documento,
                                u.tipo,
                                u.carnet,
                                carreraId = ca.carrera_id
                                }
                                ).FirstOrDefault();
            if(unUsuario != null){
                return Ok(unUsuario);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/usuarios/carrera/{idCarrera}")]
        public IActionResult getByCarrera(int idCarrera)
        {
            var usuariosList =   from u in _contexto.usuarios
                                join ca in _contexto.carreras on u.carrera_id equals ca.carrera_id
                                where ca.carrera_id == idCarrera
                                select new {
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                u.documento,
                                u.tipo,
                                u.carnet,
                                ca.nombre_carrera,
                                carreraId = ca.carrera_id
                                };
            if(usuariosList.Count()>0){
                return Ok(usuariosList);
            }                      
            return NotFound();  
        }

        [HttpPost]
        [Route("api/usuarios/insertar")]
        public IActionResult guardarUsuario([FromBody] usuarios usuarioNuevo) 
        {
            try{
                IEnumerable<usuarios> usuarioExiste = from u in  _contexto.usuarios
                                                    join ca in _contexto.carreras on u.carrera_id equals ca.carrera_id
                                                    where u.nombre == usuarioNuevo.nombre                                                    
                                                    select u;
                if(usuarioExiste.Count()==0){
                    _contexto.usuarios.Add(usuarioNuevo);
                    _contexto.SaveChanges();
                    return Ok(usuarioNuevo);
                }                                                    
                return Ok(usuarioExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/usuarios")]
        public IActionResult usuarioEquipo([FromBody] usuarios usuarioAModificar)
        {
            usuarios usuarioExiste = (from u in _contexto.usuarios
                                    where  u.usuario_id  == usuarioAModificar.usuario_id
                                    select u).FirstOrDefault();
            if(usuarioExiste is null)
            {
                return NotFound();
            }

            usuarioExiste.nombre = usuarioAModificar.nombre;
            usuarioExiste.documento = usuarioAModificar.documento;
            usuarioExiste.tipo = usuarioAModificar.tipo;
            usuarioExiste.carnet = usuarioAModificar.carnet;

            _contexto.Entry(usuarioExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(usuarioExiste);
        }
    }
}