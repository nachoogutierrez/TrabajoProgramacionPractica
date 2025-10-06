using System;
using System.Linq;
using System.Threading;
using DeliveryGO.Core.Command;
using DeliveryGO.Core.Strategy;
using DeliveryGO.Core.Order;
using DeliveryGO.Core.Order.Observers;
using DeliveryGO.Core.Facade;
using DeliveryGO.Core.Singleton;

class Program
{
    private static ICarritoPort _carrito;
    private static CheckoutFacade _facade;
    private static PedidoService _pedidoService;
    private static LogisticaObserver _logisticaObserver;
    private static bool _logisticaSuscripta = true;

    static void Main(string[] args)
    {
        Console.Title = "?? DeliveryGO - Sistema Integral";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("??????????????????????????????????????????????????????");
        Console.WriteLine("?          BIENVENIDO AL SISTEMA DELIVERYGO          ?");
        Console.WriteLine("??????????????????????????????????????????????????????");
        Console.ResetColor();
        Thread.Sleep(1000);

        InicializarSistema();
        MostrarMenuPrincipal();
    }

    static void InicializarSistema()
    {
        ConfigManager.Instance.EnvioGratisDesde = 50000m;
        ConfigManager.Instance.IVA = 0.21m;

        _carrito = new CarritoPort();
        _pedidoService = new PedidoService();

        var clienteObserver = new ClienteObserver("Cliente Demo");
        _logisticaObserver = new LogisticaObserver();
        var auditoriaObserver = new AuditoriaObserver();

        clienteObserver.Suscribir(_pedidoService);
        _logisticaObserver.Suscribir(_pedidoService);
        auditoriaObserver.Suscribir(_pedidoService);

        _facade = new CheckoutFacade(_carrito, new EnvioMoto(), _pedidoService);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nSistema inicializado correctamente ?");
        Console.ResetColor();
        Console.WriteLine($"?? Envío gratis desde: ${ConfigManager.Instance.EnvioGratisDesde}");
        Console.WriteLine($"?? IVA actual: {ConfigManager.Instance.IVA:P2}\n");
        Thread.Sleep(800);
    }

    static void MostrarMenuPrincipal()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("?????????????????????????????????????????????????");
            Console.WriteLine("?          ???  MENÚ PRINCIPAL DELIVERYGO          ?");
            Console.WriteLine("?????????????????????????????????????????????????");
            Console.ResetColor();

            Console.WriteLine("1??  Agregar ítem al carrito");
            Console.WriteLine("2??  Cambiar cantidad de ítem");
            Console.WriteLine("3??  Quitar ítem del carrito");
            Console.WriteLine("4??  Ver resumen de compra");
            Console.WriteLine("5??  Deshacer (Undo)");
            Console.WriteLine("6??  Rehacer (Redo)");
            Console.WriteLine("7??  Elegir método de envío");
            Console.WriteLine("8??  Realizar pago");
            Console.WriteLine("9??  Confirmar pedido");
            Console.WriteLine($"??  {(_logisticaSuscripta ? "Desuscribir" : "Suscribir")} Logística");
            Console.WriteLine("0??  Salir");
            Console.WriteLine("???????????????????????????????????????????????");

            var opcion = PedirEntero("Seleccione una opción: ");
            Console.WriteLine();

            try
            {
                switch (opcion)
                {
                    case 1: AgregarItem(); break;
                    case 2: CambiarCantidad(); break;
                    case 3: QuitarItem(); break;
                    case 4: MostrarResumen(); break;
                    case 5: Undo(); break;
                    case 6: Redo(); break;
                    case 7: ElegirEnvio(); break;
                    case 8: RealizarPago(); break;
                    case 9: ConfirmarPedido(); break;
                    case 10: ToggleLogistica(); break;
                    case 0:
                        MostrarDespedida();
                        return;
                    default:
                        MostrarError("Opción no válida");
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }

    static void AgregarItem()
    {
        Titulo("?? AGREGAR ÍTEM");
        var sku = PedirTexto("SKU: ");
        var nombre = PedirTexto("Nombre: ");
        var precio = PedirDecimal("Precio: ");
        var cantidad = PedirEntero("Cantidad: ");

        if (precio <= 0 || cantidad <= 0)
        {
            MostrarError("Precio y cantidad deben ser mayores a 0");
            return;
        }

        _facade.AgregarItem(sku, nombre, precio, cantidad);
        MostrarExito($"Ítem '{nombre}' agregado correctamente");
    }

    static void CambiarCantidad()
    {
        Titulo("?? CAMBIAR CANTIDAD");
        var sku = PedirTexto("SKU del ítem: ");
        var nuevaCantidad = PedirEntero("Nueva cantidad: ");
        if (nuevaCantidad <= 0)
        {
            MostrarError("La cantidad debe ser mayor a 0");
            return;
        }

        _facade.CambiarCantidad(sku, nuevaCantidad);
        MostrarExito($"Cantidad del ítem {sku} cambiada a {nuevaCantidad}");
    }

    static void QuitarItem()
    {
        Titulo("???  QUITAR ÍTEM");
        var sku = PedirTexto("SKU del ítem a quitar: ");
        _facade.QuitarItem(sku);
        MostrarExito($"Ítem {sku} quitado del carrito");
    }

    static void MostrarResumen()
    {
        Titulo("?? RESUMEN DE COMPRA");
        var items = _carrito.GetItemsSnapshot();
        if (!items.Any())
        {
            Console.WriteLine("Carrito vacío ??");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Productos en carrito:");
        Console.ResetColor();

        foreach (var item in items)
        {
            Console.WriteLine($"  • {item.Nombre} (x{item.Cantidad}) - ${item.Precio * item.Cantidad}");
        }

        var subtotal = _carrito.Subtotal();
        var total = _facade.CalcularTotal();
        var costoEnvio = total - subtotal;

        Console.WriteLine($"\nSubtotal: ${subtotal}");
        Console.WriteLine($"Costo de envío: ${costoEnvio}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"TOTAL: ${total}");
        Console.ResetColor();
    }

    static void Undo()
    {
        _carrito.Undo();
        MostrarExito("Operación deshecha (Undo)");
    }

    static void Redo()
    {
        _carrito.Redo();
        MostrarExito("Operación rehecha (Redo)");
    }

    static void ElegirEnvio()
    {
        Titulo("?? ELEGIR MÉTODO DE ENVÍO");
        Console.WriteLine("1?? Moto ($1280)");
        Console.WriteLine("2?? Correo ($3580 - Gratis desde $50000)");
        Console.WriteLine("3?? Retiro en tienda (Gratis)");

        var opcion = PedirEntero("Seleccione método: ");
        IEnvioStrategy estrategia = opcion switch
        {
            1 => new EnvioMoto(),
            2 => new EnvioCorreo(),
            3 => new RetiroEnTienda(),
            _ => throw new ArgumentException("Opción no válida")
        };

        _facade.ElegirEnvio(estrategia);
        MostrarExito($"Método de envío cambiado a: {estrategia.Nombre}");
    }

    static void RealizarPago()
    {
        Titulo("?? REALIZAR PAGO");
        Console.WriteLine("1?? Tarjeta");
        Console.WriteLine("2?? Transferencia");
        Console.WriteLine("3?? Mercado Pago");
        Console.WriteLine("4?? Mercado Pago (Adapter)");

        var opcionPago = PedirEntero("Seleccione método de pago: ");
        var tipoPago = opcionPago switch
        {
            1 => "tarjeta",
            2 => "transf",
            3 => "mp",
            4 => "mp-adapter",
            _ => throw new ArgumentException("Método de pago no válido")
        };

        var aplicarIVA = PedirSiNo("¿Aplicar IVA? (s/n): ");
        var aplicarCupon = PedirSiNo("¿Aplicar cupón de descuento? (s/n): ");
        decimal? cupon = null;

        if (aplicarCupon)
        {
            cupon = PedirDecimal("Porcentaje de descuento (ej: 0.10 para 10%): ");
            if (cupon <= 0 || cupon >= 1)
            {
                MostrarError("El cupón debe estar entre 0 y 1 (ej: 0.10 para 10%)");
                return;
            }
        }

        var exito = _facade.Pagar(tipoPago, aplicarIVA, cupon);
        if (exito)
            MostrarExito("¡Pago aprobado exitosamente!");
        else
            MostrarError("El pago fue rechazado");
    }

    static void ConfirmarPedido()
    {
        Titulo("?? CONFIRMAR PEDIDO");
        var items = _carrito.GetItemsSnapshot();
        if (!items.Any())
        {
            MostrarError("No se puede confirmar pedido: carrito vacío");
            return;
        }

        var direccion = PedirTexto("Dirección de entrega: ");
        var tipoPagoRegistro = PedirTexto("Tipo de pago a registrar: ");
        Console.WriteLine("Confirmando pedido...");
        var pedido = _facade.ConfirmarPedido(direccion, tipoPagoRegistro);

        MostrarExito($"Pedido #{pedido.Id} confirmado exitosamente!");
        Console.WriteLine($"Estado final: {pedido.Estado}");
        Console.WriteLine($"Dirección: {pedido.Direccion}");
        Console.WriteLine($"Total: ${pedido.Monto}");
    }

    static void ToggleLogistica()
    {
        if (_logisticaSuscripta)
        {
            _logisticaObserver.Desuscribir(_pedidoService);
            _logisticaSuscripta = false;
            MostrarExito("Logística desuscrita - no recibirá más notificaciones");
        }
        else
        {
            _logisticaObserver.Suscribir(_pedidoService);
            _logisticaSuscripta = true;
            MostrarExito("Logística suscrita - recibirá notificaciones");
        }
    }

    static void MostrarDespedida()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nGracias por usar DeliveryGO ??");
        Console.WriteLine("¡Hasta la próxima entrega!");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    #region Helpers
    static void Titulo(string texto)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"\n??? {texto} ???\n");
        Console.ResetColor();
    }

    static string PedirTexto(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine() ?? "";
    }

    static int PedirEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int resultado))
                return resultado;
            MostrarError("Ingrese un número válido.");
        }
    }

    static decimal PedirDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (decimal.TryParse(Console.ReadLine(), out decimal resultado))
                return resultado;
            MostrarError("Ingrese un número decimal válido.");
        }
    }

    static bool PedirSiNo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var resp = Console.ReadLine()?.Trim().ToLower();
            if (resp == "s" || resp == "si") return true;
            if (resp == "n" || resp == "no") return false;
            MostrarError("Ingrese 's' o 'n'.");
        }
    }

    static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"? {mensaje}");
        Console.ResetColor();
    }

    static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"? {mensaje}");
        Console.ResetColor();
    }
    #endregion
}
