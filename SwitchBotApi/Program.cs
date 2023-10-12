using Microsoft.Extensions.Configuration;
using SwitchBotApi;
using System.Text.Json;
using CommandLine;

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string token = configuration["token"];
string secret = configuration["secret"];

if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
{
    Console.WriteLine("tokenまたはsecretが指定されていません。");
    return;
}

Options? options = null;
Parser.Default.ParseArguments<Options>(args).WithParsed(parsedOptions =>
{
    options = parsedOptions;
});

if (options != null)
{
    string deviceId = options.DeviceId;
    var postData = new
    {
        command = options.Command,
        parameter = options.Parameter,
        commandType = options.CommandType
    };
    string json = JsonSerializer.Serialize(postData);

    try
    {
        await ApiHelper.SendCommandAsync(token, secret, options.DeviceId, json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"APIリクエストの送信中にエラーが発生しました: {ex.Message}");
    }
}
