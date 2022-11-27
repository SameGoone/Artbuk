using System.Net.Http;
using System.Linq;
using System.Net;
using System;
using System.Threading;
using System.Diagnostics;

public class DDOS
{
    private static string _port;

    public static async Task Main()
    {
        try
        {
            Console.Write("Введите порт приложения: ");
            _port = Console.ReadLine();

            Console.Write("Введите кол-во клиентов: ");
            var countOfClients = int.Parse(Console.ReadLine());

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < countOfClients; i++)
            {
                tasks.Add(Simulate($"user{i}"));
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            await Task.WhenAll(tasks);
            stopWatch.Stop();

            Console.WriteLine($"Выполнение для {countOfClients} клиентов заняло {stopWatch.ElapsedMilliseconds} милисекунд.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static async Task Simulate(string login)
    {
        var cookieContainer = new CookieContainer();
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = cookieContainer
        };

        var httpClient = new HttpClient(httpClientHandler);
        httpClient.BaseAddress = new Uri($"https://localhost:{_port}");

        (_, var password, var email) = GenerateAuthData(login);
        await Registration(httpClient, login, password, email);

        await CreatePost(httpClient, $"Body for post. {login}", "ec4e70b4-0dcb-40cf-9a31-08dac86d3527", "c51c0d28-4b6d-4839-53bf-08dac86d3541");
    }

    static (string, string, string) GenerateAuthData(string input)
    {
        return (input, input, input + "@mail.ru");
    }

    static async Task Registration(HttpClient httpClient, string login, string password, string email)
    {
        var content = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Login", login),
                new KeyValuePair<string, string>("Password", password),
            });
        var result = await httpClient.PostAsync("Profile/Registration", content);
    }

    static async Task CreatePost(HttpClient httpClient, string body, string genreId, string softwareId)
    {
        var content = new FormUrlEncodedContent(new[]
        {
                    new KeyValuePair<string, string>("Body", body),
                    new KeyValuePair<string, string>("GenreId", genreId.ToString()),
                    new KeyValuePair<string, string>("SoftwareId", softwareId.ToString()),
            });

        var result = await httpClient.PostAsync("Feed/CreatePost", content);
    }
}