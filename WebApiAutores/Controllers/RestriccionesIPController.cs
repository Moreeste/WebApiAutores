﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/restriccionesip")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RestriccionesIPController : CustomBaseController
    {
        private readonly ApplicationDbContext context;

        public RestriccionesIPController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearRestriccionesIPDTO crearRestriccionesIPDTO)
        {
            var llaveDB = await context.LlavesApi.FirstOrDefaultAsync(x => x.Id == crearRestriccionesIPDTO.LlaveId);

            if (llaveDB == null)
            {
                return NotFound();
            }

            var usuarioId = ObtenerUsuarioId();

            if (llaveDB.UsuarioId != usuarioId)
            {
                return Forbid();
            }

            var restriccionDominio = new RestriccionIP()
            {
                LlaveId = crearRestriccionesIPDTO.LlaveId,
                IP = crearRestriccionesIPDTO.IP
            };

            context.Add(restriccionDominio);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ActualizarRestriccionIPDTO actualizarRestriccionIPDTO)
        {
            var restriccionDB = await context.RestriccionesIP.Include(x => x.Llave)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restriccionDB == null)
            {
                return NotFound();
            }

            var usuarioId = ObtenerUsuarioId();

            if (restriccionDB.Llave.UsuarioId != usuarioId)
            {
                return Forbid();

            }

            restriccionDB.IP = actualizarRestriccionIPDTO.IP;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var restriccionDB = await context.RestriccionesIP.Include(x => x.Llave)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restriccionDB == null)
            {
                return NotFound();
            }

            var usuarioId = ObtenerUsuarioId();

            if (restriccionDB.Llave.UsuarioId != usuarioId)
            {
                return Forbid();
            }

            context.Remove(restriccionDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
