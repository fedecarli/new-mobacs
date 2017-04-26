using Softpark.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Softpark.CargaInicialDomicilio
{
    partial class Program
    {
        private static DomainContainer _db;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Bem vindo à carga inicial de dados para cadastro domiciliar.");
            Console.WriteLine();
            Console.WriteLine("Escolha abaixo qual configuração deseja executar.");
            Console.WriteLine();
            Console.WriteLine("[1]: Serviço Local");
            Console.WriteLine("[2]: Conexão com Santana de Parnaíba");
            Console.WriteLine("[3]: Customizado");
            Console.WriteLine("[q]: Sair do programa");

            Console.Write("Opção [1]:> ");
            char key;

            while (!(new[] { '1', '2', '3', 'q', 'Q' }).Contains((key = Console.ReadKey(true).KeyChar)));

            Console.Write(key);
            Console.WriteLine();
            Console.WriteLine();

            switch (key)
            {
                case '1':
                    Task.Run(() => Local());
                    break;
                case '2':
                    Task.Run(() => Santana());
                    break;
                case '3':
                    try
                    {
                        Console.WriteLine("Forneça a string de conexão com o banco de dados do SQL Server:");
                        Task.Run(() => Custom(Console.ReadLine()));
                    }
                    catch
                    {
                        Main(args);
                    }
                    break;
            }

            Console.Clear();
        }

        private static async Task Local()
        {
            _db = DomainContainer.Current;

            await Importar();
        }

        private static async Task Santana()
        {
            _db = new DomainContainer("name=DomainContainerSantana");

            await Importar();
        }

        private static async Task Custom(string connectionString)
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["DomainContainerSantana"].ConnectionString, connectionString);

            _db = new DomainContainer(connectionString);

            await Importar();
        }
    }
}
