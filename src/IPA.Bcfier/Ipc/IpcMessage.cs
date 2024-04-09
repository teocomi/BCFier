using System;

namespace IPA.Bcfier.Ipc
{
    public class IpcMessage
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();

        public IpcMessageCommand Command { get; set; }

        public string? Data { get; set; }
    }
}
