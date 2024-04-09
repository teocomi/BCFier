using IPA.Bcfier.Ipc;
using IPA.Bcfier.Models.Bcf;
using Newtonsoft.Json;

namespace IPA.Bcfier.Revit
{
    public class IpcBcfierCommandListener
    {
        private readonly IpcHandler _ipcHandler;
        private readonly RevitTaskQueueHandler _revitTaskQueueHandler;
        private bool _isRunning = true;

        public IpcBcfierCommandListener(IpcHandler ipcHandler,
            RevitTaskQueueHandler revitTaskQueueHandler)
        {
            _ipcHandler = ipcHandler;
            _revitTaskQueueHandler = revitTaskQueueHandler;
        }

        public void Listen()
        {
            Task.Run(async () =>
            {
                while (_isRunning)
                {
                    if (IpcHandler.ReceivedMessages.TryDequeue(out var message))
                    {
                        var ipcMessage = JsonConvert.DeserializeObject<IpcMessage>(message)!;
                        switch (ipcMessage.Command)
                        {
                            case IpcMessageCommand.AppClosed:
                                _isRunning = false;
                                break;

                            case IpcMessageCommand.CreateViewpoint:
                                _revitTaskQueueHandler.CreateRevitViewpointCallbacks.Enqueue(async (data) =>
                                {
                                    await _ipcHandler.SendMessageAsync(JsonConvert.SerializeObject(new IpcMessage
                                    {
                                        CorrelationId = ipcMessage.CorrelationId,
                                        Command = IpcMessageCommand.ViewpointCreated,
                                        Data = data
                                    }));
                                });
                                break;

                            case IpcMessageCommand.ShowViewpoint:
                                _revitTaskQueueHandler.ShowViewpointQueueItems.Enqueue(new Models.ShowViewpointQueueItem
                                {
                                    Callback = async () =>
                                    {
                                        await _ipcHandler.SendMessageAsync(JsonConvert.SerializeObject(new IpcMessage
                                        {
                                            CorrelationId = ipcMessage.CorrelationId,
                                            Command = IpcMessageCommand.ViewpointShown
                                        }));
                                    },
                                    Viewpoint = JsonConvert.DeserializeObject<BcfViewpoint>(ipcMessage.Data!)
                                });
                                break;

                            default:
                                // TODO
                                throw new NotImplementedException();
                        }
                    }

                    await Task.Delay(100);
                }

                _revitTaskQueueHandler.UnregisterEventHandler();
                _ipcHandler.Dispose();
            });
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}
