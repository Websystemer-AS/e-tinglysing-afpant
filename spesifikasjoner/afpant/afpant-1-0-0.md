# e-tinglysing-afpant-1.0.0
## Altinn Formidlingstjeneste - servicedetaljer
<table>
	<tbody>
		<tr>
			<td><p><strong>Navn</strong></p></td>
			<td><p><strong>ServiceCode</strong></p></td>
			<td><p><strong>ServiceEditionCode</strong></p></td>
		</tr>
		<tr>
			<td><p>AFPANT (Altinn Test TT02)</p></td>
			<td><p>4752</p></td>
			<td><p>1</p></td>
		</tr>
		<tr>
			<td><p>AFPANT (Altinn Prod)</p></td>
			<td><p>4752</p></td>
			<td><p>1</p></td>
		</tr>
	</tbody>
</table>

## AFPANT-tilgang for systemleverandører/datasentraler
Kartverket må gi rettigheter (READ+WRITE) i tjenesteeierstyrt rettighetsregister for alle systemleverandører/datasentraler som skal koble seg direkte til denne tjenesten.
Bestillinger av denne tilgangen må gjøres via Kartverket JIRA (http://jira.statkart.no:8080/).

## Delegering av roller fra egne kunder til systemleverandør/datasentral
Systemleverandører/datasentraler som skal utføre sending/mottak på vegne av *andre organisasjoner* (eks meglerforetak/bank) må registrere seg selv hos Kartverket (ref ovenstående punkt), og skal bruke sitt *eget* organisasjonsnummer som "reportee" mot Altinn. Systemleverandører/datasentraler som opererer på vegne av andre må også hente meldinger for sitt *eget organisasjonsnummer* (for det er dit ACK/NACK meldinger fra mottakersystem sendes). 

*Hver organisasjon/kunde* som en systemleverandør/datasentral opererer på vegne av (eks meglerforetak/bank) må logge på Altinn for å delegere rettigheter til sin gjeldende systemleverandør/datasentral sitt organisasjonsnummer for tjenesten *4752* (AFPANT).
![Oppskrift for å delegere rollen 'Utfyller/Innsender' eller enkeltrettighet til systemleverandør/datasentral sitt organisasjonsnummer finnes her](https://www.altinn.no/no/Portalhjelp/Administrere-rettigheter-og-prosessteg/Gi-roller-og-rettigheter/)

 
## Sammendrag
Bruker i avsender-bank må innhente hvilket organisasjonsnummer forsendelsen skal til (dette hentes normalt sett ut fra signert kopi av kjøpekontrakt, og er enten organisasjonsnummeret til eiendomsmeglerforetaket eller oppgjørsforetaket).

Deretter produseres det et **ZIP**-arkiv som inneholder følgende filer:
* Kjøpers pantedokument SDO (kun 1 pantedokument pr forsendelse)
* Eventuelt følgebrev (PDF/XML) (med forutsetninger for oversendelse av pantedokument, evt innbetalingsinformasjon)
* Dersom følgebrev produseres som XML må dokumentet validere i henhold til <a href="https://github.com/Websystemer-AS/e-tinglysing-afpant/blob/master/spesifikasjoner/afpant/afpant-folgebrev/afpant-folgebrev-1-0-0.md">afpant-folgebrev spesifikasjon</a>.

**NB**: Dersom mer enn 1 pantedokument fra samme lånesak skal tinglyses på samme matrikkelenhet må dette sendes som to separate forsendelser. For eksempel i tilfeller hvor det er to debitorer (låntakere) som ikke er ektefeller/samboere/registrerte partnere som skal ha likestilt prioritet, men separate pantedokumenter.

Avsender-bank angir metadata-keys på Altinn-forsendelse (i manifestet) som indikerer om avsender-bank ønsker avlesingskvittering (maskinell og/eller pr email), og hvorvidt følgebrevet er inkludert i ZIP eller om det sendes out-of-band (f.eks via fax eller mail direkte til megler/oppgjør).
Mottaker (systemleverandør) pakker ut ZIP og parser SDO for å trekke ut nøkkeldata (kreditor, debitor(er), matrikkelenhet(er)) som brukes for å rute forsendelsen til korrekt oppdrag hos korrekt megler/oppgjørsforetak.

## Validering og ruting hos mottakende system
Hver enkelt systemleverandør som skal behandle forsendelser via AFPANT vil forsøke rute forsendelsen til korrekt meglersak/oppdrag i sine egne kundedatabaser.
For å rute forsendelsen blir pantedokumentet pakket ut fra SDO, og matrikkelenheter/debitorer ekstraheres.

### Krav til filnavn i ZIP-arkiv
- Eventuelt følgebrev må følge konvensjonen: "coverletter_&ast;.[pdf|xml]"
- Pantedokumentet må følge konvensjonen: "signedmortgagedeed_&ast;.sdo"
Wildcard "&ast;" kan erstattes med en vilkårlig streng (må være et gyldig filnavn), f.eks lånesaksnummer eller annen relevant referanse for avsender.

### Implementasjonsbeskrivelse: ruting
- mottakende systemleverandør søker blant alle sine kunders matrikkelenhet(er)
- utvalget avgrenses til matrikkelenheter som tilhører meglersaker hvor organisasjonsnummeret til _enten_ meglerforetaket eller oppgjørsforetaket på meglersaken er lik organisasjonsnummeret pantedokumentet er sendt til
- utvalget avgrenses til meglersaker hvor **alle debitorene i pantedokumentet også er registrert som kjøpere på meglersaken** (hvis det mangler fødselsnummer/orgnummer på kjøper(e) kan leverandør selv velge graden av fuzzy matching som skal tillates) 
- dersom det er registrert flere kjøpere på meglersaken enn det finnes debitorer/signaturer i pantedokumentet skal mottakende system avvise forsendelsen med en SignedMortgageDeedProcessedMessage (NACK) hvor status = DebitorMismatch.

### Håndtering av feil
- Den første feilen som oppstår stopper videre behandling av forsendelsen.
- SignedMortgageDeedProcessedMessage (NACK) returneres og vil ha utfyllende beskrivelse i property statusDescription.

## Avlesningskvittering
Avsender-bank kan angi hvorvidt mottakende fagsystem skal returnere en avlesningskvittering, og man kan velge følgende metoder:
* Avsender-bank angir i Altinn-metadata keys (senderName/senderEmail/senderPhone) kontaktinformasjonen til kontaktperson i bank og key (notificationMode) om de ønsker emailvarsling fra mottakende fagsystem ved suksessfull ruting og/eller feil.
* Avsender-bank angir i Altinn-metadata key (notificationMode) enum verdi «AltinnNotification». Dette betyr at avsender-bank ønsker en strukturert ack/nack-melding fra mottakende fagsystem ved behandling. Ack/nack-meldingen kan da brukes av avsender-bank til å oppdatere state/workflow i eget (bank)fagsystem.
* Avsender-bank angir i Altinn-metadata key (coverLetter) enum verdi som tilsier hvorvidt følgebrevet ligger som PDF/XML inne i ZIP eller om det sendes til megler/oppgjør på annet vis. Eventuell PDF/XML er ment til manuell behandling av oppgjørsansvarlig på lik linje med dagens papirbaserte følgebrev. 

## Altinn Formidlingstjenester manifest metadata-keys ved innsending fra banksystem til meglersystem
<table>
	<tbody>
		<tr>
			<td><p><strong>Key</strong></p></td>
			<td><p><strong>Type</strong></p></td>
			<td><p><strong>Required</strong></p></td>
			<td><p><strong>Beskrivelse</strong></p></td>
		</tr>
		<tr>
			<td><p>messageType</p></td>
			<td><p>String</p></td>
			<td><p>Yes</p></td>
			<td><p>Denne kan være en av følgende:</p><ul><li>SignedMortgageDeed</li></ul></td>
		</tr>
		<tr>
			<td><p>notificationMode</p></td>
			<td><p>String[] (enum[])</p></td>
			<td><p>No</p></td>
			<td><p>Kommaseparert liste over alle notifications avsender ønsker. Følgende strenger kan være verdi i array:</p><ul><li>EmailNotificationWhenRoutedSuccessfully</li><li>EmailNotificationWhenFailed</li><li>AltinnNotification</li></ul><p>Hvis du f.eks. har «EmailNotificationWhenFailed» og «AltinnNotification» skal mottaker sende epost hvis behandling av pantedokumentet feiler og uansett sende en ack/nack gjennom Altinn.</p></td>
		</tr>
		<tr>
			<td><p>senderName</p></td>
			<td><p>String</p></td>
			<td><p>No</p></td>
			<td><p>Navn på avsender (mennesket)</p></td>
		</tr>
		<tr>
			<td><p>senderEmail</p></td>
			<td><p>String</p></td>
			<td><p>No</p></td>
			<td><p>Required hvis notificationMode EmailNotificationWhenRoutedSuccessfully eller EmailNotificationWhenFailed er angitt.<br> Email til avsender</p></td>
		</tr>
		<tr>
			<td><p>senderPhone</p></td>
			<td><p>String</p></td>
			<td><p>No</p></td>
			<td><p>Tlf til avsender</p></td>
		</tr>
		<tr>
			<td><p>coverLetter</p></td>
			<td><p>String (enum)</p></td>
			<td><p>Yes</p></td>
			<td><p>Denne kan være en av følgende statuser:</p><ul><li>FileAttached</li><li>SentOutOfBand</li><li>Omitted</li></ul></td>
		</tr>
		<tr>
			<td><p>payload</p></td>
			<td><p>String</p></td>
			<td><p>Yes</p></td>
			<td><p>Base64-encodet streng av ZIP-arkivet.</p></td>
		</tr>
	</tbody>
</table>

## Manifest metadata-keys ved retur av ACK/NACK notification fra fagsystem til bank (etter behandling av mottatt pantedokument):
<table>
	<tbody>
		<tr>
			<td><p><strong>Key</strong></p></td>
			<td><p><strong>Type</strong></p></td>
			<td><p><strong>Beskrivelse</strong></p></td>
		</tr>
		<tr>
			<td><p>messageType</p></td>
			<td><p>String</p></td>
			<td><p>Denne kan være en av følgende:</p><ul><li>SignedMortgageDeedProcessed</li></ul></td>
		</tr>
		<tr>
			<td><p>payload</p></td>
			<td><p>String</p></td>
			<td><p>Base64-encodet streng av SignedMortgageDeedProcessedMessage-objektet (serialisert som XML).</p></td>
		</tr>
	</tbody>
</table>

## SignedMortgageDeedProcessedMessage objekt
<table>
	<tbody>
		<tr>
			<td><p><strong>Key</strong></p></td>
			<td><p><strong>Type</strong></p></td>
			<td><p><strong>Beskrivelse</strong></p></td>
		</tr>
		<tr>
			<td><p>messageType</p></td>
			<td><p>String</p></td>
			<td><p>Denne kan være en av følgende:</p><ul><li>SignedMortgageDeedProcessed</li></ul></td>
		</tr>
		<tr>
			<td><p>status</p></td>
			<td><p>String (enum)</p></td>
			<td><p>Denne kan være en av følgende statuser:</p><ul><li>RoutedSuccessfully</li><li>UnknownCadastre (ukjent matrikkelenhet)</li><li>DebitorMismatch (fant matrikkelenhet, men antall kjøpere eller navn/id på kjøpere matcher ikke debitorer i pantedokumentet)</li><li>Rejected (sendt til et organisasjonsnummer som ikke lenger har et aktivt kundeforhold hos leverandøren - feil config i Altinn AFPANT, eller ugyldig forsendelse)</li></ul></td>
		</tr>
		<tr>
			<td><p>statusDescription</p></td>
			<td><p>String</p></td>
			<td><p>Inneholder en utfyllende human-readable beskrivelse om hvorfor en forsendelse ble NACK'et.</td>
		</tr>
		<tr>
			<td><p>externalSystemId</p></td>
			<td><p>String</p></td>
			<td><p>Optional: ID/oppdragsnummer/key i eksternt meglersystem/fagsystem.</p></td>
		</tr>
	</tbody>
</table>