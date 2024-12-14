using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using RestSharp;
using User_Report.Entities;



namespace User_Report.App_Code
{
    public class MaafinDbHelper
    {
        static void Main(string[] args)
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            var data = ExecutiveDirectQuery<string>("query", dict);

        }
        public static T ExecutiveDirectQuery<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
        {
            try
            {

                QueryData queryData = new QueryData()
                {
                    Query = sqlQuery,
                    Parameters = parameters
                };
                string qData = JsonConvert.SerializeObject(queryData);

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    // RequestUri = new Uri($"https://serv.mactech.net.in/MAAFIN_DB_API/query?Query={sqlQuery}"),
                    // RequestUri = new Uri($"http://localhost/Maafin_API_Test/query?Query={qData}"),
                    RequestUri = new Uri($"http://localhost/MaafinDB/query?Query={qData}"),  //me

                    Headers = { { "Accept", "application/json" } }
                };

                var response = client.SendAsync(request).Result;

                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;


                Console.WriteLine(responseBody);
                return JsonConvert.DeserializeObject<T>(responseBody.ToString());
            }

            catch (Exception ex)
            {
                throw;
            }


        }

        public static T ExecuteFormQueryData<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
        {
            try
            {
                QueryData queryData = new QueryData()
                {
                    Query = sqlQuery,
                    Parameters = parameters
                };
                string qData = JsonConvert.SerializeObject(queryData);

                //var client = new RestClient("https://serv.mactech.net.in/MAAFIN_DB_API/");
                //var client = new RestClient("http://localhost/Maafin_API_Test/");
                var client = new RestClient("http://10.5.101.253/MaafinDB/");   //me
                //var client = new RestClient("https://localhost:44321/");
                var request = new RestRequest("QueryAsync_form", Method.Post);

                request.AddParameter("application/json", qData, ParameterType.RequestBody);


                // request.AddJsonBody(queryData);
                var response = client.ExecuteAsync(request).Result;
                // var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Console.WriteLine("Request successful. Response:");
                    Console.WriteLine(response.Content);
                }
                else
                {
                    Console.WriteLine("Request failed. Error message:");
                    Console.WriteLine(response.ErrorMessage);
                }
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static T querysp<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    // RequestUri = new Uri($"https://serv.mactech.net.in/MAAFIN_DB_API/querysp?Query={sqlQuery}"),
                    // RequestUri = new Uri($"http://localhost/Maafin_API_Test/querysp?Query={sqlQuery}"),
                    RequestUri = new Uri($"http://localhost/MaafinDB/querysp?Query={sqlQuery}"),  //me
                    Headers = { { "Accept", "application/json" } }
                };

                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;


                Console.WriteLine(responseBody);
                return JsonConvert.DeserializeObject<T>(responseBody);
            }

            catch (Exception ex)
            {
                throw;
            }

        }


        public static T QueryStoredProcedure_form<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
        {
            try
            {
                sqlQuery = "testprocname";
                QueryData queryData = new QueryData()
                {
                    Query = sqlQuery,
                    Parameters = parameters
                };
                string qData = JsonConvert.SerializeObject(queryData);

                using (HttpClient client = new HttpClient())
                {
                    // string url = "https://serv.mactech.net.in/MAAFIN_DB_API/QueryStoredProcedure_form";
                    //string url = "http://localhost/Maafin_API_Test/QueryStoredProcedure_form";
                    string url = "http://localhost/MaafinDB/QueryStoredProcedure_form";  //me
                    StringContent content = new StringContent(qData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseContent);
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        internal static void ExecuteFormNonQuery(string sql, Dictionary<string, dynamic> parameters)
        {
            throw new NotImplementedException();
        }

        internal static LoginUser ExecuteFormQueryData<T>(object check, Dictionary<string, dynamic> parameter)
        {
            throw new NotImplementedException();
        }
    }

    public class QueryData
    {
        public string Query { get; set; }
        public Dictionary<string, dynamic> Parameters { get; set; }
    }
}







//---------------------------------------------------------------------Live-------------------------------------------------------------------------







//namespace User_Report.App_Code
//{
//    public class MaafinDbHelper
//    {
//        static void Main(string[] args)
//        {
//            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

//            var data = ExecutiveDirectQuery<string>("query", dict);

//        }
//        public static T ExecutiveDirectQuery<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
//        {
//            try
//            {

//                QueryData queryData = new QueryData()
//                {
//                    Query = sqlQuery,
//                    Parameters = parameters
//                };
//                string qData = JsonConvert.SerializeObject(queryData);

//                var client = new HttpClient();
//                var request = new HttpRequestMessage
//                {
//                    Method = HttpMethod.Post,
//                    RequestUri = new Uri($"https://serv.mactech.net.in/MAAFIN_DB_API/query?Query={sqlQuery}"),
//                    // RequestUri = new Uri($"http://localhost/Maafin_API_Test/query?Query={qData}"),
//                    //RequestUri = new Uri($"http://localhost/MaafinDB/query?Query={qData}"),  //me

//                    Headers = { { "Accept", "application/json" } }
//                };

//                var response = client.SendAsync(request).Result;

//                response.EnsureSuccessStatusCode();
//                var responseBody = response.Content.ReadAsStringAsync().Result;


//                Console.WriteLine(responseBody);
//                return JsonConvert.DeserializeObject<T>(responseBody.ToString());
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }


//        }

//        public static T ExecuteFormQueryData<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
//        {
//            try
//            {
//                QueryData queryData = new QueryData()
//                {
//                    Query = sqlQuery,
//                    Parameters = parameters
//                };
//                string qData = JsonConvert.SerializeObject(queryData);

//                var client = new RestClient("https://serv.mactech.net.in/MAAFIN_DB_API/");
//                //var client = new RestClient("http://localhost/Maafin_API_Test/");
//                //var client = new RestClient("http://localhost/MaafinDB/");   //me
//                //var client = new RestClient("https://localhost:44321/");
//                var request = new RestRequest("QueryAsync_form", Method.Post);

//                request.AddParameter("application/json", qData, ParameterType.RequestBody);


//                // request.AddJsonBody(queryData);
//                var response = client.ExecuteAsync(request).Result;
//                // var response = client.Execute(request);
//                if (response.IsSuccessful)
//                {
//                    Console.WriteLine("Request successful. Response:");
//                    Console.WriteLine(response.Content);
//                }
//                else
//                {
//                    Console.WriteLine("Request failed. Error message:");
//                    Console.WriteLine(response.ErrorMessage);
//                }
//                return JsonConvert.DeserializeObject<T>(response.Content);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public static T querysp<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
//        {
//            try
//            {
//                var client = new HttpClient();
//                var request = new HttpRequestMessage
//                {
//                    Method = HttpMethod.Post,
//                    RequestUri = new Uri($"https://serv.mactech.net.in/MAAFIN_DB_API/querysp?Query={sqlQuery}"),
//                    //RequestUri = new Uri($"http://localhost/Maafin_API_Test/querysp?Query={sqlQuery}"),
//                    //RequestUri = new Uri($"http://localhost/MaafinDB/querysp?Query={sqlQuery}"),  //me
//                    Headers = { { "Accept", "application/json" } }
//                };

//                var response = client.SendAsync(request).Result;
//                response.EnsureSuccessStatusCode();
//                var responseBody = response.Content.ReadAsStringAsync().Result;


//                Console.WriteLine(responseBody);
//                return JsonConvert.DeserializeObject<T>(responseBody);
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }

//        }


//        public static T QueryStoredProcedure_form<T>(string sqlQuery, Dictionary<string, dynamic> parameters)
//        {
//            try
//            {
//                sqlQuery = "testprocname";
//                QueryData queryData = new QueryData()
//                {
//                    Query = sqlQuery,
//                    Parameters = parameters
//                };
//                string qData = JsonConvert.SerializeObject(queryData);

//                using (HttpClient client = new HttpClient())
//                {
//                    string url = "https://serv.mactech.net.in/MAAFIN_DB_API/QueryStoredProcedure_form";
//                    //string url = "http://localhost/Maafin_API_Test/QueryStoredProcedure_form";
//                    //string url = "http://localhost/MaafinDB/QueryStoredProcedure_form";  //me
//                    StringContent content = new StringContent(qData, System.Text.Encoding.UTF8, "application/json");
//                    HttpResponseMessage response = client.PostAsync(url, content).Result;
//                    string responseContent = response.Content.ReadAsStringAsync().Result;
//                    Console.WriteLine(responseContent);
//                    return JsonConvert.DeserializeObject<T>(responseContent);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//        }

//        internal static void ExecuteFormNonQuery(string sql, Dictionary<string, dynamic> parameters)
//        {
//            throw new NotImplementedException();
//        }

//        internal static LoginUser ExecuteFormQueryData<T>(object check, Dictionary<string, dynamic> parameter)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class QueryData
//    {
//        public string Query { get; set; }
//        public Dictionary<string, dynamic> Parameters { get; set; }
//    }
//}