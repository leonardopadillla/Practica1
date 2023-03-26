using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2018AN601.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2018AN601
{
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public reservasController(prestamosContext miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/reservas")]
        public IActionResult Get(){
            var reservasList =  from r in _contexto.reservas
                                join e in _contexto.equipos on r.equipo_id equals e.id_equipos
                                join u in _contexto.usuarios on r.usuario_id equals u.usuario_id
                                join er in _contexto.estados_reserva on r.estado_reserva_id equals er.estado_res_id
                                select new {
                                reservasId = r.reserva_id,
                                equipoId = e.id_equipos,
                                nombreEquipo = e.nombre,
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                r.fecha_salida,
                                r.hora_salida,
                                r.tiempo_reserva,
                                er.estado_res_id,
                                er.estado,
                                r.fecha_retorno,
                                r.hora_retorno                                
                                };
                if(reservasList.Count()>0){
                return Ok(reservasList);
            }                      
            return NotFound();     
        } 

        [HttpGet]
        [Route("api/reservas/{id}")]
        public IActionResult getbyid(int id)
        {
            var unaReserva = (  from r in _contexto.reservas
                                join e in _contexto.equipos on r.equipo_id equals e.id_equipos
                                join u in _contexto.usuarios on r.usuario_id equals u.usuario_id
                                join er in _contexto.estados_reserva on r.estado_reserva_id equals er.estado_res_id
                                where r.reserva_id == id //filtro por id
                                select new {
                                reservasId = r.reserva_id,
                                equipoId = e.id_equipos,
                                nombreEquipo = e.nombre,
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                r.fecha_salida,
                                r.hora_salida,
                                r.tiempo_reserva,
                                er.estado_res_id,
                                er.estado,
                                r.fecha_retorno,
                                r.hora_retorno 
                                }
                                ).FirstOrDefault();
            if(unaReserva != null){
                return Ok(unaReserva);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/reservas/equipoId/{id}")]
        public IActionResult getbyequipoId(int id)
        {
            var unaReserva = (  from r in _contexto.reservas
                                join e in _contexto.equipos on r.equipo_id equals e.id_equipos
                                join u in _contexto.usuarios on r.usuario_id equals u.usuario_id
                                join er in _contexto.estados_reserva on r.estado_reserva_id equals er.estado_res_id
                                where e.id_equipos == id //filtro por id
                                select new {
                                reservasId = r.reserva_id,
                                equipoId = e.id_equipos,
                                nombreEquipo = e.nombre,
                                usuarioId = u.usuario_id,
                                nombreUsuario = u.nombre,
                                r.fecha_salida,
                                r.hora_salida,
                                r.tiempo_reserva,
                                er.estado_res_id,
                                er.estado,
                                r.fecha_retorno,
                                r.hora_retorno 
                                }
                                ).FirstOrDefault();
            if(unaReserva != null){
                return Ok(unaReserva);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/reservas/insertar")]
        public IActionResult guardarReserva([FromBody] reservas reservaNuevo) 
        {
            try{
                IEnumerable<reservas> reservaExiste = from r in _contexto.reservas
                                                    join e in _contexto.equipos on r.equipo_id equals e.id_equipos
                                                    join u in _contexto.usuarios on r.usuario_id equals u.usuario_id
                                                    join er in _contexto.estados_reserva on r.estado_reserva_id equals er.estado_res_id
                                                    where r.reserva_id== reservaNuevo.reserva_id                                                   
                                                    select r;
                if(reservaExiste.Count()==0){
                    _contexto.reservas.Add(reservaNuevo);
                    _contexto.SaveChanges();
                    return Ok(reservaNuevo);
                }                                                    
                return Ok(reservaExiste);
            }catch(System.Exception){
                return BadRequest();
            }            
        }

        [HttpPut]
        [Route("api/reservas")]
        public IActionResult reservaMod([FromBody] reservas reservaAModificar)
        {
            reservas reservaExiste = (from r in _contexto.reservas
                                    where  r.reserva_id  == reservaAModificar.reserva_id
                                    select r).FirstOrDefault();
            if(reservaExiste is null)
            {
                return NotFound();
            }

            reservaExiste.equipo_id = reservaAModificar.equipo_id;
            reservaExiste.equipo_id = reservaAModificar.equipo_id;
            reservaExiste.usuario_id = reservaAModificar.usuario_id;
            reservaExiste.fecha_salida = reservaAModificar.fecha_salida;
            reservaExiste.hora_salida = reservaAModificar.hora_salida;
            reservaExiste.tiempo_reserva = reservaAModificar.tiempo_reserva;
            reservaExiste.estado_reserva_id = reservaAModificar.estado_reserva_id;
            reservaExiste.fecha_retorno = reservaAModificar.fecha_retorno;
            reservaExiste.hora_retorno = reservaAModificar.hora_retorno;            

            _contexto.Entry(reservaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(reservaExiste);
        }
    }
}