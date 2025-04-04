namespace Bank
{
    public class KontoLimit
    {
        private Konto konto;
        private decimal limit;
        private bool debetUzyty = false;

        public decimal Bilans => konto.Bilans + (debetUzyty ? 0 : limit);
        public string Nazwa => konto.Nazwa;
        public bool Zablokowane => konto.Zablokowane;

        public decimal Limit
        {
            get => limit;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Limit nie może być ujemny");
                limit = value;
            }
        }

        public KontoLimit(string klient, decimal bilansNaStart = 0, decimal limit = 100)
        {
            this.konto = new Konto(klient, bilansNaStart);
            this.Limit = limit;
        }

        public void Wplata(decimal kwota)
        {
            konto.Wplata(kwota);
            if (konto.Bilans > 0 && debetUzyty)
            {
                debetUzyty = false;
                konto.OdblokujKonto();
            }
        }

        public void Wyplata(decimal kwota)
        {
            if (konto.Zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane");

            if (kwota <= 0)
                throw new ArgumentOutOfRangeException("Kwota wypłaty musi być dodatnia");

            try
            {
                konto.Wyplata(kwota);
            }
            catch (InvalidOperationException)
            {
                if (!debetUzyty && kwota <= konto.Bilans + limit)
                {
                    debetUzyty = true;
                    konto.Wyplata(konto.Bilans);
                    konto.BlokujKonto();
                }
                else throw;
            }
        }

        public void BlokujKonto() => konto.BlokujKonto();
        public void OdblokujKonto() => konto.OdblokujKonto();
    }
}