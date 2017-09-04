using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Folgebrev;

namespace afpant_folgebrev_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Example1();
        }

        static void Example1()
        {

            var folgebrev = new Folgebrev();

            folgebrev.Mottaker = new JuridiskPerson()
            {
                Navn = "Oppgjørsforetaket AS",
                Organisasjonsnummer = "800000000"
            };

            folgebrev.Kreditor = new RettighetsHaver()
            {
                Nummer = "983258344",
                Navn = "Nordea Bank AB (publ), filial i Norge"
            };

            folgebrev.Debitorer = new List<RettighetsHaver>()
            {
                new RettighetsHaver()
                {
                     Navn="Arne Arnesen",
                      Nummer="01018011223"
                },
                new RettighetsHaver()
                {
                    Navn="Anne Arnesen",
                    Nummer="01017922113"
                }
            }.ToArray();

            folgebrev.Registerenheter = new List<Registerenhet>()
            {
                new Registerenhet()
                {
                     KommuneNavn="Bergen",
                     Kommunenummer="1201",
                     Gardsnummer="10",
                     Bruksnummer="818",
                     Festenummer="0",
                     Seksjonsnummer="41"
                }
            }.ToArray();

            folgebrev.PantedokumentDetaljer = new Pantedokument()
            {
                Beloep = 2500000,
                ForstePrioritet = true
            };

            folgebrev.OverfoerselDetaljer = new Overfoersel()
            {
                Beloep = 2100000,
                BeloepOverfortDato = DateTime.Now.AddDays(-2).Date,
                TotalBeloep = 2500000,
                BeloepGjelder = "dekning av kjøpesum for ovennevnte eiendom",
                TilKontonummer = "65501100000",
                KID = "10001000222111",
                KreditorSaksnummer = "Lånesak 124-221/22",
                MeglerSaksnummer = "117-00-1002 Lånegaten 15 B",
                ProdusertDato = DateTime.Now
            };

            folgebrev.Forutsetninger = new List<string>()
            {
                @"Beløpet er overført under forutsetning av at vedlagte pantedokument/er blir besørget tinglyst av Dem og returnert vår depotavdeling med oppgitt prioritet i eiendommen.
Hjemmel til eiendommen forutsettes tinglyst på Arne Arnesen og Anne Arnesen.	
Det forutsettes videre at:
- forutsetning A
- og forutsetning B
Eller at forutsetning C, og så videre."
            }.ToArray();

            folgebrev.AnnenFritekst = "Dersom vi ikke har mottatt ovennevnte dokumenter innen 6 måneder fra dette brevs dato, ber vi Dem, av hensyn til bankens kontrollorganer, i god tid meddele når vi kan forvente å motta dokumentene i retur.";

            folgebrev.ReturneresTil = new Adresse()
            {
                Navn = "Nordea Bank AB (publ), filial i Norge, v/ Depot Ålesund",
                Postadresse = "PB 6001",
                Postnummer = "6012",
                Poststed = "Ålesund"
            };

            folgebrev.Avsender = new Person()
            {
                Navn = "Bjarne Bankrådgiver",
                Email = "bjarne.bank@nordea-afpant.com",
                Telefon = "23206001",
                TelefonDirekte = "80000000"
            };


            //serialize to memory, append XSD + XSLT instructions, save to file
            using (var ms = new MemoryStream())
            {
                var ser = new XmlSerializer(typeof(Folgebrev));
                ser.Serialize(ms, folgebrev);
                ms.Seek(0, SeekOrigin.Begin);

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);

                // append XSD reference
                var xsdAttribute = xmlDoc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                xsdAttribute.Value = "https://last-opp.pantedokumentet.no/AFPANT/afpant-folgebrev-1.0.0.xsd";
                xmlDoc.DocumentElement.Attributes.Append(xsdAttribute);

                // append XSLT reference
                var xsltUri = "https://last-opp.pantedokumentet.no/AFPANT/afpant-folgebrev-1.0.xslt"; // note - versioned to major.minor (enables releasing revisions without needting to modify existing xml documents)
                var xsltProcessingInstruction = xmlDoc.CreateProcessingInstruction("xml-stylesheet", $"type=\"text/xsl\" href=\"{xsltUri}\"");
                xmlDoc.InsertAfter(xsltProcessingInstruction, xmlDoc.FirstChild);

                // persist to disk
                var xmlExamplePath = $"{Environment.CurrentDirectory}\\afpant-folgebrev-example1.xml";
                if (File.Exists(xmlExamplePath))
                {
                    File.Delete(xmlExamplePath);
                }
                // implicitly encoded as utf-8 without signature/BOM and XmlDeclaration 
                xmlDoc.Save(xmlExamplePath);
            }

        }
    }
}
