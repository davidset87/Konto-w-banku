namespace Bank
{
    public class KontoPlus : Konto
    {
        private decimal limitDebetowy;
        private bool debetUzyty = false;

        public decimal Limit
        {
            get => limitDebetowy;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Limit nie może być ujemny");
                limitDebetowy = value;
            }
        }

        public new decimal Bilans => base.Bilans + (debetUzyty ? 0 : Limit);

        public KontoPlus(string klient, decimal bilansNaStart = 0, decimal limit = 100)
            : base(klient, bilansNaStart)
        {
            this.Limit = limit;
        }

        public new void Wyplata(decimal kwota)
        {
            if (Zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane");

            if (kwota <= 0)
                throw new ArgumentOutOfRangeException("Kwota wypłaty musi być dodatnia");

            if (kwota <= base.Bilans)
            {
                base.Wyplata(kwota);
                return;
            }

            if (!debetUzyty && kwota <= base.Bilans + Limit)
            {
                debetUzyty = true;
                base.Wyplata(base.Bilans); // Wybierz wszystko
                BlokujKonto();
                return;
            }

            throw new InvalidOperationException("Niewystarczające środki");
        }

        public new void Wplata(decimal kwota)
        {
            base.Wplata(kwota);
            if (base.Bilans > 0 && debetUzyty)
            {
                debetUzyty = false;
                OdblokujKonto();
            }
        }
    }
}