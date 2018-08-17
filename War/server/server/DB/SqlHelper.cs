using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace server.DB
{
    class SqlHelper
    {
        /// <summary>
        /// 执行sql语句，有事物时（存储过程）
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(SqlTransaction transaction,CommandType commandType,string commandText, SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentNullException("The transcation was rollbacked or committed");
            }
            SqlCommand command = new SqlCommand();
            PrepareCommand(command,transaction.Connection ,transaction, commandType, commandText, commandParameters);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }
        /// <summary>
        /// 执行sql语句，无事物时（存储过程）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string connectionString, CommandType commandType, string commandText,  SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                if (sqlConnection == null)
                {
                    throw new ArgumentNullException("sqlConnection");
                }
                SqlCommand command = new SqlCommand();
                PrepareCommand(command, sqlConnection, null, commandType, commandText, commandParameters);
                int num = command.ExecuteNonQuery();
                command.Parameters.Clear();
                return num;
            }
        }

        /// <summary>
        /// 准备command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        private static void PrepareCommand(SqlCommand command,SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText,  SqlParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandText == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                PrepareParameters(command,commandParameters);
            }
        }

        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        private static void PrepareParameters(SqlCommand command,SqlParameter[] commandParameters)
        {
            foreach (SqlParameter parameter in commandParameters)
            {
                if (parameter != null)
                {
                    if (parameter.Value == null&&parameter.Direction==ParameterDirection.Input)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
}
