using System;

namespace Bank
{
    public class Konto
    {
        private string klient;
        private decimal bilans;
        private bool zablokowane = false;

        public string Nazwa => klient;
        public decimal Bilans => bilans;
        public bool Zablokowane => zablokowane;

        public Konto(string klient, decimal bilansNaStart = 0)
        {
            if (string.IsNullOrWhiteSpace(klient))
                throw new ArgumentException("Nazwa klienta nie może być pusta");

            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public void Wplata(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane");
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException("Kwota wpłaty musi być dodatnia");

            bilans += kwota;
        }

        public void Wyplata(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane");
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException("Kwota wypłaty musi być dodatnia");
            if (kwota > bilans)
                throw new InvalidOperationException("Niewystarczające środki");

            bilans -= kwota;
        }

        public void BlokujKonto() => zablokowane = true;
        public void OdblokujKonto() => zablokowane = false;
    }
}