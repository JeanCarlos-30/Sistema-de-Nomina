using System;

class Program
{
    static void Main()
    {
        List<Empleado> empleados = new List<Empleado>
        {
            new EmpleadoAsalariado {
                PrimerNombre = "Jeicol", ApellidoPaterno = "Ramirez", NumeroSeguroSocial = "123",
                SalarioSemanal = 500m
            },
            new EmpleadoPorHoras {
                PrimerNombre = "Alexander", ApellidoPaterno = "Beriguete", NumeroSeguroSocial = "456",
                SueldoPorHora = 20m, HorasTrabajadas = 45m
            },
            new EmpleadoPorComision {
                PrimerNombre = "Carlos", ApellidoPaterno = "Terrero", NumeroSeguroSocial = "789",
                VentasBrutas = 10000m, TarifaComision = 0.06m
            },
            new EmpleadoAsalariadoPorComision {
                PrimerNombre = "Lucía", ApellidoPaterno = "Martínez", NumeroSeguroSocial = "101",
                VentasBrutas = 8000m, TarifaComision = 0.05m, SalarioBase = 300m
            }
        };

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE EMPLEADOS ===");
            Console.WriteLine("1. Ver empleados");
            Console.WriteLine("2. Actualizar empleado");
            Console.WriteLine("3. Ver reporte semanal");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("\nListado de empleados:");
                    for (int i = 0; i < empleados.Count; i++)
                    {
                        var emp = empleados[i];
                        Console.WriteLine($"{i + 1}. {emp.PrimerNombre} {emp.ApellidoPaterno}");
                    }
                    break;

                case "2":
                    Console.WriteLine("\nSeleccione un empleado para actualizar:");
                    for (int i = 0; i < empleados.Count; i++)
                    {
                        var emp = empleados[i];
                        Console.WriteLine($"{i + 1}. {emp.PrimerNombre} {emp.ApellidoPaterno}");
                    }
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= empleados.Count)
                    {
                        empleados[idx - 1].ActualizarDatos();
                        Console.WriteLine("Datos actualizados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Selección inválida.");
                    }
                    break;

                case "3":
                    Console.WriteLine("\n=== REPORTE SEMANAL ===");
                    foreach (var emp in empleados)
                    {
                        Console.WriteLine(emp.GenerarDetallePago());
                    }
                    break;

                case "4":
                    Console.WriteLine("Saliendo...");
                    return;

                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine("\nPresione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}

abstract class Empleado
{
    public string PrimerNombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string NumeroSeguroSocial { get; set; }

    public abstract decimal CalcularPagoSemanal();
    public abstract void ActualizarDatos();
    public abstract string GenerarDetallePago();
}

class EmpleadoAsalariado : Empleado
{
    public decimal SalarioSemanal { get; set; }

    public override decimal CalcularPagoSemanal() => SalarioSemanal;

    public override void ActualizarDatos()
    {
        Console.WriteLine($"Actualizando Empleado Asalariado: {PrimerNombre} {ApellidoPaterno}");
        Console.Write("Nuevo salario semanal: ");

        if (decimal.TryParse(Console.ReadLine(), out var nuevoSalario))
            SalarioSemanal = nuevoSalario;
    }

    public override string GenerarDetallePago()
    {
        return $"{PrimerNombre} {ApellidoPaterno} (Asalariado): Salario fijo = {SalarioSemanal:C}";
    }
}

class EmpleadoPorHoras : Empleado
{
    public decimal SueldoPorHora { get; set; }
    public decimal HorasTrabajadas { get; set; }

    public override decimal CalcularPagoSemanal()
    {
        if (HorasTrabajadas <= 40)
            return SueldoPorHora * HorasTrabajadas;
        else
            return (SueldoPorHora * 40) + (SueldoPorHora * 1.5m * (HorasTrabajadas - 40));
    }

    public override void ActualizarDatos()
    {
        Console.WriteLine($"Actualizando Empleado Por Horas: {PrimerNombre} {ApellidoPaterno}");
        Console.Write("Nuevo sueldo por hora: ");

        if (decimal.TryParse(Console.ReadLine(), out var nuevoSueldo))
            SueldoPorHora = nuevoSueldo;

        Console.Write("Nuevas horas trabajadas: ");
        if (decimal.TryParse(Console.ReadLine(), out var nuevasHoras))
            HorasTrabajadas = nuevasHoras;
    }

    public override string GenerarDetallePago()
    {
        var pago = CalcularPagoSemanal();

        string detalle = HorasTrabajadas <= 40
            ? $"{PrimerNombre} {ApellidoPaterno} (Por Horas): {HorasTrabajadas}h x {SueldoPorHora:C} = {pago:C}"
            : $"{PrimerNombre} {ApellidoPaterno} (Por Horas): 40h x {SueldoPorHora:C} + {HorasTrabajadas - 40}h x {(SueldoPorHora * 1.5m):C} = {pago:C}";
        return detalle;
    }
}

class EmpleadoPorComision : Empleado
{
    public decimal VentasBrutas { get; set; }
    public decimal TarifaComision { get; set; }

    public override decimal CalcularPagoSemanal() => VentasBrutas * TarifaComision;

    public override void ActualizarDatos()
    {
        Console.WriteLine($"Actualizando Empleado Por Comisión: {PrimerNombre} {ApellidoPaterno}");
        Console.Write("Nuevas ventas brutas: ");

        if (decimal.TryParse(Console.ReadLine(), out var ventas))
            VentasBrutas = ventas;

        Console.Write("Nueva tarifa de comisión (ej. 0.06): ");
        if (decimal.TryParse(Console.ReadLine(), out var tarifa))
            TarifaComision = tarifa;
    }

    public override string GenerarDetallePago()
    {
        var pago = CalcularPagoSemanal();
        return $"{PrimerNombre} {ApellidoPaterno} (Por Comisión): {VentasBrutas:C} x {TarifaComision:P} = {pago:C}";
    }
}

class EmpleadoAsalariadoPorComision : Empleado
{
    public decimal VentasBrutas { get; set; }
    public decimal TarifaComision { get; set; }
    public decimal SalarioBase { get; set; }

    public override decimal CalcularPagoSemanal()
    {
        return (VentasBrutas * TarifaComision) + SalarioBase + (SalarioBase * 0.10m);
    }

    public override void ActualizarDatos()
    {
        Console.WriteLine($"Actualizando Empleado Asalariado Por Comisión: {PrimerNombre} {ApellidoPaterno}");
        Console.Write("Nuevas ventas brutas: ");

        if (decimal.TryParse(Console.ReadLine(), out var ventas))
            VentasBrutas = ventas;

        Console.Write("Nueva tarifa de comisión (ej. 0.05): ");

        if (decimal.TryParse(Console.ReadLine(), out var tarifa))
            TarifaComision = tarifa;

        Console.Write("Nuevo salario base: ");

        if (decimal.TryParse(Console.ReadLine(), out var salario))
            SalarioBase = salario;
    }

    public override string GenerarDetallePago()
    {
        var pago = CalcularPagoSemanal();
        return $"{PrimerNombre} {ApellidoPaterno} (Asalariado + Comisión): ({VentasBrutas:C} x {TarifaComision:P}) + {SalarioBase:C} + 10% extra = {pago:C}";
    }
}
