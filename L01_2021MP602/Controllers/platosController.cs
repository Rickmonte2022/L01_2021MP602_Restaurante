using L01_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace L01_2021MP602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly RestauranteDbContext _restaurantedb;

        public platosController(RestauranteDbContext restaurantedb)
        {
            _restaurantedb = restaurantedb;
        }

        //lectura
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Plato> listadoEquipo = (from e in _restaurantedb.Platos select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        //Agregar
        [HttpPost]
        [Route("Add")]


        public IActionResult GuardarPlato([FromBody] Plato platos)
        {
            try
            {
                _restaurantedb.Platos.Add(platos);
                _restaurantedb.SaveChanges();
                return Ok(platos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        //modificar
        [HttpPut]
        [Route("Actualizar/{id}")]

        public IActionResult actualizarPlato(int id, [FromBody] Plato platoModificar)
        {
            Plato? platoActual = (from e in _restaurantedb.Platos where e.PlatoId == id select e).FirstOrDefault();

            if (platoActual == null) { return NotFound(); }

            platoActual.NombrePlato = platoModificar.NombrePlato;
            platoActual.Precio = platoModificar.Precio;


            _restaurantedb.Entry(platoActual).State = EntityState.Modified;
            _restaurantedb.SaveChanges();

            return Ok(platoModificar);

        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {

            Plato? eliminarPlato = (from e in _restaurantedb.Platos where e.PlatoId == id select e).FirstOrDefault();

            if (eliminarPlato == null)
                return NotFound();

            _restaurantedb.Platos.Attach(eliminarPlato);
            _restaurantedb.Platos.Remove(eliminarPlato);
            _restaurantedb.SaveChanges();

            return Ok(eliminarPlato);
        }


        //filtradoporpalbra
        [HttpGet]
        [Route("filtradoporpala")]

        public IActionResult filtradopala(string palabra)
        {
            try
            {
                var platillos = _restaurantedb.Platos
                                .Where(p => p.NombrePlato.Contains(palabra))
                                .ToList();

                if (platillos.Count == 0)
                {
                    return NotFound();
                }

                return Ok(platillos);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
    }
}
