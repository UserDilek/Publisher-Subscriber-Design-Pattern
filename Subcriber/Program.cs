// See https://aka.ms/new-console-template for more information
using Subcriber.Dtos;
using System.Net.Http.Json;

Console.WriteLine("Press Esc to stop");

do
{

    HttpClient client = new HttpClient();
    Console.WriteLine("Listening....");
    while (!Console.KeyAvailable)
    {
        List<int> ackIds = await GetMessageAsync(client);
        Thread.Sleep(3000);
        if(ackIds.Count > 0) {
            await AckMessageAsync(client, ackIds);
        }
    }





} while (Console.ReadKey(true).Key != ConsoleKey.Escape);




static async Task<List<int>> GetMessageAsync(HttpClient httpClient) {

    List<int> ackIds = new List<int>();
    List<MessageReadDto>? newMessages = new List<MessageReadDto>();

    try
    {
        newMessages = await httpClient.GetFromJsonAsync<List<MessageReadDto>>("localhost");
             
    }
    catch (Exception ex)
    {
        return ackIds;

    }

    foreach (var msg in newMessages)
    {
        Console.WriteLine($"{msg.ID} - {msg.TopicMessage} - {msg.MessageStatus}");
        ackIds.Add(msg.ID);


    }
    return ackIds;

}

static async Task AckMessageAsync(HttpClient httpClient,List<int> ackIds) {

    var response = await httpClient.PostAsJsonAsync("https://localhost",ackIds);
    var returnMessage = await response.Content.ReadAsStringAsync();

    Console.WriteLine(returnMessage);


}