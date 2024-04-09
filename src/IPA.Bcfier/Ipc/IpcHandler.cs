using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace IPA.Bcfier.Ipc
{
    public class IpcHandler : IDisposable
    {
        private readonly string _thisAppName;
        private readonly string _otherAppName;
        private bool _isDisposed = false;

        public static ConcurrentQueue<string> ReceivedMessages { get; } = new ConcurrentQueue<string>();

        public IpcHandler(string thisAppName,
            string otherAppName)
        {
            _thisAppName = thisAppName;
            _otherAppName = otherAppName;
        }

        public Task InitializeAsync()
        {
            Task.Run(async () => await InitializeServer());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public async Task SendMessageAsync(string message, int? timeout = null)
        {
            using var namedPipeClientStream = new NamedPipeClientStream(".", $"{_otherAppName}ServerPipe", PipeDirection.InOut, PipeOptions.None);
            if (timeout != null)
            {
                await namedPipeClientStream.ConnectAsync(timeout.Value);
            }
            else
            {
                await namedPipeClientStream.ConnectAsync();
            }

            namedPipeClientStream.ReadMode = PipeTransmissionMode.Message;
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            await streamWriter.WriteAsync(message);
            await streamWriter.FlushAsync();
            var messageBytes = memoryStream.ToArray();
            await namedPipeClientStream.WriteAsync(messageBytes, 0, messageBytes.Length);
            namedPipeClientStream.Close();
            namedPipeClientStream.Dispose();
        }

        private async Task InitializeServer()
        {
            while (!_isDisposed)
            {
                var receivedMessage = await ReadServerMessageAsync();
                if (!string.IsNullOrWhiteSpace(receivedMessage))
                {
                    ReceivedMessages.Enqueue(receivedMessage);
                }
            }
        }

        private async Task<string> ReadServerMessageAsync()
        {
            using var namedPipeServerStream = new NamedPipeServerStream(
                $"{_thisAppName}ServerPipe",
                PipeDirection.InOut,
                maxNumberOfServerInstances: NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message);
            await namedPipeServerStream.WaitForConnectionAsync();

            var buffer = new byte[1024];
            using (var memoryStream = new MemoryStream())
            {
                do
                {
                    var readBytes = await namedPipeServerStream.ReadAsync(buffer,
                        0,
                        buffer.Length);
                    await memoryStream.WriteAsync(buffer, 0, readBytes);
                }
                while (!namedPipeServerStream.IsMessageComplete);
                namedPipeServerStream.Close();

                memoryStream.Position = 0;
                using var streamReader = new StreamReader(memoryStream);
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
