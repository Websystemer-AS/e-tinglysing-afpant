# e-tinglysing-afpant
AFPANT - Altinn Formidlingstjenester for kjøpers e-signerte pantedokument

## Spesifikasjon for overføring av kjøpers e-signerte pantedokument

### Overordnede mål
Muliggjøre en sikker og effektiv maskinell overføring av kjøpers pantedokument-SDO fra bank-fagsystem til megler-fagsystem.
Overføringen vil benytte Altinn Formidlingstjenester etter samme modell som Kartverket selv benytter for e-tinglysing (men kjøre som en separat tjeneste/serviceCode).
Kartverket er tjenesteeier av denne formidlingstjenesten.

#### Forutsetninger
Det forutsettes en viss grad av erfaring med integrasjon mot Altinn Formidlingstjenester. AFPANT er forsøkt utformet slik at den blir mest mulig lik Kartverket's egen bruk av Altinn Formidlingstjenester.
Kartverket har også en eksempelklient: https://github.com/kartverket/eksempelklient-etinglysing-altinn

#### Hvem bruker løsningen?
AFPANT kan benyttes av systemleverandører som skal sende og/eller motta e-signerte pantedokumenter som skal brukes i en e-tinglysingsmelding. 

Leverandør | På vegne av
---------- | -----------
Ambita AS | Bank/Megler
Websystemer AS | Megler
Nordea Bank AB (publ), filial i Norge | Bank

#### AFPANT-løsningen muliggjør følgende:
* Overføring av 1 e-signert pantedokument (SDO) fra kjøpers bank til eiendomsmegler/oppgjørsforetak 
* Overføring av følgebrev som PDF eller XML 
* ACK/NACK-kvittering fra mottakersystem til avsendersystem med informasjon om forsendelsen kunne rutes korrekt

For å kunne gjennomføre overføring av pantedokumentet er også bankens følgebrev inntatt i denne spesifikasjonen.

Følgebrevet fra bank inneholder normalt sett viktige detaljer om overførselen:
* Informasjon om innbetaler, beløp, betalt til kontonummer, KID 
* Forutsetningene pantedokumentet overføres under 
* Krav til oppnådd prioritet
* Gyldig disposisjon av innbetalingen
* Retur av bekreftet grunnboksutskrift som bekrefter oppnådd prioritet


