using System;

namespace Application.ReadModels
{
    public class UniverseCurrentTimeReadModel
    {
        public UniverseCurrentTimeReadModel(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}