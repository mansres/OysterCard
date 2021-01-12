using System;
using System.Runtime.Serialization;

namespace OysterCardSystem.Core
{
    public enum Transport
    {
        BUS,
        TUBE
    }

    public static class Station
    {
        public const string HOLBORN = "1";
        public const string EARLS_COURT = "1,2";
        public const string HAMMERSMITH = "2";
        public const string WIMBLEDON = "3";
    }

    public class Zone
    {
        private readonly string _zone;

        public Zone(string zone)
        {
            _zone = zone;
        }

        public string GetZone()
        {
            return _zone;
        }
    }

    public class FareException : Exception
    {
        public FareException()
        {
        }

        public FareException(string message) : base(message)
        {
        }

        public FareException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
