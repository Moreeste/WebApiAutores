using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.Entidades;

namespace WebApiAutores.Servicios
{
    public class ServicioLlaves
    {
        private readonly ApplicationDbContext context;

        public ServicioLlaves(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CrearLlave(string usuarioId, TipoLlave tipoLlave)
        {
            var llave = GenerarLlave();

            var llaveApi = new LlaveApi
            {
                Activa = true,
                Llave = llave,
                TipoLlave = tipoLlave,
                UsuarioId = usuarioId
            };

            context.Add(llaveApi);
            await context.SaveChangesAsync();
        }

        public string GenerarLlave()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
