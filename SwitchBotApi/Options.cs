using CommandLine;

namespace SwitchBotApi
{
    public class Options
    {
        [Option('d', "deviceId", Required = true, HelpText = "デバイスIDを指定してください。")]
        public string DeviceId { get; set; }

        [Option('c', "command", Required = true, HelpText = "コマンドを指定してください。")]
        public string Command { get; set; }

        [Option('p', "parameter", Required = false, Default = "default", HelpText = "パラメータを指定してください。")]
        public string Parameter { get; set; }

        [Option('t', "commandType", Required = false, Default = "command", HelpText = "コマンドタイプを指定してください。")]
        public string CommandType { get; set; }
    }
}
