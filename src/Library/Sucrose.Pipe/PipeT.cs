using SPEMREA = Sucrose.Pipe.Event.MessageReceivedEventArgs;
using SPHPC = Sucrose.Pipe.Helper.PipeClient;
using SPHPS = Sucrose.Pipe.Helper.PipeServer;

namespace Sucrose.Pipe
{
    public class PipeT(string PipeName)
    {
        private bool ClientStarted;
        private bool ServerStarted;

        private bool ClientStopped;
        private bool ServerStopped;

        private readonly SPHPC PC = new();
        private readonly SPHPS PS = new();

        public event EventHandler<SPEMREA> MessageReceived;

        public async Task StartClient()
        {
            if (!ClientStarted)
            {
                await PC.Start(PipeName);
                ClientStarted = true;
            }

            if (!ClientStopped && !PC.IsConnected)
            {
                try
                {
                    await PC.Stop();
                }
                catch { }

                await PC.Start(PipeName);
            }
        }

        public async Task StartClient(string message)
        {
            if (!ClientStarted)
            {
                await PC.Start(PipeName);
                ClientStarted = true;
            }

            if (!ClientStopped)
            {
                if (!PC.IsConnected)
                {
                    try
                    {
                        await PC.Stop();
                    }
                    catch { }
                    await PC.Start(PipeName);
                }

                await PC.SendMessage(message);
            }
        }

        public async Task StartServer()
        {
            if (!ServerStarted || ServerStopped)
            {
                await PS.Start(PipeName, MessageReceived);

                ServerStopped = false;
                ServerStarted = true;
            }
        }

        public async Task StopClient()
        {
            await PC.Stop();

            ClientStopped = true;
            ClientStarted = false;
        }

        public async Task StopServer()
        {
            await PS.Stop();

            ServerStopped = true;
            ServerStarted = false;
        }

        public async Task DisposeClient()
        {
            ClientStarted = false;
            ClientStopped = true;

            await PC.Stop();

            PC.Dispose();
        }

        public async Task DisposeServer()
        {
            ServerStarted = false;
            ServerStopped = true;

            await PS.Stop();

            PS.Dispose();
        }

        protected virtual void OnMessageReceived(SPEMREA e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}