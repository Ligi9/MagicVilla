using MagicVilla_API.Modelo;

namespace MagicVilla_API.Repositorio.IRepositorio;

public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
{
    Task<NumeroVilla> Actualizar(NumeroVilla entidad);
}
