using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Curso.Servicios.interfaces;
using Microsoft.Extensions.Configuration;

namespace Curso.Servicios
{
    public class ApiService : IApiService
    {
        private readonly string _ApiUrl;
        public ApiService(IConfiguration configuration)
        {
            _ApiUrl = configuration["UrlAPI"];
        }

        public string getApiUrl()
        {
            return _ApiUrl;
        }
    }
}
