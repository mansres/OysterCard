using System;

namespace OysterCardSystem.Core
{
    public class Journey
    {
        private Zone _startPoint;
        private Zone _endPoint;
        private Transport _transport;
        private SmartCard _card;
        private JourneyFare _fare;

        public Journey(JourneyFare fare)
        {
            _fare = fare;
        }

        public void SetStartPoint(Transport transport, Zone startPoint, SmartCard card)
        {
            try
            {
                _fare.Validate(transport, card);
                _fare.ChargeMax(transport, card);

            }
            catch (FareException ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }

            _transport = transport;
            _card = card;
            _startPoint = startPoint;
        }

        public void SetEndPoint(Zone endPoint)
        {
            _endPoint = endPoint;
            _fare.Charge(_transport, this, _card);
        }

        public Zone GetStartPoint()
        {
            return _startPoint;
        }
        public Zone GetEndPoint()
        {
            return _endPoint;
        }

    }
}
