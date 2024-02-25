using System;
using System.Collections.Generic;

namespace L01_2021MP602.Models;

public partial class Pedido
{
    public int PedidoId { get; set; }

    public int? MotoristaId { get; set; }

    public int? ClienteId { get; set; }

    public int? PlatoId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }
}
