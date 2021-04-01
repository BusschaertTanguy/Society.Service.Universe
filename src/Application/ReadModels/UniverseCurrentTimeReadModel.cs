using System;

namespace Application.ReadModels
{
    public class UniverseCurrentTimeReadModel
    {
        public UniverseCurrentTimeReadModel(DateTime currentTime)
        {
            CurrentTime = currentTime;
        }

        public DateTime CurrentTime { get; }
    }
}