namespace OysterCardSystem.Core
{
    public class SmartCard
    {
        private float _balance;

        public SmartCard(float balance)
        {
            _balance = balance;
        }

        public SmartCard()
        {
            _balance = 0;
        }

        public float GetBalance()
        {
            return _balance;
        }

        public void SetBalance(float balance)
        {
            _balance = balance;
        }

        public void AddMoney(float money)
        {
            _balance = _balance + money;
        }

        public void Out(float fare)
        {
            Validate(fare);
            _balance = _balance - fare;
        }

        public void Validate(float fare)
        {
            if (_balance < fare)
                throw new FareException("You don't have enough balance!");
        }

        public void In(float f) //Balance Credit after the journey end.
        {
            _balance = _balance + f;
        }
    }
}
