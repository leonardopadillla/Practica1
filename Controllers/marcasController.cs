using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController] 
    public class marcasController : ControllerBase
    {
        private readonly prestamosContext _contexto;

        public marcasController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/marcas")]
        public IActionResult Get(){
            var marcasList = _contexto.marcas;
                return Ok(marcasList);          
        } 

        [HttpGet]
        [Route("api/marcas/{id}")]
        public IActionResult getbyid(int id)
        {
            marcas unEquipo = (from e in _contexto.marcas
                                where e.id_marcas == id  //filtro por id
                                select e).FirstOrDefault();
            if(unEquipo != null){
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/marcas/buscarmarca/{buscarmarca}")]
        public IActionResult getbyname(string buscarmarca)
        {
            IEnumerable<marcas> equipobyName = from e in _contexto.marcas
                                                where e.nombre_marca.Contains(buscarmarca)
                                                select e;
            if( equipobyName.Count()>0){
                return Ok(equipobyName);
            }                           
            return NotFound();         
        }

        [HttpPost]
        [Route("api/marcas/insertar")]
        public IActionResult guardarEquipo([FromBody] marcas marcaNuevo, string usuario) 
        {
            try{
                IEnumerable<marcas> equipoExiste = from e in  _contexto.marcas
                                                    where e.nombre_marca == marcaNuevo.nombre_marca
                                                    select e;
                if(equipoExiste.Count()==0){
                    _contexto.marcas.Add(marcaNuevo);
                    _contexto.SaveChanges();
                    return Ok(marcaNuevo);
                }                                                    
                return Ok(equipoExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/marcas")]
        public IActionResult updateEquipo([FromBody] marcas equipoAModificar)
        {
            marcas equipoExiste = (from e in _contexto.marcas
                                    where  e.id_marcas  == equipoAModificar.id_marcas
                                    select e).FirstOrDefault();
            if(equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.nombre_marca = equipoAModificar.nombre_marca;
            equipoExiste.estados = equipoAModificar.estados;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(equipoExiste);
        }
    }
}