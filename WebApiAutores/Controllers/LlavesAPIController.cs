using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.DTOs;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/llavesapi")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LlavesAPIController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ServicioLlaves servicioLlaves;

        public LlavesAPIController(ApplicationDbContext context, IMapper mapper, ServicioLlaves servicioLlaves)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioLlaves = servicioLlaves;
        }

        [HttpGet]
        public async Task<ActionResult<List<LlaveDTO>>> MisLlaves()
        {
            var usuarioId = ObtenerUsuarioId();
            var llaves = await context.LlavesApi
                .Include(x => x.RestriccionesDominio)
                .Include(x => x.RestriccionesIP)
                .Where(x => x.UsuarioId == usuarioId).ToListAsync();
            return mapper.Map<List<LlaveDTO>>(llaves);
        }

        [HttpPost]
        public async Task<ActionResult> CrearLlave(CrearLlaveDTO crearLlaveDTO)
        {
            var usuarioId = ObtenerUsuarioId();

            if(crearLlaveDTO.TipoLLave == Entidades.TipoLlave.Gratuita)
            {
                var tieneLlaveGratuita = await context.LlavesApi.AnyAsync(x => x.UsuarioId == usuarioId && x.TipoLlave == Entidades.TipoLlave.Gratuita);

                if(tieneLlaveGratuita)
                {
                    return BadRequest("El usuario ya tiene una llave gratuita");
                }
            }

            await servicioLlaves.CrearLlave(usuarioId, crearLlaveDTO.TipoLLave);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarLlave(ActualizarLlaveDTO actualizarLlaveDTO)
        {
            var usuarioId = ObtenerUsuarioId();

            var llaveDB = await context.LlavesApi.FirstOrDefaultAsync(x => x.Id == actualizarLlaveDTO.LlaveId);

            if(llaveDB == null)
            {
                return NotFound();
            }

            if(usuarioId != llaveDB.UsuarioId)
            {
                return Forbid();
            }

            if(actualizarLlaveDTO.ActualizarLlave)
            {
                llaveDB.Llave = servicioLlaves.GenerarLlave();
            }

            llaveDB.Activa = actualizarLlaveDTO.Activa;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
