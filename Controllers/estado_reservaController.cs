using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Practica1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Practica1
{
    [ApiController]
    public class estado_reservaController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public estado_reservaController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estado_reservas")]
        public IActionResult Get(){
            var marcasList = _contexto.estados_reserva;
                return Ok(marcasList);         
        } 

        [HttpGet]
        [Route("api/estado_reservas/{id}")]
        public IActionResult getbyid(int id)
        {
            estados_reserva un_estado_reservaEquipo = (from e in _contexto.estados_reserva
                                where e.estado_res_id == id  //filtro por id
                                select e).FirstOrDefault();
            if(un_estado_reservaEquipo != null){
                return Ok(un_estado_reservaEquipo);
            }
            return NotFound();
        }        

        [HttpPost]
        [Route("api/estado_reservas/insertar")]
        public IActionResult guardarFacultad([FromBody] estados_reserva estado_reservasdNuevo, string usuario) 
        {
            try{
                IEnumerable<estados_reserva> estado_reservaExiste = from e in  _contexto.estados_reserva
                                                    where e.estado == estado_reservasdNuevo.estado
                                                    select e;
                if(estado_reservaExiste.Count()==0){
                    _contexto.estados_reserva.Add(estado_reservasdNuevo);
                    _contexto.SaveChanges();
                    return Ok(estado_reservasdNuevo);
                }                                                    
                return Ok(estado_reservaExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/estado_reservas")]
        public IActionResult updateEquipo([FromBody] estados_reserva estado_reservasAModificar)
        {
            estados_reserva estado_reservasExiste = (from e in _contexto.estados_reserva
                                    where  e.estado_res_id  == estado_reservasAModificar.estado_res_id
                                    select e).FirstOrDefault();
            if(estado_reservasExiste is null)
            {
                return NotFound();
            }

            estado_reservasExiste.estado = estado_reservasAModificar.estado;

            _contexto.Entry(estado_reservasExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(estado_reservasExiste);
        }
    }
}