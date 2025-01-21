using System.IO.Pipes;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;

namespace Sucrose.Pipe.Helper
{
    internal class PipeClient : IDisposable
    {
        private NamedPipeClientStream _pipeClient;
        private StreamWriter _writer;

        public bool IsConnected => _pipeClient?.IsConnected ?? false;

        public void Start(string pipeName)
        {
            _pipeClient = new(SMMRG.PipeServerName, pipeName, PipeDirection.Out, PipeOptions.Asynchronous);

            _pipeClient.Connect();

            _writer = new(_pipeClient)
            {
                AutoFlush = true
            };
        }

        public void Stop()
        {
            if (_pipeClient != null && IsConnected)
            {
                _pipeClient.Close();
            }
        }

        public void SendMessage(string message)
        {
            if (_pipeClient == null)
            {
                return;
            }

            if (!IsConnected)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                _writer.WriteLine(message);
            }
        }

        public void Dispose()
        {
            _pipeClient?.Dispose();
            _writer?.Dispose();
        }
    }
}