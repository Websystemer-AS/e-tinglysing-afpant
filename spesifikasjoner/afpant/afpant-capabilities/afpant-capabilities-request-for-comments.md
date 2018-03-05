# e-tinglysing-afpant-capabilities: REQUEST FOR COMMENTS
## Sammendrag
Aktører som skal benytte AFPANT har behov for å:
* Vite om en annen aktør (mottaker-organisasjonsnummer) finnes på AFPANT
* Vite hvilke meldingstyper en annen aktør støtter

Meldingstype for CapabilityQuery vil også kunne fungere som en echo/ping-pong test for å sjekke at et gitt organisasjonsnummer er konfigurert korrekt på Altinn Formidlingstjenester (Kartverket har etablert tilgang for organisasjonsnummeret, organisasjonsnummeret har delegert tilgangsrettighet til systemleverandør).

## Alternative løsninger
Difi/Brreg arbeider med en offentlig-privat løsning som kunne vært aktuell å benytte til å løse ovenstående behov (arbeidstittel KoFuVi / digital samhandling offentlig-privat DSOP) - men dette prosjektet er ikke ferdigstilt enda (tentativ POC ultimo 2018 for de nåværende involverte offentlig/private aktører).
Det er indikert at løsningen også vil kunne benyttes mellom private næringsaktører og capability / servicediscovery er en sentral komponent i den løsningen.

## Forslag
Alle deltakende aktører på AFPANT må kunne besvare maskinelt om hvilke meldingstyper (inn- og utgående) som støttes.
Alle deltakende aktører på AFPANT må dermed som absolutt minimum implementere CapabilityQuery og CapabilityResponse i tillegg til en eller flere andre meldingstyper.

## Ny request/response meldingstype: CapabilityQuery + CapabilityResponse
## CapabilityQuery objekt
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
			<td><p>Denne kan være en av følgende:</p><ul><li>CapabilityQuery</li></ul></td>
		</tr>
		<tr>
			<td><p>messageGenerated</p></td>
			<td><p>DateTime</p></td>
			<td><p>Timestamp meldingen ble produsert hos avsender</p></td>
		</tr>        
		<tr>
			<td><p>generatedBy</p></td>
			<td><p>String</p></td>
			<td><p>Optional: Identifikasjon på hvilken integrasjonspartner som forespør på vegne av organisasjonsnummeret.
            <br>Eksempel: Websystemer AS</p></td>
		</tr>
		<tr>
			<td><p>externalSystemId</p></td>
			<td><p>String</p></td>
			<td><p>Optional: ID/oppdragsnummer/key i eksternt meglersystem/banksystem hvor forespørselen sendes fra.</p></td>
		</tr>
	</tbody>
</table>

## CapabilityResponse objekt
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
			<td><p>Denne kan være en av følgende:</p><ul><li>CapabilityResponse</li></ul></td>
		</tr>
		<tr>
			<td><p>messageGenerated</p></td>
			<td><p>DateTime</p></td>
			<td><p>Timestamp meldingen ble produsert hos avsender</p></td>
		</tr>           
		<tr>
			<td><p>supportedInboundMessageTypes</p></td>
			<td><p>String[]</p></td>
			<td><p>Array av strings som angir hvilke meldingstyper (objektnavn) som dette organisasjonsnummeret kan motta.
            <br>Eksempel: "CapabilityQuery" "SignedMortageDeed"</p></td>
		</tr>       
		<tr>
			<td><p>supportedOutboundMessageTypes</p></td>
			<td><p>String[]</p></td>
			<td><p>Array av strings som angir hvilke meldingstyper (objektnavn) som dette organisasjonsnummeret kan sende.
            <br>Eksempel: "CapabilityResponse" "SignedMortgageDeedProcessed"</p></td>
		</tr>
		<tr>
			<td><p>generatedBy</p></td>
			<td><p>String</p></td>
			<td><p>Optional: Identifikasjon på hvilken integrasjonspartner som besvarer på vegne av organisasjonsnummeret.
            <br>Eksempel: Ambita AS</p></td>
		</tr>        	
		<tr>
			<td><p>externalSystemId</p></td>
			<td><p>String</p></td>
			<td><p>Optional: ID/oppdragsnummer/key i eksternt meglersystem/banksystem hvor forespørselen sendes fra.</p></td>
		</tr>
	</tbody>
</table>