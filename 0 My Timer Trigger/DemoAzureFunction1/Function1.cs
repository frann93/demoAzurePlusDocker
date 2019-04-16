using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DemoAzureFunction1
{
    public static class Function1
    {
        [FunctionName("DeleteCheckOut")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

            using (SqlConnection conec = new SqlConnection(str))
            {
                conec.Open();
                var consulta = "UPDATE dboDemo " +
                               "SET stat = 0 " +
                               "WHERE order_date < GetDate() - 5;";
                using (SqlCommand cmd = new SqlCommand(consulta, conec))
                {
                    //ejecutar y mostrar 
                    var filas = await cmd.ExecuteNonQueryAsync();
                    log.Info($"{filas} filas han sido actualizadas");
                }
            }
        }
    }
}
