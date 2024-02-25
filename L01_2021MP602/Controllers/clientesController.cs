using L01_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace L01_2021MP602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientesController : ControllerBase
    {
        private readonly RestauranteDbContext _restaurantedb;

        public clientesController(RestauranteDbContext restaurantedb)
        {
            _restaurantedb = restaurantedb;
        }

        //lectura
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Cliente> listadoEquipo = (from e in _restaurantedb.Clientes select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        //Agregar
        [HttpPost]
        [Route("Add")]


        public IActionResult GuardarCliente([FromBody] Cliente cliente)
        {
            try
            {
                _restaurantedb.Clientes.Add(cliente);
                _restaurantedb.SaveChanges();
                return Ok(cliente);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        //modificar
        [HttpPut]
        [Route("Actualizar/{id}")]

        public IActionResult actualizarCliente(int id, [FromBody] Cliente clienteModificar)
        {
            Cliente? clienteActual = (from e in _restaurantedb.Clientes where e.ClienteId == id select e).FirstOrDefault();

            if ( clienteActual== null) { return NotFound(); }

            clienteActual.NombreCliente = clienteActual.NombreCliente;
            clienteActual.Direccion = clienteActual.Direccion;
            


            _restaurantedb.Entry(clienteActual).State = EntityState.Modified;
            _restaurantedb.SaveChanges();

            return Ok(clienteModificar);

        }

        //borrar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCliente(int id)
        {

            Cliente? eliminarCliente = (from e in _restaurantedb.Clientes where e.ClienteId == id select e).FirstOrDefault();

            if (eliminarCliente == null)
                return NotFound();

            _restaurantedb.Clientes.Attach(eliminarCliente);
            _restaurantedb.Clientes.Remove(eliminarCliente);
            _restaurantedb.SaveChanges();

            return Ok(eliminarCliente);
        }
        
        //filtradodirec
        [HttpGet]
        [Route("filtradirec")]

        public IActionResult filtradopala(string palabra)
        {
            try
            {
                var clientes = _restaurantedb.Clientes
                                .Where(p => p.Direccion.Contains(palabra))
                                .ToList();

                if (clientes.Count == 0)
                {
                    return NotFound();
                }

                return Ok(clientes);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

    }
}
