using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFPANT
{
    public class Folgebrev
    {
        public RettighetsHaver Kreditor { get; set; }

        public List<RettighetsHaver> Debitorer { get; set; }

        public List<MatrikkelEnhet> MatrikkelEnheter { get; set; }

        public Pantedokument PantedokumentDetaljer { get; set; }

        public Overfoersel OverfoerselDetaljer { get; set; }

        public List<String> Forutsetninger {get;set; }

        public string AnneFritekst { get; set; }

        public Adresse ReturneresTil { get; set; }

        public Person Avsender { get; set; }

        public class Overfoersel
        {
            public double Beloep { get; set; }
            public string TilKontonummer { get; set; }
            public string KID { get; set; }
            public string KreditorSaksnummer { get; set; }
            public string MeglerSaksnummer { get; set; }
            public string BeloepGjelder { get; set; }
        }

        public class MatrikkelEnhet
        {
            public string Kommunenummer { get; set; }
            public string Gardsnummer { get; set; }
            public string Bruksnummer { get; set; }
            public string Festenummer { get; set; }
            public string Seksjonsnummer { get; set; }
            public string Organisasjonsnummer { get; set; }
            public string Andelsnummer { get; set; }
        }

        public class RettighetsHaver
        {
            public string Nummer { get; set; }
            public string Navn { get; set; }
        }

        public class Adresse
        {
            public string Navn { get; set; }
            public string Postnummer { get; set; }
            public string Poststed { get; set; }
        }

        public class Pantedokument
        {
            public double Beloep { get; set; }
            public int Prioritet { get; set; }
        }

        public class Person
        {
            public string Navn { get; set; }
            public string Email { get; set; }
            public string TelefonDirekte { get; set; }
            public string Telefon { get; set; }
        }
    }
}
