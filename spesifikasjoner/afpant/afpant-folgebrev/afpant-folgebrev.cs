using System.Collections.Generic;

namespace AFPANT
{
    public class Folgebrev
    {
        public RettighetsHaver Kreditor { get; set; }

        public List<RettighetsHaver> Debitorer { get; set; }

        public List<MatrikkelEnhet> MatrikkelEnheter { get; set; }

        public Pantedokument PantedokumentDetaljer { get; set; }

        public Overfoersel OverfoerselDetaljer { get; set; }

        public List<string> Forutsetninger { get; set; }

        public string AnnenFritekst { get; set; }

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
			public string BorettslagNavn { get; set; }
        }

        public class RettighetsHaver
        {
            public string Nummer { get; set; }
            public string Navn { get; set; }
        }

        public class Adresse
        {
            public string Navn { get; set; }
			public string Adresse { get; set; }
            public string Postnummer { get; set; }
            public string Poststed { get; set; }
        }

        public class Pantedokument
        {
            public double Beloep { get; set; }
            public int Prioritet { get; set; }
            public string PrioritetsBeskrivelse { get; set; } // tekstlig representasjon av forventet prioritet (1. pri, likestilt med, viker for kreditor xxx)
			//todo: strukturert informasjon om hvem man viker for eller er likestilt med
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
