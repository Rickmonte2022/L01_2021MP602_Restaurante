using L01_2021MP602.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace L01_2021MP602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly RestauranteDbContext _restaurantedb;

        public pedidosController(RestauranteDbContext restaurantedb)
        {
            _restaurantedb = restaurantedb;
        }

        //lectura
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Pedido> listadoEquipo = (from e in _restaurantedb.Pedidos select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        //Agregar
        [HttpPost]
        [Route("Add")]


        public IActionResult GuardarPedido([FromBody] Pedido pedido)
        {
            try
            {
                _restaurantedb.Pedidos.Add(pedido);
                _restaurantedb.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        //modificar
        [HttpPut]
        [Route("Actualizar/{id}")]

        public IActionResult actualizarPedido(int id, [FromBody] Pedido pedidoModificar)
        {
            Pedido? pedidoActual = (from e in _restaurantedb.Pedidos where e.PedidoId == id select e).FirstOrDefault();

            if (pedidoActual == null) { return NotFound(); }

            pedidoActual.MotoristaId = pedidoModificar.MotoristaId;
            pedidoActual.ClienteId = pedidoModificar.ClienteId;
            pedidoActual.PlatoId = pedidoModificar.PlatoId;
            pedidoActual.Cantidad = pedidoModificar.Cantidad;
            pedidoActual.Precio = pedidoModificar.Precio;


            _restaurantedb.Entry(pedidoActual).State = EntityState.Modified;
            _restaurantedb.SaveChanges();

            return Ok(pedidoModificar);

        }

        //borrar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPedidos(int id)
        {
            
            Pedido? eliminarPedidos = (from e in _restaurantedb.Pedidos where e.PedidoId==id select e).FirstOrDefault();

            if(eliminarPedidos == null)
                return NotFound();

            _restaurantedb.Pedidos.Attach(eliminarPedidos);
            _restaurantedb.Pedidos.Remove(eliminarPedidos);
            _restaurantedb.SaveChanges();

            return Ok(eliminarPedidos);


        }

        //filtroporcliente

        

        //pedidos de clientes
        [HttpGet]
        [Route("GetByCliente/{id}")]
        public IActionResult Get(int id)
        {
            List<Pedido> pedidos = (from e in _restaurantedb.Pedidos where e.ClienteId == id select e).ToList();
            if (pedidos == null)
            {
                return NotFound();
            }
            return Ok(pedidos);

        }


        //envios de motoristas
        [HttpGet]
        [Route("GetByMotorista/{id}")]
        public IActionResult GetMoto(int id)
        {
            List<Pedido> pedidos = (from e in _restaurantedb.Pedidos where e.MotoristaId == id select e).ToList();
            if (pedidos == null)
            {
                return NotFound();
            }
            return Ok(pedidos);

        }

        

    }
}
