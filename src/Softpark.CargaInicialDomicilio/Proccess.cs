using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Softpark.CargaInicialDomicilio
{
    partial class Program
    {
        static async Task Importar()
        {
            //var bairroRegExp = new Regex

            var enderecos = _db.ASSMED_Endereco.GroupBy(x => new { Bairro = Regex.Replace(x.Bairro, "", ""), x.CEP, x.CodCidade, x.CodTpLogra, x.Logradouro, x.Numero, x.Complemento });

            
        }
    }
}
