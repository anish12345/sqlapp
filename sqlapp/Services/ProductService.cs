using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using sqlapp.Models;
using System.Data.SqlClient;

namespace sqlapp.Services
{

    // This service will interact with our Product data in the SQL database
    public class ProductService
    {
        

        private SqlConnection GetConnection()
        {
            string tenantId = "dd393912-1b31-48f1-b206-c7273175d5b8";
            string clientId = "5350486d-d5fa-4e9d-86b3-54d1a0ce9ce2";
            string clientSecret = "MhU8Q~Yt6bm8~IrhUYCUgXuhM7zXe-_b7hkrhaVa";

            string keyvaultUrl = "https://anishkeyvault786.vault.azure.net/";
            string secretName = "dbconnectionstring";
            
            ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            SecretClient secretClient = new SecretClient(new Uri(keyvaultUrl), clientSecretCredential);

            var secret = secretClient.GetSecret(secretName);

            string connectionString = secret.Value.Value;


            return new SqlConnection(connectionString);
        }
        public List<Product> GetProducts()
        {
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = GetConnection();
            
            _connection.Open();
            
            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);
            
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }

    }
}

