using System;

namespace Assets.Scripts._Project.Code.Slasher.Game.Services
{
    public interface IEventBusConsumer : IDisposable
    {
        void Subscribe();
        void Unsubscribe();
    }
}
