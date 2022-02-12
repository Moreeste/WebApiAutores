using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorDTO>();

            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(x => x.Libros, op => op.MapFrom(MapAutorDTOLibros));

            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.AutoresLibros, op => op.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>();

            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(x => x.Autores, op => op.MapFrom(MapLibroDTOAutores));

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();

            CreateMap<Comentario, ComentarioDTO>();

            CreateMap<LlaveApi, LlaveDTO>();

            CreateMap<RestriccionDominio, RestriccionDominioDTO>();

            CreateMap<RestriccionIP, RestriccionIPDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros (Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null) { return resultado; }

            foreach (var autoresLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autoresLibro.LibroId,
                    Titulo = autoresLibro.Libro.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if(libro.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro ibro)
        {
            var resultado = new List<AutorLibro>();

            if(libroCreacionDTO.AutoresIds == null) { return resultado; }

            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }
    }
}
