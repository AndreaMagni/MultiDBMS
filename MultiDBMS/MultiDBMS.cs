using System;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace MultiDBMS
{
    public class MultiDBMS
    {
        DataTable dataTable = new DataTable();

        public string GetJson(string databaseType, string connectionString, string query)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type classType = assembly.GetType("MultiDBMS.DBMS." + databaseType, true);
            object classInstance = Activator.CreateInstance(classType, connectionString);
            bool openConnection = (bool)classType.InvokeMember("OpenConnection", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, classInstance, null);

            if (openConnection)
            {
                classType.InvokeMember("BeginTransaction", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, classInstance, null);
                // TODO: dataTable = (DataTable)classType.InvokeMember("FillDataTable", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, classInstance, new object[] { query });
                classType.InvokeMember("CommitTransaction", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, classInstance, null);
            }
            else
            {
                throw new ApplicationException("La connessione al DB non è stata aperta correttamente"); // TODO: Gestire meglio le eccezioni con classi apposite
            }

            bool closeConnection = (bool)classType.InvokeMember("CloseConnection", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, classInstance, null);

            if (!closeConnection)
            {
                throw new ApplicationException("La connessione al DB non è stata chiusa correttamente"); // TODO: Gestire meglio le eccezioni con classi apposite
            }

            return JsonConvert.SerializeObject(dataTable);
        }

    }
}
