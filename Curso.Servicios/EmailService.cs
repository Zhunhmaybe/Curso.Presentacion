using AutoMapper.Configuration;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using MimeKit;
using Curso.Servicios.interfaces;

namespace Curso.Servicios
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(string Para,string Asunto,string Contenido)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(Para));
            email.Subject = Asunto;


            var body = new BodyBuilder
            {
                HtmlBody = Contenido
            };
            //var imagePath = Path.Combine(_env.WebRootPath, "img", "LogoBlanco.png");

            //// Verifica que el archivo exista
            //if (!File.Exists(imagePath))
            //{
            //    throw new FileNotFoundException("Imagen no encontrada", imagePath);
            //}

            //var image = (MimePart)body.LinkedResources.Add(imagePath);
            //image.ContentId = "LogoBlanco";
            //image.ContentTransferEncoding = ContentEncoding.Base64; // Ahora sí funciona

            email.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }


    }
}
