var http = new HttpClient();
var tasks = new List<Task>();

for (int i = 0; i < 150; i++)
{
    tasks.Add(Task.Run(async () =>
    {
        var response = await http.GetAsync("https://localhost:7062/api/products-mn");
        Console.WriteLine($"{response.StatusCode}");
    }));
}

await Task.WhenAll(tasks);