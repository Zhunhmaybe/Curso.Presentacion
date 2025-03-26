using Curso.Servicios.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Servicios
{
    public class NotificationService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IServiceProvider services,
            ILogger<NotificationService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<Curso.Data.Dbcontext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var recordatorios = await dbContext.Recordatorios
                        .Include(r => r.Tarea)
                        .Where(r => r.FechaEnvio <= DateTime.Now && !r.Enviado)
                        .ToListAsync(stoppingToken);

                    foreach (var recordatorio in recordatorios)
                    {
                        try
                        {
                            var mensaje = $"Recordatorio: {recordatorio.Tarea.Descripcion}";
                            emailService.SendEmail(
                                recordatorio.EmailUsuario,
                                "Recordatorio de tarea",
                                mensaje);

                            recordatorio.Enviado = true;
                            dbContext.Update(recordatorio);
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error enviando recordatorio ID: {Id}", recordatorio.RecordatorioID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el servicio de notificaciones");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Verifica cada minuto
            }
        }
    }
}
